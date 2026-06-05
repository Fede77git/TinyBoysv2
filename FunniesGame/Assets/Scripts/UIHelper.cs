using UnityEngine;
using UnityEngine.UI;

public static class UIHelper
{
    public static void ShowWinBackground(Text textWin)
    {
        if (textWin != null && textWin.transform.parent != null)
        {
            GameObject bg = new GameObject("WinBackground");
            bg.transform.SetParent(textWin.transform.parent, false);
            bg.transform.SetSiblingIndex(textWin.transform.GetSiblingIndex());
            Image img = bg.AddComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.7f);
            RectTransform rect = bg.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
