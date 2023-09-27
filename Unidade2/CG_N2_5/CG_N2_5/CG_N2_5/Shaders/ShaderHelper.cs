namespace CG_N2_5.Shaders;

public static class ShaderHelper
{
    private const string Vert = "Shaders/shader.vert";

    public static Shader Vermelho => new Shader(Vert, "Shaders/shaderVermelha.frag");
    public static Shader Verde => new Shader(Vert, "Shaders/shaderVerde.frag");
    public static Shader Azul => new Shader(Vert, "Shaders/shaderAzul.frag");
    public static Shader Branca => new(Vert, "Shaders/shaderBranca.frag");
    public static Shader Ciano => new(Vert, "Shaders/shaderCiano.frag");
}