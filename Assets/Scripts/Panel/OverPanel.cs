using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverPanel : PanelBase
{
    public Button restartBtn;
    public Button backBtn;
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        restartBtn = skin.transform.Find("RestartBtn").GetComponent<Button>();
        restartBtn.onClick.AddListener(OnButtonClick_Restart);

        backBtn = skin.transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(OnButtonClick_Back);

        score = skin.transform.Find("Score").GetComponent<Text>();
        score.text = "得分：" + Game.instance.Score.ToString();
    }

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/Panel/OverPanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Restart()
    {
        Game.instance.gameController.InitilizeAll();
        Close();
    }

    public void OnButtonClick_Back()
    {
        SceneManager.LoadScene("Title");
        Close();
    }

    
}
