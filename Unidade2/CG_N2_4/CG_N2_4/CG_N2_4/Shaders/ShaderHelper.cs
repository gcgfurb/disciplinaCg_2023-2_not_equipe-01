namespace CG_N2_4.Shaders;

public static class ShaderHelper
{
    private const string Vert = "Shaders/shader.vert";

    public static Shader Vermelho => new Shader(Vert, "Shaders/shaderVermelha.frag");
    public static Shader Branca => new(Vert, "Shaders/shaderBranca.frag");
    public static Shader Ciano => new(Vert, "Shaders/shaderCiano.frag");
}