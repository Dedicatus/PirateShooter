using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject target;

    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] float shakeRange = 1.0f;
    [SerializeField] float kickDistance = 2.0f;
    [SerializeField] Vector3 Offset;

    bool isShaking;

    Vector3 targetPosition;
    Vector3 shakeOffset;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector3 finalOffset;
        if (Movement.facingRight)
        {
            finalOffset = new Vector3(Offset.x, Offset.y, Offset.z);
        }
        else
        {
            finalOffset = new Vector3(-Offset.x, Offset.y, Offset.z);
        }

        targetPosition = target.transform.position + finalOffset;
        if (isShaking) targetPosition += shakeOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public IEnumerator Shake(float duration, float magnitude, bool faceRight)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            isShaking = true;

            float x = Random.Range(-shakeRange, shakeRange) * magnitude;
            float y = Random.Range(-shakeRange, shakeRange) * magnitude;

            x += faceRight ? -kickDistance : kickDistance;

            shakeOffset = new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        isShaking = false;
    }
}
