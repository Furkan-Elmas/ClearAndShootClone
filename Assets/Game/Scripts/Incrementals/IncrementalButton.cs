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
        protected abstract int MaxIncrementalLevel { get; }
        protected abstract float IncrementalCost { get; }
        protected abstract BaseIncrementalType IncrementalType { get; }

        protected virtual void Start()
        {
            RefreshButton();
            button.onClick.AddListener(OnClickButton);
        }

        protected virtual void OnEnable()
        {
            EventManager.OnMoneyChanged += RefreshButton;
        }

        protected virtual void OnDisable()
        {
            EventManager.OnMoneyChanged -= RefreshButton;
        }

        public virtual void OnClickButton()
        {
            if (Database.Instance.DataSO.Currency < IncrementalCost)
                return;

            Database.Instance.DataSO.Currency -= IncrementalCost;
            IncrementalLevel++;
            EventManager.OnLevelUpIncremental?.Invoke(IncrementalType);
            EventManager.OnMoneyChanged?.Invoke();
            RefreshButton();
            Database.Instance.SaveData();
        }

        protected virtual void RefreshButton()
        {
            bool isReachedMax = IncrementalLevel >= MaxIncrementalLevel;
            levelText.text = isReachedMax ? "MAX" : "Level " + IncrementalLevel.ToString("F0");
            costText.text = isReachedMax ? "COMING SOON" : IncrementalCost.ToString("F0");
            button.interactable = !isReachedMax && IncrementalCost <= Database.Instance.DataSO.Currency;
            print(button.interactable);
        }
    }
}
