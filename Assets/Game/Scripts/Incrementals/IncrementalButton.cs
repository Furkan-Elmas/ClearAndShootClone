using HyperlabCase.Interfaces;
using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HyperlabCase
{
    public abstract class IncrementalButton : MonoBehaviour
    {
        [SerializeField] protected TMP_Text levelText;
        [SerializeField] protected TMP_Text costText;
        [SerializeField] protected Button button;

        protected abstract int IncrementalLevel { get; set; }
        protected abstract float IncrementalCost { get; }
        protected abstract BaseIncrementalType IncrementalType { get; }

        protected virtual void Start()
        {
            RefreshButton();
            CheckInteractable();
            button.onClick.AddListener(OnClickButton);
        }

        protected virtual void OnEnable()
        {
            EventManager.OnMoneyChanged += CheckInteractable;
        }

        protected virtual void OnDisable()
        {
            EventManager.OnMoneyChanged -= CheckInteractable;
        }

        public virtual void OnClickButton()
        {
            if (Database.Instance.DataSO.Currency < IncrementalCost)
                return;

            Database.Instance.DataSO.Currency -= IncrementalCost;
            IncrementalLevel++;
            EventManager.OnLevelUpIncremental?.Invoke(IncrementalType);
            RefreshButton();
            EventManager.OnMoneyChanged?.Invoke();
            Database.Instance.SaveData();
        }

        protected virtual void CheckInteractable()
        {
            button.interactable = IncrementalCost <= Database.Instance.DataSO.Currency;
        }

        protected virtual void RefreshButton()
        {
            bool isReachedMax = Database.Instance.DataSO.IncomeLevel >= Database.Instance.DataSO.MaxIncomeLevel;
            levelText.text = isReachedMax ? "MAX" : "Level " + IncrementalLevel.ToString("F0");
            costText.text = isReachedMax ? "COMING SOON" : IncrementalCost.ToString("F0");
            button.interactable = !isReachedMax;
        }
    }
}
