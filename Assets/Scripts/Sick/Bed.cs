using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Bed : BaseGameObject
{
    [field:SerializeField]
    public int IncomingAmbulanceDirection { get; private set; }

    public bool IsOccupied { get; set; }

    private SickPerson _patient;
    public SickPerson Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            if (value != null)
            {
                value.OnPersonCured += Value_OnPersonCured;
                value.OnPersonKilled += Value_OnPersonCured;
            }
        }
    }

    private void Value_OnPersonCured(SickPerson obj)
    {
        _patient = null;
        IsOccupied = false;
        obj.OnPersonCured -= Value_OnPersonCured;
        obj.OnPersonKilled -= Value_OnPersonCured;
    }
    public HospitalSection Section { get; private set; }

    private BedManager _bedManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        Section = GetComponentInParent<HospitalSection>(true);
        _bedManager = _bedManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _bedManager.AddBed(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _bedManager.RemoveBed(this);
    }
}
