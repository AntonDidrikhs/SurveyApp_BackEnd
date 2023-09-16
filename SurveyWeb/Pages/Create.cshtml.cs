using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyEF;
using SurveyEF.Entities;
using SurveyWeb.DTO;
using SurveyWeb.Services;
using SurveyWeb.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace SurveyWeb.Pages
{
    public class CreateModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;
        private readonly SurveyUserServiceEF _service;
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public SurveyDTO Survey { get; set; }
        [BindProperty]
        public string NewQuestionType { get; set; }
        [BindProperty]
        public int OptionsNumber { get; set; }

        //[ModelBinder(BinderType = typeof(QuestionModelBinder))]
        [BindProperty]
        public List<QuestionDTO> questions { get; set; }
        

        public CreateModel(SurveyEF.SurveyDBContext context, SurveyUserServiceEF service, IHttpClientFactory clientFactory)
        {
            _context = context;
            _service = service;
            _clientFactory = clientFactory;
            Survey = new SurveyDTO();
            //questions = HttpContext.Session.Get<List<QuestionDTO>>("questions") ?? new List<QuestionDTO>();
        }

        public IActionResult OnGet()
        {
            questions = HttpContext.Session.GetQuestionListFromSession("surveyQuestions") ?? new List<QuestionDTO>();
            return Page();
        }

        public async Task<IActionResult> OnPostAddQuestion()
        {
            HttpContext.Session.SetObjectAsJson("surveyQuestions", questions);

            List<QuestionDTO> existingQuestions = HttpContext.Session.GetQuestionListFromSession("surveyQuestions");
            if (existingQuestions == null)
            {
                existingQuestions = new List<QuestionDTO>();
            }
            
            string newQuestionType = Request.Form["NewQuestionType"];
            string optionNum = Request.Form["OptionsNumber"];
            QuestionDTO newQuestion = _service.CreateNewQuestionDTO(newQuestionType, optionNum);
            existingQuestions.Add(newQuestion);
            questions = existingQuestions;
            HttpContext.Session.SetObjectAsJson("surveyQuestions", existingQuestions);
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostSave()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid || _context.Surveys == null || Survey == null)
            {
                return Page();
            }
            /*if (Survey.Status == Status.Domain && (!Survey.DomainReq.StartsWith("@") || !Survey.DomainReq.Contains(".") || Survey.DomainReq.Skip(1).First().Equals(".") || Survey.DomainReq.EndsWith(".")))
            {
                return Page();
            }*/
            Survey.SurveyQuestions = questions;

            var NewSurvey = _service.SurveyDTOtoNewSurveyDTO(Survey);

            


            var client = _clientFactory.CreateClient();

            var response = await client.PostAsJsonAsync(@$"http://localhost:5041/NewSurvey/Create", NewSurvey);
            /*
            _context.Add(saved);
            _context.SaveChanges();
            */
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} {responseBody}");
            }
            HttpContext.Session.Clear();
            return RedirectToPage("./Index");
        }
    }
}
