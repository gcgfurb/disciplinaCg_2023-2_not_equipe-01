using CG_N2_4.Shaders;

namespace CG_N2_4;
using OpenTK.Graphics.OpenGL4;

public abstract class Objeto
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private readonly PrimitiveType _primitiveType;
    private float[] _vertex;
    private readonly List<PontoCoordenada> _points = new();
    private readonly Shader _shader = new("Shaders/shader.vert", "Shaders/shaderBranca.frag");
    private readonly List<Objeto> _objects = new();

    protected Objeto(PrimitiveType primitiveType)
    {
        _primitiveType = primitiveType;
    }

    protected void AdicionarPonto(PontoCoordenada pontoCoordenada) => _points.Add(pontoCoordenada);
    
    protected void AlterarPontoPorIndex(PontoCoordenada pontoCoordenada, int index) => _points[index] = pontoCoordenada;

    public void Atualizar()
    {
        _vertex = new float[_points.Count * 3];

        int pontoLista = 0;
        for (var i = 0; i < _vertex.Length; i += 3)
        {
            _vertex[i] = (float)_points[pontoLista].X;
            _vertex[i + 1] = (float)_points[pontoLista].Y;
            _vertex[i + 2] = 0.0f;
            pontoLista++;
        }

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertex.Length * sizeof(float), _vertex, BufferUsageHint.StaticDraw);
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }
    
    public void Renderizar()
    {
        GL.PointSize(10f);
        GL.BindVertexArray(_vertexArrayObject);
        _shader.Use();
        GL.DrawArrays(_primitiveType, 0, _points.Count);
    }

    protected void AdicionarObjetoFilho(Objeto objeto) => _objects.Add(objeto);
}