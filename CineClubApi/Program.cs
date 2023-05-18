using CineClubApi.Common.Helpers;
using CineClubApi.Common.Interfaces;
using CineClubApi.Persistance;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.MovieRepository;
using CineClubApi.Services;
using CineClubApi.Services.AccountService;
using CineClubApi.Services.ListService;
using CineClubApi.Services.MovieService;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Lists;
using CineClubApi.Services.TokenService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<ITokenService, TokenServiceImpl>();

builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IPasswordService, PasswordServiceImpl>();
builder.Services.AddScoped<IListRepository, ListRepositoryImpl>();
builder.Services.AddScoped<IListService, ListServiceImpl>();
builder.Services.AddScoped<ITMDBMovieService, ItmdbMovieServiceImpl>();
builder.Services.AddScoped<IMovieRepository, MovieRepositoryImpl>();
builder.Services.AddScoped<IMovieService, MovieServiceImpl>();
builder.Services.AddScoped<IPaginator, PaginatorImpl>();
builder.Services.AddScoped<ITmdbListService, TmdbListServiceImpl>();
builder.Services.AddScoped<ITMDBGenreService, TmdbGenreServiceImpl>();


builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Configuration
    .AddEnvironmentVariables();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();


app.UseCors();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();
}



app.Run();

/*static void Main()
{
    Console.WriteLine("App started");
}*/