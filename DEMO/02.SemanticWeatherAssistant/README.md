# 智能天氣助手

這是一個使用 .NET 9 和 Semantic Kernel 1.42.0 開發的智能天氣助手 Console 應用程式。本應用程式展示了如何使用 OpenAPI 插件來實現自然語言查詢天氣信息的功能。

## 功能特點

- 自然語言查詢：使用自然語言提出問題，系統能智能識別您的意圖
- 自動函式呼叫：AI 模型可以自動呼叫系統功能
- 即時天氣資訊：獲取指定城市的當前天氣和未來預報
- 美化的終端機輸出：使用 Spectre.Console 提供更好的使用者體驗

## 安裝需求

- .NET 9 SDK
- Azure OpenAI API 金鑰
- Weather.gov API 存取權限

## 安裝步驟

1. 克隆此專案到本地：
```bash
git clone [repository-url]
cd SemanticWeatherAssistant
```

2. 安裝相依套件：
```bash
dotnet restore
```

3. 在 appsettings.json 中設定您的 Azure OpenAI API 金鑰：
```json
{
  "AzureOpenAI": {
    "ApiKey": "您的金鑰",
    "Endpoint": "您的端點"
  }
}
```

## 執行方式

```bash
dotnet run
```

## 使用範例

1. 查詢當前天氣：
```
輸入: 請問台北現在的天氣如何？
輸出: 
台北市目前天氣情況：
溫度：25°C
濕度：65%
天氣狀況：晴時多雲
```

2. 查詢未來天氣預報：
```
輸入: 明天舊金山會下雨嗎？
輸出:
舊金山明天天氣預報：
早上：陰天，降雨機率 30%
下午：多雲，降雨機率 10%
氣溫：最高 22°C，最低 15°C
```

3. 一般查詢：
```
輸入: 現在幾點了？
輸出: 現在時間是 14:30:25
```

## 專案結構

- `Program.cs`: 程式進入點
- `Models/`: 資料模型定義
- `Services/`: 核心服務實作
- `Utils/`: 工具類別