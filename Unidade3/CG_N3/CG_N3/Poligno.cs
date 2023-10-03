using OpenTK.Graphics.OpenGL4;

namespace CG_N3;

public class Poligno : Objeto
{
    public Poligno(List<PontoCoordenada> pontosCoordenadas) : base(PrimitiveType.LineLoop)
    {
        pontosCoordenadas.ForEach(AdicionarPonto);
        Atualizar();
    }
}