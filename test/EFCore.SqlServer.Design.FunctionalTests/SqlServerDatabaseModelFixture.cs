// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Relational.Design.Specification.TestUtilities;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.FunctionalTests.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.SqlServer.Design.FunctionalTests
{
    public class SqlServerDatabaseModelFixture : IDisposable
    {
        public SqlServerDatabaseModelFixture()
        {
            TestStore = SqlServerTestStore.CreateScratch();
        }

        public TestDesignLoggerFactory TestDesignLoggerFactory { get; } = new TestDesignLoggerFactory();

        public DatabaseModel CreateModel(string createSql, TableSelectionSet selection = null, ILogger logger = null)
        {
            TestStore.ExecuteNonQuery(createSql);

            return new SqlServerDatabaseModelFactory(
                    new DiagnosticsLogger<LoggerCategory.Scaffolding>(
                        new InterceptingLogger<LoggerCategory.Scaffolding>(
                            TestDesignLoggerFactory,
                            new LoggingOptions()),
                        new DiagnosticListener("Fake")))
                .Create(TestStore.ConnectionString, selection ?? TableSelectionSet.All);
        }

        public SqlServerTestStore TestStore { get; }

        public void ExecuteNonQuery(string sql) => TestStore.ExecuteNonQuery(sql);

        public void Dispose() => TestStore.Dispose();
    }
}
