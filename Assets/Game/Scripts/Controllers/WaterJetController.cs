using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.ProBuilder.MeshOperations;
using HyperlabCase.Managers;
using MoreMountains.Feedbacks;

namespace HyperlabCase.Controllers
{
    public class WaterJetController : MonoBehaviour
    {
        [SerializeField] private Transform waterJetTransform;
        [SerializeField] private P3dPaintSphere paintSphere;
        [SerializeField] private MMFeedbacks levelUpFeedback;

        private MeshRenderer meshRenderer;
        private float basePaintWidth;
        private float baseWaterJetWidth;
        private float baseWaterJetPosition;

        public Vector3 WaterJetSize { get => meshRenderer.bounds.size; }

        private void Awake()
        {
            meshRenderer = waterJetTransform.GetComponent<MeshRenderer>();
            baseWaterJetPosition = waterJetTransform.position.x;
            basePaintWidth = paintSphere.Scale.x;
            baseWaterJetWidth = waterJetTransform.localScale.x;
        }

        private void OnEnable()
        {
            EventManager.OnLevelUpIncremental += CheckForClearLevelUp;
        }

        private void OnDisable()
        {
            EventManager.OnLevelUpIncremental -= CheckForClearLevelUp;
        }

        private void Start()
        {
            RefreshWaterJetSize();
        }

        private void CheckForClearLevelUp(BaseIncrementalType type)
        {
            if (type != BaseIncrementalType.ClearLevel)
                return;

            RefreshWaterJetSize();
            levelUpFeedback.PlayFeedbacks();
        }

        private void RefreshWaterJetSize()
        {
            Vector3 originalScale = waterJetTransform.localScale;
            Vector3 scaleChange = new Vector3(baseWaterJetWidth * (1f + (Database.Instance.DataSO.ClearLevel - 1) * 0.1f), 1f, 1f);
            if (scaleChange.x >= 7f)
                scaleChange.x = 7f;

            Vector3 newScale = new Vector3(
                1 * scaleChange.x,
                originalScale.y * scaleChange.y,
                originalScale.z * scaleChange.z
            );

            Vector3 centerToPivot = waterJetTransform.position;
            centerToPivot.x = baseWaterJetPosition * (1f + (Database.Instance.DataSO.ClearLevel - 1) * 0.1f);

            waterJetTransform.localScale = newScale;
            waterJetTransform.position = centerToPivot;

            paintSphere.Scale = new Vector3(basePaintWidth * (1 + (Database.Instance.DataSO.ClearLevel - 1) * 0.1f), 5f, 0.3f);
        }
    }
}
