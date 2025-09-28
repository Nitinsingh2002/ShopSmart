using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoapMart.Api.Data;
using ShoapMart.Api.Data.Seed;
using ShoapMart.Api.interfaces;
using ShoapMart.Api.Mappings;
using ShoapMart.Api.Repositories;
using ShopMart.Api.Entities;
using ShopMart.Api.Interfaces;
using ShopMart.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//for adding database
builder.Services.AddDbContext<ShopMartContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("ShopMartConnection")));




//injecting identity
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<ApplicationUser>()
.AddRoles<IdentityRole>()
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("ShoapMart")
.AddEntityFrameworkStores<ShopMartContext>()
.AddDefaultTokenProviders();

//registering services for DI
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenRepository, JwtTokenServices>();



//inject automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//setting password rules and other things
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});

//middleware for authentication(authenticating user using jwt token)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
    };
});
var app = builder.Build();

//calling seeded roles and admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedRolesAndAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
