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
            .HasForeignKey(u => u.JobStatusId);

        modelBuilder = SetJobStatuses(modelBuilder);
    }

    private static ModelBuilder SetJobStatuses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobStatusModel>().HasData(
            new JobStatusModel { JobStatusId = 1, StatusName = "N/A" },
            new JobStatusModel { JobStatusId = 2, StatusName = "Applied" },
            new JobStatusModel { JobStatusId = 3, StatusName = "Rejection" },
            new JobStatusModel { JobStatusId = 4, StatusName = "Holding CV" },
            new JobStatusModel { JobStatusId = 5, StatusName = "Awaiting Response" },
            new JobStatusModel { JobStatusId = 6, StatusName = "Scheduled Phone Call" },
            new JobStatusModel { JobStatusId = 7, StatusName = "Screening/ Pre-Interview" },
            new JobStatusModel { JobStatusId = 8, StatusName = "Interview" },
            new JobStatusModel { JobStatusId = 9, StatusName = "Offer" }
        );

        return modelBuilder;
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
