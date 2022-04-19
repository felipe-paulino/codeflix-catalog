using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using InfraEF = FC.Codeflix.Catalog.Infra.Data.EF;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - UseCases")]
    public async Task CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraEF.UnitOfWork(dbContext);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.Should().NotBe(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Integration/Application", "CreateCategory - UseCases")]
    public async Task CreateCategoryWithOnlyName()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraEF.UnitOfWork(dbContext);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var input = new CreateCategoryInput(_fixture.GetInput().Name);

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().BeEmpty();
        dbCategory.IsActive.Should().BeTrue();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().BeEmpty();
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.Should().NotBe(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Integration/Application", "CreateCategory - UseCases")]
    public async Task CreateCategoryWithOnlyNameAndDescription()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraEF.UnitOfWork(dbContext);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().BeTrue();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.Should().NotBe(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "CreateCategory - UseCases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
        2,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCategory(
        CreateCategoryInput input,
        string exceptionMsg
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraEF.UnitOfWork(dbContext);
        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMsg);

        dbContext = _fixture.CreateDbContext(true);
        var categoryAmount = await dbContext.Categories.CountAsync();
        categoryAmount.Should().Be(0);
    }
}
