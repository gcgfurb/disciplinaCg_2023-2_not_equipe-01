using CG_N2_5.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_N2_5;

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

    private Ponto _pontoCentro;
    private Circulo _circuloMenor;
    private Circulo _circuloMaior;
    private Retangulo _bboxInterna;

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

        var nmueroCirculoSimetrico = Matematica.GerarPtosCirculoSimetrico(0.3);

        _pontoCentro = new Ponto(new PontoCoordenada(0.3, 0.3));
        _circuloMenor = new Circulo(_pontoCentro.ObterPontoCoordenadaPorIndex(0), 0.1);
        _circuloMaior = new Circulo(_pontoCentro.ObterPontoCoordenadaPorIndex(0), 0.3);
        _bboxInterna = new Retangulo(
            new PontoCoordenada(-nmueroCirculoSimetrico, -nmueroCirculoSimetrico) +
            _pontoCentro.ObterPontoCoordenadaPorIndex(0),
            new PontoCoordenada(nmueroCirculoSimetrico, nmueroCirculoSimetrico) +
            _pontoCentro.ObterPontoCoordenadaPorIndex(0));

        _shaderVermelha = ShaderHelper.Vermelho;
        _shaderVerde = ShaderHelper.Verde;
        _shaderAzul = ShaderHelper.Azul;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        Sru3D();

        _pontoCentro.Renderizar();
        _circuloMenor.Renderizar();
        _bboxInterna.Renderizar();
        _circuloMaior.Renderizar();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        var input = KeyboardState;
        var pontoAtual = _pontoCentro.ObterPontoCoordenadaPorIndex(0);
        double distanciaParaPercorrer = 0.0005;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        else
        {
            var delta = new PontoCoordenada(0, 0);

            if (input.IsKeyDown(Keys.D)) delta.X = distanciaParaPercorrer;
            else if (input.IsKeyDown(Keys.E)) delta.X = -distanciaParaPercorrer;
            else if (input.IsKeyDown(Keys.C)) delta.Y = distanciaParaPercorrer;
            else if (input.IsKeyDown(Keys.B)) delta.Y = -distanciaParaPercorrer;

            MoverPonto(ref pontoAtual, delta);
        }
    }

    private void MoverPonto(ref PontoCoordenada pontoAtual, PontoCoordenada delta)
    {
        pontoAtual.X += delta.X;
        pontoAtual.Y += delta.Y;

        if (Matematica.Dentro(_bboxInterna.BBox, pontoAtual))
        {
            _bboxInterna.PrimitiveType = PrimitiveType.LineLoop;
            _pontoCentro.AlterarPontoPorIndex(pontoAtual, 0);
            _pontoCentro.Atualizar();
            _circuloMenor.Atualizar(pontoAtual);
        }
        else
        {
            _bboxInterna.PrimitiveType = PrimitiveType.Points;
            if (Matematica.DistanciaQuadrado(pontoAtual, new PontoCoordenada(0.3, 0.3)) <= 0.09)
            {
                _pontoCentro.AlterarPontoPorIndex(pontoAtual, 0);
                _pontoCentro.Atualizar();
                _circuloMenor.Atualizar(pontoAtual);
            }
            else
            {
                pontoAtual.X -= delta.X;
                pontoAtual.Y -= delta.Y;
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