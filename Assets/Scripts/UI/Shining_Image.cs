using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shining_Image : MonoBehaviour
{
    public GameObject[] targetObj;
    Image[] images = null;
    float speed = 2.0f;
    bool up = false;
    float targetA;
    Vector4 nowColor = Vector4.one;
    public float timeToDestroy = 0;
    float beginTime;

    // Start is called before the first frame update
    void Start()
    {
        beginTime = Time.time;
        targetA = 0.1f;
        up = false;
        if(targetObj.Length != 0)
        {
            images = new Image[targetObj.Length];
            for(int i = 0; i < images.Length; i++)
            {
                images[i] = targetObj[i].GetComponent<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(up)
        {
            if(nowColor.w <= targetA)
            {
                nowColor.w += speed * Time.deltaTime;
            }
            else
            {
                up = false;
                targetA = 0.1f;
            }
        }
        else
        {
            if (nowColor.w >= targetA)
            {
                nowColor.w += -speed * Time.deltaTime;
            }
            else
            {
                up = true;
                targetA = 0.9f;
                if (timeToDestroy > 0)
                {
                    if (Time.time - beginTime > timeToDestroy)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        for(int i = 0; i < images.Length; i++)
        {
            images[i].color = nowColor;
        }
    }
}
