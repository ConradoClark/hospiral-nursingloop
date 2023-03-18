using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class ReplaceObjectOnInteraction : BaseGameObject
{
    [field: SerializeField]
    public InteractiveObject InteractiveObject { get; private set; }

    [field: SerializeField]
    public string[] IdentifierRequirement { get; private set; }

    [field: SerializeField]
    public ScriptPrefab ReplacementObject { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents, PlayerInteraction>
            (PlayerEvents.OnInteractionButtonPressed, OnInteractionButtonPressed);
    }

    private void OnInteractionButtonPressed(PlayerInteraction interaction)
    {
        if (!InteractiveObject.InContact || !interaction.IsHoldingObject) return;
        if (IdentifierRequirement.All(req => !interaction.HeldObject.HasTag("Identifier", req))) return;

        interaction.HeldObject.Pool.Release(interaction.HeldObject);
        if (!ReplacementObject.TrySpawnEffect(interaction.transform.position + interaction.HoldObjectOffset, out var obj)) return;

        obj.Component.transform.SetParent(interaction.transform);
        interaction.HeldObject = obj;
    }
}
