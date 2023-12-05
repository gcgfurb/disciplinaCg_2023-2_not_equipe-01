using CG_N4_V2.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_N4_V2.Lights;

public class PointLights : Cubo
{
    private readonly float[] _vertices =
    {
        // Positions          Normals              Texture coords
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,

        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f
    };

    private Texture _diffuseMap;
    private Texture _specularMap;

    public PointLights()
    {
        Shader = new Shader("Shaders/PointLights/shader.vert", "Shaders/PointLights/lighting.frag");

        for (var i = 0; i < _vertices.Length; i += 8)
        {
            AdicionarPonto(new PontoCoordenada(
                _vertices[i],
                _vertices[i + 1],
                _vertices[i + 2],
                _vertices[i + 3],
                _vertices[i + 4],
                _vertices[i + 5],
                _vertices[i + 6],
                _vertices[i + 7]
            ));
        }

        _diffuseMap = Texture.LoadFromFile("Resources/container2.png");
        _specularMap = Texture.LoadFromFile("Resources/container2_specular.png");

        Atualizar();
    }

    public sealed override void Atualizar()
    {
        Vertex = new float[PontoCoordenadas.Count * 8];

        var pontoLista = 0;
        for (var i = 0; i < Vertex.Length; i += 8)
        {
            Vertex[i] = (float)PontoCoordenadas[pontoLista].X;
            Vertex[i + 1] = (float)PontoCoordenadas[pontoLista].Y;
            Vertex[i + 2] = (float)PontoCoordenadas[pontoLista].Z;

            Vertex[i + 3] = (float)PontoCoordenadas[pontoLista].NormalX;
            Vertex[i + 4] = (float)PontoCoordenadas[pontoLista].NormalY;
            Vertex[i + 5] = (float)PontoCoordenadas[pontoLista].NormalZ;

            Vertex[i + 6] = (float)PontoCoordenadas[pontoLista].TextureX;
            Vertex[i + 7] = (float)PontoCoordenadas[pontoLista].TextureY;
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
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = Shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            3 * sizeof(float));

        var texCoordLocation = Shader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            6 * sizeof(float));
    }

    public override void Renderizar(Transformacao4D matrizGrafo, Camera camera)
    {
        GL.PointSize(PrimitiveSize);

        GL.BindVertexArray(VertexArrayObject);

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
        Shader.Use();

        matrizGrafo = matrizGrafo.MultiplicarMatriz(Matriz);

        Shader.SetMatrix4("model", matrizGrafo.ObterDadosOpenTk());
        Shader.SetMatrix4("view", camera.GetViewMatrix());
        Shader.SetMatrix4("projection", camera.GetProjectionMatrix());
        Shader.SetVector3("viewPos", camera.Position);

        Shader.SetInt("material.diffuse", 0);
        Shader.SetInt("material.specular", 1);
        Shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        Shader.SetFloat("material.shininess", 32.0f);

        Shader.SetVector3("light.position", new Vector3(1.2f, 1.0f, 2.0f));
        Shader.SetFloat("light.constant", 1.0f);
        Shader.SetFloat("light.linear", 0.09f);
        Shader.SetFloat("light.quadratic", 0.032f);
        Shader.SetVector3("light.ambient", new Vector3(0.2f));
        Shader.SetVector3("light.diffuse", new Vector3(0.5f));
        Shader.SetVector3("light.specular", new Vector3(1.0f));

        GL.DrawArrays(PrimitiveType, 0, PontoCoordenadas.Count);
    }
}