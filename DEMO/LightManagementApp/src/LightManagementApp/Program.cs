using Microsoft.SemanticKernel;
using LightManagementApp.Kernel;
using LightManagementApp.Kernel.Plugins;
using LightManagementApp.Services;
using LightManagementApp.Services.InMemory;
using LightManagementApp.UI;

namespace LightManagementApp
{
    /// <summary>
    /// 應用程式主要入口點
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 應用程式的進入點
        /// </summary>
        /// <param name="args">命令列參數</param>
        /// <returns>非同步任務</returns>
        public static async Task Main(string[] args)
        {
            try
            {
                // 初始化用戶介面
                var consoleUI = new ConsoleUI();

                // 顯示歡迎訊息
                consoleUI.ShowWelcome();

                // 初始化資料存儲
                IInMemoryDataStore dataStore = new InMemoryDataStore();
                var dataSeeder = new DataSeeder(dataStore);

                // 生成樣本資料
                consoleUI.ShowSpinner("正在初始化系統和生成範例資料...", () =>
                {
                    dataSeeder.SeedData();
                });

                // 初始化服務
                var lightService = new LightService(dataStore);

                // 初始化 Semantic Kernel
                Microsoft.SemanticKernel.Kernel kernel = null!;

                consoleUI.ShowSpinner("正在初始化 AI 核心...", () =>
                {
                    kernel = KernelBuilder.Build();

                    // 註冊燈光插件
                    var lightPlugin = new LightPlugin(lightService);
                    kernel.ImportPluginFromObject(lightPlugin, "LightPlugin");
                });

                // 主迴圈
                bool exit = false;
                while (!exit)
                {
                    consoleUI.ShowMainMenu();
                    var choice = consoleUI.ReadUserInput("請輸入選項 (1-5):");

                    switch (choice)
                    {
                        case "1": // 自然語言指令
                            await HandleNaturalLanguageCommand(consoleUI, kernel);
                            break;

                        case "2": // 顯示所有電燈
                            var allLights = lightService.GetAllLights();
                            consoleUI.DisplayLights(allLights, "所有電燈");
                            consoleUI.WaitForKeyPress();
                            break;

                        case "3": // 顯示開啟的電燈
                            var onLights = lightService.GetAllOnLights();
                            consoleUI.DisplayLights(onLights, "已開啟的電燈");
                            consoleUI.WaitForKeyPress();
                            break;

                        case "4": // 顯示關閉的電燈
                            var offLights = lightService.GetAllOffLights();
                            consoleUI.DisplayLights(offLights, "已關閉的電燈");
                            consoleUI.WaitForKeyPress();
                            break;

                        case "5": // 離開系統
                            exit = true;
                            consoleUI.DisplayMessage("感謝使用電燈控制系統，再見！");
                            break;

                        default:
                            consoleUI.DisplayMessage("無效的選擇，請再試一次。", false);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"應用程式發生錯誤: {ex.Message}");
                Console.WriteLine($"錯誤詳情: {ex}");
            }
        }

        /// <summary>
        /// 處理自然語言指令
        /// </summary>
        /// <param name="consoleUI">UI 實例</param>
        /// <param name="kernel">Semantic Kernel 實例</param>
        /// <returns>非同步任務</returns>
        private static async Task HandleNaturalLanguageCommand(ConsoleUI consoleUI, Microsoft.SemanticKernel.Kernel kernel)
        {
            var command = consoleUI.ReadUserInput("請輸入您的燈光控制指令 (例如: \"開啟客廳的燈\"):");

            if (string.IsNullOrWhiteSpace(command))
            {
                consoleUI.DisplayMessage("未輸入任何指令。", false);
                return;
            }

            string response = "";

            try
            {
                // 執行處理指令的函數
                var args = new KernelArguments
                {
                    { "command", command }
                };

                var result = await kernel.InvokeAsync("LightPlugin", "ProcessLightCommand", args);
                response = result.ToString();
            }
            catch (Exception ex)
            {
                response = $"處理指令時發生錯誤: {ex.Message}";
            }

            consoleUI.DisplayAIResponse(response);
            consoleUI.WaitForKeyPress();
        }
    }
}
