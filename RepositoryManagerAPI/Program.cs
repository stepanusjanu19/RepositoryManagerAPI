using Microsoft.EntityFrameworkCore;
using RepositoryManagerLib;
using RepositoryManagerLib.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.jfeduzpmtljxaxsoyfij;Password=TsywqpHyd2NAmNzLB";

builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
public partial class Program { }
