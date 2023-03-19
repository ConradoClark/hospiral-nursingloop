using System;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Events;
using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SickPerson : BaseGameRunner
{
    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public Sprite SickIcon { get; private set; }
    public Disease Disease { get; private set; }
    [field: SerializeField]
    public CounterStat Health { get; private set; }
    [field: SerializeField]
    public float HealthTickInSeconds { get; private set; }

    [field: SerializeField]
    public InteractiveObject InteractiveObject { get; private set; }

    [field: SerializeField]
    public Transform SpeechBubbleTransform { get; private set; }

    [field: SerializeField]
    public Vector3 IconOffset { get; private set; }

    [field:SerializeField]
    public ScriptPrefab HealthyPerson { get; private set; }

    [field: SerializeField]
    public EffectPoolable PooledObject { get; private set; }

    [field: SerializeField]
    public Vector3 HealthyPersonOffset { get; private set; }

    [field: SerializeField]
    public AudioClip SoundOnCure { get; private set; }

    private AudioSources _audioSources;

    private DiseaseCompendium _diseaseCompendium;
    private EffectsManager _effectsManager;
    private IEventPublisher<PlayerEvents, SickPerson> _eventPublisher;
    private IPoolableComponent _currentIcon;
    public event Action<SickPerson> OnPersonCured;

    protected override void OnAwake()
    {
        base.OnAwake();
        _diseaseCompendium = _diseaseCompendium.FromScene();
        _effectsManager = _effectsManager.FromScene(true,true);
        _eventPublisher = this.RegisterAsEventPublisher<PlayerEvents, SickPerson>();
        _audioSources = _audioSources.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents,IPoolableComponent>(PlayerEvents.OnItemUse, OnItemUse);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<PlayerEvents, IPoolableComponent>(PlayerEvents.OnItemUse, OnItemUse);
    }

    private void OnItemUse(IPoolableComponent obj)
    {
        if (!obj.HasTag("Identifier", Disease.Definition.Cure)) return;
        if (HealthyPerson.TrySpawnEffect(transform.position + HealthyPersonOffset, out var healthyPerson))
        {
            healthyPerson.CustomProps["Direction"] =
                PooledObject.HasProp("Direction") ? PooledObject.CustomProps["Direction"] : 1f;
        }

        if (SoundOnCure != null)
        {
            _audioSources.PlayAudio("Sick", SoundOnCure, pitch: 0.9f + Random.Range(0, 0.2f));
        }

        OnPersonCured?.Invoke(this);
        Disease = null;
        if (PooledObject!= null) PooledObject.EndEffect();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        yield return TimeYields.WaitOneFrameX;
        if (ComponentEnabled && Disease == null)
        {
            Disease = _diseaseCompendium.PickDisease();
            InteractiveObject.Description = Disease.Definition.Description;
            if (_effectsManager.GetEffect(Disease.Definition.MedicineIcon).TryGetFromPool(out var icon))
            {
                icon.Component.transform.SetParent(SpeechBubbleTransform);
                icon.Component.transform.localPosition = IconOffset;
                _currentIcon = icon;
                PooledObject.OnEffectOver += PooledObject_OnEffectOver;
            }
            _eventPublisher.PublishEvent(PlayerEvents.OnSickSpawned, this);
        }
        while (Disease != null && ComponentEnabled)
        {
            if (Health.Value > 0)
            {
                Health.Value -= 1;
            }
            else
            {
                // die
            }

            yield return TimeYields.WaitSeconds(GameTimer, HealthTickInSeconds);
        }
    }

    private void PooledObject_OnEffectOver()
    {
        PooledObject.OnEffectOver -= PooledObject_OnEffectOver;
        _currentIcon?.Pool.Release(_currentIcon);
    }
}
