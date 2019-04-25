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
    public Button refreshBtn;
    RectTransform rect = null;
    Vector3 scale = Vector3.one;
    STATE state;
    Text listText;
    Text myScore;

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
        refreshBtn = skin.transform.Find("RefreshBtn").GetComponent<Button>();
        refreshBtn.onClick.AddListener(OnButtonClick_Refresh);
        rect = skin.GetComponent<RectTransform>();
        scale.y = 0.1f;
        rect.localScale = scale;
        state = STATE.Open;

        listText = skin.transform.Find("ListText").GetComponent<Text>();
        listText.text = "";
        myScore = skin.transform.Find("MyScore").GetComponent<Text>();
        myScore.text = "";
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

    void OnButtonClick_Refresh()
    {
        TitleController titleCtrl = Camera.main.GetComponent<TitleController>();
        titleCtrl.GetListFromServer();
        LoadSaveFile();
        SetListText();
        PanelMgr.instance.OpenPanel<TipsPanel>("", "排行榜刷新成功");
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
        for(int i = 0; i < saveData.data.Count; i++)
        {
            str.AppendFormat("{0, -9}", "第" + (i + 1).ToString() + "名");
            str.AppendFormat("{0, -9}", saveData.data[i].username);
            str.AppendFormat("{0, 9}", saveData.data[i].score.ToString());
            str.Append("\n");
        }
        listText.text = str.ToString();

        myScore.text = saveData.maxScore.ToString();
    }
}
