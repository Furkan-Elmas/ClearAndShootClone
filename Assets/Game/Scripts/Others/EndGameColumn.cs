using HyperlabCase.Interfaces;
using LiquidVolumeFX;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HyperlabCase
{
	public class EndGameColumn : MonoBehaviour, IDamageable
	{
		[SerializeField] private EndGameColumnData data;
		[SerializeField] private TMP_Text healthText;
        [SerializeField] private GameObject moneyGO;

        private MMFeedbacks feedback;
        private float currentHealth;

        private void Awake()
        {
            healthText = GetComponentInChildren<TMP_Text>();
            feedback = GetComponent<MMFeedbacks>();

            healthText.text = data.Health.ToString();
            currentHealth = data.Health;
        }

        public void TakeDamage(float damage)
        {
            if (currentHealth <= 0)
                return;

            feedback.PlayFeedbacks();
            currentHealth -= damage;
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}";
            if (currentHealth <= 0)
                Demolish();
        }

        private void Demolish()
        {
            moneyGO.transform.SetParent(null);
            gameObject.SetActive(false);
        }
    } 
}
