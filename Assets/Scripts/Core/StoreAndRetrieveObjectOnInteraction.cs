using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

internal class StoreAndRetrieveObjectOnInteraction : BaseGameObject
{
    [field: SerializeField] public InteractiveObject InteractiveObject { get; private set; }

    [field: SerializeField] public string[] IdentifierRequirement { get; private set; }

    [field:SerializeField] public EffectPoolable StarterItem { get; private set; }

    public IPoolableComponent StoredObject { get; private set; }

    [field: SerializeField] public bool CanStoreAnything { get; private set; }

    [field: SerializeField] public AudioClip StoreSound { get; private set; }

    [field: SerializeField] public AudioClip RetrieveSound { get; private set; }

    [field: SerializeField] public Transform Storage { get; private set; }

    private AudioSources _audioSources;

    protected override void OnAwake()
    {
        base.OnAwake();
        _audioSources = _audioSources.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (StarterItem != null)
        {
            StoredObject = StarterItem;
        }
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
        var canStore = interaction.IsHoldingObject && StoredObject == null;
        var canRetrieve = !interaction.IsHoldingObject && StoredObject != null;
        if (!InteractiveObject.InContact || (!canStore && !canRetrieve)) return;

        if (canStore)
        {
            if (!CanStoreAnything && IdentifierRequirement.All(req => !interaction.HeldObject.HasTag("Identifier", req))) return;
            interaction.HeldObject.Component.transform.SetParent(Storage);
            StoredObject = interaction.HeldObject;
            if (StoreSound != null)
            {
                _audioSources.PlayAudio("Interaction", StoreSound);
            }

            interaction.HeldObject = null;
        }

        if (canRetrieve)
        {
            StoredObject.Component.transform.SetParent(interaction.transform);
            interaction.HeldObject = StoredObject;

            if (RetrieveSound != null)
            {
                _audioSources.PlayAudio("Interaction", RetrieveSound);
            }

            StoredObject = null;
        }
    }
}
