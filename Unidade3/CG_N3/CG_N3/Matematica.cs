namespace CG_N3;

public static class Matematica
{
    public static PontoCoordenada GerarPtosCirculo(double angulo, double raio) => new()
    {
        X = raio * Math.Cos(Math.PI * angulo / 180.0),
        Y = raio * Math.Sin(Math.PI * angulo / 180.0),
        Z = 0.0
    };
    
    public static bool Dentro(this BBox bBox, PontoCoordenada pto) => 
        pto.X >= bBox.MenorX && pto.X <= bBox.MaiorX && pto.Y >= bBox.MenorY && 
        pto.Y <= bBox.MaiorY && pto.Z >= bBox.MenorZ && pto.Z <= bBox.MaiorZ;
    
    public static double DistanciaQuadrado(PontoCoordenada ptoA, PontoCoordenada ptoB) => 
        Math.Pow(ptoB.X - ptoA.X, 2.0) + Math.Pow(ptoB.Y - ptoA.Y, 2.0);
    
    public static double GerarPtosCirculoSimetrico(double raio) => raio * Math.Cos(Math.PI / 4.0);
    
    public static bool ScanLine(PontoCoordenada ptoClique, PontoCoordenada ptoIni, PontoCoordenada ptoFim)
    {
        var ti = ScanLineInterseccao(ptoClique.Y, ptoIni.Y, ptoFim.Y);
        return ti is >= 0.0 and <= 1.0 && ScanLineCalculaXi(ptoIni.X, ptoFim.X, ti) > ptoClique.X;
    }
    
    private static double ScanLineInterseccao(double yi, double y1, double y2) => (yi - y1) / (y2 - y1);
    private static double ScanLineCalculaXi(double x1, double x2, double ti) => x1 + (x2 - x1) * ti;
}