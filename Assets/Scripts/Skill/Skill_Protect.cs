using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Protect : SkillBase
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void BeginSkill()
    {
        base.BeginSkill();
        Player player = Game.instance.playerScript;
        player.SetProtected();
        //特效相关设置
    }

    new void OverSkill()
    {
        DestoryGameObject();
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
