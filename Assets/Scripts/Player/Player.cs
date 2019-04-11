using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float SPEED_UP_NORMAL;
    float SPEED_UP_MAX;
    float SPEED_SLOWDOWN_ADD;
    float SPEED_UP_ADD;
    int POWER_MAX;
    GameObject sprites = null;
    //属性
    float speed_up = 8.0f;
    float maxSpeed_hor = 8.0f;
    float lastPos_Y_setPower = 0.0f;
    float lastPos_Y_setMonster = 0.0f;
    float lastPos_Y_setSkill = 0.0f;
    Rigidbody2D rigid;
    float force = 1.8f;
    int power = 0;
    int action = 0;

    STATE state = STATE.None;
    float protectTime = 1.0f;  //受伤之后的无敌时间
    float dyingTime = 2.0f;  //死亡中的时间
    float powerUpTime = 1.0f;  //能量满了之后的保护时间
    bool isProtecting = false;  //保护中不会受伤
    float stateTime_last = 0.0f;
    
    //技能相关
    bool portected = false;  //进入保护状态，免疫一次伤害
    bool god = false;  //无敌
    bool usingSkill = false;
    CircleCollider2D getTrigger = null;
    Collider2D attackCollider = null;
    float getTriggerRadius = 0.8f;
    GameObject protectPopo = null;
    ParticleSystem superLight_out = null;  //加速雨效果
    List<ParticleSystem> superLights_in = null;  //自身加速效果
    List<ParticleSystem> magnetLights = null;  //磁铁效果

    //动画相关
    Animator animator;

    //动作变量
    Vector3 deltaPos = Vector3.zero;
    Vector2 nowForce = Vector2.zero;

    enum STATE
    {
        None = 0,  //普通状态
        SlowDown,  //减速到普通速度
        PowerUp,  //满能量，一小段时间进入保护状态，免得
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
    public event Handler eventPowerMax;  //能量满了
    public event Handler eventBeHurted;  //意外受伤
    public event Handler eventBeAttack;  //被攻击，也意味着能量有降低

    public int Power { 
        get => power;
        set{
            if(value < 0)
                power = 0; 
            else if(value >= POWER_MAX)
                power = POWER_MAX;
            else
                power = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        sprites = gameObject.transform.Find("sprite").gameObject;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        getTrigger = gameObject.transform.Find("GetTrigger").GetComponent<CircleCollider2D>();
        attackCollider = gameObject.GetComponent<Collider2D>();
        protectPopo = gameObject.transform.Find("sprite/body/protect").gameObject;
        animator = gameObject.GetComponent<Animator>();
        GameObject temp = Camera.main.transform.Find("superLight").gameObject;
        if(temp != null)
            superLight_out = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("sprite/superLight/superLight1").gameObject;
        superLights_in = new List<ParticleSystem>();
        if(temp != null)
            superLights_in.Add(temp.GetComponent<ParticleSystem>());
        temp = gameObject.transform.Find("sprite/superLight/superLight2").gameObject;
        if(temp != null)
            superLights_in.Add(temp.GetComponent<ParticleSystem>());
        magnetLights = new List<ParticleSystem>();
        temp = gameObject.transform.Find("sprite/magnetLight/magnetLight1").gameObject;
        if(temp != null)
            magnetLights.Add(temp.GetComponent<ParticleSystem>());
        temp = gameObject.transform.Find("sprite/magnetLight/magnetLight2").gameObject;
        if(temp != null)
            magnetLights.Add(temp.GetComponent<ParticleSystem>());
        Initialize();
    }

    public void Initialize()
    {
        sprites.SetActive(true);
        gameObject.transform.position = new Vector3(0.0f, Game.instance.offset_camera, 0.0f);
        attackCollider.enabled = true;
        getTrigger.enabled = true;
        getTrigger.radius = getTriggerRadius;
        POWER_MAX = Game.instance.maxPower;
        SPEED_UP_NORMAL = Game.instance.speed_up_player;
        SPEED_UP_MAX = SPEED_UP_NORMAL * Game.instance.speed_max_rate;
        SPEED_SLOWDOWN_ADD = (-SPEED_UP_MAX + SPEED_UP_NORMAL) / Game.instance.time_speed_slowdown;
        // SPEED_UP_ADD = SPEED_UP_NORMAL / 1.0f;
        lastPos_Y_setPower = gameObject.transform.position.y;
        lastPos_Y_setMonster = lastPos_Y_setPower;
        lastPos_Y_setSkill = lastPos_Y_setPower;
        speed_up = SPEED_UP_NORMAL;
        Power = 50;
        state = STATE.None;
        portected = false;
        action = 0;
        isProtecting = false;
        stateTime_last = Time.time;
        god = false;
        usingSkill = false;
        if(protectPopo != null)
            protectPopo.SetActive(false);
        if(superLight_out != null)
            superLight_out.Stop();
        if(superLights_in != null)
        {
            foreach(ParticleSystem n in superLights_in)
                n.Stop();
        }
        if(magnetLights != null)
        {
            foreach(ParticleSystem n in magnetLights)
                n.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //生成power检查
        float nowY = gameObject.transform.position.y;
        if(nowY - lastPos_Y_setPower >= Game.instance.offset_setPower)
        {
            eventSetPower();
            lastPos_Y_setPower = float.MaxValue;
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
                    god = false;
                    IntoState_None();
                }
                break;

            case STATE.PowerUp:
                if(Time.time - stateTime_last > powerUpTime)
                {
                    IntoState_None();
                }
                break;
                    
            case STATE.Hurt:  //如果受伤致死则直接进入Dying状态，不需要在此判断是否死亡
                if(Time.time - stateTime_last > protectTime)
                {
                    IntoState_None();
                }
                break;
                    
            case STATE.Dying:
                if(Time.time - stateTime_last > dyingTime)
                {
                    state = STATE.End;
                }
                break;
            
            case STATE.End:
                // gameObject.SetActive(false);
                sprites.SetActive(false);
                PanelMgr.instance.OpenPanel<OverPanel>("");
                break;
        }
    }

    void FixedUpdate()
    {
        //改变y值
        float y = this.speed_up * Time.deltaTime;
        deltaPos.y = y;
        gameObject.transform.position += deltaPos;
        if(state >= STATE.Dying)  //死亡后不再可操作
            return;
        //横向施力
        float force = 0.0f;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(rigid.velocity.x > -this.maxSpeed_hor)
                force = this.force * -1.0f;
            action = -1;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            if(rigid.velocity.x < this.maxSpeed_hor)
                force = this.force * 1.0f;
            action = 1;
        }
        // //触屏测试
        // if(Input.touchCount == 1)  //有触摸
        // {
        //     //测试，只检测x值
        //     if(Input.GetTouch(0).phase == TouchPhase.Stationary)
        //     {
        //         Vector2 pos = Input.GetTouch(0).position;
        //         if(pos.x < Game.instance.screenRect.rect.width / 2)  //左边
        //         {
        //             if(rigid.velocity.x > -this.maxSpeed_hor)
        //                 force = this.force * -1.0f;
        //         }
        //         else  //右边
        //         {
        //             if(rigid.velocity.x < this.maxSpeed_hor)
        //                 force = this.force * 1.0f;
        //         }
        //     }
        // }

        if(action == -1)  //左边
        {
            if(rigid.velocity.x > -this.maxSpeed_hor)
            force = this.force * -1.0f;
        }
        else if(action == 1)  //右边
        {
            if(rigid.velocity.x < this.maxSpeed_hor)
                force = this.force * 1.0f;
        }

        if (force != 0.0f)
        {
            nowForce.x = force;
            rigid.AddForce(nowForce, ForceMode2D.Impulse);
        }

        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -action * 30.0f);
    }

    //被敌对势力攻击
    public bool BeAttacked(int attack)
    {
        if(god || isProtecting)
            return false;
        if(portected)
        {
            portected = false;
            protectPopo.SetActive(false);
            return false;
        }
        
        Power -= attack;
        animator.SetTrigger("BeAttack");
        state = STATE.Hurt;
        stateTime_last = Time.time;
        eventBeAttack();
        //判断死亡
        if(Power < 1)
        {
            state = STATE.Dying;
            stateTime_last = Time.time;
            getTrigger.enabled = false;
            attackCollider.enabled = false;
            action = 0;
            animator.SetTrigger("Dead");
            eventDead();
        }
        isProtecting = true;  //死亡后也一直保持保护状态不会再度受伤
        //通知刷新power的UI，分数实时刷新即可

        return true;
    }

    //受到陷阱伤害，至少会保留1点能量
    public void BeHurted(int hurted)
    {
        Power -= hurted;
        eventBeHurted();
        //判断是否低于1
        if(Power < 1)
            Power = 1;
    }

    public void SpeedMax()
    {
        speed_up = SPEED_UP_MAX;
        eventSpeedMax();
    }

    public void IntoState_None()
    {
        if(usingSkill)
            BeHurted(150);  //技能结束后扣除150能量
        usingSkill = false;
        if(speed_up > SPEED_UP_NORMAL)
        {
            state = STATE.SlowDown;
            stateTime_last = Time.time;
            eventSpeedSlow();
        }
        else
        {
            state = STATE.None;
            isProtecting = false;
        }
    }

    public void SetProtected(bool value=true)
    {
        if(value)
            protectPopo.SetActive(true);
        portected = value;
    }

    public bool GetProtected()
    {
        return portected;
    }

    public void SetTriggerSize(float rate)
    {
        if(getTrigger != null)
        {
            getTrigger.radius = rate * getTriggerRadius;
            if(magnetLights != null)
            {
                foreach(ParticleSystem n in magnetLights)
                    n.Stop();
            }
        }
    }

    public void SetActionState(int right)
    {
        if(action == right)
            return;
        /*设置运动方向*/
        if(right < -1 || right > 1)
            return;
        action = right;
    }

    public void GetPower(int power)
    {
        if(state >= STATE.Dying)
            return;
        int lastPower = Power;
        Power += power;
        if(Power == POWER_MAX)
        {
            if(!usingSkill)  //如果当前正在技能的生效期间，则不再发布，避免无限技能
                eventPowerMax();
            if(lastPower == POWER_MAX)  //原本就满了
                return;
            if(state != STATE.SlowDown  && !IsSuper())
            {
                state = STATE.PowerUp;
                stateTime_last = Time.time;
            }
            isProtecting = true;
        }
    }

    public void OnSkill_Super(bool isPlayerSkill)
    {
        if(state >= STATE.Dying)
            return;
        usingSkill = isPlayerSkill;
        SpeedMax();
        god = true;
        state = STATE.None;
        stateTime_last = Time.time;
        MyAudio.instance.PlaySpeedUp();
    }

    public void OnSuperLight()
    {
        //无敌加速特效
        if(superLight_out != null)
            superLight_out.Play();
        if(superLights_in != null)
        {
            foreach(ParticleSystem n in superLights_in)
                n.Play();
        }
    }

    public void OnMagnetLight()
    {
        //磁铁特效
        if(magnetLights != null)
        {
            foreach(ParticleSystem n in magnetLights)
                n.Play();
        }
    }

    public bool IsSuper()
    {
        //判断是否处于无敌加速状态
        if(god == true && speed_up >= SPEED_UP_MAX)
            return true;
        else
            return false;
    }

    public void SetPowerY_Last()
    {
        lastPos_Y_setPower = gameObject.transform.position.y;
    }
}
