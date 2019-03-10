using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    int nextIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        //初始化背景的位置
        for(int i=0; i < Game.instance.backgrounds.Length; i++)
        {
            Game.instance.backgrounds[i].transform.position = new Vector3(0, Game.instance.offset_updateBackground * i, 0);
        }

        Player p = Game.instance.player.GetComponent<Player>();
        p.eventUpdateBackground += OnUpadateBackground;  //注册
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUpadateBackground()
    {
        GameObject[] backgrounds = Game.instance.backgrounds;
        backgrounds[nextIdx % backgrounds.Length].transform.position += new Vector3(0, backgrounds.Length * Game.instance.offset_updateBackground, 0);
        nextIdx++;
    }
}
