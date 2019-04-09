using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLight : MonoBehaviour
{
    ParticleSystem inputLight = null;
    RectTransform inputRect;
    Vector3 viewPos = Vector3.zero;
    int maxX  = 1080;
    int maxY = 1920;
    
    // Start is called before the first frame update
    void Start()
    {
        inputLight = gameObject.GetComponent<ParticleSystem>();
        if(inputLight != null)
            inputLight.Stop();
        inputRect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //触屏测试
        if(Input.touchCount == 1)  //有触摸
        {
            if(inputLight.isStopped)
                inputLight.Play();
            viewPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);  //得到x，y为0-1，左下角0,0
            viewPos.z = 0.0f;
            viewPos.x = (viewPos.x - 0.5f) * (float)maxX;
            viewPos.y = (viewPos.y - 0.5f) * (float)maxY;
            inputRect.anchoredPosition3D = viewPos;
        }
        else
        {
            if(inputLight.isPlaying)
                inputLight.Stop();
        }
    }
}
