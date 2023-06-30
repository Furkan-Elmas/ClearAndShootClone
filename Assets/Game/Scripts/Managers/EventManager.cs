using System;
using HyperlabCase.Collectables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.Managers
{
    public static class EventManager
    {
        public static Action OnPlayerTapToStart;
        public static Action OnCleaningFinish;
        public static Action OnRunnerFinish;
        public static Action OnPlayerHitColumn;
        public static Action<int> OnLevelPassed;
        public static Action OnMoneyChanged;
        public static Action<Washable> OnCollectedObject;
        public static Action<GateIncrementalType, float> OnEarnedGateIncrement;
        public static Action<BaseIncrementalType> OnLevelUpIncremental;

        public static Func<WeaponPoolManager> GetWeaponPoolManager;
    }
}
