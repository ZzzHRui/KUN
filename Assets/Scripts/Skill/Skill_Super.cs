﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*无敌加速状态 */
public class Skill_Super : SkillBase
{
    bool isPlayerSkill = false;

    new public void Start()
    {
        base.Start();
        keepTime = 10.0f;
        id = 1;
    }

    new void BeginSkill()
    {
        Game.instance.playerScript.IntoState_None();
        MyAudio.instance.PlaySpeedUp();
        MyAudio.instance.SetPitch(1.3f);
        Game.instance.playerScript.OnSkill_Super(isPlayerSkill);
        //特效相关设置
        Game.instance.playerScript.OnSuperLight();
        base.BeginSkill();
    }

    new void OverSkill()
    {
        Game.instance.playerScript.IntoState_None();
        MyAudio.instance.SetPitch(1.0f);
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

    public void ForceBegin(bool isPlayerSkill)
    {
        /*强制启动此技能 */
        this.isPlayerSkill = isPlayerSkill;
        Start();
        BeginSkill();
    }
}
