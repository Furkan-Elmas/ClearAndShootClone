using HyperlabCase.Controllers;
using HyperlabCase.Interfaces;
using HyperlabCase.Managers;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace HyperlabCase
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private float moneyValue = 33f;

        private LayerMask playerLayer;
        private TMP_Text healthText;
        private SkinnedMeshRenderer m_Renderer;
        private MMFeedbacks feedback;
        private Animator m_Animator;
        private float currentHealth;

        private void Awake()
        {
            healthText = GetComponentInChildren<TMP_Text>();
            m_Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            m_Animator = GetComponent<Animator>();
            feedback = GetComponent<MMFeedbacks>();
            currentHealth = enemyData.Health;
            healthText.text = currentHealth.ToString();
            playerLayer = LayerMask.GetMask("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                other.transform.root.GetComponentInChildren<PlayerController>().JumpBackward();
                TakeDamage(currentHealth);
            }
        }

        public void TakeDamage(float damage)
        {
            if (currentHealth <= 0)
                return;

            feedback.PlayFeedbacks();
            currentHealth -= damage;
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}";
            if (currentHealth <= 0)
            {
                healthText.enabled = false;
                Die();
            }

            m_Renderer.material.SetFloat("_Fill_Amount", currentHealth / enemyData.Health);
        }

        private void Die()
        {
            m_Animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            Database.Instance.DataSO.Currency += moneyValue;
            EventManager.OnMoneyChanged?.Invoke();
        }
    }
}
