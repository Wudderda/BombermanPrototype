using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Window to be displayed during the loading of the next level.
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image image;

    public const float fadeInDuration = 1.5f;
    public const float fadeOutDuration = 1.5f;

    public void OpenLoadingScreen()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void CloseLoadingScreen()
    {
        StartCoroutine(FadeOutRoutine());
    }

    /// <summary>
    /// Fades in the window slowly.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInRoutine()
    {
        image.enabled = true;
        Color imageColor = image.color;
        while (imageColor.a < 1)
        {
            imageColor.a += Time.unscaledDeltaTime / fadeInDuration;
            image.color = imageColor;

            yield return null;
        }
    }

    /// <summary>
    /// Fades out the window slowly.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutRoutine()
    {
        Color imageColor = image.color;
        while (imageColor.a > 0)
        {
            imageColor.a -= Time.unscaledDeltaTime / fadeOutDuration;
            image.color = imageColor;

            yield return null;
        }

        image.enabled = false;
    }
}