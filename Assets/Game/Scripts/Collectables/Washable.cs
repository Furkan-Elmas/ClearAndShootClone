using MoreMountains.Feedbacks;
using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperlabCase.Managers;

namespace HyperlabCase.Collectables
{
    public class Washable : MonoBehaviour
    {
        [SerializeField] protected float paintPercentage = 0.98f;
        [SerializeField] protected MMFeedbacks cleanedFeedback;
        [SerializeField] protected MMFeedbacks collectedFeedback;

        protected P3dPaintableTexture P3dPaintableTexture;
        protected P3dChannelCounter P3dchannelCounter;
        protected bool isCollected;
        protected bool canBeCleaned = true;

        public MMFeedbacks CollectedFeedback { get => collectedFeedback; }


        protected virtual void Awake()
        {
            P3dchannelCounter = GetComponent<P3dChannelCounter>();
            P3dPaintableTexture = GetComponent<P3dPaintableTexture>();
        }

        protected virtual void OnEnable()
        {
            P3dchannelCounter.OnUpdated += HandleCollection;
        }

        protected virtual void OnDisable()
        {
            P3dchannelCounter.OnUpdated -= HandleCollection;
        }

        public void MakeCollected()
        {
            isCollected = true;
        }

        protected virtual bool CanBeCollected()
        {
            return P3dchannelCounter.RatioA > paintPercentage;
        }

        protected virtual void HandleCollection()
        {
            if (CanBeCollected() && !isCollected && canBeCleaned)
                StartCoroutine(BeCollected());
        }

        protected virtual IEnumerator BeCollected()
        {
            isCollected = true;
            cleanedFeedback.PlayFeedbacks();
            GetComponent<Collider>().enabled = false;
            P3dPaintableTexture.Deactivate();
            yield return new WaitForSeconds(cleanedFeedback.TotalDuration);
            EventManager.OnCollectedObject?.Invoke(this);
        }
    }
}
