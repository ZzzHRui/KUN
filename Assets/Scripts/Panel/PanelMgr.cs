using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PanelLayer
{
	Panel,//面板
	Tips,//提示
}

public class PanelMgr : MonoBehaviour 
{
	public static PanelMgr instance;//单例
	private GameObject canvas;//画板
	public Dictionary<string,PanelBase> dict;//面板,存放已打开的面板,如果已经打开则不用重复操作
	private Dictionary<PanelLayer,Transform> layerDict;//层级 

	void Awake () 
	{
		instance =this;
		InitLayer();
		dict =new Dictionary<string,PanelBase>();
	}
	
	//初始化层
	private void InitLayer()
	{
		//画布
		canvas=GameObject.Find("Canvas");
		if(canvas==null)
		{
			Debug.LogError("panelMgr.InitLayerfail,canvas is null");
		}
		//各个层级
		layerDict = new Dictionary<PanelLayer,Transform>();
		foreach(PanelLayer pl in Enum.GetValues(typeof(PanelLayer)))
		{
			string name=pl.ToString();
			Transform transform=canvas.transform.Find(name);
			layerDict.Add(pl,transform);
		}
	}

	//打开面板
	public void OpenPanel<T>(string skinPath,params object[] args) where T:PanelBase
	{
		//已经打开
		string name=typeof(T).ToString();
		if(dict.ContainsKey(name))
		{
			return;
		}
		//面板脚本
		PanelBase panel=canvas.AddComponent<T>();
		panel.Init(args);
		dict.Add(name,panel);
		//加载皮肤
		skinPath=(skinPath!=""?skinPath:panel.skinPath);
		GameObject skin=Resources.Load<GameObject>(skinPath);
		if(skin==null)
		{
			Debug.LogError("panelMgr.OpenPanel fail,skin is null ,skinPath="+skinPath);
		}
		panel.skin=(GameObject)Instantiate(skin);
		//坐标
		Transform skinTrans=panel.skin.transform;
		PanelLayer layer=panel.layer;
		Transform parent=layerDict[layer];
		skinTrans.SetParent(parent,false);
		//panel的生命周期
		panel.OnShowing();
		//anm
		panel.OnShowed();
	}

	//关闭面板
	public void ClosePanel(string name)
	{
		PanelBase panel=(PanelBase)dict[name];
		if(panel==null)
		{
			return;
		}
		panel.OnClosing();
		dict.Remove(name);
		panel.OnClosed();
		GameObject.Destroy(panel.skin);
		Component.Destroy(panel);
	}
}
