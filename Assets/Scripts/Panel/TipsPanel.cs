using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TipsPanel : PanelBase {
	public Text text;//文字说明
	float beginTime = 0;//开始时间
	float lifeTime = 1.0f;
	// Use this for initialization
	void Start () 
    {
		text = skin.transform.Find("Text").GetComponent<Text>();
		text.text = Convert.ToString(args[0]);
		beginTime = Time.time;
	}
	
	public override void Init(params object[] args)
    {
		base.Init(args);
		skinPath = "Prefabs/Panel/TipsPanel";
		layer = PanelLayer.Tips;
	}

	// Update is called once per frame
	public override void Update () {
		if(Time.time - beginTime > lifeTime)
        {//达到时间删除
			Close();
		}
	}

}