using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f; // ����������������� ������
    public float shakeAmplitude = 0.1f; // ��������� ������

    private Vector3 initialPosition; // �������� ��������� ������
    private Quaternion initialRotation; // �������� ������� ������

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
            transform.localPosition = new Vector3(x, y, 0); // ���������� localPosition ��� ������
            yield return null;
        }
        // ���������� ������ � �������� ���������
        transform.localPosition = new Vector3(0, (float)1.66, 0);
    }
}