using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class OverPanel : PanelBase
{
    public Button restartBtn;
    public Button backBtn;
    public Button urlBtn;
    public Text score;
    public Text tips;

    int nowScore;  //本局分数
    float nowTime; //本局用时
    string[] sp = null;
    Save saveData;
    string fileNam = "/save.dt";
    BinaryFormatter bf = new BinaryFormatter();
    bool hasLoadFromServ = false;

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
        hasLoadFromServ = false;

        restartBtn = skin.transform.Find("RestartBtn").GetComponent<Button>();
        restartBtn.onClick.AddListener(OnButtonClick_Restart);

        backBtn = skin.transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(OnButtonClick_Back);

        score = skin.transform.Find("Score").GetComponent<Text>();
        score.text = "";

        tips = skin.transform.Find("Tips").GetComponent<Text>();
        tips.text = "";

        urlBtn = skin.transform.Find("UrlBtn").GetComponent<Button>();
        urlBtn.onClick.AddListener(OnButtonClick_Url);

        //记录分数和用时
        nowScore = Game.instance.Score;
        nowTime = Time.time - Game.instance.beginTime;

        //加载存档
        LoadSaveFile();
        StartCoroutine("GetUserInfo");
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
                    if(!hasLoadFromServ)
                        return;
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
        string str = "本次得分：" + nowScore.ToString();
        int number;
        int count;
        if(sp != null)
        {
            number = int.Parse(sp[1]);
            count = int.Parse(sp[2]);
            if(sp[0] == "1")  //刷新记录
            {
                str += "\n排名刷新：第" + sp[1] + "名";
                str += "\n超过了" + string.Format("{0:F1}", ((float)(count - number) * 100.0f / (float)count)) + "% 的玩家";
                tips.text = "恭喜！刷新了记录！";
            }
            else if(sp[0] == "0")  //没刷新记录
            {
                str += "\n历史排名：第" + sp[1] + "名";
                str += "\n超过了" + string.Format("{0:F1}", ((float)(count - number) * 100.0f / (float)count)) + "% 的玩家";
                if(number <= 10)
                    tips.text = string.Format("恭喜您继续保持第{0}名的记录！", number);
                else
                    tips.text = string.Format("加油！差一点就上榜了，还差{0}分", saveData.data[saveData.data.Count - 1].score - nowScore);
            }
        }
        else
        {
            str += "\n历史最高得分：" + saveData.maxScore;
        }
        score.text = str;
        //
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
                if(nowScore > saveData.maxScore)
                {
                    saveData.maxScore = nowScore;  // 更新自身的最高分数
                }
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

    void GetUserInfo()
    {
        //更新服务器的分数
        if(saveData.username != "" && saveData.username != null)
        {
            IPAddress ip = IPAddress.Parse(Game.instance.HOST);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
            byte[] result = new byte[1024];
            int bytes = 0;
            try
            {
                sock.Connect(new IPEndPoint(ip, Game.instance.PORT));
                sock.Send(System.Text.Encoding.Default.GetBytes("USER " + saveData.username + " " + nowScore.ToString()));
                bytes = sock.Receive(result);
            }
            catch
            {
                hasLoadFromServ = true;
                PanelMgr.instance.OpenPanel<TipsPanel>("", "服务器异常");
            }
            if(bytes != 0)
            {
                string str = System.Text.Encoding.Default.GetString (result);
                sp = str.Split(" ".ToCharArray()[0]);  //得到0/1 , 排名， 总用户数
            }
            //更新一下最高分
            if(saveData.maxScore != nowScore)
            {
                sock.Send(System.Text.Encoding.Default.GetBytes("USER " + saveData.username + " " + saveData.maxScore.ToString()));
                bytes = sock.Receive(result);
            }
            sock.Close();
        }
        hasLoadFromServ = true;
    }

    public void OnButtonClick_Url()
    {
        Application.OpenURL("https://www.wjx.cn/jq/37568677.aspx");
    }
}
