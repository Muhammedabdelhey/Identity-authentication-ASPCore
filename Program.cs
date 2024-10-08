using Identity_Authentication;
using Identity_Authentication.Authorization;
using Identity_Authentication.Models;
using Identity_Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region register DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));
#endregion

#region register JwtOptions that get Jwt credentials 
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
#endregion

# region register Jwt Service
builder.Services.AddTransient<JWTService>();
#endregion

#region register Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDBContext>();
#endregion

#region JWTBearerToken Authentication
builder.Services.AddAuthentication(options =>
{
    // assign all this shceme to jwt default Scheme 
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SigningKey)
        )
    };
});

#endregion

# region add authorization Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", builder =>
    {
        // can add policy based on roles by this two ways 
        builder.RequireRole("Admin");
        builder.RequireClaim(ClaimTypes.Role, "Admin");
        // can add policy based on spacfic condations , pass lamda that return true
        builder.RequireAssertion(context =>
        {
            return context.User.IsInRole("Admin");
        });
    });

    //options.AddPolicy("CheckAge", builder =>
    //{
    //    builder.RequireAssertion(context =>
    //    {
    //        return DateTime.Parse(context.User.FindFirstValue("DateOfBirth")).Year - DateTime.Today.Year > 25;
    //    });
    //});
    options.AddPolicy("CheckAge", builder =>
    {
        builder.AddRequirements(new AgeAuthorizationRequirement(25));
    });

    options.AddPolicy("AdminOrUser", policy =>
    {
        policy.RequireRole(["Admin", "User"]);
    });
});

#endregion
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();