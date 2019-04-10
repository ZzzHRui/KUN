using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    protected float scale = 1.0f;
    protected int attack = 0;
    public float speed_down = 0.0f;
    protected float speed_down_min = -1.0f;
    bool hasCollide = false;
    protected Vector3 deltaPos;
    protected bool fly = false;
    float speed_X = 0.0f;

    protected void Start()
    {
        gameObject.transform.Rotate(0.0f, 0.0f, 180.0f);
        Invoke("DestroyGameObject", 20.0f);  //防止有怪物意外不清除
    }
    
    protected void FixedUpdate()
    {
        if(!fly)
        {
            deltaPos.Set(0.0f, -speed_down * Time.deltaTime, 0.0f);
        }
        else
        {
            if(speed_down < 0)
                speed_down += 20.0f * Time.deltaTime;
            gameObject.transform.Rotate(0.0f, 0.0f, 360.0f * Time.deltaTime);
            deltaPos.Set(speed_X * Time.deltaTime, -speed_down * Time.deltaTime, 0.0f);
        }
        gameObject.transform.position += deltaPos;
    }
    
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player" || hasCollide)
            return;
        if(Game.instance.playerScript.BeAttacked(attack))  //有效撞击才会出特效
        {
            //撞击特效
            Vector3 pos = (gameObject.transform.position + other.transform.position) / 2.0f;
            if(Game.instance.pre_beAttackLight != null)
            {
                GameObject.Instantiate(Game.instance.pre_beAttackLight, pos, Quaternion.identity);
            }
            MyAudio.instance.PlayBeAttack();
        }
        hasCollide = true;
        //如果player处于无敌加速状态，则怪物会进入被撞飞状态
        if(Game.instance.playerScript.IsSuper())
        // if(true)  //test
        {
            fly = true;
            speed_down = -20.0f;
            if(Game.instance.player.transform.position.x > gameObject.transform.position.x)
                speed_X = (float)(Random.Range(-5, 0));
            else
                speed_X = (float)(Random.Range(1, 6));
            MyAudio.instance.PlayBeAttack();
        }
    }

    protected void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
