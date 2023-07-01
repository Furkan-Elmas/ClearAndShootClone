using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Scriptable Objects/Game Data")]
    public class GameDataSO : ScriptableObject
    {
        public int GameLevel;
        public float Currency;
        public int DamageLevel = 1;
        public int FireRateLevel = 1;
        public int ClearLevel = 1;
        public int IncomeLevel = 1;

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
