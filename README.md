# SurveyApp

Projekt ten umożliwia tworzenie, edytowanie, wysyłanie i wypełnianie ankiet. Projekt jest aplikacją internetową, zawierającą takie strony: Index - strona "główna" i lista wszystkich dostępnych ankiet 
1. Create - strona do tworzenia nowych ankiet (Aby dodać pytanie, wybierz typ pytania z listy, w przypadku pytań MultipleChoice i Ranking użyj drugiej listy, aby wybrać liczbę opcji w pytaniu, a następnie kliknij przycisk "Add Question").
2. Delete - strona służąca do usuwania ankiet, umożliwiająca usunięcie tylko tych ankiet, które nie mają przypisanych odpowiedzi. 
3. Fill Out/Details - strona umożliwiająca udzielenie odpowiedzi na ankietę i zapisanie tych odpowiedzi w bazie danych. 
4. Edit - strona umożliwiająca edycję ankiety, zmianę jej tytułu, opisu, typu, a także tekstu pytań ankiety.

Przed uruchomieniem projektu zmień ciąg połączenia wewnątrz SurveyEF\SurveyDBContext.cs na swój serwer i bazę danych. Uruchom "dotnet ef database update --project SurveyEF --startup-project SurveyWeb" w CLI z folderu rozwiązania. Upewnij się, że adres URL aplikacji projektu WebAPI jest ustawiony na "http://localhost:5041" (właściwości projektu WebAPI > Debug). Uruchom projekt WebAPI, a następnie projekt SurveyWeb.

Author: Anton Didrikhs

Libraries used: EntityFrameworkCore, EntityFrameworkCore.Design, EntityFrameworkCore.SqlServer, AspNetCore.Identity.Entityframework, JWT, AspNetCore.Authentication.JwtBearer, AspNetCore.Components.Forms, AspNetCore.Identity.EntityFrameworkCore, AspNetCore.Identity.UI, AspNetCore.MVC.NewtonsoftJson, Newtonsoft.Jsob, EntityFrameworkCore.SqlServer, EntityFrameworkCore.Tools, VisualStudio.Web.CodeGenerationDesign, Microsoft.AspNetCore.OpenApi, Swashbuckle.AspNetCore
