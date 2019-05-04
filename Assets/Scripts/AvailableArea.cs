using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*用于销毁离开有效区域的物体 */
public class AvailableArea : MonoBehaviour
{
    Collider2D col = null;
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
		if(other.gameObject.tag == "Power" || other.gameObject.tag == "Monster")
        {
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Skill")  //意味着没吃到本次的道具，降低下次生成道具的距离
        {
            Game.instance.offset_setSkill /= 2.0f;
        }
	}
}
