using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    /*保存的数据 */
    public List<SaveData> data = new List<SaveData>();
    public string username = "";  //本用户名
    public int maxScore = 0;
    
    public void Sort()
    {
        Sort s = new Sort();
        data.Sort(s);
    }

    public void SetList(string list)
    {
        data.Clear();
        string[] allList = list.Split(" ".ToCharArray()[0]);
        SaveData tempData;
        for(int i = 0; i < allList.Length; i += 2)
        {
            if(allList[i].Contains("\0"))
                break;
            tempData.username = allList[i];
            tempData.score = (int)float.Parse(allList[i + 1]);
            data.Add(tempData);
        }
        
    }
}

[System.Serializable]
public struct SaveData
{
    public string username;  //用户名
    public int score;  //分数
}
