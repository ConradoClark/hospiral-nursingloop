using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;

public class UI_Success : BaseGameObject
{
    private UI_ReportCounter _reportCounter;
    protected override void OnAwake()
    {
        base.OnAwake();
        _reportCounter = _reportCounter.FromScene(true);
    }

    public IEnumerable<IEnumerable<Action>> Show()
    {
        gameObject.SetActive(true);
        yield return TimeYields.WaitOneFrameX;

        _reportCounter.SetRank();
        yield return TimeYields.WaitSeconds(UITimer, 0.5f);
    }
}
