using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;
using System.Collections.Generic;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (var i = 0; i < times; i++)
        {
            var exampleCategory = fixture.GetValidCategory();

            var input = fixture.GetValidInput(exampleCategory.Id);

            yield return new object[]
            {
                exampleCategory,
                input
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();

        for (var i = 0; i < times; i++)
        {
            var exampleCategory = fixture.GetValidCategory();
            yield return new object[] {
                exampleCategory,
                fixture.GetInvalidInputShortName(exampleCategory.Id),
                "Name should be at least 3 characters long"
            };
            yield return new object[] {
                exampleCategory,
                fixture.GetInvalidInputLongName(exampleCategory.Id),
                "Name should be less or equal 255 characters long"
            };
            yield return new object[] {
                exampleCategory,
                fixture.GetInvalidInputNullName(exampleCategory.Id),
                "Name should not be null or empty"
            };
            yield return new object[] {
                exampleCategory,
                fixture.GetInvalidInputLongDescription(exampleCategory.Id),
                "Description should be less or equal 10000 characters long"
            };

        }
    }
}
