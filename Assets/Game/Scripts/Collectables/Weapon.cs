using HyperlabCase.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperlabCase.ScriptableObjects;
using Lean.Transition;
using HyperlabCase.StateMachine;
using HyperlabCase.Controllers;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;

namespace HyperlabCase.Collectables
{
    public class Weapon : Washable
    {
        [SerializeField] private PoolObjectTypes projectileType;
        [SerializeField] private MMFeedbacks fireFeedback;
        [SerializeField] private WeaponDataSO weaponData;
        [SerializeField] private Transform firePoint;
        [SerializeField] private ParticleSystem muzzle;
        [SerializeField] private GameObject weaponLevelPlate;

        private Transform playerTransform;
        private Coroutine attackCoroutine;
        private Coroutine followCoroutine;
        private WaitForSeconds fireRateWaitingTime;
        private WeaponPoolManager poolManager;
        private GameObject projectile;
        private LayerMask cleaningFinishLayer;
        private float currentDamage;
        private float currentFireRate;
        private bool cleaningFinished;
        private bool canMove = true;

        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            cleaningFinishLayer = LayerMask.GetMask("CleaningFinish");
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.OnEarnedGateIncrement += EarnIncremental;
            EventManager.OnPlayerHitColumn += StopAttack;
            EventManager.OnLevelUpIncremental += CheckWeaponLevel;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.OnEarnedGateIncrement -= EarnIncremental;
            EventManager.OnPlayerHitColumn -= StopAttack;
            EventManager.OnLevelUpIncremental -= CheckWeaponLevel;
        }

        private void Start()
        {
            currentDamage = weaponData.Damage * (1f + (Database.Instance.DataSO.PlayerData.DamageLevel - 1) * 0.1f);
            currentFireRate = weaponData.FireRate * (1f - (Database.Instance.DataSO.PlayerData.FireRateLevel - 1) * 0.01f);
            fireRateWaitingTime = new WaitForSeconds(currentFireRate);

            RefreshWeaponPlate();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckLinePassage(other.gameObject);
        }

        public void FollowPosition(Transform point, Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            if (followCoroutine != null)
                StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(FollowCoroutine(point));
        }

        private IEnumerator FollowCoroutine(Transform point)
        {
            while (canMove)
            {
                transform.position = Vector3.Lerp(transform.position, point.position, 10f * Time.deltaTime);
                yield return null;
            }
        }

        private void CheckLinePassage(GameObject _object)
        {
            if ((cleaningFinishLayer.value & (1 << _object.layer)) > 0 && !cleaningFinished)
            {
                BeginToAttack();
            }
        }

        public void BeginToAttack()
        {
            if (!isCollected)
                return;

            cleaningFinished = true;
            transform.SetParent(playerTransform);
            poolManager = EventManager.GetWeaponPoolManager.Invoke();

            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            while (true)
            {
                projectile = poolManager.GetPooledObject(projectileType);
                projectile.transform.position = firePoint.position;
                projectile.GetComponent<Rigidbody>().velocity = transform.root.forward * 2f;
                projectile.GetComponent<Projectile>().SetData(currentDamage);

                fireFeedback.PlayFeedbacks();

                if (muzzle != null)
                    muzzle.Play();

                yield return fireRateWaitingTime;
            }
        }

        private void EarnIncremental(GateIncrementalType type, float value)
        {
            switch (type)
            {
                case GateIncrementalType.Damage:
                    currentDamage += value;
                    break;

                case GateIncrementalType.FireRate:
                    currentFireRate -= currentFireRate * value / 100;
                    fireRateWaitingTime = new WaitForSeconds(currentFireRate);
                    break;
            }
        }

        private void StopAttack()
        {
            if (!isCollected)
                return;

            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);

            canMove = false;
        }

        private void CheckWeaponLevel(BaseIncrementalType type)
        {
            if (type != BaseIncrementalType.ClearLevel)
                return;

            RefreshWeaponPlate();
        }

        private void RefreshWeaponPlate()
        {
            if (Database.Instance.DataSO.PlayerData.ClearLevel < weaponData.Level)
            {
                canBeCleaned = false;
                Color newColor = P3dPaintableTexture.Color;
                newColor.a = 0.2f;
                P3dPaintableTexture.Color = newColor;
                weaponLevelPlate.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                canBeCleaned = true;
                Color newColor = P3dPaintableTexture.Color;
                newColor.a = 1f;
                P3dPaintableTexture.Color = newColor;
                weaponLevelPlate.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
    }
}
