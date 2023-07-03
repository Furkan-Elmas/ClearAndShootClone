using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class IncomeIncrementalButton : IncrementalButton
    {
        protected override int IncrementalLevel { get => Database.Instance.DataSO.IncomeLevel; set => Database.Instance.DataSO.IncomeLevel = value; }
        protected override float IncrementalCost => Database.Instance.DataSO.IncomeLevelCost();
        protected override BaseIncrementalType IncrementalType => BaseIncrementalType.Income;
    }
}
