using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct Volume
{
    public float volumeBGM;
    public float VolumeClip;
}

public class MyAudio : MonoBehaviour
{
    /*用于加载并存放所有音频资源 */
    public AudioClip clickBtn;
    public AudioClip getPower;
    public AudioClip getPowerUp;
    public AudioClip speedUp;
    public AudioClip getSkill;
    public AudioClip beAttack;

    public AudioClip titleBGM;
    public AudioClip mainBGM;

    static public MyAudio instance;
    AudioSource audioSource;
    public Volume volume;
    string fileNam = "/config.cf";
    BinaryFormatter bf = new BinaryFormatter();

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        audioSource = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        //加载配置文件，设置音量
        LoadConfig();
    }

    public void PlayClickBtn()
    {
        if(clickBtn != null)
            audioSource.PlayOneShot(clickBtn, 1.0f * volume.VolumeClip);
    }

    public void PlayGetPower()
    {
        if(getPower != null)
            audioSource.PlayOneShot(getPower, 0.6f * volume.VolumeClip);
    }

    public void PlayGetPowerUp()
    {
        if(getPowerUp != null)
            audioSource.PlayOneShot(getPowerUp, 1.0f * volume.VolumeClip);
    }

    public void PlaySpeedUp()
    {
        if(speedUp != null)
            audioSource.PlayOneShot(speedUp, 1.0f * volume.VolumeClip);
    }

    public void PlayGetSkill()
    {
        if(getSkill != null)
            audioSource.PlayOneShot(getSkill, 1.0f * volume.VolumeClip);
    }

    public void PlayBeAttack()
    {
        if(beAttack != null)
            audioSource.PlayOneShot(beAttack, 1.0f * volume.VolumeClip);
    }

    public void SetBGM(string bgm)
    {
        audioSource.volume = volume.volumeBGM;
        switch(bgm)
        {
            case "Title":
                if(titleBGM == null)
                    return;
                audioSource.clip = titleBGM;
                break;

            case "Main":
                if(mainBGM == null)
                    return;
                audioSource.clip = mainBGM;
                break;  
        }
    }

    public void PlayBGM()
    {
        if(audioSource.clip != null)
            audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetPitch(float n)
    {
        audioSource.pitch = n;
    }

    public void LoadConfig()
    {
        if(File.Exists(Application.persistentDataPath + fileNam))
        {
            FileStream f = null;
            try
            {
                f = File.Open(Application.persistentDataPath + fileNam, FileMode.Open);
                Volume tempVolume = (Volume)bf.Deserialize(f);
                MyAudio.instance.volume = tempVolume;
                MyAudio.instance.SetVolume(MyAudio.instance.volume.volumeBGM);
            }
            catch(IOException)
            {
                volume.volumeBGM = 1.0f;
                volume.VolumeClip = 1.0f;
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                volume.volumeBGM = 1.0f;
                volume.VolumeClip = 1.0f;
            }
            finally
            {
                f.Close();
            }
        }
    }
}
