using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*会追踪玩家的小体积低速度怪物 */
public class Monster1 : MonsterBase
{
    float SPEED_HORIZONTAL = 2.0f;

    float speed_horizontal = 6.0f;  //横向移动速度
    float speed_roll = 180.0f;  //转向角速度
    float left = -2.6f;
    float right = 2.6f;
    bool toRight;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        attack = Game.instance.attack[1];
        toRight = Random.Range(0, 2) == 1 ? true : false;
        SetDirection(toRight);
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                Destroy(gameObject);
                // speed_down = speed_down_min - 1.0f;
                break;

            case LEVEL.Level2:
                speed_down = speed_down_min - 1.0f;
                break;

            case LEVEL.Level3:
                speed_down = speed_down_min - 1.0f;
                break;
            
            case LEVEL.Level4:
                speed_down = speed_down_min - 1.0f;
                break;
        }
    }

    // Update is called once per frame
   new void Update()
    {
        base.Update();
        if(toRight)
            gameObject.transform.position += new Vector3(speed_horizontal * Time.deltaTime, 0.0f, 0.0f);
        else
            gameObject.transform.position -= new Vector3(speed_horizontal * Time.deltaTime, 0.0f, 0.0f);
        if(gameObject.transform.position.x < left || gameObject.transform.position.x > right)
        {    
            toRight = !toRight;
            SetDirection(toRight);
        }
    }

    void SetDirection(bool toRight)
    {
        gameObject.transform.rotation = Quaternion.identity;
        if(toRight)
            gameObject.transform.Rotate(0.0f, 0.0f, 225.0f);
        else
            gameObject.transform.Rotate(0.0f, 0.0f, 135.0f);
    }
}
