using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    float SPEED_NORMAL = 6.0f;
    float SPEED_MAX;
    float SPEED_SLOWDOWN_ADD;
    int nextIdx = 0;
    float speed = 6.0f;  //相对于player的位移速度
    float z = 20.0f;  //本物体的position.z值
    GameObject player;
    STATE nowState = STATE.Normal;

    enum STATE
    {
        Normal = 1,
        SlowDown
    }

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
        p.eventSpeedMax += OnSpeedMax;
        p.eventSpeedSlow += OnSpeedSlowDown;

        SPEED_NORMAL = Game.instance.speed_down_background;
        SPEED_MAX = SPEED_NORMAL * Game.instance.speed_max_rate;
        SPEED_SLOWDOWN_ADD = -(SPEED_MAX - SPEED_NORMAL) / Game.instance.time_speed_slowdown;
        speed = SPEED_NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        switch(nowState)
        {
            case STATE.Normal:
                break;

            case STATE.SlowDown:
                if(speed > SPEED_NORMAL)
                {
                    speed += SPEED_SLOWDOWN_ADD * Time.deltaTime;
                }
                else
                {
                    speed = SPEED_NORMAL;
                    nowState = STATE.Normal;
                }
                break;
        }
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

    void OnSpeedMax()
    {
        speed = SPEED_MAX;
        nowState = STATE.Normal;
    }

    void OnSpeedSlowDown()
    {
        nowState = STATE.SlowDown;
    }
}
