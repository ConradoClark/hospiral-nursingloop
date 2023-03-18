using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class ConstrainCamera : CinemachineExtension
{
    [field: SerializeField]
    public float MinY{ get; private set; }
    [field: SerializeField]
    public float MaxY { get; private set; }

    [field:SerializeField]
    public float MinX { get; private set; }
    [field: SerializeField]
    public float MaxX { get; private set; }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body) return;
        var pos = state.RawPosition;
        pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
        pos.y = Mathf.Clamp(pos.y, MinY, MaxY);
        state.RawPosition = pos;
    }
}
