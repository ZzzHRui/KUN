using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreShow : MonoBehaviour
{
    Text text = null;
    int lastScore = 0;
    int nowScore;
    float nowSize = 100;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        nowScore = Game.instance.Score;
        if(nowScore != lastScore)  //分数发生变化
        {
            text.text = nowScore.ToString();
            lastScore = nowScore;
            nowSize = 110f;
        }
        if(nowSize > 100)
        {
            nowSize -= 100f * Time.deltaTime;
            text.fontSize = (int)nowSize;
        }
    }
}
