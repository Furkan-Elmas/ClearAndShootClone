using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class DamageIncrementalButton : IncrementalButton
    {
        protected override int IncrementalLevel { get => Database.Instance.DataSO.DamageLevel; set => Database.Instance.DataSO.DamageLevel = value; }
        protected override int MaxIncrementalLevel => Database.Instance.DataSO.MaxDamageLevel;
        protected override float IncrementalCost => Database.Instance.DataSO.DamageLevelCost();
        protected override BaseIncrementalType IncrementalType => BaseIncrementalType.Damage;
    }
}
