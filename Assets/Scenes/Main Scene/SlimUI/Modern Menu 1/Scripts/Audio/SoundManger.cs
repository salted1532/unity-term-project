using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//사운드의 타입이다. 사운드를 중단을 식별하기위해 사용한다.
public enum SoundType
{
    BGM,
    EFFECT,
    MONSTER_SOUND,
    MASTER,
}

public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// 오디오 믹서, 오디오의 타입별로 사운드를 조절할 수 있도록 한다.
    /// </summary>
    [SerializeField] private AudioMixer mAudioMixer;

    //옵션에서 설정된 현재 배경음악과 효과 사운드의 불륨이다. 효과는 BGM을 제외한 모든 소리의 불륨을 담당한다.
    public float mCurrentBGMVolume, mCurrentEffectVolume;

    /// <summary>
    /// 클립들을 담는 딕셔너리
    /// </summary>
    private Dictionary<string, AudioClip> mClipsDictionary;

    /// <summary>
    /// 사전에 미리 로드하여 사용할 클립들
    /// </summary>
    [SerializeField] private AudioClip[] mPreloadClips;

    private List<TemporarySoundPlayer> mInstantiatedSounds;

    private void Start()
    {
        mClipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in mPreloadClips)
        {
            mClipsDictionary.Add(clip.name, clip);
        }

        mInstantiatedSounds = new List<TemporarySoundPlayer>();
    }

    /// <summary>
    /// 오디오의 이름을 기반으로 찾는다.
    /// </summary>
    /// <param name="clipName">오디오의 이름(파일 이름 기준)</param>
    /// <returns></returns>
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = mClipsDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "이 존재하지 않습니다."); }

        return clip;
    }

    /// <summary>
    /// 사운드를 재생할 때, 루프 형태로 재생된경우에는 나중에 제거하기위해 리스트에 저장한다.
    /// </summary>
    /// <param name="soundPlayer"></param>
    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        mInstantiatedSounds.Add(soundPlayer);
    }

    public bool IsSoundInLoop(string clipName)
    {
        foreach(TemporarySoundPlayer audioPlayer in mInstantiatedSounds)
        {
            if(audioPlayer.ClipName == clipName)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 루프 사운드 중 리스트에 있는 오브젝트를 이름으로 찾아 제거한다.
    /// </summary>
    /// <param name="clipName"></param>
    public void StopLoopSound(string clipName)
    {
        foreach(TemporarySoundPlayer audioPlayer in mInstantiatedSounds)
        {
            if(audioPlayer.ClipName == clipName)
            {
                mInstantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogWarning(clipName + "을 찾을 수 없습니다.");
    }

    public void StopLoopSoundAfterEnd(string clipName)
    {
        foreach(TemporarySoundPlayer audioPlayer in mInstantiatedSounds)
        {
            if(audioPlayer.ClipName == clipName)
            {
                mInstantiatedSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }
    }

    /// <summary>
    /// 2D 사운드로 재생한다. 거리에 상관 없이 같은 소리 크기로 들린다.
    /// </summary>
    /// <param name="clipName">오디오 클립 이름</param>
    /// <param name="type">오디오 유형(BGM, EFFECT 등.)</param>
    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        GameObject old = GameObject.Find(clipName);
        if(old != null) {
            Destroy(old);
        }

        GameObject obj = new GameObject(clipName);
        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //루프를 사용하는경우 사운드를 저장한다.
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    /// <summary>
    /// 3D 사운드로 재생한다.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="audioTarget"></param>
    /// <param name="type"></param>
    /// <param name="attachToTarget"></param>
    /// <param name="minDistance"></param>
    /// <param name="maxDistance"></param>
    public void PlaySound3D(string clipName, GameObject audioTarget, float minDistance = 0.0f, float maxDistance = 25.0f, bool isLoop = false, SoundType type = SoundType.EFFECT, float delay = 0f, bool attachToTarget = true)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 3D");
        obj.transform.localPosition = audioTarget.transform.position;
        if (attachToTarget) { obj.transform.parent = audioTarget.transform; }

        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //루프를 사용하는경우 사운드를 저장한다.
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound3D(GetClip(clipName), minDistance, maxDistance);
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    }

    //씬이 로드될 때 옵션 매니저에의해 모든 사운드 불륨을 저장된 옵션의 크기로 초기화시키는 함수.
    public void InitVolumes(float bgm, float effect,float master)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.EFFECT, effect);
        SetVolume(SoundType.MASTER, master);
    }

    //옵션을 변경할 때 소리의 불륨을 조절하는 함수
    public void SetVolume(SoundType type, float value)
    {
        if(value <= 0){
            value = 0.001f;
        }
        mAudioMixer.SetFloat(type.ToString(), Mathf.Log10(value) * 20);
    }

    /// <summary>
    /// 무작위 사운드를 실행하기위해 랜덤 값을 리턴 (included)
    /// </summary>
    /// <param name="from">시작하는 인덱스 번호</param>
    /// <param name="includedTo">끝나는 인덱스 번호(포함)</param>
    /// <param name="isStartZero">한자리일경우 0으로 시작하는가? 예)01</param>
    /// <returns></returns>
    public static string Range(int from, int includedTo, bool isStartZero = false)
    {
        if (includedTo > 100 && isStartZero) { Debug.LogWarning("0을 포함한 세자리는 지원하지 않습니다."); }

        int value = UnityEngine.Random.Range(from, includedTo + 1);

        return value < 10 && isStartZero ? '0' + value.ToString() : value.ToString();
    }
}