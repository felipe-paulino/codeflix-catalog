using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<Category> Categories
        => _context.Set<Category>();

    public CategoryRepository(CodeflixCatalogDbContext context)
        => _context = context;

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
        => Task.FromResult(Categories.Remove(aggregate));

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
        return category!;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        => await Categories.AddAsync(aggregate, cancellationToken);

    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var query = Categories.AsNoTracking();

        query = ApplyOrdering(query, input.OrderBy, input.Order);
        if (!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));

        //var amountToBeSkipped = input.Page > 0 ?
        //    (input.Page - 1) * input.PerPage : 0;
        var total = await query.CountAsync();
        var items = await query
            .Skip((input.Page - 1) * input.PerPage)
            .Take(input.PerPage)
            .ToListAsync();
        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Category> ApplyOrdering(
        IQueryable<Category> categories,
        string orderBy,
        SearchOrder order)
    {
        var orderedQuery = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => categories.OrderBy(x => x.Name).ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => categories.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => categories.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => categories.OrderByDescending(x => x.CreatedAt),
            ("id", SearchOrder.Asc) => categories.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => categories.OrderByDescending(x => x.Id),
            _ => categories.OrderBy(x => x.Name).ThenBy(x => x.Id)
        };
        return orderedQuery.ThenBy(x => x.CreatedAt);
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
        => Task.FromResult(Categories.Update(aggregate));
}
