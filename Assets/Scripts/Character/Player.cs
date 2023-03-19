using System.Collections;
using System.Collections.Generic;
using Licht.Unity.CharacterControllers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField]
    public Animator MainPlayerAnimator { get; private set; }

    [field: SerializeField]
    public PlayerInteraction Interaction { get; private set; }

    [field: SerializeField]
    public LichtPlatformerMoveController MoveController { get; private set; }
}
