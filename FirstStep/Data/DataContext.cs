
using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Data
{
    public class DataContext : DbContext
    {        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Advertisement> Advertisements { get; set; } = null!;

        public DbSet<Application> Applications { get; set; } = null!;

        public DbSet<SeekerSkill> Seekerskill { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        
        public DbSet<Company> Companys { get; set; } = null!;
        
        public DbSet<JobField> JobFields { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Seeker> Seekers { get; set; } = null!;

        public DbSet<HRManager> HRManagers { get; set; } = null!;

        public DbSet<HRAssistant> HRAssistants { get; set; } = null!;

        public DbSet<CompanyAdmin> CompanyAdmins { get; set; } = null!;

        public DbSet<SystemAdmin> SystemAdmins { get; set; } = null!;

        public DbSet<RegisteredCompany> RegisteredCompanys { get; set; } = null!;

        public DbSet<ProfessionKeyword> ProfessionKeywords { get; set; } = null!;

        public DbSet<Revision> Revisions { get; set; } = null!;


        //public DbSet<Advertisement_Seeker> AdvertisementSeekers { get; set; } = null!;

        //public DbSet<Advertisement_ProfessionKeyword> AdvertisementProfessionKeywords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seeker>().ToTable("Seekers");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<HRManager>().ToTable("HRManagers");
            modelBuilder.Entity<HRAssistant>().ToTable("HRAssistants");
            modelBuilder.Entity<CompanyAdmin>().ToTable("CompanyAdmins");
            modelBuilder.Entity<SystemAdmin>().ToTable("SystemAdmins");
            modelBuilder.Entity<RegisteredCompany>().ToTable("RegisteredCompanys");

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
                    
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.regCompany)
                    .WithMany(e => e.employees)
                    .HasForeignKey(e => e.company_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<ProfessionKeyword>(entity =>
            {
                entity.HasOne(e => e.job_Field)
                    .WithMany(e => e.professionKeywords)
                    .HasForeignKey(e => e.field_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            /*
            modelBuilder.Entity<Advertisement_Seeker>(entity =>
            {
                entity.HasKey(e => new {e.advertisement_id, e.seeker_id});

                entity.HasOne(e => e.advertisement)
                    .WithMany(e => e.advertisement_seekers)
                    .HasForeignKey(e => e.advertisement_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.seeker)
                    .WithMany(e => e.advertisement_seekers)
                    .HasForeignKey(e => e.seeker_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
            /*
            
            modelBuilder.Entity<Advertisement_ProfessionKeyword>(entity =>
            {
                entity.HasKey(e => new {e.advertisement_id, e.profession_id});
                               
                entity.HasOne(e => e.advertisement)
                    .WithMany(e => e.advertisement_professionKeywords)
                    .HasForeignKey(e => e.advertisement_id)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(e => e.professionKeyword)
                    .WithMany(e => e.advertisement_professionKeywords)
                    .HasForeignKey(e => e.profession_id)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });*/
        }
    }
}
