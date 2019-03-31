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
        Game.instance.backgroundController = GameObject.Find("Backgrounds").GetComponent<BackgroundController>();
        GameObject canvas = GameObject.Find("Canvas").gameObject;
        Game.instance.screenRect = canvas.GetComponent<RectTransform>();
        Game.instance.monsterSetter = gameObject.GetComponent<MonsterSetter>();
        Game.instance.skillButton = canvas.transform.Find("Panel/InputController/PowerUp").GetComponent<Button_Skill>();
        Initilize();
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
        Game.instance.Score = 0;
        Game.instance.Level = LEVEL.Level1;
        Game.instance.speed_down_background = 6.0f;
        Game.instance.offset_setPower = 1.0f;
        Game.instance.offset_setMonster = 10.0f;
        Game.instance.offset_setSkill = 120.0f;
        Invoke("OnLevelUp", 32.0f);
    }

    public void InitilizeAll()
    {
        Initilize();
        Game.instance.player.SetActive(true);
        //调用其它的Controller和Setter的Initialize
        Game.instance.playerScript.Initialize();
        Game.instance.monsterSetter.Initialize();
        Game.instance.powerSetter.Initialize();
        Game.instance.backgroundController.Initialize();
        Game.instance.skillController.Initialize();
        Game.instance.skillButton.Initialize();
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
