namespace CG_N3.Shaders;

public static class ShaderHelper
{
    private const string Vert = "Shaders/shader.vert";

    public static Shader Vermelho => new(Vert, "Shaders/shaderVermelha.frag");
    public static Shader Verde => new(Vert, "Shaders/shaderVerde.frag");
    public static Shader Azul => new(Vert, "Shaders/shaderAzul.frag");
    public static Shader Branca => new(Vert, "Shaders/shaderBranca.frag");
    public static Shader Ciano => new(Vert, "Shaders/shaderCiano.frag");
    public static Shader Magenta => new(Vert, "Shaders/shaderMagenta.frag");
    public static Shader Amarela => new(Vert, "Shaders/shaderAmarela.frag");
}