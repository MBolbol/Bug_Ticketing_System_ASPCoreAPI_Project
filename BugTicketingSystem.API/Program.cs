using BugTicketingSystem.DAL;
using BugTicketingSystem.BL;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Data;
using BugTicketingSystem.BL.Options;
using BugTicketingSystem.API.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BugTicketingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using BugTicketingSystem.BL.Constants;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
/******************************************************************************/
builder.Services.AddBusinessServices();
builder.Services.AddDataAccessServices(builder.Configuration);
/******************************************************************************/
#region bind class passwordOptiens
//builder.Services.Configure<PasswordOptions>(
//    builder.Configuration.GetSection(PasswordOptions.Key)
//);
#endregion

#region ExceptionHandling
/******************************************************************************/
// Custom ExceptionHandling
//builder.Services.AddSingleton<ExceptionHandling>();

/******************************************************************************/
// BuiltInExceptionHandling
builder.Services.AddExceptionHandler<BuiltInExceptionHandling>();
builder.Services.AddProblemDetails();
#endregion

#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration.GetValue<string>("SecretKey");
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        var Key = new SymmetricSecurityKey(secretKeyBytes);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,

  
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = Key,
            RequireExpirationTime = true,
        };
    });
#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        AuthConstants.Ploicies.Manager,
        policy => policy.RequireRole(UserRole.Manager.ToString()));
    options.AddPolicy(
        AuthConstants.Ploicies.Developing,
        policy => policy.RequireRole(UserRole.Developer.ToString()));
    options.AddPolicy(
        AuthConstants.Ploicies.Testing,
        policy => policy.RequireRole(UserRole.Tester.ToString()));
});
#endregion

#region Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;

    //options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<BugDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Enum Service
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
#endregion

#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

#endregion
#endregion
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

#region Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BugDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    // Apply pending migrations
    context.Database.Migrate();

    // Seed initial data
    await SeedData.Initialize(context, userManager);
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseCors("AllowAll");
#region ExceptionHandling
/******************************************************************************/
// Custom ExceptionHandling
//app.UseMiddleware<ExceptionHandling>();
/******************************************************************************/
// BuiltInExceptionHandling
app.UseExceptionHandler();
/******************************************************************************/
#endregion
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Attachments")),
    RequestPath = "/attachments"
});
app.UseRouting();


#region Authentiction&Authorization middleware
// Authentiction at first
app.UseAuthentication();
app.UseAuthorization();
#endregion


app.MapControllers();

app.Run();