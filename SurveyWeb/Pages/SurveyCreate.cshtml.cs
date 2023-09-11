using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyEF;
using SurveyEF.Entities;

namespace SurveyWeb.Pages
{
    public class SurveyCreateModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;

        public SurveyCreateModel(SurveyEF.SurveyDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SurveyEntity SurveyEntity { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Surveys == null || SurveyEntity == null)
            {
                return Page();
            }

            _context.Surveys.Add(SurveyEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
