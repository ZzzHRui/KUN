using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    float[] scale = {0.3f, 0.4f, 0.5f};  //三种大小对应三种能量
    int power;
    int score;
    float dyingTime = 0.2f;
    float beginTime = float.MaxValue;

    enum MODE
    {
        None = 1,
        Dying,
        End
    }
    MODE nowMode = MODE.None;
    // Start is called before the first frame update
    void Start()
    {
        int r = UnityEngine.Random.Range(0, 21);
        if(r <= 14)
        {
            gameObject.transform.localScale = new Vector3(scale[0], scale[0], scale[0]);
            power = Game.instance.power[0];
            score = Game.instance.baseScore[0] * (int)Game.instance.Level * Game.instance.multiScore;
        }
        else if(r <= 18)
        {
            gameObject.transform.localScale = new Vector3(scale[1], scale[1], scale[1]);
            power = Game.instance.power[1];
            score = Game.instance.baseScore[1] * (int)Game.instance.Level * Game.instance.multiScore;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(scale[2], scale[2], scale[2]);
            power = Game.instance.power[2];
            score = Game.instance.baseScore[2] * (int)Game.instance.Level * Game.instance.multiScore;
        }
        nowMode = MODE.None;
    }

    // Update is called once per frame
    void Update()
    {
        switch(nowMode)
        {
            case MODE.Dying:
                Vector3 pos = Vector3.Lerp(gameObject.transform.position, Game.instance.player.transform.position, Time.deltaTime / dyingTime);
                gameObject.transform.position = pos;
                if(Time.time - beginTime >= dyingTime)
                {
                    nowMode = MODE.End;
                    return;
                }
                break;

            case MODE.End:
                //todo, 生成吸收的光效，并且放到player子物体下，设置localPosition在player的位置
                OnGetReward();
                Destroy(gameObject);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.gameObject.name == "GetTrigger")
        {
            beginTime = Time.time;
            nowMode = MODE.Dying;
        }
	}

    void OnGetReward()
    {
        Game.instance.Score += score;
        Game.instance.playerScript.Power += power;
    }
}
