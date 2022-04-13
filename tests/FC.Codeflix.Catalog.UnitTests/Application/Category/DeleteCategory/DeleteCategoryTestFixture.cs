﻿using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.DeleteCategory;

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{ }

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{ }