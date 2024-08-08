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
            .InclusiveBetween(
                0,
                Enum.GetValues(typeof(FanVersion.Values)).Length
            )
            .WithMessage(
                $"Исполнение вентилятора должно быть: {string.Join(", ", FanVersion.Names)}."
            );
        RuleFor(input => input.UserInputFan.NumberOfFans)
            .InclusiveBetween(
                NumberOfFans.Values.First(),
                NumberOfFans.Values.Last()
            )
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
            .When(
                input =>
                    input.UserInputAir.RelativeHumidity != 0
                    && input.UserInputAir.RelativeHumidity != null
            )
            .WithMessage(
                "Значение Относительная влажность воздуха должна быть: >= 0 и <= 100  [%]."
            );
        RuleFor(input => input.UserInputFan.NominalPower)
            .Must(input => NominalPowers.Values.Contains(input))
            .When(input => input.UserInputFan.NominalPower != 0)
            .WithMessage(
                $"Номинальная мощность двигателя должна быть: {string.Join("; ", NominalPowers.Names)}  [кВт]"
            );
        RuleFor(
                input =>
                    input
                        .UserInputFan
                        .NominalImpellerRotationSpeedWithoutSlidingEngine
            )
            .Must(input => NominalImpellerRotationSpeeds.Values.Contains(input))
            .When(
                input =>
                    input
                        .UserInputFan
                        .NominalImpellerRotationSpeedWithoutSlidingEngine != 0
            )
            .WithMessage(
                $"Условное число оборотов двигателя должно быть: {string.Join(", ", NominalImpellerRotationSpeeds.Names)} [об/мин]."
            );
        RuleFor(input => input.UserInputFan.FanBodyExecutionMaterial)
            .Must(input => CaseExecutionMaterials.Values.Contains(input))
            .When(
                input =>
                    !string.IsNullOrEmpty(
                        input.UserInputFan.FanBodyExecutionMaterial
                    )
            )
            .WithMessage(
                $"Материал корпуса функциональной сборки должен быть: {string.Join(", ", CaseExecutionMaterials.Names)}."
            );

        // Проверка для OsuDu
        RuleFor(input => input.UserInputFan.FanVersion)
            .InclusiveBetween(
                0,
                Enum.GetValues(typeof(FanVersion.Values)).Length
            )
            .DependentRules(() =>
            {
                // Проверка input.UserInputFan.Size для OsuDu
                RuleFor(input => input.UserInputFan.ConditionalStandardSize)
                    .Must(input => Sizes.ValuesForOsuDu.Contains(input))
                    .When(
                        input => input.UserInputFan.ConditionalStandardSize != 0
                    )
                    .WithMessage(
                        $"Условный типоразмер крыльчатки должен быть: {string.Join(", ", Sizes.NamesForOsuDu)}"
                    );
                // Проверка input.UserInputFan.FanBodyLength для OsuDu
                RuleFor(input => input.UserInputFan.FanBodyLength)
                    .Must(
                        input =>
                            FanBodyLengths.ValuesForOsuDu.Contains(
                                input.ToString()
                            )
                    )
                    .When(input => input.UserInputFan.FanBodyLength != 0)
                    .WithMessage(
                        $"Длина корпуса функциональной сборки должна быть : {string.Join(", ", FanBodyLengths.NamesForOsuDu)}."
                    );
                // Проверка input.UserInputAir.FanOperatingMaxTemperature для OsuDu
                RuleFor(input => input.UserInputAir.FanOperatingMaxTemperature)
                    .Must(
                        input =>
                            FanOperatingMaxTemperatures.ValuesForOsuDu.Contains(
                                input
                            )
                    )
                    .When(
                        input =>
                            input.UserInputAir.FanOperatingMaxTemperature != 0
                    )
                    .WithMessage(
                        $"Температура перемещаемой среды должна быть: {string.Join(", ", FanOperatingMaxTemperatures.NamesForOsuDu)} [°C]."
                    );

                // Проверка input.UserInputFan.ImpellerRotationDirection для OsuDu
                RuleFor(input => input.UserInputFan.ImpellerRotationDirection)
                    .Must(
                        input =>
                            ImpellerRotationDirections.ValuesForOsuDu.Contains(
                                input
                            )
                    )
                    .When(
                        input =>
                            !string.IsNullOrEmpty(
                                input.UserInputFan.ImpellerRotationDirection
                            )
                    )
                    .WithMessage(
                        $"Направление вращения крыльчатки должна быть: {string.Join(", ", ImpellerRotationDirections.NamesForOsuDu)}."
                    );
            })
            .When(input => input.UserInputFan.FanVersion == 0);

        // Проверка для EuFan
        RuleFor(input => input.UserInputFan.FanVersion)
            .InclusiveBetween(
                0,
                Enum.GetValues(typeof(FanVersion.Values)).Length
            )
            .DependentRules(() =>
            {
                // Проверка input.UserInputFan.Size для EuFan
                RuleFor(input => input.UserInputFan.ConditionalStandardSize)
                    .Must(input => Sizes.ValuesForEuFan.Contains(input))
                    .When(
                        input => input.UserInputFan.ConditionalStandardSize != 0
                    )
                    .WithMessage(
                        $"Условный типоразмер крыльчатки должен быть: {string.Join(", ", Sizes.NamesForEuFan)}"
                    );
                // Проверка input.UserInputFan.FanBodyLength для EuFan
                RuleFor(input => input.UserInputFan.FanBodyLength)
                    .Must(
                        input =>
                            FanBodyLengths.ValuesForEuFan.Contains(
                                input.ToString()
                            )
                    )
                    .When(input => input.UserInputFan.FanBodyLength != 0)
                    .WithMessage(
                        $"Длина корпуса функциональной сборки должна быть : {string.Join(", ", FanBodyLengths.NamesForEuFan)}."
                    );

                // Проверка input.UserInputAir.FanOperatingMaxTemperature для EuFan
                RuleFor(input => input.UserInputAir.FanOperatingMaxTemperature)
                    .Must(
                        input =>
                            FanOperatingMaxTemperatures.ValuesForEuFan.Contains(
                                input
                            )
                    )
                    .When(
                        input =>
                            input.UserInputAir.FanOperatingMaxTemperature != 0
                    )
                    .WithMessage(
                        $"Температура перемещаемой среды должна быть: {string.Join(", ", FanOperatingMaxTemperatures.NamesForEuFan)} [°C]."
                    );

                // Проверка input.UserInputFan.ImpellerRotationDirection для EuFan
                RuleFor(input => input.UserInputFan.ImpellerRotationDirection)
                    .Must(
                        input =>
                            ImpellerRotationDirections.ValuesForEuFan.Contains(
                                input
                            )
                    )
                    .When(
                        input =>
                            !string.IsNullOrEmpty(
                                input.UserInputFan.ImpellerRotationDirection
                            )
                    )
                    .WithMessage(
                        $"Направление вращения крыльчатки должна быть: {string.Join(", ", ImpellerRotationDirections.NamesForEuFan)}."
                    );
            })
            .When(input => input.UserInputFan.FanVersion == 1);
    }
}
