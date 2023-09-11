using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF.Entities
{
    public abstract class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public virtual SurveyEntity Survey { get; set; }
        public string Text { get; set; }
        public bool IsRequired { get; set; }
        //public string QuestionType { get; set; }
        public virtual ICollection<UserAnswerEntity> UserAnswers { get; set; } = new HashSet<UserAnswerEntity>();
    }
    public class MultipleChoiceQuestion : Question
    {
        public string OptionsJson { get; set; }
        //public virtual ICollection<OptionEntity> Options { get; set; } = new HashSet<OptionEntity>();
        
    }
    public class TextFieldQuestion : Question
    {
        //public string Answer { get; set; }
        
    }
    public class LikertScaleQuestion : Question
    {
        //public int LikertScore { get; set; }
        
    }
    public class RankingQuestion : Question
    {
        public string OptionsJson { get; set; }
    }
}
