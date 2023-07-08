using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class FireRateIncrementalButton : IncrementalButton
    {
        protected override int IncrementalLevel { get => Database.Instance.DataSO.FireRateLevel; set => Database.Instance.DataSO.FireRateLevel = value; }
        protected override int MaxIncrementalLevel => Database.Instance.DataSO.MaxFireRateLevel;
        protected override float IncrementalCost => Database.Instance.DataSO.FireRateLevelCost();
        protected override BaseIncrementalType IncrementalType => BaseIncrementalType.FireRate;
    }
}
