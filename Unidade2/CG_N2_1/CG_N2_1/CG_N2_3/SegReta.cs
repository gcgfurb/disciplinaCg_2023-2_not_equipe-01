using OpenTK.Graphics.OpenGL4;

namespace CG_N2_3;

public class SegReta : Objeto
{
    private PontoCoordenada _ptoPe;
    private PontoCoordenada _ptoCabeca;
    private double _angulo = 45.0;
    private double _raio = 0.5;

    public SegReta(PontoCoordenada ptoPe, PontoCoordenada ptoCabeca) : base(PrimitiveType.Lines)
    {
        _ptoPe = ptoPe;
        _ptoCabeca = ptoCabeca;

        AdicionarPonto(_ptoPe);
        AdicionarPonto(_ptoCabeca);
        Atualizar();
    }

    private new void Atualizar()
    {
        AlterarPontoPorIndex(_ptoPe, 0);
        _ptoCabeca = Matematica.GerarPtosCirculo(_angulo, _raio);
        _ptoCabeca.X += _ptoPe.X;
        AlterarPontoPorIndex(_ptoCabeca, 1);
        base.Atualizar();
    }

    public void AtualizarPe(double peInc)
    {
        _ptoPe = new PontoCoordenada(_ptoPe.X + peInc, _ptoPe.Y);
        Atualizar();
    }

    public void AtualizarRaio(double raioInc)
    {
        _raio += raioInc;
        Atualizar();
    }

    public void AtualizarAngulo(double anguloInc)
    {
        _angulo += anguloInc;
        Atualizar();
    }
}