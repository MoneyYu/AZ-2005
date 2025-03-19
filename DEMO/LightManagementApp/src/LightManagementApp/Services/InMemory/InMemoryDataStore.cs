using LightManagementApp.Models;

namespace LightManagementApp.Services.InMemory
{
    /// <summary>
    /// 記憶體資料儲存的實現類別
    /// </summary>
    public class InMemoryDataStore : IInMemoryDataStore
    {
        private readonly List<Light> _lights;

        /// <summary>
        /// 初始化記憶體資料儲存
        /// </summary>
        public InMemoryDataStore()
        {
            _lights = new List<Light>();
        }

        /// <summary>
        /// 取得所有電燈
        /// </summary>
        /// <returns>所有電燈的集合</returns>
        public IEnumerable<Light> GetAllLights()
        {
            return _lights.ToList(); // 傳回複製的清單以避免外部修改
        }

        /// <summary>
        /// 根據ID取得特定電燈
        /// </summary>
        /// <param name="id">電燈的唯一識別碼</param>
        /// <returns>找到的電燈，若未找到則返回null</returns>
        public Light GetLightById(Guid id)
        {
            return _lights.FirstOrDefault(l => l.Id == id);
        }

        /// <summary>
        /// 根據名稱取得電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <returns>符合名稱的電燈集合</returns>
        public IEnumerable<Light> GetLightsByName(string name)
        {
            return _lights.Where(l => l.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// 根據位置取得電燈
        /// </summary>
        /// <param name="location">電燈位置</param>
        /// <returns>在該位置的所有電燈</returns>
        public IEnumerable<Light> GetLightsByLocation(string location)
        {
            return _lights.Where(l => l.Location.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// 根據狀態取得電燈
        /// </summary>
        /// <param name="isOn">電燈開啟狀態</param>
        /// <returns>符合指定狀態的所有電燈</returns>
        public IEnumerable<Light> GetLightsByState(bool isOn)
        {
            return _lights.Where(l => l.IsOn == isOn).ToList();
        }

        /// <summary>
        /// 新增電燈到資料庫
        /// </summary>
        /// <param name="light">要新增的電燈</param>
        public void AddLight(Light light)
        {
            if (light == null)
                throw new ArgumentNullException(nameof(light));
            
            _lights.Add(light);
        }

        /// <summary>
        /// 更新電燈狀態
        /// </summary>
        /// <param name="light">更新後的電燈資訊</param>
        /// <returns>是否成功更新</returns>
        public bool UpdateLight(Light light)
        {
            if (light == null)
                throw new ArgumentNullException(nameof(light));
            
            var existingLight = GetLightById(light.Id);
            if (existingLight == null)
                return false;

            // 更新已有燈的各屬性
            existingLight.Name = light.Name;
            existingLight.Location = light.Location;
            existingLight.IsOn = light.IsOn;
            existingLight.BrightnessLevel = light.BrightnessLevel;
            
            return true;
        }

        /// <summary>
        /// 移除電燈
        /// </summary>
        /// <param name="id">要移除的電燈識別碼</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveLight(Guid id)
        {
            var light = GetLightById(id);
            if (light == null)
                return false;
            
            return _lights.Remove(light);
        }
    }
}