﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*小体蛇皮走位怪物 */
public class Monster1 : MonsterBase
{
    float speed_horizontal = 4.0f;  //横向移动速度
    float left = -2.6f;
    float right = 2.6f;
    float targetX;
    bool toRight;
    Vector3 deltaPos_X = Vector3.zero;

    // Start is called before the first frame update
    new void Start()
    {
        attack = Game.instance.attack[1];
        toRight = Random.Range(0, 2) == 1 ? true : false;
        SetDirection(toRight);
        speed_down = speed_down_min - 1.0f;
        Invoke("DestroyGameObject", 20.0f);  //防止怪物意外不清除
    }

    // Update is called once per frame
    void Update()
    {
        if(toRight ^ gameObject.transform.position.x <= targetX)
        {    
            toRight = !toRight;
            SetDirection(toRight);
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if(fly)
            return;
        deltaPos_X.Set(speed_horizontal * Time.deltaTime, 0.0f, 0.0f);
        if(toRight)
            gameObject.transform.position += deltaPos_X;
        else
            gameObject.transform.position -= deltaPos_X;
    }

    void SetDirection(bool toRight)
    {
        gameObject.transform.rotation = Quaternion.identity;
        if(toRight)
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 225.0f);
            targetX = right;
        }    
        else
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 135.0f);
            targetX = left;
        }
    }
}
