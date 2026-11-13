
using System.Net;
using System.Net.Http.Json;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Tests;

public class ProjectsEndpointsTests
{
    [Fact]
    public async Task GetProjects_ReturnsEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/projects/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var projects = await response.Content
            .ReadFromJsonAsync<List<Project>>();

        Assert.NotNull(projects);
        Assert.Empty(projects);
    }
}
