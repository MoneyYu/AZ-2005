using LightManagementApp.Models;

namespace LightManagementApp.Services.InMemory
{
    /// <summary>
    /// 資料種子生成器，用於初始化範例資料
    /// </summary>
    public class DataSeeder
    {
        private readonly IInMemoryDataStore _dataStore;

        /// <summary>
        /// 初始化資料種子生成器
        /// </summary>
        /// <param name="dataStore">記憶體資料庫儲存服務</param>
        public DataSeeder(IInMemoryDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        /// <summary>
        /// 產生範例資料
        /// </summary>
        public void SeedData()
        {
            // 創建10個燈的範例資料
            var sampleLights = new List<Light>
            {
                new Light("客廳主燈", "客廳", false),
                new Light("客廳角落燈", "客廳", true),
                new Light("廚房天花板燈", "廚房", false),
                new Light("廚房櫥櫃燈", "廚房", true),
                new Light("主臥室燈", "主臥室", false),
                new Light("主臥閱讀燈", "主臥室", true),
                new Light("客房燈", "客房", false),
                new Light("浴室燈", "浴室", false),
                new Light("走廊燈", "走廊", true),
                new Light("門廊燈", "門廊", true)
            };

            // 將範例資料添加到資料庫
            foreach (var light in sampleLights)
            {
                _dataStore.AddLight(light);
            }

            Console.WriteLine($"已成功創建 {sampleLights.Count} 個樣本燈具。");
        }
    }
}