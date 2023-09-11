using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF.Entities
{

    public class OptionEntity
    {
        [Key]
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int OptionOrder { get; set; }
        public int QuestionId { get; set; }
        public virtual MultipleChoiceQuestion Question { get; set; }
        public virtual ICollection<MultipleChoiceAnswerEntity> Answers { get; set; } = new HashSet<MultipleChoiceAnswerEntity>();
        
    }
    /*
    public class RankingOptionEntity 
    {
        [Key]
        public int RankOptionId { get; set; }
        public string OptionText { get; set; }
        public int OptionOrder { get; set; }
        public int QuestionId { get; set; }
        public virtual RankingQuestion Question { get; set; }
        public virtual ICollection<RankingAnswerEntity> Answers { get; set; } = new HashSet<RankingAnswerEntity>();
    } */
}
