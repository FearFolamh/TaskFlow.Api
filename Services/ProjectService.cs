using TaskFlow.Api.Data;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Services;

    public class ProjectService
{
    private readonly AppDbContext _db;
    public ProjectService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Project>> GetAllAsync()
    {
        return await _db.Projects.ToListAsync();
    }
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _db.Projects.FindAsync(id);
    }
    public async Task<Project> CreateAsync(Project newProject)
    {
        _db.Projects.Add(newProject);
        await _db.SaveChangesAsync();
        return newProject;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true; 
    }

    public async Task<Project?> UpdateAsync(int id, UpdateProjectRequest UpdateProjectRequest)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return null;
        }
        project.Title = UpdateProjectRequest.Title;
        project.Description = UpdateProjectRequest.Description;
        await _db.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> PatchAsync(int id, UpdateProjectRequest  UpdateProjectRequest )
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
        {
            return null;
        }
        if (UpdateProjectRequest .Title != null)
        {
            project.Title = UpdateProjectRequest .Title;
        }
        if (UpdateProjectRequest .Description != null)
        {
            project.Description = UpdateProjectRequest .Description;
        }
        await _db.SaveChangesAsync();
        return project;
    }
}
