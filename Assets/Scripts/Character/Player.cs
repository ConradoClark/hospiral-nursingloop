using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField]
    public Animator MainPlayerAnimator { get; private set; }

    [field: SerializeField]
    public PlayerInteraction Interaction { get; private set; }
}
