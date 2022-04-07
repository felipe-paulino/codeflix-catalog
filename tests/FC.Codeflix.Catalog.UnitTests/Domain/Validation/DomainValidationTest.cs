using Bogus;
using Xunit;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using System;
using FC.Codeflix.Catalog.Domain.Exceptions;
using System.Collections.Generic;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    public Faker Faker { get; set; } = new Faker();

    // não ser null
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNull(value, "Value");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;

        Action action =
            () => DomainValidation.NotNull(value, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null");
    }

    // não ser null ou vazio
    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "FieldName");

        action.Should().NotThrow();
    }

    // tamanho mínimo
    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength, "FieldName");

        action.Should()
            .NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesWithLessThanMinCharacters), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesWithLessThanMinCharacters(int numberOfTests = 6)
    {
        var faker = new Faker();
        var random = new Random();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var minLength = value.Length + random.Next(1, 20);
            yield return new object[] {
                value,
                minLength
            };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 6)
    {
        var faker = new Faker();
        var random = new Random();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var minLength = value.Length - random.Next(1, value.Length);
            yield return new object[] {
                value,
                minLength
            };
        }
    }

    // Tamanho máximo

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should be less or equal {maxLength} characters long");
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, "FieldName");

        action.Should()
            .NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 6)
    {
        var faker = new Faker();
        var random = new Random();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var maxLength = value.Length - random.Next(1, value.Length);
            yield return new object[] {
                value,
                maxLength
            };
        }
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 6)
    {
        var faker = new Faker();
        var random = new Random();
        for (int i = 0; i < numberOfTests; i++)
        {
            var value = faker.Commerce.ProductName();
            var maxLength = value.Length + random.Next(1, 20);
            yield return new object[] {
                value,
                maxLength
            };
        }
    }
}
