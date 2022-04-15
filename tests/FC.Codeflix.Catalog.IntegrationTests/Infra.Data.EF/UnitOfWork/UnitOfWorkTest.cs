using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using InfraUOW = FC.Codeflix.Catalog.Infra.Data.EF;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork")]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategories = _fixture.GetExampleCategoriesList();
        await dbContext.AddRangeAsync(exampleCategories);
        var unitOfWork = new InfraUOW.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        dbContext = _fixture.CreateDbContext(true);
        var dbCategories = dbContext.Categories.AsNoTracking().ToList();
        dbCategories.Should().NotBeNull();
        dbCategories.Should().HaveCount(exampleCategories.Count);
    }

    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new InfraUOW.UnitOfWork(dbContext);

        var task = async () => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }

}
