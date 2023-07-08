using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HyperlabCase
{
    public class JumpCoroutine : MonoBehaviour
    {
        [SerializeField] private AnimationCurve movementCurveY;

        private Coroutine jumpCoroutine;
        private float initialPositionY;

        private void Awake()
        {
            initialPositionY = transform.localPosition.y;
        }

        private void OnEnable()
        {
            if (jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);
            jumpCoroutine = StartCoroutine(Jump());
        }

        private void OnDisable()
        {
            if (jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);
        }

        private IEnumerator Jump()
        {
            float timer = 0.5f;
            float height = 0.2f;

            Vector3 destinationPos = transform.localPosition;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                destinationPos.Set(transform.localPosition.x, initialPositionY + movementCurveY.Evaluate(0.5f - timer) * height, transform.localPosition.z);
                transform.localPosition = destinationPos;
                yield return null;
            }

            yield return Jump();
        }
    }
}
