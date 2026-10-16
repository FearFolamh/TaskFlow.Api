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
    
};



app.MapGet("/projects", () => Results.Ok(projects));
/*{  
    return projects;
})
.WithName("GetProjects");
*/

app.MapPost("/projects", (Project newProject) =>
{
    var project = new Project(projects.Select(p => p.id).DefaultIfEmpty(0).Max() + 1, newProject.Title, newProject.Description);
    projects = projects.Append(project).ToArray();
    return Results.Created($"/projects/{project.id}", project);
    
})
.WithName("PostProjects");



app.Run();

record Project(int id, string? Title, string? Description)
{
   
}
