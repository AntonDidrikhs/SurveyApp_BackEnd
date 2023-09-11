using SurveyEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyWeb.DTO;
using SurveyEF.Exceptions;
using SurveyEF;
using Microsoft.Extensions.Options;
using NuGet.Protocol;
using Newtonsoft.Json;

namespace SurveyWeb.Services
{
    public class SurveyUserServiceEF
    {
        private SurveyDBContext _dbContext;
        public SurveyUserServiceEF(SurveyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public QuestionDTO CreateNewQuestionDTO(string QuestionType, string optionsNum)
        {
            int num = 2;
            switch (optionsNum)
            {
                case "2":
                    num = 2;
                    break;
                case "3":
                    num = 3;
                    break;
                case "4":
                    num = 4;
                    break;
                case "5":
                    num = 5;
                    break;
                case "6":
                    num = 6;
                    break;
            }
            switch(QuestionType)
            {
                case "MultipleChoice":
                    var options = new List<string>();
                    options.Capacity = num;
                    for(int i = 0; i < num; i++)
                    {
                        options.Add("New Option");
                    }
                    return new MultipleChoiceQDTO() { Options = options };
                    break;
                case "TextField":
                    return new TextFieldQDTO();
                    break;
                case "LikertScale":
                    return new LikertScaleQDTO();
                case "Ranking":
                    var rankOptions = new List<string>();
                    rankOptions.Capacity = num;
                    for (int i = 0; i < num; i++)
                    {
                        rankOptions.Add("New Ranking Option");
                    }
                    return new RankingQDTO() { Options = rankOptions };
            }
            throw new NoMatchingQuestionTypeException("Couldn't find a matching Question type.");
        }
        public SurveyEntity SaveCreatedSurvey(SurveyDTO survey)
        {
            var questionsDTO = survey.SurveyQuestions;
            List<Question> questions = new List<Question>();
            foreach(var question in questionsDTO)
            {
                Question savedQuestion;
                switch(question.QuestionType)
                {
                    case "TextField":
                        savedQuestion = new TextFieldQuestion
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,

                        };
                        questions.Add(savedQuestion);
                        break;
                    case "MultipleChoice":
                        string options = JsonConvert.SerializeObject(question.Options);
                        var optionsDTO = question.AccessList();
                        savedQuestion = new MultipleChoiceQuestion
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            OptionsJson = options 
                        };
                        questions.Add(savedQuestion);
                        break;
                    case "LikertScale":
                        savedQuestion = new LikertScaleQuestion
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,

                        };
                        questions.Add(savedQuestion);
                        break;
                    case "Ranking":
                        string rankOptions = new string("");
                        var rankOptionsDTO = question.Options;
                        rankOptions = rankOptionsDTO.ToJson();
                        savedQuestion = new RankingQuestion
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            OptionsJson = rankOptions
                        };
                        questions.Add(savedQuestion);
                        break;
                }
            }
            if(survey.Status != Status.Domain)
            {
                survey.DomainReq = null;
            }
            var saved = new SurveyEntity
            {
                Title = survey.Title,
                Description = survey.Description,
                Status = survey.Status,
                DomainReq = survey.DomainReq,
                Questions = questions
            };

            //_dbContext.Add(saved);
            //_dbContext.SaveChanges();

            return saved;

        }

        public QuestionDTO QuestionEntityToQuestionDTO(Question question)
        {
            switch(question)
            {
                case TextFieldQuestion tf:
                    return new TextFieldQDTO
                    {
                        Text = question.Text,
                        //IsRequired = question.IsRequired,
                        QuestionType = "TextField"
                    };
                case MultipleChoiceQuestion mc:
                    var MCQuestion = (MultipleChoiceQuestion)question;
                    var opts = JsonConvert.DeserializeObject<List<string>>(MCQuestion.OptionsJson);
                    return new MultipleChoiceQDTO
                    {
                        Text = question.Text,
                        QuestionType = "MultipleChoice",
                        Options = opts
                    };
                case LikertScaleQuestion ls:
                    return new LikertScaleQDTO
                    {
                        Text = question.Text,
                        QuestionType = "LikertScale"

                    };
                case RankingQuestion rn:
                    var rankQ = (RankingQuestion)question;
                    List<string> rankOptions = JsonConvert.DeserializeObject<List<string>>(rankQ.OptionsJson);
                    return new RankingQDTO
                    {
                        Text = question.Text,
                        QuestionType = "Ranking",
                        Options = rankOptions
                    };
            }
            throw new Exception("No matching Type found.");
        }
    }
}
