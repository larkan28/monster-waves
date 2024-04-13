using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate;
    [SerializeField] private bool loop;
    [SerializeField] private bool autoStart;

    private SpriteRenderer m_spriteRenderer;
    private int m_spriteId;
    private bool m_isPlaying;
    private float m_timer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = sprites[0];

        if (autoStart)
            m_isPlaying = true;
    }

    private void Update()
    {
        if (!m_isPlaying)
            return;

        m_timer += Time.deltaTime;
        
        if (m_timer >= frameRate)
        {
            if (m_spriteId >= (sprites.Length - 1) && !loop)
            {
                m_isPlaying = false;
                m_spriteRenderer.sprite = null;
                return;
            }

            m_timer -= frameRate;
            m_spriteId = (m_spriteId + 1) % sprites.Length;
            m_spriteRenderer.sprite = sprites[m_spriteId];
        }
    }

    public void Play()
    {
        m_spriteRenderer.sprite = sprites[0];
        m_isPlaying = true;
        m_spriteId = 0;
        m_timer = 0f;
    }
}
