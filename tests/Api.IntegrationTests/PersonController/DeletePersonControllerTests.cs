﻿using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

[Collection("Test collection")]
public class DeletePersonControllerTests: ControllerTestsBase
{
    public DeletePersonControllerTests(PersonApiFactory apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenPersonExists()
    {
        // Arrange
        var person = PersonGenerator.Generate();
        var personId = Guid.NewGuid();
        
        var createdResponse = await _client.PostAsJsonAsync($"people/{personId}", person);
        var createdPerson = await createdResponse.Content.ReadFromJsonAsync<PersonResponse>();

        // Act
        var response = await _client.DeleteAsync($"people/{personId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync($"people/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
