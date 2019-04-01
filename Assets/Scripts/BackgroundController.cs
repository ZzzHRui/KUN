using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    float SPEED_NORMAL = 6.0f;
    float SPEED_MAX;
    float SPEED_SLOWDOWN_ADD;
    float speed = 6.0f;  //相对于player的位移速度
    float z = 20.0f;  //本物体的position.z值
    GameObject player;
    STATE nowState = STATE.Normal;
    Vector3 deltaPos = Vector3.zero;
    GameObject[] backgrounds;
    public float offset_rate = 0.03f;

    enum STATE
    {
        Normal = 1,
        SlowDown
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Game.instance.player;
        Player p = player.GetComponent<Player>();
        p.eventSpeedMax += OnSpeedMax;
        p.eventSpeedSlow += OnSpeedSlowDown;
         SPEED_NORMAL = Game.instance.speed_down_background;
        SPEED_MAX = SPEED_NORMAL * Game.instance.speed_max_rate;
        SPEED_SLOWDOWN_ADD = -(SPEED_MAX - SPEED_NORMAL) / Game.instance.time_speed_slowdown;
        backgrounds = Game.instance.backgrounds;
        Initialize();
    }

    public void Initialize()
    {
        gameObject.transform.position = new Vector3(0, 0, z);
        //初始化背景的位置
        for(int i = 0; i < Game.instance.backgrounds.Length; i++)
        {
            Game.instance.backgrounds[i].transform.localPosition = new Vector3(0, Game.instance.offset_updateBackground * i, 0);
        }
        speed = SPEED_NORMAL;
        nowState = STATE.Normal;
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
        gameObject.transform.position = new Vector3(-player.transform.position.x * offset_rate, player.transform.position.y, z);  //更新自身位置
        //子物体相对后移
        deltaPos = new Vector3(0, speed * Time.deltaTime, 0);
        for(int i = 0; i < backgrounds.Length; i++)
        {
            if(player.transform.position.y - backgrounds[i].transform.position.y > Game.instance.offset_updateBackground)
            {
                OnUpadateBackground(i);
            }
            backgrounds[i].transform.localPosition -= deltaPos;
        }

    }

    void OnUpadateBackground(int i)
    {
        backgrounds[i].transform.localPosition += new Vector3(0, backgrounds.Length * Game.instance.offset_updateBackground, 0);
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
