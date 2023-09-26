using CG_N2_4.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

public class Spline : Objeto
{
    private readonly List<Ponto> _pontosControle = new();
    private int _quantidadeInicialPontosSpline = 10;
    private Poligno _poliedro;
    private int _indexPontoSelecionado;

    private const int PrimeiroIndice = 0;
    private const int NumeroDePontosControle = 4;

    public Spline() : base(PrimitiveType.LineStrip)
    {
        AdicionarPontosControle();
        AdicionarPoliedro();
        AtualizarSpline();
    }

    // Altera o ponto de controle selecionado
    public void AlterarPontoControle()
    {
        _pontosControle[_indexPontoSelecionado].AlterarShader(ShaderHelper.Branca);
        _indexPontoSelecionado = _indexPontoSelecionado == NumeroDePontosControle - 1
            ? PrimeiroIndice
            : _indexPontoSelecionado + 1;
        _pontosControle[_indexPontoSelecionado].AlterarShader(ShaderHelper.Vermelho);
        AtualizarSpline();
    }

    // Move a spline baseado no ponto coordenada fornecido
    public void MoverSpline(PontoCoordenada pontoCoordenada)
    {
        _pontosControle[_indexPontoSelecionado].AlterarPontoPorIndex(
            _pontosControle[_indexPontoSelecionado].ObterPontoCoordenadaPorIndex(PrimeiroIndice) + pontoCoordenada,
            PrimeiroIndice);
        _pontosControle[_indexPontoSelecionado].Atualizar();

        AtualizarSpline();
    }

    public void AdicionarPontoSpline() => AtualizarQuantidadePontosSpline(1);

    public void RetirarPontoSpline() => AtualizarQuantidadePontosSpline(-1);

    private void AtualizarQuantidadePontosSpline(int alteracao)
    {
        _quantidadeInicialPontosSpline += alteracao;
        AtualizarSpline();
    }

    private void AtualizarSpline()
    {
        var p0 = _pontosControle[0].ObterPontoCoordenadaPorIndex(PrimeiroIndice);
        var p1 = _pontosControle[1].ObterPontoCoordenadaPorIndex(PrimeiroIndice);
        var p2 = _pontosControle[2].ObterPontoCoordenadaPorIndex(PrimeiroIndice);
        var p3 = _pontosControle[3].ObterPontoCoordenadaPorIndex(PrimeiroIndice);

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
        _poliedro.SobreescreverPontos(_pontosControle.Select(x => x.ObterPontoCoordenadaPorIndex(PrimeiroIndice))
            .ToList());
        _poliedro.Atualizar();
        Atualizar();
    }

    // Adiciona um poliedro ao objeto
    private void AdicionarPoliedro()
    {
        var pontosCoordenadas = _pontosControle.Select(x =>
            x.ObterPontoCoordenadaPorIndex(PrimeiroIndice)).ToList();
        _poliedro = new Poligno(pontosCoordenadas);
        AdicionarObjetoFilho(_poliedro);
    }

    // Adiciona pontos de controle ao objeto
    private void AdicionarPontosControle()
    {
        for (int i = 0; i < NumeroDePontosControle; i++)
        {
            var ponto = new Ponto(new PontoCoordenada(0.5 * (i % 2 == 0 ? 1 : -1), 0.5 * (i < 2 ? 1 : -1)));
            _pontosControle.Add(ponto);
            AdicionarObjetoFilho(ponto);
            if (i == PrimeiroIndice) ponto.AlterarShader(ShaderHelper.Vermelho);
        }
    }
}