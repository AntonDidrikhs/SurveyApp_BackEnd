using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SurveyEF;
using SurveyEF.Entities;
using SurveyWeb.DTO;
using SurveyWeb.Services;

namespace SurveyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyEntitiesController : ControllerBase
    {
        private readonly SurveyDBContext _context;
        private readonly SurveyUserServiceEF _service;

        public SurveyEntitiesController(SurveyDBContext context, SurveyUserServiceEF service)
        {
            _context = context;
            _service = service;
        }   

        // GET: api/SurveyEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurveyEntity>>> GetSurveys()
        {
            if (_context.Surveys == null)
            {
                return NotFound();
            }
            return await _context.Surveys.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyEntity>> GetSurvey(int id)
        {
            if (_context.Surveys == null)
            {
                return NotFound();
            }
            var surveyEntity = await _context.Surveys.FindAsync(id);


            if (surveyEntity == null)
            {
                return NotFound();
            }

            return surveyEntity;
        }

        [HttpGet("{id}/Questions")]
        public async Task<ActionResult<string>> GetQuestionDTOs(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var surveyQuestions = await _context.Questions.Where(q => q.SurveyId == id).ToListAsync();

            List<QuestionDTO> questionDTOs = new List<QuestionDTO>();
            foreach (var question in surveyQuestions)
            {
                switch (question)
                {
                    case TextFieldQuestion tf:
                        questionDTOs.Add(new TextFieldQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "TextField"
                        });
                        break;
                    case MultipleChoiceQuestion mc:
                        var MCQuestion = (MultipleChoiceQuestion)question;
                        var opts = JsonConvert.DeserializeObject<List<string>>(mc.OptionsJson);
                        questionDTOs.Add(new MultipleChoiceQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "MultipleChoice",
                            Options = opts
                        });
                        break;
                    case LikertScaleQuestion ls:
                        questionDTOs.Add(new LikertScaleQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "LikertScale"
                        });
                        break;
                    case RankingQuestion rn:
                        var RNQuestion = (RankingQuestion)question;
                        List<string> rankText = JsonConvert.DeserializeObject<List<string>>(RNQuestion.OptionsJson);

                        questionDTOs.Add(new RankingQDTO
                        {
                            Text = question.Text,
                            IsRequired = question.IsRequired,
                            QuestionType = "Ranking",
                            Options = rankText
                        });
                        break;
                }
            }
            var jsonQuestionDTOs = JsonConvert.SerializeObject(questionDTOs, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return jsonQuestionDTOs;
        }


        [HttpPost("{id}/Questions/Save")]
        public async Task<IActionResult> SaveAnswers(int id, [FromBody] List<AnswerDTO> JsonAnswers)
        {
            List<AnswerDTO> answerDTOs = JsonAnswers;
            var surveyQuestions = _context.Questions.Where(q => q.SurveyId == id).ToList();
            var saveAnswers = new List<UserAnswerEntity>();

            for (int i = 0; i < answerDTOs.Count; i++)
            {
                switch (answerDTOs[i].QuestionType)
                {
                    case "TextField":
                        var newTFAnswer = new TextFieldAnswerEntity()
                        {
                            UserEmail = answerDTOs[i].Email,
                            Survey = _context.Surveys.Where(s => s.SurveyId == id).First(),
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answerDTOs[i].Answer
                        };
                        saveAnswers.Add(newTFAnswer);
                        break;
                    case "MultipleChoice":
                        var newMCAnswer = new MultipleChoiceAnswerEntity()
                        {
                            UserEmail = answerDTOs[i].Email,
                            SurveyId = id,
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answerDTOs[i].Answer
                        };
                        saveAnswers.Add(newMCAnswer);
                        break;
                    case "LikertScale":
                        var newLSAnswer = new LikertScaleAnswerEntity()
                        {
                            UserEmail = answerDTOs[i].Email,
                            SurveyId = id,
                            QuestionId = surveyQuestions[i].QuestionId,
                            Answer = answerDTOs[i].Answer
                        };
                        saveAnswers.Add(newLSAnswer);
                        break;
                    case "Ranking":
                        var newRNAnswer = new RankingAnswerEntity()
                        {
                            UserEmail = answerDTOs[i].Email,
                            SurveyId = id,
                            QuestionId = surveyQuestions[i].QuestionId,
                            JsonAnswer = answerDTOs[i].Answer
                        };
                        saveAnswers.Add(newRNAnswer);
                        break;
                }
            }
            _context.AddRange(saveAnswers);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPost("/NewSurvey/Create")]
        public async Task<IActionResult> CreateSurvey([FromBody] NewSurveyDTO toSave)
        {
            var saved = _service.SaveCreatedSurvey(toSave);
            //var survey = _service.SaveCreatedSurvey(saved);
            _context.Add(saved);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/HasAnswers")]
        public async Task<ActionResult<bool>> CheckForAnswers(int id)
        {
            var hasAnswers = _context.UserAnswers.Where(a => a.SurveyId == id).Any();
            return hasAnswers;
        }


        // PUT: api/SurveyEntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/SaveEdit")]
        public async Task<IActionResult> PutSurveyEntity(int id, NewSurveyDTO surveyEntity)
        {
            if (id != surveyEntity.Id)
            {
                return BadRequest();
            }
            var saved = _service.SaveCreatedSurvey(surveyEntity);
            saved.SurveyId = id;
            _context.Questions.Where(q => q.SurveyId == id).ExecuteDelete();
            _context.Update(saved);

            //_context.Entry(surveyEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SurveyEntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // DELETE: api/SurveyEntities/5
        [HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeleteSurveyEntity(int id)
        {
            if (_context.Surveys == null)
            {
                return NotFound();
            }
            var surveyEntity = await _context.Surveys.FindAsync(id);
            if (surveyEntity == null)
            {
                return NotFound();
            }
            if(_context.UserAnswers.Where(a => a.SurveyId == surveyEntity.SurveyId).Any())
            {
                return BadRequest();
            }
            _context.Surveys.Remove(surveyEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SurveyEntityExists(int id)
        {
            return (_context.Surveys?.Any(e => e.SurveyId == id)).GetValueOrDefault();
        }
    }
}
