using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*固定角度直线移动*/
public class Monster0 : MonsterBase
{
    float speed_horizontal = 0.0f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        attack = Game.instance.attack[0];
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                speed_down = Random.Range(0, 11) * 0.5f + speed_down_min;
                break;

            case LEVEL.Level2:
                speed_down = Random.Range(6, 16) * 0.5f + speed_down_min;
                break;

            case LEVEL.Level3:
                speed_down = Random.Range(8, 11) * 0.5f + speed_down_min;
                break;
            
            case LEVEL.Level4:
                speed_down = Random.Range(8, 13) * 0.5f + speed_down_min;
                break;
        }
        SetDirectionAndSpeed(Game.instance.monsterSetter.GetSetPoint_X());
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        gameObject.transform.position += new Vector3(speed_horizontal * Time.deltaTime, 0.0f, 0.0f);
    }

    public void SetDirectionAndSpeed(float[] setPos_X)
    {
        /*设置方向以及xy速度 */
        float targetX = setPos_X[Random.Range(0, setPos_X.Length)];
        float targetY = Game.instance.player.transform.position.y;
        // float toRight = (targetX - gameObject.transform.position.x) > 0.0f ? 1.0f : -1.0f;
        float angle = MyMath.GetAngleY(gameObject.transform.position, new Vector2(targetX, targetY));
        gameObject.transform.Rotate(0.0f, 0.0f, angle * 180.0f / Mathf.PI);
        speed_horizontal = speed_down * Mathf.Sin(angle);
        speed_down = speed_down * Mathf.Cos(angle);
    }
}
