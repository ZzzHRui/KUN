using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    int nowID = 0;
    Dictionary<int, SkillBase> skillDict;  //存放当前生效的技能，存放主要是避免技能实例被GC清除掉

    public int NowID { get => nowID++;}  //id用完一次自增
    

    // Start is called before the first frame update
    void Start()
    {
        skillDict = new Dictionary<int, SkillBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill(ref int id,SkillBase skill)
    {
        int tempId = NowID;
        skillDict.Add(tempId, skill);
        //将id记录到skill对象中
        id = tempId;
    }

    public void RemoveSkill(int id)
    {
        skillDict.Remove(id);
    }
}
