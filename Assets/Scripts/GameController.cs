using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void Handler();
    public event Handler eventLevelUp;  //提高难度
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
        Invoke("OnLevelUp", 30.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initilize()
    {
        
    }

    void OnLevelUp()
    {
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                Game.instance.Level = LEVEL.Level2;
                eventLevelUp();
                Invoke("OnLevelUp", 90.0f);
                break;

            case LEVEL.Level2:
                Game.instance.Level = LEVEL.Level3;
                eventLevelUp();
                Invoke("OnLevelUp", 120.0f);
                break;

            case LEVEL.Level3:
                Game.instance.Level = LEVEL.Level4;
                eventLevelUp();
                break;
        }
    }
}
