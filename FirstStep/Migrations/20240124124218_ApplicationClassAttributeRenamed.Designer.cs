﻿// <auto-generated />
using System;
using FirstStep.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirstStep.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240124124218_ApplicationClassAttributeRenamed")]
    partial class ApplicationClassAttributeRenamed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("FirstStep.Models.Advertisement", b =>
                {
                    b.Property<int>("advertisement_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("advertisement_id"));

                    b.Property<string>("arrangement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("current_status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("employeement_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("field_id")
                        .HasColumnType("int");

                    b.Property<int>("hrManager_id")
                        .HasColumnType("int");

                    b.Property<bool>("is_experience_required")
                        .HasColumnType("bit");

                    b.Property<string>("job_benefits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("job_number")
                        .HasColumnType("int");

                    b.Property<string>("job_other_details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("job_overview")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("job_qualifications")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("job_responsibilities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("location_city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("location_province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("posted_date")
                        .HasColumnType("datetime2");

                    b.Property<float>("salary")
                        .HasColumnType("real");

                    b.Property<DateTime>("submission_deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("advertisement_id");

                    b.HasIndex("field_id");

                    b.HasIndex("hrManager_id");

                    b.ToTable("Advertisements");
                });

            modelBuilder.Entity("FirstStep.Models.Advertisement_Seeker", b =>
                {
                    b.Property<int>("advertisement_id")
                        .HasColumnType("int");

                    b.Property<int>("seeker_id")
                        .HasColumnType("int");

                    b.HasKey("advertisement_id", "seeker_id");

                    b.HasIndex("seeker_id");

                    b.ToTable("AdvertisementSeekers");
                });

            modelBuilder.Entity("FirstStep.Models.Application", b =>
                {
                    b.Property<string>("applicationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("reviewDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("submittedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("applicationId");

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

                    b.Property<string>("company_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("company_phone_number")
                        .HasColumnType("int");

                    b.Property<string>("company_website")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("verification_status")
                        .HasColumnType("bit");

                    b.HasKey("company_id");

                    b.ToTable("Companys");

                    b.UseTptMappingStrategy();
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

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("FirstStep.Models.RegisteredCompany", b =>
                {
                    b.HasBaseType("FirstStep.Models.Company");

                    b.Property<string>("company_business_scale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("company_province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("company_registered_date")
                        .HasColumnType("datetime2");

                    b.ToTable("RegisteredCompanys", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.Employee", b =>
                {
                    b.HasBaseType("FirstStep.Models.User");

                    b.Property<int>("company_id")
                        .HasColumnType("int");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("linkedin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("phone_number")
                        .HasColumnType("int");

                    b.Property<string>("profile_picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("university")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Seekers", (string)null);
                });

            modelBuilder.Entity("FirstStep.Models.SystemAdmin", b =>
                {
                    b.HasBaseType("FirstStep.Models.User");

                    b.ToTable("SystemAdmins");
                });

            modelBuilder.Entity("FirstStep.Models.HRManager", b =>
                {
                    b.HasBaseType("FirstStep.Models.Employee");

                    b.ToTable("HRManagers", (string)null);
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

            modelBuilder.Entity("FirstStep.Models.Advertisement_Seeker", b =>
                {
                    b.HasOne("FirstStep.Models.Advertisement", "advertisement")
                        .WithMany("advertisement_seekers")
                        .HasForeignKey("advertisement_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.Seeker", "seeker")
                        .WithMany("advertisement_seekers")
                        .HasForeignKey("seeker_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("advertisement");

                    b.Navigation("seeker");
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

            modelBuilder.Entity("FirstStep.Models.RegisteredCompany", b =>
                {
                    b.HasOne("FirstStep.Models.Company", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.RegisteredCompany", "company_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.Employee", b =>
                {
                    b.HasOne("FirstStep.Models.RegisteredCompany", "regCompany")
                        .WithMany("employees")
                        .HasForeignKey("company_id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.Employee", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("regCompany");
                });

            modelBuilder.Entity("FirstStep.Models.Seeker", b =>
                {
                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.Seeker", "user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstStep.Models.SystemAdmin", b =>
                {
                    b.HasOne("FirstStep.Models.User", null)
                        .WithOne()
                        .HasForeignKey("FirstStep.Models.SystemAdmin", "user_id")
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
                    b.Navigation("advertisement_seekers");
                });

            modelBuilder.Entity("FirstStep.Models.JobField", b =>
                {
                    b.Navigation("advertisements");

                    b.Navigation("professionKeywords");
                });

            modelBuilder.Entity("FirstStep.Models.RegisteredCompany", b =>
                {
                    b.Navigation("employees");
                });

            modelBuilder.Entity("FirstStep.Models.Seeker", b =>
                {
                    b.Navigation("advertisement_seekers");
                });

            modelBuilder.Entity("FirstStep.Models.HRManager", b =>
                {
                    b.Navigation("advertisements");
                });
#pragma warning restore 612, 618
        }
    }
}
