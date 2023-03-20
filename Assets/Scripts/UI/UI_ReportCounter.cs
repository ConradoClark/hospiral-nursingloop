using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_ReportCounter : BaseGameObject
{
    [field: SerializeField]
    public int MinForBRank { get; private set; }
    [field: SerializeField]
    public int MinForARank { get; private set; }
    [field: SerializeField]
    public int MinForSRank { get; private set; }

    [field: SerializeField]
    public LevelScore LevelScore { get; private set; }

    [field: SerializeField]
    public TMP_Text Cured { get; private set; }
    [field: SerializeField]
    public TMP_Text Died { get; private set; }
    [field: SerializeField]
    public TMP_Text MedicalError { get; private set; }
    [field: SerializeField]
    public TMP_Text Rank { get; private set; }


    private int _cured;
    private int _died;
    private int _medicalError;
    private string _rank;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickCured, OnSickCured);
        this.ObserveEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickDied, OnSickDied);
        this.ObserveEvent<PlayerEvents, SickPerson>(PlayerEvents.OnMedicalError, OnMedicalError);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.StopObservingEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickCured, OnSickCured);
        this.StopObservingEvent<PlayerEvents, SickPerson>(PlayerEvents.OnSickDied, OnSickDied);
        this.StopObservingEvent<PlayerEvents, SickPerson>(PlayerEvents.OnMedicalError, OnMedicalError);
    }

    private void OnMedicalError(SickPerson obj)
    {
        _medicalError++;
        UpdateCounters();
    }

    private void OnSickDied(SickPerson obj)
    {
        _died++;
        UpdateCounters();
    }

    private void OnSickCured(SickPerson obj)
    {
        _cured++;
        UpdateCounters();
    }

    private void UpdateCounters()
    {
        Cured.text = _cured.ToString().PadLeft(3, '0');
        Died.text = _died.ToString().PadLeft(3, '0');
        MedicalError.text = _medicalError.ToString().PadLeft(3, '0');

        _rank = CalculateRank();
        Rank.text = _rank;
    }

    public void SetRank()
    {
        LevelScore.Rank = CalculateRank();
    }

    private string CalculateRank()
    {
        if (_died == 0 && _medicalError == 0)
        {
            if (_cured >= MinForSRank) return "S";
            if (_cured >= MinForARank) return "A";
            return _cured >= MinForBRank ? "B" : "C";
        }

        if (_died == 0 && _medicalError is > 0 and < 2)
        {
            if (_cured >= MinForSRank) return "A";
            if (_cured >= MinForARank) return "B";
            return _cured >= MinForBRank ? "C" : "D";
        }

        if (_died == 0 && _medicalError >= 2)
        {
            if (_cured >= MinForSRank) return "B";
            if (_cured >= MinForARank) return "C";
            return _cured >= MinForBRank ? "D" : "E";
        }

        if (_died is > 0 and < 2)
        {
            if (_cured >= MinForSRank) return "C";
            if (_cured >= MinForARank) return "D";
            return _cured >= MinForBRank ? "E" : "F";
        }

        if (_died is >= 2 and < 4)
        {
            if (_cured >= MinForSRank) return "D";
            return _cured >= MinForARank ? "E" : "F";
        }

        if (_died >= 4)
        {
            return _cured >= MinForSRank ? "E" : "F";
        }

        return "F";
    }
}
