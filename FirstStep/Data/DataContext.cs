
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Data
{
    public class DataContext : DbContext
    {        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Advertisement> Advertisements { get; set; } = null!;

        public DbSet<Application> Applications { get; set; } = null!;

        public DbSet<Skill> Skills { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        
        public DbSet<Company> Companies { get; set; } = null!;
        
        public DbSet<JobField> JobFields { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Seeker> Seekers { get; set; } = null!;

        public DbSet<HRManager> HRManagers { get; set; } = null!;

        public DbSet<HRAssistant> HRAssistants { get; set; } = null!;

        public DbSet<SystemAdmin> SystemAdmins { get; set; } = null!;

        public DbSet<ProfessionKeyword> ProfessionKeywords { get; set; } = null!;

        public DbSet<Revision> Revisions { get; set; } = null!;

        public DbSet<Appointment> Appointments { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seeker>().ToTable("Seekers");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<SystemAdmin>().ToTable("SystemAdmins");

            modelBuilder.Entity<Advertisement>(entity => 
            {
                entity.HasOne(e => e.job_Field)
                    .WithMany(e => e.advertisements)
                    .HasForeignKey(e => e.field_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
                
                entity.HasOne(e => e.hrManager)
                    .WithMany(e => e.advertisements)
                    .HasForeignKey(e => e.hrManager_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasMany(e => e.professionKeywords)
                    .WithMany(e => e.advertisements)
                    .UsingEntity(e => e.ToTable("AdvertisementProfessionKeywords"));

                entity.HasMany(e => e.savedSeekers)
                    .WithMany(e => e.savedAdvertisemnts)
                    .UsingEntity(e => e.ToTable("AdvertisementSeekers"));

                entity.HasMany(e => e.skills)
                    .WithMany(e => e.advertisements)
                    .UsingEntity(e => e.ToTable("AdvertisementSkills"));
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.company)
                    .WithMany(e => e.employees)
                    .HasForeignKey(e => e.company_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<HRManager>(entity =>
            {
                entity.HasOne(e => e.admin_company)
                    .WithOne(e => e.company_admin)
                    .HasForeignKey<Company>(e => e.company_admin_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<ProfessionKeyword>(entity =>
            {
                entity.HasOne(e => e.job_Field)
                    .WithMany(e => e.professionKeywords)
                    .HasForeignKey(e => e.field_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasOne(e => e.verified_system_admin)
                    .WithMany(e => e.verified_companies)
                    .HasForeignKey(e => e.verified_system_admin_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Seeker>(entity =>
            {
                entity.HasOne(e => e.job_Field)
                    .WithMany(e => e.seekers)
                    .HasForeignKey(e => e.field_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasMany(e => e.skills)
                    .WithMany(e => e.seekers)
                    .UsingEntity(e => e.ToTable("SeekerSkills"));
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasOne(e => e.advertisement)
                    .WithMany(e => e.applications)
                    .HasForeignKey(e => e.advertisement_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.seeker)
                    .WithMany(e => e.applications)
                    .HasForeignKey(e => e.seeker_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.assigned_hrAssistant)
                    .WithMany(e => e.applications)
                    .HasForeignKey(e => e.assigned_hrAssistant_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Revision>(entity =>
            {
                entity.HasOne(e => e.application)
                    .WithMany(e => e.revisions)
                    .HasForeignKey(e => e.application_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.employee)
                    .WithMany(e => e.revisions)
                    .HasForeignKey(e => e.employee_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasOne(e => e.company)
                    .WithMany(e => e.appointments)
                    .HasForeignKey(e => e.company_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.advertisement)
                    .WithMany(e => e.appointments)
                    .HasForeignKey(e => e.advertisement_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.seeker)
                    .WithMany(e => e.appointments)
                    .HasForeignKey(e => e.seeker_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}
