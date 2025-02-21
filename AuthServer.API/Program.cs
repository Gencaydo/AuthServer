using AuthServer.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure application services
builder.Services.AddApplicationServices(builder.Configuration);

// Configure CORS
builder.Services.AddCorsPolicies();

// Configure FluentValidation
builder.Services.AddValidation();

// Configure Identity and Authentication
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("LocalhostPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
