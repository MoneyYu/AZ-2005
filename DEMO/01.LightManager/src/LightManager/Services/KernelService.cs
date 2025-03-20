using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using LightManager.Functions;

namespace LightManager.Services
{
    /// <summary>
    /// Semantic Kernel 服務類別
    /// </summary>
    public class KernelService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ChatHistory _chatHistory;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="azureOpenAIEndpoint">Azure OpenAI 端點</param>
        /// <param name="azureOpenAIKey">Azure OpenAI 金鑰</param>
        /// <param name="deploymentName">部署名稱</param>
        /// <param name="lightPlugin">電燈控制插件</param>
        public KernelService(string azureOpenAIEndpoint, string azureOpenAIKey, string deploymentName, LightPlugin lightPlugin)
        {
            // 設定 Azure OpenAI
            var builder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(
                    deploymentName,
                    azureOpenAIEndpoint,
                    azureOpenAIKey
                );

            _kernel = builder.Build();

            // 註冊插件
            _kernel.Plugins.AddFromObject(lightPlugin, "LightPlugin");

            // 設定聊天服務
            _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            _chatHistory = new ChatHistory();

            // 設定系統提示
            _chatHistory.AddSystemMessage(@"你是一個智慧家庭的燈光控制助手。
                你可以協助用戶控制家中的燈光，包括開關燈和調整亮度。
                當用戶提出要求時，請使用適當的函數來處理。
                使用中文回應用戶的問題。");
        }

        /// <summary>
        /// 處理用戶輸入
        /// </summary>
        /// <param name="input">用戶輸入的文字</param>
        /// <returns>處理結果</returns>
        public async Task<string> ProcessUserInputAsync(string input)
        {
            try
            {
                _chatHistory.AddUserMessage(input);

                // 使用 OpenAI 處理用戶輸入
                var result = await _chatCompletionService.GetChatMessageContentAsync(
                    _chatHistory,
                    kernel: _kernel,
                    executionSettings: new OpenAIPromptExecutionSettings
                    {
                        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                    });

                string response = result?.Content ?? "無回應";
                _chatHistory.AddAssistantMessage(response);

                return response;
            }
            catch (Exception ex)
            {
                return $"處理時發生錯誤：{ex.Message}";
            }
        }
    }
}