using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.DTO
{
    public class NewSurveyDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [RegularExpression(@"^@[a-zA-Z0-9]+(?:[.][a-zA-Z0-9]+)+$", ErrorMessage = "Must be valid email domain.")]
        public string? DomainReq { get; set; }
        public List<NewQuestionDTO> Questions { get; set; }
    }
}
