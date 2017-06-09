using UnityEngine;

class AudioManager : MonoBehaviourSingleton<AudioManager, AudioManager.Meta>
{
    public class Meta : MonoBehaviourSingletonMeta
    {
        public override string PrefabName
        {
            get
            {
                return "AudioManager";
            }
        }
    }

    private bool m_isInitialized = false;

    public Sound SoundMusic;
    public Sound SoundButton;
    public Sound[] SoundPixels;
    public Sound SoundFail;
    public Sound SoundVictory;
    public Sound SoundBonus1;
    public Sound SoundBonus2;
    public Sound SoundBonus3;
    public Sound SoundPaletteRollOut;
    public Sound SoundPaletteRollIn;

    public bool MusicEnabled { get; private set; }
    public bool SoundsEnabled { get; private set; }

    public void SetMusicEnabled(bool enabled)
    {
        MusicEnabled = enabled;

        if (enabled)
            SoundMusic.Play();
        else
            SoundMusic.Stop();
    }

    public void SetSoundsEnabled(bool enabled)
    {
        SoundsEnabled = enabled;
    }

    public void PlayPixelSound()
    {
        int randIndex = Random.Range(0, SoundPixels.Length);
        SoundPixels[randIndex].Play();
    }

    protected override void Awake()
    {
        if (!m_isInitialized)
            Init();

        m_instance = this;

        // SoundMusic = transform.FindChild("Music").GetComponent<Sound>();
        // SoundButton = transform.FindChild("Button").GetComponent<Sound>();
        // SoundDie = transform.FindChild("Die").GetComponent<Sound>();
        // SoundEndRound = transform.FindChild("EndRound").GetComponent<Sound>();
        // SoundJump = transform.FindChild("Jump").GetComponent<Sound>();
        // SoundJumpBridge = transform.FindChild("JumpBridge").GetComponent<Sound>();
        // SoundLand = transform.FindChild("Land").GetComponent<Sound>();
        // SoundSave = transform.FindChild("Save").GetComponent<Sound>();
        // SoundSummary = transform.FindChild("Summary").GetComponent<Sound>();
        // SoundSwim = transform.FindChild("Swim").GetComponent<Sound>();
        // SoundWaterSplash = transform.FindChild("WaterSplash").GetComponent<Sound>();
        // SoundPanic = transform.FindChild("Panic").GetComponent<Sound>();
        // SoundFire = transform.FindChild("Fire").GetComponent<Sound>();
        // SoundEarthquake = transform.FindChild("Earthquake").GetComponent<Sound>();
    }

    private void Init()
    {
        var options = Pix.Game.GetInstance().Options;

        options.SoundChanged += HandleSoundChanged;
        options.MusicChanged += HandleMusicChanged;

        SoundsEnabled = options.IsSoundEnabled();
        MusicEnabled = options.IsMusicEnabled();
        m_isInitialized = true;
    }

    private void HandleSoundChanged()
    {
        var options = Pix.Game.GetInstance().Options;
        SetSoundsEnabled(options.IsSoundEnabled());
    }

    private void HandleMusicChanged()
    {
        var options = Pix.Game.GetInstance().Options;
        SetMusicEnabled(options.IsMusicEnabled());
    }
}
