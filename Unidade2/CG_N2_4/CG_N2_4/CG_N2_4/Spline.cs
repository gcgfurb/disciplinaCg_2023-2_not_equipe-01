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

        var novosPontos = CalcularPontosSpline();
        SobreescreverPontos(novosPontos);

        _poliedro.SobreescreverPontos(_pontosControle.Select(x => x.ObterPontoCoordenadaPorIndex(0)).ToList());
        _poliedro.Atualizar();
        Atualizar();
    }

    public void AdicionarPontoSpline()
    {
        _quantidadeInicialPontosSpline += 1;
        var novosPontos = CalcularPontosSpline();
        SobreescreverPontos(novosPontos);
        Atualizar();
    }

    public void RetirarPontoSpline()
    {
        _quantidadeInicialPontosSpline -= 1;
        var novosPontos = CalcularPontosSpline();
        SobreescreverPontos(novosPontos);
        Atualizar();
    }

    private void AdicionarPontosSpline()
    {
        var novosPontos = CalcularPontosSpline();
        foreach (var ponto in novosPontos)
        {
            AdicionarPonto(ponto);
        }
    }

    private List<PontoCoordenada> CalcularPontosSpline()
    {
        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(0);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(0);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(0);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(0);

        var pontosSpline = new List<PontoCoordenada>();

        int numSegments = _quantidadeInicialPontosSpline;
        for (int i = 0; i <= numSegments; i++)
        {
            double t = i / (double)numSegments;
            
            double x = Math.Round(Math.Pow(1 - t, 3) * p0.X + 3 * Math.Pow(1 - t, 2) * t * p1.X +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.X + Math.Pow(t, 3) * p3.X, 2);

            double y = Math.Round(Math.Pow(1 - t, 3) * p0.Y + 3 * Math.Pow(1 - t, 2) * t * p1.Y +
                       3 * (1 - t) * Math.Pow(t, 2) * p2.Y + Math.Pow(t, 3) * p3.Y, 2);

            pontosSpline.Add(new PontoCoordenada(x, y));
        }

        return pontosSpline;
    }


    private void AdicionarPoliedro()
    {
        var pontosCoordenadas = _pontosControle.Select(x =>
            x.ObterPontoCoordenadaPorIndex(0)).ToList();
        _poliedro = new Poligno(pontosCoordenadas);
        AdicionarObjetoFilho(_poliedro);
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