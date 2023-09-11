using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SurveyWeb.DTO;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace SurveyWeb.Extensions
{
    public class QuestionModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public QuestionModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var boundList = new List<QuestionDTO>();
            for (int i = 0; ;  i++) 
            {
                var indexKey = $"{bindingContext.ModelName}[{i}]";
                var modelTypeKey = ModelNames.CreatePropertyModelName(indexKey, nameof(QuestionDTO.QuestionType));

                var modelOptionsKey = ModelNames.CreatePropertyModelName(indexKey, nameof(MultipleChoiceQDTO.Options));
                
                var modelTypeValueProviderResult = bindingContext.ValueProvider.GetValue(modelTypeKey);
                
                if (modelTypeValueProviderResult == ValueProviderResult.None)
                {
                    break;
                }

                var modelTypeValue = modelTypeValueProviderResult.FirstValue;

                IModelBinder modelBinder;
                ModelMetadata modelMetadata;
                if (modelTypeValue == "TextField")
                {
                    (modelMetadata, modelBinder) = binders[typeof(TextFieldQDTO)];

                }
                else if (modelTypeValue == "MultipleChoice")
                {
                    (modelMetadata, modelBinder) = binders[typeof(MultipleChoiceQDTO)];
                }
                else if (modelTypeValue == "LikertScale")
                {
                    (modelMetadata, modelBinder) = binders[typeof(LikertScaleQDTO)];
                }
                else if(modelTypeValue == "Ranking")
                {
                    (modelMetadata, modelBinder) = binders[typeof(RankingQDTO)];
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }
                
                

                var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                indexKey);

                await modelBinder.BindModelAsync(newBindingContext);
                var modelBindingResult = newBindingContext.Result;
                if (modelBindingResult.IsModelSet)
                {
                    bindingContext.ValidationState[newBindingContext.Result.Model] = new ValidationStateEntry
                    {
                        Metadata = modelMetadata,
                    };
                    if (modelTypeValue == "TextField")
                    {
                        boundList.Add((TextFieldQDTO)modelBindingResult.Model);

                    }
                    else if (modelTypeValue == "MultipleChoice")
                    {
                        
                        boundList.Add((MultipleChoiceQDTO)modelBindingResult.Model);
                    }
                    else if(modelTypeValue == "LikertScale")
                    {
                        boundList.Add((LikertScaleQDTO)modelBindingResult.Model);
                    }
                    else if(modelTypeValue == "Ranking")
                    {
                        boundList.Add((RankingQDTO)modelBindingResult.Model);
                    }
                    else
                    {
                        boundList.Add((QuestionDTO)modelBindingResult.Model);
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(boundList);

            return;
        }
    }

    public class QuestionModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(List<QuestionDTO>))
            {
                return null;
            }
            if (context.Metadata.ModelType == typeof(List<QuestionDTO>))
            {
                var subclasses = new[] { typeof(TextFieldQDTO), typeof(MultipleChoiceQDTO), typeof(LikertScaleQDTO), typeof(RankingQDTO) };

                var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
                foreach (var type in subclasses)
                {
                    var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                    binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
                }

                return new QuestionModelBinder(binders);
            }
            return null;
        }
    }

        /*    public class QuestionModelBinderProvider : IModelBinderProvider
            {
                public IModelBinder GetBinder(ModelBinderProviderContext context)
                {
                    if(context.Metadata.ModelType != typeof(QuestionDTO) &&
                    !(context.Metadata.ModelType.IsGenericType &&
                      context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(List<>) &&
                      context.Metadata.ModelType.GetGenericArguments()[0].IsAssignableFrom(typeof(QuestionDTO))))
                    {
                        return null;
                    }
                    var subclass = new[] { typeof(TextFieldQDTO), typeof(MultipleChoiceQDTO) };

                    var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();

                    foreach(var type in subclass)
                    {
                        var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                        binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
                    }

                    return new QuestionDTOModelBinder(binders);
            }*/
    }