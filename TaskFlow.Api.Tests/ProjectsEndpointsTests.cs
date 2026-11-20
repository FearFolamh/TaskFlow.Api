
using System.Net;
using System.Net.Http.Json;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
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

    [Fact]

    public async Task PutProject_ReturnsUpdatedProject_WhenProjectExists()
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
        var updateProject = new UpdateProjectRequest
        {
            Title = "Updated Project",
            Description = "This is an updated test project."
        };
        var putResponse = await client.PutAsJsonAsync($"/projects/{createdProject.id}", updateProject);
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var getResponse = await client.GetAsync($"/projects/{createdProject.id}");
        
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var getProject = await getResponse.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(getProject);

        Assert.Equal(updateProject.Title, getProject.Title);
        Assert.Equal(updateProject.Description, getProject.Description);

    }

    [Fact]

    public async Task PutProject_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();
        var updateProject = new UpdateProjectRequest
        {
            Title = "Updated Project",
            Description = "This is an updated test project."
        };
        var putResponse = await client.PutAsJsonAsync($"/projects/999", updateProject); // Assuming 999 does not exist
        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
    }

    [Fact]

    public async Task PatchProject_UpdatesOnlyTitle_WhenTitleIsProvided()
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
        var updateProject = new UpdateProjectRequest
        {
            Title = "Updated Project"
            
        };
        var patchResponse = await client.PatchAsJsonAsync($"/projects/{createdProject.id}", updateProject);
        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

        var getResponse = await client.GetAsync($"/projects/{createdProject.id}");
        
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var getProject = await getResponse.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(getProject);

        Assert.Equal(updateProject.Title, getProject.Title);
        Assert.Equal(newProject.Description, getProject.Description);

    }

    [Fact]

    public async Task PatchProject_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();
        var updateProject = new UpdateProjectRequest
        {
            Title = "Updated Project",
            
        };
        var patchResponse = await client.PatchAsJsonAsync($"/projects/999", updateProject); // Assuming 999 does not exist
        Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
    }
    
        [Fact]
    public async Task DeleteProject_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/projects/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]

    public async Task DeleteProject_ReturnsNoContent_WhenProjectExists()
    {   
        using var factory = new TaskFlowApiFactory();
        using var client = factory.CreateClient();

        var newProject = new Project
        {
            Title = "Test title",
            Description = "This is a test project."
        };

        var response = await client.PostAsJsonAsync("/projects/", newProject);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdProject = await response.Content.ReadFromJsonAsync<Project>();
        Assert.NotNull(createdProject);    
        var getResponse = await client.GetAsync($"/projects/{createdProject.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/projects/{createdProject.id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

    }

}



