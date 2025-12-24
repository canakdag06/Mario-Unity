using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public int World { get; private set; }
    public int Stage { get; private set; }
    public int Lives { get; private set; }
    public int Coins { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        Lives = 3;
        Coins = 0;
        LoadLevel(1, 1);
    }

    private void LoadLevel(int world, int stage)
    {
        World = world;
        Stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void NextLevel()
    {
        LoadLevel(World, Stage + 1);
    }

    public void ResetLevel()
    {
        Lives--;

        if (Lives > 0)
        {
            LoadLevel(World, Stage);
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        Coins++;

        if (Coins == 100)
        {
            AddLife();
            Coins = 0;
        }
    }

    public void AddLife()
    {
        Lives++;
    }

    private void GameOver()
    {
        // TODO
        NewGame();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
