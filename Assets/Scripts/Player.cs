using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //属性
    float speed_up = 8.0f;
    float maxSpeed_hor = 10.0f;
    float lastPos_Y_background = 0.0f;
    float lastPos_Y_setPower = 0.0f;
    Rigidbody2D rigid;
    float force = 1.8f;

    //事件
    public delegate void Handler();
    public event Handler eventUpdateBackground;  //需要更新背景的位置
    public event Handler eventSetPower;  //生成power

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        lastPos_Y_background = gameObject.transform.position.y;
        lastPos_Y_setPower = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //改变y值
        float y = this.speed_up * Time.deltaTime;
        Vector3 deltaPos =  new Vector3(0, y, 0);
        gameObject.transform.position += deltaPos;
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

        //更新背景检查
        float nowY = gameObject.transform.position.y;
        if(nowY - lastPos_Y_background >= Game.instance.offset_updateBackground)
        {
            eventUpdateBackground();
            lastPos_Y_background = nowY;
        }
        if(nowY - lastPos_Y_setPower >= Game.instance.offset_setPower)
        {
            eventSetPower();
            lastPos_Y_setPower = nowY;
        }
    }
}
