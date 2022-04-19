using System.Collections.Generic;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.CreateCategory;
public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfCasesForEachInvalidInputType = 1)
    {
        var fixture = new CreateCategoryTestFixture();

        for (var i = 0; i < numberOfCasesForEachInvalidInputType; i++)
        {
            yield return new object[] { 
                fixture.GetInvalidInputShortName(), 
                "Name should be at least 3 characters long" 
            };
            yield return new object[] { 
                fixture.GetInvalidInputLongName(), 
                "Name should be less or equal 255 characters long" 
            };
            yield return new object[] { 
                fixture.GetInvalidInputNullName(), 
                "Name should not be null or empty" 
            };
            yield return new object[] { 
                fixture.GetInvalidInputNullDescription(), 
                "Description should not be null" 
            };
            yield return new object[] { 
                fixture.GetInvalidInputLongDescription(), 
                "Description should be less or equal 10000 characters long" 
            };

        }
    }
}
