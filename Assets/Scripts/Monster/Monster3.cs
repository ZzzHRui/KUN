using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*小体积的竖直加速*/
public class Monster3 : MonsterBase
{
    float speed_add1 = 0.0f;
    float speed_add2 = 20.0f;
    float beginTime = float.MaxValue;
    float speedUpTime = 1.0f;
    // Start is called before the first frame update
    new void Start()
    {
        attack = Game.instance.attack[3];
        beginTime = Time.time;
        speed_down = -1.5f;
        if (Game.instance.Level >= LEVEL.Level5)
        {
            speedUpTime = 0.3f;
            speed_add2 += 2.0f;
        }
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if(fly)
            return;
        if(Time.time - beginTime < speedUpTime)
            speed_down += speed_add1 * Time.deltaTime;
        else
            speed_down += speed_add2 * Time.deltaTime;
    }
}
