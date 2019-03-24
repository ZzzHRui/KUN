using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    float[] scale = {0.3f, 0.4f, 0.5f};  //三种大小对应三种能量
    int power;
    int score;
    float dyingTime = 0.25f;
    float redyTime = 0.1f;
    float beginTime = float.MaxValue;

    enum MODE
    {
        None = 1,
        Begin,  //开始准备
        Dying,
        Keep,
        End
    }
    MODE nowMode = MODE.None;

    void Awake()
    {
        nowMode = MODE.None;
    }
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
    }

    // Update is called once per frame
    void Update()
    {
        switch(nowMode)
        {
            case MODE.Begin:
                if(Time.time - beginTime >= redyTime)
                {
                    SetToDying();
                    return;
                }
                break;

            case MODE.Dying:
                Vector3 pos = Vector3.Lerp(gameObject.transform.localPosition, Vector3.zero, Time.deltaTime / dyingTime + 0.05f);
                gameObject.transform.localPosition = pos;
                if(Time.time - beginTime >= dyingTime)
                {
                    beginTime = Time.time;
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
            nowMode = MODE.Begin;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
	}

    void OnGetReward()
    {
        Game.instance.Score += score;
        Game.instance.playerScript.Power += power;
    }

    public void SetToDying()
    {
        float playerY = Game.instance.player.gameObject.transform.position.y;
        Vector3 thisPos = gameObject.transform.position;
        if(playerY - thisPos.y >= 3.0f)  //提高视觉效果
            gameObject.transform.position = new Vector3(thisPos.x, playerY - 3.0f, thisPos.z);
        gameObject.transform.SetParent(Game.instance.player.gameObject.transform);
        nowMode = MODE.Dying;
        beginTime = Time.time;
    }
}
