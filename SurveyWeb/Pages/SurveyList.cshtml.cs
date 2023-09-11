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
    public class SurveyListModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;

        public SurveyListModel(SurveyEF.SurveyDBContext context)
        {
            _context = context;
        }

        public IList<SurveyEntity> SurveyEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Surveys != null)
            {
                SurveyEntity = await _context.Surveys.ToListAsync();
            }
        }
    }
}
