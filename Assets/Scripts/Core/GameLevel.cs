using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameLevel : BaseGameRunner
{
    [field: SerializeField]
    public CounterStat Level { get; private set; }

    [field:SerializeField]
    public ScriptPrefab Ambulance { get; private set; }

    [field: SerializeField]
    public Vector3 LeftAmbulanceOffset { get; private set; }

    [field: SerializeField]
    public Vector3 RightAmbulanceOffset { get; private set; }

    [field:SerializeField]
    public int WorkShiftInSeconds { get; private set; }

    private Player _player;
    private BedManager _bedManager;
    private PlayerHealth _playerHealth;
    private UI_GameOver _gameOver;
    public bool GameOver { get; private set; }

    public float GetIntervalBetweenPatients()
    {
        return 60 - Mathf.Clamp(5 * Level.Value, 0, 40);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _bedManager = _bedManager.FromScene();
        _playerHealth = _playerHealth.FromScene();
        _player = _player.FromScene();
        _gameOver = _gameOver.FromScene(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerHealth.Health.OnChange += Health_OnChange;
    }

    private void Health_OnChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        if (obj.NewValue <= 0) GameOver = true;
        if (!GameOver) return;
        DefaultMachinery.AddUniqueMachine($"gameOver_{GetInstanceID()}", UniqueMachine.UniqueMachineBehaviour.Cancel,
            ShowGameOver());
    }

    private IEnumerable<IEnumerable<Action>> ShowGameOver()
    {
        _player.MoveController.BlockMovement(this);
        yield return _gameOver.Show().AsCoroutine();
        _player.MoveController.UnblockMovement(this);
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (ComponentEnabled && !GameOver)
        {
            var bed = _bedManager.GetRandomUnoccupiedBed();

            while (bed == null)
            {
                yield return TimeYields.WaitSeconds(GameTimer, 1);
                bed = _bedManager.GetRandomUnoccupiedBed();
            }

            if (Ambulance.TrySpawnEffect(bed.Section.transform.position +
                                         (bed.IncomingAmbulanceDirection > 0
                                             ? RightAmbulanceOffset
                                             : LeftAmbulanceOffset),
                    out var comp
                ))
            {
                var ambulance = comp.Component.GetComponent<Ambulance>();
                ambulance.Target = bed;
                bed.IsOccupied = true;
            }

            yield return TimeYields.WaitSeconds(GameTimer, GetIntervalBetweenPatients()
                , breakCondition: () => _bedManager.IsHospitalEmpty);

            if (_bedManager.IsHospitalEmpty) yield return TimeYields.WaitSeconds(GameTimer, Random.Range(1, 5f));
        }
    }
}
