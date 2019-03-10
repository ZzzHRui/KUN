using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreShow : MonoBehaviour
{
    Text text = null;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string temp = ",";
        if(Input.touchCount == 1){
            if(Input.GetTouch(0).phase == TouchPhase.Stationary){
                temp += Input.GetTouch(0).position.x.ToString();
            }
        }
        text.text = Game.instance.Score.ToString() + temp;
    }
}
