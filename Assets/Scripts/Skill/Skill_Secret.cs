using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Secret : SkillBase
{
    public int nowNum = 0;
    int setPowerNum = 30;
    int setPowerNum_min = 5;
    int setPowerNum_max = 10;
    int setPowerNum_per = 5;
    int redussPower = 25;
    int goodRate = 6;
    ParticleSystem goodLight = null;
    ParticleSystem badLight = null;

    GameObject pre_power = null;
    new void Start()
    {
        base.Start();
        setPowerNum = Random.Range(setPowerNum_min, setPowerNum_max + 1) * setPowerNum_per;
        pre_power = Game.instance.powerSetter.GetPrePower();
        keepTime = 2.0f;
        id = 3;
        goodLight = gameObject.transform.Find("Secreat_good").GetComponent<ParticleSystem>();
        badLight = gameObject.transform.Find("Secreat_bad").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    new void BeginSkill()
    {
        //生成大量的能量
        int roll = Random.Range(0, 11);
        // InvokeRepeating("SetPower", 0.02f, 0.02f);  //test
        if(roll <= goodRate)
        {
            InvokeRepeating("SetPower", 0.02f, 0.02f);  //0.1秒后才出现能量
            //特效
            goodLight.Play();
        }
        else
        {
            Game.instance.playerScript.BeHurted(redussPower);
            //特效
            badLight.Play();
        }
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
		if(other.gameObject.name == "GetTrigger")
        {
            BeginSkill();
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
	}

    void SetPower()
    {
        if(nowNum >= setPowerNum)
            DestoryGameObject();
        if(pre_power)
        {
            float offsetX = Random.Range(-5, 6) * 0.1f;
            float offsetY = Random.Range(-5, 6) * 0.1f;
            Vector3 offset = new Vector3(offsetX, offsetY, 0.0f);
            GameObject power = GameObject.Instantiate(pre_power, gameObject.transform.position + offset, Quaternion.identity);
            power.GetComponent<Collider2D>().enabled = false;
            power.GetComponent<Power>().SetToDying();
        }
        nowNum++;
    }
}
