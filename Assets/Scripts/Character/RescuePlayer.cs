using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class RescuePlayer : BaseGameObject
{
    [field: SerializeField]
    public float LimitY { get; private set; }
    [field:SerializeField]
    public float ResetY { get; private set; }

    private void Update()
    {
        if (transform.position.y < LimitY)
        {
            transform.position = new Vector3(transform.position.x, ResetY, transform.position.z);
        }
    }
}
