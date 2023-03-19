using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaySoundOnJump : BaseGameObject
{
    [field:SerializeField]
    public LichtPlatformerJumpController JumpController { get; private set; }

    [field: SerializeField] public AudioClip Sound { get; private set; }

    [field: SerializeField] public float Volume { get; private set; }

    private AudioSources _audioSources;

    protected override void OnAwake()
    {
        base.OnAwake();
        _audioSources = _audioSources.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        JumpController.OnJumpStart += JumpController_OnJumpStart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        JumpController.OnJumpStart -= JumpController_OnJumpStart;
    }

    private void JumpController_OnJumpStart(LichtPlatformerJumpController.LichtPlatformerJumpEventArgs obj)
    {
        _audioSources.PlayAudio("Jump", Sound, Volume, 0.9f + Random.value * 0.2f);
    }
}
