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
    //player
    public float speed_up_player = 8.0f;
    //难度
    public LEVEL Level = LEVEL.Level1;
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
    public int[] attack = {10, 10, 30, 10};  //每种怪物的攻击力，攻击力分别是0123的
    public int[][] monsterLevel = {  //每组（每个难度）的每个元素为对应种类怪物出现的概率
        new int[]{4, 4, 5, 5},  //总长度为最后一个元素的值，i种类占用的长度为 int[i] - int[i - 1]，本行种类0:2 = 4:1
        new int[]{5, 8, 10, 10},
        new int[]{5, 10, 13, 18},
        new int[]{5, 10, 13, 18}
    };
    public float offset_setMonster = 3.0f;

}

