using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class PlayerHealth : BaseGameObject
{
    [field:SerializeField]
    public CounterStat Health { get; private set; }
}

