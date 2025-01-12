﻿using CG_N2_4.Shaders;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

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

    private Spline _spline;

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

        _spline = new Spline();

        _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
        _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
        _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        Sru3D();

        _spline.Renderizar();

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
            _spline.AlterarPontoControle();
        else if (input.IsKeyPressed(Keys.D))
            _spline.MoverSpline(new PontoCoordenada(0.05));
        else if (input.IsKeyPressed(Keys.E))
            _spline.MoverSpline(new PontoCoordenada(-0.05));
        else if (input.IsKeyPressed(Keys.C))
            _spline.MoverSpline(new PontoCoordenada(y: 0.05));
        else if (input.IsKeyPressed(Keys.B))
            _spline.MoverSpline(new PontoCoordenada(y: -0.05));
        else if (input.IsKeyPressed(Keys.Minus))
            _spline.RetirarPontoSpline();
        else if (input.IsKeyPressed(Keys.KeyPadAdd) ||
                 (input.IsKeyDown(Keys.LeftShift) &&
                  input.IsKeyPressed(Keys.Equal)))
            _spline.AdicionarPontoSpline();
        else if (input.IsKeyPressed(Keys.R))
            _spline.Resetar();
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