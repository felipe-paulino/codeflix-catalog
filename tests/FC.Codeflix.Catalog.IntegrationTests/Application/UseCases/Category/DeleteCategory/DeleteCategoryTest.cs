using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using InfraData = FC.Codeflix.Catalog.Infra.Data.EF;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - UseCases")]
    public async Task DeleteCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var input = new DeleteCategoryInput(exampleCategory.Id);
        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);

        await useCase.Handle(input, CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategory = dbContext.Categories.FirstOrDefault(x => x.Id == exampleCategory.Id);
        dbCategory.Should().BeNull();
    }


    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Integration/Application", "DeleteCategory - UseCases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var exampleGuid = Guid.NewGuid();
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new InfraData.UnitOfWork(dbContext);
        var input = new DeleteCategoryInput(exampleGuid);
        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleGuid}' not found.");
    }
}
