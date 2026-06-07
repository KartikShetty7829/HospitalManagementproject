using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.SwaggerForOcelot;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot configuration depending on environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("ocelot.local.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.azure.json", optional: false, reloadOnChange: true);
}

// Register Ocelot + SwaggerForOcelot
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Swagger aggregation (Gateway only)
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
    // No RoutePrefix here — SwaggerForOcelotUIOptions doesn’t support it
    // By default, the aggregated UI will be available at /swagger
});

// Ocelot middleware
await app.UseOcelot();

app.Run();
