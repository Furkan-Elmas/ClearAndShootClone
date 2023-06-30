using HyperlabCase.Interfaces;
using HyperlabCase.Managers;
using HyperlabCase.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private PoolObjectTypes impactType;

        private WeaponPoolManager poolManager;
        private GameObject impact;

        private float damage;


        private void Awake()
        {
            poolManager = EventManager.GetWeaponPoolManager.Invoke();
        }

        private void OnEnable()
        {
            Invoke(nameof(DestroySelf), 1.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((interactableLayers.value & (1 << other.gameObject.layer)) > 0)
            {
                ApplyDamage(other);
            }
        }

        public void SetData(float damage)
        {
            this.damage = damage;
        }

        private void ApplyDamage(Collider target)
        {
            target.GetComponent<IDamageable>().TakeDamage(damage);

            impact = poolManager.GetPooledObject(impactType);
            if (impact != null)
                impact.transform.position = transform.position;

            Invoke(nameof(DestroyImpact), 1f);

            CancelInvoke(nameof(DestroySelf));
            DestroySelf();
        }

        private void DestroySelf()
        {
            poolManager.SendObjectToPool(gameObject);
        }

        private void DestroyImpact()
        {
            poolManager.SendObjectToPool(impact);
        }
    }
}
