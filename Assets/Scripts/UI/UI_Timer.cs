using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_Timer : BaseGameObject
{
    [field: SerializeField]
    public TMP_Text ShiftEndComponent { get; private set; }

    private ShiftTimer _shiftTimer;
    protected override void OnAwake()
    {
        base.OnAwake();
        _shiftTimer = _shiftTimer.FromScene();
    }

    private void Update()
    {
        var secondsRemaining = (int) _shiftTimer.SecondsRemaining;
        var minutes = secondsRemaining / 60;
        var seconds = secondsRemaining % 60;

        ShiftEndComponent.text = $"{minutes:D2}:{seconds:D2}";
    }
}
