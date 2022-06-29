# UniBudouX

![cap1.png](https://github.com/Accky/UniBudouX/blob/main/doc/img/cap1.png)

UniBudouX は [BudouX](https://github.com/google/budoux) (the machine learning powered line break organizer tool)をUnity上で使用できるように移植したものです。

Note:
このプロジェクトには [BudouX](https://github.com/google/budoux)プロジェクトのアセットが含まれます。

## Unity Version

2021.3.0f1 で動作確認しています。

## Demo

デモを動かす場合はプロジェクトをそのままClone or ダウンロードしてください。DemoSceneを開いて実行することで、UniBudouX+WordWrapperの動作を確認できます。
シーン内にあるSampleTextオブジェクトにWordWrapperが追加されており、このコンポーネントがRectTransformの幅に合わせて改行調整を行っています。

Button AとButton Bが配置されており、それぞれのボタンを押すことで表示テキストと表示範囲を変更することができます。

![cap5.gif](https://github.com/Accky/UniBudouX/blob/main/doc/img/cap5.gif)

またエディタで実行中に枠の大きさを手動で変更すると以下のような挙動になります。

![cap4.gif](https://github.com/Accky/UniBudouX/blob/main/doc/img/cap4.gif)

## Usage

### Import

Package Managerの`Add package from git url...`を選び`https://github.com/Accky/UniBudouX.git?path=Assets/Scripts `を入力してください。

![cap2.png](https://github.com/Accky/UniBudouX/blob/main/doc/img/cap2.png)
![cap3.png](https://github.com/Accky/UniBudouX/blob/main/doc/img/cap3.png)

または、`Packages/manifest.json`に`"com.accky.unibudoux": "https://github.com/Accky/UniBudouX.git?path=Assets/Scripts" `を追記してください。

### Scripting

UniBudouXネームスペースをusingすることでUniBudouXのパーサーを利用できるようになります。

```csharp
using UniBudouX;
```

Parser.Parse関数を使用して文章をチャンクごとに分割することができます。

```csharp
// textをチャンクに分割する
string text = "一人の下人が、羅生門の下で雨やみを待っていた。";
List<string> chunks = Parser.Parse(text);
// Result => {"一人の", "下人が、", "羅生門の", "下で", "雨やみを", "待っていた。"}
```

分割した文字列は、WordWrapperコンポーネントを使用することで、TextMeshProUGUIの文字を自動改行させることができるようになっています。

使用するモデルを変更する場合には以下のようにModeを変更するか、学習させたjsonファイルを読み込ませます。

```csharp
// 日本語モデルを利用する
Parser.Mode = Parser.KnbcMode.Japanese;
// 中国/韓国語モデルを利用する
Parser.Mode = Parser.KnbcMode.ZnHans;
```

```csharp
[SerializeField] private TextAsset jsonModel;

private void Start()
{
    // Original Model Test
    if (jsonModel == null) { return;}
    Parser.MakeModel(jsonModel.text);
}
```

独自モデルの出力については、[BucouXのCLIを利用して出力](https://github.com/google/budoux#building-a-custom-model)してください。

#### WordWrapperコンポーネント

RectTransformの幅に応じて読みやすい場所に改行コードを挟み込むコンポーネントです。TextMeshProUGUIを持つゲームオブジェクトに追加して使用します。

現在はしかしながらTMPro_EventManager.TEXT_CHANGED_EVENTの実行タイミングの関係で、このコンポーネントをのSetText関数を介して文字列を変更する必要があります。

```csharp
public class WordWrapper : MonoBehaviour
{
    /// <summary>
    /// 表示文字列を変更する
    /// </summary>
    public void SetText(string text)
    {
        if (tmpText == null) { return; }

        // TODO: 本当なら文字列が変更されたときのコールバックで処理で行いたい。
        // しかしながらTMPro_EventManager.TEXT_CHANGED_EVENTはCanvas更新時にコールバックが実行されるため、
        // 文字列が変更されたタイミングで改行処理を行う(再度文字列を変更する)と再度コールバックが呼ばれてしまい
        // 文字列のチャンク化処理が何度も呼ばれてしまうためこの方法で対応。
        chunks = Parser.Parse(text);
        tmpText.text = text;
        hasTextChanged = true;
    }
    ...
```
## Assets

* [SourceHansSans-light Font](https://github.com/adobe-fonts/source-han-sans/tree/release)
* [青空文庫 - 羅生門 / 芥川龍之介](https://www.aozora.gr.jp/cards/000879/files/127_15260.html)