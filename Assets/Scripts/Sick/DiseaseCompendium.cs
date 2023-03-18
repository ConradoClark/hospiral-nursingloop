using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiseaseCompendium : BaseGameObject
{
    [field:SerializeField]
    public Disease[] Diseases { get; private set; }

    private GameLevel _level;

    protected override void OnAwake()
    {
        base.OnAwake();
        _level = _level.FromScene();
    }

    public Disease PickDisease()
    {
        var validDiseases = Diseases.Where(d => d.MinLevel <= _level.Level.Value).ToArray();
        return validDiseases[Random.Range(0, validDiseases.Length)];
    }

}
