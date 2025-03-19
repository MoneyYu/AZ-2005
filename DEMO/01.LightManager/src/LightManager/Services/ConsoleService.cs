using Spectre.Console;
using LightManager.Models;

namespace LightManager.Services
{
    /// <summary>
    /// 控制台介面服務類別
    /// </summary>
    public class ConsoleService
    {
        private readonly LightDbContext _context;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        public ConsoleService(LightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 顯示歡迎訊息
        /// </summary>
        public void ShowWelcome()
        {
            AnsiConsole.Write(
                new FigletText("Light Manager")
                    .Centered()
                    .Color(Color.Yellow));

            AnsiConsole.MarkupLine("[yellow]歡迎使用智慧燈光管理系統！[/]");
            AnsiConsole.MarkupLine("[grey]輸入 'exit' 可以結束程式[/]");
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// 顯示所有燈的狀態
        /// </summary>
        public void ShowLightsStatus()
        {
            var table = new Table();
            table.AddColumn("名稱");
            table.AddColumn("狀態");
            table.AddColumn("亮度");
            table.AddColumn("位置");
            table.AddColumn("最後更新時間");

            foreach (var light in _context.Lights)
            {
                table.AddRow(
                    light.Name,
                    light.IsOn ? "[green]開啟[/]" : "[red]關閉[/]",
                    $"{light.Brightness}%",
                    light.Location,
                    light.LastStateChange.ToString("yyyy-MM-dd HH:mm:ss")
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// 取得用戶輸入
        /// </summary>
        /// <returns>用戶輸入的文字</returns>
        public string GetUserInput()
        {
            return AnsiConsole.Ask<string>("[blue]請輸入指令：[/]");
        }

        /// <summary>
        /// 顯示回應訊息
        /// </summary>
        /// <param name="message">要顯示的訊息</param>
        public void ShowResponse(string message)
        {
            AnsiConsole.MarkupLine($"[green]系統回應：[/]{message}");
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// 顯示錯誤訊息
        /// </summary>
        /// <param name="error">錯誤訊息</param>
        public void ShowError(string error)
        {
            AnsiConsole.MarkupLine($"[red]錯誤：{error}[/]");
            AnsiConsole.WriteLine();
        }
    }
}