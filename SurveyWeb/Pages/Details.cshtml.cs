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
        private readonly IHttpClientFactory _clientFactory;

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
        public DetailsModel(SurveyEF.SurveyDBContext context, IAuthorizationService authorizationService, IHttpClientFactory clientFactory)
        {
            _context = context;
            _authorizationService = authorizationService;
            _clientFactory = clientFactory;

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


            var surveyRequest = new HttpRequestMessage(HttpMethod.Get, @$"http://localhost:5041/api/SurveyEntities/{id}");
            var client = _clientFactory.CreateClient();
            var surveyResponse = await client.SendAsync(surveyRequest);
            if (surveyResponse.IsSuccessStatusCode)
            {
                survey = await surveyResponse.Content.ReadFromJsonAsync<SurveyEntity>();
            }

            var questionRequest = new HttpRequestMessage(HttpMethod.Get, @$"http://localhost:5041/api/SurveyEntities/{id}/Questions");
            var questionResponse = await client.SendAsync(questionRequest);
            if (questionResponse.IsSuccessStatusCode)
            {
                var questionDTOResponse = await questionResponse.Content.ReadAsStringAsync();
                questionDTOs = JsonConvert.DeserializeObject<List<QuestionDTO>>(questionDTOResponse, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSave(List<AnswerDTO> answers)
        {
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

                }
            }


            var SaveDTOs = new List<AnswerDTO>();
            var email = Request.Form["Email"];

            for (int i = 0; i < answers2.Count; i++)
            {
                switch (types[i])
                {
                    case "TextField":
                        var NewTFDTO = new AnswerDTO()
                        {
                            Answer = answers2[i],
                            QuestionType = types[i],
                            Email = email
                        };
                        SaveDTOs.Add(NewTFDTO);
                        break;
                    case "LikertScale":
                        var NewLSDTO = new AnswerDTO()
                        {
                            Answer = answers2[i],
                            QuestionType = types[i],
                            Email = email
                        };
                        SaveDTOs.Add(NewLSDTO);
                        break;
                    case "MultipleChoice":
                        var NewMCDTO = new AnswerDTO()
                        {
                            QuestionType = types[i],
                            Answer = answers2[i],
                            Email = email
                        };
                        SaveDTOs.Add(NewMCDTO);
                        break;
                    case "Ranking":
                        var NewRNDTO = new AnswerDTO()
                        {
                            QuestionType = types[i],
                            Answer = rankStrings[i].ToJson(),
                            Email = email
                        };
                        SaveDTOs.Add(NewRNDTO);
                        break;
                }
            }

            var client = _clientFactory.CreateClient();
            var answerResponse = await client.PostAsJsonAsync(@$"http://localhost:5041/api/SurveyEntities/{survey.SurveyId}/Questions/Save", SaveDTOs);

            return RedirectToPage("./Index");
        }
    }
}
