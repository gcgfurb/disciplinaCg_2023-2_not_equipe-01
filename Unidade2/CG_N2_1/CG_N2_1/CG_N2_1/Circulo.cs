using OpenTK.Graphics.OpenGL4;

namespace CG_N2_1;

public class Circulo
{
    private const int Pontos = 72;
    private const float Raio = 0.5f;

    public void Renderizar()
    {
        GL.PointSize(5f);
        GL.DrawArrays(PrimitiveType.Points, 0, Pontos);  // Desenha os pontos
    }

    public float[] GerarPontosCirculo(float[] vertices)
    {
        vertices = new float[Pontos * 2];
        for (var i = 0; i < Pontos; ++i)
        {
            var angulo = i * 2.0f * (float)Math.PI / Pontos;
            vertices[i * 2] = Raio * (float)Math.Cos(angulo);
            vertices[i * 2 + 1] = Raio * (float)Math.Sin(angulo);
        }

        return vertices;
    }
}