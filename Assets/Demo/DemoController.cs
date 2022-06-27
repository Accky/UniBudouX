using UniBudouX.Components;
using UnityEngine;

/// <summary>
/// DEMO program controller
/// </summary>
public class DemoController : MonoBehaviour
{
    [SerializeField] private WordWrapper wordWrapper;
    [SerializeField] private RectTransform bgRectTransform;
    
    // DEMO A
    [HeaderAttribute ("Button A")]
    [SerializeField][TextArea(6,6)] private string textA = @"";
    [SerializeField] private Vector2 textSizeA;

    public void OnButtonPushA()
    {
        if (wordWrapper == null) { return; }
        if (bgRectTransform == null) { return; }

        wordWrapper.SetText(textA);
        bgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textSizeA.x);
        bgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textSizeA.y);
    }
    
    // DEMO B
    [HeaderAttribute ("Button B")]
    [SerializeField][TextArea(6,6)] private string textB = @"";
    [SerializeField] private Vector2 textSizeB;

    public void OnButtonPushB()
    {
        if (wordWrapper == null) { return; }
        if (bgRectTransform == null) { return; }

        wordWrapper.SetText(textB);
        bgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textSizeB.x);
        bgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textSizeB.y);
    }
}
