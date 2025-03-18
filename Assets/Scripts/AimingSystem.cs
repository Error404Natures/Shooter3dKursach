using System.Collections;
using UnityEngine;

public class AimingSystem : MonoBehaviour
{
    public float aimDuration = 0.2f; // Продолжительность входа/выхода из прицела
    public float aimFOV = 30f; // Угол обзора при прицеливании
    public float hipFOV = 60f; // Угол обзора при стрельбе с бедра

    private Camera mainCamera;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private bool isAiming = false;

    private void Start()
    {
        mainCamera = Camera.main;
        initialCameraPosition = mainCamera.transform.position;
        initialCameraRotation = mainCamera.transform.rotation;
    }

    public void EnterAim()
    {
        if (!isAiming)
        {
            StartCoroutine(EnterAimCoroutine());
        }
    }

    public void ExitAim()
    {
        if (isAiming)
        {
            StartCoroutine(ExitAimCoroutine());
        }
    }

    private IEnumerator EnterAimCoroutine()
    {
        float timer = 0f;
        while (timer < aimDuration)
        {
            timer += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(hipFOV, aimFOV, timer / aimDuration);
            yield return null;
        }
        mainCamera.fieldOfView = aimFOV;
        isAiming = true;
    }

    private IEnumerator ExitAimCoroutine()
    {
        float timer = 0f;
        while (timer < aimDuration)
        {
            timer += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(aimFOV, hipFOV, timer / aimDuration);
            yield return null;
        }
        mainCamera.fieldOfView = hipFOV;
        isAiming = false;
    }
}