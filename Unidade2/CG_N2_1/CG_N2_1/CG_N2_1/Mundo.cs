using CG_N2_1.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_N2_1;

public class Mundo : GameWindow
{
    private float[] _vertices;

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    private Circulo _circulo;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _circulo = new Circulo();
        _vertices = _circulo.GerarPontosCirculo(_vertices);

        _vertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);

        _shader = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);

        _circulo.Renderizar();
        
        GL.PointSize(1f);
        var comprimentoDaLinha = 0.5f;
        float[] linhaVertices = {
            0.0f, 0.0f,
            0.0f, comprimentoDaLinha,
            0.0f, 0.0f,
            comprimentoDaLinha, 0.0f
        };

        var linhaVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, linhaVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, linhaVertices.Length * sizeof(float), linhaVertices, BufferUsageHint.StaticDraw);

        var linhaVao = GL.GenVertexArray();
        GL.BindVertexArray(linhaVao);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(linhaVao);
        GL.DrawArrays(PrimitiveType.Lines, 0, 4);

        GL.DeleteBuffer(linhaVbo);
        GL.DeleteVertexArray(linhaVao);

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