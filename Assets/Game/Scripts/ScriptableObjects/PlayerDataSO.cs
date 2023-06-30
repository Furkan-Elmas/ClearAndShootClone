using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Objects/Player Data")]
    public class PlayerDataSO : ScriptableObject
    {
        public int DamageLevel;
        public int FireRateLevel;
        public int ClearLevel;
        public int IncomeLevel;

        public int MaxDamageLevel = 50;
        public int MaxFireRateLevel = 50;
        public int MaxClearLevel = 50;
        public int MaxIncomeLevel = 50;

        public float ClearLevelCost()
        {
            return (75f / 2f * (ClearLevel + 1));
        }

        public float IncomeLevelCost()
        {
            return (75f / 2f * (IncomeLevel + 1));
        }

        public float DamageLevelCost()
        {
            return (250f / 2f * (DamageLevel + 1));
        }

        public float FireRateLevelCost()
        {
            return (250f / 2f * (FireRateLevel + 1));
        }
    }
}
