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

    //服务器相关
    public string HOST = "129.204.150.195";
    public int PORT = 2971;

    public GameController gameController = null;
    public BackgroundController backgroundController = null;
    public ForegroundController foregroundController = null;
    public PowerSetter powerSetter = null;
    public SkillController skillController = null;
    public MonsterSetter monsterSetter = null;
    public GameObject player = null;
    public Player playerScript = null;
    public Camera camera = null;
    public GameObject[] backgrounds = null;
    public GameObject[] foregrounds = null;
    public RectTransform screenRect;
    public GameObject pre_beAttackLight = null;  //攻击光效

    private int score;  //分数
    public int Score { get => score; set => score = value; }
    public float beginTime;
    //player
    public float speed_up_player = 5.0f;
    //难度
    public LEVEL Level = LEVEL.Level1;
    //背景前景
    public float offset_updateBackground = 12.8f;  //更新背景前景的距离
    public float speed_down_background;
    public float speed_down_foreground;

    //摄像机
    public float offset_camera = 3.0f;  //摄像机和player的偏移距离

    //能量
    public float offset_setPower;  //生成power的player的移动距离
    public int[] power = {1, 2, 4};  //能量值
    public int[] baseScore = {1, 2, 3};  //能量的基础分值，分值为 baseScore * multiScore * level
    public int multiScore = 7;
    public int maxPower = 200;

    //怪物
    public int[] attack = {50, 50, 100, 50};  //每种怪物的攻击力，攻击力分别是0123的
    public int[][] monsterLevel = {  //每组（每个难度）的每个元素为对应种类怪物出现的概率
        new int[]{4, 4, 5, 5},  //总长度为最后一个元素的值，i种类占用的长度为 int[i] - int[i - 1]，本行种类0:2 = 4:1
        new int[]{5, 8, 10, 10},
        new int[]{5, 10, 13, 18},
        new int[]{5, 10, 13, 18}
    };
    public float offset_setMonster;

    //道具
    public float offset_setSkill;
    public float time_speed_slowdown = 2.0f;
    public float speed_max_rate = 2.0f;  //加速时相对于正常速度的比率

    //按钮
    //位置
    public Vector2[] leftBtnPos = {
        new Vector2(150f, -850f),   //右边
        new Vector2(-400f, -850f),  //左边
        new Vector2(-350f, -850f)   //分两侧
    };
    public Vector2[] rightBtnPos = {
        new Vector2(400f, -850f),  //右边
        new Vector2(-150f, -850f),  //左边
        new Vector2(350f, -850f)   //分两侧
    };
    public Vector2[] powerUpBtnPos = {
        new Vector2(-180f, -850f),  //右边
        new Vector2(180f, -850f),  //左边
        new Vector2(0f, -850f)   //分两侧
    };
    public Button_Skill skillButton;
}

