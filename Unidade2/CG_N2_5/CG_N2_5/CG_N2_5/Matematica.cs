namespace CG_N2_5;

public static class Matematica
{
    public static PontoCoordenada GerarPtosCirculo(double angulo, double raio) => new()
    {
        X = raio * Math.Cos(Math.PI * angulo / 180.0),
        Y = raio * Math.Sin(Math.PI * angulo / 180.0),
        Z = 0.0
    };
    
    public static double GerarPtosCirculoSimetrico(double raio) => raio * Math.Cos(Math.PI / 4.0);
}