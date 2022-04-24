using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using System.Collections.Generic;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;
public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var numberOfCasesForEachInvalidInputType = 1;

        for (var i = 0; i < numberOfCasesForEachInvalidInputType; i++)
        {
            yield return new object[] {
                new CreateCategoryInput(
                    fixture.GetInvalidNameTooShort(),
                    fixture.GetValidCategoryDescription(),
                    fixture.GetRandomBoolean()
                ),
                "Name should be at least 3 characters long"
            };
            yield return new object[] {
                new CreateCategoryInput(
                    fixture.GetInvalidNameTooLong(),
                    fixture.GetValidCategoryDescription(),
                    fixture.GetRandomBoolean()
                ),
                "Name should be less or equal 255 characters long"
            };
            yield return new object[] {
                new CreateCategoryInput(
                    "",
                    fixture.GetValidCategoryDescription(),
                    fixture.GetRandomBoolean()
                ),
                "Name should not be null or empty"
            };
            yield return new object[] {
                new CreateCategoryInput(
                    fixture.GetValidCategoryName(),
                    fixture.GetInvalidDescriptionTooLong(),
                    fixture.GetRandomBoolean()
                ),
                "Description should be less or equal 10000 characters long"
            };

        }
    }
}
