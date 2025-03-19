## 01
``` markdown
幫我使用 .NET 9 C# 建立一個使用 Semantic Kernel 1.42.0 版本，來管理電燈狀態與開關燈，使用 Console 來輸入和輸出。

簡單範例可以參考:　https://studyhost.blogspot.com/2024/02/semantic-kernel.html

- 使用者輸入 Prompt
- 使用 Spectre.Console 0.49.1 版本來美化 Console 輸出

系統的網站架構與設計應該要有:
- 資料存在 in-memory 資料庫，不需要實作登入功能。
  - 使用 Microsoft.EntityFrameworkCore.InMemory 9.0.3
- 提供 README.md 檔案，說明如何啟動網站。
  - README.md 檔案要包含如何安裝、啟動和使用範例
  - 需要有使用範例
- 程式碼必須包含適當的註解。
  - 所有的 Class 與 Method 一定要有註解
- 使用 Azure OpenAI
- Semantic Kernel 啟用自動函式呼叫並使用自動函式呼叫

先不要執行，請先產出一個執行計畫與檔案文件目錄結構給我看，輸出請使用繁體中文
```

## 02
``` markdown
幫我使用 .NET 9 C# 建立一個使用 Semantic Kernel 1.42.0 版本，用來展示 OpenAPI plugin，目的請幫我想一個，使用 Console 來輸入和輸出。

- 使用者輸入 Prompt
- 使用 Spectre.Console 0.49.1 版本來美化 Console 輸出
- **自然語言查詢**：使用自然語言提出問題，系統能智能識別您的意圖
- **自動函式呼叫**：AI 模型可以自動呼叫系統功能，例如獲取當前時間或日期資訊
- **即時天氣資訊**：獲取指定城市的當前天氣和未來預報

系統的網站架構與設計應該要有:
- 提供 README.md 檔案，說明如何啟動網站。
  - README.md 檔案要包含如何安裝、啟動和使用範例
  - 需要有使用範例
    - 包含輸入與預期輸出
- 程式碼必須包含適當的註解。
  - 所有的 Class 與 Method 一定要有註解
- 使用 Microsoft.SemanticKernel.Plugins.OpenApi 1.42.0 版本的 OpenAPI plugin
- 使用 Microsoft.SemanticKernel.Connectors.AzureOpenAI 1.42.0 版本的 Azure OpenAI
- Semantic Kernel 啟用自動函式呼叫並使用自動函式呼叫
  - 使用 https://api.weather.gov/openapi.json 來作為 OpenAPI plugin 來源
  - 範例:
    ``` csharp
      await kernel.ImportPluginFromOpenApiAsync(
        pluginName: "lights",
        uri: new Uri("https://example.com/v1/swagger.json"),
        executionParameters: new OpenApiFunctionExecutionParameters()
        {
            // Determines whether payload parameter names are augmented with namespaces.
            // Namespaces prevent naming conflicts by adding the parent parameter name
            // as a prefix, separated by dots
            EnablePayloadNamespacing = true
        }
      );
    ```

先不要執行，請先產出一個執行計畫與檔案文件目錄結構給我看，輸出請使用繁體中文
```