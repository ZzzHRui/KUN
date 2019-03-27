using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*小体积的竖直加速*/
public class Monster3 : MonsterBase
{
    float speed_add1 = 1.0f;
    float speed_add2 = 30.0f;
    float beginTime = float.MaxValue;
    float speedUpTime = 0.4f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        attack = Game.instance.attack[3];
        beginTime = Time.time;
        speed_down = -1.0f;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if(Time.time - beginTime < speedUpTime)
            speed_down += speed_add1 * Time.deltaTime;
        else
            speed_down += speed_add2 * Time.deltaTime;
    }
}
