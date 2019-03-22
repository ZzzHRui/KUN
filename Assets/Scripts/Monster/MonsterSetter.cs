using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSetter : MonoBehaviour
{
    GameObject pre_monsterA = null;
    GameObject pre_monsterB = null;
    GameObject pre_monsterC = null;

    int setPointNum =8;  //设置生成点的数量
    float minX = -2.6f;  //生成点平均分布在此范围内
    float maxX = 2.6f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量

    float offset_setMonster_min = 5.0f;
    int offset_setMonster_minQuantum = 0;  //用于随机生成下一次的offset_setPower，int类型减少开销
    int offset_setMonster_maxQuantum = 6;
    float offset_setMonster_perQuantum = 1.0f;  //每个单位的间隔距离
    
    void Awake()
    {
        pre_monsterA = (GameObject)Resources.Load("Prefabs/MonsterA");
        pre_monsterB = (GameObject)Resources.Load("Prefabs/MonsterB");
        // pre_monsterC = (GameObject)Resources.Load("Prefabs/MonsterC");
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
        float nowY = Game.instance.player.transform.position.y;
        //根据难度生成
        switch(Game.instance.Level)
        {
            case LEVEL.Level1:
                index = new int[1];
                index[0] = Random.Range(0, setPointNum);
                // GameObject.Instantiate(pre_monsterA, 
                GameObject.Instantiate(pre_monsterB, 
                    new Vector3(setPos_X[index[0]], offset_setPosY + nowY, 0.0f),
                    Quaternion.identity);
                break;

            case LEVEL.Level2:
                break;

            case LEVEL.Level3:
                break;
            
            case LEVEL.Level4:
                break;
        }
        //生成下一次的间距,5-10米
        Game.instance.offset_setMonster = (float)UnityEngine.Random.Range(offset_setMonster_minQuantum, offset_setMonster_maxQuantum)
                                    * offset_setMonster_perQuantum + offset_setMonster_min;
    }
}