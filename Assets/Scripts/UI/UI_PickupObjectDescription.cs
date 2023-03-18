using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using TMPro;
using UnityEngine;

public class UI_PickupObjectDescription : BaseGameObject
{
    [field: SerializeField]
    public TMP_Text ObjectName { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        ObjectName.text = "";
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents,IPoolableComponent>(PlayerEvents.OnItemPickup, OnPickup);
        this.ObserveEvent<PlayerEvents, IPoolableComponent>(PlayerEvents.OnItemDiscard, OnPickup);
    }

    private void OnPickup(IPoolableComponent obj)
    {
        if (obj == null) ObjectName.text = "";
        else
        {
            ObjectName.text = obj.HasTag("DisplayName") ? obj.CustomTags["DisplayName"] : "";
            ObjectName.color = obj.HasTag("DisplayColor") && 
                               ColorUtility.TryParseHtmlString(obj.CustomTags["DisplayColor"], out var color)
                               ? color : Color.white;
        }
    }
}
