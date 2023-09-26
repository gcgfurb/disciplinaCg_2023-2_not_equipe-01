using CG_N2_4.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N2_4;

public class Poligno : Objeto
{
    public Poligno(List<PontoCoordenada> pontosCoordenadas) : base(PrimitiveType.LineStrip)
    {
        pontosCoordenadas.ForEach(AdicionarPonto);
        AlterarShader(ShaderHelper.Ciano);
        Atualizar();
    }
}