using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    // int nowID = 0;
    Dictionary<int, SkillBase> skillDict;  //存放当前生效的技能
    GameObject[] pre_skills;
    int setPointNum = 6;  //设置生成点的数量
    float minX = -2.6f;  //生成点平均分布在此范围内
    float maxX = 2.6f;
    float[] setPos_X;  //生成的位置
    float offset_setPosY = 10.0f;  //生成位置对于player的Y偏移量
    float skilZ = 0.0f;

    int offset_set_min = 10;
    int offset_set_max = 20;
    float offset_set_per = 8.0f;

    // public int NowID { get => nowID++;}  //id用完一次自增
    
    void Awake()
    {
        Object[] temp = Resources.LoadAll("Prefabs/Skill/");
        pre_skills = new GameObject[temp.Length];
        for(int i = 0; i < temp.Length; i++)
        {
            pre_skills[i] = (GameObject)temp[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.instance.playerScript.eventSetSkill += SetSkill;
        setPos_X = new float[setPointNum];
        float offset = (maxX - minX) / setPointNum;
        for(int i = 0; i < setPointNum; i++)
        {
            setPos_X[i] = (i + 0.5f) * offset + minX;
        }

        Initialize();

    }

    public void Initialize()
    {
        skillDict = new Dictionary<int, SkillBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSkill()
    {
        if(pre_skills == null)
            return;
        int skillIdx = Random.Range(0, pre_skills.Length);
        float playerY = Game.instance.player.transform.position.y;
        float x = setPos_X[Random.Range(0, setPos_X.Length)];
        GameObject.Instantiate(pre_skills[skillIdx], new Vector3(x, playerY + offset_setPosY, skilZ), Quaternion.identity);
        //生成下一次间距
        Game.instance.offset_setSkill = (float)Random.Range(offset_set_min, offset_set_max + 1) * offset_set_per;
    }

    public void AddSkill(SkillBase skill)
    {
        int tempId = skill.GetID();
        if(skillDict.ContainsKey(tempId))  //已经存在相同效果的技能，删除上一个，加入新的
        {
            skillDict[tempId].DestoryGameObject();
            skillDict[tempId] = skill;
        }
        else
        {
            skillDict.Add(tempId, skill);
        }
    }

    public void RemoveSkill(int id)
    {
        // skillDict[id].DestoryGameObject();
        skillDict.Remove(id);
    }
}
