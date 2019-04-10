using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : PanelBase
{
    public Button closeBtn;
    RectTransform rect = null;
    Vector3 scale = Vector3.one;
    STATE state;
    Text listText;

    Save saveData;
    string fileNam = "/save.dt";
    BinaryFormatter bf = new BinaryFormatter();

    enum STATE
    {
        None = 1,
        Open,
        Close
    }

    // Start is called before the first frame update
    void Start()
    {
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnButtonClick_Close);
        rect = skin.GetComponent<RectTransform>();
        scale.y = 0.1f;
        rect.localScale = scale;
        state = STATE.Open;

        listText = skin.transform.Find("ListText").GetComponent<Text>();
        listText.text = "";
        //加载存档
        LoadSaveFile();
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
                    SetListText();
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
        skinPath = "Prefabs/Panel/ListPanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Close()
    {
        state = STATE.Close;
        MyAudio.instance.PlayClickBtn();
    }

    void LoadSaveFile()
    {
        if(File.Exists(Application.persistentDataPath + fileNam))
        {
            FileStream f = null;
            try
            {
                f = File.Open(Application.persistentDataPath + fileNam, FileMode.Open);
                saveData = (Save)bf.Deserialize(f);
            }
            catch(IOException)
            {
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                saveData = null;
            }
            finally
            {
                f.Close();
            }
        }
    }

    void SetListText()
    {
        if(saveData == null)
            return;
        if(saveData.data.Count == 0)
        {
            listText.text = "还没有记录哦!";
            return;
        }
        StringBuilder str = new StringBuilder();
        string s = "";
        for(int i = 0; i < saveData.data.Count; i++)
        {
            str.AppendFormat("{0, -10}", "第" + (i + 1).ToString() + "名");
            str.AppendFormat("{0, -10}", saveData.data[i].score.ToString());
            s = string.Format("{0:F2}", saveData.data[i].time);
            str.AppendFormat("{0, -10}", s + "s");
            str.Append("\n");
        }
        listText.text = str.ToString();
    }
}
