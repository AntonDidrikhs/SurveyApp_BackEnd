using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SurveyEF;
using SurveyEF.Entities;
using SurveyWeb.DTO;
using SurveyWeb.Services;

namespace SurveyWeb.Pages
{
    public class EditModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;
        private readonly SurveyUserServiceEF _service;
        private readonly IHttpClientFactory _clientFactory;
        public EditModel(SurveyEF.SurveyDBContext context, SurveyUserServiceEF service, IHttpClientFactory clientFactory)
        {
            _context = context;
            _service = service;
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public bool hasAnswers { get; set; }
        [BindProperty]
        public int? SurvId { get; set; }
        [BindProperty]
        public SurveyDTO Survey { get; set; }
        [BindProperty]
        public List<QuestionDTO> questions { get; set; }
        //[BindProperty]
        //public SurveyEntity SurveyEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var client = _clientFactory.CreateClient();
            
            var surveyRequest = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5041/api/SurveyEntities/{id}");
            var surveyResponse = await client.SendAsync(surveyRequest);
            SurveyEntity surveyEntity = new SurveyEntity();
            if (surveyResponse.IsSuccessStatusCode)
            {
                surveyEntity = await surveyResponse.Content.ReadFromJsonAsync<SurveyEntity>();
                SurvId = id;
                Survey = new SurveyDTO();
                Survey.Status = surveyEntity.Status;
                Survey.Title = surveyEntity.Title;
                Survey.Description = surveyEntity.Description;
            }
            
            questions = new List<QuestionDTO> { };

            var questionRequest = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5041/api/SurveyEntities/{id}/Questions");
            var questionResponse = await client.SendAsync(questionRequest);
            List<Question> surveyQuestions = new List<Question>();
            if (questionResponse.IsSuccessStatusCode)
            {
                var questionDTOResponse = await questionResponse.Content.ReadAsStringAsync();
                questions = JsonConvert.DeserializeObject<List<QuestionDTO>>(questionDTOResponse, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            }

            var HasAnswersRequest = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5041/api/SurveyEntities/{id}/HasAnswers");
            var HasAnswersResponse = await client.SendAsync(HasAnswersRequest);
            if (HasAnswersResponse.IsSuccessStatusCode)
            {
                hasAnswers = await HasAnswersResponse.Content.ReadFromJsonAsync<bool>();
            }

            //var surveyQuestions = _context.Questions.Where(q => q.SurveyId == id);
            /*foreach (var q in surveyQuestions)
            {
                questions.Add(_service.QuestionEntityToQuestionDTO(q));
            }*/
            //SurveyEntity = surveyentity;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //var SurveyMod = _context.Surveys.Where(s => s.SurveyId == SurvId).FirstAsync();

            Survey.SurveyQuestions = questions;
            NewSurveyDTO _newSurvey = _service.SurveyDTOtoNewSurveyDTO(Survey);
            //var NewSurvey = _service.SaveCreatedSurvey(_newSurvey);
            
            var client = _clientFactory.CreateClient();
            
            
            if (!hasAnswers)
            {
                if (SurvId.HasValue)
                {
                    _newSurvey.Id = (int)SurvId;
                }

                var response = await client.PutAsJsonAsync($@"http://localhost:5041/api/SurveyEntities/{_newSurvey.Id}/SaveEdit", _newSurvey);
                //_context.Questions.Where(q => q.SurveyId == SurvId).ExecuteDelete();
                //_context.Update(NewSurvey);
                //_context.Attach(NewSurvey).State = EntityState.Modified;
            }
            else
            {
                var response = await client.PostAsJsonAsync(@$"http://localhost:5041/NewSurvey/Create", _newSurvey);
            }

            
            return RedirectToPage("./Index");
        }

        private bool SurveyEntityExists(int id)
        {
          return (_context.Surveys?.Any(e => e.SurveyId == id)).GetValueOrDefault();
        }
    }
}
