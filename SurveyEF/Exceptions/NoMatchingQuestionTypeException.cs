using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyEF.Exceptions
{
    public class NoMatchingQuestionTypeException : Exception
    {
        public NoMatchingQuestionTypeException(string? message) : base(message)
        {
        }
    }
}
