using UnityEngine;
using UnityEngine.Audio;

public class GameInit : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        GameSound.Instance.Init(audioMixer);
        GameManager.Instance.Init(gameEvent);
    }
}
