using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_LR : MonoBehaviour
{
    Vector3 viewPos = Vector3.zero;

    void Start()
    {
    }

    void Update()
    {
        //触屏
        if(Input.touchCount == 1)  //有触摸
        {
            viewPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if(viewPos.x >= 0.55f)
                Game.instance.playerScript.SetActionState(1);
            else if(viewPos.x <= 0.45f)
                Game.instance.playerScript.SetActionState(-1);
        }
        else
        {
            Game.instance.playerScript.SetActionState(0);
        }
    }
}
