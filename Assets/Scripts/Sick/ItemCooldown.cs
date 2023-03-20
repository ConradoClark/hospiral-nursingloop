using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class ItemCooldown : BaseGameRunner
{
    [field: SerializeField]
    public EffectPoolable PooledObject { get; private set; }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        PooledObject.CustomTags["Cooldown"] = "true";
        yield return TimeYields.WaitOneFrameX;
        PooledObject.CustomTags.Remove("Cooldown");
    }
}
