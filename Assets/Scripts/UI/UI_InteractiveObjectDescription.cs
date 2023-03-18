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

internal class UI_InteractiveObjectDescription : BaseGameObject
{
    [field: SerializeField]
    public TMP_Text Description { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Description.text = "";
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents, InteractiveObject>(PlayerEvents.OnInteractiveObjectHover, OnHover);
        this.ObserveEvent<PlayerEvents, InteractiveObject>(PlayerEvents.OnInteractiveObjectLeave, OnLeave);
    }

    private void OnHover(InteractiveObject obj)
    {
        Description.text = obj.Description;
    }

    private void OnLeave(InteractiveObject obj)
    {
        if (Description.text == obj.Description)
        {
            Description.text = "";
        }
    }
}
