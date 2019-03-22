using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LEVEL {
        Level1 = 1,
        Level2,
        Level3,
        Level4
}

/*全局变量*/
public class Game
{
    public static Game instance;  //单例

    public GameController gameController = null;
    public PowerSetter powerSetter = null;
    public SkillController skillController = null;
    public MonsterSetter monsterSetter = null;
    public GameObject player = null;
    public Player playerScript = null;
    public Camera camera = null;
    public GameObject[] backgrounds = null;
    public RectTransform screenRect;

    private int score = 0;  //分数
    public int Score { get => score; set => score = value; }
    // private int playerPower = 0;
    public LEVEL Level = LEVEL.Level1;  //难度
    //背景
    public float offset_updateBackground = 10.8f;  //更新背景的距离

    //摄像机
    public float offset_camera = 3.4f;  //摄像机和player的偏移距离

    //能量
    public float offset_setPower = 1.0f;  //生成power的player的移动距离
    public int[] power = {1, 2, 4};  //能量值
    public int[] baseScore = {1, 2, 3};  //能量的基础分值，分值为 baseScore * multiScore * level
    public int multiScore = 7;

    //怪物
    public float offset_setMonster = 3.0f;

}

