using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest : IDisposable
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
        => _fixture = fixture;


    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategory()
    {
        //Arrange
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleCategory = exampleCategories[10];
        var input = _fixture.GetExampleApiInput();


        // Act
        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>(
            $"/categories/{exampleCategory.Id}",
            input 
        );

        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Should().NotBeNull();
        output!.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);
        output.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);
        var dbCategory = await _fixture.Persistence
            .GetById(output.Data.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
    }


    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryApiInput(
            _fixture.GetValidCategoryName()
        );

        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture
            .Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }


    [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryNameAndDescription()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryApiInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture
            .Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }


    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleGuid = Guid.NewGuid();
        var input = _fixture.GetExampleApiInput();

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{exampleGuid}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not found");
        output.Type.Should().Be("NotFound");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Detail.Should().Be($"Category '{exampleGuid}' not found.");
    }


    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    [MemberData(
        nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryApiTestDataGenerator)
    )]
    public async Task ErrorWhenCantInstantiateAggregate(
        UpdateCategoryApiInput input,
        string errorMessage
    )
    {
        var exampleCategories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategories);
        var exampleCategory = exampleCategories[10];

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(errorMessage);
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
