using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSetter : MonoBehaviour
{
    GameObject pre_monsterA = null;
    GameObject pre_monsterB = null;
    GameObject pre_monsterC = null;
    GameObject pre_monsterD = null;
    List<GameObject> pre_monsters;

    int setPointNum =8;  //设置生成点的数量
    float minX = -2.6f;  //生成点平均分布在此范围内
    float maxX = 2.6f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量
    int[] monsterLevel;  //当前难度对应的可能出现的怪物表

    float offset_setMonster_min = 5.0f;
    int offset_setMonster_minQuantum = 0;  //用于随机生成下一次的offset_setPower，int类型减少开销
    int offset_setMonster_maxQuantum = 6;
    float offset_setMonster_perQuantum = 1.0f;  //每个单位的间隔距离
    
    void Awake()
    {
        pre_monsters = new List<GameObject>();
        string path = "Prefabs/Monster";
        for(int i = 0; i < Game.instance.attack.Length; i++)
        {
            string tempPath = path + i.ToString();
            GameObject tempObj = (GameObject)Resources.Load(tempPath);
            pre_monsters.Add(tempObj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setPos_X = new float[setPointNum];
        float offset = (maxX - minX) / setPointNum;
        for(int i = 0; i < setPointNum; i++)
        {
            setPos_X[i] = (i + 0.5f) * offset + minX;
        }
        Player player = Game.instance.player.GetComponent<Player>();
        if(player != null)
            player.eventSetMonster += OnSetMonster;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSetMonster()
    {
        int[] index;
        int monsterIdx;
        int num = 1;  //生成的数量
        float nowY = Game.instance.player.transform.position.y;
        monsterLevel = Game.instance.monsterLevel[(int)Game.instance.Level - 1];
        //根据难度生成
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                num = 1;
                break;

            case LEVEL.Level2:
                num = Random.Range(1, 3); //生成一到两个
                break;

            case LEVEL.Level3:
                num = Random.Range(1, 3);
                break;
            
            case LEVEL.Level4:
                num = 2;
                break;
        }
        
        index = new int[num];
        index[0] = Random.Range(0, setPointNum);
            if(index.Length == 2)
        index[1] = (index[0] + Random.Range(1, setPointNum / 2 + 1)) % setPointNum;
        for(int i = 0; i < index.Length; i++)
        {
            monsterIdx = GetMonstIdx(Random.Range(0, monsterLevel[monsterLevel.Length - 1] + 1));
            GameObject nowObj = GameObject.Instantiate(pre_monsters[monsterIdx], 
                new Vector3(setPos_X[index[0]], offset_setPosY + nowY, 0.0f),
                Quaternion.identity);
            // if (monsterIdx == 0)
            // nowObj.GetComponent<Monster0>().SetDirectionAndSpeed(setPos_X);
        }

        //生成下一次的间距,5-10米
        // Game.instance.offset_setMonster = (float)UnityEngine.Random.Range(offset_setMonster_minQuantum, offset_setMonster_maxQuantum)
        //                             * offset_setMonster_perQuantum + offset_setMonster_min;
        //另一种方案，根据节奏
        float rate;  //每秒产生的怪物波次，每拍0.5
        if(Game.instance.Level < LEVEL.Level3)
            rate = 1.0f;
        else
            rate = 1.3f;
        Game.instance.offset_setMonster = Game.instance.speed_up_player / rate;
    }

    int GetMonstIdx(int num)
    {
        /*根据传入的数字得到对应的monsterIdx */
        int monsterIdx = 0;
        foreach(int n in monsterLevel)
        {
            if(num <= n)
                return monsterIdx;
            else
                monsterIdx++;
        }
        return 0;
    }

    public float[] GetSetPoint_X()
    {
        return (float[])setPos_X.Clone();
    }
}