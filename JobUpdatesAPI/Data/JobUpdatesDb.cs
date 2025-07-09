using Microsoft.EntityFrameworkCore;
using JobUpdatesAPI.Models;

namespace JobUpdatesAPI.Data;

public class JobUpdatesDbContext : DbContext
{
    public JobUpdatesDbContext(DbContextOptions<JobUpdatesDbContext> options)
    : base(options) { }

    

    public DbSet<JobModel> Jobs { get; set; } = null!;
    public DbSet<JobUpdateModel> JobUpdates { get; set; } = null!;
    public DbSet<JobStatusModel> JobStatuses { get; set; } = null!;
    public DbSet<JobKeywordModel> JobKeywords { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobUpdateModel>()
            .HasOne(j => j.Job)
            .WithMany(j => j.JobUpdates)
            .HasForeignKey(j => j.JobId);

        modelBuilder = SetJobKeywords(modelBuilder);

        modelBuilder.Entity<JobUpdateModel>()
            .HasOne(u => u.Status)
            .WithMany(s => s.JobUpdates)
            .HasForeignKey(u => u.JobStatusModelId);

        modelBuilder.Entity<JobStatusModel>().HasData(
            new JobStatusModel { JobStatusId = 1, StatusName = "Pending" },
            new JobStatusModel { JobStatusId = 2, StatusName = "InProgress" },
            new JobStatusModel { JobStatusId = 3, StatusName = "Completed" }
        );


    }

    private static ModelBuilder SetJobKeywords(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobKeywordModel>()
            .HasKey(jk => new { jk.JobId, jk.KeywordId }); // Composite key

        modelBuilder.Entity<JobKeywordModel>()
            .HasOne(jk => jk.Job)
            .WithMany(j => j.JobKeywords)
            .HasForeignKey(jk => jk.JobId);

        modelBuilder.Entity<JobKeywordModel>()
            .HasOne(jk => jk.Keyword)
            .WithMany(k => k.JobKeywords)
            .HasForeignKey(jk => jk.KeywordId);

        return modelBuilder;
    }
}
