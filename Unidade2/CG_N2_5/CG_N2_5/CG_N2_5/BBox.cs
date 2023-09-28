namespace CG_N2_5;

public class BBox
{
    public double MenorX;
    public double MenorY;
    public double MenorZ;
    public double MaiorX;
    public double MaiorY;
    public double MaiorZ;
    public PontoCoordenada PontoCoordenadaCentral = new();

    public void Atualizar(List<PontoCoordenada> pontosLista)
    {
        MenorX = pontosLista[0].X;
        MenorY = pontosLista[0].Y;
        MenorZ = pontosLista[0].Z;
        MaiorX = pontosLista[0].X;
        MaiorY = pontosLista[0].Y;
        MaiorZ = pontosLista[0].Z;
        
        for (int index = 1; index < pontosLista.Count; ++index)
            Atualizar(pontosLista[index].X, pontosLista[index].Y, pontosLista[index].Z);
        ProcessarCentro();
    }

    private void Atualizar(double x, double y, double z)
    {
        if (x < MenorX)
            MenorX = x;
        else if (x > MaiorX)
            MaiorX = x;
        if (y < MenorY)
            MenorY = y;
        else if (y > MaiorY)
            MaiorY = y;
        if (z < MenorZ)
        {
            MenorZ = z;
        }
        else
        {
            if (z <= MaiorZ)
                return;
            MaiorZ = z;
        }
    }

    public void ProcessarCentro()
    {
        PontoCoordenadaCentral.X = (MaiorX + MenorX) / 2.0;
        PontoCoordenadaCentral.Y = (MaiorY + MenorY) / 2.0;
        PontoCoordenadaCentral.Z = (MaiorZ + MenorZ) / 2.0;
    }
}