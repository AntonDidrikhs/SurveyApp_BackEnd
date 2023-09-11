using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF.Entities
{
    public enum Status
    {
        Public = 1,
        Private = 2,
        Domain = 3
    }
    public class SurveyEntity
    {
        [Key]
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string? DomainReq { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public virtual ICollection<UserAnswerEntity> UserAnswers { get; set; } = new HashSet<UserAnswerEntity>();
        /*public SurveyEntity()
        {
            Questions = new HashSet<Question>();
        }*/
    }
}
