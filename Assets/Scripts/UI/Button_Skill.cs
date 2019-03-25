using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Skill : MonoBehaviour, IPointerDownHandler
{
    public GameObject pre_skill_super = null;
    GameObject skill = null;
    Image image = null;

    void Start()
    {
        if(pre_skill_super == null)
            pre_skill_super = (GameObject)Resources.Load("Prefabs/Skill/Skill_Super");
        image = gameObject.GetComponent<Image>();
        image.enabled = false;
        Game.instance.playerScript.eventPowerMax += OnPowerMax;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skill = GameObject.Instantiate(pre_skill_super, Vector3.zero, Quaternion.identity);
        skill.GetComponent<Skill_Super>().ForceBegin();
        image.enabled = false;
    }

    void OnPowerMax()
    {
        image.enabled = true;
        //特效
    }
}
