using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using SurveyEF;
using SurveyEF.Entities;
using SurveyWeb.DTO;

namespace SurveyWeb.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;
        private readonly IAuthorizationService _authorizationService;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public List<AnswerDTO> answers { get; set; }
        [BindProperty]
        public SurveyEntity survey { get; set; }
        [BindProperty]
        public List<Question> questions { get; set; }
        [BindProperty]
        public List<List<string>> ranks { get; set; }
        [BindProperty]
        public List<QuestionDTO> questionDTOs { get; set; }
        public DetailsModel(SurveyEF.SurveyDBContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;


            answers = new List<AnswerDTO>();
            ranks = new List<List<string>>();
            questionDTOs = new List<QuestionDTO>();
        }

      public SurveyEntity SurveyEntity { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }


            survey = await _context.Surveys.FirstOrDefaultAsync(m => m.SurveyId == id);
            var surveyQuestions = _context.Questions.Where(q => q.SurveyId == id).ToList();
            
            foreach(var question in surveyQuestions)
            {
                switch(question)
                {
                    case TextFieldQuestion tf:
                        questionDTOs.Add(new TextFieldQDTO { 
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "TextField" });
                        break;
                    case MultipleChoiceQuestion mc:
                        var MCQuestion = (MultipleChoiceQuestion)question;
                        var opts = JsonConvert.DeserializeObject<List<string>>(mc.OptionsJson);
                        questionDTOs.Add(new MultipleChoiceQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "MultipleChoice",
                            Options = opts
                        });
                        break;
                    case LikertScaleQuestion ls:
                        questionDTOs.Add(new LikertScaleQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "LikertScale"
                        });
                        break;
                    case RankingQuestion rn:
                        var RNQuestion = (RankingQuestion)question;
                        List<string> rankText = JsonConvert.DeserializeObject<List<string>>(RNQuestion.OptionsJson);
                        
                        questionDTOs.Add(new RankingQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "Ranking",
                            Options = rankText
                        });
                        break;
                }
            }
            
            answers.Capacity = questionDTOs.Count;
            for (int i = 0; i<answers.Capacity; i++)
            {
                answers.Add(new AnswerDTO());
            }

            foreach (var question in questionDTOs)
            {
                var rankList = new List<string>();
                if (question.QuestionType != "Ranking")
                {
                    ranks.Add(rankList);
                }
                else
                {
                    foreach (var option in question.Options)
                    {
                        rankList.Add("");
                    }
                    ranks.Add(rankList);
                }
            }
            
            if (survey == null)
            {
                return NotFound();
            }
            else 
            {
                SurveyEntity = survey;
                questions = surveyQuestions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSave(List<AnswerDTO> answers)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            List<string> answers2 = new List<string>();
            List<string> types = new List<string>();
            List<List<string>> rankStrings = new List<List<string>>( new List<string>[(Request.Form.Keys.Count - 3)/2]);
            if (survey.Status == Status.Domain && !survey.DomainReq.IsNullOrEmpty())
            {
                string domain = new string(Email.TakeLast(survey.DomainReq.Length).ToArray());
                if (survey.DomainReq != domain)
                {
                    return RedirectToPage("./Error");
                }
            }
            

            foreach(string key in Request.Form.Keys)
            {

                if (key.StartsWith("answers"))
                {
                    if (key.EndsWith("Answer"))
                    {
                        answers2.Add(Request.Form[key]);
                    }
                    if (key.EndsWith("QuestionType"))
                    {
                        types.Add(Request.Form[key]);
                    }
                }
                    if (key.StartsWith("ranks"))
                {
                    var index = int.Parse(new string(key.Skip(6).SkipLast(4).ToArray()));
                    var innerIndex = int.Parse( key.SkipLast(1).Last().ToString());
                    
                    if(innerIndex == 0)
                    {
                        answers2.Add("Placeholder");
                        types.Add("Ranking");
                        rankStrings[index] = new List<string>() { Request.Form[key] };
                    }
                    else
                    {
                        rankStrings[index].Add(Request.Form[key]);
                    }
                    /*
                    if (strings[int.Parse(index)] != "Placeholder")
                    {
                        strings[int.Parse(index)] = "Placeholder";
                    }*/
                }
            }


            var surveyQuestions = _context.Questions.Where(q => q.SurveyId == survey.SurveyId).ToList();
            var saveAnswers = new List<UserAnswerEntity>();
            for (int i = 0; i<answers2.Count; i++)
            {
                switch(types[i])
                {
                    case "TextField":
                        var newTFAnswer = new TextFieldAnswerEntity()
                        {
                            UserEmail = Request.Form["Email"],
                            Survey = _context.Surveys.Where(s => s.SurveyId == survey.SurveyId).First(),
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answers2[i]
                        };
                        saveAnswers.Add(newTFAnswer);
                        break;
                    case "MultipleChoice":
                        var newMCAnswer = new MultipleChoiceAnswerEntity()
                        {
                            UserEmail = Request.Form["Email"],
                            SurveyId = survey.SurveyId,
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answers2[i]
                        };
                        saveAnswers.Add(newMCAnswer);
                        break;
                    case "LikertScale":
                        var newLSAnswer = new LikertScaleAnswerEntity()
                        {
                            UserEmail = Request.Form["Email"],
                            SurveyId = survey.SurveyId,
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answers2[i]
                        };
                        saveAnswers.Add(newLSAnswer);
                        break;
                    case "Ranking":
                        var jsonRanks = rankStrings[i].ToJson();
                        var newRNAnswer = new RankingAnswerEntity()
                        {
                            UserEmail = Request.Form["Email"],
                            SurveyId = survey.SurveyId,
                            QuestionId = surveyQuestions[i].QuestionId,
                            JsonAnswer = jsonRanks
                        };
                        saveAnswers.Add(newRNAnswer);
                        break;
                }
            }
            _context.AddRange(saveAnswers);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
