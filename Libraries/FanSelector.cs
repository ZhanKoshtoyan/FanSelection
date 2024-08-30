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
    public static async Task ProcessTheRequest(UserInput userInput)
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
                // sortFans = SortFans2.Sort<OsuDu>(fansList, userInput);
                sortFans = await CreatingListOfFans.CreateAsync<OsuDu>(
                    fansListAsyncTask.Result,
                    userInput
                );
                ToPrint.Print((List<OsuDu>)sortFans, userInput);
                break;
            case 1:
                //sortFans = SortFans2.Sort<EuFan>(fansList, userInput);
                sortFans = await CreatingListOfFans.CreateAsync<EuFan>(
                    fansListAsyncTask.Result,
                    userInput
                );
                ToPrint.Print((List<EuFan>)sortFans, userInput);
                break;
            case 2:
                //sortFans = SortFans2.Sort<EuFan>(fansList, userInput);
                sortFans =
                    await CreatingListOfFans.CreateAsync<HighPressureFan>(
                        fansListAsyncTask.Result,
                        userInput
                    );
                ToPrint.Print((List<HighPressureFan>)sortFans, userInput);
                break;
        }
    }
}
