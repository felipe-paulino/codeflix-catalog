using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;
using System;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.Categories(1)[0];
        if (categoryDescription.Length > 255)
            categoryDescription = categoryDescription[..255];
        return categoryDescription;
    }

    public bool GetRandomBoolean()
        => (new Random()).NextDouble() < 0.5;

    public Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

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

    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
    : ICollectionFixture<UpdateCategoryTestFixture>
{ }
