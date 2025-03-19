using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using LightManagementApp.Models;

namespace LightManagementApp.UI
{
    /// <summary>
    /// Console UI 類別，負責處理應用程式的使用者介面
    /// </summary>
    public class ConsoleUI
    {
        /// <summary>
        /// 顯示應用程式的標題和歡迎訊息
        /// </summary>
        public void ShowWelcome()
        {
            Console.Clear();
            var rule = new Rule("[yellow]電燈控制系統[/]");
            rule.Centered();
            rule.Border = BoxBorder.Double;
            
            AnsiConsole.Write(rule);
            AnsiConsole.MarkupLine("\n[bold]歡迎使用 Semantic Kernel 電燈控制系統[/]");
            AnsiConsole.MarkupLine("使用此系統，您可以透過自然語言控制家中的電燈。\n");
        }

        /// <summary>
        /// 顯示主選單
        /// </summary>
        public void ShowMainMenu()
        {
            AnsiConsole.MarkupLine("[green]請選擇操作：[/]");
            AnsiConsole.MarkupLine("1. [cyan]直接輸入自然語言指令[/]");
            AnsiConsole.MarkupLine("2. [cyan]查看所有電燈狀態[/]");
            AnsiConsole.MarkupLine("3. [cyan]查看開啟的電燈[/]");
            AnsiConsole.MarkupLine("4. [cyan]查看關閉的電燈[/]");
            AnsiConsole.MarkupLine("5. [cyan]離開系統[/]");
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// 讀取使用者輸入
        /// </summary>
        /// <param name="prompt">提示訊息</param>
        /// <returns>使用者輸入的內容</returns>
        public string ReadUserInput(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"[yellow]{prompt}[/]")
                    .PromptStyle("green")
                    .AllowEmpty());
        }

        /// <summary>
        /// 等待使用者按下任意鍵繼續
        /// </summary>
        public void WaitForKeyPress()
        {
            AnsiConsole.MarkupLine("\n[grey]按任意鍵繼續...[/]");
            Console.ReadKey(true);
        }

        /// <summary>
        /// 顯示電燈列表
        /// </summary>
        /// <param name="lights">電燈集合</param>
        /// <param name="title">表格標題</param>
        public void DisplayLights(IEnumerable<Light> lights, string title)
        {
            var lightList = lights.ToList();
            if (!lightList.Any())
            {
                AnsiConsole.MarkupLine("[yellow]沒有符合條件的電燈。[/]");
                return;
            }

            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.Expand();
            table.Title = new TableTitle(title);

            // 添加欄位
            table.AddColumn(new TableColumn("[cyan]序號[/]").Centered());
            table.AddColumn(new TableColumn("[cyan]名稱[/]").Centered());
            table.AddColumn(new TableColumn("[cyan]位置[/]").Centered());
            table.AddColumn(new TableColumn("[cyan]狀態[/]").Centered());
            table.AddColumn(new TableColumn("[cyan]亮度[/]").Centered());

            // 添加資料
            int index = 1;
            foreach (var light in lightList)
            {
                string statusMarkup = light.IsOn 
                    ? "[bold green]開啟[/]" 
                    : "[bold red]關閉[/]";
                
                table.AddRow(
                    index.ToString(),
                    light.Name,
                    light.Location,
                    statusMarkup,
                    $"{light.BrightnessLevel}%"
                );
                index++;
            }

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// 顯示操作結果訊息
        /// </summary>
        /// <param name="message">訊息內容</param>
        /// <param name="isSuccess">是否為成功訊息</param>
        public void DisplayMessage(string message, bool isSuccess = true)
        {
            var panel = new Panel(message);
            panel.Border = BoxBorder.Rounded;
            panel.Padding = new Padding(1, 1, 1, 1);
            
            if (isSuccess)
            {
                panel.Header = new PanelHeader("成功");
            }
            else
            {
                panel.Header = new PanelHeader("錯誤");
            }

            AnsiConsole.Write(panel);
        }

        /// <summary>
        /// 顯示 AI 回應
        /// </summary>
        /// <param name="response">AI 回應內容</param>
        public void DisplayAIResponse(string response)
        {
            var panel = new Panel(Markup.Remove(response));
            panel.Border = BoxBorder.Rounded;
            panel.Header = new PanelHeader("[bold]AI 助理回應[/]");
            panel.Padding = new Padding(1, 1, 1, 1);
            AnsiConsole.Write(panel);
        }

        /// <summary>
        /// 顯示載入動畫
        /// </summary>
        /// <param name="message">載入訊息</param>
        /// <param name="action">要執行的動作</param>
        public void ShowSpinner(string message, Action action)
        {
            AnsiConsole.Status()
                .Start($"[yellow]{message}[/]", ctx => 
                {
                    ctx.Spinner(Spinner.Known.Dots);
                    ctx.SpinnerStyle(Style.Parse("yellow"));
                    action();
                });
        }

        /// <summary>
        /// 顯示進度條
        /// </summary>
        /// <param name="message">進度訊息</param>
        /// <param name="totalSteps">總步驟數</param>
        public void ShowProgressBar(string message, int totalSteps)
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask($"[green]{message}[/]");
                    task.MaxValue = totalSteps;

                    for (int i = 0; i < totalSteps; i++)
                    {
                        task.Increment(1);
                        System.Threading.Thread.Sleep(50);
                    }
                });
        }
    }
}