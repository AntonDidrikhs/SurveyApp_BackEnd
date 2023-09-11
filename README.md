# SurveyApp

Projekt ten umożliwia tworzenie, edytowanie, wysyłanie i wypełnianie ankiet. Projekt jest aplikacją internetową, zawierającą takie strony: Index - strona "główna" i lista wszystkich dostępnych ankiet Create - strona do tworzenia nowych ankiet Delete - strona służąca do usuwania ankiet, umożliwiająca usunięcie tylko tych ankiet, które nie mają przypisanych odpowiedzi. Fill Out/Details - strona umożliwiająca udzielenie odpowiedzi na ankietę i zapisanie tych odpowiedzi w bazie danych. Edit - strona umożliwiająca edycję ankiety, zmianę jej tytułu, opisu, typu, a także tekstu pytań ankiety.

Przed uruchomieniem projektu zmień ciąg połączenia wewnątrz SurveyEF\SurveyDBContext.cs na swój serwer i bazę danych. Uruchom "dotnet ef database update --project SurveyEF --startup-project SurveyWeb" w CLI z folderu rozwiązania. Uruchom projekt SurveyWeb

Author: Anton Didrikhs

Libraries used: EntityFrameworkCore, EntityFrameworkCore.Design, EntityFrameworkCore.SqlServer, AspNetCore.Identity.Entityframework, JWT, AspNetCore.Authentication.JwtBearer, AspNetCore.Components.Forms, AspNetCore.Identity.EntityFrameworkCore, AspNetCore.Identity.UI, AspNetCore.MVC.NewtonsoftJson, Newtonsoft.Jsob, EntityFrameworkCore.SqlServer, EntityFrameworkCore.Tools, VisualStudio.Web.CodeGenerationDesign
