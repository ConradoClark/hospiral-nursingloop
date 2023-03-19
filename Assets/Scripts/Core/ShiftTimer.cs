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
    private PlayerHealth _playerHealth;
    private UI_Success _success;

    protected override void OnAwake()
    {
        base.OnAwake();
        _gameLevel = _gameLevel.FromScene();
        _playerHealth = _playerHealth.FromScene();
        _success = _success.FromScene(true);
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
        if (SecondsRemaining < 0)
        {
            SecondsRemaining = 0f;
            if (_playerHealth.Health.Value <= 0) yield break;

            yield return _success.Show().AsCoroutine();
            enabled = false;
        }
    }
}
