using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_RankText : BaseGameObject
{
    [field:SerializeField]
    public TMP_Text RankTextComponent { get; private set; }

    [field: SerializeField]
    public LevelScore LevelScore { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        RankTextComponent.text = LevelScore.Rank ?? "-";
        if (string.IsNullOrWhiteSpace(LevelScore.Rank))
        {
            RankTextComponent.text = "-";
        }
    }
}
