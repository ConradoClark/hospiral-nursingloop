using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName="LevelScore", menuName="Hospiral/LevelScore")]
public class LevelScore : ScriptableObject
{
    [field:SerializeField]
    public string LevelName { get; private set; }

    [field: SerializeField]
    public string Rank { get; set; }
}
