﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolIS.DAL;

#nullable disable

namespace SchoolIS.DAL.Migrations
{
    [DbContext(typeof(SchoolISDbContext))]
    [Migration("20240302132824_AddedUserAttributes")]
    partial class AddedUserAttributes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("SchoolIS.DAL.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ActivityType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndsAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartsFrom")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("RoomId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.EnrollsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Enrolls");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.EvaluationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<uint?>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("StudentId");

                    b.ToTable("Evaluation");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.SubjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Info")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Shortcut")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.TeachesEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Teaches");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.ActivityEntity", b =>
                {
                    b.HasOne("SchoolIS.DAL.Entities.UserEntity", "Creator")
                        .WithMany("Activities")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolIS.DAL.Entities.RoomEntity", "Room")
                        .WithMany("Activities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SchoolIS.DAL.Entities.SubjectEntity", "Subject")
                        .WithMany("Activities")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Room");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.EnrollsEntity", b =>
                {
                    b.HasOne("SchoolIS.DAL.Entities.UserEntity", "Student")
                        .WithMany("Enrolls")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolIS.DAL.Entities.SubjectEntity", "Subject")
                        .WithMany("Enrolls")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.EvaluationEntity", b =>
                {
                    b.HasOne("SchoolIS.DAL.Entities.ActivityEntity", "Activity")
                        .WithMany("Evaluations")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolIS.DAL.Entities.UserEntity", "Student")
                        .WithMany("Evaluations")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.TeachesEntity", b =>
                {
                    b.HasOne("SchoolIS.DAL.Entities.SubjectEntity", "Subject")
                        .WithMany("Teachers")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolIS.DAL.Entities.UserEntity", "Teacher")
                        .WithMany("Teaches")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.ActivityEntity", b =>
                {
                    b.Navigation("Evaluations");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.RoomEntity", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.SubjectEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Enrolls");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("SchoolIS.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Enrolls");

                    b.Navigation("Evaluations");

                    b.Navigation("Teaches");
                });
#pragma warning restore 612, 618
        }
    }
}
