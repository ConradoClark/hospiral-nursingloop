using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.UI;
using UnityEngine;

public class UI_Button_ColorOnSelect : BaseGameObject
{
    [field: SerializeField]
    public UIAction UIAction { get; private set; }

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public Color DefaultColor { get; private set; }

    [field: SerializeField]
    public Color SelectedColor { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIAction.OnSelected += UIAction_OnSelected;
        UIAction.OnDeselected += UIAction_OnDeselected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UIAction.OnSelected -= UIAction_OnSelected;
        UIAction.OnDeselected -= UIAction_OnDeselected;
    }

    private void UIAction_OnDeselected()
    {
        SpriteRenderer.color = DefaultColor;
    }

    private void UIAction_OnSelected(bool obj)
    {
        SpriteRenderer.color = SelectedColor;
    }
}
