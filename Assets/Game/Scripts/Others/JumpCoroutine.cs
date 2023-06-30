using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase
{
    public class JumpCoroutine : MonoBehaviour
    {
        [SerializeField] private float jumpHeight = 0.05f;
        [SerializeField] private float jumpDuration = 0.25f;
        [SerializeField] private float pauseDuration = 0.1f;

        private IEnumerator jumpCoroutine;

        private void OnEnable()
        {
            // Start the jump coroutine when the script is enabled
            jumpCoroutine = DoJumpCoroutine();
            StartCoroutine(jumpCoroutine);
        }

        private IEnumerator DoJumpCoroutine()
        {
            while (true)
            {
                float startY = transform.position.y;
                float targetY = startY + jumpHeight;
                float elapsedTime = 0f;

                while (elapsedTime < jumpDuration)
                {
                    float t = elapsedTime / jumpDuration;
                    float newY = Mathf.Lerp(startY, targetY, t);

                    // Update the object's position
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final position is exactly the target position
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

                // Wait for a short duration
                yield return new WaitForSeconds(pauseDuration);

                // Reset the position of the object after the jump
                elapsedTime = 0f;
                startY = targetY;
                targetY = startY - jumpHeight;

                while (elapsedTime < jumpDuration)
                {
                    float t = elapsedTime / jumpDuration;
                    float newY = Mathf.Lerp(startY, targetY, t);

                    // Update the object's position
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final position is exactly the target position
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
            }
        }

        private void OnDisable()
        {
            // Stop the coroutine when the script is disabled or destroyed
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
        }
    }
}
