using FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection
    : ICollectionFixture<GetCategoryTestFixture>
{ }

public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{
}
