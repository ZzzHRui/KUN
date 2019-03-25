using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080,1920,false);
		PanelMgr.instance.OpenPanel<TitlePanel>("");
    }
}
