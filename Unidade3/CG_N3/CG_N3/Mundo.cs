using CG_N3.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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

    private readonly Objeto _objectPooling;

    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderBranca;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;
    private Poligno? _polignoSelecionado;
    private Poligno? _poligno;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _objectPooling = new Objeto(PrimitiveType.LineLoop);
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
        _shaderBranca = ShaderHelper.Branca;
        _shaderCiano = ShaderHelper.Ciano;
        _shaderMagenta = ShaderHelper.Magenta;
        _shaderAmarela = ShaderHelper.Amarela;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _objectPooling.Renderizar(new Transformacao4D(), _polignoSelecionado);

        Sru3D();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        else if (_polignoSelecionado is not null)
        {
            if (KeyboardState.IsKeyPressed(Keys.R))
                _polignoSelecionado.AlterarShader(_shaderVermelha);
            else if (KeyboardState.IsKeyPressed(Keys.G))
                _polignoSelecionado.AlterarShader(_shaderVerde);
            else if (KeyboardState.IsKeyPressed(Keys.B))
                _polignoSelecionado.AlterarShader(_shaderAzul);
            else if (KeyboardState.IsKeyPressed(Keys.D))
            {
                _polignoSelecionado.Remover();
                _polignoSelecionado = null;
            }
            else if (KeyboardState.IsKeyPressed(Keys.E))
                _polignoSelecionado.RemoverPontoMaisProximo(
                    Utilitario.NDC_TelaSRU(Size.X, Size.Y,
                        new PontoCoordenada(MousePosition.X, MousePosition.Y)));
            else if (KeyboardState.IsKeyPressed(Keys.P))
            {
                _polignoSelecionado.PrimitiveType = _polignoSelecionado.PrimitiveType != PrimitiveType.LineLoop
                    ? PrimitiveType.LineLoop
                    : PrimitiveType.LineStrip;
                _polignoSelecionado.Atualizar();
            }
            else if (KeyboardState.IsKeyDown(Keys.V))
            {
                var pontoCoordenada =
                    Utilitario.NDC_TelaSRU(Size.X, Size.Y, new PontoCoordenada(MousePosition.X, MousePosition.Y));
                var posicao = _polignoSelecionado.ObterPontoMaisPerto(pontoCoordenada);
                _polignoSelecionado.AlterarPontoPorIndex(pontoCoordenada, posicao);
            }
            else if (KeyboardState.IsKeyPressed(Keys.Left))
                _polignoSelecionado.MatrizTranslacaoXyz(-0.05, 0.0, 0.0);
            else if (KeyboardState.IsKeyPressed(Keys.Right))
                _polignoSelecionado.MatrizTranslacaoXyz(0.05, 0.0, 0.0);
            else if (KeyboardState.IsKeyPressed(Keys.Up))
                _polignoSelecionado.MatrizTranslacaoXyz(0.0, 0.05, 0.0);
            else if (KeyboardState.IsKeyPressed(Keys.Down))
                _polignoSelecionado.MatrizTranslacaoXyz(0.0, -0.05, 0.0);
            else if (KeyboardState.IsKeyPressed(Keys.Home))
                _polignoSelecionado.MatrizEscalaXyzbBox(0.5, 0.5, 0.5);
            else if (KeyboardState.IsKeyPressed(Keys.End))
                _polignoSelecionado.MatrizEscalaXyzbBox(2.0, 2.0, 2.0);
            else if (KeyboardState.IsKeyPressed(Keys.D3))
                _polignoSelecionado.MatrizRotacaoZbBox(10.0);
            else if (KeyboardState.IsKeyPressed(Keys.D4))
                _polignoSelecionado.MatrizRotacaoZbBox(-10.0);
        }

        if (KeyboardState.IsKeyPressed(Keys.Enter) && _poligno is not null)
        {
            _polignoSelecionado = _poligno;
            _poligno = null;
        }

        if (MouseState.IsButtonPressed(MouseButton.Right))
        {
            var mousePosition = Utilitario.NDC_TelaSRU(Size.X, Size.Y, new PontoCoordenada(MouseState.X, MouseState.Y));
            var pontosPoligono = new List<PontoCoordenada> { mousePosition, mousePosition };

            if (_poligno is null)
            {
                if (_polignoSelecionado is null)
                    _poligno = new Poligno(pontosPoligono, _objectPooling);
                else
                {
                    _poligno = new Poligno(pontosPoligono, _polignoSelecionado);
                    _polignoSelecionado = null;
                }
            }
            else
                _poligno.AdicionarPonto(mousePosition);
        }

        if (!MouseState.IsButtonPressed(MouseButton.Left)) return;
        {
            var pontoCoordenada =
                Utilitario.NDC_TelaSRU(Size.X, Size.Y, new PontoCoordenada(MousePosition.X, MousePosition.Y));
            _polignoSelecionado = null;
            _objectPooling.ScanLine(pontoCoordenada, ref _polignoSelecionado);
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        _objectPooling.OnUnload();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteProgram(_shaderBranca.Handle);
        GL.DeleteProgram(_shaderVermelha.Handle);
        GL.DeleteProgram(_shaderVerde.Handle);
        GL.DeleteProgram(_shaderAzul.Handle);
        GL.DeleteProgram(_shaderCiano.Handle);
        GL.DeleteProgram(_shaderMagenta.Handle);
        GL.DeleteProgram(_shaderAmarela.Handle);
    }

    private void Sru3D()
    {
        var identity = Matrix4.Identity;
        GL.BindVertexArray(_vertexArrayObject);
        _shaderVermelha.SetMatrix4("transform", identity);
        _shaderVermelha.Use();
        GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        _shaderVerde.SetMatrix4("transform", identity);
        _shaderVerde.Use();
        GL.DrawArrays(PrimitiveType.Lines, 2, 2);
        _shaderAzul.SetMatrix4("transform", identity);
        _shaderAzul.Use();
        GL.DrawArrays(PrimitiveType.Lines, 4, 2);
    }
}