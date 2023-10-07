using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.BusinessLogic.Services;
using EAD_APP.Core.Helpers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<IMongoDatabase>(options => {
    var settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITrainService, TrainService>();
builder.Services.AddSingleton<IScheduleService, ScheduleService>();

var app = builder.Build();

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
