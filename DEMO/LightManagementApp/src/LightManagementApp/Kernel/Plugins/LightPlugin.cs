using System.ComponentModel;
using System.Text;
using Microsoft.SemanticKernel;
using LightManagementApp.Services;

namespace LightManagementApp.Kernel.Plugins
{
    /// <summary>
    /// 燈光管理插件，為 Semantic Kernel 提供燈光控制功能
    /// </summary>
    public class LightPlugin
    {
        private readonly LightService _lightService;

        /// <summary>
        /// 初始化燈光插件
        /// </summary>
        /// <param name="lightService">燈光服務</param>
        public LightPlugin(LightService lightService)
        {
            _lightService = lightService ?? throw new ArgumentNullException(nameof(lightService));
        }

        /// <summary>
        /// 獲取所有電燈的狀態
        /// </summary>
        /// <returns>所有電燈狀態的描述</returns>
        [KernelFunction, Description("獲取所有電燈的當前狀態")]
        public string GetAllLightsStatus()
        {
            var lights = _lightService.GetAllLights().ToList();
            if (!lights.Any())
                return "目前系統中沒有登記的電燈。";

            var sb = new StringBuilder();
            sb.AppendLine("所有電燈的狀態:");
            
            foreach (var light in lights)
            {
                sb.AppendLine($"- {light.Name} ({light.Location}): {(light.IsOn ? "開啟" : "關閉")} - 亮度: {light.BrightnessLevel}%");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 獲取所有開啟的電燈狀態
        /// </summary>
        /// <returns>所有開啟電燈的描述</returns>
        [KernelFunction, Description("獲取所有目前開啟中的電燈")]
        public string GetOnLights()
        {
            var lights = _lightService.GetAllOnLights().ToList();
            if (!lights.Any())
                return "目前沒有任何電燈開啟。";

            var sb = new StringBuilder();
            sb.AppendLine("目前開啟的電燈:");
            
            foreach (var light in lights)
            {
                sb.AppendLine($"- {light.Name} ({light.Location}) - 亮度: {light.BrightnessLevel}%");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 獲取所有關閉的電燈狀態
        /// </summary>
        /// <returns>所有關閉電燈的描述</returns>
        [KernelFunction, Description("獲取所有目前關閉的電燈")]
        public string GetOffLights()
        {
            var lights = _lightService.GetAllOffLights().ToList();
            if (!lights.Any())
                return "目前沒有任何電燈關閉，所有電燈都是開啟狀態。";

            var sb = new StringBuilder();
            sb.AppendLine("目前關閉的電燈:");
            
            foreach (var light in lights)
            {
                sb.AppendLine($"- {light.Name} ({light.Location})");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根據位置獲取電燈狀態
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>指定位置電燈的描述</returns>
        [KernelFunction, Description("獲取指定位置的所有電燈狀態")]
        public string GetLightsByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return "請提供有效的位置名稱。";

            var lights = _lightService.GetLightsByLocation(location).ToList();
            if (!lights.Any())
                return $"在 '{location}' 找不到任何電燈。";

            var sb = new StringBuilder();
            sb.AppendLine($"'{location}' 的電燈狀態:");
            
            foreach (var light in lights)
            {
                sb.AppendLine($"- {light.Name}: {(light.IsOn ? "開啟" : "關閉")} - 亮度: {light.BrightnessLevel}%");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 開啟指定位置的所有電燈
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>操作結果描述</returns>
        [KernelFunction, Description("開啟指定位置的所有電燈")]
        public string TurnOnLightsByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return "請提供有效的位置名稱。";

            int count = _lightService.TurnOnLightsByLocation(location);
            if (count == 0)
                return $"在 '{location}' 找不到任何可開啟的電燈，可能所有燈都已經開啟或該位置沒有電燈。";

            return $"已成功開啟 '{location}' 的 {count} 個電燈。";
        }

        /// <summary>
        /// 關閉指定位置的所有電燈
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>操作結果描述</returns>
        [KernelFunction, Description("關閉指定位置的所有電燈")]
        public string TurnOffLightsByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return "請提供有效的位置名稱。";

            int count = _lightService.TurnOffLightsByLocation(location);
            if (count == 0)
                return $"在 '{location}' 找不到任何可關閉的電燈，可能所有燈都已經關閉或該位置沒有電燈。";

            return $"已成功關閉 '{location}' 的 {count} 個電燈。";
        }

        /// <summary>
        /// 開啟指定名稱的電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <returns>操作結果描述</returns>
        [KernelFunction, Description("根據名稱開啟電燈")]
        public string TurnOnLightByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "請提供有效的電燈名稱。";

            var lights = _lightService.GetLightsByName(name).ToList();
            if (!lights.Any())
                return $"找不到名稱包含 '{name}' 的電燈。";

            int successCount = 0;
            foreach (var light in lights.Where(l => !l.IsOn))
            {
                if (_lightService.TurnOnLight(light.Id))
                    successCount++;
            }

            if (successCount == 0)
                return $"名稱包含 '{name}' 的電燈都已經是開啟狀態。";

            return $"已成功開啟 {successCount} 個名稱包含 '{name}' 的電燈。";
        }

        /// <summary>
        /// 關閉指定名稱的電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <returns>操作結果描述</returns>
        [KernelFunction, Description("根據名稱關閉電燈")]
        public string TurnOffLightByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "請提供有效的電燈名稱。";

            var lights = _lightService.GetLightsByName(name).ToList();
            if (!lights.Any())
                return $"找不到名稱包含 '{name}' 的電燈。";

            int successCount = 0;
            foreach (var light in lights.Where(l => l.IsOn))
            {
                if (_lightService.TurnOffLight(light.Id))
                    successCount++;
            }

            if (successCount == 0)
                return $"名稱包含 '{name}' 的電燈都已經是關閉狀態。";

            return $"已成功關閉 {successCount} 個名稱包含 '{name}' 的電燈。";
        }

        /// <summary>
        /// 解析並執行自然語言燈光控制命令
        /// </summary>
        /// <param name="command">用戶的自然語言指令</param>
        /// <returns>執行結果的描述</returns>
        [KernelFunction, Description("處理自然語言的燈光控制命令")]
        public string ProcessLightCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return "請提供有效的燈光控制命令。";
            
            command = command.ToLowerInvariant();

            // 解析命令中的位置信息
            string[] locations = { "客廳", "廚房", "臥室", "主臥室", "客房", "浴室", "走廊", "門廊" };
            string targetLocation = locations.FirstOrDefault(loc => command.Contains(loc));

            // 解析是否為開啟或關閉命令
            bool isTurnOn = command.Contains("開") || command.Contains("打開") || command.Contains("開啟");
            bool isTurnOff = command.Contains("關") || command.Contains("關閉") || command.Contains("熄滅");
            bool isToggle = command.Contains("切換") || command.Contains("反轉") || command.Contains("轉換");
            bool isStatus = command.Contains("狀態") || command.Contains("查看") || command.Contains("列出");
            bool isAllLights = command.Contains("全部") || command.Contains("所有");

            // 基於解析結果執行操作
            if (isStatus)
            {
                if (!string.IsNullOrEmpty(targetLocation))
                    return GetLightsByLocation(targetLocation);
                else if (isAllLights)
                    return GetAllLightsStatus();
                else
                    return "請指定您想查看哪些電燈的狀態。";
            }
            else if (isTurnOn)
            {
                if (!string.IsNullOrEmpty(targetLocation))
                    return TurnOnLightsByLocation(targetLocation);
                else if (isAllLights)
                {
                    var allLights = _lightService.GetAllOffLights();
                    foreach (var light in allLights)
                    {
                        _lightService.TurnOnLight(light.Id);
                    }
                    return "已開啟所有電燈。";
                }
                else
                    return "請指定您想開啟哪些電燈。";
            }
            else if (isTurnOff)
            {
                if (!string.IsNullOrEmpty(targetLocation))
                    return TurnOffLightsByLocation(targetLocation);
                else if (isAllLights)
                {
                    var allLights = _lightService.GetAllOnLights();
                    foreach (var light in allLights)
                    {
                        _lightService.TurnOffLight(light.Id);
                    }
                    return "已關閉所有電燈。";
                }
                else
                    return "請指定您想關閉哪些電燈。";
            }
            else if (isToggle && !string.IsNullOrEmpty(targetLocation))
            {
                var lights = _lightService.GetLightsByLocation(targetLocation).ToList();
                bool allOn = lights.All(l => l.IsOn);
                
                if (allOn)
                    return TurnOffLightsByLocation(targetLocation);
                else
                    return TurnOnLightsByLocation(targetLocation);
            }
            else
            {
                return $"無法理解您的命令：'{command}'。請嘗試使用明確的開燈/關燈指令，例如「開啟客廳的燈」或「關閉所有燈」。";
            }
        }
    }
}