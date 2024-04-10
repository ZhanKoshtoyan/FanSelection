using FluentValidation;
using Libraries.Description_of_objects.UserInput;
using Libraries.Fans;
using Libraries.Loader;
using Libraries.PrintFolder;
using Libraries.Validate_and_sort;

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

        var fansList = JsonLoader.Download(UserInput.PathJsonFile);

        object? sortFans;
        switch (userInput.UserInputFan.FanVersion)
        {
            case 0:
                sortFans = SortFans.Sort<OsuDu>(fansList, userInput);
                ToPrint.Print((List<OsuDu>) sortFans, userInput);
                break;
            case 1:
                sortFans = SortFans.Sort<EuFan>(fansList, userInput);
                ToPrint.Print((List<EuFan>) sortFans, userInput);
                break;
        }
    }
}
