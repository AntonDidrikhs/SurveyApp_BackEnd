using SurveyEF.Entities;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.DTO
{
    public class SurveyDTO
    {
        public string Title { get; set; }
        public Status Status { get; set; } //maybe move enum to a seperate folder
        [RegularExpression(@"^@[a-zA-Z0-9]+(?:[.][a-zA-Z0-9]+)+$", ErrorMessage ="Must be valid email domain.")]
        public string? DomainReq { get; set; }
        public string Description { get; set; }
        public List<QuestionDTO> SurveyQuestions { get; set; }
        /*public SurveyDTO() {
        SurveyQuestions = new List<QuestionDTO>();
        } */
    }
}
