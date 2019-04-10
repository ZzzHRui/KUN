using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverPanel : PanelBase
{
    public Button restartBtn;
    public Button backBtn;
    public Text score;
    public Text tips;

    int nowScore;  //本局分数
    float nowTime; //本局用时
    Save saveData;
    string fileNam = "/save.dt";
    BinaryFormatter bf = new BinaryFormatter();

    RectTransform rect = null;
    Vector3 scale = Vector3.one;
    STATE state;

    enum STATE
    {
        None = 1,
        Open,
        Restart,
        Close
    }

    // Start is called before the first frame update
    void Start()
    {
        rect = skin.GetComponent<RectTransform>();
        scale.y = 0.1f;
        rect.localScale = scale;
        state = STATE.Open;

        restartBtn = skin.transform.Find("RestartBtn").GetComponent<Button>();
        restartBtn.onClick.AddListener(OnButtonClick_Restart);

        backBtn = skin.transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(OnButtonClick_Back);

        score = skin.transform.Find("Score").GetComponent<Text>();
        score.text = "";

        tips = skin.transform.Find("Tips").GetComponent<Text>();
        tips.text = "";

        //记录分数和用时
        nowScore = Game.instance.Score;
        nowTime = Time.time - Game.instance.beginTime;

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
                    ShowScoreAndTips();
                }
                break;
            
            case STATE.Restart:
                if(rect.localScale.y >= 0.1f)
                {
                    scale.y += -6f * Time.deltaTime;
                }
                else
                {
                    Game.instance.gameController.InitilizeAll();
                    Close();
                    return;
                }
                break;

            case STATE.Close:
                if(rect.localScale.y >= 0.1f)
                {
                    scale.y += -6f * Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene("Title");
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
        skinPath = "Prefabs/Panel/OverPanel";
        layer = PanelLayer.Panel;
    }

    public void ShowScoreAndTips()
    {
        string str = "得分：" + nowScore.ToString();
        str += "\n坚持时间：" + nowTime.ToString() + "s";
        score.text = str;
        if(saveData == null)  //不存在该文件，为第一名
        {
            tips.text = "破纪录啦！获得第1名！";
            saveData = new Save();
            saveData.Insert(nowScore, nowTime);
            Debug.LogError("unexit");
        }
        else
        {
            int i = saveData.Insert(nowScore, nowTime);
            if(i == -1)  //没上榜
            {
                tips.text = "差一点就刷新记录了，加油！";
            }
            else if (i == 1)
            {
                tips.text = "恭喜！破记录啦！获得第1名!";
                //触发特效?
            }
            else
            {
                tips.text = "刷新榜单啦！获得第" + i.ToString() + "名!\n距离前一名只差" + (saveData.data[i - 2].score - nowScore).ToString() + "分啦！";
            }
        }
        SaveSaveFile();
    }

    public void OnButtonClick_Restart()
    {
        state = STATE.Restart;
        MyAudio.instance.PlayClickBtn();
    }

    public void OnButtonClick_Back()
    {
        state = STATE.Close;
        MyAudio.instance.PlayClickBtn();
    }

    void SaveSaveFile()
    {
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
}
