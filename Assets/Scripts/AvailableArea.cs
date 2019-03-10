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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
		if(other.gameObject.tag == "Power" || other.gameObject.tag == "Monster")
        {
            Destroy(other.gameObject);
        }
	}
}
