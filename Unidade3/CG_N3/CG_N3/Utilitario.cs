namespace CG_N3;

public static class Utilitario
{
    public static PontoCoordenada NDC_TelaSRU(int largura, int altura, PontoCoordenada mousePosition)
    {
        var x = 2.0 * (mousePosition.X / largura) - 1.0;
        var y = 2.0 * (-mousePosition.Y / altura) + 1.0;
        return new PontoCoordenada(x, y);
    }
}