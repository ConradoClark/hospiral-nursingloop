using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class HealthyPerson : BaseGameRunner
{
    [field:SerializeField]
    public float Speed { get; private set; }
    [field: SerializeField]
    public EffectPoolable PooledObject { get; private set; }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (transform.position.x > -7.2)
        {
            transform.Translate(Vector3.left 
                                * (float)GameTimer.UpdatedTimeInMilliseconds * 0.001f * Speed);
            yield return TimeYields.WaitOneFrameX;
        }

        PooledObject.EndEffect();
    }
}
