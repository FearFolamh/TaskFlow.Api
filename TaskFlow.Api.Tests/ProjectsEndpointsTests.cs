
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

    [Fact]
    public async Task PostProject_ReturnsCreatedProject_WhenRequestIsValid()
    {
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        var newProject = new Project
        {
            Title = "Test Project",
            Description = "This is a test project."
        };

        var response = await client.PostAsJsonAsync("/projects/", newProject);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdProject = await response.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(createdProject);
        Assert.Equal(newProject.Title, createdProject.Title);
        Assert.Equal(newProject.Description, createdProject.Description);
        Assert.True(createdProject.id > 0);

        var getResponse = await client.GetAsync($"/projects/{createdProject.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var project = await getResponse.Content
            .ReadFromJsonAsync<Project>();

        Assert.NotNull(project);
        Assert.Equal(createdProject.id, project.id);
        Assert.Equal(createdProject.Title, project.Title);
        Assert.Equal(createdProject.Description, project.Description);
    }

    [Fact]

    public async Task PostProject_ReturnsBadRequest_WhenRequestIsNotValid()
    {   
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        var newProject = new Project
        {
            Title = "", // Invalid title (empty)
            Description = "This is a test project."
        };

        var response = await client.PostAsJsonAsync("/projects/", newProject);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var newProject2 = new Project
        {
            Title = new string('A', 101), // Invalid title (too long)
            Description = "This is a test project."
        };

        var response2 = await client.PostAsJsonAsync("/projects/", newProject2);
        
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
     

        var getResponse = await client.GetAsync("/projects/");


        var projects = await getResponse.Content
            .ReadFromJsonAsync<List<Project>>();

        Assert.NotNull(projects);
        Assert.Empty(projects);
    }

    [Fact]

    public async Task GetProjectById_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/projects/999"); // Assuming 999 does not exist

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]

    public async Task GetProjectById_ReturnsProject_WhenProjectExists()
    {
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        var newProject = new Project
        {
            Title = "Test Project",
            Description = "This is a test project."
        };

        var postResponse = await client.PostAsJsonAsync("/projects/", newProject);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        var createdProject = await postResponse.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(createdProject);

        var getResponse = await client.GetAsync($"/projects/{createdProject.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var project = await getResponse.Content.ReadFromJsonAsync<Project>();

        Assert.NotNull(project);
        Assert.Equal(createdProject.id, project.id);
        Assert.Equal(createdProject.Title, project.Title);
        Assert.Equal(createdProject.Description, project.Description);
    }

}

 