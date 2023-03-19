using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class UI_PlayerHealth : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer FullHeartSpriteRenderer { get; private set; }

    private PlayerHealth _playerHealth;
    protected override void OnAwake()
    {
        base.OnAwake();
        _playerHealth = _playerHealth.FromScene();
        FullHeartSpriteRenderer.size = new Vector2(_playerHealth.Health.InitialValue * 0.5f, 0.5f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerHealth.Health.OnChange += Health_OnChange;
    }
    private void Health_OnChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        FullHeartSpriteRenderer.size = new Vector2(obj.NewValue * 0.5f, 0.5f);
    }
}
