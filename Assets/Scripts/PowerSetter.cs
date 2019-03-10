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

    float offset_setPower_min = 1.0f;
    int offset_setPower_minQuantum = 0;  //用于随机生成下一次的offset_setPower
    int offset_setPower_maxQuantum = 6;
    // Start is called before the first frame update
    void Start()
    {
        setPos_X = new float[9];
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
        
    }

    void OnSetPower()
    {
        float x = setPos_X[UnityEngine.Random.Range(0, setPos_X.Length)];
        Vector3 pos = new Vector3(x, offset_setPosY + Game.instance.player.transform.position.y, 0);
        if(pre_power != null)
        {
            GameObject.Instantiate(pre_power, pos, Quaternion.identity);
        }
        Game.instance.offset_setPower = (float)UnityEngine.Random.Range(offset_setPower_minQuantum, offset_setPower_maxQuantum) * 0.2f + offset_setPower_min;
    }
}
