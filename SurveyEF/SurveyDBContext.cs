using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF
{
    public class SurveyDBContext : IdentityDbContext<IdentityUser>
    {
        /*public SurveyDBContext(DbContextOptions<SurveyDBContext> options) : base(options)
        {
        }*/
        public DbSet<SurveyEntity> Surveys { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleQuestions { get; set; }
        public DbSet<TextFieldQuestion> TextFieldQuestions { get; set; }

        //public DbSet<OptionEntity> Options { get; set; }

        public DbSet<UserAnswerEntity> UserAnswers { get; set; }
        public DbSet<MultipleChoiceAnswerEntity> MultipleAnswers { get; set; }
        public DbSet<TextFieldAnswerEntity> TextFieldAnswers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                @"DATA SOURCE=DESKTOP\SQLSERVER;DATABASE=TestDB;Integrated Security=true;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Question>().HasDiscriminator<string>("QuestionType")
                .HasValue<MultipleChoiceQuestion>("MultipleChoice")
                .HasValue<TextFieldQuestion>("TextField")
                .HasValue<LikertScaleQuestion>("LikertScale")
                .HasValue<RankingQuestion>("Ranking");

            modelBuilder.Entity<UserAnswerEntity>().HasDiscriminator<string>("AnswerType")
                .HasValue<TextFieldAnswerEntity>("TextField")
                .HasValue<MultipleChoiceAnswerEntity>("MultipleChoice")
                .HasValue<LikertScaleAnswerEntity>("LikerScale")
                .HasValue<RankingAnswerEntity>("Ranking");

            
            modelBuilder.Entity<SurveyEntity>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .HasForeignKey(q => q.SurveyId);

            /*modelBuilder.Entity<MultipleChoiceQuestion>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId);*/

            /*
            modelBuilder.Entity<RankingQuestion>()
                .HasMany(r => r.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.RankOptionId); */

            modelBuilder.Entity<Question>()
                .HasIndex(q => new { q.QuestionId, q.SurveyId })
                .IsUnique();

            /*modelBuilder.Entity<OptionEntity>()
                .HasIndex(o => new { o.OptionId, o.QuestionId })
                .IsUnique();*/

            /*
            modelBuilder.Entity<RankingOptionEntity>()
                .HasIndex(ro => new { ro.RankOptionId, ro.QuestionId })
                .IsUnique(); */
            
            modelBuilder.Entity<SurveyEntity>()
                .HasMany(s => s.UserAnswers)
                .WithOne(a => a.Survey)
                .HasForeignKey(a => a.SurveyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.UserAnswers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            /*
            modelBuilder.Entity<RankingOptionEntity>()
                .HasMany(ro => ro.Answers)
                .WithMany();
            */

            /*modelBuilder.Entity<OptionEntity>()
                .HasMany(o => o.Answers)
                .WithOne(a => a.AnswerOption)
                .HasForeignKey(a => a.OptionId)
                .OnDelete(DeleteBehavior.Restrict);*/

        }
    }
}
