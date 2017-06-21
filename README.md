#### ChangeLog 
##### Last Update 2017-06-22

###### v1.4.7
- new：新增兩個網站的支援
- 版號以後就只用三碼

###### v1.4.6.9
- Fix：ck http to https

###### v1.4.6.8
- Fix：ck抓取規則調整，測試樣本不多，請網友有問題再反應囉

###### v1.4.6.7
- Fix：en的頁數在10頁以下時，無法正確取得最後一頁的頁數

######  v1.4.6.6
- Fix：ck因頁數的html修改而無法分析的問題

###### v1.4.6.5
- 修正某屋的排序，但我忘記當初為什麼要加排序那段了...

###### v1.4.6
- 調整轻DownloaderXPath路徑

###### v1.4.6.3
- 修改伊ny的下載方式

###### v1.4.6.2
- 調整卡X的XPath路徑

###### v1.4.6.1
- 修改其中一個插件作者及書名的判斷，但仍然只能涵蓋大部份的。
- 新增格式化檔名不能為空白 系統列增加顯示視窗選項(也許能改善某部份無法再彈出視窗?)


###### v1.4.6.0 
- 新增自訂檔名格式， 例%Title%─%Author%輸出就是書名─作者， 但如果沒辦法分辨出書名跟作者就沒辦法了

###### v1.4.5.1 
- 修復輕小說X庫?

###### v1.4.5
- 對卡X諾插件做了調整，以後將會比較少發生失敗。

###### v1.4.4.X
- 對插件做了小部份修改。

###### v1.4.4
- 縮至系統列設定改為最小化時判定，按關閉(X)時將直接關閉。 
- 增加黃X屋及燃X支援。 筆X閣轉為繁體(只能做逐字轉換，無法進行詞意的轉換 )。

###### v1.4.3 
- 增加對筆X閣及輕小說X庫的支援。 
- 以上兩個下載速度都很慢(如果錯誤次數沒增加，請耐心等候)。 
- 分析錯誤可能要重新加入任務。

###### v1.4.2
- 增加Unicode跟UTF- 8編碼選擇，請從其他→設定去修改。

###### v1.4
- 增加錯誤重試次數。 修正可能漏掉一部份章節的問題。

###### v1.3
- UI設定上做了點修改，以免看不懂(雖然我也不知道這樣有沒有比較好XD)。 
- 修正訂閱檢查到新章節不會下載。
- 修正右鍵開啟檔案時，如果檔案不存在，程式會出錯。 修正下載目錄不存在時，會建立資料夾。

###### v1.2
- 修正訂閱狀態卡在檢查訂閱。

###### v1.1
- 修正錯誤的判斷導致不會更新訂閱。 
- 修正icon的資源沒被移除。 
- 訂閱更新週期不能輸入小於1的數字。 
- icon大小修改為16*16。

###### v1.0
- 多任務下載，每任務只有一線程，單一任務速度並不快。 
- 任務可自訂起始(樓層數)位置及結束位置。 
- 提供排版功能(不可選用)，段落之間插入空白行，縮排兩個全形空白。
- 訂閱模式每十分鐘會執行分析的動作，看有沒有新的文章，如果目前進度不是最新的，會執行下載的動作。 
- 可縮小至系統列。 
- 關閉時會暫停所有任務，等下次開啟，可接續下載。
- 複製連結時，如果可以下載，會直接加入到任務列表。
