﻿using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory;
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{ }

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryFixtureCollection :
    ICollectionFixture<GetCategoryTestFixture>
{ }