using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class WrapAround : BaseGameObject
{
    [field: SerializeField]
    public float MinX { get; private set; }
    [field: SerializeField]
    public float MaxX { get; private set; }
    [field: SerializeField]
    public float LeftX { get; private set; }
    [field: SerializeField]
    public float RightX { get; private set; }

    [field: SerializeField]
    public float MaxY { get; private set; }
    [field: SerializeField]
    public float BottomY { get; private set; }

    [field: SerializeField]
    public float BottomYCameraOffset { get; private set; }

    [field: SerializeField]
    public Transform Target { get; private set; }

    [field: SerializeField]
    public CinemachineVirtualCamera VirtualCamera { get; private set; }

    private CinemachineFramingTransposer _framingTransposer;
    protected override void OnAwake()
    {
        base.OnAwake();
        _framingTransposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void LateUpdate()
    {
        if (Target.position.x < MinX)
        {
            Target.position = new Vector3(RightX, Target.position.y, Target.position.z);
        }

        if (Target.position.x > MaxX)
        {
            Target.position = new Vector3(LeftX, Target.position.y, Target.position.z);
        }

        if (Target.position.y > MaxY)
        {
            var damping = _framingTransposer.m_YDamping;
            var offset = _framingTransposer.m_TrackedObjectOffset.y;

            _framingTransposer.m_YDamping = 0f;
            _framingTransposer.m_TrackedObjectOffset.y = BottomYCameraOffset;

            Target.position = new Vector3(Target.position.x, BottomY, Target.position.z);

            DefaultMachinery.AddBasicMachine(
                TimeYields.WaitOneFrameX.ThenRun(() =>
                {
                    _framingTransposer.m_YDamping = damping;
                    _framingTransposer.m_TrackedObjectOffset.y = offset;
                }));
        }
    }


}
