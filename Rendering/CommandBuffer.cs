using ConsoleGameRenderer.Component;
using ConsoleGameRenderer.Rendering.Shading;
using System.Drawing;
using System.Numerics;

namespace ConsoleGameRenderer.Rendering
{
    internal class CommandBuffer
    {
        private readonly Queue<Action> _commandQueue;

        public CommandBuffer()
        {
            _commandQueue = new Queue<Action>();
        }

        public CommandBuffer(Queue<Action> commandQueue)
        {
            _commandQueue = new Queue<Action>(commandQueue);
        }

        public void Copy<T>(Texture<T> source, Texture<T> destination, Vector2 sourcePosition, Vector2 sourceSize, Vector2 destinationPosition)
            where T : struct
        {
            int startX = (int)sourcePosition.X;
            int startY = (int)sourcePosition.Y;

            int endX = (int)(sourcePosition.X + sourceSize.X);
            int endY = (int)(sourcePosition.Y + sourceSize.Y);

            int destX = (int)destinationPosition.X;
            int destY = (int)destinationPosition.Y;

            _commandQueue.Enqueue(() =>
            {
                destination.Deconstruct(out T[,] destinationElements, out _);
                lock (destinationElements)
                {
                    Parallel.For(startX, endX, x =>
                    {
                        Parallel.For(startY, endY, y =>
                        {
                            destinationElements[x - startX + destX, y - startY + destY] = source.GetTextureElement(x, y);
                        });
                    });
                }
            });
        }

        public void Copy<T>(Texture<T> source, Texture<T> destination)
            where T : struct => Copy(source,
                                destination,
                                Vector2.Zero,
                                new Vector2(source.Width, source.Height),
                                Vector2.Zero);

        public void Copy<T>(Texture<T> source, Texture<T> destination, Vector2 sourcePosition, Vector2 sourceSize)
            where T : struct => Copy(source,
                                     destination,
                                     sourcePosition,
                                     sourceSize,
                                     Vector2.Zero);

        public void Copy<T>(T source, Texture<T> destination)
            where T : struct
        {
            int width = destination.Width;
            int height = destination.Height;

            _commandQueue.Enqueue(() =>
            {
                destination.Deconstruct(out T[,] destinationElements, out _);
                lock (destinationElements)
                {
                    Parallel.For(0, width, x =>
                    {
                        Parallel.For(0, height, y =>
                        {
                            destinationElements[x, y] = source;
                        });
                    });
                }
            });
        }

        public void Blit<T>(Texture<T> source, Texture<T> destination, IMaterial<Vector2, T> material)
            where T : struct
        { 
            int width = source.Width;
            int height = source.Height;

            _commandQueue.Enqueue(() =>
            {
                //material.PassUniforms(source);
                destination.Deconstruct(out T[,] destinationElements, out _);
                lock (destinationElements)
                {
                    Parallel.For(0u, width, x =>
                    {
                        Parallel.For(0u, height, y =>
                        {
                            destinationElements[x, y] = material.ApplyShader(new Vector2(x, y) / new Vector2(width, height));
                        });
                    });
                }
            });
        }

        public void DrawElement<T>(T element, Texture<T> destination, Vector2 elementPosition)
            where T : struct
        {
            destination.Deconstruct(out T[,] destinationElements, out _);
            _commandQueue.Enqueue(() =>
            {
                destinationElements[(int)elementPosition.X, (int)elementPosition.Y] = element;
            });
        }

        public void DrawLine<T>(VertexData start, VertexData end, Texture<T> destination, IMaterial<VertexData, T> material)
            where T : struct
        {
            Vector2 startVector = start.Position.ToVector2();
            Vector2 endVector = end.Position.ToVector2();

            float distance = Vector2.Distance(startVector, endVector);
            int steps = (int)distance;
            Vector2 direction = Vector2.Normalize(endVector - startVector);

            _commandQueue.Enqueue(() =>
            {
                destination.Deconstruct(out T[,] destinationElements, out _);
                lock (destinationElements)
                {
                    Parallel.For(0, steps, i =>
                    {
                        int positionX = (int)(startVector.X + direction.X * i);
                        int positionY = (int)(startVector.Y + direction.Y * i);
                        destinationElements[positionX, positionY] = material.ApplyShader(VertexData.Lerp(start, end, i / distance));
                    });
                }
            });
        }

        public void DrawTriangle<T>(VertexData vertex0SS, VertexData vertex1SS, VertexData vertex2SS, Texture<T> destination, IMaterial<VertexData, T> material)
            where T : struct
        {
            DrawLine(vertex0SS, vertex1SS, destination, material);
            DrawLine(vertex1SS, vertex2SS, destination, material);
            DrawLine(vertex2SS, vertex0SS, destination, material);
        }

        public void FillTriangle<T>(VertexData vertex0SS, VertexData vertex1SS, VertexData vertex2SS, Texture<T> destination, IMaterial<VertexData, T> material)
            where T : struct
        {
            int minX = (int)Math.Min(Math.Min(vertex0SS.Position.X, vertex1SS.Position.X), vertex2SS.Position.X);
            int minY = (int)Math.Min(Math.Min(vertex0SS.Position.Y, vertex1SS.Position.Y), vertex2SS.Position.Y);

            int maxX = (int)Math.Max(Math.Max(vertex0SS.Position.X, vertex1SS.Position.X), vertex2SS.Position.X);
            int maxY = (int)Math.Max(Math.Max(vertex0SS.Position.Y, vertex1SS.Position.Y), vertex2SS.Position.Y);

            _commandQueue.Enqueue(() =>
            {
                destination.Deconstruct(out T[,] destinationElements, out _);
                lock (destinationElements)
                {
                    Parallel.For(minX, maxX, x =>
                    {
                        Parallel.For(minY, maxY, y =>
                        {
                            Vector2 p = new Vector2(x, y);
                            Vector3 barycentricCoordinates = BarycentricCoordinates(p, vertex0SS.Position.ToVector2(), vertex1SS.Position.ToVector2(), vertex2SS.Position.ToVector2());
                            if (!IsPointInTriangle(barycentricCoordinates))
                                return;

                            destinationElements[x, y] = material.ApplyShader(vertex0SS * barycentricCoordinates.X +
                                                                             vertex1SS * barycentricCoordinates.Y +
                                                                             vertex2SS * barycentricCoordinates.Z);
                        });
                    });
                }
            });

            static Vector3 BarycentricCoordinates(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
            {
                float determinant = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
                Vector2 coordinates = new Vector2(
                    ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / determinant,
                    ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / determinant);

                return new Vector3(
                    coordinates.X,
                    coordinates.Y,
                    1.0f - coordinates.X - coordinates.Y);
            }

            static bool IsPointInTriangle(Vector3 barycentricCoordinates) => barycentricCoordinates.X >= 0
                                                                             && barycentricCoordinates.Y >= 0
                                                                             && barycentricCoordinates.Z >= 0;
        }

        public void Clear() => _commandQueue.Clear();

        public void Execute()
        {
            while (_commandQueue.Count > 0)
                _commandQueue.Dequeue()?.Invoke();
        }
    }
}