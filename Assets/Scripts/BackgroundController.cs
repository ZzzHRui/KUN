using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    int nextIdx = 0;
    float speed = 6.0f;  //相对于player的位移速度
    float z = 20.0f;  //本物体的position.z值
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, z);
        //初始化背景的位置
        for(int i = 0; i < Game.instance.backgrounds.Length; i++)
        {
            Game.instance.backgrounds[i].transform.localPosition = new Vector3(0, Game.instance.offset_updateBackground * i, 0);
        }
        player = Game.instance.player;
        Player p = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = player.transform.position.y;
        gameObject.transform.position = new Vector3(0, y, z);  //更新自身位置
        //子物体相对后移
        Vector3 deltaPos = new Vector3(0, speed * Time.deltaTime, 0);
        GameObject[] backgrounds = Game.instance.backgrounds;
        for(int i = 0; i < backgrounds.Length; i++)
        {
            if(player.transform.position.y - backgrounds[i].transform.position.y > Game.instance.offset_updateBackground)
                OnUpadateBackground();
            backgrounds[i].transform.localPosition -= deltaPos;
        }

    }

    void OnUpadateBackground()
    {
        GameObject[] backgrounds = Game.instance.backgrounds;
        backgrounds[nextIdx % backgrounds.Length].transform.localPosition += new Vector3(0, backgrounds.Length * Game.instance.offset_updateBackground, 0);
        nextIdx++;
    }
}
