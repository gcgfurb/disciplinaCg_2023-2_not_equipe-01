namespace CG_N2_3;

public abstract class Matematica
{
    public static PontoCoordenada GerarPtosCirculo(double angulo, double raio)
    {
        var pto = new PontoCoordenada(
            raio * Math.Cos(Math.PI * angulo / 180.0),
            raio * Math.Sin(Math.PI * angulo / 180.0));
        return pto;
    }
}