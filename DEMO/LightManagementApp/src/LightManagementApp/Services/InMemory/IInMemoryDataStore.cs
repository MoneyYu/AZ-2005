using System;
using System.Collections.Generic;
using LightManagementApp.Models;

namespace LightManagementApp.Services.InMemory
{
    /// <summary>
    /// 定義記憶體資料儲存的介面
    /// </summary>
    public interface IInMemoryDataStore
    {
        /// <summary>
        /// 取得所有電燈
        /// </summary>
        /// <returns>所有電燈的集合</returns>
        IEnumerable<Light> GetAllLights();

        /// <summary>
        /// 根據ID取得特定電燈
        /// </summary>
        /// <param name="id">電燈的唯一識別碼</param>
        /// <returns>找到的電燈，若未找到則返回null</returns>
        Light GetLightById(Guid id);

        /// <summary>
        /// 根據名稱取得電燈
        /// </summary>
        /// <param name="name">電燈名稱</param>
        /// <returns>符合名稱的電燈集合</returns>
        IEnumerable<Light> GetLightsByName(string name);

        /// <summary>
        /// 根據位置取得電燈
        /// </summary>
        /// <param name="location">電燈位置</param>
        /// <returns>在該位置的所有電燈</returns>
        IEnumerable<Light> GetLightsByLocation(string location);

        /// <summary>
        /// 根據狀態取得電燈
        /// </summary>
        /// <param name="isOn">電燈開啟狀態</param>
        /// <returns>符合指定狀態的所有電燈</returns>
        IEnumerable<Light> GetLightsByState(bool isOn);

        /// <summary>
        /// 新增電燈到資料庫
        /// </summary>
        /// <param name="light">要新增的電燈</param>
        void AddLight(Light light);

        /// <summary>
        /// 更新電燈狀態
        /// </summary>
        /// <param name="light">更新後的電燈資訊</param>
        /// <returns>是否成功更新</returns>
        bool UpdateLight(Light light);

        /// <summary>
        /// 移除電燈
        /// </summary>
        /// <param name="id">要移除的電燈識別碼</param>
        /// <returns>是否成功移除</returns>
        bool RemoveLight(Guid id);
    }
}