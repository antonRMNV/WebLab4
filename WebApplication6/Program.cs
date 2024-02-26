using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(@"data/books.json").AddJsonFile(@"data/profiles.json");
var app = builder.Build();
app.Map("/Library", async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("<b style=\"color:limegreen;font-size:20px\">Welcome to the Library!</b>");
});
app.Map("/Library/Books", async (HttpContext context, IConfiguration appConfig) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    StringBuilder sb = new StringBuilder("<b style=\"color:limegreen;font-size:20px\">Books in the Library:</b>");
    for (int i = 0; i < 4; i++)
    {
        IConfigurationSection book = appConfig.GetSection($"books:{i}");
        sb.Append($"<li style=\"color:limegreen;font-size:16px\"> {book["title"]} | {book["author"]}" +
            $" | {book["genre"]} | {book["year"]}</li>");
    }
    await context.Response.WriteAsync($"{sb.ToString()}");
});
app.Map("/Library/Profile/{id:range(0,5)?}", async (HttpContext context, IConfiguration appConfig, int id) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    StringBuilder sb = new StringBuilder($"<b style=\"color:limegreen;font-size:20px\">User profile with id number " +
        $"<i style=\"color:red\">{id}</i></b>");
    IConfiguration user = appConfig.GetSection($"profiles:{id}");
    sb.Append($"<div style=\"color:limegreen;font-size:16px\"> Id: <b>{user["id"]}</b></div>");
    sb.Append($"<div style=\"color:limegreen;font-size:16px\"> Name: <b>{user["name"]}</b></div>");
    sb.Append($"<div style=\"color:limegreen;font-size:16px\"> Age: <b>{user["age"]}</b> years old</div>");
    await context.Response.WriteAsync($"{sb.ToString()}");
});
app.Map("/Library/Profile/", async (HttpContext context, IConfiguration appConfig) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync($"<b style=\"color:limegreen;font-size:20px\">Information about current user</b>" +
        $"<div style=\"color:limegreen;font-size:16px\"> Id: <b>unknown</b></div>" +
        $"<div style=\"color:limegreen;font-size:16px\"> Name: <b>Anton Romanov</b></div>" +
        $"<div style=\"color:limegreen;font-size:16px\"> Age: <b>20</b> years old</div>");
});
app.Run();
