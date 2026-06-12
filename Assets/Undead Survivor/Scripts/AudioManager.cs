using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("# SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayer;
    int channelIndex;

    public enum Sfx { Dead,Hit,LevelUp=3,Lose,Melee,Range=7,Select,Win}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[channels];

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[i].playOnAwake = false;
            sfxPlayer[i].bypassListenerEffects = true;
            sfxPlayer[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
     bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayer.Length; index++) {

            int loopIndex = (index + channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if(sfx ==Sfx.Hit||sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayer[loopIndex].clip = sfxClip[(int)sfx];
            sfxPlayer[loopIndex].Play();
            break;
        }


      
    }


}
