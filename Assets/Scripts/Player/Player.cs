using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float SPEED_UP_NORMAL = 8.0f;
    float SPEED_UP_MAX = 16.0f;
    float SPEED_SLOWDOWN_ADD;
    int POWER_MAX = 200;
    //属性
    float speed_up = 8.0f;
    float maxSpeed_hor = 10.0f;
    float lastPos_Y_setPower = 0.0f;
    float lastPos_Y_setMonster = 0.0f;
    float lastPos_Y_setSkill = 0.0f;
    Rigidbody2D rigid;
    float force = 1.8f;
    int power = 0;

    STATE state = STATE.None;
    float protectTime = 0.2f;  //受伤之后的无敌时间
    bool isProtecting = false;
    float stateTime_last = 0.0f;
    
    //技能相关
    bool portected = false;  //进入保护状态，免疫一次伤害
    bool god = false;  //无敌
    CircleCollider2D getTrigger = null;
    float getTriggerRadius = 0.8f;

    enum STATE
    {
        None = 0,  //普通状态
        SlowDown,  //减速到普通速度
        Hurt,  //受伤状态
        Dying,  //死亡中
        End  //死亡，游戏结束
    }

    //事件
    public delegate void Handler();
    public event Handler eventSetPower;  //生成power
    public event Handler eventSetMonster;  //生成怪物
    public event Handler eventSetSkill;  //生成道具
    public event Handler eventDead;  //死亡
    public event Handler eventSpeedMax;  //进入加速状态
    public event Handler eventSpeedSlow;  //进入减速状态

    public int Power { 
        get => power;
        set{
            if(value < 0)
                power = 0; 
            else if(value > POWER_MAX)
                power = POWER_MAX;
            else
                power = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        getTrigger = gameObject.transform.Find("GetTrigger").GetComponent<CircleCollider2D>();
        getTrigger.radius = getTriggerRadius;
        Initialize();
    }

    void Initialize()
    {
        SPEED_UP_NORMAL = Game.instance.speed_up_player;
        SPEED_UP_MAX = SPEED_UP_NORMAL * Game.instance.speed_max_rate;
        SPEED_SLOWDOWN_ADD = (-SPEED_UP_MAX + SPEED_UP_NORMAL) / Game.instance.time_speed_slowdown;
        lastPos_Y_setPower = gameObject.transform.position.y;
        lastPos_Y_setMonster = lastPos_Y_setPower;
        lastPos_Y_setSkill = lastPos_Y_setPower;
        speed_up = SPEED_UP_NORMAL;
        Power = 50;
    }

    // Update is called once per frame
    void Update()
    {
        //改变y值
        float y = this.speed_up * Time.deltaTime;
        Vector3 deltaPos =  new Vector3(0, y, 0);
        gameObject.transform.position += deltaPos;
        //生成power检查
        float nowY = gameObject.transform.position.y;
        if(nowY - lastPos_Y_setPower >= Game.instance.offset_setPower)
        {
            eventSetPower();
            lastPos_Y_setPower = nowY;
        }
        //生成monster检查
        if(nowY - lastPos_Y_setMonster >= Game.instance.offset_setMonster)
        {
            eventSetMonster();
            lastPos_Y_setMonster = nowY;
        }
        //生成道具检查
        if(nowY - lastPos_Y_setSkill >= Game.instance.offset_setSkill)
        {
            eventSetSkill();
            lastPos_Y_setSkill = nowY;
        }

        switch(state)
        {
            case STATE.None:
                break;

            case STATE.SlowDown:
                if(speed_up > SPEED_UP_NORMAL)
                {
                    speed_up += SPEED_SLOWDOWN_ADD * Time.deltaTime;
                }
                else
                {
                    speed_up = SPEED_UP_NORMAL;
                    state = STATE.None;
                    god = false;
                }
                break;
                    
            case STATE.Hurt:  //如果受伤致死则直接进入Dying状态
                if(Time.time - stateTime_last > protectTime)
                {
                    state = STATE.None;
                    isProtecting = false;
                }
                break;
                    
            case STATE.Dying:
                break;
            
            case STATE.End:
                break;
        }
    }

    void FixedUpdate()
    {
        //横向施力
        float force = 0.0f;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(rigid.velocity.x > -this.maxSpeed_hor)
                force = this.force * -1.0f;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            if(rigid.velocity.x < this.maxSpeed_hor)
                force = this.force * 1.0f;
        }
        //触屏测试
        if(Input.touchCount == 1)  //有触摸
        {
            //测试，只检测x值
            if(Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                Vector2 pos = Input.GetTouch(0).position;
                if(pos.x < Game.instance.screenRect.rect.width / 2)  //左边
                {
                    if(rigid.velocity.x > -this.maxSpeed_hor)
                        force = this.force * -1.0f;
                }
                else  //右边
                {
                    if(rigid.velocity.x < this.maxSpeed_hor)
                        force = this.force * 1.0f;
                }
            }
        }

        if (force != 0.0f)
        {
            Vector2 tempForce = new Vector2(force, 0);
            rigid.AddForce(tempForce, ForceMode2D.Impulse);
        }
    }

    //被敌对势力攻击
    public void BeAttacked(int attack)
    {
        if(god || isProtecting)
            return;
        if(portected)
        {
            portected = false;
            return;
        }
        Power -= attack;
        state = STATE.Hurt;
        stateTime_last = Time.time;
        //判断死亡

        isProtecting = true;
    }

    //受到陷阱伤害，至少会保留1点能量
    public void BeHurted(int hurted)
    {
        Power -= hurted;
        //判断是否低于1
        if(Power < 1)
            Power = 1;
    }

    public void SpeedMax()
    {
        speed_up = SPEED_UP_MAX;
        eventSpeedMax();
    }

    public void GodIn()
    {
        god = true;
    }

    public void IntoState_None()
    {
        if(speed_up > SPEED_UP_NORMAL)
        {
            state = STATE.SlowDown;
            eventSpeedSlow();
        }
        else
            state = STATE.None;
    }

    public void SetProtected(bool value=true)
    {
        portected = value;
    }

    public bool GetProtected()
    {
        return portected;
    }

    public void SetTriggerSize(float rate)
    {
        if(getTrigger != null)
            getTrigger.radius = rate * getTriggerRadius;
    }
}
