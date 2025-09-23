using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoapMart.Api.Data;
using ShoapMart.Api.Data.Seed;
using ShopMart.Api.Entities;

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

//setting password rules and other things
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
