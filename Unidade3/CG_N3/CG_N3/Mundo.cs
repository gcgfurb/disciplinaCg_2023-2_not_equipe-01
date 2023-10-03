using CG_N3.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_N3;

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
    
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;

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

        GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos,
            BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);
        
        _shaderVermelha = ShaderHelper.Vermelho;
        _shaderVerde = ShaderHelper.Verde;
        _shaderAzul = ShaderHelper.Azul;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        Sru3D();
        
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        var keyboardInput = KeyboardState;
        var mouseInput = MouseState;
        
        if (keyboardInput.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        else if (mouseInput.IsButtonPressed(MouseButton.Right))
        {
            var mousePosition = Utilitario.NDC_TelaSRU(Size.X, Size.Y, new PontoCoordenada(mouseInput.X, mouseInput.Y));
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

        GL.DeleteProgram(_shaderVermelha.Handle);
        GL.DeleteProgram(_shaderAzul.Handle);
        GL.DeleteProgram(_shaderVerde.Handle);

        base.OnUnload();
    }

    private void Sru3D()
    {
        GL.BindVertexArray(_vertexArrayObject);
        _shaderVermelha.Use();
        GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        _shaderVerde.Use();
        GL.DrawArrays(PrimitiveType.Lines, 2, 2);
        _shaderAzul.Use();
        GL.DrawArrays(PrimitiveType.Lines, 4, 2);
    }
}