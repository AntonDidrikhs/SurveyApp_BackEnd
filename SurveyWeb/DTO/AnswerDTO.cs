using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SurveyWeb.DTO
{
    public class AnswerDTO
    {
        public string? QuestionType { get; set; }
        [NotNull]
        [MinLength(1, ErrorMessage ="Answer must be at least 1 character long.")]
        public string Answer { get; set; }
        public AnswerDTO() { }

    }
}
