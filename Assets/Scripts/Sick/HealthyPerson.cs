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
        yield return TimeYields.WaitOneFrameX;

        var direction = PooledObject.HasProp("Direction") ? PooledObject.CustomProps["Direction"] : 1;
        while (transform.position.x is < 8 and > -8)
        {
            transform.Translate((float)GameTimer.UpdatedTimeInMilliseconds * 0.001f 
                                * Speed * new Vector3(direction,0,0));
            yield return TimeYields.WaitOneFrameX;
        }

        PooledObject.EndEffect();
    }
}
