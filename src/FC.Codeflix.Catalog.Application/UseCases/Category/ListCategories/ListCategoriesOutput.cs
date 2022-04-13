using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategoriesOutput
    : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoriesOutput(
        int page, 
        int perPage, 
        int total, 
        IReadOnlyList<CategoryModelOutput> items) 
        : base(page, perPage, total, items)
    {
    }
}
