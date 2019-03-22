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
        Game.instance.playerScript = Game.instance.player.GetComponent<Player>();
        Game.instance.camera = Camera.main;
        Game.instance.backgrounds = GameObject.FindGameObjectsWithTag("Background");
        Game.instance.powerSetter = gameObject.GetComponent<PowerSetter>();
        Game.instance.skillController = gameObject.GetComponent<SkillController>();
        GameObject canvas = GameObject.Find("Canvas").gameObject;
        Game.instance.screenRect = canvas.GetComponent<RectTransform>();
        Game.instance.monsterSetter = gameObject.GetComponent<MonsterSetter>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initilize()
    {
        
    }
}
