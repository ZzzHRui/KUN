using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    /*保存的数据 */
    public List<SaveData> data = new List<SaveData>();
    
    public void Sort()
    {
        Sort s = new Sort();
        data.Sort(s);
    }

    public int Insert(int s, float t)
    {
        SaveData now;
        now.score = s;
        now.time = t;
        if(data.Count < 6)  //还没满的，直接插入然后排序
        {
            int i = 1;
            foreach(SaveData n in data)
            {
                if(now.score > n.score)
                {
                    break;
                }
                i++;
            }
            data.Add(now);
            if(data.Count >= 2)
            {
                Sort();
            }
            return i;
        }
        /*已经排好序，如果长度超过6则删除末尾，返回本次的名次 */
        for(int i = 0; i < data.Count - 1; i++)
        {
            if(now.score > data[i].score)
            {
                data.Insert(i, now);
                if(data.Count > 6)
                {
                    data.RemoveRange(6, data.Count - 6);  //删除之后所有
                }
                return i + 1;
            }
            //当相同分数则考虑用时，待写
        }
        return -1;  //没上榜
    }
}

[System.Serializable]
public struct SaveData
{
    public int score;
    public float time;
}
