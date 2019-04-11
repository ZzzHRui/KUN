using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerShow : MonoBehaviour
{
    ParticleSystem[] powers = null;
    Image[] circles = null;
    int nowPower = 0;
    int perPower;  //每一格的能量值
    Vector4 nomalColor;
    Vector4 unNormalColor;
    RectTransform delLight_rect;
    ParticleSystem delLight_par;  //损血特效
    int lastPerPower = 0;  //之前的血格数0-3(第1-4格),-1代表0格
    int nowPerPower = 0;  //本轮的血格数0-3
    float[] X;  //记录血格的X位置

    // Start is called before the first frame update
    void Start()
    {
        nomalColor = new Vector4(0, 0.788f, 1.0f, 1.0f);
        unNormalColor = new Vector4(0, 0.788f, 1.0f, 0.4f);

        GameObject delLight_obj = gameObject.transform.Find("delPower").gameObject;
        delLight_par = delLight_obj.GetComponent<ParticleSystem>();
        delLight_rect = delLight_obj.GetComponent<RectTransform>();

        powers = new ParticleSystem[4];
        GameObject temp = gameObject.transform.Find("playerPower1/light").gameObject;
        powers[0] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower2/light").gameObject;
        powers[1] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower3/light").gameObject;
        powers[2] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower4/light").gameObject;
        powers[3] = temp.GetComponent<ParticleSystem>();

        X = new float[4];
        temp = gameObject.transform.Find("playerPower1").gameObject;
        X[0] = temp.GetComponent<RectTransform>().anchoredPosition3D.x;
        temp = gameObject.transform.Find("playerPower2").gameObject;
        X[1] = temp.GetComponent<RectTransform>().anchoredPosition3D.x;
        temp = gameObject.transform.Find("playerPower3").gameObject;
        X[2] = temp.GetComponent<RectTransform>().anchoredPosition3D.x;
        temp = gameObject.transform.Find("playerPower4").gameObject;
        X[3] = temp.GetComponent<RectTransform>().anchoredPosition3D.x;

        perPower = Game.instance.maxPower / powers.Length;

        circles = new Image[4];
        temp = gameObject.transform.Find("playerPower1").gameObject;
        circles[0] = temp.GetComponent<Image>();
        temp = gameObject.transform.Find("playerPower2").gameObject;
        circles[1] = temp.GetComponent<Image>();
        temp = gameObject.transform.Find("playerPower3").gameObject;
        circles[2] = temp.GetComponent<Image>();
        temp = gameObject.transform.Find("playerPower4").gameObject;
        circles[3] = temp.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        nowPower = Game.instance.playerScript.Power;
        nowPerPower = -1;
        for(int i = 0; i < powers.Length; i++)
        {
            if(nowPower >= (i + 1) * perPower)
            {
                if(powers[i].isStopped)
                {
                    powers[i].Play();
                }
                circles[i].color = nomalColor;
                nowPerPower++;
            }
            else
            {
                if(powers[i].isPlaying)
                {
                    powers[i].Stop();
                }
                circles[i].color = unNormalColor;
            }
        }
        if(nowPerPower < lastPerPower)
        {
            //发生了损血，播放特效
            Vector3 pos = Vector3.zero;
            pos.x = X[nowPerPower + 1];
            delLight_rect.anchoredPosition3D = pos;
            delLight_par.Play();
        }
        lastPerPower = nowPerPower;
    }
}
