using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

internal class GameLevel : BaseGameObject
{
    [field: SerializeField]
    public CounterStat Level { get; private set; }
}
