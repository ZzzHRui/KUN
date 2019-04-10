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
    public Button aboutBtn;
    public Button optionBtn;

    // Start is called before the first frame update
    void Start()
    {
        startBtn = skin.transform.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnButtonClick_Start);

        listBtn = skin.transform.Find("ListBtn").GetComponent<Button>();
        listBtn.onClick.AddListener(OnButtonClick_List);

        aboutBtn = skin.transform.Find("AboutBtn").GetComponent<Button>();
        aboutBtn.onClick.AddListener(OnButtonClick_About);

        quitBtn = skin.transform.Find("QuitBtn").GetComponent<Button>();
        quitBtn.onClick.AddListener(OnButtonClick_Quit);

        optionBtn = skin.transform.Find("OptionBtn").GetComponent<Button>();
        optionBtn.onClick.AddListener(OnButtonClick_Option);
        
        MyAudio.instance.SetBGM("Title");
        MyAudio.instance.PlayBGM();
    }

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "Prefabs/Panel/TitlePanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Start()
    {
        MyAudio.instance.PlayClickBtn();
        SceneManager.LoadScene("Loading");
    }

    public void OnButtonClick_List()
    {
        MyAudio.instance.PlayClickBtn();
        PanelMgr.instance.OpenPanel<ListPanel>("");
    }

    public void OnButtonClick_About()
    {
        MyAudio.instance.PlayClickBtn();
        PanelMgr.instance.OpenPanel<AboutPanel>("");
    }

    public void OnButtonClick_Option()
    {
        MyAudio.instance.PlayClickBtn();
        PanelMgr.instance.OpenPanel<OptionPanel>("");
    }

    public void OnButtonClick_Quit()
    {
        MyAudio.instance.PlayClickBtn();
        Application.Quit();
    }
}
