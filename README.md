# MarkdownMemo #
### 概要 ###
- HTMLプレビュー機能付Markdownテキストエディタです。
- Markdown パーサに
[MarkdownSharp](http://code.google.com/p/markdownsharp/)を使用しています。
- WPF/C#で開発したWindows用デスクトップアプリケーションです。

#### 機能　###
Markdownエディタとしての機能も徐々に充実させて行く予定です。  

##### 実装済みの機能 #####
- 日本語文字コード自動判別
- リアルタイムHTMLプレビュー機能
- CSSファイルの適用(１種類のみ。現状、ユーザによる切り替えは不可)
- HTML形式での保存
- 参照画像、リンク要素の登録・保存

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

####　MVVMデザインパターン　####
このプロジェクトは、MVVMデザインパターンによるWPFアプリケーション開発について、実践、習得することを主な目的としています。
