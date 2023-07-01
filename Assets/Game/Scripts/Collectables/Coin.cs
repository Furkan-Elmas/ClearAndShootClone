using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.Collectables
{
    public class Coin : Washable
    {
        [SerializeField] private float moneyValue = 10f;

        protected override IEnumerator BeCollected()
        {
            float newValue = moneyValue * (1 + (Database.Instance.DataSO.IncomeLevel - 1) * 0.1f);
            Database.Instance.DataSO.Currency += newValue;
            EventManager.OnMoneyChanged?.Invoke();
            return base.BeCollected();
        }
    }
}
