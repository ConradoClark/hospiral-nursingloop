using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCameraY : MonoBehaviour
{
    [field:SerializeField]
    public Camera SourceCamera { get; private set; }
    [field: SerializeField]
    public Camera TargetCamera { get; private set; }

    [field: SerializeField]
    public float YOffset { get; private set; }

    private void LateUpdate()
    {
        TargetCamera.transform.position = new Vector3(TargetCamera.transform.position.x,
            SourceCamera.transform.position.y + YOffset, SourceCamera.transform.position.z);
    }
}
