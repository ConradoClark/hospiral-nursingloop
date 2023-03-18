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
    public string DisplayName { get; private set; }

    [field: SerializeField]
    public Color DisplayColor { get; private set; }

    public override void OnActivation()
    {
        CustomTags["Identifier"] = Identifier;
        CustomTags["DisplayName"] = DisplayName;
        CustomTags["DisplayColor"] = $"#{ColorUtility.ToHtmlStringRGBA(DisplayColor)}";
    }
}
