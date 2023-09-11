using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public EditModel(SurveyEF.SurveyDBContext context, SurveyUserServiceEF service)
        {
            _context = context;
            _service = service;
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

            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }
            hasAnswers = _context.UserAnswers.Where(a => a.SurveyId == id).Any();


            SurvId = id;
            Survey = new SurveyDTO();
            Survey.Status = _context.Surveys.Where(s => s.SurveyId == id).FirstOrDefault().Status;
            Survey.Title = _context.Surveys.Where(s => s.SurveyId == id).FirstOrDefault().Title;
            Survey.Description = _context.Surveys.Where(s => s.SurveyId == id).FirstOrDefault().Description;
            
            
            questions = new List<QuestionDTO> { };
            var surveyentity =  await _context.Surveys.FirstOrDefaultAsync(m => m.SurveyId == id);
            if (surveyentity == null)
            {
                return NotFound();
            }
            var surveyQuestions = _context.Questions.Where(q => q.SurveyId == id);
            foreach(var q in surveyQuestions)
            {
                questions.Add(_service.QuestionEntityToQuestionDTO(q));
            }
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
            var NewSurvey = _service.SaveCreatedSurvey(Survey);

            if (!hasAnswers)
            {
                if (SurvId.HasValue)
                {
                    NewSurvey.SurveyId = (int)SurvId;
                }
                _context.Questions.Where(q => q.SurveyId == SurvId).ExecuteDelete();
                _context.Update(NewSurvey);
                //_context.Attach(NewSurvey).State = EntityState.Modified;
            }
            else
            {
                _context.Add(NewSurvey);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SurveyEntityExists(int id)
        {
          return (_context.Surveys?.Any(e => e.SurveyId == id)).GetValueOrDefault();
        }
    }
}
