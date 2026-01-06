using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image fadeOutImage;
    public float fadeDuration = 1.0f;

    public void StartGame()
    {
        StartCoroutine(FadeOutAndLoadScene("1-1"));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        Color color = fadeOutImage.color;
        color.a = 0f;
        fadeOutImage.color = color;

        fadeOutImage.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            color.a = alpha;
            fadeOutImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
