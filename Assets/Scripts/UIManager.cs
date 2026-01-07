using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject exitGamePanel;
    [SerializeField] private Image fadeImage;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        inputReader.PauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= TogglePause;

        transform.DOKill();
    }

    public IEnumerator FadeOut(float duration)
    {
        Color c = fadeImage.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            fadeImage.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        Color c = fadeImage.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / duration);
            fadeImage.color = c;
            yield return null;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            inputReader.InputActions.Gameplay.Disable();
            Time.timeScale = 0f;
            ShowExitGamePanel();
            inputReader.SetUI();
        }
        else
        {
            inputReader.InputActions.Gameplay.Enable();
            Time.timeScale = 1f;
            exitGamePanel.SetActive(false);
            inputReader.SetGameplay();
        }
    }

    private void ShowExitGamePanel()
    {
        exitGamePanel.SetActive(true);
        exitGamePanel.transform.localScale = Vector3.zero;
        exitGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
        .SetUpdate(true);
    }

    public void ConfirmExit()
    {
        inputReader.InputActions.Enable();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelExit()
    {
        exitGamePanel.transform.DOScale(Vector3.zero, 0.2f).SetUpdate(true).OnComplete(() => {
            TogglePause(); // State değişimini ve input değişimini burada yap
        });
    }
}
