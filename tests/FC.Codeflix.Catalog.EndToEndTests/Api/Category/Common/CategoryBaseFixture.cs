using FC.Codeflix.Catalog.EndToEndTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
public class CategoryBaseFixture : BaseFixture
{
    public CategoryPersistence Persistence;

    public CategoryBaseFixture()
        : base()
    {
        Persistence = new CategoryPersistence(CreateDbContext());
    }

    public DomainEntity.Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory())
            .ToList();

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
        => new Random().NextDouble() < 0.5;

    public string GetInvalidNameTooShort() 
        => Faker.Commerce.ProductName()[..2];

    public string GetInvalidNameTooLong()
    {
        var nameTooLong = Faker.Commerce.ProductName();
        while (nameTooLong.Length <= 255)
            nameTooLong = $"{nameTooLong} {Faker.Commerce.ProductName()}";
        return nameTooLong;
    }

    public string GetInvalidDescriptionTooLong()
    {
        var longDescription = Faker.Commerce.ProductDescription();
        while (longDescription.Length <= 10000)
            longDescription = $"{longDescription} {Faker.Commerce.ProductDescription()}";
        return longDescription;
    }
}
