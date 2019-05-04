using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*用于生成power */
public class PowerSetter : MonoBehaviour
{
    float OFFSET_DISTANCE_NORMAL = 0.5f;
    float OFFSET_DISTANCE_SP = 1.0f;
    bool active = true; 

    GameObject pre_power = null;   //能量预设体
    int setPointNum = 8;  //设置生成点的数量
    float minX = -2.3f;  //生成点平均分布在此范围内
    float maxX = 2.3f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量
    float powerZ = 5.0f;

    float offset_setPower_distance = 0.5f;  //用于生成本波能量的间距
    float offset_setPower_last = 0.0f;   //用于记录上一次生成的距离

    float offset_setPower_min = 4.5f;
    int offset_setPower_minQuantum = 0;  //用于随机生成下一次的offset_setPower，int类型减少开销
    int offset_setPower_maxQuantum = 6;
    float offset_setPower_perQuantum = 1f;  //每个单位的间隔距离
    //特殊模式
    float sp_time_last = 0.0f;
    float sp_time_delay = 30.0f;
    int sp_time_delay_min = 5;
    int sp_time_delay_max = 10;
    float sp_time_delay_per = 5.0f;

    List<List<int>>.Enumerator setMap;  //每个元素记录list的位置

    enum MODE  //能量生成的模式
    {
        None = 0,
        Cube = 1,   //方块排列
        Row,        //成一行
        List,       //成一列
        Diagonal,   //成斜线
        End         //*非模式，仅用于随机生成时用
    }
    MODE nowMode = MODE.None;


    void Awake()
    {
        //预设
        pre_power = (GameObject)Resources.Load("Prefabs/Power");
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
            player.eventSetPower += OnSetPower;
        player.eventDead += OnPlayerDead;
        
        Initialize();
    }

    public void Initialize()
    {
        setMap.Dispose();
        // active = true;
        sp_time_last = Time.time;
        offset_setPower_distance = OFFSET_DISTANCE_NORMAL;
        offset_setPower_last = 0.0f;
        sp_time_delay = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
            return;
        if(nowMode != MODE.None)
        {
            float nowY = Game.instance.player.transform.position.y;
            if(nowY - offset_setPower_last >= offset_setPower_distance)
            {
                //根据setMap的每行依次生成
                List<int> internalList = setMap.Current;
                foreach(int x in internalList)
                {
                    if(pre_power)
                    {
                        Vector3 pos = new Vector3(setPos_X[x], offset_setPosY + nowY, powerZ);
                        //todo, 如果生成道具，则此处改成道具的预设
                        GameObject.Instantiate(pre_power, pos, Quaternion.identity);
                    }
                }
                if(!setMap.MoveNext())
                {
                    //生成下一次的间距
                    Game.instance.offset_setPower = (float)UnityEngine.Random.Range(offset_setPower_minQuantum, offset_setPower_maxQuantum) 
                                            * offset_setPower_perQuantum + offset_setPower_min;
                    Game.instance.playerScript.SetPowerY_Last();
                    if (Game.instance.Level >= LEVEL.Level5)
                        Game.instance.offset_setPower *= 0.5f;
                    else if (Game.instance.Level >= LEVEL.Level3)
                        Game.instance.offset_setPower *= 0.667f;
                    nowMode = MODE.None;
                    setMap.Dispose();
                    offset_setPower_distance = OFFSET_DISTANCE_NORMAL;
                    active = false;
                }
                offset_setPower_last = nowY;
            }
        }
    }

    void OnSetPower()
    {
        active = true;
        //生成模式
        bool is_sp_time = Time.time - sp_time_last >= sp_time_delay;
        if(is_sp_time)
            nowMode = MODE.Cube;
        else
            nowMode = (MODE)Random.Range(1, (int)MODE.End);
        // nowMode = MODE.Diagonal;
        List<List<int>> setMap = new List<List<int>>();  //临时用的setMap
        int w = 0;  //宽、高
        int h = 0;
        int beginX = 0;
        int[] index;
        
        switch (nowMode)
        {
            case MODE.Cube:
                //开启特殊模式，特殊模式也是Cube的特殊情况
                if(is_sp_time)
                {
                    offset_setPower_distance = OFFSET_DISTANCE_SP;
                    w = Random.Range(0, setPointNum) > setPointNum / 2 ? setPointNum : setPointNum / 2;
                    h = 20;
                    if(w == setPointNum)
                        beginX = 0;
                    else
                        beginX = Random.Range(0, 2) > 0 ? 0 : setPointNum / 2;
                    sp_time_delay = (float)Random.Range(sp_time_delay_min, sp_time_delay_max) * sp_time_delay_per;
                    sp_time_last = Time.time;
                }
                else
                {
                    //宽2，高3-6
                    w = 3;
                    h = Random.Range(3, 7);
                    beginX = Random.Range(0, setPointNum - w + 1);
                }
                index = new int[w];
                for(int i = 0; i < w; i++)
                    index[i] = beginX + i;
                for(int i = 0; i < h; i++)
                    AddSetMap(setMap, index);
                break;

            case MODE.Row:
                //一整排，高2-3
                w = setPointNum;
                h = Random.Range(2, 4);
                beginX = 0;
                index = new int[w];
                for(int i = 0; i < w; i++)
                    index[i] = beginX + i;
                for(int i = 0; i < h; i++)
                    AddSetMap(setMap, index);
                break;

            case MODE.List:
                //1-3列,长度3-6
                w = Random.Range(1, 4);
                h = Random.Range(3, 7);
                index = new int[w];
                index[0] = Random.Range(0, setPointNum);
                if(w > 1)
                    index[1] = (index[0] + setPointNum / 2) % setPointNum;
                if(w > 2)
                    index[2] = (index[0] + setPointNum / 4) % setPointNum;
                for(int i = 0; i < h; i++)
                    AddSetMap(setMap, index);
                break;

            case MODE.Diagonal:
                w = 2;
                beginX = Random.Range(0, 2) * setPointNum / 2;  //要么从0要么从中间开始
                index = new int[1];
                int left2right = Random.Range(0, 2) * 2 - 1;  //左到右上升, ±1
                if(beginX == 0)  //从0往右，或者从末尾往左
                    if(left2right == -1)
                        beginX = setPointNum - 1;
                for(int i = 0; i < setPointNum; i++)
                {
                    index[0] = (beginX + left2right * i + setPointNum) % setPointNum;
                    AddSetMap(setMap, index);
                }
                break;
        }
        this.setMap = setMap.GetEnumerator();
        if(!this.setMap.MoveNext())
        {
            this.setMap.Dispose();
            active = false;
        }
        Game.instance.offset_setPower = float.MaxValue;
        
    }

    void OnPlayerDead()
    {
        active = false;
        this.setMap.Dispose();
    }

    //给setMap增加一行，并设置生成的x
    void AddSetMap(List<List<int>> setMap, int[] x)
    {
        List<int> tempList = new List<int>();
        foreach(int i in x)
        {
            tempList.Add(i);
        }
        setMap.Add(tempList);
    }

    public GameObject GetPrePower()
    {
        return pre_power;
    }
}
