using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Interfaces.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class UseObjectOnInteraction : BaseGameObject
{
    [field: SerializeField]
    public InteractiveObject InteractiveObject { get; private set; }

    [field: SerializeField]
    public string[] IdentifierRequirement { get; private set; }

    public event Action<IPoolableComponent> OnObjectUsed;
    private IEventPublisher<PlayerEvents, IPoolableComponent> _eventPublisher;

    protected override void OnAwake()
    {
        base.OnAwake();
        _eventPublisher = this.RegisterAsEventPublisher<PlayerEvents, IPoolableComponent>();
    }

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

        OnObjectUsed?.Invoke(interaction.HeldObject);
        _eventPublisher.PublishEvent(PlayerEvents.OnItemUse, interaction.HeldObject);

        interaction.HeldObject = null;
    }
}
