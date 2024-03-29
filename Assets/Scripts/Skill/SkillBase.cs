﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected int id = -1;
    protected float keepTime = 0.0f;
    Vector3 deltaPos = Vector3.zero;
    bool hasUsed = false;

    protected void Start()
    {
        Invoke("DestoryGameObject", 20.0f);  //用于清除没被捡到的
    }

    protected void Update()
    {
        if(hasUsed)
            return;
        deltaPos.y = 0.5f * Time.deltaTime;
        gameObject.transform.position += deltaPos;
    }

    protected void BeginSkill()
    {
        if(keepTime > 0.0f)
        {
            Game.instance.skillController.AddSkill(this);
            Invoke("OverSkill", keepTime);
        }
        else  //属于直接使用的技能，直接结束即可
        {
            OverSkill();
        }
    }

    protected void OverSkill()
    {
        if(id != -1 && keepTime > 0.0f)
        {
            Game.instance.skillController.RemoveSkill(id);
            DestoryGameObject();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.transform.position = Vector3.zero;
        MyAudio.instance.PlayGetSkill();
	}

    public void DestoryGameObject()
    {
        this.enabled = false;
        if(gameObject != null)
            Destroy(gameObject);
    }

    public int GetID()
    {
        return id;
    }
}
