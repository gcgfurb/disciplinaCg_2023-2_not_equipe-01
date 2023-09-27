using CG_N2_5;
using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

public class Ponto : Objeto
{
    public Ponto(PontoCoordenada pontoCoordenada) : base(PrimitiveType.Points)
    {
        AdicionarPonto(pontoCoordenada);
        Atualizar();
    }
}