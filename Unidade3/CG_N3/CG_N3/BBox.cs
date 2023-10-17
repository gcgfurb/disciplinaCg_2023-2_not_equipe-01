using CG_N3.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace CG_N3;

public class BBox
{
    public double MenorX;
    public double MenorY;
    public double MenorZ;
    public double MaiorX;
    public double MaiorY;
    public double MaiorZ;
    public PontoCoordenada PontoCoordenadaCentral = new();
    private int _vertexBufferObjectBbox;
    private int _vertexArrayObjectBbox;
    private Shader _shaderAmarela = ShaderHelper.Amarela;

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

    public void Desenhar(Transformacao4D matrizGrafo)
    {
        var data = new[]
        {
            (float)MenorX,
            (float)MenorY,
            0.0f,
            (float)MaiorX,
            (float)MenorY,
            0.0f,
            (float)MaiorX,
            (float)MaiorY,
            0.0f,
            (float)MenorX,
            (float)MaiorY,
            0.0f,
            (float)PontoCoordenadaCentral.X,
            (float)PontoCoordenadaCentral.Y,
            0.0f
        };
        _vertexBufferObjectBbox = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectBbox);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
        _vertexArrayObjectBbox = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObjectBbox);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, 0);
        GL.EnableVertexAttribArray(0);
        GL.BindVertexArray(_vertexArrayObjectBbox);
        _shaderAmarela.SetMatrix4("transform", matrizGrafo.ObterDadosOpenTk());
        _shaderAmarela.Use();
        GL.DrawArrays(PrimitiveType.LineLoop, 0, (data.Length - 1) / 3);
        GL.PointSize(20f);
        GL.DrawArrays(PrimitiveType.Points, (data.Length - 1) / 3, 1);
    }
}