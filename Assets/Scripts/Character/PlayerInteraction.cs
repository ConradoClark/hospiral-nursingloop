using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Events;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : BaseGameRunner
{
    [field: SerializeField]
    public InputActionReference InteractionButton { get; private set; }

    [field: SerializeField]
    public Vector3 HoldObjectOffset { get; private set; }

    private IPoolableComponent _heldObject;
    public IPoolableComponent HeldObject
    {
        get => _heldObject;
        set
        {
            _heldObject = value;
            _heldObjectPublisher.PublishEvent(
                _heldObject != null ? PlayerEvents.OnItemPickup : PlayerEvents.OnItemDiscard, _heldObject);
        }
    }
    public bool IsHoldingObject => HeldObject != null;

    public event Action OnPressedInteractionButton;
    private IEventPublisher<PlayerEvents, PlayerInteraction> _eventPublisher;
    private IEventPublisher<PlayerEvents, IPoolableComponent> _heldObjectPublisher;

    protected override void OnAwake()
    {
        base.OnAwake();
        _eventPublisher = this.RegisterAsEventPublisher<PlayerEvents, PlayerInteraction>();
        _heldObjectPublisher = this.RegisterAsEventPublisher<PlayerEvents, IPoolableComponent>();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (InteractionButton.action.WasPerformedThisFrame())
        {
            OnPressedInteractionButton?.Invoke();
            _eventPublisher.PublishEvent(PlayerEvents.OnInteractionButtonPressed, this);
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
