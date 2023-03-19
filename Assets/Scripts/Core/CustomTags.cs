using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class CustomTags : BaseGameObject
{
    [Serializable]
    public struct CustomTag
    {
        public string Name;
        public string Value;
    }
    [field: SerializeField]
    public CustomTag[] Tags { get; private set; }

    [field:SerializeField]
    public EffectPoolable PooledObject { get;private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (PooledObject == null) return;
        DefaultMachinery.AddBasicMachine(Set());
    }

    private IEnumerable<IEnumerable<Action>> Set()
    {
        yield return TimeYields.WaitOneFrameX;
        foreach (var customTag in Tags)
        {
            PooledObject.CustomTags[customTag.Name] = customTag.Value;
        }
    }
}
