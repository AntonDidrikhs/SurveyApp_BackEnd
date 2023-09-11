using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SurveyEF;
using SurveyEF.Entities;

namespace SurveyWeb.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;

        

        public DeleteModel(SurveyEF.SurveyDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public bool hasAnswers { get; set; }
        [BindProperty]
      public SurveyEntity SurveyEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }

            var surveyentity = await _context.Surveys.FirstOrDefaultAsync(m => m.SurveyId == id);
            hasAnswers = _context.UserAnswers.Where(a => a.SurveyId == id).Any();
            
            if (surveyentity == null)
            {
                return NotFound();
            }
            else 
            {
                SurveyEntity = surveyentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }
            var surveyentity = await _context.Surveys.FindAsync(id);

            if (surveyentity != null)
            {
                SurveyEntity = surveyentity;
                _context.Surveys.Remove(SurveyEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
