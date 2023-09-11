using SurveyWeb.DTO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurveyWeb.Extensions
{
    /*public class QuestionDTOConverter : JsonConverter<QuestionDTO>
    {
        public override QuestionDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var questions = new List<QuestionDTO>();

            if(reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if(reader.TokenType == JsonTokenType.StartObject)
                {
                    string type = null;
                    while (reader.Read())
                    {
                        if(reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();
                            if (propertyName == "$type")
                            {
                                type = reader.GetString();
                            }
                            else if(type != null)
                            {
                                reader.Skip();
                                //var Question = (QuestionDTO)JsonSerializer.Deserialize(type, options);
                                
                            }
                        }
                    }
                }

            }
        }

    public override void Write(Utf8JsonWriter writer, QuestionDTO value, JsonSerializerOptions options)
        {
            writer.WriteString("type", value.GetType().Name);
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }*/
}
