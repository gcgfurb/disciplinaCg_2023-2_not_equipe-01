using OpenTK.Graphics.OpenGL4;

namespace CG_N2_2;

public class Retangulo
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private readonly PrimitiveType _primitiveType;
    private float[] _vertex;
    private readonly List<Ponto> _points;
    
    public void CalcularVertex()
    {
        _vertex = new float[_points.Count * 3];

        int pontoLista = 0;
        for (var i = 0; i < _vertex.Length; i+= 3)
        {
            _vertex[i] = (float)_points[pontoLista].X;
            _vertex[i + 1] = (float)_points[pontoLista].Y;
            _vertex[i + 2] = 0.0f;
            pontoLista++;
        }
    }

    public Retangulo(PrimitiveType? primitiveType, List<Ponto> points)
    {
        _primitiveType = primitiveType ?? PrimitiveType.Points;
        _points = points.Any()
            ? points
            : new List<Ponto>
            {
                new(-0.5, -0.5),
                new(0.5, -0.5),
                new(0.5, 0.5),
                new(-0.5, 0.5)
            };
    }

    public void Atualizar()
    {
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
        GL.DrawArrays(_primitiveType, 0, _points.Count);
    }
}