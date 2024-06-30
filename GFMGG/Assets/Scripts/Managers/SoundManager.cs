using HietakissaUtils.Pooling;
using HietakissaUtils.QOL;
using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class SoundManager : Manager
{
    public static SoundManager Instance;

    [SerializeField] bool initializeOnStart;
    ObjectPool<AudioSource> sourcePool;

    [SerializeField] SoundContainer ballHitBallSound;
    [SerializeField] SoundContainer ballHitWallSound;
    [SerializeField] SoundContainer ballHitSound;
    [SerializeField] SoundContainer keyboardClackSound;
    [SerializeField] SoundContainer successSound;
    [SerializeField] SoundContainer failSound;
    [SerializeField] SoundContainer walkSound;
    [SerializeField] SoundContainer ballPocketedSound;
    [SerializeField] SoundContainer gruntSound;
    [SerializeField] SoundContainer punchSound;
    [SerializeField] SoundContainer blockSound;

    AudioSource CreateSource()
    {
        AudioSource source = new GameObject($"Pooled AudioSource [{sourcePool.pooledObjects.Length + 1}]", typeof(AudioSource)).GetComponent<AudioSource>();
        source.transform.parent = transform;
        return source;
    }
    void GetFromPool(AudioSource source) { }
    void ReturnToPool(AudioSource source) { }

    void Awake()
    {
        if (initializeOnStart) Initialize();
    }


    public override void Initialize()
    {
        Instance = this;
        sourcePool = new ObjectPool<AudioSource>(ReturnToPool, GetFromPool, CreateSource);
    }

    public void PlaySound(SoundContainer sound)
    {
        AudioSource source = sourcePool.Get();
        sound.ApplyToAudioSource(source);
        source.Play();
    }

    public void PlaySound(SoundType type)
    {
        AudioSource source = sourcePool.Get();
        switch (type)
        {
            case SoundType.BallHitBall: ballHitBallSound.ApplyToAudioSource(source); break;
            case SoundType.BallHitWall: ballHitWallSound.ApplyToAudioSource(source); break;
            case SoundType.BallHit: ballHitSound.ApplyToAudioSource(source); break;
            case SoundType.KeyboardClack: keyboardClackSound.ApplyToAudioSource(source); break;
            case SoundType.Success: successSound.ApplyToAudioSource(source); break;
            case SoundType.Fail: failSound.ApplyToAudioSource(source); break;
            case SoundType.Walk: walkSound.ApplyToAudioSource(source); break;
            case SoundType.BallPocketed: ballPocketedSound.ApplyToAudioSource(source); break;
            case SoundType.Grunt: gruntSound.ApplyToAudioSource(source); break;
            case SoundType.Punch: punchSound.ApplyToAudioSource(source); break;
            case SoundType.Block: blockSound.ApplyToAudioSource(source); break;
            default: Debug.Log($"Sound not implemented for '{type}'"); break;
        }

        ReturnSourceToPool(source);
        source.Play();
    }

    void ReturnSourceToPool(AudioSource source)
    {
        StartCoroutine(ReturnSourceToPoolCor());

        IEnumerator ReturnSourceToPoolCor()
        {
            float time = source.GetMaxClipLength().RoundUp();
            yield return QOL.WaitForSeconds.Get(time);
            sourcePool.Return(source);
        }
    }
}

public enum SoundType
{
    BallHitBall,
    BallHitWall,
    BallHit,
    KeyboardClack,
    Success,
    Fail,
    Walk,
    BallPocketed,
    Grunt,
    Punch,
    Block
}