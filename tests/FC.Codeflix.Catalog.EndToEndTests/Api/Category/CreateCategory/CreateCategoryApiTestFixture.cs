using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTesteFixtureCollection
    : ICollectionFixture<CreateCategoryApiTestFixture>
{ }

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInput GetInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
}
