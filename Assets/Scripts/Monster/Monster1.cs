using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*小体蛇皮走位怪物 */
public class Monster1 : MonsterBase
{
    float speed_horizontal = 6.0f;  //横向移动速度
    float left = -2.6f;
    float right = 2.6f;
    bool toRight;
    Vector3 deltaPos_X = Vector3.zero;

    // Start is called before the first frame update
    new void Start()
    {
        attack = Game.instance.attack[1];
        toRight = Random.Range(0, 2) == 1 ? true : false;
        SetDirection(toRight);
        speed_down = speed_down_min - 1.0f;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.x < left || gameObject.transform.position.x > right)
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
            gameObject.transform.Rotate(0.0f, 0.0f, 225.0f);
        else
            gameObject.transform.Rotate(0.0f, 0.0f, 135.0f);
    }
}
