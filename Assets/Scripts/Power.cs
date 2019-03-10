using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    float[] scale = {0.3f, 0.4f, 0.5f};  //三种大小对应三种能量
    int power;
    int score;
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
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.gameObject.name == "GetTrigger")
        {
            OnGetReward();
            Destroy(gameObject);
        }
	}

    void OnGetReward()
    {
        Game.instance.Score += score;
        Game.instance.PlayerPower += power;
    }
}
