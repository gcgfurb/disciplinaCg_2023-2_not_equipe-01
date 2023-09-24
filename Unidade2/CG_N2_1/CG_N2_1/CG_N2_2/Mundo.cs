using CG_N2_2.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_N2_2;

public class Mundo : GameWindow
{
    private readonly float[] _sruEixos =
    {
        0.0f,
        0.0f,
        0.0f,
        0.5f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.5f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.0f,
        0.5f
    };

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    private PrimitiveType _currentPrimitiveType;
    private readonly List<Ponto> _currentPrimitivePoints = new();

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.5f, 0.5f, 1f);

        _vertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);

        _shader = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        var retangulo = new Retangulo(_currentPrimitiveType, _currentPrimitivePoints);
        retangulo.CalcularVertex();
        
        _shader.Use();

        retangulo.Atualizar();
        retangulo.Renderizar();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        
        if (input.IsKeyPressed(Keys.Space))
        {
            switch (_currentPrimitiveType)
            {
                case PrimitiveType.Points:
                    _currentPrimitiveType = PrimitiveType.Lines;
                    break;
                case PrimitiveType.Lines:
                    _currentPrimitiveType = PrimitiveType.LineLoop;
                    break;
                case PrimitiveType.LineLoop:
                    _currentPrimitiveType = PrimitiveType.LineStrip;
                    break;
                case PrimitiveType.LineStrip:
                    _currentPrimitiveType = PrimitiveType.Triangles;
                    break;
                case PrimitiveType.Triangles:
                    _currentPrimitiveType = PrimitiveType.TriangleStrip;
                    break;
                case PrimitiveType.TriangleStrip:
                    _currentPrimitiveType = PrimitiveType.TriangleFan;
                    break;
                case PrimitiveType.TriangleFan:
                    _currentPrimitiveType = PrimitiveType.Points;
                    break;
            }
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        GL.DeleteProgram(_shader.Handle);

        base.OnUnload();
    }
}