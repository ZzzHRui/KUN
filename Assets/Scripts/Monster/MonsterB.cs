using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB : MonsterBase
{
    float SPEED_HORIZONTAL = 1.0f;
    /*会追踪玩家的小体积低速度怪物 */
    float speed_horizontal = 0.0f;  //横向移动速度
    float speed_roll = 180.0f;  //转向角速度

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Rotate(0.0f, 0.0f, 180.0f);
        attack = 20;
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                // Destroy(gameObject);
                speed_down = speed_down_min - 1.0f;
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
        Vector3 playerPos = Game.instance.player.transform.position;
        Vector3 thisPos = gameObject.transform.position;
        // float angle = GetAngleY(thisPos, playerPos);
        float toRight = (playerPos.x - thisPos.x) > 0.0f ? 1.0f : -1.0f;
        //旋转到朝向player
        if(Mathf.Abs(playerPos.x - thisPos.x) > 0.3f)
        {
            //计算两点角度
            float angle = GetAngleY(thisPos, playerPos);
            
            //移动
            gameObject.transform.position += new Vector3(speed_horizontal * Time.deltaTime, 0.0f);
            //低过player不再追踪
            if(playerPos.y >= thisPos.y)
                return;
            //计算速度
            speed_horizontal = SPEED_HORIZONTAL * Mathf.Sin(angle);
            //旋转到angle + 180
            float nowRotate = gameObject.transform.rotation.eulerAngles.z;
            float targetRotate = (angle * 180.0f / Mathf.PI) + 180.0f;
            if(Mathf.Abs(nowRotate - targetRotate) > 5.0f)
            {
                gameObject.transform.Rotate(0.0f, 0.0f, toRight * speed_roll * Time.deltaTime);
            }
        }
    }

    float GetAngleY(Vector2 posA, Vector2 posB)
    {
        /*计算a和b的连线在x方向上的夹角(弧度)*/
        Vector3 delta = posB - posA;
        if(delta.x == 0)
            return 0.0f;
        float eulerAngle = Mathf.Atan2(delta.x, -delta.y);
        return eulerAngle;
    }
}
