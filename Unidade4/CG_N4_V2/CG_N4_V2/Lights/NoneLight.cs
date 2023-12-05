using CG_N4_V2.Common;
using OpenTK.Graphics.OpenGL4;

namespace CG_N4_V2.Lights;

public class NoneLight : Cubo
{
    private readonly float[] _vertices =
    {
        // Positions         Texture coords
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,

        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,

        -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

        0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f
    };

    private Texture _diffuseMap;
    private Texture _specularMap;

    public NoneLight()
    {
        Shader = new Shader("Shaders/None/shader.vert", "Shaders/None/shader.frag");

        for (var i = 0; i < _vertices.Length; i += 5)
        {
            AdicionarPonto(new PontoCoordenada(
                _vertices[i],
                _vertices[i + 1],
                _vertices[i + 2],
                textureX: _vertices[i + 3],
                textureY: _vertices[i + 4]
            ));
        }

        _diffuseMap = Texture.LoadFromFile("Resources/image.png");
        _specularMap = Texture.LoadFromFile("Resources/container2_specular.png");

        Atualizar();
    }

    public sealed override void Atualizar()
    {
        Vertex = new float[PontoCoordenadas.Count * 5];

        var pontoLista = 0;
        for (var i = 0; i < Vertex.Length; i += 5)
        {
            Vertex[i] = (float)PontoCoordenadas[pontoLista].X;
            Vertex[i + 1] = (float)PontoCoordenadas[pontoLista].Y;
            Vertex[i + 2] = (float)PontoCoordenadas[pontoLista].Z;

            Vertex[i + 3] = (float)PontoCoordenadas[pontoLista].TextureX;
            Vertex[i + 4] = (float)PontoCoordenadas[pontoLista].TextureY;
            pontoLista++;
        }

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Length * sizeof(float), Vertex, BufferUsageHint.StaticDraw);
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        var positionLocation = Shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = Shader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
            3 * sizeof(float));
    }

    public override void Renderizar(Transformacao4D matrizGrafo, Camera camera)
    {
        GL.BindVertexArray(VertexArrayObject);

        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);

        Shader.SetInt("texture0", 0);
        Shader.SetInt("texture1", 1);
        Shader.Use();

        matrizGrafo = matrizGrafo.MultiplicarMatriz(Matriz);

        Shader.SetMatrix4("model", matrizGrafo.ObterDadosOpenTk());
        Shader.SetMatrix4("view", camera.GetViewMatrix());
        Shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType, 0, PontoCoordenadas.Count);
    }
}