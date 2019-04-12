using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel : PanelBase
{
    public Button closeBtn;
    public Button urlBtn;
    RectTransform rect = null;
    Vector3 scale = Vector3.one;
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
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnButtonClick_Close);
        urlBtn = skin.transform.Find("UrlBtn").GetComponent<Button>();
        urlBtn.onClick.AddListener(OnButtonClick_Url);
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
        skinPath = "Prefabs/Panel/AboutPanel";
        layer = PanelLayer.Panel;
    }

    public void OnButtonClick_Close()
    {
        state = STATE.Close;
        MyAudio.instance.PlayClickBtn();
    }

    public void OnButtonClick_Url()
    {
        Application.OpenURL("https://www.wjx.cn/jq/37568677.aspx");
    }
}
