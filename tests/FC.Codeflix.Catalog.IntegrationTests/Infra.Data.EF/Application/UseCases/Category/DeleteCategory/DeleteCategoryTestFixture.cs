using FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Application.UseCases.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{ }

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{

}
