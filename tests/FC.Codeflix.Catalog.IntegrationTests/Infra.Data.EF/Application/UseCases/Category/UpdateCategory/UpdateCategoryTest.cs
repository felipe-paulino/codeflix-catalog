using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using System.Threading.Tasks;
using Xunit;
using InfraData = FC.Codeflix.Catalog.Infra.Data.EF;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FC.Codeflix.Catalog.Application.Exceptions;
using System;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategory(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput input)
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
        );
        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
    }


    [Theory(DisplayName = nameof(UpdateCategoryNameAndDescriptionOnly))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryNameAndDescriptionOnly(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description
        );
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
        );
        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }


    [Theory(DisplayName = nameof(UpdateCategoryNameOnly))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryNameOnly(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name
        );
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
        );
        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var output = await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = await dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }


    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoestExist))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    public async Task NotFoundExceptionWhenCategoryDoestExist()
    {

        var exampleGuid = Guid.NewGuid();
        var input = _fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }


    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        3,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task ThrowWhenCantUpdateCategory(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput input,
        string exceptionMsg
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCategory(
            repository,
            unitOfWork
        );
        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMsg);
    }
}
