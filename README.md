# UniBudouX

![cap1.png](/doc/img/cap1.png)

UniBudouX is [BudouX](https://github.com/google/budoux) (the machine learning powered line break organizer tool) for Unity.

Note:
This project contains the deliverables of the [BudouX](https://github.com/google/budoux) project.
## Unity Version

Checi in Unity 2021.3.0f1.

## Import

You can add `https://github.com/Accky/UniBudouX.git?path=Assets/Scripts ` to Package Manager.

![cap2.png](/doc/img/cap2.png)
![cap3.png](/doc/img/cap3.png)

Or, add `"com.accky.unibudoux": "https://github.com/Accky/UniBudouX.git?path=Assets/Scripts" ` to `Packages/manifest.json`.

## Usage

You can use the `UniBudouX Parser` by using the UniBudouX namespace.

```csharp
using UniBudouX;
```

Use the `Parser.Parse` method to split the text into chunks.

```csharp
// separete text
string text = "一人の下人が、羅生門の下で雨やみを待っていた。";
List<string> chunks = Parser.Parse(text);
// Result => {"一人の", "下人が、", "羅生門の", "下で", "雨やみを", "待っていた。"}
```

WordWrapper component uses chunks. They automatically break the characters in TextMeshProUGUI.

If you need to change the model, change the Mode or use the trained json file.

```csharp
// Use Japanese model
Parser.Mode = Parser.KnbcMode.Japanese;
// Use ZnHans model
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

[Use the BucouX CLI](https://github.com/google/budoux#building-a-custom-model) to create your custom model.

### WordWrapper Component

The WordWrapper component embeds a line feed code according to the width of the RectTransform. Used by adding to a game object that has TextMeshProUGUI.

However, due to the timing of execution of TMPro_EventManager.TEXT_CHANGED_EVENT, it is necessary to use the SetText function of this component.

```csharp
public class WordWrapper : MonoBehaviour
{
    public void SetText(string text)
    {
        if (tmpText == null) { return; }

        // TODO: Actually, I would like to process this with a callback on the text is changed.
        chunks = Parser.Parse(text);
        tmpText.text = text;
        hasTextChanged = true;
    }
    ...
```

## Demo

Clone or download the project to run the demo. You can check the operation of UniBudou X + WordWrapper in DemoScene.

You can change the display text and range by pressing the `Button A` and` Button B` buttons.

![cap5.gif](/doc/img/cap5.gif)

If you resize the frame while it's running in the editor, you'll see something like this:

![cap4.gif](/doc/img/cap4.gif)

## Assets

* [SourceHansSans-light Font](https://github.com/adobe-fonts/source-han-sans/tree/release)
* [青空文庫 - 羅生門 / 芥川龍之介](https://www.aozora.gr.jp/cards/000879/files/127_15260.html)