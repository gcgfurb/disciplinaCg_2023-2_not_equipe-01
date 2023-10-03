namespace CG_N3;

public static class Utilitario
{
    public static PontoCoordenada NDC_TelaSRU(int largura, int altura, PontoCoordenada mousePosition)
    {
        var x = 2 * (mousePosition.X / largura) - 1;
        var y = 2 * (-mousePosition.Y / altura) + 1;
        return new PontoCoordenada(x, y);
    }
}