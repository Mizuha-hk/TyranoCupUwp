# TyranoCupUwp
## アプリリリースページ
https://mizuha-hk.github.io/TyranoReleasePage/

## アプリ要件

### フレームワーク
`Universal Windows Platform (UWP)`

### ビルドバージョン

ターゲット バージョン

`Windows 11, version 22H2(10.0; ビルド 22621)`

最小バージョン

`Windows 10, version 1809 (10.0; ビルド 17763)`

## ディレクトリ構造図
※ソリューションをVisual Studioで読み込んだ際のディレクトリ構成であるため，github上ではこの構成になっていない．
```
TyranoCupUwpApp
  ├ src
  │  ├ dev 
  │  │  └ shared
  │  │     └ TyranoCupUwpApp.Shared
  │  │        ├ Models
  │  │        │  └ *Model.cs
  │  │        └ *Service.cs       //ライブラリ定義
  │  │
  │  └ TyranoCupUwpApp
  │     ├ Assets
  │     │   └ *.png    //アプリに使用する画像類
  │     ├ Strings      //各言語ごとのリソースファイル
  │     │   ├ en-US
  │     │   │   └ *.resw
  │     │   └ ja-JP
  │     │       └ *.resw
  │     ├ Views
  │     │   ├ NavViews  //NavigationView Content
  │     │   │   ├ *.xaml
  │     │   │   └ *.xaml.cs
  │     │   ├ UI
  │     │   │  └ Controls  //Custom User Controls(コンポーネント)
  │     │   │      ├ *.xaml
  │     │   │      └ *.xaml.cs
  │     │   ├ MainPage.xaml  //メインウィンドウのUI定義
  │     │   └ MainPage.xaml.cs  //メインウィンドウの処理系
  │     ├ App.xaml
  │     ├ App.xaml.cs
  │     ├ Package.appxmanifest  //アプリ用リソース生成用
  │     └ Package.appinstaller  //アプリインストーラー構成ファイル
  │
  └ test
      └ TyranoCupUwpApp.Test  //ライブラリテストプロジェクト
          ├ UnitTest.cs
          ├ UnitTestApp.xaml
          └ UnitTestApp.xaml.cs
```

### TyranoCupUwpApp

UI/UX定義プロジェクト
- Views : MainPage.xamlをベースに，UI要素を定義する
- NavViews : NavigationViewのナビゲーション先のページを定義
- UI/Controls ： UserControl(コンポーネント)定義/再利用可能なUI/UX要素を定義する

### TyrenoCupUwpApp.Shared
内部処理，ライブラリ定義
- *Service.cs : メインプロジェクトから呼び出し可能なライブラリの定義ファイル
- Models : ライブラリの処理や，データバインディング等に使用するモデルクラス

### TyranoCupUwpApp.Test
UIとの繋ぎこみを終えずとも，Sharedで定義したライブラリをテストすることが可能なプロジェクト
以下のようにコードを書くと，テストエクスプローラーから実行結果を見ることができる．
```cs
using SampleNamespace

namespace TyrnoCupUwpApp.Test
{
  [TestClass]
  public class UnitTest
  {
    [TestMethod]
    public void TestMethod1()
    {
      var service1 = new SampleService();

      service.SampleMethod();
    }
  }
}

```
単体テストの使い方は以下のドキュメントを参照

https://learn.microsoft.com/ja-jp/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-windows-store-apps?view=vs-2022
