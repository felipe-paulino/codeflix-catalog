using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
        => _categoryTestFixture = categoryTestFixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = 
            () => new DomainEntity.Category(name!, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    // Nome deve ter no mínimo 3 caracteres
    [Theory(DisplayName = nameof(InstatianteErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("aa")]
    public void InstatianteErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    // Nome deve ter no máximo 255 characters
    [Fact(DisplayName = nameof(InstatianteErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatianteErrorWhenNameGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    // Descrição deve ter no máximo 10000 caracteres
    [Fact(DisplayName = nameof(InstatianteErrorWhenDescriptionGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatianteErrorWhenDescriptionGreaterThan10000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();

        category.Update(newName);

        category.Name.Should().Be(newName);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();
        Action action =
            () => category.Update(name!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameGreaterThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        
        Action action =
            () => category.Update(invalidName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("aa")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        
        Action action =
            () => category.Update(invalidName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionGreaterThan10000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        
        Action action =
            () => category.Update("Category New Name", invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }
}
