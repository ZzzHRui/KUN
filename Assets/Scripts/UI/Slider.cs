using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum VOLUMETYPE
{
    BGM = 0,
    Clip
}

public class Slider : MonoBehaviour, IDragHandler
{
    /*滑块控制 */
    public float min = -80.0f;
    public float max = 340.0f;
    RectTransform rect = null;
    Vector3 pos = Vector3.zero;
    public VOLUMETYPE type = VOLUMETYPE.BGM;

    void Awake()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pos.y = rect.anchoredPosition3D.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        pos.x = eventData.position.x - 540f;
        if(pos.x < min)
            pos.x = min;
        else if(pos.x > max)
            pos.x = max;
        rect.anchoredPosition3D = pos;
        if(type == VOLUMETYPE.BGM)
            MyAudio.instance.SetVolume(GetRate());
    }

    public float GetRate()
    {
        return (rect.anchoredPosition3D.x - min) / (max - min);
    }

    public void SetRate(float rate)
    {
        pos.x = min + rate * (max - min);
        pos.y = rect.anchoredPosition3D.y;
        rect.anchoredPosition3D = pos;
    }
}
