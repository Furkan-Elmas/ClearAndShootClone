using UnityEngine;
using System.Collections.Generic;
using PaintIn3D;
using HyperlabCase.Managers;
using HyperlabCase.Collectables;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using System.Collections;
using Lean.Transition;
using HyperlabCase.Interfaces;
using MoreMountains.FeedbacksForThirdParty;

namespace HyperlabCase.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private List<WeaponPosition> weaponPositions;
        [SerializeField] private GameObject cleaningVisual;
        [SerializeField] private float maxDistanceX = 0.5f;
        [SerializeField] private float movementSpeed = 0.2f;
        [SerializeField] private float swerveSpeed = 5f;
        [SerializeField] private float distanceMultiplier = 0.0005f;
        [SerializeField] private Weapon spareWeapon;
        [SerializeField] private AnimationCurve jumpAnimationCurveZ;
        [SerializeField] private AnimationCurve jumpAnimationCurveY;

        private List<Weapon> earnedWeapons = new List<Weapon>();
        private WaterJetController waterJetController;
        private Coroutine backwardJumpCor;
        private Vector2 lastMousePosition;
        private Vector3 targetPosition;
        private LayerMask cleaningFinishLayer;
        private LayerMask runnerFinishLayer;
        private LayerMask columnLayer;
        private LayerMask enemyLayer;
        private float maxBoundary;
        private float minBoundary;
        private bool cleaningFinished;
        private bool runningFinished;
        private bool canMove;
        private bool jumpingBackward;


        private void Awake()
        {
            waterJetController = GetComponent<WaterJetController>();
            cleaningFinishLayer = LayerMask.GetMask("CleaningFinish");
            runnerFinishLayer = LayerMask.GetMask("RunnerFinish");
            columnLayer = LayerMask.GetMask("Column");
            enemyLayer = LayerMask.GetMask("Enemy");
        }

        private void OnEnable()
        {
            EventManager.OnPlayerTapToStart += OnPlayerTapToMove;
            EventManager.OnCollectedObject += AddWeaponToMovement;
            EventManager.OnCleaningFinish += CheckForSpareWeapon;
            EventManager.OnRunnerFinish += SetWeaponPositions;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerTapToStart -= OnPlayerTapToMove;
            EventManager.OnCollectedObject -= AddWeaponToMovement;
            EventManager.OnCleaningFinish -= CheckForSpareWeapon;
            EventManager.OnRunnerFinish -= SetWeaponPositions;
        }

        private void Start()
        {
            targetPosition = transform.position;
            minBoundary = -maxDistanceX + waterJetController.WaterJetSize.x / 2;
            maxBoundary = maxDistanceX - waterJetController.WaterJetSize.x / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckLinePassage(other.gameObject);
            CheckColumnHit(other.gameObject);
            CheckEnemyHit(other.gameObject);
        }

        private void Update()
        {
            HandleMovement();
        }

        private void OnPlayerTapToMove()
        {
            canMove = true;
        }

        private void HandleMovement()
        {
            if (!canMove)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = Input.mousePosition;
                float swipeDistance = currentMousePosition.x - lastMousePosition.x;

                targetPosition += distanceMultiplier * swipeDistance * Vector3.right;

                lastMousePosition = currentMousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPosition = transform.position;
            }

            targetPosition.z = transform.position.z;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBoundary, maxBoundary);
            transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward, Space.World);
            transform.position = Vector3.Lerp(transform.position, targetPosition, swerveSpeed * Time.deltaTime);
        }

        private void AddWeaponToMovement(Washable weapon)
        {
            if (weapon is not Weapon)
                return;

            Weapon weaponTemp = weapon as Weapon;
            weapon.transform.SetParent(null);
            MMFeedbackPosition positionFeedback = weapon.CollectedFeedback.GetComponent<MMFeedbackPosition>();

            foreach (WeaponPosition weaponPosition in weaponPositions)
            {
                if (weaponPosition.isAvailable)
                {
                    positionFeedback.InitialPosition = weapon.transform.position;
                    positionFeedback.DestinationPosition = weaponPosition.transform.position;
                    weaponTemp.FollowPosition(weaponPosition.transform, transform);
                    weaponPosition.isAvailable = false;
                    break;
                }
            }
            weapon.CollectedFeedback.PlayFeedbacks();
            earnedWeapons.Add((Weapon)weapon);
        }

        private void CheckLinePassage(GameObject _object)
        {
            if ((cleaningFinishLayer.value & (1 << _object.layer)) > 0 && !cleaningFinished)
            {
                cleaningFinished = true;
                movementSpeed = 0.5f;
                cleaningVisual.SetActive(false);
                EventManager.OnCleaningFinish?.Invoke();
            }
            else if ((runnerFinishLayer.value & (1 << _object.layer)) > 0 && !runningFinished)
            {
                runningFinished = true;
                EventManager.OnRunnerFinish?.Invoke();
            }
        }

        private void CheckColumnHit(GameObject _object)
        {
            if ((columnLayer.value & (1 << _object.layer)) > 0)
            {
                foreach (Weapon weapon in earnedWeapons)
                {
                    canMove = false;
                    weapon.Rigidbody.useGravity = true;
                    weapon.Collider.isTrigger = false;
                    EventManager.OnPlayerHitColumn?.Invoke();
                }
            }
        }

        private void CheckEnemyHit(GameObject _object)
        {
            if ((enemyLayer.value & (1 << _object.layer)) > 0)
            {
                if (jumpingBackward)
                    return;

                if (backwardJumpCor != null)
                    StopCoroutine(backwardJumpCor);
                backwardJumpCor = StartCoroutine(JumpBackCoroutine());

                _object.GetComponent<IDamageable>().TakeDamage(1000f);
            }
        }

        private IEnumerator JumpBackCoroutine()
        {
            jumpingBackward = true;
            canMove = false;

            float timer = 0.5f;
            float height = 0.3f;
            float backwardDistance = -0.3f;

            Vector3 initialPos = transform.localPosition;
            Vector3 destinationPos = transform.localPosition;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                destinationPos.Set(initialPos.x, initialPos.y + jumpAnimationCurveY.Evaluate(0.5f - timer) * height, initialPos.z + jumpAnimationCurveZ.Evaluate(0.5f - timer) * backwardDistance);
                transform.localPosition = destinationPos;

                yield return null;
            }

            jumpingBackward = false;
            canMove = true;
        }

        private void SetWeaponPositions()
        {
            foreach (var position in weaponPositions)
            {
                position.transform.localPosition /= 2;
            }
        }

        private void CheckForSpareWeapon()
        {
            minBoundary = -maxDistanceX;
            maxBoundary = maxDistanceX;

            foreach (var weapon in weaponPositions)
            {
                if (!weapon.isAvailable)
                    return;
            }

            spareWeapon.MakeCollected();
            spareWeapon.gameObject.SetActive(true);
            AddWeaponToMovement(spareWeapon);
            spareWeapon.BeginToAttack();
        }
    }
}
