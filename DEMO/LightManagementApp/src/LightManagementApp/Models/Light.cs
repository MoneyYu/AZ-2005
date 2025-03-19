using System;

namespace LightManagementApp.Models
{
    /// <summary>
    /// 代表一個電燈設備的實體模型
    /// </summary>
    public class Light
    {
        /// <summary>
        /// 電燈的唯一識別碼
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 電燈的名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 電燈所在的位置或房間
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 電燈目前的狀態（開或關）
        /// </summary>
        public bool IsOn { get; set; }

        /// <summary>
        /// 電燈的亮度等級，範圍從0到100
        /// </summary>
        public int BrightnessLevel { get; set; }

        /// <summary>
        /// 創建新的電燈實例
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <param name="location">電燈位置</param>
        /// <param name="isOn">初始狀態，預設為關閉</param>
        public Light(string name, string location, bool isOn = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            IsOn = isOn;
            BrightnessLevel = isOn ? 100 : 0;
        }

        /// <summary>
        /// 傳回電燈狀態的描述
        /// </summary>
        /// <returns>包含電燈名稱、位置和狀態的字串描述</returns>
        public override string ToString()
        {
            return $"{Name} ({Location}): {(IsOn ? "開啟" : "關閉")} - 亮度：{BrightnessLevel}%";
        }
    }
}