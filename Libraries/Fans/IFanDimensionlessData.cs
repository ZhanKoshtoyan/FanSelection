using Libraries.Description_of_objects;
using Libraries.Methods;

namespace Libraries.Fans;

public interface IFanDimensionlessData
{
    public double ImpellerRotationSpeed { get; }

    //Расчет безразмерных характеристик

    /// <summary>
    /// Окружная скорость по концам лопаток [м/с]
    /// </summary>
    public double CircumferentialSpeed =>
        DimensionlessData.CircumferentialSpeed(
            ((IFan)this).Size,
            ImpellerRotationSpeed
        );

    /// <summary>
    /// Площадь диска колеса по концам лопаток [м2]
    /// </summary>
    public double AreaOfWheelDisc =>
        DimensionlessData.AreaOfWheelDisc(((IFan)this).Size);

    /// <summary>
    /// Коэффициент производительности
    /// </summary>
    public double PerformanceCoefficient =>
        DimensionlessData.PerformanceCoefficient(
            ((IFan)this).VolumeFlow,
            AreaOfWheelDisc,
            CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент полного давления
    /// </summary>
    public double TotalPressureCoefficient =>
        DimensionlessData.PressureCoefficient(
            ((IFan)this).TotalPressure,
            FanData.AirInTests,
            CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент статического давления
    /// </summary>
    public double StaticPressureCoefficient =>
        DimensionlessData.PressureCoefficient(
            ((IFan)this).StaticPressure,
            FanData.AirInTests,
            CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент потребляемой мощности
    /// </summary>
    public double PowerCoefficient =>
        DimensionlessData.PowerCoefficient(
            ((IFan)this).Power,
            FanData.AirInTests,
            CircumferentialSpeed,
            AreaOfWheelDisc
        );

    /// <summary>
    /// Коэффициент быстроходности
    /// </summary>
    public double SpeedCoefficient =>
        DimensionlessData.SpeedCoefficient(
            PerformanceCoefficient,
            TotalPressureCoefficient
        );

    /// <summary>
    /// Коэффициент габаритности
    /// </summary>
    public double SizeCoefficient =>
        DimensionlessData.SizeCoefficient(
            PerformanceCoefficient,
            TotalPressureCoefficient
        );
}
