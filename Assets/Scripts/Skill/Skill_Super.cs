using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*无敌加速状态 */
public class Skill_Super : SkillBase
{
    new void Start()
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
        Player player = Game.instance.playerScript;
        player.SpeedMax();
        player.GodIn();
        //特效相关设置

        base.BeginSkill();
    }

    new void OverSkill()
    {
        Player player = Game.instance.playerScript;
        player.IntoState_None();
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
