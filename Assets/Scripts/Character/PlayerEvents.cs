using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public enum PlayerEvents
{
    OnInteractionButtonPressed,
    OnItemPickup,
    OnItemUse,
    OnItemDiscard,
    OnInteractiveObjectHover,
    OnInteractiveObjectLeave,
    OnSickSpawned
}
