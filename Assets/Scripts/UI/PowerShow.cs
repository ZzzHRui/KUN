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

    // Start is called before the first frame update
    void Start()
    {
        nomalColor = new Vector4(0, 0.788f, 1.0f, 1.0f);
        unNormalColor = new Vector4(0, 0.788f, 1.0f, 0.4f);

        powers = new ParticleSystem[4];
        GameObject temp = gameObject.transform.Find("playerPower1/light").gameObject;
        powers[0] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower2/light").gameObject;
        powers[1] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower3/light").gameObject;
        powers[2] = temp.GetComponent<ParticleSystem>();
        temp = gameObject.transform.Find("playerPower4/light").gameObject;
        powers[3] = temp.GetComponent<ParticleSystem>();

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
        for(int i = 0; i < powers.Length; i++)
        {
            if(nowPower >= (i + 1) * perPower)
            {
                if(powers[i].isStopped)
                {
                    powers[i].Play();
                }
                circles[i].color = nomalColor;
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
    }
}
