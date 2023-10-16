using ConsoleGameRenderer.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct Mesh(VertexData[] VerticesData, int[] Indices)
    {
        public static readonly Mesh s_Empty = new Mesh(Array.Empty<VertexData>(), Array.Empty<int>());

        public static readonly Mesh s_Quad = new Mesh(
            new VertexData[]
            {
                new VertexData(new Vector3(-0.5f, 0.5f, 0.0f),  Color.Blue.ToVector4(),     new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 0.0f, 0.0f),   new Vector2(0.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, 0.5f, 0.0f),   Color.Red.ToVector4(),      new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f),  new Vector2(1.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, -0.5f, 0.0f),  Color.Green.ToVector4(),    new Vector3(0.0f, 0.0f, -1.0f), new Vector3(-1.0f, 0.0f, 0.0f),  new Vector2(1.0f, 0.0f)),
                new VertexData(new Vector3(-0.5f, -0.5f, 0.0f), Color.Yellow.ToVector4(),   new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, 1.0f, 0.0f),   new Vector2(0.0f, 0.0f)),
            },
            new int[]
            {
                0, 1, 2,
                0, 2, 3,
            });

        public static readonly Mesh s_Cube = new Mesh(
            new VertexData[]
            {
                //Full data with normals and tangents

                // Front
                new VertexData(new Vector3(-0.5f, 0.5f, 0.5f),  Color.Blue.ToVector4(),     new Vector3(0.0f, 0.0f, 1.0f),  new Vector3(1.0f, 0.0f, 0.0f),   new Vector2(0.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, 0.5f, 0.5f),   Color.Red.ToVector4(),      new Vector3(0.0f, 0.0f, 1.0f),  new Vector3(0.0f, -1.0f, 0.0f),  new Vector2(1.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, -0.5f, 0.5f),  Color.Green.ToVector4(),    new Vector3(0.0f, 0.0f, 1.0f),  new Vector3(-1.0f, 0.0f, 0.0f),  new Vector2(1.0f, 0.0f)),
                new VertexData(new Vector3(-0.5f, -0.5f, 0.5f), Color.Yellow.ToVector4(),   new Vector3(0.0f, 0.0f, 1.0f),  new Vector3(0.0f, 1.0f, 0.0f),   new Vector2(0.0f, 0.0f)),

                // Back
                new VertexData(new Vector3(-0.5f, 0.5f, -0.5f),  Color.Blue.ToVector4(),     new Vector3(0.0f, 0.0f, -1.0f), new Vector3(-1.0f, 0.0f, 0.0f),  new Vector2(0.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, 0.5f, -0.5f),   Color.Red.ToVector4(),      new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f),  new Vector2(1.0f, 1.0f)),
                new VertexData(new Vector3(0.5f, -0.5f, -0.5f),  Color.Green.ToVector4(),    new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 0.0f, 0.0f),   new Vector2(1.0f, 0.0f)),
                new VertexData(new Vector3(-0.5f, -0.5f, -0.5f), Color.Yellow.ToVector4(),   new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, 1.0f, 0.0f),   new Vector2(0.0f, 0.0f)),
            },
            new int[]
            {
                // Front
                0, 1, 2,
                2, 3, 0,
                // Back
                4, 5, 6,
                6, 7, 4,
                // Left
                4, 0, 3,
                3, 7, 4,
                // Right
                1, 5, 6,
                6, 2, 1,
                // Top
                3, 2, 6,
                6, 7, 3,
                // Bottom
                4, 5, 1,
                1, 0, 4,
            });

        public static readonly Mesh s_Dodecahedron = new Mesh(
            new VertexData[]
            {
                new VertexData(new Vector3(-0.577350269189625764f, 0.577350269189625764f, 0.577350269189625764f), Color.Blue.ToVector4(),     new Vector3(-0.577350269189625764f, 0.577350269189625764f, 0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.934172358962715696f, 0.356822089773089931f, 0.0f), Color.Red.ToVector4(),      new Vector3(0.934172358962715696f, 0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.934172358962715696f, -0.356822089773089931f, 0.0f), Color.Green.ToVector4(),    new Vector3(0.934172358962715696f, -0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.577350269189625764f, -0.577350269189625764f, 0.577350269189625764f), Color.Yellow.ToVector4(),   new Vector3(-0.577350269189625764f, -0.577350269189625764f, 0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.0f, 0.934172358962715696f, 0.356822089773089931f), Color.Purple.ToVector4(),   new Vector3(0.0f, 0.934172358962715696f, 0.356822089773089931f), Vector3.Zero, Vector2.Zero),

                new VertexData(new Vector3(0.0f, -0.934172358962715696f, 0.356822089773089931f), Color.Cyan.ToVector4(),     new Vector3(0.0f, -0.934172358962715696f, 0.356822089773089931f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.934172358962715696f, 0.356822089773089931f, 0.0f), Color.Purple.ToVector4(),   new Vector3(-0.934172358962715696f, 0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.934172358962715696f, -0.356822089773089931f, 0.0f), Color.Cyan.ToVector4(),     new Vector3(-0.934172358962715696f, -0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Color.Blue.ToVector4(),     new Vector3(0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Color.Red.ToVector4(),      new Vector3(-0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),

                new VertexData(new Vector3(-0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Color.Green.ToVector4(),    new Vector3(-0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Color.Yellow.ToVector4(),   new Vector3(0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.577350269189625764f, 0.577350269189625764f, 0.577350269189625764f), Color.Purple.ToVector4(),   new Vector3(0.577350269189625764f, 0.577350269189625764f, 0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.577350269189625764f, -0.577350269189625764f, 0.577350269189625764f), Color.Cyan.ToVector4(),     new Vector3(0.577350269189625764f, -0.577350269189625764f, 0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.0f, 0.934172358962715696f, -0.356822089773089931f), Color.Blue.ToVector4(),     new Vector3(0.0f, 0.934172358962715696f, -0.356822089773089931f), Vector3.Zero, Vector2.Zero),

                new VertexData(new Vector3(0.0f, -0.934172358962715696f, -0.356822089773089931f), Color.Red.ToVector4(),      new Vector3(0.0f, -0.934172358962715696f, -0.356822089773089931f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.934172358962715696f, 0.356822089773089931f, 0.0f), Color.Green.ToVector4(),    new Vector3(0.934172358962715696f, 0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.934172358962715696f, -0.356822089773089931f, 0.0f), Color.Yellow.ToVector4(),   new Vector3(0.934172358962715696f, -0.356822089773089931f, 0.0f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Color.Purple.ToVector4(),   new Vector3(-0.577350269189625764f, 0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Color.Cyan.ToVector4(),     new Vector3(-0.577350269189625764f, -0.577350269189625764f, -0.577350269189625764f), Vector3.Zero, Vector2.Zero),
            },
            new int[]
            {
                0, 1, 4,
                0, 4, 7,
                0, 7, 8,
                0, 8, 16,
                0, 16, 17,
                1, 9, 5,
                1, 5, 10,
                1, 10, 11,
                1, 11, 4,
                2, 3, 6,
                2, 6, 13,
                2, 13, 12,
                2, 12, 9,
                2, 9, 1,
                3, 14, 15,
                3, 15, 6,
                3, 6, 11,
                3, 11, 10,
                3, 10, 5,
                4, 11, 6,
                4, 6, 15,
                4, 15, 14,
                4, 14, 7,
                5, 9, 12,
                5, 12, 13,
                5, 13, 8,
                5, 8, 7,
                6, 7, 8,
                6, 8, 16,
                6, 16, 17,
                6, 17, 15,
                9, 12, 13,
                9, 13, 16,
                9, 16, 8,
                10, 11, 17,
                10, 17, 15,
                10, 15, 17,
                12, 13, 16,
                13, 16, 17,
                14, 15, 17,
                14, 17, 16,
                14, 16, 13,
            });

        public static readonly Mesh s_Icosahedron = new Mesh(
            new VertexData[]
            {
                new VertexData(new Vector3(-0.525731112119133606f, 0.0f, 0.850650808352039932f), Color.Blue.ToVector4(),     new Vector3(-0.525731112119133606f, 0.0f, 0.850650808352039932f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.525731112119133606f, 0.0f, 0.850650808352039932f),  Color.Red.ToVector4(),      new Vector3(0.525731112119133606f, 0.0f, 0.850650808352039932f),  Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.525731112119133606f, 0.0f, -0.850650808352039932f), Color.Green.ToVector4(),    new Vector3(-0.525731112119133606f, 0.0f, -0.850650808352039932f), Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.525731112119133606f, 0.0f, -0.850650808352039932f),  Color.Yellow.ToVector4(),   new Vector3(0.525731112119133606f, 0.0f, -0.850650808352039932f),  Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.0f, 0.850650808352039932f, 0.525731112119133606f),  Color.Purple.ToVector4(),   new Vector3(0.0f, 0.850650808352039932f, 0.525731112119133606f),   Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.0f, 0.850650808352039932f, -0.525731112119133606f), Color.Cyan.ToVector4(),     new Vector3(0.0f, 0.850650808352039932f, -0.525731112119133606f),  Vector3.Zero, Vector2.Zero),

                new VertexData(new Vector3(0.0f, -0.850650808352039932f, 0.525731112119133606f), Color.Purple.ToVector4(),   new Vector3(0.0f, -0.850650808352039932f, 0.525731112119133606f),   Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.0f, -0.850650808352039932f, -0.525731112119133606f), Color.Cyan.ToVector4(),     new Vector3(0.0f, -0.850650808352039932f, -0.525731112119133606f),  Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.850650808352039932f, -0.525731112119133606f, 0.0f), Color.Blue.ToVector4(),     new Vector3(-0.850650808352039932f, -0.525731112119133606f, 0.0f),  Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(-0.850650808352039932f, 0.525731112119133606f, 0.0f),  Color.Red.ToVector4(),      new Vector3(-0.850650808352039932f, 0.525731112119133606f, 0.0f),   Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.850650808352039932f, -0.525731112119133606f, 0.0f),  Color.Green.ToVector4(),    new Vector3(0.850650808352039932f, -0.525731112119133606f, 0.0f),   Vector3.Zero, Vector2.Zero),
                new VertexData(new Vector3(0.850650808352039932f, 0.525731112119133606f, 0.0f),   Color.Yellow.ToVector4(),   new Vector3(0.850650808352039932f, 0.525731112119133606f, 0.0f),    Vector3.Zero, Vector2.Zero),
            },
            new int[]
            {
                0, 4, 1,
                0, 9, 4,
                9, 5, 4,
                4, 5, 8,
                4, 8, 1,
                8, 10, 1,
                8, 3, 10,
                5, 3, 8,
                5, 2, 3,
                2, 7, 3,
                7, 10, 3,
                7, 6, 10,
                7, 11, 6,
                11, 0, 6,
                0, 1, 6,
                6, 1, 10,
                9, 0, 11,
                9, 11, 2,
                9, 2, 5,
                7, 2, 11,
            });

        public static Mesh CreateTorus(float outterRadius, float ringRadius, int outterSubdivisions, int ringSubdivisions)
        {
            List<VertexData> vertices = new List<VertexData>();
            List<int> indices = new List<int>();
            for (int i = 0; i < outterSubdivisions; i++)
            {
                float outterAngle = (float)i / (float)outterSubdivisions * MathF.PI * 2.0f;
                Vector3 outterDirection = new Vector3(MathF.Cos(outterAngle), 0.0f, MathF.Sin(outterAngle));
                for (int j = 0; j < ringSubdivisions; j++)
                {
                    float ringAngle = j / (float)ringSubdivisions * MathF.PI * 2.0f;
                    Vector3 ringDirection = new Vector3(MathF.Sin(ringAngle), MathF.Cos(ringAngle), 0.0f);
                    Vector3 position = outterDirection * outterRadius + ringDirection * ringRadius;
                    Vector3 normal = Vector3.Normalize(position);
                    Vector3 tangent = Vector3.Normalize(Vector3.Cross(normal, Vector3.UnitY));
                    Vector3 bitangent = Vector3.Normalize(Vector3.Cross(normal, tangent));
                    vertices.Add(new VertexData(position, Color.White.ToVector4(), normal, tangent, Vector2.Zero));
                }
            }
            for (int i = 0; i < outterSubdivisions; i++)
            {
                for (int j = 0; j < ringSubdivisions; j++)
                {
                    int a = i * ringSubdivisions + j;
                    int b = i * ringSubdivisions + (j + 1) % ringSubdivisions;
                    int c = (i + 1) % outterSubdivisions * ringSubdivisions + j;
                    int d = (i + 1) % outterSubdivisions * ringSubdivisions + (j + 1) % ringSubdivisions;
                    indices.Add(a);
                    indices.Add(c);
                    indices.Add(b);
                    indices.Add(b);
                    indices.Add(c);
                    indices.Add(d);
                }
            }
            return new Mesh(vertices.ToArray(), indices.ToArray());
        }
    }
}
