using System;

namespace LightManager.Models
{
    /// <summary>
    /// 代表一個電燈裝置的模型類別
    /// </summary>
    public class Light
    {
        /// <summary>
        /// 取得或設定電燈的唯一識別碼
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 取得或設定電燈的名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 取得或設定電燈的狀態（開/關）
        /// </summary>
        public bool IsOn { get; set; }

        /// <summary>
        /// 取得或設定電燈的亮度（0-100）
        /// </summary>
        public int Brightness { get; set; }

        /// <summary>
        /// 取得或設定電燈的位置描述
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// 取得或設定上次狀態更改的時間
        /// </summary>
        public DateTime LastStateChange { get; set; }
    }
}