using CG_N3.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N3;

public class Objeto
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    public PrimitiveType PrimitiveType;
    public readonly float PrimitiveSize;
    private float[] _vertex;
    private readonly List<PontoCoordenada> _pontoCoordenadas = new();
    private Shader _shader = ShaderHelper.Branca;
    private readonly List<Objeto> _objetosFilhos = new();
    private Transformacao4D _matriz = new();
    private static readonly Transformacao4D MatrizTmpTranslacao = new();
    private static readonly Transformacao4D MatrizTmpTranslacaoInversa = new();
    private static readonly Transformacao4D MatrizTmpEscala = new();
    private static Transformacao4D _matrizTmpRotacao = new();
    private Transformacao4D _matrizGlobal = new();
    private readonly Objeto? _objetoPai;
    private readonly BBox _bBox = new();
    private char _eixoRotacao = 'z';

    public Objeto(PrimitiveType primitiveType, float primitiveSize = 10, Objeto? objetoPai = null)
    {
        PrimitiveType = primitiveType;
        PrimitiveSize = primitiveSize;
        _objetoPai = objetoPai;

        _objetoPai?.AdicionarObjetoFilho(this);
    }

    public void AlterarShader(Shader shader) => _shader = shader;

    public void AdicionarPonto(PontoCoordenada pontoCoordenada)
    {
        _pontoCoordenadas.Add(pontoCoordenada);
        Atualizar();
    }

    public void SobreescreverPontos(List<PontoCoordenada> pontosCoordenadas)
    {
        _pontoCoordenadas.Clear();
        _pontoCoordenadas.AddRange(pontosCoordenadas);
    }

    public void AlterarPontoPorIndex(PontoCoordenada pontoCoordenada, int index)
    {
        _pontoCoordenadas[index] = pontoCoordenada;
        Atualizar();
    }

    public PontoCoordenada ObterPontoCoordenadaPorIndex(int index) => _pontoCoordenadas[index];

    public void Atualizar()
    {
        _vertex = new float[_pontoCoordenadas.Count * 3];

        var pontoLista = 0;
        for (var i = 0; i < _vertex.Length; i += 3)
        {
            _vertex[i] = (float)_pontoCoordenadas[pontoLista].X;
            _vertex[i + 1] = (float)_pontoCoordenadas[pontoLista].Y;
            _vertex[i + 2] = (float)_pontoCoordenadas[pontoLista].Z;
            pontoLista++;
        }

        _bBox.Atualizar(_pontoCoordenadas);
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertex.Length * 4, _vertex, BufferUsageHint.StaticDraw);
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, 0);
        GL.EnableVertexAttribArray(0);
    }

    public void Renderizar(Transformacao4D matrizGrafo, Objeto objetoSelecionado)
    {
        GL.PointSize(PrimitiveSize);
        GL.BindVertexArray(_vertexArrayObject);

        if (_objetoPai is not null)
        {
            matrizGrafo = matrizGrafo.MultiplicarMatriz(_matriz);
            _shader.SetMatrix4("transform", matrizGrafo.ObterDadosOpenTk());
            _shader.Use();
            GL.DrawArrays(PrimitiveType, 0, _pontoCoordenadas.Count);
            if (objetoSelecionado == this)
                _bBox.Desenhar(matrizGrafo);
        }

        _objetosFilhos.ForEach(x => x.Renderizar(matrizGrafo, objetoSelecionado));
    }

    public int ObterPontoMaisPerto(PontoCoordenada mousePto)
    {
        var num1 = 0;
        var num2 = Matematica.DistanciaQuadrado(mousePto, _pontoCoordenadas[num1]);

        for (var index = 1; index < _pontoCoordenadas.Count; ++index)
        {
            var num3 = Matematica.DistanciaQuadrado(mousePto, _pontoCoordenadas[index]);
            if (num3 < num2)
            {
                num2 = num3;
                num1 = index;
            }
        }

        return num1;
    }

    public void RemoverPontoMaisProximo(PontoCoordenada mousePto)
    {
        var num1 = ObterPontoMaisPerto(mousePto);

        _pontoCoordenadas.RemoveAt(num1);

        if (_pontoCoordenadas.Count < 2)
            Remover();
        else
            Atualizar();
    }

    public bool ScanLine(PontoCoordenada pontoCoordenada, ref Poligno? objetoSelecionado)
    {
        if (_bBox.Dentro(pontoCoordenada))
        {
            ushort num = 0;
            if (_pontoCoordenadas.Count >= 2)
            {
                for (var index = 0; index < _pontoCoordenadas.Count - 1; ++index)
                {
                    if (Matematica.ScanLine(pontoCoordenada, _matriz.MultiplicarPonto(_pontoCoordenadas[index]),
                            _matriz.MultiplicarPonto(_pontoCoordenadas[index + 1])))
                        ++num;
                }

                if (Matematica.ScanLine(pontoCoordenada, _matriz.MultiplicarPonto(_pontoCoordenadas[^1]),
                        _matriz.MultiplicarPonto(_pontoCoordenadas[0])))
                    ++num;
            }

            if (num % 2 != 0)
            {
                objetoSelecionado = (Poligno)this;
                return true;
            }
        }

        foreach (var objeto in _objetosFilhos)
            objeto.ScanLine(pontoCoordenada, ref objetoSelecionado);

        return false;
    }

    public void MatrizTranslacaoXyz(double tx, double ty, double tz)
    {
        var transformacao4D = new Transformacao4D();
        transformacao4D.AtribuirTranslacao(tx, ty, tz);
        _matriz = transformacao4D.MultiplicarMatriz(_matriz);
        Atualizar();
    }

    public void MatrizEscalaXyzbBox(double sx, double sy, double sz)
    {
        _matrizGlobal.AtribuirIdentidade();
        var obterCentro = _bBox.PontoCoordenadaCentral;
        MatrizTmpTranslacao.AtribuirTranslacao(-obterCentro.X, -obterCentro.Y, -obterCentro.Z);
        _matrizGlobal = MatrizTmpTranslacao.MultiplicarMatriz(_matrizGlobal);
        MatrizTmpEscala.AtribuirEscala(sx, sy, sz);
        _matrizGlobal = MatrizTmpEscala.MultiplicarMatriz(_matrizGlobal);
        MatrizTmpTranslacaoInversa.AtribuirTranslacao(obterCentro.X, obterCentro.Y, obterCentro.Z);
        _matrizGlobal = MatrizTmpTranslacaoInversa.MultiplicarMatriz(_matrizGlobal);
        _matriz = _matriz.MultiplicarMatriz(_matrizGlobal);
        Atualizar();
    }
    
    public void MatrizRotacaoZbBox(double angulo)
    {
        _matrizGlobal.AtribuirIdentidade();
        var obterCentro = _bBox.PontoCoordenadaCentral;
        MatrizTmpTranslacaoInversa.AtribuirTranslacao(-obterCentro.X, -obterCentro.Y, -obterCentro.Z);
        _matrizGlobal = MatrizTmpTranslacaoInversa.MultiplicarMatriz(_matrizGlobal);
        MatrizRotacaoEixo(angulo);
        _matrizGlobal = _matrizTmpRotacao.MultiplicarMatriz(_matrizGlobal);
        MatrizTmpTranslacaoInversa.AtribuirTranslacao(obterCentro.X, obterCentro.Y, obterCentro.Z);
        _matrizGlobal = MatrizTmpTranslacaoInversa.MultiplicarMatriz(_matrizGlobal);
        _matriz = _matriz.MultiplicarMatriz(_matrizGlobal);
        Atualizar();
    }

    private void MatrizRotacaoEixo(double angulo)
    {
        switch (_eixoRotacao)
        {
            case 'x':
                _matrizTmpRotacao.AtribuirRotacaoX(Transformacao4D.DegToRad * angulo);
                break;
            case 'y':
                _matrizTmpRotacao.AtribuirRotacaoY(Transformacao4D.DegToRad * angulo);
                break;
            case 'z':
                _matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DegToRad * angulo);
                break;
            default:
                Console.WriteLine("opção de eixoRotacao: ERRADA!");
                break;
        }
        Atualizar();
    }

    public void Remover()
    {
        _objetosFilhos.ForEach(x => x.Remover());
        _objetoPai?._objetosFilhos.Remove(this);
        OnUnload();
        _objetosFilhos.Clear();
        _pontoCoordenadas.Clear();
    }

    public void OnUnload()
    {
        _objetosFilhos.ForEach(x => x.OnUnload());
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
        GL.DeleteProgram(_shader.Handle);
    }

    public void AdicionarObjetoFilho(Objeto objeto) => _objetosFilhos.Add(objeto);
}