using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectButton : Button {
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(delegate { Select(); });
    }

    public void Select(bool selected = true)
    {
        image.color =
            selected ?
            colors.pressedColor :
            colors.normalColor;
    }
}
