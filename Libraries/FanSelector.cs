using FluentValidation;
using Libraries.DescriptionOfObjects.UserInput;
using Libraries.Fans;
using Libraries.Loader;
using Libraries.PrintFolder;
using Libraries.StructureOfObjects;
using Libraries.ValidateAndSort;

namespace Libraries;

public static class FanSelector
{
    public static void DoIt(UserInput userInput)
    {
        var validator = new UserInputValidator();

        validator.ValidateAndThrow(userInput);

        /*var resultValidation = validator.Validate(userInput);
        var allMessages = resultValidation.ToString();

        if (!string.IsNullOrEmpty(allMessages))
        {
            throw new ArgumentException(allMessages);
        }*/

        var fansList = JsonLoader.Download<FanData>(
            UserInput.PathDataOfFansJsonFile
        );

        object? sortFans;
        switch (userInput.UserInputFan.FanVersion)
        {
            case 0:
                // sortFans = SortFans2.Sort<OsuDu>(fansList, userInput);
                sortFans = CreatingListOfFans.Create<OsuDu>(
                    fansList,
                    userInput
                );
                ToPrint.Print((List<OsuDu>)sortFans, userInput);
                break;
            case 1:
                //sortFans = SortFans2.Sort<EuFan>(fansList, userInput);
                sortFans = CreatingListOfFans.Create<EuFan>(
                    fansList,
                    userInput
                );
                ToPrint.Print((List<EuFan>)sortFans, userInput);
                break;
        }
    }
}
