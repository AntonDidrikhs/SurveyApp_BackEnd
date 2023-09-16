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
    public class IndexModel : PageModel
    {
        private readonly SurveyEF.SurveyDBContext _context;
        private readonly IHttpClientFactory _clientFactory;
        public IndexModel(SurveyEF.SurveyDBContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        public IList<SurveyEntity> SurveyEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, @"http://localhost:5041/api/SurveyEntities");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} {responseBody}");
            }

            if (response.IsSuccessStatusCode)
            {
                var Surveys = await response.Content.ReadFromJsonAsync(typeof(List<SurveyEntity>));
                SurveyEntity = (List<SurveyEntity>)Surveys;
            }
        }
    }
}
