using Libraries.Methods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp1.ViewModels;

public class OutputValueConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is double doubleValue)
        {
            if (parameter != null)
            {
                if (parameter.ToString() == "Size")
                {
                    var formattedValue = Math.Round(
                        doubleValue * 100,
                        0,
                        MidpointRounding.ToNegativeInfinity
                    );
                    return formattedValue.ToString("000");
                }

                if (parameter.ToString()!.Contains("Power"))
                {
                    var formattedValue = Math.Round(
                        doubleValue,
                        2,
                        MidpointRounding.ToNegativeInfinity
                    );
                    return formattedValue;
                }

                if (parameter.ToString()!.Contains("Deviation"))
                {
                    var formattedValue = Math.Round(
                        doubleValue,
                        2,
                        MidpointRounding.ToNegativeInfinity
                    );
                    return formattedValue;
                }

                if (
                    parameter.ToString() == "TotalEfficiency"
                    || parameter.ToString() == "AirVelocity"
                    || parameter.ToString() == "Weight"
                )
                {
                    var formattedValue = Math.Round(
                        doubleValue,
                        1,
                        MidpointRounding.ToNegativeInfinity
                    );
                    return formattedValue;
                }

                if (parameter.ToString()!.Contains("SumNoise"))
                {
                    var formattedSumNoise = Math.Round(
                        doubleValue,
                        1,
                        MidpointRounding.ToNegativeInfinity
                    );

                    return formattedSumNoise;
                }
            }

            var formattedValueDefault = Math.Round(
                doubleValue,
                0,
                MidpointRounding.ToNegativeInfinity
            );
            return formattedValueDefault;
        }

        if (
            parameter != null
            && value is List<ValueTuple<int, double>> data
            && parameter.ToString()!.Contains("ListNoise")
        )
        {
            return Calculate.GetOctaveNoiseAString(data);
        }

        return value;
    }

    /*public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture
    ) => throw new NotImplementedException();*/
}
