using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Events;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class InteractiveObject : BaseGameRunner
{
    [field: SerializeField]
    public ScriptIdentifier ContactTrigger { get; private set; }

    [field: SerializeField]
    [field: Multiline]
    public string Description { get; set; }

    public bool InContact { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private LichtPhysicsObject _physicsObject;
    private IEventPublisher<PlayerEvents, InteractiveObject> _eventPublisher;
    protected override void OnAwake()
    {
        base.OnAwake();
        _physicsObject = GetComponent<LichtPhysicsObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _eventPublisher = this.RegisterAsEventPublisher<PlayerEvents, InteractiveObject>();

        if (_physicsObject != null)
        {
            _physicsObject.AddCustomObject(this);
        }
    }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var hasTouched = false;
        while (ComponentEnabled)
        {
            if (_spriteRenderer == null)
            {
                InContact = false;
                yield return TimeYields.WaitOneFrameX;
                yield break;
            }

            var trigger = _physicsObject.GetPhysicsTrigger(ContactTrigger);

            if (trigger)
            {
                hasTouched = true;
                InContact = true;
                _spriteRenderer.material.SetFloat("_ShowOutline", 1);

                _eventPublisher.PublishEvent(PlayerEvents.OnInteractiveObjectHover, this);

                while (trigger && ComponentEnabled)
                {
                    trigger = _physicsObject.GetPhysicsTrigger(ContactTrigger);
                    yield return TimeYields.WaitOneFrameX;
                }
            }
            else
            {
                InContact = false;
                _spriteRenderer.material.SetFloat("_ShowOutline", 0);

                if (hasTouched) _eventPublisher.PublishEvent(PlayerEvents.OnInteractiveObjectLeave, this);

                while (!trigger && ComponentEnabled)
                {
                    trigger = _physicsObject.GetPhysicsTrigger(ContactTrigger);
                    yield return TimeYields.WaitOneFrameX;
                }
            }
        }

        InContact = false;
        yield return TimeYields.WaitOneFrameX;
    }
}
