using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Pooling;
using UnityEngine;

public class PickupObject : EffectPoolable
{
    [field:SerializeField]
    public string Identifier { get; private set; }

    [field: SerializeField]
    [field:Multiline]
    public string DisplayName { get; private set; }

    [field: SerializeField]
    public Color DisplayColor { get; private set; }

    [field: SerializeField]
    public bool StageObject { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!StageObject) return;
        Initialize();
        OnActivation();
    }

    public override void OnActivation()
    {
        CustomTags["Identifier"] = Identifier;
        CustomTags["DisplayName"] = DisplayName;
        CustomTags["DisplayColor"] = $"#{ColorUtility.ToHtmlStringRGBA(DisplayColor)}";
        OnEffectOver+= OnOnEffectOver;
    }

    private void OnOnEffectOver()
    {
        if (Pool==null) gameObject.SetActive(false);
    }
}
