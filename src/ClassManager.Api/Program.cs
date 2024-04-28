
using ClassManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddServices();
builder.AddControllers();
builder.AddDatabase();
builder.AddJwtAuthentication();

builder.AddMediator();

var app = builder.Build();
app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
