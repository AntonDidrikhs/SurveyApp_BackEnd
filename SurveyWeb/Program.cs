using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SurveyEF;
using Newtonsoft;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyWeb.Services;
using SurveyWeb.Extensions;
using SurveyWeb.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SurveyDBContextConnection") ?? throw new InvalidOperationException("Connection string 'SurveyDBContextConnection' not found.");

// Add services to the container.
builder.Services.AddSingleton(new Dictionary<Type, (ModelMetadata, IModelBinder)>());
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.ModelBinderProviders.Insert(0, new QuestionModelBinderProvider());
}); ; 
builder.Services.AddDbContext<SurveyEF.SurveyDBContext>();
builder.Services.AddScoped<SurveyUserServiceEF>();
builder.Services.AddSession();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SurveyDBContext>();
builder.Services.AddControllers().AddNewtonsoftJson();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(opt => opt.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser().Build()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseSession();
app.MapRazorPages();

app.Run();
