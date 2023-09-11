﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurveyEF;

#nullable disable

namespace SurveyEF.Migrations
{
    [DbContext(typeof(SurveyDBContext))]
    [Migration("20230731172227_UserAnswerEntityAdded")]
    partial class UserAnswerEntityAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SurveyEF.Entities.OptionEntity", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OptionId"));

                    b.Property<string>("OptionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("OptionId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("OptionId", "QuestionId")
                        .IsUnique();

                    b.ToTable("Options");
                });

            modelBuilder.Entity("SurveyEF.Entities.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.HasIndex("SurveyId");

                    b.HasIndex("QuestionId", "SurveyId")
                        .IsUnique();

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("QuestionType").HasValue("Question");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SurveyEF.Entities.SurveyEntity", b =>
                {
                    b.Property<int>("SurveyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SurveyId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SurveyId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("SurveyEF.Entities.UserAnswerEntity", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerId"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SurveyId");

                    b.ToTable("UserAnswers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UserAnswerEntity");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SurveyEF.Entities.MultipleChoiseQuestion", b =>
                {
                    b.HasBaseType("SurveyEF.Entities.Question");

                    b.HasDiscriminator().HasValue("MultipleChoice");
                });

            modelBuilder.Entity("SurveyEF.Entities.TextFieldQuestion", b =>
                {
                    b.HasBaseType("SurveyEF.Entities.Question");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("TextField");
                });

            modelBuilder.Entity("SurveyEF.Entities.MultipleChoiseAnswerEntity", b =>
                {
                    b.HasBaseType("SurveyEF.Entities.UserAnswerEntity");

                    b.Property<int>("AnswerOptionOptionId")
                        .HasColumnType("int");

                    b.Property<int>("OptionId")
                        .HasColumnType("int");

                    b.HasIndex("AnswerOptionOptionId");

                    b.HasDiscriminator().HasValue("MultipleChoiseAnswerEntity");
                });

            modelBuilder.Entity("SurveyEF.Entities.OptionEntity", b =>
                {
                    b.HasOne("SurveyEF.Entities.MultipleChoiseQuestion", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("SurveyEF.Entities.Question", b =>
                {
                    b.HasOne("SurveyEF.Entities.SurveyEntity", "Survey")
                        .WithMany("Questions")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("SurveyEF.Entities.UserAnswerEntity", b =>
                {
                    b.HasOne("SurveyEF.Entities.Question", "Question")
                        .WithMany("UserAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SurveyEF.Entities.SurveyEntity", "Survey")
                        .WithMany("UserAnswers")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("SurveyEF.Entities.MultipleChoiseAnswerEntity", b =>
                {
                    b.HasOne("SurveyEF.Entities.OptionEntity", "AnswerOption")
                        .WithMany()
                        .HasForeignKey("AnswerOptionOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnswerOption");
                });

            modelBuilder.Entity("SurveyEF.Entities.Question", b =>
                {
                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("SurveyEF.Entities.SurveyEntity", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("SurveyEF.Entities.MultipleChoiseQuestion", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
