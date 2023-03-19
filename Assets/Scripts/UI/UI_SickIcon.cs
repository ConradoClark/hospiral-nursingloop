using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class UI_SickIcon : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    public SickPerson SickReference { get; set; }

    [SerializeField]
    private SpriteRenderer _heartSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _emptyHeartSpriteRenderer;

    protected override void OnAwake()
    {
        base.OnAwake();
        SpriteRenderer.enabled = _heartSpriteRenderer.enabled = _emptyHeartSpriteRenderer.enabled = enabled;
    }

    private void Update()
    {
        if (SickReference == null) return;

        _heartSpriteRenderer.size = new Vector2 (Mathf.Lerp(0.1f, 0.5f,
            (float)SickReference.Health.Value / SickReference.Health.InitialValue), 0.5f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SpriteRenderer.enabled = _heartSpriteRenderer.enabled = _emptyHeartSpriteRenderer.enabled = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SpriteRenderer.enabled = _heartSpriteRenderer.enabled = _emptyHeartSpriteRenderer.enabled = false;
    }
}
