using Libraries.Methods;

namespace Libraries.Fans;

public interface IFanDimensionlessData
{
    //Расчет безразмерных характеристик

    /// <summary>
    /// Окружная скорость по концам лопаток [м/с]
    /// </summary>
    public double DataCircumferentialSpeed =>
        DimensionlessData.CircumferentialSpeed(
            ((IFan)this).ConditionalStandardSize,
            ((IFan)this).Data.ImpellerRotationSpeedWithSlidingEngineForWorkPoint
        );

    /// <summary>
    /// Площадь диска колеса по концам лопаток [м2]
    /// </summary>
    public double DataAreaOfWheelDisc =>
        DimensionlessData.AreaOfWheelDisc(((IFan)this).ConditionalStandardSize);

    /// <summary>
    /// Коэффициент производительности для расхода и давления, введенных пользователем
    /// </summary>
    public double PerformanceCoefficientUserInput =>
        DimensionlessData.PhiCoefficient(
            ((IFan)this).UserInput.UserInputWorkPoint.VolumeFlow
                / ((IFan)this).UserInput.UserInputFan.NumberOfFans,
            ((IFan)this).Data.AreaOfWheelDisc,
            ((IFan)this).Data.CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент полного давления для расхода и давления, введенных пользователем
    /// </summary>
    public double TotalPressureCoefficientUserInput =>
        Calculate.Polynomial(
            ((IFan)this).Data.PsiPhiCoefficients,
            PerformanceCoefficientUserInput
        );

    /// <summary>
    /// Коэффициент статического давления для расхода и давления, введенных пользователем
    /// </summary>
    public double StaticPressureCoefficientUserInput =>
        DimensionlessData.PsiCoefficient(
            ((IFan)this).StaticPressure,
            ((IFan)this).Data.AirDensity,
            ((IFan)this).Data.CircumferentialSpeed
        );

    /// <summary>
    /// Коэффициент потребляемой мощности для расхода и давления, введенных пользователем
    /// </summary>
    public double PowerCoefficientUserInput =>
        DimensionlessData.PowerCoefficient(
            ((IFan)this).Power,
            ((IFan)this).Data.AirDensity,
            ((IFan)this).Data.CircumferentialSpeed,
            ((IFan)this).Data.AreaOfWheelDisc
        );

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSpeedEfficiencyMax =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSpeedPhiCoefficients,
            ((IFan)this).Data.PhiEfficiencyMax
        );

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMin =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSpeedPhiCoefficients,
            ((IFan)this).Data.PhiMin
        );

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSpeedPhiMax =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSpeedPhiCoefficients,
            ((IFan)this).Data.PhiMax
        );

    /// <summary>
    /// Коэффициент быстроходности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSpeedCoefficientWithImpellerRotationSpeed =>
        DimensionlessData.SpeedCoefficient(
            ((IFan)this)
                .Data
                .ImpellerRotationSpeedWithSlidingEngineForWorkPoint,
            ((IFan)this).UserInput.UserInputWorkPoint.VolumeFlow
                / ((IFan)this).UserInput.UserInputFan.NumberOfFans,
            ((IFan)this).InputTotalNormalPressure
        );

    /// <summary>
    /// Коэффициент габаритности при максимальном значении полного КПД
    /// </summary>
    public double SpecificSizeEfficiencyMax =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSizePhiCoefficients,
            ((IFan)this).Data.PhiEfficiencyMax
        );

    /// <summary>
    /// Коэффициент быстроходности при минимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMin =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSizePhiCoefficients,
            ((IFan)this).Data.PhiMin
        );

    /// <summary>
    /// Коэффициент быстроходности при максимальном значении Phi
    /// </summary>
    public double SpecificSizePhiMax =>
        Calculate.Polynomial(
            ((IFan)this).Data.SpecificSizePhiCoefficients,
            ((IFan)this).Data.PhiMax
        );

    /// <summary>
    /// Коэффициент габаритности для расхода и давления, введенных пользователем
    /// </summary>
    public double SpecificSizeCoefficientWithRequiredSize =>
        DimensionlessData.SizeCoefficient(
            ((IFan)this).ConditionalStandardSize,
            ((IFan)this).UserInput.UserInputWorkPoint.VolumeFlow
                / ((IFan)this).UserInput.UserInputFan.NumberOfFans,
            ((IFan)this).InputTotalNormalPressure
        );
}
