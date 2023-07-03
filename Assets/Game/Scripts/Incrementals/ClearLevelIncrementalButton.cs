using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class ClearLevelIncrementalButton : IncrementalButton
    {
        protected override int IncrementalLevel { get => Database.Instance.DataSO.ClearLevel; set => Database.Instance.DataSO.ClearLevel = value; }
        protected override float IncrementalCost => Database.Instance.DataSO.ClearLevelCost();
        protected override BaseIncrementalType IncrementalType => BaseIncrementalType.ClearLevel;
    }
}