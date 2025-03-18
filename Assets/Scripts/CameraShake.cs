using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f; // Продолжительность тряски
    public float shakeAmplitude = 0.1f; // Амплитуда тряски

    private Vector3 initialPosition; // Исходное положение камеры
    private Quaternion initialRotation; // Исходный поворот камеры

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float x = Mathf.Sin(timer * 10) * shakeAmplitude;
            float y = Mathf.Cos(timer * 10) * shakeAmplitude;
            transform.localPosition = new Vector3(x, y, 0); // Используем localPosition для тряски
            yield return null;
        }
        // Возвращаем камеру в исходное положение
        transform.localPosition = new Vector3(0, (float)1.66, 0);
    }
}