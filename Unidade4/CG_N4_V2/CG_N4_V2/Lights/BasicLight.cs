using CG_N4_V2.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_N4_V2.Lights;

public class BasicLight : Cubo
{
    private readonly float[] _vertices =
    {
        // Position          Normal
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, // Front face
        0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,

        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, // Back face
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,

        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, // Left face
        -0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f,

        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, // Right face
        0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f,

        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, // Bottom face
        0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,

        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // Top face
        0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f
    };

    public BasicLight()
    {
        Shader = new Shader("Shaders/BasicLighting/shader.vert", "Shaders/BasicLighting/lighting.frag");

        for (var i = 0; i < _vertices.Length; i += 6)
        {
            AdicionarPonto(new PontoCoordenada(
                _vertices[i],
                _vertices[i + 1],
                _vertices[i + 2],
                _vertices[i + 3],
                _vertices[i + 4],
                _vertices[i + 5]));
        }
        
        Atualizar();
    }
    
    public sealed override void Atualizar()
    {
        Vertex = new float[PontoCoordenadas.Count * 6];

        var pontoLista = 0;
        for (var i = 0; i < Vertex.Length; i += 6)
        {
            Vertex[i] = (float)PontoCoordenadas[pontoLista].X;
            Vertex[i + 1] = (float)PontoCoordenadas[pontoLista].Y;
            Vertex[i + 2] = (float)PontoCoordenadas[pontoLista].Z;

            Vertex[i + 3] = (float)PontoCoordenadas[pontoLista].NormalX;
            Vertex[i + 4] = (float)PontoCoordenadas[pontoLista].NormalY;
            Vertex[i + 5] = (float)PontoCoordenadas[pontoLista].NormalZ;
            pontoLista++;
        }

        GL.PointSize(PrimitiveSize);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Length * sizeof(float), Vertex, BufferUsageHint.StaticDraw);
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        var positionLocation = Shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

        var normalLocation = Shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float),
            3 * sizeof(float));
    }

    public override void Renderizar(Transformacao4D matrizGrafo, Camera camera)
    {
        GL.PointSize(PrimitiveSize);

        GL.BindVertexArray(VertexArrayObject);

        Shader.Use();

        matrizGrafo = matrizGrafo.MultiplicarMatriz(Matriz);

        Shader.SetMatrix4("model", matrizGrafo.ObterDadosOpenTk());
        Shader.SetMatrix4("view", camera.GetViewMatrix());
        Shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        Shader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
        Shader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        Shader.SetVector3("lightPos", new Vector3(1.2f, 1.0f, 2.0f));
        Shader.SetVector3("viewPos", camera.Position);

        GL.DrawArrays(PrimitiveType, 0, PontoCoordenadas.Count);
    }
}