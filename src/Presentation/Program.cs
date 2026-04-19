using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
using Shop.Presentation.Middleware;
using Shop.Infrastructure.Security;
using FluentValidation.AspNetCore;
using FluentValidation;
using Shop.Presentation.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt-token"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerRegisterValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var customerRepo = services.GetRequiredService<ICustomerRepository>();
    var customerFactory = services.GetRequiredService<ICustomerFactory>();

    await DbInitializer.SeedAdminAsync(customerRepo, customerFactory);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
