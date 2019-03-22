using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : MonsterBase
{
    // Start is called before the first frame update
    void Start()
    {
        attack = 10;
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                speed_down = Random.Range(0, 11) * 0.5f + speed_down_min;
                break;

            case LEVEL.Level2:
                speed_down = Random.Range(6, 16) * 0.5f + speed_down_min;
                break;

            case LEVEL.Level3:
                speed_down = Random.Range(10, 21) * 0.5f + speed_down_min;
                break;
            
            case LEVEL.Level4:
                speed_down = Random.Range(10, 21) * 0.5f + speed_down_min;
                break;
        }
    }

    new void Update()
    {
        base.Update();
    }
}
