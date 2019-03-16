using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*用于生成power */
public class PowerSetter : MonoBehaviour
{
    public GameObject pre_power = null;   //能量预设体
    public int setPointNum = 9;  //设置生成点的数量
    public float minX = -3.0f;  //生成点平均分布在此范围内
    public float maxX = 3.0f;
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

    // Start is called before the first frame update
    void Start()
    {
        setPos_X = new float[setPointNum];
        float offset = (maxX - minX) / setPointNum;
        for(int i = 0; i < setPointNum; i++)
        {
            setPos_X[i] = i * offset + minX;
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
                //根据setMap的一行生成
                List<int> internalList = setMap.Current;
                foreach(int x in internalList)
                {
                    if(pre_power)
                    {
                        Vector3 pos = new Vector3(setPos_X[x], offset_setPosY + nowY, 0.0f);
                        GameObject.Instantiate(pre_power, pos, Quaternion.identity);
                    }
                }
                if(!setMap.MoveNext())
                {
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
        // nowMode = Random.Range(1, (int)MODE.End);
        nowMode = MODE.Cube;
        List<List<int>> setMap = new List<List<int>>();  //临时用的setMap
        switch (nowMode)
        {
            case MODE.Cube:
                int w = 2;  //宽、高
                int h = Random.Range(2, 6);
                int beginX = Random.Range(0, setPointNum - w + 1);
                int[] index = new int[w];
                for(int i = 0; i < w; i++)
                {
                    index[i] = beginX + i;
                }
                for(int i = 0; i < h; i++)
                {
                    AddSetMap(setMap, index);
                }
                break;
            case MODE.Row:
                break;
            case MODE.List:
                break;
            case MODE.Diagonal:
                break;
        }
        this.setMap = setMap.GetEnumerator();
        if(!this.setMap.MoveNext())
            this.setMap.Dispose();
        //生成下一次的间距
        Game.instance.offset_setPower = (float)UnityEngine.Random.Range(offset_setPower_minQuantum, offset_setPower_maxQuantum) 
                                            * offset_setPower_perQuantum + offset_setPower_min;
        
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
