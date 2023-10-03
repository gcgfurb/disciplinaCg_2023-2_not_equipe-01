using CG_N3.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N3;

public abstract class Objeto
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    public PrimitiveType PrimitiveType;
    public readonly float PrimitiveSize;
    private float[] _vertex;
    private readonly List<PontoCoordenada> _pontoCoordenadas = new();
    private Shader _shader = ShaderHelper.Branca;
    private readonly List<Objeto> _objetosFilhos = new();

    protected Objeto(PrimitiveType primitiveType, float primitiveSize = 10)
    {
        PrimitiveType = primitiveType;
        PrimitiveSize = primitiveSize;
    }
    
    public void AlterarShader(Shader shader) => _shader = shader;

    protected void AdicionarPonto(PontoCoordenada pontoCoordenada) => _pontoCoordenadas.Add(pontoCoordenada);

    public void SobreescreverPontos(List<PontoCoordenada> pontosCoordenadas)
    {
        _pontoCoordenadas.Clear();
        _pontoCoordenadas.AddRange(pontosCoordenadas);
    }
    
    public void AlterarPontoPorIndex(PontoCoordenada pontoCoordenada, int index) => _pontoCoordenadas[index] = pontoCoordenada;

    public PontoCoordenada ObterPontoCoordenadaPorIndex(int index) => _pontoCoordenadas[index];

    public void Atualizar()
    {
        _vertex = new float[_pontoCoordenadas.Count * 3];

        int pontoLista = 0;
        for (var i = 0; i < _vertex.Length; i += 3)
        {
            _vertex[i] = (float)_pontoCoordenadas[pontoLista].X;
            _vertex[i + 1] = (float)_pontoCoordenadas[pontoLista].Y;
            _vertex[i + 2] = (float)_pontoCoordenadas[pontoLista].Z;
            pontoLista++;
        }
        
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertex.Length * 4, _vertex, BufferUsageHint.StaticDraw);
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, 0);
        GL.EnableVertexAttribArray(0);
    }
    
    public void Renderizar()
    {
        GL.PointSize(PrimitiveSize);
        GL.BindVertexArray(_vertexArrayObject);
        _shader.Use();
        GL.DrawArrays(PrimitiveType, 0, _pontoCoordenadas.Count);
        
        _objetosFilhos.ForEach(x => x.Renderizar());
    }

    public void Resetar()
    {
        _pontoCoordenadas.Clear();
        _objetosFilhos.Clear();
    }
    
    protected void AdicionarObjetoFilho(Objeto objeto) => _objetosFilhos.Add(objeto);
}