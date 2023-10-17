using OpenTK.Graphics.OpenGL4;

namespace CG_N3;

public class Poligno : Objeto
{
    public Poligno(List<PontoCoordenada> pontosCoordenadas, Objeto? objetoPai = null) : base(PrimitiveType.LineLoop, 10f, objetoPai)
    {
        pontosCoordenadas.ForEach(AdicionarPonto);
        Atualizar();
    }
}