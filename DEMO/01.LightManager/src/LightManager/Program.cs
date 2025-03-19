using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LightManager.Models;
using LightManager.Services;
using LightManager.Functions;

namespace LightManager
{
    /// <summary>
    /// 程式進入點類別
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主程式進入點
        /// </summary>
        public static async Task Main()
        {
            // 讀取設定檔
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 設定資料庫
            var dbContextOptions = new DbContextOptionsBuilder<LightDbContext>()
                .UseInMemoryDatabase("LightManager")
                .Options;

            // 建立資料庫上下文
            using var dbContext = new LightDbContext(dbContextOptions);

            // 初始化一些測試資料
            InitializeTestData(dbContext);

            // 建立服務實例
            var lightPlugin = new LightPlugin(dbContext);
            var kernelService = new KernelService(
                configuration["AzureOpenAI:Endpoint"]!,
                configuration["AzureOpenAI:ApiKey"]!,
                configuration["AzureOpenAI:DeploymentName"]!,
                lightPlugin
            );
            var consoleService = new ConsoleService(dbContext);

            // 顯示歡迎畫面
            consoleService.ShowWelcome();

            // 主要程式迴圈
            while (true)
            {
                try
                {
                    // 顯示目前燈光狀態
                    consoleService.ShowLightsStatus();

                    // 取得用戶輸入
                    var input = consoleService.GetUserInput();

                    // 檢查是否要結束程式
                    if (input.ToLower() == "exit")
                        break;

                    // 處理用戶輸入
                    var response = await kernelService.ProcessUserInputAsync(input);
                    consoleService.ShowResponse(response);
                }
                catch (Exception ex)
                {
                    consoleService.ShowError(ex.Message);
                }
            }
        }

        /// <summary>
        /// 初始化測試資料
        /// </summary>
        private static void InitializeTestData(LightDbContext dbContext)
        {
            if (!dbContext.Lights.Any())
            {
                dbContext.Lights.AddRange(
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "客廳主燈",
                        Location = "客廳",
                        IsOn = false,
                        Brightness = 100,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "客廳角落燈",
                        Location = "客廳",
                        IsOn = false,
                        Brightness = 70,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "客廳桌燈",
                        Location = "客廳",
                        IsOn = true,
                        Brightness = 60,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "主臥主燈",
                        Location = "主臥室",
                        IsOn = false,
                        Brightness = 100,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "主臥床頭燈",
                        Location = "主臥室",
                        IsOn = true,
                        Brightness = 40,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "書房燈",
                        Location = "書房",
                        IsOn = false,
                        Brightness = 100,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "廚房吊燈",
                        Location = "廚房",
                        IsOn = false,
                        Brightness = 100,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "廚房櫥櫃燈",
                        Location = "廚房",
                        IsOn = true,
                        Brightness = 80,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "浴室燈",
                        Location = "浴室",
                        IsOn = false,
                        Brightness = 90,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "陽台燈",
                        Location = "陽台",
                        IsOn = false,
                        Brightness = 100,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "玄關燈",
                        Location = "玄關",
                        IsOn = true,
                        Brightness = 75,
                        LastStateChange = DateTime.Now
                    },
                    new Light
                    {
                        Id = Guid.NewGuid(),
                        Name = "兒童房燈",
                        Location = "兒童房",
                        IsOn = false,
                        Brightness = 85,
                        LastStateChange = DateTime.Now
                    }
                );
                dbContext.SaveChanges();
            }
        }
    }
}
