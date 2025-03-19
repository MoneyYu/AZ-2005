using LightManagementApp.Models;
using LightManagementApp.Services.InMemory;

namespace LightManagementApp.Services
{
    /// <summary>
    /// 提供電燈管理相關功能的服務類別
    /// </summary>
    public class LightService
    {
        private readonly IInMemoryDataStore _dataStore;

        /// <summary>
        /// 初始化電燈服務
        /// </summary>
        /// <param name="dataStore">記憶體資料存儲實例</param>
        public LightService(IInMemoryDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        /// <summary>
        /// 獲取所有電燈
        /// </summary>
        /// <returns>所有電燈的集合</returns>
        public IEnumerable<Light> GetAllLights()
        {
            return _dataStore.GetAllLights();
        }

        /// <summary>
        /// 根據ID獲取特定電燈
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <returns>指定ID的電燈，若不存在則返回null</returns>
        public Light GetLightById(Guid id)
        {
            return _dataStore.GetLightById(id);
        }

        /// <summary>
        /// 根據名稱獲取電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <returns>符合名稱的電燈集合</returns>
        public IEnumerable<Light> GetLightsByName(string name)
        {
            return _dataStore.GetLightsByName(name);
        }

        /// <summary>
        /// 根據位置獲取電燈
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>在指定位置的所有電燈</returns>
        public IEnumerable<Light> GetLightsByLocation(string location)
        {
            return _dataStore.GetLightsByLocation(location);
        }

        /// <summary>
        /// 獲取所有開啟的電燈
        /// </summary>
        /// <returns>所有開啟狀態的電燈</returns>
        public IEnumerable<Light> GetAllOnLights()
        {
            return _dataStore.GetLightsByState(true);
        }

        /// <summary>
        /// 獲取所有關閉的電燈
        /// </summary>
        /// <returns>所有關閉狀態的電燈</returns>
        public IEnumerable<Light> GetAllOffLights()
        {
            return _dataStore.GetLightsByState(false);
        }

        /// <summary>
        /// 開啟特定電燈
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <returns>操作是否成功</returns>
        public bool TurnOnLight(Guid id)
        {
            var light = _dataStore.GetLightById(id);
            if (light == null)
                return false;

            light.IsOn = true;
            light.BrightnessLevel = 100;
            return _dataStore.UpdateLight(light);
        }

        /// <summary>
        /// 關閉特定電燈
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <returns>操作是否成功</returns>
        public bool TurnOffLight(Guid id)
        {
            var light = _dataStore.GetLightById(id);
            if (light == null)
                return false;

            light.IsOn = false;
            light.BrightnessLevel = 0;
            return _dataStore.UpdateLight(light);
        }

        /// <summary>
        /// 開啟指定位置的所有電燈
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>成功開啟的電燈數量</returns>
        public int TurnOnLightsByLocation(string location)
        {
            var lights = _dataStore.GetLightsByLocation(location);
            int count = 0;

            foreach (var light in lights)
            {
                if (!light.IsOn)
                {
                    light.IsOn = true;
                    light.BrightnessLevel = 100;
                    if (_dataStore.UpdateLight(light))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 關閉指定位置的所有電燈
        /// </summary>
        /// <param name="location">位置名稱</param>
        /// <returns>成功關閉的電燈數量</returns>
        public int TurnOffLightsByLocation(string location)
        {
            var lights = _dataStore.GetLightsByLocation(location);
            int count = 0;

            foreach (var light in lights)
            {
                if (light.IsOn)
                {
                    light.IsOn = false;
                    light.BrightnessLevel = 0;
                    if (_dataStore.UpdateLight(light))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 調整燈光亮度
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <param name="brightnessLevel">亮度等級(0-100)</param>
        /// <returns>操作是否成功</returns>
        public bool SetBrightness(Guid id, int brightnessLevel)
        {
            if (brightnessLevel < 0 || brightnessLevel > 100)
                throw new ArgumentOutOfRangeException(nameof(brightnessLevel), "亮度等級必須在0到100之間");

            var light = _dataStore.GetLightById(id);
            if (light == null)
                return false;

            light.BrightnessLevel = brightnessLevel;
            light.IsOn = brightnessLevel > 0;
            return _dataStore.UpdateLight(light);
        }

        /// <summary>
        /// 切換燈光狀態（開/關）
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <returns>新的狀態，成功為true（開啟）或false（關閉），失敗則為null</returns>
        public bool? ToggleLight(Guid id)
        {
            var light = _dataStore.GetLightById(id);
            if (light == null)
                return null;

            light.IsOn = !light.IsOn;
            light.BrightnessLevel = light.IsOn ? 100 : 0;
            
            bool success = _dataStore.UpdateLight(light);
            return success ? light.IsOn : null;
        }

        /// <summary>
        /// 新增電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <param name="location">位置</param>
        /// <param name="isOn">初始狀態</param>
        /// <returns>新增的電燈實體</returns>
        public Light AddLight(string name, string location, bool isOn = false)
        {
            var light = new Light(name, location, isOn);
            _dataStore.AddLight(light);
            return light;
        }

        /// <summary>
        /// 刪除電燈
        /// </summary>
        /// <param name="id">電燈ID</param>
        /// <returns>操作是否成功</returns>
        public bool RemoveLight(Guid id)
        {
            return _dataStore.RemoveLight(id);
        }
    }
}