using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;
using System.IO;

namespace TodoApp.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        try 
        {
            var currentDir = Directory.GetCurrentDirectory();
            var solutionDir = Path.GetFullPath(Path.Combine(currentDir, ".."));
            
            var envPath = Path.Combine(solutionDir, ".env");
            Console.WriteLine($"Looking for .env at: {envPath}");
            
            if (File.Exists(envPath))
            {
                Console.WriteLine(".env file found!");
                // Print .env content
                Console.WriteLine("ENV file content:");
                Console.WriteLine(File.ReadAllText(envPath));
            }
            else
            {
                Console.WriteLine(".env file NOT found!");
            }
            
            Env.Load(envPath);
            
            var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
            Console.WriteLine($"Connection String after loading: {connectionString ?? "NULL"}");
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DesignTimeDbContextFactory: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}