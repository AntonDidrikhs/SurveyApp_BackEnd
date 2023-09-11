using Microsoft.AspNetCore.Mvc;

namespace SurveyWeb.DTO
{
    
    public abstract class QuestionDTO
    {
        public string Text { get; set; }
        public string QuestionType { get; set; }
        public bool IsRequired { get; set; }
        public List<string>? Options { get; set; }
        public virtual List<string> AccessList() 
        { 
            throw new Exception("No list available for this type.");
        }
        public QuestionDTO() {
            Text = "Enter new Question";
        }
        
    }
    public class TextFieldQDTO : QuestionDTO
    {
        //public string Answer { get; set; }
        public TextFieldQDTO() {
            QuestionType = "TextField";
            Text = "Enter new Text Question";
            //Answer = "Test Answer";
        }
    }
    public class MultipleChoiceQDTO : QuestionDTO
    {
        
        public override List<string> AccessList() 
        {
            return Options;
        }
        public MultipleChoiceQDTO() {
            QuestionType = "MultipleChoice";
        }
    }
    public class LikertScaleQDTO : QuestionDTO
    {
        public LikertScaleQDTO()
        {
            QuestionType = "LikertScale";
            
        }
    }
    public class RankingQDTO : QuestionDTO
    {
        public RankingQDTO()
        {
            QuestionType = "Ranking";
        }
    }
}
