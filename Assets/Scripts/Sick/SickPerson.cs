using System;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Events;
using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
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

    [field: SerializeField]
    public AudioClip SoundOnDeath { get; private set; }

    [field: SerializeField]
    public AudioClip SoundOnWrongItem { get; private set; }

    private AudioSources _audioSources;
    private PlayerHealth _playerHealth;

    private DiseaseCompendium _diseaseCompendium;
    private EffectsManager _effectsManager;
    private IEventPublisher<PlayerEvents, SickPerson> _eventPublisher;
    private IPoolableComponent _currentIcon;
    public event Action<SickPerson> OnPersonCured;
    public event Action<SickPerson> OnPersonKilled;

    private bool _dead;
    protected override void OnAwake()
    {
        base.OnAwake();
        _diseaseCompendium = _diseaseCompendium.FromScene();
        _effectsManager = _effectsManager.FromScene(true,true);
        _eventPublisher = this.RegisterAsEventPublisher<PlayerEvents, SickPerson>();
        _audioSources = _audioSources.FromScene();
        _playerHealth = _playerHealth.FromScene();
    }

    protected override void OnEnable()
    {
        SpriteRenderer.color = Color.white;
        SpeechBubbleTransform.gameObject.SetActive(true);
        _dead = false;
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
        if (_dead) return;
        if (!obj.HasTag("Identifier", Disease.Definition.Cure))
        {
            if (!obj.HasTag("Neutral"))
            {
                if (Health.Value > 0)
                {
                    Health.Value -= 20;
                }
                _audioSources.PlayAudio("Sick", SoundOnWrongItem);
                _eventPublisher.PublishEvent(PlayerEvents.OnMedicalError, this);
                DefaultMachinery.AddBasicMachine(SpriteRenderer.BlinkForSeconds(1f));
            }
            return;
        }
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
        _eventPublisher.PublishEvent(PlayerEvents.OnSickCured, this);
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
                if (!_dead) _playerHealth.Health.Value -= 1;
                else yield break;

                _dead = true;
                SpeechBubbleTransform.gameObject.SetActive(false);

                if (SoundOnDeath != null)
                {
                    _audioSources.PlayAudio("Sick", SoundOnDeath, pitch: 0.9f + Random.Range(0, 0.2f));
                }

                var floatEffect = transform.GetAccessor()
                    .Position
                    .Y
                    .Increase(0.5f)
                    .Over(1)
                    .Build();

                var faded = SpriteRenderer.GetAccessor()
                    .Color
                    .A
                    .SetTarget(0)
                    .Over(1)
                    .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
                    .Build();

                yield return floatEffect.Combine(faded);
                OnPersonKilled?.Invoke(this);
                _eventPublisher.PublishEvent(PlayerEvents.OnSickDied, this);
                Disease = null;
                PooledObject.EndEffect();
                yield break;
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
