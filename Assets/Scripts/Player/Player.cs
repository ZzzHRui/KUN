using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float SPEED_UP_NORMAL = 8.0f;
    float SPEED_UP_MAX = 16.0f;
    int POWER_MAX = 200;
    //属性
    float speed_up = 8.0f;
    float maxSpeed_hor = 10.0f;
    float lastPos_Y_setPower = 0.0f;
    float lastPos_Y_setMonster = 0.0f;
    Rigidbody2D rigid;
    float force = 1.8f;
    int power = 0;

    bool portected = false;  //无敌不会受伤
    bool powerUp = false;  //满能量，不会随时间减少能量
    STATE state = STATE.None;

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
        Initialize();
    }

    void Initialize()
    {
        lastPos_Y_setPower = gameObject.transform.position.y;
        lastPos_Y_setMonster = lastPos_Y_setPower;
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
        if(portected)
            return;
        Power -= attack;
        //判断死亡
    }

    //受到陷阱伤害，至少会保留1点能量
    public void BeHurted(int hurted)
    {
        if(portected)
            return;
        Power -= hurted;
        //判断是否低于1
    }

    //能量满了
    void OnPowerUp()
    {

    }
}
