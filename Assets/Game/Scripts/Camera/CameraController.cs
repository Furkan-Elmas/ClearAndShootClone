using Cinemachine;
using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 startFollowOffset = new Vector3(0f, 0.6f, -0.5f);
        [SerializeField] private Vector3 cleaningFollowOffset = new Vector3(0, 0.6f, -0.3f);
        [SerializeField] private Vector3 runningFollowOffset = new Vector3(0, 0.3f, -0.6f);
        [SerializeField] private Vector3 columnAttackFollowOffset = new Vector3(-0.02f, 0.33f, -0.9f);
        [SerializeField] private float baseCameraFOV = 55f;

        private CinemachineVirtualCamera cinemachineCamera;
        private CinemachineTransposer transposer;


        private void Awake()
        {
            cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
            transposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void OnEnable()
        {
            EventManager.OnPlayerTapToStart += SetCameraForCleaningState;
            EventManager.OnCleaningFinish += SetCameraForRunningState;
            EventManager.OnRunnerFinish += SetCameraForColumnAttack;
            EventManager.OnLevelUpIncremental += SetCameraFOV;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerTapToStart -= SetCameraForCleaningState;
            EventManager.OnCleaningFinish -= SetCameraForRunningState;
            EventManager.OnRunnerFinish -= SetCameraForColumnAttack;
            EventManager.OnLevelUpIncremental -= SetCameraFOV;
        }

        private void Start()
        {
            transposer.m_FollowOffset = startFollowOffset;
            SetCameraFOV();
        }

        private void SetCameraForCleaningState()
        {
            StartCoroutine(FollowOffsetChangingCoroutine(cleaningFollowOffset, 2f));
        }

        private void SetCameraForRunningState()
        {
            StartCoroutine(FollowOffsetChangingCoroutine(runningFollowOffset, 2f));
        }

        private void SetCameraForColumnAttack()
        {
            StartCoroutine(FollowOffsetChangingCoroutine(columnAttackFollowOffset, 0.5f));
        }

        private void SetCameraFOV(BaseIncrementalType type = default)
        {
            cinemachineCamera.m_Lens.FieldOfView = baseCameraFOV + Database.Instance.DataSO.ClearLevel * 0.5f;
        }

        private IEnumerator FollowOffsetChangingCoroutine(Vector3 targetVector, float speed)
        {
            while (transposer.m_FollowOffset != targetVector)
            {
                transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetVector, Time.deltaTime * speed);
                yield return null;
            }
        }
    }
}