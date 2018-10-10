using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicroPhoneRecord : MonoBehaviour
{
    public enum AudioRecordResultState { Success, NoMicrophone, TooShort }

    public static MicroPhoneRecord Instance;
    public static AudioSource Audio;

    private static string[] micArray = null;
    public float sensitivity = 100;
    public float loudness = 0;

    private int HEADER_SIZE = 44;
    private int RECORD_TIME = 10;

    private string fileSavePaht;
    private float recordTimer = 0.0f;

    private bool isRecording = false;

    private void Awake()
    {
        fileSavePaht = "Wav";
        Instance = this;
        micArray = Microphone.devices;
        if (micArray.Length == 0)
        {
            Debug.LogError("Microphone.devices is null");
        }
        foreach (string deviceStr in Microphone.devices)
        {
            Debug.Log("device name = " + deviceStr);
        }
        if (micArray.Length == 0)
        {
            Debug.LogError("no mic device");
        }

        Audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isRecording)
        {
            recordTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// 开始录制音频文件
    /// </summary>
    public void StartRecord()
    {
        Audio.Stop();
        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return;
        }
        Audio.loop = false;
        Audio.mute = true;
        Audio.clip = Microphone.Start(null, false, RECORD_TIME, 44100); //22050
        while (!(Microphone.GetPosition(null) > 0))
        {
        }
        Audio.Play();
        Debug.Log("StartRecord");
        isRecording = true;
        //倒计时
        StartCoroutine(TimeDown());
    }

    /// <summary>
    /// 停止录音 并且 调整音频文件的时间
    /// </summary>
    public AudioRecordResultState StopRecord()
    {
        Debug.Log("停止录音");

        isRecording = false;
        StopCoroutine(TimeDown());

        if (micArray.Length == 0)
        {
            Debug.Log("No Record Device!");
            return AudioRecordResultState.NoMicrophone;
        }
        if (!Microphone.IsRecording(null))
        {
            return AudioRecordResultState.NoMicrophone;
        }
        if (recordTimer < 0.5f)
        {
            return AudioRecordResultState.TooShort;
        }

        int position = Microphone.GetPosition(null);
        var soundData = new float[Audio.clip.samples * Audio.clip.channels];
        Audio.clip.GetData(soundData, 0);

        var newData = new float[position * Audio.clip.channels];

        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }

        Audio.clip = AudioClip.Create(Audio.clip.name, position, Audio.clip.channels, Audio.clip.frequency, false);
        Audio.clip.SetData(newData, 0);        //Give it the data from the old clip

        Microphone.End(null);
        Audio.Stop();

        return AudioRecordResultState.Success;
    }

    /// <summary>
    /// 上传音频文件
    /// </summary>
    public string UploadAudioClip()
    {
        var clip = WavUtility.FromAudioClip(Audio.clip);
        TextType tt = new TextType{};
        DownloadProp.Instance.UploadAudioText(tt, clip);
        return PublicAttribute.GetSecretKey();
    }

    /// <summary>
    /// 下载并播放音频文件
    /// </summary>
    public void PlayRecordByLoad()
    {
        DownloadProp.Instance.GetAudioText(PublicAttribute.GetSecretKey(), ((clip, type) =>
         {
             if (clip != null)
             {
                 Audio.clip = clip;
                 PlayRecordLocal();
             }
         }));
    }
    /// <summary>
    /// 下载并播放音频文件
    /// </summary>
    /// <param name="SecretKey">外部指定密钥</param>
    public void PlayRecordByLoad(string SecretKey)
    {
        DownloadProp.Instance.GetAudioText(SecretKey, ((clip, type) =>
        {
            if (clip != null)
            {
                Audio.clip = clip;
                PlayRecordLocal();
            }
        }));
    }

    /// <summary>
    /// 本地播放音频
    /// </summary>
    public void PlayRecordLocal()
    {
        if (Audio.clip == null)
        {
            Debug.Log("Audio.clip=null");
            return;
        }
        Audio.mute = false;
        Audio.loop = false;
        Audio.Play();
        Debug.Log("PlayRecordLocal");
    }

    private IEnumerator TimeDown()
    {
        int time = 0;
        while (time < RECORD_TIME)
        {
            if (!Microphone.IsRecording(null))
            { //如果没有录制
                yield break;
            }
            //Debug.Log("yield return new WaitForSeconds " + time);
            yield return new WaitForSeconds(1);
            time++;
        }
        if (time >= RECORD_TIME)
        {
            //Debug.Log("RECORD_TIME is out! stop record!");
            StopRecord();
        }
        yield return 0;
    }
}