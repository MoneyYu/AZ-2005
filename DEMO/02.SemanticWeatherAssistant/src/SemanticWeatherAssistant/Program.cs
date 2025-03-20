using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.OpenApi;
using Spectre.Console;

var builder = Kernel.CreateBuilder();



// Load configuration from appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var deploymentName = config["AzureOpenAI:DeploymentName"];
var endpoint = config["AzureOpenAI:Endpoint"];
var apiKey = config["AzureOpenAI:ApiKey"];

// 設定 Azure OpenAI 
builder.AddAzureOpenAIChatCompletion(
    deploymentName,
    endpoint,
    apiKey
);


var kernel = builder.Build();

var plugin = await kernel.ImportPluginFromOpenApiAsync("Weather", new Uri("https://api.weather.gov/openapi.json"));

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory();
chatHistory.AddSystemMessage("你是一個天氣助手，幫忙使用者查詢相關天氣，回復請使用繁體中文。");

AnsiConsole.Write(new FigletText("Weather Assistant").Color(Color.Blue));
AnsiConsole.MarkupLine("[blue]歡迎使用智能天氣助手![/]");
AnsiConsole.MarkupLine("[grey]輸入 'exit' 來結束程式[/]");

while (true)
{
    var prompt = AnsiConsole.Ask<string>("[green]請輸入您的問題:[/]");

    if (prompt.ToLower() == "exit")
        break;

    chatHistory.AddUserMessage(prompt);

    try
    {
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("思考中...", async ctx =>
            {
                var result = await chatCompletionService.GetChatMessageContentAsync(
                    chatHistory,
                    kernel: kernel,
                    executionSettings: new OpenAIPromptExecutionSettings
                    {
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                    });

                string response = result?.Content ?? "無回應";
                chatHistory.AddAssistantMessage(response);

                var panel = new Panel(response)
                {
                    Header = new PanelHeader("🤖 助手回應"),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(1, 1),
                };
                panel.BorderStyle = new Style(Color.Blue);

                AnsiConsole.Write(panel);
                AnsiConsole.WriteLine();
            });
    }
    catch (Exception ex)
    {
        var errorPanel = new Panel($"[bold red]{ex.Message}[/]")
        {
            Header = new PanelHeader("❌ 錯誤"),
            Border = BoxBorder.Heavy,
            BorderStyle = new Style(Color.Red),
        };
        AnsiConsole.Write(errorPanel);
    }
}

var farewell = new Panel("感謝使用，希望有幫助到您！")
{
    Border = BoxBorder.Double,
    Padding = new Padding(1, 1),
};
farewell.BorderStyle = new Style(Color.Blue);
AnsiConsole.Write(farewell);
