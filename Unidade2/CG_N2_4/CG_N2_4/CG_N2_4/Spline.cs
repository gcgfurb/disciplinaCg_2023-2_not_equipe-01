using CG_N2_4.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

public class Spline : Objeto
{
    private readonly List<Ponto> _pontosControle = new();
    private int _quantidadeInicialPontosSpline = 10;
    private Poligno _poliedro;
    private int _indexPontoSelecionado;

    public Spline() : base(PrimitiveType.LineStrip)
    {
        AdicionarPontosControle();
        AdicionarPoliedro();
        AdicionarPontosSpline();
        Atualizar();
    }
    
    public void AlterarPontoControle()
    {
        _pontosControle[_indexPontoSelecionado].AlterarShader(ShaderHelper.Branca);
        _indexPontoSelecionado = _indexPontoSelecionado == 3 ? 0 : _indexPontoSelecionado += 1;
        _pontosControle[_indexPontoSelecionado].AlterarShader(ShaderHelper.Vermelho);
        Atualizar();
    }

    public void MoverSpline(PontoCoordenada pontoCoordenada)
    {
        _pontosControle[_indexPontoSelecionado].AlterarPontoPorIndex(
            _pontosControle[_indexPontoSelecionado].ObterPontoCoordenadaPorIndex(0) + pontoCoordenada, 0);
        _pontosControle[_indexPontoSelecionado].Atualizar();

        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(0);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(0);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(0);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(0);

        var index = 0;
        for (double t = 0; t <= 1; t += 1.0 / _quantidadeInicialPontosSpline)
        {
            double x = Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X;

            double y = Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y;

            AlterarPontoPorIndex(new PontoCoordenada(x, y), index);

            index++;
        }

        _poliedro.SobreescreverPontos(_pontosControle.Select(x => x.ObterPontoCoordenadaPorIndex(0)).ToList());
        _poliedro.Atualizar();
        Atualizar();
    }

    public void AdicionarPontoSpline()
    {
        _quantidadeInicialPontosSpline += 1;
        
        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(0);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(0);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(0);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(0);

        var novosPontos = new List<PontoCoordenada>();
        
        for (double t = 0; t <= 1; t += 1.0 / _quantidadeInicialPontosSpline)
        {
            double x = Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X;

            double y = Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y;
            
            novosPontos.Add(new PontoCoordenada(x, y));
        }
        
        SobreescreverPontos(novosPontos);
        
        Atualizar();
    }
    
    public void RetirarPontoSpline()
    {
        _quantidadeInicialPontosSpline -= 1;
        
        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(0);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(0);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(0);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(0);

        var novosPontos = new List<PontoCoordenada>();
        
        for (double t = 0; t <= 1; t += 1.0 / _quantidadeInicialPontosSpline)
        {
            double x = Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X;

            double y = Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y;
            
            novosPontos.Add(new PontoCoordenada(x, y));
        }
        
        SobreescreverPontos(novosPontos);
        
        Atualizar();
    }
    
    private void AdicionarPoliedro()
    {
        var pontosCoordenadas = _pontosControle.Select(x =>
            x.ObterPontoCoordenadaPorIndex(0)).ToList();
        _poliedro = new Poligno(pontosCoordenadas);
        AdicionarObjetoFilho(_poliedro);
    }
    
    private void AdicionarPontosSpline()
    {
        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(0);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(0);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(0);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(0);

        for (double t = 0; t <= 1; t += 1.0 / _quantidadeInicialPontosSpline)
        {
            double x = Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X;

            double y = Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y;

            AdicionarPonto(new PontoCoordenada(x, y));
        }
    }

    private void AdicionarPontosControle()
    {
        _pontosControle.Add(new Ponto(new PontoCoordenada(0.5, -0.5)));
        AdicionarObjetoFilho(_pontosControle[0]);
        _pontosControle[0].AlterarShader(ShaderHelper.Vermelho);

        _pontosControle.Add(new Ponto(new PontoCoordenada(0.5, 0.5)));
        AdicionarObjetoFilho(_pontosControle[1]);

        _pontosControle.Add(new Ponto(new PontoCoordenada(-0.5, 0.5)));
        AdicionarObjetoFilho(_pontosControle[2]);

        _pontosControle.Add(new Ponto(new PontoCoordenada(-0.5, -0.5)));
        AdicionarObjetoFilho(_pontosControle[3]);
    }
}