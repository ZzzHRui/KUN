using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class OptionPanel : PanelBase
{
    public Button confirmBtn;
    RectTransform rect = null;
    Vector3 scale = Vector3.one;
    Slider volumBGM = null;
    Slider volumClip = null;
    string fileNam = "/config.cf";
    BinaryFormatter bf = new BinaryFormatter();
    STATE state;

    enum STATE
    {
        None = 1,
        Open,
        Close
    }

    // Start is called before the first frame update
    void Start()
    {
        confirmBtn = skin.transform.Find("ConfirmBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnButtonClick_Confirm);
        rect = skin.GetComponent<RectTransform>();
        scale.y = 0.1f;
        rect.localScale = scale;
        state = STATE.Open;
        
        volumBGM = skin.transform.Find("VolumeBGM").GetComponent<Slider>();
        volumClip = skin.transform.Find("VolumeClip").GetComponent<Slider>();
        //设置滑块位置
        volumBGM.SetRate(MyAudio.instance.volume.volumeBGM);
        volumClip.SetRate(MyAudio.instance.volume.VolumeClip);
    }

    public override void Update()
    {
        switch(state)
        {
            case STATE.None:
                return;

            case STATE.Open:
                if(rect.localScale.y < 1f)
                {
                    scale.y += 6f * Time.deltaTime;
                }
                else
                {
                    scale.y = 1.0f;
                    state = STATE.None;
                }
                break;

            case STATE.Close:
                if(rect.localScale.y >= 0.1f)
                {
                    scale.y += -6f * Time.deltaTime;
                }
                else
                {
                    Close();
                    return;
                }
                break;
        }
        rect.localScale = scale;
    }

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/Panel/OptionPanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Confirm()
    {
        state = STATE.Close;
        //保存新的音量设置
        MyAudio.instance.volume.volumeBGM = volumBGM.GetRate();
        MyAudio.instance.volume.VolumeClip = volumClip.GetRate();
        FileStream f = null;
        try
        {
            f = File.Open(Application.persistentDataPath + fileNam, FileMode.Create);
            bf.Serialize(f, MyAudio.instance.volume);
        }
        catch(IOException)
        {
        }
        catch(System.Runtime.Serialization.SerializationException)
        {
        }
        finally
        {
            f.Close();
        }
        
        MyAudio.instance.PlayClickBtn();
    }
}
