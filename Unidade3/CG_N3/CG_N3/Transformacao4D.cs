using OpenTK.Mathematics;

namespace CG_N3;

public class Transformacao4D
{
    public const double DegToRad = Math.PI / 180.0;

    private readonly double[] _matriz =
    {
        1.0, 0.0, 0.0, 0.0,
        0.0, 1.0, 0.0, 0.0,
        0.0, 0.0, 1.0, 0.0,
        0.0, 0.0, 0.0, 1.0
    };

    public void AtribuirTranslacao(double tx, double ty, double tz)
    {
        AtribuirIdentidade();
        _matriz[12] = tx;
        _matriz[13] = ty;
        _matriz[14] = tz;
    }

    public void AtribuirEscala(double sX, double sY, double sZ)
    {
        AtribuirIdentidade();
        _matriz[0] = sX;
        _matriz[5] = sY;
        _matriz[10] = sZ;
    }

    public void AtribuirRotacaoX(double radians)
    {
        AtribuirIdentidade();
        _matriz[5] = Math.Cos(radians);
        _matriz[9] = -Math.Sin(radians);
        _matriz[6] = Math.Sin(radians);
        _matriz[10] = Math.Cos(radians);
    }

    public void AtribuirRotacaoY(double radians)
    {
        AtribuirIdentidade();
        _matriz[0] = Math.Cos(radians);
        _matriz[8] = Math.Sin(radians);
        _matriz[2] = -Math.Sin(radians);
        _matriz[10] = Math.Cos(radians);
    }

    public void AtribuirRotacaoZ(double radians)
    {
        AtribuirIdentidade();
        _matriz[0] = Math.Cos(radians);
        _matriz[4] = -Math.Sin(radians);
        _matriz[1] = Math.Sin(radians);
        _matriz[5] = Math.Cos(radians);
    }

    public Matrix4 ObterDadosOpenTk() => Matrix4.Identity with
    {
        M11 = (float)_matriz[0],
        M21 = (float)_matriz[4],
        M31 = (float)_matriz[8],
        M41 = (float)_matriz[12],
        M12 = (float)_matriz[1],
        M22 = (float)_matriz[5],
        M32 = (float)_matriz[9],
        M42 = (float)_matriz[13],
        M13 = (float)_matriz[2],
        M23 = (float)_matriz[6],
        M33 = (float)_matriz[10],
        M43 = (float)_matriz[14],
        M14 = (float)_matriz[3],
        M24 = (float)_matriz[7],
        M34 = (float)_matriz[11],
        M44 = (float)_matriz[15]
    };

    public PontoCoordenada MultiplicarPonto(PontoCoordenada pto) => new(
        _matriz[0] * pto.X + _matriz[4] * pto.Y + _matriz[8] * pto.Z + _matriz[12] * pto.W,
        _matriz[1] * pto.X + _matriz[5] * pto.Y + _matriz[9] * pto.Z + _matriz[13] * pto.W,
        _matriz[2] * pto.X + _matriz[6] * pto.Y + _matriz[10] * pto.Z + _matriz[14] * pto.W,
        _matriz[3] * pto.X + _matriz[7] * pto.Y + _matriz[11] * pto.Z + _matriz[15] * pto.W);

    public Transformacao4D MultiplicarMatriz(Transformacao4D t)
    {
        var transformacao4D = new Transformacao4D();
        for (var index = 0; index < 16; ++index)
            transformacao4D._matriz[index] = _matriz[index % 4] * t._matriz[index / 4 * 4] +
                                             _matriz[index % 4 + 4] * t._matriz[index / 4 * 4 + 1] +
                                             _matriz[index % 4 + 8] * t._matriz[index / 4 * 4 + 2] +
                                             _matriz[index % 4 + 12] * t._matriz[index / 4 * 4 + 3];
        return transformacao4D;
    }

    public void AtribuirIdentidade()
    {
        for (var index = 0; index < 16; ++index)
            _matriz[index] = 0.0;
        _matriz[0] = _matriz[5] = _matriz[10] = _matriz[15] = 1.0;
    }
}