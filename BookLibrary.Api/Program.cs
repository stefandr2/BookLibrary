using BookLibrary.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core with SQL Server (connection string in appsettings.json)
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

n// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// Initialize DB (creates DB and seeds sample data). For production, use migrations instead.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    await DbInitializer.InitializeAsync(db);
}
app.Run();






















app.Run();app.MapControllers();app.UseAuthorization();
napp.UseHttpsRedirection();
n}    app.UseSwaggerUI();    app.UseSwagger();    app.UseDeveloperExceptionPage();{if (app.Environment.IsDevelopment())
n// Configure the HTTP request pipeline.var app = builder.Build();    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));builder.Services.AddDbContext<LibraryContext>(options =>
n// Configure EF Core with SQL Server (connection string in appsettings.json)builder.Services.AddSwaggerGen();builder.Services.AddEndpointsApiExplorer();builder.Services.AddControllers();n// Add services to the container.