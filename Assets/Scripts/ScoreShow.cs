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
        temp += Game.instance.playerScript.Power;
        text.text = Game.instance.Score.ToString() + temp;
    }
}
