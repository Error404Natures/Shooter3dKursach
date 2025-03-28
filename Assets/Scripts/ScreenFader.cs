using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
       fadeImage.gameObject.SetActive(true);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); 

        while (timer < fadeDuration)
        {
            //������ ���� ����� ������� � ������
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        //� ����� ������ ���� ��������� ������
        fadeImage.color = endColor;
    }
}
