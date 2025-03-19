using System;
using Microsoft.SemanticKernel;
using LightManager.Models;
using System.ComponentModel;
using System.Linq;

namespace LightManager.Functions
{
    /// <summary>
    /// 電燈控制插件，提供 Semantic Kernel 使用的功能
    /// </summary>
    public class LightPlugin
    {
        private readonly LightDbContext _context;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        public LightPlugin(LightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 開啟指定的燈
        /// </summary>
        [KernelFunction]
        [Description("開啟指定名稱的燈。輸入燈的名稱（例如：客廳主燈、書房燈），系統會將該燈打開。回傳開啟成功訊息或找不到燈的錯誤訊息。")]
        [return: Description("回傳開啟成功訊息（例如：已開啟客廳主燈）或找不到燈的錯誤訊息")]
        public string TurnOn(
            [Description("要開啟的燈的名稱，例如：客廳主燈、書房燈")] string name)
        {
            var light = _context.Lights.FirstOrDefault(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (light == null)
                return $"找不到名稱為 {name} 的燈";

            light.IsOn = true;
            light.LastStateChange = DateTime.Now;
            _context.SaveChanges();
            return $"已開啟 {name}";
        }

        /// <summary>
        /// 關閉指定的燈
        /// </summary>
        [KernelFunction]
        [Description("關閉指定名稱的燈。輸入燈的名稱（例如：客廳主燈、書房燈），系統會將該燈關閉。回傳關閉成功訊息或找不到燈的錯誤訊息。")]
        [return: Description("回傳關閉成功訊息（例如：已關閉客廳主燈）或找不到燈的錯誤訊息")]
        public string TurnOff(
            [Description("要關閉的燈的名稱，例如：客廳主燈、書房燈")] string name)
        {
            var light = _context.Lights.FirstOrDefault(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (light == null)
                return $"找不到名稱為 {name} 的燈";

            light.IsOn = false;
            light.LastStateChange = DateTime.Now;
            _context.SaveChanges();
            return $"已關閉 {name}";
        }

        /// <summary>
        /// 設定指定燈的亮度
        /// </summary>
        [KernelFunction]
        [Description("設定指定燈的亮度級別。輸入燈的名稱和想要設定的亮度（0-100的數字），系統會調整該燈的亮度。亮度0表示最暗，100表示最亮。")]
        [return: Description("回傳設定成功訊息（例如：已將客廳主燈的亮度設為 50%）或錯誤訊息")]
        public string SetBrightness(
            [Description("要調整亮度的燈的名稱，例如：客廳主燈、書房燈")] string name,
            [Description("要設定的亮度值，範圍是 0-100 的整數，0 表示最暗，100 表示最亮")] int brightness)
        {
            if (brightness < 0 || brightness > 100)
                return "亮度必須在 0 到 100 之間";

            var light = _context.Lights.FirstOrDefault(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (light == null)
                return $"找不到名稱為 {name} 的燈";

            light.Brightness = brightness;
            light.LastStateChange = DateTime.Now;
            _context.SaveChanges();
            return $"已將 {name} 的亮度設為 {brightness}%";
        }

        /// <summary>
        /// 取得指定燈的狀態
        /// </summary>
        [KernelFunction]
        [Description("查詢指定燈的當前狀態，包括開關狀態、亮度和位置。輸入燈的名稱，系統會回傳該燈的詳細狀態資訊。")]
        [return: Description("回傳燈的狀態資訊，包含開關狀態、亮度百分比和位置，或找不到燈的錯誤訊息")]
        public string GetStatus(
            [Description("要查詢狀態的燈的名稱，例如：客廳主燈、書房燈")] string name)
        {
            var light = _context.Lights.FirstOrDefault(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (light == null)
                return $"找不到名稱為 {name} 的燈";

            return $"{name} 目前狀態：{(light.IsOn ? "開啟" : "關閉")}，亮度：{light.Brightness}%，位置：{light.Location}";
        }

        /// <summary>
        /// 根據位置開啟所有燈
        /// </summary>
        [KernelFunction]
        [Description("根據指定的位置開啟該位置的所有燈。輸入位置名稱（例如：客廳、主臥室），系統會開啟該位置的所有燈具。適合一次控制同一空間的多盞燈。")]
        [return: Description("回傳開啟成功訊息，包含已開啟的燈具數量，或找不到該位置的錯誤訊息")]
        public string TurnOnByLocation(
            [Description("要開啟燈具的位置名稱，例如：客廳、主臥室、廚房")] string location)
        {
            var lights = _context.Lights.Where(l => l.Location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (!lights.Any())
                return $"找不到位於 {location} 的燈";

            int count = 0;
            foreach (var light in lights)
            {
                light.IsOn = true;
                light.LastStateChange = DateTime.Now;
                count++;
            }
            
            _context.SaveChanges();
            return $"已開啟 {location} 的 {count} 盞燈";
        }

        /// <summary>
        /// 根據位置關閉所有燈
        /// </summary>
        [KernelFunction]
        [Description("根據指定的位置關閉該位置的所有燈。輸入位置名稱（例如：客廳、主臥室），系統會關閉該位置的所有燈具。適合一次控制同一空間的多盞燈。")]
        [return: Description("回傳關閉成功訊息，包含已關閉的燈具數量，或找不到該位置的錯誤訊息")]
        public string TurnOffByLocation(
            [Description("要關閉燈具的位置名稱，例如：客廳、主臥室、廚房")] string location)
        {
            var lights = _context.Lights.Where(l => l.Location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (!lights.Any())
                return $"找不到位於 {location} 的燈";

            int count = 0;
            foreach (var light in lights)
            {
                light.IsOn = false;
                light.LastStateChange = DateTime.Now;
                count++;
            }
            
            _context.SaveChanges();
            return $"已關閉 {location} 的 {count} 盞燈";
        }

        /// <summary>
        /// 開啟所有燈
        /// </summary>
        [KernelFunction]
        [Description("開啟整個住家的所有燈具。這個功能會一次性開啟所有位置的所有燈。適合要一次開啟所有燈的場合，例如回家時或安全檢查時。")]
        [return: Description("回傳開啟成功訊息，包含新開啟的燈具數量，如果所有燈都已經開啟則回傳相應訊息")]
        public string TurnOnAll()
        {
            var lights = _context.Lights.ToList();
            int count = 0;
            
            foreach (var light in lights)
            {
                if (!light.IsOn)
                {
                    light.IsOn = true;
                    light.LastStateChange = DateTime.Now;
                    count++;
                }
            }
            
            _context.SaveChanges();
            return count > 0 ? $"已開啟所有 {count} 盞燈" : "所有燈都已經是開啟狀態";
        }

        /// <summary>
        /// 關閉所有燈
        /// </summary>
        [KernelFunction]
        [Description("關閉整個住家的所有燈具。這個功能會一次性關閉所有位置的所有燈。適合要一次關閉所有燈的場合，例如外出時或就寢時。")]
        [return: Description("回傳關閉成功訊息，包含新關閉的燈具數量，如果所有燈都已經關閉則回傳相應訊息")]
        public string TurnOffAll()
        {
            var lights = _context.Lights.ToList();
            int count = 0;
            
            foreach (var light in lights)
            {
                if (light.IsOn)
                {
                    light.IsOn = false;
                    light.LastStateChange = DateTime.Now;
                    count++;
                }
            }
            
            _context.SaveChanges();
            return count > 0 ? $"已關閉所有 {count} 盞燈" : "所有燈都已經是關閉狀態";
        }
    }
}