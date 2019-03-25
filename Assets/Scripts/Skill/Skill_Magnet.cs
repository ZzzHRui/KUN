using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Magnet : SkillBase
{
    new void Start()
    {
        base.Start();
        keepTime = 10.0f;
        id = 4;
    }

    new void BeginSkill()
    {
        Game.instance.playerScript.SetTriggerSize(3.0f);  //放大为三倍
        //特效相关设置

        base.BeginSkill();
    }

    new void OverSkill()
    {
        Game.instance.playerScript.SetTriggerSize(1.0f);
        base.OverSkill();
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
		if(other.gameObject.name == "GetTrigger")
        {
            this.BeginSkill();
            base.OnTriggerEnter2D(other);
        }
	}
}
