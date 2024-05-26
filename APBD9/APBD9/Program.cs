using APBD9.Context;
using APBD9.Services;

var builder = WebApplication.CreateBuilder(args);


//Registering services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddDbContext<MasterContext>();

var app = builder.Build();

//Configuring the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();