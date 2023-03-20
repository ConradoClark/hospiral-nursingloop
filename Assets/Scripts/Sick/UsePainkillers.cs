using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

namespace Assets.Scripts.Sick
{
    public class UsePainkillers : BaseGameObject
    {
        [field:SerializeField]
        public UseObjectOnInteraction Interaction { get; private set; }

        [field: SerializeField]
        public SickPerson SickPerson { get; private set; }
        [field: SerializeField]
        public int EffectDurationInSeconds { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            Interaction.OnObjectUsed += Interaction_OnObjectUsed;
        }

        private void Interaction_OnObjectUsed(Licht.Unity.Pooling.IPoolableComponent obj)
        {
            SickPerson.ActivatePainkiller(EffectDurationInSeconds);
        }
    }
}
