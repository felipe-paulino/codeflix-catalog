﻿using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - UseCases")]
    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var input = new DeleteCategoryInput(exampleCategory.Id);
        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);
        

        await useCase.Handle(input, CancellationToken.None);


        repositoryMock.Verify(
            x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        repositoryMock.Verify(
            x => x.Delete(
                exampleCategory,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
           ), 
           Times.Once
        );
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Application", "DeleteCategory - UseCases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));
        var input = new DeleteCategoryInput(exampleGuid);
        var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>();

        repositoryMock.Verify(
            x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
}
