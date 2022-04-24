using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryInput GetInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputLongName()
    {
        var invalidInputLongName = GetInput();
        while (invalidInputLongName.Name.Length <= 255)
            invalidInputLongName.Name = $"{invalidInputLongName.Name} {Faker.Commerce.ProductName()}";
        return invalidInputLongName;
    }

    public CreateCategoryInput GetInvalidInputNullName()
    {
        var invalidInputNullName = GetInput();
        invalidInputNullName.Name = null!;
        return invalidInputNullName;
    }

    public CreateCategoryInput GetInvalidInputNullDescription()
    {
        var invalidInputNullDescription = GetInput();
        invalidInputNullDescription.Description = null!;
        return invalidInputNullDescription;
    }

    public CreateCategoryInput GetInvalidInputLongDescription()
    {
        var invalidInputLongDescription = GetInput();
        while (invalidInputLongDescription.Description.Length <= 10000)
            invalidInputLongDescription.Description = $"{invalidInputLongDescription.Description} {Faker.Commerce.ProductDescription()}";
        return invalidInputLongDescription;
    }
}
