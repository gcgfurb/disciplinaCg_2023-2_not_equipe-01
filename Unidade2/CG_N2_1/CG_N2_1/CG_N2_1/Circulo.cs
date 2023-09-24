using OpenTK.Graphics.OpenGL4;

namespace CG_N2_1;

public class Circulo
{
    private readonly int _pontos = 72;
    private readonly float _raio = 0.5f;

    public Circulo()
    {
    }
    
    public void Renderizar()
    {
        GL.PointSize(5f);
        GL.DrawArrays(PrimitiveType.Points, 0, _pontos);  // Desenha os pontos
    }

    public float[] GerarPontosCirculo(float[] vertices)
    {
        vertices = new float[_pontos * 2];
        for (int i = 0; i < _pontos; ++i)
        {
            var angulo = i * 2.0f * (float)Math.PI / _pontos;
            vertices[i * 2] = _raio * (float)Math.Cos(angulo);
            vertices[i * 2 + 1] = _raio * (float)Math.Sin(angulo);
        }

        return vertices;
    }
}