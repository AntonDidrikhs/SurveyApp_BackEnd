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
        private readonly IHttpClientFactory _clientFactory;


        public DeleteModel(SurveyEF.SurveyDBContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
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

            var request = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5041/api/SurveyEntities/{id}");
            SurveyEntity surveyentity = new SurveyEntity();
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                surveyentity = await response.Content.ReadFromJsonAsync<SurveyEntity>();

            }
            var HasAnswersRequest = new HttpRequestMessage(HttpMethod.Get, $@"http://localhost:5041/api/SurveyEntities/{id}/HasAnswers");
            var HasAnswersResponse = await client.SendAsync(HasAnswersRequest);
            if (HasAnswersResponse.IsSuccessStatusCode)
            {
                hasAnswers = await HasAnswersResponse.Content.ReadFromJsonAsync<bool>();
            }



            //var surveyentity = await _context.Surveys.FirstOrDefaultAsync(m => m.SurveyId == id);
            //hasAnswers = _context.UserAnswers.Where(a => a.SurveyId == id).Any();
            
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

            var client = _clientFactory.CreateClient();
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $@"http://localhost:5041/api/SurveyEntities/{id}/Delete");

            var deleteResponse = await client.SendAsync(deleteRequest);
            if (deleteResponse.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else { return Page(); }
            
        }
    }
}
