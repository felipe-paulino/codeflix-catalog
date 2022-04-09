using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;
using System;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTestFixture : BaseFixture
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

    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }
