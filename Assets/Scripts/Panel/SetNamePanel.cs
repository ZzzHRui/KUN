using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SetNamePanel : PanelBase
{
    public Button confirmBtn;
    RectTransform rect = null;
    Vector3 scale = Vector3.one;
    BinaryFormatter bf = new BinaryFormatter();
    string fileNam = "/save.dt";
    STATE state;
    Text textName = null;
    Text tipText = null;

    enum STATE
    {
        None = 1,
        Open,
        Close
    }

    void Start()
    {
        confirmBtn = skin.transform.Find("ConfirmBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnButtonClick_Confirm);
        textName = skin.transform.Find("InputField/Text").GetComponent<Text>();
        tipText = skin.transform.Find("tips").GetComponent<Text>();
        rect = skin.GetComponent<RectTransform>();
        scale.y = 0.1f;
        rect.localScale = scale;
        state = STATE.Open;
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
        skinPath = "Prefabs/Panel/SetNamePanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Confirm()
    {
        //检查名字是否非法
        if(textName.text == "")
        {
            tipText.text = "请输入名字";
            return;
        }
        else if(textName.text.Contains(" "))
        {
            tipText.text = "包含非法字符，请换一个";
            return;
        }
        else if(textName.text.Length > 10)
        {
            tipText.text = "名字太长啦!需小于10个字符";
            return;
        }
        else
        {
            tipText.text = "名字可用";
        }
        string name = textName.text;
        name.Replace("\n", "");
        //todo，测试是否重名
        //新建一个savedata并保存
        Save saveData = new Save();
        saveData.username = name;
        saveData.maxScore = 0;
        FileStream f = null;
        try
        {
            f = File.Open(Application.persistentDataPath + fileNam, FileMode.Create);
            bf.Serialize(f, saveData);
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
        state = STATE.Close;
    }
}
