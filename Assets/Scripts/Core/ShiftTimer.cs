using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;

public class ShiftTimer : BaseGameRunner
{
    public float SecondsRemaining { get; private set; }
    private GameLevel _gameLevel;

    protected override void OnAwake()
    {
        base.OnAwake();
        _gameLevel = _gameLevel.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SecondsRemaining = _gameLevel.WorkShiftInSeconds;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        yield return TimeYields.WaitOneFrameX;
        SecondsRemaining -= (float) GameTimer.UpdatedTimeInMilliseconds * 0.001f;
        if (SecondsRemaining < 0) SecondsRemaining = 0f;
    }
}
