using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour {
	public string skinPath;//皮肤路径
	public GameObject skin;//皮肤
	public PanelLayer layer;//层级
	public object[] args;//面板参数

	//初始化
	public virtual void Init(params object[] args)
	{
		this.args=args;
	}

	//面板开始前
	public virtual void OnShowing(){}
	//面板显示后
	public virtual void OnShowed(){}
	//帧更新
	public virtual void Update(){}
	//关闭前
	public virtual void OnClosing(){}
	//关闭后
	public virtual void OnClosed(){}

	protected virtual void Close()
	{
		string name=this.GetType().ToString();
		PanelMgr.instance.ClosePanel(name);
	}
	
}
