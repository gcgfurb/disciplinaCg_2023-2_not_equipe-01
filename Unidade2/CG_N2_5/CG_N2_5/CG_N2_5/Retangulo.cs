using OpenTK.Graphics.OpenGL4;

namespace CG_N2_5;

public class Retangulo : Objeto
{
    public Retangulo(PontoCoordenada pontoInferiorEsquerdo, PontoCoordenada pontoSuperiorDireito) : base(PrimitiveType.LineLoop)
    {
        AdicionarPonto(pontoInferiorEsquerdo);
        AdicionarPonto(new PontoCoordenada(pontoSuperiorDireito.X, pontoInferiorEsquerdo.Y));
        AdicionarPonto(pontoSuperiorDireito);
        AdicionarPonto(new PontoCoordenada(pontoInferiorEsquerdo.X, pontoSuperiorDireito.Y));
        Atualizar();
    }
}