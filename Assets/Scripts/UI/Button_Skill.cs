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
    List<ParticleSystem> lights = null;

    void Start()
    {
        lights = new List<ParticleSystem>();
        GameObject temp = gameObject.transform.Find("light1").gameObject;
        lights.Add(temp.GetComponent<ParticleSystem>());
        temp = gameObject.transform.Find("light2").gameObject;
        lights.Add(temp.GetComponent<ParticleSystem>());
        if(pre_skill_super == null)
            pre_skill_super = (GameObject)Resources.Load("Prefabs/Skill/Skill_Super");
        image = gameObject.GetComponent<Image>();
        Game.instance.playerScript.eventPowerMax += OnPowerMax;
        Game.instance.playerScript.eventBeAttack += OnPowerDown;
        Game.instance.playerScript.eventBeHurted += OnPowerDown;
        Initialize();
    }

    public void Initialize()
    {
        foreach(ParticleSystem n in lights)
        {
            if(n.isPlaying)
                n.Stop();
        }
        if(image != null)
            image.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skill = GameObject.Instantiate(pre_skill_super, Vector3.zero, Quaternion.identity);
        skill.GetComponent<Skill_Super>().ForceBegin(true);
        OnPowerDown();
    }

    void OnPowerMax()
    {
        image.enabled = true;
        //特效
        foreach(ParticleSystem n in lights)
        {
            if(n.isStopped)
                n.Play();
        }
    }

    void OnPowerDown()
    {
        image.enabled = false;
        foreach(ParticleSystem n in lights)
        {
            if(n.isPlaying)
                n.Stop();
        }
    }
}
