using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*用于生成power */
public class PowerSetter : MonoBehaviour
{
    GameObject pre_power = null;   //能量预设体
    int setPointNum =8;  //设置生成点的数量
    float minX = -2.6f;  //生成点平均分布在此范围内
    float maxX = 2.6f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量

    float offset_setPower_distance = 0.5f;  //用于生成本波能量的间距
    float offset_setPower_last = 0.0f;   //用于记录上一次生成的距离

    float offset_setPower_min = 10.0f;
    int offset_setPower_minQuantum = 0;  //用于随机生成下一次的offset_setPower，int类型减少开销
    int offset_setPower_maxQuantum = 6;
    float offset_setPower_perQuantum = 1f;  //每个单位的间隔距离

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
    }

    // Update is called once per frame
    void Update()
    {
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
                        Vector3 pos = new Vector3(setPos_X[x], offset_setPosY + nowY, 0.0f);
                        //todo, 如果生成道具，则此处改成道具的预设
                        GameObject.Instantiate(pre_power, pos, Quaternion.identity);
                    }
                }
                if(!setMap.MoveNext())
                {
                    //生成下一次的间距
                    Game.instance.offset_setPower = (float)UnityEngine.Random.Range(offset_setPower_minQuantum, offset_setPower_maxQuantum) 
                                            * offset_setPower_perQuantum + offset_setPower_min;
                    nowMode = MODE.None;
                    setMap.Dispose();
                }
                offset_setPower_last = nowY;
            }
        }
    }

    void OnSetPower()
    {
        //生成模式
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
                //宽2，高2-5
                w = 2;
                h = Random.Range(2, 6);
                beginX = Random.Range(0, setPointNum - w + 1);
                index = new int[w];
                for(int i = 0; i < w; i++)
                    index[i] = beginX + i;
                for(int i = 0; i < h; i++)
                    AddSetMap(setMap, index);
                break;

            case MODE.Row:
                //一整排，高1-2
                w = setPointNum;
                h = Random.Range(1, 3);
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
            this.setMap.Dispose();
        Game.instance.offset_setPower = float.MaxValue;
        
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
}
