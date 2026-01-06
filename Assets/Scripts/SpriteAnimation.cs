using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    public SpriteRenderer Renderer { get; private set; }
    public Sprite[] sprites;
    public float FPS = 15f;
    public int Frame { get; private set; }
    public bool loop = true;

    private float timer;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Restart();
    }

    private void Update()
    {
        if (FPS <= 0) return;
        timer += Time.deltaTime;

        if (timer >= 1f / FPS)
        {
            timer = 0f;
            NextFrame();
        }
    }

    private void NextFrame()
    {
        if (!Renderer.enabled || sprites.Length == 0) return;

        Frame++;

        if (Frame >= sprites.Length)
        {
            if (loop)
            {
                Frame = 0;
            }
            else
            {
                Frame = sprites.Length - 1;
                enabled = false;
                return;
            }
        }

        Renderer.sprite = sprites[Frame];
    }

    public void Restart()
    {
        Frame = 0;
        timer = 0f;
        if (sprites.Length > 0)
        {
            Renderer.sprite = sprites[0];
        }
    }
}