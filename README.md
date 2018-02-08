# StyleCopCodeChecker
Unity のエディタ拡張で利用できるコードチェッカー。

# 動作環境
Mac/Win 両対応
Unity 2017 +  .NET 4.6

# 使い方
1. このソースをダウンロードしたら、ディレクトリ名を StyleCopCodeChecker に変更してください。
(例 : StyleCopCodeChecker-ver1.0.0 -> StyleCopCodeChecker)

1. StyleCopCodeChecker ディレクトリを Assets の直下に配置してください。(それを想定してパス指定してしまっているので)

1. https://github.com/Nylle/StyleCop.Console をダウンロードして、ディレクトリ名を StyleCop.Console に変更してください。(例 : StyleCop.Console-master -> StyleCop.Console)

1. 名前変更したディレクトリをStyleCopCodeChecker/Editor/StyleCop/ の直下に配置してください。

1. AssemblyInfo.cs がエラーを吐くので、中身を全てコメントアウトしてください。(ファイルを削除してしまうと動作しない)

1. StyleCopCodeChecker/Editor/CodeChecker/CodeCheckPath.cs を編集して、コードチェッカーをかけたいディレクトリを指定してください。

# 参考にしたページ
- C#静的解析によるコーディング規約チェッカーを作った話
https://developers.cyberagent.co.jp/blog/archives/4257/

- Unity でコーディングスタイルを機械的にチェックする
https://qiita.com/takawyi/items/e15a1184c126b5d5ace8
