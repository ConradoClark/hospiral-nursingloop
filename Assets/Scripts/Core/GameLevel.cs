using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
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

    private BedManager _bedManager;

    public float GetIntervalBetweenPatients()
    {
        return 60 - Mathf.Clamp(5 * Level.Value, 0, 40);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _bedManager = _bedManager.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (ComponentEnabled)
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
