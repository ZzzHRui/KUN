using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSetter : MonoBehaviour
{
    List<GameObject> pre_monsters;

    int setPointNum =8;  //设置生成点的数量
    float minX = -2.6f;  //生成点平均分布在此范围内
    float maxX = 2.6f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量
    int[] monsterLevel;  //当前难度对应的可能出现的怪物表

    // float offset_setMonster_min = 5.0f;
    // int offset_setMonster_minQuantum = 0;  //用于随机生成下一次的offset_setPower，int类型减少开销
    // int offset_setMonster_maxQuantum = 6;
    // float offset_setMonster_perQuantum = 1.0f;  //每个单位的间隔距离

    //特殊怪物群相关
    bool isSp = false;
    float sp_lastTime = float.MaxValue;
    float sp_offset = 1.0f;
    float sp_delay = 0.0f;
    int sp_delay_maxQuantum = 10;
    int sp_delay_minQuantum = 5;
    float sp_delay_perQuantum = 5f;  //每个单位的间隔距离
    int sp_monsterIdx = -1;
    List<List<int>>.Enumerator setMap;  //每个元素记录list的位置
    SP_MODE nowMode = SP_MODE.None;
    enum SP_MODE
    {
        None = 0,
        Mode1 = 1,  //竖直剑鱼群，怪物3
        Mode2,  //蛇皮鱼群，怪物1
        End
    }
    
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
        Game.instance.gameController.eventLevelUp += OnLevelUp;
        OnLevelUp();
        setPos_X = new float[setPointNum];
        float offset = (maxX - minX) / setPointNum;
        for(int i = 0; i < setPointNum; i++)
        {
            setPos_X[i] = (i + 0.5f) * offset + minX;
        }
        Player player = Game.instance.player.GetComponent<Player>();
        if(player != null)
            player.eventSetMonster += OnSetMonster;
        sp_lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSetMonster()
    {
        if(isSp)
        {
            SetSPMonster();
            return;
        }

        if(Game.instance.Level >= LEVEL.Level3 && Time.time - sp_lastTime >= sp_delay)
        {
            SetSPMode();
            return;
        }
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
        }
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

    void SetSPMonster()
    {
        float nowY = Game.instance.player.transform.position.y;
        List<int> internalList = setMap.Current;
        foreach(int x in internalList)
        {
            Vector3 pos = new Vector3(setPos_X[x], offset_setPosY + nowY, 0.0f);
            GameObject.Instantiate(pre_monsters[sp_monsterIdx], pos, Quaternion.identity);
        }
        if(!setMap.MoveNext())
        {
            setMap.Dispose();
            isSp = false;
            sp_lastTime = Time.time;
            sp_delay = (float)Random.Range(sp_delay_minQuantum, sp_delay_maxQuantum + 1) * sp_delay_perQuantum;
        }
    }

    void SetSPMode()
    {
        //开启特殊怪物群
        isSp = true;
        nowMode = (SP_MODE)Random.Range(1, (int)SP_MODE.End);
        List<List<int>> setMap = new List<List<int>>();  //临时用的setMap
        int w = 0;  //宽、高
        int h = 0;
        int beginX = 0;
        int[] index;
        switch(nowMode)
        {
            case SP_MODE.Mode1:
                Game.instance.offset_setMonster = Game.instance.speed_up_player / 1.0f;
                int secureNum = 2;  //保留2个空的作为出口
                sp_monsterIdx = 3;
                h = 10;
                w = setPointNum - secureNum;
                index = new int[w];
                int next = 2;
                beginX = Random.Range(secureNum, setPointNum + 1);
                for(int i = 0; i < h; i++)
                {
                    int now = beginX + next;
                    if(now >= setPointNum || now < 2)
                    {
                        next *= -1;
                        now = beginX + next;
                    }
                    beginX = now;
                    for(int j = 0; j < w; j++)
                    {
                        index[j] = (beginX + j) % setPointNum;
                    }
                    AddSetMap(setMap, index);
                }
                break;

            case SP_MODE.Mode2:
                Game.instance.offset_setMonster = Game.instance.speed_up_player / 1.5f;
                sp_monsterIdx = 1;
                h = 10;
                w = 2;
                index = new int[w];
                for(int i = 0; i < h; i++)
                {
                    beginX = Random.Range(0, setPointNum - 1);
                    for(int j = 0; j < w; j++)
                    {
                        index[j] =( beginX + j + 1) % setPointNum;
                    }
                    AddSetMap(setMap, index);
                }
                break;
        }
        this.setMap = setMap.GetEnumerator();
        if(!this.setMap.MoveNext())
            this.setMap.Dispose();
        else
            SetOffset_SetMonster();
    }

    void OnLevelUp()
    {
        SetOffset_SetMonster();
    }

    void SetOffset_SetMonster()
    {
        float rate;  //每秒产生的怪物波次，每拍0.5
        if(Game.instance.Level < LEVEL.Level3)
            rate = 1.0f;
        else
            rate = 1.3f;
        Game.instance.offset_setMonster = Game.instance.speed_up_player / rate;
    }

    void AddSetMap(List<List<int>> setMap, int[] x)
    {
        List<int> tempList = new List<int>();
        foreach(int i in x)
        {
            tempList.Add(i);
        }
        setMap.Add(tempList);
    }
}