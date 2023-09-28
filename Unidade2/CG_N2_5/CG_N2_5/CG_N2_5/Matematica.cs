namespace CG_N2_5;

public static class Matematica
{
    public static PontoCoordenada GerarPtosCirculo(double angulo, double raio) => new()
    {
        X = raio * Math.Cos(Math.PI * angulo / 180.0),
        Y = raio * Math.Sin(Math.PI * angulo / 180.0),
        Z = 0.0
    };
    
    public static bool Dentro(BBox bBox, PontoCoordenada pto) => 
        pto.X >= bBox.MenorX && pto.X <= bBox.MaiorX && pto.Y >= bBox.MenorY && 
        pto.Y <= bBox.MaiorY && pto.Z >= bBox.MenorZ && pto.Z <= bBox.MaiorZ;
    
    public static double DistanciaQuadrado(PontoCoordenada ptoA, PontoCoordenada ptoB) => 
        Math.Pow(ptoB.X - ptoA.X, 2.0) + Math.Pow(ptoB.Y - ptoA.Y, 2.0);
    
    public static double GerarPtosCirculoSimetrico(double raio) => raio * Math.Cos(Math.PI / 4.0);
}