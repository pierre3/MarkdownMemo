# MarkdownMemo #
### 概要 ###
- HTMLプレビュー機能付Markdownテキストエディタです。
- Markdown パーサに
[MarkdownSharp](http://code.google.com/p/markdownsharp/)を使用しています。
- WPF/C#で開発したWindows用デスクトップアプリケーションです。

#### 機能　###

##### 実装済み #####
- 日本語文字コード自動判別
- HTMLプレビュー機能
- CSSファイルの適用(１種類のみ。ユーザによる切り替えは不可)
- HTML形式での保存
- 参照画像、リンク要素の登録・保存

##### 実装予定 #####
- Gistとの連携機能
 + Gistにアップロード
 + Gistの編集、削除

- エディタ機能の強化
 + フォント設定
 + CSSの切り替え
 + シンタックスハイライト表示


### 開発 ###
Ver. 0.1.0.0

#### 開発環境 ####
- IDE
 + Microsoft Visual Studio Express 2012 for Windows Desktop
- Framework 
 + .Net Framework 4.5  
 + Windows Presentation Foundation (WPF)
- Library
 + [MarkdownSharp](http://code.google.com/p/markdownsharp/) V1.13
 + [Reactive Extensions for .Net](http://msdn.microsoft.com/en-us/data/gg577609.aspx) v1.0.10621 

#### MVVM ####
MVVMパターンの学習用に立ち上げたプロジェクトです。

