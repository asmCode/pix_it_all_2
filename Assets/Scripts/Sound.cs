using UnityEngine;

class Sound : MonoBehaviour
{
    public bool m_playFromBeginning = true;

    private AudioSource m_audioSource;

    public float Volume
    {
        get { return m_audioSource.volume; }
        set { m_audioSource.volume = value; }
    }

    public void Play()
    {
        if (AudioManager.GetInstance().SoundsEnabled)
        {
            if (!IsPlaying() || m_playFromBeginning)
                m_audioSource.Play();
        }
    }

    public void Stop()
    {
        m_audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return m_audioSource.isPlaying;
    }

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
}
