using UnityEngine.Audio;

public class GameSound : Singleton<GameSound>
{
    private AudioMixer m_audioMixer;

    public float SoundVolume
    {
        get { return GetVolume("SoundVolume"); }
        set { m_audioMixer.SetFloat("SoundVolume", value); }
    }

    public float MusicVolume
    {
        get { return GetVolume("MusicVolume"); }
        set { m_audioMixer.SetFloat("MusicVolume", value); }
    }

    public void Init(AudioMixer audioMixer)
    {
        m_audioMixer = audioMixer;
    }

    private float GetVolume(string name)
    {
        m_audioMixer.GetFloat(name, out float volume);
        return volume;
    }
}
