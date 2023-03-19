using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class PlayAudio : BaseGameRunner
{
    [field:SerializeField]
    public string AudioSource { get; private set; }

    [field: SerializeField]
    public AudioClip AudioClip { get; private set; }

    [field: SerializeField] public float Volume { get; private set; } = 1f;

    private AudioSources _sources;
    protected override void OnAwake()
    {
        base.OnAwake();
        _sources = _sources.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        _sources.PlayAudio(AudioSource, AudioClip, Volume);
        yield break;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _sources.StopAudio(AudioSource, AudioClip);
    }
}