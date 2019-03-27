using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*较大体积的缓慢直线移动*/
public class Monster2 : MonsterBase
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        attack = Game.instance.attack[2];
        speed_down = speed_down_min;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
