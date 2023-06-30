using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperlabCase.Interfaces;
using TMPro;
using MoreMountains.Feedbacks;
using LiquidVolumeFX;
using HyperlabCase.Managers;

namespace HyperlabCase
{
    public class Gate : MonoBehaviour, IDamageable
    {
        [SerializeField] private GateData gateData;
        [SerializeField] private TMP_Text incrementalValueText;
        [SerializeField] private TMP_Text incrementalTypeText;
        [SerializeField] private List<Gate> sideGates;

        private LayerMask layer;
        private MMFeedbacks feedback;
        private LiquidVolume liquidVolume;
        private float currentIncreaseValue;


        private void Awake()
        {
            feedback = GetComponent<MMFeedbacks>();
            liquidVolume = GetComponentInChildren<LiquidVolume>();
            layer = LayerMask.GetMask("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layer.value & (1 << other.gameObject.layer)) > 0)
            {
                GiveIncremental();
                SetGatesNonactive();
            }
        }

        private void Start()
        {
            incrementalValueText.text = gateData.IncreaseValue.ToString("F1");
            incrementalTypeText.text = gateData.IncrementalType.ToString().ToUpper();
            currentIncreaseValue = gateData.IncreaseValue;

            if (currentIncreaseValue >= 0)
            {
                liquidVolume.liquidColor2 = Color.green;
                incrementalValueText.text = incrementalValueText.text.Insert(0, "+");
            }
            else
                liquidVolume.liquidColor2 = Color.red;
        }

        public void TakeDamage(float damage)
        {
            feedback.PlayFeedbacks();
            currentIncreaseValue += damage * 0.01f;
            incrementalValueText.text = currentIncreaseValue.ToString("F1");

            if (System.Math.Round(currentIncreaseValue,2) >= 0)
            {
                liquidVolume.liquidColor2 = Color.green;
                incrementalValueText.text = incrementalValueText.text.Insert(0, "+");
            }
            else if (System.Math.Round(currentIncreaseValue, 2) == 0)
            {
                liquidVolume.liquidColor2 = Color.gray;
            }
            else
                liquidVolume.liquidColor2 = Color.red;
        }

        private void SetGatesNonactive()
        {
            foreach (var gate in sideGates)
                gate.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void GiveIncremental()
        {
            EventManager.OnEarnedGateIncrement?.Invoke(gateData.IncrementalType, currentIncreaseValue);
        }
    }
}
