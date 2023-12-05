using CG_N4_V2.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_N4_V2;

public class Cubo
{
    protected int VertexBufferObject;
    protected int VertexArrayObject;
    protected PrimitiveType PrimitiveType = PrimitiveType.Triangles;
    protected float[] Vertex;
    protected readonly List<PontoCoordenada> PontoCoordenadas = new();
    protected Shader Shader;
    protected Transformacao4D Matriz = new();
    public Matrix4? Model;

    protected void AdicionarPonto(PontoCoordenada pontoCoordenada)
    {
        PontoCoordenadas.Add(pontoCoordenada);
    }

    public virtual void Atualizar()
    {
    }

    public virtual void Renderizar(Transformacao4D matrizGrafo, Camera camera)
    {
    }
}