# Semantic Kernel 電燈控制系統

使用 .NET 9 和 Semantic Kernel 1.42.0 建立的智能電燈控制系統，透過自然語言來管理家庭電燈的狀態與開關。

## 專案特色

- 使用 Microsoft Semantic Kernel 1.42.0 處理自然語言指令
- 美化的控制台界面 (使用 Spectre.Console 0.49.1)
- 資料存儲在記憶體資料庫中
- 10個預設電燈範例資料
- 支持多種電燈操作：開/關、查詢狀態、依位置控制等

## 系統需求

- .NET 9 SDK
- 若要使用真實 AI 功能：OpenAI API 金鑰 (需設置在環境變數 `OPENAI_API_KEY`)

## 如何啟動

1. 確保已安裝 .NET 9 SDK
2. 複製專案到本機
3. 開啟命令提示字元或終端機，切換到專案資料夾
4. 執行以下命令以建置和運行應用程式：

```bash
cd [專案資料夾路徑]
dotnet build
dotnet run
```

5. 若要使用真實的 OpenAI 功能，請設置環境變數：

Windows:
```
set OPENAI_API_KEY=你的OpenAI_API金鑰
```

Linux/macOS:
```
export OPENAI_API_KEY=你的OpenAI_API金鑰
```

## 使用方法

啟動後，系統會顯示主選單，您可以：

1. **直接輸入自然語言指令**：例如「開啟客廳的燈」、「關閉臥室的燈」、「查看所有電燈的狀態」等。
2. **查看所有電燈狀態**：顯示所有電燈的詳細資訊。
3. **查看開啟的電燈**：只顯示目前開啟的電燈。
4. **查看關閉的電燈**：只顯示目前關閉的電燈。
5. **離開系統**：結束應用程式。

## 自然語言指令範例

- 「開啟客廳的燈」
- 「關閉臥室的所有燈」
- 「查看廚房的燈」
- 「打開所有燈」
- 「關閉所有燈」
- 「客廳的燈光狀態如何？」

## 專案結構

- `Models/` - 資料模型 (如 Light 類別)
- `Services/` - 業務邏輯服務
  - `InMemory/` - 記憶體資料儲存相關類別
- `Kernel/` - Semantic Kernel 相關功能
  - `Plugins/` - Semantic Kernel 擴充功能
- `UI/` - 使用者介面相關類別