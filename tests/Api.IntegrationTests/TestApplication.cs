﻿using System.Data.Common;
using DockerTestsSample.Api.IntegrationTests.Extensions;
using DockerTestsSample.Client.Abstract;
using DockerTestsSample.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

public sealed class TestApplication :
    WebApplicationFactory<IApiMarker>, 
    IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithImage(DockerImages.PostgreSql)
            .WithDatabase("TestDb")
            .WithUsername("user")
            .WithPassword("password")
            .Build();
    
    private DbConnection? _dbConnection;
    private Respawner? _respawner;
    private HttpClient? _httpClient;

    private DbConnection DbConnection => _dbConnection.Required();
    private Respawner Respawner => _respawner.Required();
    public HttpClient HttpClient => _httpClient.Required();
    
    public ISampleClient SampleClient => Services.GetRequiredService<ISampleClient>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => services.AddSampleClientTest(CreateClient));
        
        builder
            .UseSetting("ConnectionStrings:PopulationDb", _dbContainer.GetConnectionString())
            .UseSetting("Logging:LogLevel:Default", LogLevel.Warning.ToString())
            .UseSetting("Logging:LogLevel:Microsoft", LogLevel.Warning.ToString());
    }

    public async Task ResetDatabaseAsync() 
        => await Respawner.ResetAsync(DbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _httpClient = CreateClient();
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await DbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(DbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    public new async Task DisposeAsync() 
        => await _dbContainer.StopAsync();
}
