using System;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class SickPerson : BaseGameRunner
{
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

    //[field: SerializeField]
    //public EffectPoolable PooledObject { get; private set; }

    [field: SerializeField]
    public Vector3 HealthyPersonOffset { get; private set; }

    private DiseaseCompendium _diseaseCompendium;
    private EffectsManager _effectsManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _diseaseCompendium = _diseaseCompendium.FromScene();
        _effectsManager = _effectsManager.FromScene(true,true);
    }

    protected override void OnEnable()
    {
        Disease = _diseaseCompendium.PickDisease();
        Debug.Log(Disease);
        Debug.Log(Disease.Definition.MedicineIcon);
        Debug.Log(_effectsManager);
        InteractiveObject.Description = Disease.Definition.Description;
        if (_effectsManager.GetEffect(Disease.Definition.MedicineIcon).TryGetFromPool(out var icon))
        {
            icon.Component.transform.SetParent(SpeechBubbleTransform);
            icon.Component.transform.localPosition = IconOffset;
        }
        base.OnEnable();
        this.ObserveEvent<PlayerEvents,IPoolableComponent>(PlayerEvents.OnItemUse, OnItemUse);
    }
    private void OnItemUse(IPoolableComponent obj)
    {
        if (obj.HasTag("Identifier", Disease.Definition.Cure))
        {
            HealthyPerson.TrySpawnEffect(transform.position + HealthyPersonOffset, out _);
            gameObject.SetActive(false); // temporary
        }
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (Disease != null)
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
}
