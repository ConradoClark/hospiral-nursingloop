using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using UnityEngine;

public class UI_SickIcons : BaseGameObject
{
    [field: SerializeField]
    public SpriteRenderer[] PatientIcons { get; private set; }

    private Queue<SickPerson> _sickList;
    protected override void OnEnable()
    {
        _sickList = new Queue<SickPerson>();
        base.OnEnable();
        this.ObserveEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickSpawned, OnSickSpawned);
    }
    private void OnSickSpawned(SickPerson obj)
    {
        _sickList.Enqueue(obj);
        obj.OnPersonCured += Obj_OnPersonCured;
        SetIcons();
    }

    private void Obj_OnPersonCured(SickPerson obj)
    {
        obj.OnPersonCured -= Obj_OnPersonCured;
        _sickList = new Queue<SickPerson>(_sickList.Where(sick=>sick!=obj));
        SetIcons();
    }

    private void SetIcons()
    {
        var list = _sickList.ToArray();
        for (var index = 0; index < list.Length; index++)
        {
            if (PatientIcons.Length <= index) return;
            var icon = PatientIcons[index];
            var sick = list[index];

            icon.enabled = true;
            icon.sprite = sick.SickIcon;
        }

        for (var index = list.Length; index < PatientIcons.Length; index++)
        {
            var icon = PatientIcons[index];
            icon.enabled = false;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickSpawned, OnSickSpawned);
    }
}