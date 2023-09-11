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

namespace SurveyWeb.Pages
{
    public class CreateModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;
        private readonly SurveyUserServiceEF _service;

        [BindProperty]
        public SurveyDTO Survey { get; set; }
        [BindProperty]
        public string NewQuestionType { get; set; }
        [BindProperty]
        public int OptionsNumber { get; set; }

        //[ModelBinder(BinderType = typeof(QuestionModelBinder))]
        [BindProperty]
        public List<QuestionDTO> questions { get; set; }
        

        public CreateModel(SurveyEF.SurveyDBContext context, SurveyUserServiceEF service)
        {
            _context = context;
            _service = service;
            Survey = new SurveyDTO();
            //questions = HttpContext.Session.Get<List<QuestionDTO>>("questions") ?? new List<QuestionDTO>();
        }

        public IActionResult OnGet()
        {
            questions = HttpContext.Session.GetQuestionListFromSession("surveyQuestions") ?? new List<QuestionDTO>();
            return Page();
        }

        /* Old refresh Method
        public async Task<IActionResult> OnPostRefreshList() 
        {
            //HttpClient client = new HttpClient();

            HttpContext.Session.SetObjectAsJson("surveyQuestions", questions);

            return Page();
        }

        */
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
            var saved = _service.SaveCreatedSurvey(Survey);
            _context.Add(saved);
            _context.SaveChanges();
            HttpContext.Session.Clear();
            return RedirectToPage("./Index");
        }
    }
}
