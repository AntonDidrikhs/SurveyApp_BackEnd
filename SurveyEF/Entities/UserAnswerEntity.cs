using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF.Entities
{
    
    public abstract class UserAnswerEntity
    {
        [Key]
        public int AnswerId { get; set; }
        public string UserEmail { get; set; }
        public int SurveyId { get; set; }
        public virtual SurveyEntity Survey { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

    }
    public class MultipleChoiceAnswerEntity : UserAnswerEntity
    {
        public string Answer { get; set; }
        //public int OptionId { get; set; }
        //public virtual OptionEntity AnswerOption { get; set; }
    }
    public class TextFieldAnswerEntity : UserAnswerEntity
    {
        public string Answer { get; set; }
    }
    public class LikertScaleAnswerEntity : UserAnswerEntity
    {
        public string Answer { get; set; }
    }
    public class RankingAnswerEntity : UserAnswerEntity 
    {
        public string JsonAnswer { get; set; }
        //public virtual OptionEntity  
    }
}
