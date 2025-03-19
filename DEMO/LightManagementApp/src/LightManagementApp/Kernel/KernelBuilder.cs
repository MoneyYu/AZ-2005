using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace LightManagementApp.Kernel
{
    /// <summary>
    /// Semantic Kernel 建構器類別，用於設置和配置 Semantic Kernel 實例
    /// </summary>
    public class KernelBuilder
    {
        /// <summary>
        /// 建立一個新的 Semantic Kernel 實例
        /// </summary>
        /// <returns>設置好的 Semantic Kernel 實例</returns>
        public static Microsoft.SemanticKernel.Kernel Build()
        {
            // 創建一個新的 Kernel 建構器
            var builder = Microsoft.SemanticKernel.Kernel.CreateBuilder();

            // 設定 OpenAI 服務 (使用環境變數或設定檔讀取金鑰)
            // 註：在實際使用時請設定您的 OpenAI API 金鑰
            // 這裡僅為演示目的，讓我們從環境變數獲取
            string apiKey = "GHT2kwhvNqrBsbXxyxjVKkEr7dZ3FsLIPnWOCWifxgPUzvIirF1DJQQJ99BAACHYHv6XJ3w3AAAAACOGibmP";


            if (!string.IsNullOrEmpty(apiKey))
            {
                builder.AddAzureOpenAIChatCompletion(
                    deploymentName: "o3-mini", // 部署名稱
                    endpoint: "https://demo-ai-svc-res929348950322.openai.azure.com/openai/deployments/o3-mini/chat/completions?api-version=2024-12-01-preview", // Azure OpenAI 端點
                    apiKey: apiKey
                );
                Console.WriteLine("已成功設定 OpenAI 服務");
            }
            else
            {
                Console.WriteLine("警告：找不到 OpenAI API 金鑰，將使用開發模式");
                // 使用本地模擬AI服務，輸出固定回應，僅用於開發測試
                builder.Services.AddSingleton(new DevelopmentOnlyAIService());
            }

            return builder.Build();
        }

        /// <summary>
        /// 開發模式 AI 服務，用於開發環境中無法連接真實 AI 服務時
        /// </summary>
        private class DevelopmentOnlyAIService
        {
            /// <summary>
            /// 開發模式下生成文本的方法
            /// </summary>
            public ValueTask<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default)
            {
                Console.WriteLine($"開發模式下的 AI 服務收到提示: {prompt}");
                
                // 針對燈光指令提供簡單的回應
                string response = prompt.ToLowerInvariant() switch
                {
                    var s when s.Contains("開燈") || s.Contains("打開燈") => "我會幫您開燈。",
                    var s when s.Contains("關燈") || s.Contains("關閉燈") => "我會幫您關燈。",
                    var s when s.Contains("調整亮度") => "我會幫您調整燈光亮度。",
                    var s when s.Contains("客廳") => "我會處理客廳的燈光。",
                    var s when s.Contains("廚房") => "我會處理廚房的燈光。",
                    var s when s.Contains("臥室") => "我會處理臥室的燈光。",
                    var s when s.Contains("查看燈") || s.Contains("列出燈") => "我會列出所有燈的狀態。",
                    _ => "我無法理解您的燈光控制請求，請再說一次。"
                };

                return new ValueTask<string>(response);
            }
        }
    }
}