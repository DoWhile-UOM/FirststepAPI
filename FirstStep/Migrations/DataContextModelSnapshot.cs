﻿// <auto-generated />
using System;
using FirstStep.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Firststep.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AdvertisementProfessionKeyword", b =>
                {
                    b.Property<int>("advertisementsadvertisement_id")
                        .HasColumnType("int");

                    b.Property<int>("professionKeywordsprofession_id")
                        .HasColumnType("int");

                    b.HasKey("advertisementsadvertisement_id", "professionKeywordsprofession_id");

                    b.HasIndex("professionKeywordsprofession_id");

                    b.ToTable("AdvertisementProfessionKeywords", (string)null);
                });

            modelBuilder.Entity("AdvertisementSeeker", b =>
                {
                    b.Property<int>("savedAdvertisemntsadvertisement_id")
                        .HasColumnType("int");

                    b.Property<int>("savedSeekersuser_id")
                        .HasColumnType("int");

                    b.HasKey("savedAdvertisemntsadvertisement_id", "savedSeekersuser_id");

                    b.HasIndex("savedSeekersuser_id");

                    b.ToTable("AdvertisementSeekers", (string)null);
                });

            modelBuilder.Entity("AdvertisementSkill", b =>
                {
                    b.Property<int>("advertisementsadvertisement_id")
                        .HasColumnType("int");

                    b.Property<int>("skillsskill_id")
                        .HasColumnType("int");

                    b.HasKey("advertisementsadvertisement_id", "skillsskill_id");

                    b.HasIndex("skillsskill_id");

                    b.ToTable("AdvertisementSkills", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.Advertisement", b =>
                {
                    b.Property<int>("advertisement_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("advertisement_id"));

                    b.Property<string>("arrangement")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("country")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("currency_unit")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("current_status")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("employeement_type")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("experience")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime?>("expired_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("field_id")
                        .HasColumnType("int");

                    b.Property<int>("hrManager_id")
                        .HasColumnType("int");

                    b.Property<string>("job_description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("job_number")
                        .HasColumnType("int");

                    b.Property<DateTime>("posted_date")
                        .HasColumnType("datetime2");

                    b.Property<float?>("salary")
                        .HasColumnType("real");

                    b.Property<DateTime?>("submission_deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("advertisement_id");

                    b.HasIndex("field_id");

                    b.HasIndex("hrManager_id");

                    b.ToTable("Advertisements");
                });

            modelBuilder.Entity("FirstStep.Models.Application", b =>
                {
                    b.Property<int>("application_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("application_Id"));

                    b.Property<string>("CVurl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("advertisement_id")
                        .HasColumnType("int");

                    b.Property<int?>("assigned_hrAssistant_id")
                        .HasColumnType("int");

                    b.Property<string>("doc1_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("doc2_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("seeker_id")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("submitted_date")
                        .HasColumnType("datetime2");

                    b.HasKey("application_Id");

                    b.HasIndex("advertisement_id");

                    b.HasIndex("assigned_hrAssistant_id");

                    b.HasIndex("seeker_id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("FirstStep.Models.Company", b =>
                {
                    b.Property<int>("company_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("company_id"));

                    b.Property<string>("business_reg_certificate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("business_reg_no")
                        .HasColumnType("int");

                    b.Property<string>("certificate_of_incorporation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("company_admin_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("company_applied_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("company_business_scale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_city")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("company_phone_number")
                        .HasColumnType("int");

                    b.Property<string>("company_province")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("company_registered_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("company_website")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("registration_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("verification_status")
                        .HasColumnType("bit");

                    b.Property<int?>("verified_system_admin_id")
                        .HasColumnType("int");

                    b.HasKey("company_id");

                    b.HasIndex("company_admin_id")
                        .IsUnique()
                        .HasFilter("[company_admin_id] IS NOT NULL");

                    b.HasIndex("verified_system_admin_id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("FirstStep.Models.JobField", b =>
                {
                    b.Property<int>("field_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("field_id"));

                    b.Property<string>("field_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("field_id");

                    b.ToTable("JobFields");
                });

            modelBuilder.Entity("FirstStep.Models.OTPRequest", b =>
                {
                    b.Property<string>("email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("expiry_date_time")
                        .HasColumnType("datetime2");

                    b.Property<int>("otp")
                        .HasColumnType("int");

                    b.HasKey("email");

                    b.ToTable("OTPRequests");
                });

            modelBuilder.Entity("FirstStep.Models.ProfessionKeyword", b =>
                {
                    b.Property<int>("profession_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("profession_id"));

                    b.Property<int>("field_id")
                        .HasColumnType("int");

                    b.Property<string>("profession_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("profession_id");

                    b.HasIndex("field_id");

                    b.ToTable("ProfessionKeywords");
                });

            modelBuilder.Entity("FirstStep.Models.Revision", b =>
                {
                    b.Property<int>("revision_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("revision_id"));

                    b.Property<int>("application_id")
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("employee_id")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("revision_id");

                    b.HasIndex("application_id");

                    b.HasIndex("employee_id");

                    b.ToTable("Revisions");
                });

            modelBuilder.Entity("FirstStep.Models.Skill", b =>
                {
                    b.Property<int>("skill_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("skill_id"));

                    b.Property<string>("skill_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("skill_id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("FirstStep.Models.User", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("user_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password_hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("refresh_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("refresh_token_expiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("SeekerSkill", b =>
                {
                    b.Property<int>("seekersuser_id")
                        .HasColumnType("int");

                    b.Property<int>("skillsskill_id")
                        .HasColumnType("int");

                    b.HasKey("seekersuser_id", "skillsskill_id");

                    b.HasIndex("skillsskill_id");

                    b.ToTable("SeekerSkills", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.Employee", b =>
                {
                    b.HasBaseType("FirstStep.Models.User");

                    b.Property<int>("company_id")
                        .HasColumnType("int");

                    b.HasIndex("company_id");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.Seeker", b =>
                {
                    b.HasBaseType("FirstStep.Models.User");

                    b.Property<string>("CVurl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("field_id")
                        .HasColumnType("int");

                    b.Property<string>("linkedin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("phone_number")
                        .HasColumnType("int");

                    b.Property<string>("profile_picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("university")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("field_id");

                    b.ToTable("Seekers", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.SystemAdmin", b =>
                {
                    b.HasBaseType("FirstStep.Models.User");

                    b.ToTable("SystemAdmins", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.HRAssistant", b =>
                {
                    b.HasBaseType("FirstStep.Models.Employee");

                    b.ToTable("HRAssistants");
                });

            modelBuilder.Entity("FirstStep.Models.HRManager", b =>
                {
                    b.HasBaseType("FirstStep.Models.Employee");

                    b.ToTable("HRManagers");
                });

            modelBuilder.Entity("AdvertisementProfessionKeyword", b =>
                {
                    b.HasOne("FirstStep.Models.Advertisement", null)
                        .WithMany()
                        .HasForeignKey("advertisementsadvertisement_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.ProfessionKeyword", null)
                        .WithMany()
                        .HasForeignKey("professionKeywordsprofession_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdvertisementSeeker", b =>
                {
                    b.HasOne("FirstStep.Models.Advertisement", null)
                        .WithMany()
                        .HasForeignKey("savedAdvertisemntsadvertisement_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.Seeker", null)
                        .WithMany()
                        .HasForeignKey("savedSeekersuser_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdvertisementSkill", b =>
                {
                    b.HasOne("FirstStep.Models.Advertisement", null)
                        .WithMany()
                        .HasForeignKey("advertisementsadvertisement_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.Skill", null)
                        .WithMany()
                        .HasForeignKey("skillsskill_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.Advertisement", b =>
                {
                    b.HasOne("FirstStep.Models.JobField", "job_Field")
                        .WithMany("advertisements")
                        .HasForeignKey("field_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.HRManager", "hrManager")
                        .WithMany("advertisements")
                        .HasForeignKey("hrManager_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("hrManager");

                    b.Navigation("job_Field");
                });

            modelBuilder.Entity("FirstStep.Models.Application", b =>
                {
                    b.HasOne("FirstStep.Models.Advertisement", "advertisement")
                        .WithMany("applications")
                        .HasForeignKey("advertisement_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.HRAssistant", "assigned_hrAssistant")
                        .WithMany("applications")
                        .HasForeignKey("assigned_hrAssistant_id")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("FirstStep.Models.Seeker", "seeker")
                        .WithMany("applications")
                        .HasForeignKey("seeker_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("advertisement");

                    b.Navigation("assigned_hrAssistant");

                    b.Navigation("seeker");
                });

            modelBuilder.Entity("FirstStep.Models.Company", b =>
                {
                    b.HasOne("FirstStep.Models.HRManager", "company_admin")
                        .WithOne("admin_company")
                        .HasForeignKey("FirstStep.Models.Company", "company_admin_id")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("FirstStep.Models.SystemAdmin", "verified_system_admin")
                        .WithMany("verified_companies")
                        .HasForeignKey("verified_system_admin_id")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("company_admin");

                    b.Navigation("verified_system_admin");
                });

            modelBuilder.Entity("FirstStep.Models.ProfessionKeyword", b =>
                {
                    b.HasOne("FirstStep.Models.JobField", "job_Field")
                        .WithMany("professionKeywords")
                        .HasForeignKey("field_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("job_Field");
                });

            modelBuilder.Entity("FirstStep.Models.Revision", b =>
                {
                    b.HasOne("FirstStep.Models.Application", "application")
                        .WithMany("revisions")
                        .HasForeignKey("application_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.Employee", "employee")
                        .WithMany("revisions")
                        .HasForeignKey("employee_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("application");

                    b.Navigation("employee");
                });

            modelBuilder.Entity("SeekerSkill", b =>
                {
                    b.HasOne("FirstStep.Models.Seeker", null)
                        .WithMany()
                        .HasForeignKey("seekersuser_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.Skill", null)
                        .WithMany()
                        .HasForeignKey("skillsskill_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.Employee", b =>
                {
                    b.HasOne("FirstStep.Models.Company", "company")
                        .WithMany("employees")
                        .HasForeignKey("company_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.Employee", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("company");
                });

            modelBuilder.Entity("FirstStep.Models.Seeker", b =>
                {
                    b.HasOne("FirstStep.Models.JobField", "job_Field")
                        .WithMany("seekers")
                        .HasForeignKey("field_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.Seeker", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("job_Field");
                });

            modelBuilder.Entity("FirstStep.Models.SystemAdmin", b =>
                {
                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.SystemAdmin", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.HRAssistant", b =>
                {
                    b.HasOne("FirstStep.Models.Employee", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.HRAssistant", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.HRManager", b =>
                {
                    b.HasOne("FirstStep.Models.Employee", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.HRManager", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.Advertisement", b =>
                {
                    b.Navigation("applications");
                });

            modelBuilder.Entity("FirstStep.Models.Application", b =>
                {
                    b.Navigation("revisions");
                });

            modelBuilder.Entity("FirstStep.Models.Company", b =>
                {
                    b.Navigation("employees");
                });

            modelBuilder.Entity("FirstStep.Models.JobField", b =>
                {
                    b.Navigation("advertisements");

                    b.Navigation("professionKeywords");

                    b.Navigation("seekers");
                });

            modelBuilder.Entity("FirstStep.Models.Employee", b =>
                {
                    b.Navigation("revisions");
                });

            modelBuilder.Entity("FirstStep.Models.Seeker", b =>
                {
                    b.Navigation("applications");
                });

            modelBuilder.Entity("FirstStep.Models.SystemAdmin", b =>
                {
                    b.Navigation("verified_companies");
                });

            modelBuilder.Entity("FirstStep.Models.HRAssistant", b =>
                {
                    b.Navigation("applications");
                });

            modelBuilder.Entity("FirstStep.Models.HRManager", b =>
                {
                    b.Navigation("admin_company");

                    b.Navigation("advertisements");
                });
#pragma warning restore 612, 618
        }
    }
}
