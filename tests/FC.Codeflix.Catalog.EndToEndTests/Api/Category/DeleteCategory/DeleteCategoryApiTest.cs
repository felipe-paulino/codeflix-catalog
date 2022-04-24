using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTest
{
    private readonly DeleteCategoryApiTestFixture _fixture;

    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async Task DeleteCategory()
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleCategory = exampleCategories[10];

        var (response, output) = await _fixture.ApiClient.Delete<object>(
            $"/categories/{exampleCategory.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        output.Should().BeNull();
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>(
            $"/categories/{exampleGuid}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not found");
        output.Type.Should().Be("NotFound");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Detail.Should().Be($"Category '{exampleGuid}' not found.");
    }
}
