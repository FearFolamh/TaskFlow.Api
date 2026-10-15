using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


Project[] projects = new Project[]   
{
    new Project(1, "Project 1", "Description for Project 1"),
    new Project(2, "Project 2", "Description for Project 2"),
    new Project(3, "Project 3", "Description for Project 3")
};

app.MapGet("/projects", () => Results.Ok(projects));
/*{  
    return projects;
})
.WithName("GetProjects");
*/

app.MapPost("/projects", (Project newProject) =>
{
    var project = new Project(projects.Length + 1, newProject.Title, newProject.Description);
    projects = projects.Append(project).ToArray();

    
})
.WithName("PostProjects");




app.Run();

record Project(int id, string? Title, string? Description)
{
   
}
