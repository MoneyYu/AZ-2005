# Light Manager - 智慧燈光管理系統

這是一個使用 .NET 9 和 Semantic Kernel 1.42.0 開發的智慧燈光管理系統，可以通過自然語言來控制家中的燈光。

## 功能特點

- 使用自然語言控制燈光
- 支援開關燈功能
- 可調整燈光亮度
- 顯示燈光即時狀態
- 美化的控制台介面
- 使用 in-memory 資料庫儲存狀態

## 系統需求

- .NET 9 SDK
- Azure OpenAI 服務
- Visual Studio 2022 或 VS Code

## 安裝步驟

1. 複製專案到本機：
   ```
   git clone <repository-url>
   cd LightManager
   ```

2. 安裝相依套件：
   ```
   dotnet restore
   ```

3. 設定 Azure OpenAI：
   - 打開 `appsettings.json`
   - 填入您的 Azure OpenAI 設定：
     - Endpoint
     - ApiKey
     - DeploymentName

## 執行方式

1. 在專案根目錄執行：
   ```
   dotnet run
   ```

2. 程式啟動後，您可以看到目前所有燈光的狀態表格。

3. 輸入自然語言指令來控制燈光，例如：
   - "開啟客廳燈"
   - "把主臥燈的亮度調整到 50%"
   - "關閉所有燈"
   - "書房燈的狀態如何？"

4. 輸入 "exit" 可以結束程式。

## 使用範例

以下是一些常用的指令範例：

1. 開關燈：
   ```
   請開啟客廳燈
   幫我關掉書房的燈
   開啟所有的燈
   ```

2. 調整亮度：
   ```
   將客廳燈調整到 70% 亮度
   把主臥燈調暗一點，設為 30%
   ```

3. 查詢狀態：
   ```
   客廳燈現在是什麼狀態？
   顯示所有燈的狀態
   ```

## 技術架構

- .NET 9
- Semantic Kernel 1.42.0
- Entity Framework Core InMemory 9.0.3
- Spectre.Console 0.49.1
- Azure OpenAI

## 注意事項

- 這是一個示範應用程式，使用 in-memory 資料庫，資料不會永久保存
- 請確保您有有效的 Azure OpenAI 服務訂閱
- 需要在 appsettings.json 中設定正確的 Azure OpenAI 連接資訊