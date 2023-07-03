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

        private void RefreshCurrencyText()
        {
            float currency = Database.Instance.DataSO.Currency;
            string text = currency >= 1000 ? $"{(float)System.Math.Round(currency / 1000, 1)}K" : $"{(float)System.Math.Round(currency, 0)}";
            currencyText.text = text;
        }

        private void ShowEndGamePanel()
        {
            endGamePanelObjects.EndGamePanel.SetActive(true);
            Database.Instance.SaveData();
        }
    }
}
