using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeAttackLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Del", 1.1f);
    }

    void Del()
    {
        Destroy(gameObject);
    }
}
