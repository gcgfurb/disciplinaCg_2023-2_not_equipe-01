namespace CG_N3;

public class PontoCoordenada
{
    public PontoCoordenada()
    {
    }

    public PontoCoordenada(double x = 0.0, double y = 0.0, double z = 0.0, double w = 1.0)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static PontoCoordenada operator +(PontoCoordenada ponto1, PontoCoordenada ponto2)
        => new(ponto1.X + ponto2.X, ponto1.Y + ponto2.Y, ponto1.Z + ponto2.Z);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double W { get; }
}