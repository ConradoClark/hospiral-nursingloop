using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

[CreateAssetMenu(fileName="Disease", menuName = "Hospiral/Disease")]
public class Disease : ScriptableObject
{
    [field:SerializeField]
    public int MinLevel { get; private set; }

    [Serializable]
    public class DiseaseDefinition 
    {
        [field: SerializeField]
        public string Cure { get; private set; }
        [field: SerializeField]
        public ScriptPrefab MedicineIcon { get; private set; }

        [field: SerializeField]
        [field: Multiline]
        public string Description { get; private set; }
    }

    [field: SerializeField]
    public DiseaseDefinition Definition { get; private set; }
}
