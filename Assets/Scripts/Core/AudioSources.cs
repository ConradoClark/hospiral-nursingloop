using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Objects;
using UnityEngine;

public class AudioSources : BaseGameObject
{
    [field:SerializeField]
    public AudioSourceDefinition[] Sources { get; private set; }

    [Serializable]
    public struct AudioSourceDefinition
    {
        public string Name;
        public AudioSource Source;
    }

    private Dictionary<string, AudioSource> _sourcesDict;
    protected override void OnAwake()
    {
        base.OnAwake();
        _sourcesDict = new Dictionary<string, AudioSource>(
            Sources.Select(kvp => new KeyValuePair<string, AudioSource>(kvp.Name, kvp.Source)));
    }

    public void PlayAudio(string sourceName, AudioClip clip, float volume = 1f, float pitch =1f)
    {
        if (!_sourcesDict.ContainsKey(sourceName)) return;

        _sourcesDict[sourceName].clip = clip;
        _sourcesDict[sourceName].pitch = pitch;
        DefaultMachinery.AddUniqueMachine($"fadeIn_{sourceName}", UniqueMachine.UniqueMachineBehaviour.Replace,
            FadeInAudio(_sourcesDict[sourceName], volume));

    }

    public void StopAudio(string sourceName, AudioClip clip)
    {
        if (!_sourcesDict.ContainsKey(sourceName) || _sourcesDict[sourceName].clip != clip) return;
        DefaultMachinery.AddUniqueMachine($"fadeOut_{sourceName}", UniqueMachine.UniqueMachineBehaviour.Cancel,
            FadeOutAudio(_sourcesDict[sourceName]));
    }

    private IEnumerable<IEnumerable<Action>> FadeInAudio(AudioSource source, float volume)
    {
        source.volume = 0f;
        source.Play();
        yield return new LerpBuilder(f => source.volume = f, () => source.volume)
            .SetTarget(volume)
            .Over(0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> FadeOutAudio(AudioSource source)
    {
        yield return new LerpBuilder(f => source.volume = f, () => source.volume)
            .SetTarget(0)
            .Over(0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        source.Stop();
    }
}
