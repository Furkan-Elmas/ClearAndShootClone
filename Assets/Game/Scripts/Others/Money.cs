using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private bool canRotate = true;
        [SerializeField] private float rotateSpeed = 75f;
        [SerializeField] private float moneyValue = 13f;

        private LayerMask layer;


        private void Awake()
        {
            layer = LayerMask.GetMask("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layer.value & (1 << other.gameObject.layer)) > 0)
            {
                float newValue = moneyValue * (1 + (Database.Instance.DataSO.PlayerData.IncomeLevel - 1) * 0.1f);
                Database.Instance.DataSO.Currency += newValue;
                EventManager.OnMoneyChanged?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!canRotate)
                return;

            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
    }
}
