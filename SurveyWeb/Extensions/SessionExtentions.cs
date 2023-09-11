using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SurveyWeb.DTO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurveyWeb.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
            session.SetString(key + "_type", value.GetType().AssemblyQualifiedName);
        }

        public static List<QuestionDTO> GetQuestionListFromSession(this ISession session, string key)
        {
            var jsonString = session.GetString(key);
            var typeString = session.GetString(key + "_type");

            if (jsonString == null || typeString == null)
            {
                return default;
            }
            if(typeString == null)
            {
                throw new Newtonsoft.Json.JsonException("Missing 'type' property in the JSON");
            }

            Type actualType = Type.GetType(typeString);

            List<QuestionDTO> ?QuestionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionDTO>>(jsonString, new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            });
            if(QuestionList == null)
            {
                throw new NotImplementedException();
            }
            return QuestionList;
            //return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }

}
