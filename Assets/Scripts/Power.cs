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
    int index = -1;

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
        if(index < 0 || index > scale.Length)
        {
            int r = UnityEngine.Random.Range(0, 21);
            if (r <= 14)
                index = 0;
            else if (r <= 18)
                index = 1;
            else
                index = 2;
        }
        gameObject.transform.localScale = new Vector3(scale[index], scale[index], scale[index]);
        power = Game.instance.power[index];
        score = Game.instance.baseScore[index] * (int)Game.instance.Level * Game.instance.multiScore;
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

            MyAudio.instance.PlayGetPower();
        }
	}

    void OnGetReward()
    {
        Game.instance.Score += score;
        Game.instance.playerScript.GetPower(power);
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

    public void SetIndex(int i)
    {
        index = i;
    }
}
