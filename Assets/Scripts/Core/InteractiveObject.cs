using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class InteractiveObject : BaseGameRunner
{
    [field: SerializeField]
    public ScriptIdentifier ContactTrigger { get; private set; }

    public bool InContact { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private LichtPhysicsObject _physicsObject;

    protected override void OnAwake()
    {
        base.OnAwake();
        _physicsObject = GetComponent<LichtPhysicsObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_physicsObject != null)
        {
            _physicsObject.AddCustomObject(this);
        }
    }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (_spriteRenderer != null)
        {
            var trigger = _physicsObject.GetPhysicsTrigger(ContactTrigger);
            InContact = trigger;
            _spriteRenderer.material.SetFloat("_ShowOutline", trigger ? 1 : 0);
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
