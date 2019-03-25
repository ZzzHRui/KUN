using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_LR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    int right = 1;

    void Start()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        if(gameObject.name == "Left")
        {
            right = -1;
            rect.anchoredPosition = Game.instance.leftBtnPos[2];  //中间
        }
        else
        {
            right = 1;
            rect.anchoredPosition = Game.instance.rightBtnPos[2];  //中间
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Game.instance.playerScript.SetActionState(right);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Game.instance.playerScript.SetActionState(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Game.instance.playerScript.SetActionState(0);
    }
}
