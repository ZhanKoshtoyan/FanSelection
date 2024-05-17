using FluentValidation;
using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;

namespace Libraries.ValidateAndSort;

public class UserInputValidator : AbstractValidator<UserInput>
{
    public UserInputValidator()
    {
        RuleFor(input => input.UserInputWorkPoint.VolumeFlow)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Объемный расход воздуха должен быть >= 0 [м3/ч].");
        RuleFor(input => input.UserInputWorkPoint.TotalPressure)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Полное давление воздуха должно быть >= 0 [Па].");
        RuleFor(input => input.UserInputWorkPoint.TotalPressureDeviation)
            .InclusiveBetween(0, 30)
            .When(input => input is not null)
            .WithMessage(
                "Допустимая погрешность подбора по полному давлению воздуха должна быть: >= 0 и <= 30  [%]."
            );
        RuleFor(input => input.UserInputFan.FanVersion)
            .InclusiveBetween(0, Enum.GetValues(typeof(FanVersion.Values)).Length)
            .WithMessage(
                $"Исполнение вентилятора должно быть: {string.Join(", ", FanVersion.Names)}."
            );
        RuleFor(input => input.UserInputFan.NumberOfFans)
            .InclusiveBetween(NumberOfFans.Values.First(), NumberOfFans.Values.Last())
            .When(input => input.UserInputFan.NumberOfFans != 0)
            .WithMessage(
                $"Количество вентиляторов должно быть: {string.Join(", ", NumberOfFans.Values)}."
            );
        RuleFor(input => input.UserInputFan.FanLogic)
            .InclusiveBetween(0, Enum.GetValues(typeof(FanLogic.Values)).Length)
            .WithMessage(
                $"Исполнение вентилятора должно быть: {string.Join(", ", FanLogic.Names)}."
            );
        RuleFor(input => input.UserInputAir.RelativeHumidity)
            .InclusiveBetween(0, 100)
            .When(input => input.UserInputAir.RelativeHumidity != 0 && input.UserInputAir.RelativeHumidity != null)
            .WithMessage(
                "Значение Относительная влажность воздуха должна быть: >= 0 и <= 100  [%]."
            );
        RuleFor(input => input.UserInputFan.Size)
            .Must(input => Sizes.Values.Contains(input))
            .When(input => input.UserInputFan.Size != 0)
            .WithMessage(
                $"Условный типоразмер крыльчатки должен быть: {string.Join(", ", Sizes.Names)}"
            );
        RuleFor(input => input.UserInputFan.FanBodyLength)
            .Must(input => FanBodyLengths.Values.Contains(input.ToString()))
            .When(input => input.UserInputFan.FanBodyLength != 0)
            .WithMessage(
                $"Длина корпуса функциональной сборки должна быть : {string.Join(", ", FanBodyLengths.Names)}."
            );
        RuleFor(input => input.UserInputAir.FanOperatingMaxTemperature)
            .Must(input => FanOperatingMaxTemperatures.Values.Contains(input))
            .When(input => input.UserInputAir.FanOperatingMaxTemperature != 0)
            .WithMessage(
                $"Температура перемещаемой среды должна быть: {string.Join(", ", FanOperatingMaxTemperatures.Names)} [°C]."
            );
        RuleFor(input => input.UserInputFan.ImpellerRotationDirection)
            .Must(input => ImpellerRotationDirections.Values.Contains(input))
            .When(
                input => !string.IsNullOrEmpty(input.UserInputFan.ImpellerRotationDirection)
            )
            .WithMessage(
                $"Направление вращения крыльчатки должна быть: {string.Join(", ", ImpellerRotationDirections.Names)}."
            );
        RuleFor(input => input.UserInputFan.NominalPower)
            .Must(input => NominalPowers.Values.Contains(input))
            .When(input => input.UserInputFan.NominalPower != 0)
            .WithMessage(
                $"Номинальная мощность двигателя должна быть: {string.Join("; ", NominalPowers.Names)}  [кВт]"
            );
        RuleFor(input => input.UserInputFan.NominalImpellerRotationSpeed)
            .Must(input => NominalImpellerRotationSpeeds.Values.Contains(input))
            .When(input => input.UserInputFan.NominalImpellerRotationSpeed != 0)
            .WithMessage(
                $"Условное число оборотов двигателя должно быть: {string.Join(", ", NominalImpellerRotationSpeeds.Names)} [об/мин]."
            );
        RuleFor(input => input.UserInputFan.CaseExecutionMaterial)
            .Must(input => CaseExecutionMaterials.Values.Contains(input))
            .When(input => !string.IsNullOrEmpty(input.UserInputFan.CaseExecutionMaterial))
            .WithMessage(
                $"Материал корпуса функциональной сборки должен быть: {string.Join(", ", CaseExecutionMaterials.Names)}."
            );
        RuleFor(input => input.UserInputFan.RequiredSize)
            .Must(input => Sizes.Values.Contains(input))
            .When(input => input.UserInputFan.RequiredSize != 0)
            .WithMessage(
                $"Размер крыльчатки должен быть: {string.Join(", ", Sizes.Names)}"
            );
    }
}
