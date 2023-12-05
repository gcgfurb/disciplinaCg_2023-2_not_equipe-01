using CG_N4_V2.Common;
using OpenTK.Graphics.OpenGL4;

namespace CG_N4_V2;

public class RotationalCube : Cubo
{
    private readonly float[] _vertices =
    {
        // Position
        -0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f, 0.5f, -0.5f,
        0.5f, 0.5f, -0.5f,
        -0.5f, 0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f,

        -0.5f, -0.5f, 0.5f,
        0.5f, -0.5f, 0.5f,
        0.5f, 0.5f, 0.5f,
        0.5f, 0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f,
        -0.5f, -0.5f, 0.5f,

        -0.5f, 0.5f, 0.5f,
        -0.5f, 0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f,

        0.5f, 0.5f, 0.5f,
        0.5f, 0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, 0.5f,
        0.5f, 0.5f, 0.5f,

        -0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, 0.5f,
        0.5f, -0.5f, 0.5f,
        -0.5f, -0.5f, 0.5f,
        -0.5f, -0.5f, -0.5f,

        -0.5f, 0.5f, -0.5f,
        0.5f, 0.5f, -0.5f,
        0.5f, 0.5f, 0.5f,
        0.5f, 0.5f, 0.5f,
        -0.5f, 0.5f, 0.5f,
        -0.5f, 0.5f, -0.5f
    };

    public RotationalCube()
    {
        Shader = new Shader("Shaders/BasicLighting/shader.vert", "Shaders/BasicLighting/shader.frag");

        for (var i = 0; i < _vertices.Length; i += 3)
        {
            var x = _vertices[i];
            var y = _vertices[i + 1];
            var z = _vertices[i + 2];
            AdicionarPonto(new PontoCoordenada(x, y, z));
        }
        
        Atualizar();
    }

    public sealed override void Atualizar()
    {
        Vertex = new float[PontoCoordenadas.Count * 3];

        var pontoLista = 0;
        for (var i = 0; i < Vertex.Length; i += 3)
        {
            Vertex[i] = (float)PontoCoordenadas[pontoLista].X;
            Vertex[i + 1] = (float)PontoCoordenadas[pontoLista].Y;
            Vertex[i + 2] = (float)PontoCoordenadas[pontoLista].Z;
            pontoLista++;
        }

        

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Length * sizeof(float), Vertex, BufferUsageHint.StaticDraw);
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        var positionLocation = Shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
    }

    public override void Renderizar(Transformacao4D matrizGrafo, Camera camera)
    {
        

        GL.BindVertexArray(VertexArrayObject);

        Shader.Use();

        Shader.SetMatrix4("model", Model!.Value);
        Shader.SetMatrix4("view", camera.GetViewMatrix());
        Shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType, 0, PontoCoordenadas.Count);
    }
}