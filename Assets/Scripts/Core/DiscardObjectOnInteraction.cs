using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class DiscardObjectOnInteraction : BaseGameObject
{
    [field: SerializeField]
    public InteractiveObject InteractiveObject { get; private set; }

    [field: SerializeField]
    public AudioClip Sound { get; private set; }

    private AudioSources _audioSources;
    protected override void OnAwake()
    {
        base.OnAwake();
        _audioSources = _audioSources.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents, PlayerInteraction>
            (PlayerEvents.OnInteractionButtonPressed, OnInteractionButtonPressed);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<PlayerEvents, PlayerInteraction>
            (PlayerEvents.OnInteractionButtonPressed, OnInteractionButtonPressed);
    }

    private void OnInteractionButtonPressed(PlayerInteraction interaction)
    {
        if (!InteractiveObject.InContact || !interaction.IsHoldingObject) return;
        if (interaction.HeldObject.HasTag("Cooldown")) return;
        interaction.HeldObject.Pool.Release(interaction.HeldObject);
        interaction.HeldObject = null;

        if (Sound != null)
        {
            _audioSources.PlayAudio("Interaction", Sound);
        }
    }
}
