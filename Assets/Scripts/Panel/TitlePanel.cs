using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePanel : PanelBase
{
    public Button startBtn;
    public Button listBtn;
    public Button quitBtn;

    // Start is called before the first frame update
    void Start()
    {
        startBtn = skin.transform.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnButtonClick_Start);

        listBtn = skin.transform.Find("ListBtn").GetComponent<Button>();
        listBtn.onClick.AddListener(OnButtonClick_List);

        quitBtn = skin.transform.Find("QuitBtn").GetComponent<Button>();
        quitBtn.onClick.AddListener(OnButtonClick_Quit);

    }

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/Panel/TitlePanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Start()
    {
        SceneManager.LoadScene("Loading");
    }

    public void OnButtonClick_List()
    {
        //todo
    }

    public void OnButtonClick_Quit()
    {
        //todo
    }
}
