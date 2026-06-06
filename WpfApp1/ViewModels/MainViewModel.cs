using FluentValidation;
using Libraries.DescriptionOfObjects.Parameters;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Loader;
using Libraries.Methods;
using Libraries.StructureOfObjects;
using Libraries.ValidateAndSort;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Models;
using AbstractFan = Libraries.Fans.AbstractFan;

namespace WpfApp1.ViewModels;

public class MainViewModel : BaseViewModel
{
    private const string IdProjectDefaultValue = "xx";

    #region PropertyContainers
    #region FanVersionList
    private readonly List<FanVersionItem> _fanVersionList = null!;
    public List<FanVersionItem> FanVersionList
    {
        get => _fanVersionList;
        private init
        {
            _fanVersionList = value;
            OnPropertyChanged();
        }
    }
    #endregion FanVersionList

    #region FanOperatingMaxTemperatureList
    private List<string> _fanOperatingMaxTemperatureList = null!;

    public List<string> FanOperatingMaxTemperatureList
    {
        get => _fanOperatingMaxTemperatureList;
        private set
        {
            _fanOperatingMaxTemperatureList = value;
            OnPropertyChanged();
        }
    }
    #endregion FanOperatingMaxTemperatureList

    #region FanSizeList
    private List<string> _fanSizeList = null!;

    public List<string> FanSizeList
    {
        get => _fanSizeList;
        private set
        {
            _fanSizeList = value;
            OnPropertyChanged();
        }
    }
    #endregion FanSizeList

    #region FanBodyLengthList
    private List<string> _fanBodyLengthList = null!;

    public List<string> FanBodyLengthList
    {
        get => _fanBodyLengthList;
        private set
        {
            _fanBodyLengthList = value;
            OnPropertyChanged();
        }
    }
    #endregion FanBodyLengthList

    #region ImpellerRotationDirectionList
    private List<string> _impellerRotationDirectionList = null!;

    public List<string> ImpellerRotationDirectionList
    {
        get => _impellerRotationDirectionList;
        private set
        {
            _impellerRotationDirectionList = value;
            OnPropertyChanged();
        }
    }
    #endregion ImpellerRotationDirectionList

    #region NominalPowerList
    private List<string> _nominalPowerList = null!;

    public List<string> NominalPowerList
    {
        get => _nominalPowerList;
        private set
        {
            _nominalPowerList = value;
            OnPropertyChanged();
        }
    }
    #endregion NominalPowerList

    #region NominalImpellerRotationSpeedList
    private List<string> _nominalImpellerRotationSpeedList = null!;

    public List<string> NominalImpellerRotationSpeedList
    {
        get => _nominalImpellerRotationSpeedList;
        private set
        {
            _nominalImpellerRotationSpeedList = value;
            OnPropertyChanged();
        }
    }
    #endregion NominalImpellerRotationSpeedList

    #region FanBodyExecutionMaterialList
    private List<string> _fanBodyExecutionMaterialList = null!;

    public List<string> FanBodyExecutionMaterialList
    {
        get => _fanBodyExecutionMaterialList;
        private set
        {
            _fanBodyExecutionMaterialList = value;
            OnPropertyChanged();
        }
    }
    #endregion FanBodyExecutionMaterialList

    #region FanLogicList
    private readonly string[] _fanLogicList = null!;
    public string[] FanLogicList
    {
        get => _fanLogicList;
        private init
        {
            _fanLogicList = value;
            OnPropertyChanged();
        }
    }
    #endregion fanLogicList

    #region NumberOfFansList
    private readonly List<string> _numberOfFansList = null!;
    public List<string> NumberOfFansList
    {
        get => _numberOfFansList;
        private init
        {
            _numberOfFansList = value;
            OnPropertyChanged();
        }
    }
    #endregion NumberOfFansList

    #region ProjectIdText
    private string _projectIdText = null!;
    public string ProjectIdText
    {
        get => _projectIdText;
        private set
        {
            _projectIdText = value;
            OnPropertyChanged();
        }
    }
    #endregion ProjectIdText

    #region AltitudeText
    private string _altitudeText = null!;
    public string AltitudeText
    {
        get => _altitudeText;
        set
        {
            _altitudeText = value;
            OnPropertyChanged();
        }
    }
    #endregion AltitudeText

    #region AltitudeTextWithUnit
    private string _altitudeTextWithUnit = null!;
    public string AltitudeTextWithUnit
    {
        get => _altitudeTextWithUnit;
        private set
        {
            _altitudeTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion AltitudeTextWithUnit

    #region VolumeFlowText
    private string _volumeFlowText = null!;
    public string VolumeFlowText
    {
        get => _volumeFlowText;
        set
        {
            if (SetField(ref _volumeFlowText, value))
                _selectFanCommand.RaiseCanExecuteChanged();
        }
    }
    #endregion VolumeFlowText

    #region TotalPressureText
    private string _totalPressureText = null!;
    public string TotalPressureText
    {
        get => _totalPressureText;
        set
        {
            if (SetField(ref _totalPressureText, value))
                _selectFanCommand.RaiseCanExecuteChanged();
        }
    }
    #endregion TotalPressureText

    #region TotalPressureDeviationText
    private double _totalPressureDeviationText;
    public double TotalPressureDeviationText
    {
        get => _totalPressureDeviationText;
        set
        {
            _totalPressureDeviationText = value;
            OnPropertyChanged();
        }
    }
    #endregion TotalPressureDeviationText

    #region TotalPressureDeviationTextWithUnit
    private readonly string _totalPressureDeviationTextWithUnit = null!;

    public string TotalPressureDeviationTextWithUnit
    {
        get => _totalPressureDeviationTextWithUnit;
        private init
        {
            _totalPressureDeviationTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion TotalPressureDeviationTextWithUnit

    #region SpecificDeviationLeftText
    private double _specificDeviationLeftText;
    public double SpecificDeviationLeftText
    {
        get => _specificDeviationLeftText;
        set
        {
            _specificDeviationLeftText = value;
            OnPropertyChanged();
        }
    }
    #endregion SpecificDeviationLeftText

    #region SpecificDeviationLeftTextWithUnit
    private readonly string _specificDeviationLeftTextWithUnit = null!;
    public string SpecificDeviationLeftTextWithUnit
    {
        get => _specificDeviationLeftTextWithUnit;
        private init
        {
            _specificDeviationLeftTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion SpecificDeviationLeftTextWithUnit

    #region SpecificDeviationRightText
    private double _specificDeviationRightText;
    public double SpecificDeviationRightText
    {
        get => _specificDeviationRightText;
        set
        {
            _specificDeviationRightText = value;
            OnPropertyChanged();
        }
    }
    #endregion SpecificDeviationRightText

    #region SpecificDeviationRightTextWithUnit
    private readonly string _specificDeviationRightTextWithUnit = null!;
    public string SpecificDeviationRightTextWithUnit
    {
        get => _specificDeviationRightTextWithUnit;
        private init
        {
            _specificDeviationRightTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion SpecificDeviationRightTextWithUnit

    #region FanOperatingCurrentTemperatureText
    private double _fanOperatingCurrentTemperatureText;
    public double FanOperatingCurrentTemperatureText
    {
        get => _fanOperatingCurrentTemperatureText;
        set
        {
            _fanOperatingCurrentTemperatureText = value;
            OnPropertyChanged();
        }
    }
    #endregion FanOperatingCurrentTemperatureText

    #region FanOperatingCurrentTemperatureTextWithUnit
    private readonly string _fanOperatingCurrentTemperatureTextWithUnit = null!;
    public string FanOperatingCurrentTemperatureTextWithUnit
    {
        get => _fanOperatingCurrentTemperatureTextWithUnit;
        private init
        {
            _fanOperatingCurrentTemperatureTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion FanOperatingCurrentTemperatureTextWithUnit

    #region RelativeHumidityText
    private double _relativeHumidityText;
    public double RelativeHumidityText
    {
        get => _relativeHumidityText;
        set
        {
            _relativeHumidityText = value;
            OnPropertyChanged();
        }
    }
    #endregion RelativeHumidityText

    #region RelativeHumidityTextWithUnit
    private readonly string _relativeHumidityTextWithUnit = null!;
    public string RelativeHumidityTextWithUnit
    {
        get => _relativeHumidityTextWithUnit;
        private init
        {
            _relativeHumidityTextWithUnit = value;
            OnPropertyChanged();
        }
    }
    #endregion RelativeHumidityTextWithUnit

    #region ListOfFansViewModel
    private ObservableCollection<AbstractFan> _listOfFansViewModel = null!;
    public ObservableCollection<AbstractFan> ListOfFansViewModel
    {
        get => _listOfFansViewModel;
        set
        {
            _listOfFansViewModel = value;
            OnPropertyChanged();
        }
    }
    #endregion ListOfFansViewModel

    #region Phones
    private List<Phone> _phones = null!;
    public List<Phone> Phones
    {
        get => _phones;
        set
        {
            _phones = value;
            OnPropertyChanged();
        }
    }
    #endregion SortedListOfFans

    #endregion PropertyContainers

    #region SelectedContainers
    #region SelectedFanVersion
    private FanVersionItem _selectedFanVersion = null!;
    public FanVersionItem SelectedFanVersion
    {
        get => _selectedFanVersion;
        set
        {
            if (SetField(ref _selectedFanVersion, value))
                _selectFanCommand.RaiseCanExecuteChanged();

            #region Binding FanOperatingMaxTemperatureList
            FanOperatingMaxTemperatureList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanOperatingMaxTemperatureList;
            SelectedFanOperatingMaxTemperature = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanOperatingMaxTemperatureList[0];
            OnPropertyChanged(nameof(FanOperatingMaxTemperatureList));
            #endregion Binding FanOperatingMaxTemperatureList

            #region Binding FanSizeList
            FanSizeList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanSizeList;
            OnPropertyChanged(nameof(FanSizeList));
            #endregion Binding FanSizeList

            #region Binding FanBodyLengthList
            FanBodyLengthList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanBodyLengthList;
            SelectedFanBodyLength = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanBodyLengthList[0];
            OnPropertyChanged(nameof(FanBodyLengthList));
            OnPropertyChanged(nameof(SelectedFanBodyLength));
            #endregion Binding FanBodyLengthList

            #region Binding ImpellerRotationDirectionList
            ImpellerRotationDirectionList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .ImpellerRotationDirectionList;
            SelectedImpellerRotationDirection = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .ImpellerRotationDirectionList[0];
            OnPropertyChanged(nameof(ImpellerRotationDirectionList));
            #endregion Binding ImpellerRotationDirectionList

            #region Binding NominalPowerList
            NominalPowerList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .NominalPowerList;
            OnPropertyChanged(nameof(NominalPowerList));
            #endregion Binding NominalPowerList

            #region Binding NominalImpellerRotationSpeedList
            NominalImpellerRotationSpeedList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .NominalImpellerRotationSpeedList;
            OnPropertyChanged(nameof(NominalImpellerRotationSpeedList));
            #endregion Binding NominalImpellerRotationSpeedList

            #region Binding FanBodyExecutionMaterialList
            FanBodyExecutionMaterialList = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanBodyExecutionMaterialList;
            SelectedFanBodyExecutionMaterial = _selectedFanVersion
                .FanParameterNamesForComboBoxes
                .FanBodyExecutionMaterialList[0];
            OnPropertyChanged(nameof(FanBodyExecutionMaterialList));
            #endregion Binding FanBodyExecutionMaterialList

            AltitudeText = _selectedFanVersion.FanParameterByDefault.Altitude;
            AltitudeTextWithUnit =
                _selectedFanVersion.FanParameterByDefault.Altitude + " [м]";

            UpdateText();
        }
    }
    #endregion SelectedFanVersion

    #region SelectedFanOperatingMaxTemperature
    private string _selectedFanOperatingMaxTemperature = IdProjectDefaultValue;
    public string SelectedFanOperatingMaxTemperature
    {
        get =>
            CheckArgumentOutOfRangeException(
                _selectedFanVersion
                    .FanParameterValuesForProjectId
                    .FanOperatingMaxTemperatureList,
                _selectedFanVersion
                    .FanParameterNamesForComboBoxes
                    .FanOperatingMaxTemperatureList,
                _selectedFanOperatingMaxTemperature
            );
        set
        {
            _selectedFanOperatingMaxTemperature = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedFanSize

    #region SelectedFanSize
    private string _selectedFanSize = IdProjectDefaultValue;
    public string SelectedFanSize
    {
        get => _selectedFanSize;
        set
        {
            _selectedFanSize = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedFanSize

    #region SelectedFanBodyLength
    private string _selectedFanBodyLength = IdProjectDefaultValue;
    public string SelectedFanBodyLength
    {
        get => _selectedFanBodyLength;
        set
        {
            _selectedFanBodyLength = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedFanBodyLength

    #region SelectedImpellerRotationDirection
    private string _selectedImpellerRotationDirection = IdProjectDefaultValue;
    public string SelectedImpellerRotationDirection
    {
        get => _selectedImpellerRotationDirection;
        set
        {
            _selectedImpellerRotationDirection = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedImpellerRotationDirection

    #region SelectedNominalPower
    private string _selectedNominalPower = IdProjectDefaultValue;
    public string SelectedNominalPower
    {
        get => _selectedNominalPower;
        set
        {
            _selectedNominalPower = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedNominalPower

    #region SelectedNominalImpellerRotationSpeed
    private string _selectedNominalImpellerRotationSpeed =
        IdProjectDefaultValue;
    public string SelectedNominalImpellerRotationSpeed
    {
        get => _selectedNominalImpellerRotationSpeed;
        set
        {
            _selectedNominalImpellerRotationSpeed = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedNominalImpellerRotationSpeed

    #region SelectedFanBodyExecutionMaterial
    private string _selectedFanBodyExecutionMaterial = IdProjectDefaultValue;
    public string SelectedFanBodyExecutionMaterial
    {
        get => _selectedFanBodyExecutionMaterial;
        set
        {
            _selectedFanBodyExecutionMaterial = value;
            OnPropertyChanged();
            UpdateText();
        }
    }
    #endregion SelectedFanBodyExecutionMaterial

    #region SelectedNumberOfFans
    private string _selectedNumberOfFans = null!;
    public string SelectedNumberOfFans
    {
        get => _selectedNumberOfFans;
        set
        {
            _selectedNumberOfFans = value;
            OnPropertyChanged();
        }
    }
    #endregion SelectedFanLogic

    #region SelectedFanLogic
    private string _selectedFanLogic = null!;
    public string SelectedFanLogic
    {
        get => _selectedFanLogic;
        set
        {
            _selectedFanLogic = value;
            OnPropertyChanged();
        }
    }

    #endregion SelectedFanLogic

    #endregion SelectedContainers

    public ICommand SelectFanCommand => _selectFanCommand;
    
    private RelayCommandAsync _selectFanCommand;

    private UserInput _userInput = null!;
    
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
            // Перепроверяем, может ли команда выполняться
            (SelectFanCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

// Вспомогательные методы для проверки полей
    private bool IsVolumeFlowValid => 
        double.TryParse(VolumeFlowText, out double v) && v > 0;
    private bool IsTotalPressureValid => 
        double.TryParse(TotalPressureText, out double p) && p > 0;
    private bool IsFanVersionSelected => SelectedFanVersion != null;

    private bool CanExecuteSelectFan()
    {
        // Проверка числовых значений
        bool isVolumeFlowValid = double.TryParse(VolumeFlowText, out double v) && v > 0;
        bool isTotalPressureValid = double.TryParse(TotalPressureText, out double p) && p > 0;
        bool isFanVersionSelected = SelectedFanVersion != null;

        return isVolumeFlowValid && isTotalPressureValid && isFanVersionSelected;
    }

    private async Task ExecuteSelectFanAsync()
    {
        if (
            SelectedFanLogic == FanLogicList[1]
            && (
                string.IsNullOrEmpty(SelectedFanSize)
                || SelectedFanSize == IdProjectDefaultValue
            )
        )
        {
            MessageBox.Show(
                "Пожалуйста, выберите 'условный типоразмер крыльчатки'!"
            ); // Сообщение пользователю
        }
        else
        {
            _userInput = Calculate.ProcessUserInput(
                VolumeFlowText,
                TotalPressureText,
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .FanOperatingMaxTemperatureList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .FanOperatingMaxTemperatureList,
                    SelectedFanOperatingMaxTemperature
                ),
                Calculate
                    .ReturnCorrectOrDefaultIndex(
                        FanVersion.NamesForComboBox,
                        SelectedFanVersion
                            .FanParameterNamesForComboBoxes
                            .FanName
                    )
                    .ToString(),
                Calculate
                    .ReturnCorrectOrDefaultIndex(
                        FanLogic.NamesForComboBox,
                        SelectedFanLogic
                    )
                    .ToString(),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .FanSizeList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .FanSizeList,
                    SelectedFanSize
                ),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .FanBodyLengthList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .FanBodyLengthList,
                    SelectedFanBodyLength
                ),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .ImpellerRotationDirectionList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .ImpellerRotationDirectionList,
                    SelectedImpellerRotationDirection
                ),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .NominalPowerList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .NominalPowerList,
                    SelectedNominalPower
                ),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .NominalImpellerRotationSpeedList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .NominalImpellerRotationSpeedList,
                    SelectedNominalImpellerRotationSpeed
                ),
                CheckArgumentOutOfRangeException(
                    SelectedFanVersion
                        .FanParameterValuesForProjectId
                        .FanBodyExecutionMaterialList,
                    SelectedFanVersion
                        .FanParameterNamesForComboBoxes
                        .FanBodyExecutionMaterialList,
                    SelectedFanBodyExecutionMaterial
                ),
                TotalPressureDeviationText.ToString(
                    CultureInfo.InvariantCulture
                ),
                SpecificDeviationLeftText.ToString(
                    CultureInfo.InvariantCulture
                ),
                SpecificDeviationRightText.ToString(
                    CultureInfo.InvariantCulture
                ),
                RelativeHumidityText.ToString(CultureInfo.InvariantCulture),
                AltitudeText,
                FanOperatingCurrentTemperatureText.ToString(
                    CultureInfo.InvariantCulture
                ),
                NumberOfFansList[
                    Calculate.ReturnCorrectOrDefaultIndex(
                        NumberOfFans.Names,
                        SelectedNumberOfFans
                    )
                ]
            );

            await ProcessTheRequestAsync(_userInput);
        }
    }

    private void UpdateText()
    {
        ProjectIdText =
            $"{SelectedFanVersion.FanParameterValuesForProjectId.FanName}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                .FanParameterValuesForProjectId
                .FanOperatingMaxTemperatureList, SelectedFanVersion.FanParameterNamesForComboBoxes.FanOperatingMaxTemperatureList,
                SelectedFanOperatingMaxTemperature, IdProjectDefaultValue, 3)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                .FanParameterValuesForProjectId
                .FanSizeList, SelectedFanVersion.FanParameterNamesForComboBoxes.FanSizeList, SelectedFanSize, IdProjectDefaultValue, 3, 0.1)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                    .FanParameterValuesForProjectId
                    .FanBodyLengthList, SelectedFanVersion.FanParameterNamesForComboBoxes.FanBodyLengthList,
                SelectedFanBodyLength, IdProjectDefaultValue)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                    .FanParameterValuesForProjectId
                    .ImpellerRotationDirectionList, SelectedFanVersion.FanParameterNamesForComboBoxes.ImpellerRotationDirectionList,
                SelectedImpellerRotationDirection, IdProjectDefaultValue)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                    .FanParameterValuesForProjectId
                    .NominalPowerList, SelectedFanVersion.FanParameterNamesForComboBoxes.NominalPowerList,
                SelectedNominalPower, IdProjectDefaultValue, 4, 100)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                    .FanParameterValuesForProjectId
                    .NominalImpellerRotationSpeedList, SelectedFanVersion.FanParameterNamesForComboBoxes.NominalImpellerRotationSpeedList,
                SelectedNominalImpellerRotationSpeed, IdProjectDefaultValue, 4)}."
            + $"{CheckArgumentOutOfRangeException(SelectedFanVersion
                    .FanParameterValuesForProjectId
                    .FanBodyExecutionMaterialList, SelectedFanVersion.FanParameterNamesForComboBoxes.FanBodyExecutionMaterialList,
                SelectedFanBodyExecutionMaterial, IdProjectDefaultValue)}."
            + "Y2";
    }

    private static string CheckArgumentOutOfRangeException<T>(
        IReadOnlyList<T> projectIdList,
        List<string> comboBoxList,
        string selectedValue,
        string defaultValue = "",
        int roundingAccuracy = 0,
        double multiplier = 1.0
    )
        where T : IComparable, IConvertible
    {
        int index;
        try
        {
            index = comboBoxList.FindIndex(i => Equals(i, selectedValue));
        }
        catch (ArgumentOutOfRangeException)
        {
            index = 0;
        }
        catch (Exception e)
        {
            throw new Exception(
                $"Произошла ошибка {e} при проверке индекса в списке {comboBoxList}"
            );
        }

        switch (index)
        {
            case -1:
                return defaultValue;
            default:

                if (typeof(T) != typeof(string) && typeof(T) != typeof(double))
                {
                    throw new ArgumentException(
                        "Тип T должен быть double или string."
                    );
                }

                if (roundingAccuracy <= 0)
                {
                    return projectIdList[index].ToString() ?? defaultValue;
                }

                var convertedNumber = Convert.ToDouble(projectIdList[index]);
                var multipliedNumber = (int)(convertedNumber * multiplier);
                return multipliedNumber.ToString($"D{roundingAccuracy}");
        }
    }

    private async Task ProcessTheRequestAsync(UserInput userInput)
    {
        var validator = new UserInputValidator();

        var validateAsyncTask = validator.ValidateAndThrowAsync(userInput);

        /*var resultValidation = validator.Validate(userInput);
        var allMessages = resultValidation.ToString();

        if (!string.IsNullOrEmpty(allMessages))
        {
            throw new ArgumentException(allMessages);
        }*/

        var fansListAsyncTask = JsonLoader.DownloadAsync<FanData>(
            UserInput.PathDataOfFansJsonFile
        );

        await Task.WhenAll(validateAsyncTask, fansListAsyncTask);

        object? sortFans;
        switch (userInput.UserInputFan.FanVersion)
        {
            case 0:
                //sortFans = SortFans2.Sort<OsuDu>(fansList, userInput);
                sortFans = await CreatingListOfFans.CreateAsync<OsuDu>(
                    fansListAsyncTask.Result,
                    userInput
                );
                var newDataOsuDu = new ObservableCollection<AbstractFan>(
                    (List<OsuDu>)sortFans
                );
                ListOfFansViewModel = newDataOsuDu;
                /*Phones = new List<Phone>()
                {
                    new Phone()
                    {
                        Title = "iPhone 6S",
                        Company = "Apple",
                        Price = 54990
                    },
                    new Phone()
                    {
                        Title = "Lumia 950",
                        Company = "Microsoft",
                        Price = 39990
                    }
                };*/
                break;
            case 1:
                //sortFans = SortFans2.Sort<EuFan>(fansList, userInput);
                sortFans = await CreatingListOfFans.CreateAsync<EuFan>(
                    fansListAsyncTask.Result,
                    userInput
                );
                var newDataEuFan = new ObservableCollection<AbstractFan>(
                    (List<EuFan>)sortFans
                );
                ListOfFansViewModel = newDataEuFan;
                break;
            case 2:
                //sortFans = SortFans2.Sort<EuFan>(fansList, userInput);
                sortFans =
                    await CreatingListOfFans.CreateAsync<HighPressureFan>(
                        fansListAsyncTask.Result,
                        userInput
                    );
                var newDataHighPressureFanFan =
                    new ObservableCollection<AbstractFan>(
                        (List<HighPressureFan>)sortFans
                    );
                ListOfFansViewModel = newDataHighPressureFanFan;
                break;
        }
    }

    public MainViewModel()
    {
        //Содержимое ComboBoxes
        FanVersionList = new List<FanVersionItem>
        {
            new()
            {
                FanParameterNamesForComboBoxes =
                    new FanParameterNamesForComboBoxes
                    {
                        FanName = FanVersion.NamesForComboBox[0],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.NamesForOsuDu.ToList(),
                        FanSizeList = Sizes.NamesForOsuDu.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.NamesForOsuDu.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.NamesForOsuDu.ToList(),
                        NominalPowerList = NominalPowers.Names.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Names.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Names.ToList()
                    },
                FanParameterValuesForProjectId =
                    new FanParameterValuesForProjectId
                    {
                        FanName = FanVersion.ValuesForComboBox[0],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.ValuesForOsuDu.ToList(),
                        FanSizeList = Sizes.ValuesForOsuDu.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.ValuesForOsuDu.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.ValuesForOsuDu.ToList(),
                        NominalPowerList = NominalPowers.Values.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Values.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Values.ToList()
                    },
                FanParameterByDefault = new FanParameterByDefault
                {
                    Altitude = FanVersion.Altitude[0]
                }
            },
            new()
            {
                FanParameterNamesForComboBoxes =
                    new FanParameterNamesForComboBoxes
                    {
                        FanName = FanVersion.NamesForComboBox[1],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.NamesForEuFan.ToList(),
                        FanSizeList = Sizes.NamesForEuFan.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.NamesForEuFan.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.NamesForEuFan.ToList(),
                        NominalPowerList = NominalPowers.Names.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Names.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Names.ToList()
                    },
                FanParameterValuesForProjectId =
                    new FanParameterValuesForProjectId
                    {
                        FanName = FanVersion.ValuesForComboBox[1],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.ValuesForEuFan.ToList(),
                        FanSizeList = Sizes.ValuesForEuFan.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.ValuesForEuFan.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.ValuesForEuFan.ToList(),
                        NominalPowerList = NominalPowers.Values.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Values.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Values.ToList()
                    },
                FanParameterByDefault = new FanParameterByDefault
                {
                    Altitude = FanVersion.Altitude[1]
                }
            },
            new()
            {
                FanParameterNamesForComboBoxes =
                    new FanParameterNamesForComboBoxes
                    {
                        FanName = FanVersion.NamesForComboBox[2],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.NamesForHighPressureFan.ToList(),
                        FanSizeList = Sizes.NamesForHighPressureFan.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.NamesForHighPressureFan.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.NamesForHighPressureFan.ToList(),
                        NominalPowerList = NominalPowers.Names.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Names.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Names.ToList()
                    },
                FanParameterValuesForProjectId =
                    new FanParameterValuesForProjectId
                    {
                        FanName = FanVersion.ValuesForComboBox[2],
                        FanOperatingMaxTemperatureList =
                            FanOperatingMaxTemperatures.ValuesForHighPressureFan.ToList(),
                        FanSizeList = Sizes.ValuesForHighPressureFan.ToList(),
                        FanBodyLengthList =
                            FanBodyLengths.ValuesForHighPressureFan.ToList(),
                        ImpellerRotationDirectionList =
                            ImpellerRotationDirections.ValuesForHighPressureFan.ToList(),
                        NominalPowerList = NominalPowers.Values.ToList(),
                        NominalImpellerRotationSpeedList =
                            NominalImpellerRotationSpeeds.Values.ToList(),
                        FanBodyExecutionMaterialList =
                            CaseExecutionMaterials.Values.ToList()
                    },
                FanParameterByDefault = new FanParameterByDefault
                {
                    Altitude = FanVersion.Altitude[2]
                }
            }
        };
        FanLogicList = FanLogic.NamesForComboBox;
        SelectedFanLogic = FanLogic.NamesForComboBox[0];
        NumberOfFansList = NumberOfFans.Names.ToList();
        SelectedNumberOfFans = NumberOfFansList[0];

        TotalPressureDeviationText =
            UserInputWorkPoint.TotalPressureDeviationByDefault;
        TotalPressureDeviationTextWithUnit =
            UserInputWorkPoint.TotalPressureDeviationByDefault + " [%]";

        SpecificDeviationLeftText =
            UserInputWorkPoint.SpecificDeviationLeftByDefault;
        SpecificDeviationLeftTextWithUnit =
            UserInputWorkPoint.SpecificDeviationLeftByDefault + " [%]";

        SpecificDeviationRightText =
            UserInputWorkPoint.SpecificDeviationRightByDefault;
        SpecificDeviationRightTextWithUnit =
            UserInputWorkPoint.SpecificDeviationRightByDefault + " [%]";

        FanOperatingCurrentTemperatureText =
            UserInputAir.FanOperatingCurrentTemperatureByDefault;
        FanOperatingCurrentTemperatureTextWithUnit =
            UserInputAir.FanOperatingCurrentTemperatureByDefault + " [°C]";
        RelativeHumidityText = UserInputAir.RelativeHumidityByDefault;
        RelativeHumidityTextWithUnit =
            UserInputAir.RelativeHumidityByDefault + " [°C]";
        _selectFanCommand = new RelayCommandAsync(
            ExecuteSelectFanAsync, 
            CanExecuteSelectFan
        );
    }
}