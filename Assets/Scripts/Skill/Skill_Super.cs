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

    // Update is called once per frame
    void Update()
    {
    }

    new void BeginSkill()
    {
        Game.instance.playerScript.OnSkill_Super(isPlayerSkill);
        //特效相关设置

        base.BeginSkill();
    }

    new void OverSkill()
    {
        Player player = Game.instance.playerScript;
        player.IntoState_None();
        if(isPlayerSkill)
            Game.instance.playerScript.BeHurted(150);  //技能结束后扣除150能量
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
