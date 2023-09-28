using OpenTK.Graphics.OpenGL4;

namespace CG_N2_5;

public class Circulo : Objeto
{
    private double _raio;

    public Circulo(PontoCoordenada ponto, double raio = 0.3f) : base(PrimitiveType.LineLoop, 5f)
    {
        _raio = raio;
        for (int angulo = 0; angulo < 360; angulo += 5)
            AdicionarPonto(Matematica.GerarPtosCirculo(angulo, _raio) + ponto);
        Atualizar();
    }

    public new void Atualizar(PontoCoordenada pontoCoordenada)
    {
        int posicao = 0;
        for (int angulo = 0; angulo < 360; angulo += 5)
        {
            AlterarPontoPorIndex(Matematica.GerarPtosCirculo(angulo, _raio) + pontoCoordenada, posicao);
            ++posicao;
        }

        base.Atualizar();
    }
}