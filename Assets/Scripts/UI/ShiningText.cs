using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiningText : MonoBehaviour
{
    Text text = null;
    float targetA;
    Vector4 nowColor = Vector4.one;
    bool up = false;
    float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        targetA = 0.1f;
        text.color = nowColor;
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
            if(nowColor.w >= targetA)
            {
                nowColor.w += -speed * Time.deltaTime;
            }
            else
            {
                up = true;
                targetA = 1.0f;
            }
        }
        text.color = nowColor;
    }
}
