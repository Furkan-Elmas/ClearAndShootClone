using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HyperlabCase.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private EndGamePanelObjects endGamePanelObjects;
        [SerializeField] private StartGamePanelObjects startGamePanelObject;


        private void OnEnable()
        {
            EventManager.OnMoneyChanged += RefreshCurrencyText;
            EventManager.OnPlayerHitColumn += ShowEndGamePanel;
        }

        private void OnDisable()
        {
            EventManager.OnMoneyChanged -= RefreshCurrencyText;
            EventManager.OnPlayerHitColumn -= ShowEndGamePanel;
        }

        private void Start()
        {
            RefreshCurrencyText();
            RefreshClearLevelButton();
            RefreshIncomeButton();
        }

        public void MoveToStartButtonClick()
        {
            startGamePanelObject.StartGamePanel.SetActive(false);
            EventManager.OnPlayerTapToStart?.Invoke();
        }

        public void TapToGoNextLevelButtonClick()
        {
            LevelManager.LoadNextLevel();
        }

        public void ClearLevelButtonClick()
        {
            if (Database.Instance.DataSO.Currency < Database.Instance.DataSO.PlayerData.ClearLevelCost())
                return;

            Database.Instance.DataSO.Currency -= Database.Instance.DataSO.PlayerData.ClearLevelCost();
            Database.Instance.DataSO.PlayerData.ClearLevel++;
            RefreshClearLevelButton();
            RefreshCurrencyText();
            EventManager.OnLevelUpIncremental?.Invoke(BaseIncrementalType.ClearLevel);
            Database.Instance.SaveData();
        }

        public void IncomeButtonClick()
        {
            if (Database.Instance.DataSO.Currency < Database.Instance.DataSO.PlayerData.IncomeLevelCost())
                return;

            Database.Instance.DataSO.Currency -= Database.Instance.DataSO.PlayerData.IncomeLevelCost();
            Database.Instance.DataSO.PlayerData.IncomeLevel++;
            RefreshIncomeButton();
            RefreshCurrencyText();
            EventManager.OnLevelUpIncremental?.Invoke(BaseIncrementalType.Income);
            Database.Instance.SaveData();
        }

        public void DamageButtonClick()
        {
            if (Database.Instance.DataSO.Currency < Database.Instance.DataSO.PlayerData.DamageLevelCost())
                return;

            Database.Instance.DataSO.Currency -= Database.Instance.DataSO.PlayerData.DamageLevelCost();
            Database.Instance.DataSO.PlayerData.DamageLevel++;
            RefreshDamageButton();
            RefreshCurrencyText();
            EventManager.OnLevelUpIncremental?.Invoke(BaseIncrementalType.Damage);
            Database.Instance.SaveData();
        }

        public void FireRateButtonClick()
        {
            if (Database.Instance.DataSO.Currency < Database.Instance.DataSO.PlayerData.FireRateLevelCost())
                return;

            Database.Instance.DataSO.Currency -= Database.Instance.DataSO.PlayerData.FireRateLevelCost();
            Database.Instance.DataSO.PlayerData.FireRateLevel++;
            RefreshFireRateButton();
            RefreshCurrencyText();
            EventManager.OnLevelUpIncremental?.Invoke(BaseIncrementalType.FireRate);
            Database.Instance.SaveData();
        }

        private void RefreshClearLevelButton()
        {
            if (Database.Instance.DataSO.PlayerData.ClearLevel >= Database.Instance.DataSO.PlayerData.MaxClearLevel)
            {
                startGamePanelObject.ClearLevelLevelText.text = "MAX";
                startGamePanelObject.ClearLevelCostText.text = "COMING SOON";
                startGamePanelObject.ClearLevelButton.interactable = false;
                return;
            }

            startGamePanelObject.ClearLevelLevelText.text = "Level " + Database.Instance.DataSO.PlayerData.ClearLevel.ToString("F0");
            startGamePanelObject.ClearLevelCostText.text = Database.Instance.DataSO.PlayerData.ClearLevelCost().ToString("F0");
            CheckButtonInteractable(startGamePanelObject.ClearLevelButton, Database.Instance.DataSO.PlayerData.ClearLevelCost());
            CheckButtonInteractable(startGamePanelObject.IncomeButton, Database.Instance.DataSO.PlayerData.IncomeLevelCost());
        }

        private void RefreshIncomeButton()
        {
            if (Database.Instance.DataSO.PlayerData.IncomeLevel >= Database.Instance.DataSO.PlayerData.MaxIncomeLevel)
            {
                startGamePanelObject.IncomeLevelText.text = "MAX";
                startGamePanelObject.IncomeCostText.text = "COMING SOON";
                startGamePanelObject.IncomeButton.interactable = false;
                return;
            }

            startGamePanelObject.IncomeLevelText.text = "Level " + Database.Instance.DataSO.PlayerData.IncomeLevel.ToString("F0");
            startGamePanelObject.IncomeCostText.text = Database.Instance.DataSO.PlayerData.IncomeLevelCost().ToString("F0");
            CheckButtonInteractable(startGamePanelObject.IncomeButton, Database.Instance.DataSO.PlayerData.IncomeLevelCost());
            CheckButtonInteractable(startGamePanelObject.ClearLevelButton, Database.Instance.DataSO.PlayerData.ClearLevelCost());
        }

        private void RefreshDamageButton()
        {
            if (Database.Instance.DataSO.PlayerData.DamageLevel >= Database.Instance.DataSO.PlayerData.MaxDamageLevel)
            {
                endGamePanelObjects.DamageLevelText.text = "MAX";
                endGamePanelObjects.DamageCostText.text = "COMING SOON";
                endGamePanelObjects.DamageButton.interactable = false;
                return;
            }

            endGamePanelObjects.DamageLevelText.text = "Level " + Database.Instance.DataSO.PlayerData.DamageLevel.ToString("F0");
            endGamePanelObjects.DamageCostText.text = Database.Instance.DataSO.PlayerData.DamageLevelCost().ToString("F0");
            CheckButtonInteractable(endGamePanelObjects.DamageButton, Database.Instance.DataSO.PlayerData.DamageLevelCost());
            CheckButtonInteractable(endGamePanelObjects.FireRateButton, Database.Instance.DataSO.PlayerData.FireRateLevelCost());
        }

        private void RefreshFireRateButton()
        {
            if (Database.Instance.DataSO.PlayerData.FireRateLevel >= Database.Instance.DataSO.PlayerData.MaxFireRateLevel)
            {
                endGamePanelObjects.FireRateText.text = "MAX";
                endGamePanelObjects.FireRateCostText.text = "COMING SOON";
                endGamePanelObjects.FireRateButton.interactable = false;
                return;
            }

            endGamePanelObjects.FireRateText.text = "Level " + Database.Instance.DataSO.PlayerData.FireRateLevel.ToString("F0");
            endGamePanelObjects.FireRateCostText.text = Database.Instance.DataSO.PlayerData.FireRateLevelCost().ToString("F0");
            CheckButtonInteractable(endGamePanelObjects.FireRateButton, Database.Instance.DataSO.PlayerData.FireRateLevelCost());
            CheckButtonInteractable(endGamePanelObjects.DamageButton, Database.Instance.DataSO.PlayerData.DamageLevelCost());

        }

        private void RefreshCurrencyText()
        {
            float currency = Database.Instance.DataSO.Currency;
            string text = currency >= 1000 ? $"{(float)System.Math.Round(currency / 1000, 1)}K" : $"{(float)System.Math.Round(currency, 0)}";
            currencyText.text = text;
        }

        private void CheckButtonInteractable(Button button, float cost)
        {
            if (cost > Database.Instance.DataSO.Currency)
                button.interactable = false;
            else
                button.interactable = true;
        }

        private void ShowEndGamePanel()
        {
            RefreshFireRateButton();
            RefreshDamageButton();
            endGamePanelObjects.EndGamePanel.SetActive(true);
            Database.Instance.SaveData();
        }
    }
}
