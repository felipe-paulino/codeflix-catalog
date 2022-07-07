using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task GetCategory()
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleCategory = exampleCategories[10];

        var (response, output) = await _fixture.ApiClient.Get<ApiResponse<CategoryModelOutput>>(
            $"/categories/{exampleCategory.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Should().NotBeNull();
        output!.Data.Name.Should().Be(exampleCategory.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        output.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.CreatedAt.Should().BeCloseTo(exampleCategory.CreatedAt, 1.Milliseconds());
    }


    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
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

    public void Dispose()
        => _fixture.CleanPersistence();
}
