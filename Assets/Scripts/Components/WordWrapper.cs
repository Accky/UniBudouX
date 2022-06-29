using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace UniBudouX.Components
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TMP_Text))]
    public class WordWrapper : MonoBehaviour
    {
        private TMP_Text tmpText = null;
        private RectTransform rectTransform = null;
        private List<string> chunks = null;
        
        private Rect rectCache = Rect.zero;
        private bool hasTextChanged = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            tmpText = GetComponent<TMP_Text>();

            if (tmpText == null) { return;}
            
            // 初期文字列をチャンク化
            chunks = Parser.Parse(tmpText.text);
        }

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
        
        private void Update()
        {
            UpdateTextWidth();
        }

        private void UpdateTextWidth()
        {
            // テキストに変化があったときには必ず処理を行う & rectに変更がなければ更新を行わない
            if (!hasTextChanged && rectTransform.rect.Equals(rectCache)) { return; }
            
            rectCache = rectTransform.rect;

            var builder = new StringBuilder();
            var accumulator = string.Empty;
            
            foreach (var chunk in chunks)
            {
                if (chunk.Length == 0) { continue; }
                
                var newText = accumulator + chunk;
                var v = tmpText.GetPreferredValues(newText);
                
                if (v.x < rectCache.width)
                {
                    accumulator = newText;
                }
                else
                {
                    builder.AppendLine(accumulator);
                    accumulator = chunk;
                }
            }
            
            builder.Append(accumulator);
            
            tmpText.text = builder.ToString();
            tmpText.ForceMeshUpdate(true, true);
            hasTextChanged = false;
        }
    }
}