using ConsoleGameRenderer.Rendering;
using System.Numerics;
using System.Drawing;
using ConsoleGameRenderer.Component;
using ConsoleGameRenderer.Rendering.Shading;
using System.Diagnostics;
using ConsoleGameRenderer.Rendering.Pipeline;
using ConsoleGameRenderer.Engine;

const int WIDTH = 16 * 3;
const int HEIGHT = 9 * 3;
const float FIXED_DELTA_TIME = 1.0f / 144.0f;
const string RESOURCES_PATH = "D:\\chich\\Projects\\UCM\\Programming\\CSharp\\Console Applications\\ConsoleGameRenderer\\Resources\\";

Transform testTransform = new Transform(Vector3.Zero, Quaternion.Identity, Vector3.One * 4.0f);

// Texture Loading
const string IMAGE_NAME = "Bocchi1.png";
Bitmap testImageBitmap = new Bitmap(RESOURCES_PATH + IMAGE_NAME);
Color[,] testImageColors = new Color[testImageBitmap.Width, testImageBitmap.Height];
for (int x = 0; x < testImageBitmap.Width; x++)
    for (int y = 0; y < testImageBitmap.Height; y++)
        testImageColors[x, y] = testImageBitmap.GetPixel(x, y);

char[] luminanceCharacters = new char[] { ' ', '.', ':', '-', '=', '+', '*', '#', '%' };
Func<Color, ConsoleSpriteElement> spriteElementFromColor = c => new ConsoleSpriteElement(c, luminanceCharacters[(int)(c.GetBrightness() * (luminanceCharacters.Length - 1))]);

var vertexShader = new UniformShader<(IRenderable<Color>, CameraBundle), VertexData, VertexData>((u, v) =>
{
    (IRenderable<Color> renderable, CameraBundle cameraBundle) = u;

    Vector4 vertexPositionOS = v.Position;
    Matrix4x4 projectionMatrix = Matrix4x4.CreateOrthographic(cameraBundle.Camera.Size.X, cameraBundle.Camera.Size.Y, cameraBundle.Camera.NearPlaneDistance, cameraBundle.Camera.FarPlaneDistance);

    Vector4 vertexPositionCS = Vector4.Transform(Vector4.Transform(Vector4.Transform(vertexPositionOS, renderable.Transform.ObjectToWorldMatrix), cameraBundle.ViewMatrix), projectionMatrix);
    Vector4 normalCS = Vector4.Transform(Vector4.Transform(Vector4.Transform(v.Normal, renderable.Transform.ObjectToWorldMatrix), cameraBundle.ViewMatrix), projectionMatrix);
    // TODO - Move this to strategy, return them in CS
    return v with 
    { 
        Position = vertexPositionCS / vertexPositionCS.W,
    };
},
(string id, object? passed, (IRenderable<Color>, CameraBundle) stored, out (IRenderable<Color>, CameraBundle) assigned) =>
{
    if (passed is IRenderable<Color> renderable)
    {
        assigned = (renderable, stored.Item2);
        return true;
    }

    if (passed is CameraBundle cameraBundle)
    {
        assigned = (stored.Item1, cameraBundle);
        return true;
    }

    assigned = stored;
    return false;
});

//Vector3 mainLightDirection = Vector3.Normalize(new Vector3(2.0f, 1.0f, 4.0f) * -1.0f);
Vector3 mainLightDirection = Vector3.Normalize(new Vector3(0.15f, 0.0f, 1.0f));
var fragmentShader = new UniformShader<(IRenderable<Color>, Sprite<Color>), VertexData, Color>((u, v) =>
{
    return (new Vector4(Vector3.Dot(v.Normal.ToVector3(), -mainLightDirection) * 0.5f + 0.5f) *
            u.Item2.SampleSprite(v.UV0).ToVector4())
            .ToColor();
},
(string id, object? passed, (IRenderable<Color>, Sprite<Color>) stored, out (IRenderable<Color>, Sprite<Color>) assigned) =>
{
    if (passed is IRenderable<Color> renderable)
    {
        assigned = (renderable, stored.Item2);
        return true;
    }

    if (passed is Sprite<Color> sprite)
    {
        assigned = (stored.Item1, sprite);
        return true;
    }

    assigned = stored;
    return false;
});

Sprite<Color> sprite = new Sprite<Color>(new Texture<Color>(testImageColors), false);
SpriteRenderable<Color> testSpriteRenderable = new SpriteRenderable<Color>(sprite, testTransform, new Material<VertexData, VertexData>(vertexShader), new Material<VertexData, Color>(fragmentShader));
testSpriteRenderable.FragmentMaterial.TryPassUniforms("uSprite", sprite);

Texture<Color> screenTexture = new Texture<Color>(WIDTH, HEIGHT);
BufferedRenderingContext<Color> renderingContext = new BufferedRenderingContext<Color>(screenTexture);

Random random = new Random();
Console.CursorVisible = false;

Stopwatch time = Stopwatch.StartNew();
while (true)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    MeshRenderable testMeshRenderable = new MeshRenderable(Mesh.CreateTorus(0.5f, 0.2f, 12, 8), new Transform(Vector3.Zero, Quaternion.CreateFromYawPitchRoll(
        MathF.PI * (float)time.Elapsed.TotalSeconds,
        MathF.PI * (float)time.Elapsed.TotalSeconds,
        MathF.PI * (float)time.Elapsed.TotalSeconds), Vector3.One * 2.0f), new Material<VertexData, VertexData>(vertexShader), new Material<VertexData, Color>(fragmentShader));

    CameraBundle testCameraBundle = new CameraBundle(new Transform(Vector3.UnitZ * -5.0f), new Camera(new Vector2(9.0f, 9.0f) * 0.5f, 90.0f, 0.1f, 100.0f));
    renderingContext.ScheduleClear(true, true, Color.BlueViolet);
    renderingContext.ScheduleRenderersDrawing(new IRenderable<Color>[] { testMeshRenderable }, in testCameraBundle);
    renderingContext.Submit();

    Console.SetCursorPosition(0, 0);

    for (int y = 0; y < HEIGHT - 1; y++)
    {
        for (int x = 0; x < WIDTH - 1; x++)
        {
            Color element = screenTexture.GetTextureElement(x, y);
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = element.ToConsoleColor();
            Console.Write(spriteElementFromColor(element).Character);
        }
    }
    Console.WriteLine();
    Console.WriteLine($"FPS: {1.0f / stopwatch.Elapsed.TotalSeconds}");

    Thread.Sleep((int)(1000 * FIXED_DELTA_TIME));
}