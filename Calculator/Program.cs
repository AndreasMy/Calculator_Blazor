using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Calculator;
using Calculator.Services.ExpressionHandler;
using Calculator.Services.NCalcCalculator;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IExpressionHandler, ExpressionHandler>();
builder.Services.AddScoped<INCalcCalculator, NCalcCalculator>();

await builder.Build().RunAsync();