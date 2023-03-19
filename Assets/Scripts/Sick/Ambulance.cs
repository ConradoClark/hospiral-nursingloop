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
using Random = UnityEngine.Random;

public class Ambulance : EffectPoolable
{
    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public Bed Target { get; set; }

    [field: SerializeField]
    public ScriptPrefab[] PatientPrefabs { get; private set; }

    [field: SerializeField]
    public Vector3 PatientOffset { get; private set; }

    public override void OnActivation()
    {
        base.OnActivation();

        DefaultMachinery.AddBasicMachine(HandleAmbulance());
    }

    private IEnumerable<IEnumerable<Action>> HandleAmbulance()
    {
        yield return TimeYields.WaitOneFrameX;

        SpriteRenderer.flipX = Target.transform.position.x < transform.position.x;
        var initialPosX = transform.position.x;

        yield return transform.GetAccessor()
            .Position
            .X
            .SetTarget(Target.transform.position.x)
            .Over(Mathf.Abs(Target.transform.position.x - transform.position.x) / Mathf.Max(0.1f, Speed))
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        var randomPatient = PatientPrefabs[Random.Range(0, PatientPrefabs.Length)];
        if (randomPatient.TrySpawnEffect(transform.position + PatientOffset, out var patient))
        {
            Target.Patient = patient.Component.GetComponent<SickPerson>();
            Target.Patient.SpriteRenderer.flipX = Target.transform.position.x - initialPosX < 0;
            Target.Patient.SpeechBubbleTransform.localScale = new Vector3(
                Mathf.Sign(Target.transform.position.x - initialPosX)
                , 1, 1);

            Target.Patient.SpeechBubbleTransform.transform.localPosition = new Vector3(
                Mathf.Abs(Target.Patient.SpeechBubbleTransform.transform.localPosition.x) * Mathf.Sign(Target.transform.position.x - initialPosX),
                Target.Patient.SpeechBubbleTransform.transform.localPosition.y,
                Target.Patient.SpeechBubbleTransform.transform.localPosition.z);

            patient.CustomProps["Direction"] = Mathf.Sign(initialPosX - Target.transform.position.x);
        }

        SpriteRenderer.flipX = !SpriteRenderer.flipX;

        yield return transform.GetAccessor()
             .Position
             .X
             .SetTarget(initialPosX)
             .Over(Mathf.Abs(initialPosX - transform.position.x) / Mathf.Max(0.1f, Speed))
             .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
             .Build();

        EndEffect();
    }
}
