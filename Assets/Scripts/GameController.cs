using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    void Awake()
    {
        Game.instance = new Game();
        Game.instance.gameController = this;
        Game.instance.player = GameObject.FindGameObjectWithTag("Player");
        Game.instance.camera = Camera.main;
        Game.instance.backgrounds = GameObject.FindGameObjectsWithTag("Background");
        Game.instance.powerSetter = gameObject.GetComponent<PowerSetter>();
        GameObject canvas = GameObject.Find("Canvas").gameObject;
        Game.instance.screenRect = canvas.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
