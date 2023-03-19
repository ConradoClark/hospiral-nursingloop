using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class PlayerAnimator : BaseGameObject
{
    [field:SerializeField]
    public LichtPlatformerMoveController MoveController { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public SpriteRenderer CharacterSprite { get; private set; }

    [field:SerializeField]
    public ScriptIdentifier Grounded { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStartMoving, OnStartMoving);

        this.ObserveEvent<LichtPlatformerMoveController.LichtPlatformerMoveEvents,
            LichtPlatformerMoveController.LichtPlatformerMoveEventArgs>(
            LichtPlatformerMoveController.LichtPlatformerMoveEvents.OnStopMoving, OnStopMoving);

        this.ObserveEvent<LichtPlatformerJumpController.LichtPlatformerJumpEvents, 
            LichtPlatformerJumpController.LichtPlatformerJumpEventArgs>(
            LichtPlatformerJumpController.LichtPlatformerJumpEvents.OnJumpStart, 
            OnJumpStart);
    }

    private void OnJumpStart(LichtPlatformerJumpController.LichtPlatformerJumpEventArgs obj)
    {
        Animator.SetTrigger("Jump");
    }

    private void OnStopMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("Walking", false);
    }

    private void OnStartMoving(LichtPlatformerMoveController.LichtPlatformerMoveEventArgs obj)
    {
        Animator.SetBool("Walking", true);
    }

    private void Update()
    {
        if (MoveController.LatestDirection != 0)
        {
            CharacterSprite.flipX = MoveController.LatestDirection < 0;
        }

        Animator.SetBool("Grounded", PhysicsObject.GetPhysicsTrigger(Grounded));
    }
}
