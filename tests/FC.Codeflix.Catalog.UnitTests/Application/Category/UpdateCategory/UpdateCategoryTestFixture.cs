using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using System;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;
public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
        => new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public UpdateCategoryInput GetInvalidInputShortName(Guid? id = null)
    {
        var invalidInputShortName = GetValidInput(id);
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputLongName(Guid? id = null)
    {
        var invalidInputLongName = GetValidInput(id);
        while (invalidInputLongName.Name.Length <= 255)
            invalidInputLongName.Name = $"{invalidInputLongName.Name} {Faker.Commerce.ProductName()}";
        return invalidInputLongName;
    }

    public UpdateCategoryInput GetInvalidInputNullName(Guid? id = null)
    {
        var invalidInputNullName = GetValidInput(id);
        invalidInputNullName.Name = null!;
        return invalidInputNullName;
    }

    public UpdateCategoryInput GetInvalidInputLongDescription(Guid? id = null)
    {
        var invalidInputLongDescription = GetValidInput(id);
        while (invalidInputLongDescription.Description!.Length <= 10000)
            invalidInputLongDescription.Description = $"{invalidInputLongDescription.Description} {Faker.Commerce.ProductDescription()}";
        return invalidInputLongDescription;
    }
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
    : ICollectionFixture<UpdateCategoryTestFixture>
{ }
