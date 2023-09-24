using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

public class Spline : Objeto
{
    private readonly List<Ponto> _pontosControle = new();
    private readonly int _quantidadeInicialPontosSpline = 10;
    
    public Spline() : base(PrimitiveType.LineStrip)
    {
        AdicionarPontosControle();
        AdicionarPontosSpline();
    }

    private void AdicionarPontosSpline()
    {
        for (int i = 0; i < _quantidadeInicialPontosSpline; i++)
        {
            AdicionarPonto(new PontoCoordenada(0.0, 0.0));
        }
        
        Atualizar();
    }

    private void AdicionarPontosControle()
    {
        _pontosControle.Add(new Ponto(new PontoCoordenada(0.5, -0.5)));
        AdicionarObjetoFilho(_pontosControle[0]);
        _pontosControle[0].Atualizar();
        
        _pontosControle.Add(new Ponto(new PontoCoordenada(0.5, 0.5)));
        AdicionarObjetoFilho(_pontosControle[1]);
        _pontosControle[1].Atualizar();
        
        _pontosControle.Add(new Ponto(new PontoCoordenada(-0.5, 0.5)));
        AdicionarObjetoFilho(_pontosControle[2]);
        _pontosControle[2].Atualizar();
        
        _pontosControle.Add(new Ponto(new PontoCoordenada(-0.5, -0.5)));
        AdicionarObjetoFilho(_pontosControle[3]);
        _pontosControle[3].Atualizar();
    }
}