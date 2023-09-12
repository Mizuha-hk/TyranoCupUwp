# TyranoCupUwp

### TyranoCupUwpApp

UI/UX定義プロジェクト

### TyrenoCupUwpApp.Shared

内部処理，ライブラリ定義

### TyranoCupUwpApp.Test

ライブラリテスト用プロジェクト

### TyranoCupUwpApp.Package

リリース用パッケージ生成プロジェクト

## ディレクトリ構造図
```
TyranoCupUwpApp
  ├ src
  │  ├ dev 
  │  │  ├ shared
  │  │  │   └ TyranoCupUwpApp.Shared
  │  │  │       ├ Models
  │  │  │       │  └ *Model.cs
  │  │  │       └ *Service0.cs       //ライブラリ定義
  │  │  │
  │  │  └ TyranoCupUwpApp
  │  │      ├ Assets
  │  │      │   └ *.png    //アプリに使用する画像類
  │  │      ├ Strings      //各言語ごとのリソースファイル
  │  │      │   ├ en-US
  │  │      │   │   └ *.resw
  │  │      │   └ ja-JP
  │  │      │       └ *.resw
  │  │      ├ Views
  │  │      │   ├ NavViews  //NavigationView Content
  │  │      │   │   ├ *.xaml
  │  │      │   │   └ *.xaml.cs
  │  │      │   ├ UI
  │  │      │   │  └ Controls  //Custom User Controls(コンポーネント)
  │  │      │   │      ├ *.xaml
  │  │      │   │      └ *.xaml.cs
  │  │      │   ├ MainPage.xaml  //メインウィンドウのUI定義
  │  │      │   └ MainPage.xaml.cs  //メインウィンドウの処理系
  │  │      ├ App.xaml
  │  │      ├ App.xaml.cs
  │  │      └ Package.appxmanifest  //アプリ用リソース生成用
  │  │
  │  └ TyranoCupUwpApp.Package  //インストーラー構成用プロジェクト
  │      ├ AppPackages  //配布用静的サイトに配置
  │      │   ├ .git~
  │      │   ├ index.html
  │      │   ├ Package.appinstaller
  │      │   └ 0.0.0.1_Test~
  │      ├ Images
  │      │   └ *.png
  │      ├ Package.appinstaller
  │      └ Package.appxmanifest  //パッケージ生成
  │
  └ test
      └ TyranoCupUwpApp.Test  //ライブラリテストプロジェクト
          ├ UnitTest.cs
          ├ UnitTestApp.xaml
          └ UnitTestApp.xaml.cs
```
