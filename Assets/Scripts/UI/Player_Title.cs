using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Title : MonoBehaviour
{
    bool right = true;
    float min = -4.7f;
    float max = 4.7f;
    float speed = 1.5f;
    float target;
    Vector3 deltaPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        target = max;
        gameObject.transform.position = new Vector3(-3.6f, 1.34f, -2.0f);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        deltaPos.x = speed * Time.deltaTime;
        if(right)
        {
            if(gameObject.transform.position.x >= target)
                ChangeDirection();
            gameObject.transform.position += deltaPos;
        }
        else
        {
            if(gameObject.transform.position.x <= target)
                ChangeDirection();
            gameObject.transform.position -= deltaPos;
        }

    }

    void ChangeDirection()
    {
        right = !right;
        gameObject.transform.Rotate(0.0f, 0.0f, 180.0f);
        Vector3 pos = gameObject.transform.position;
        if(right)
        {
            target = max;
            pos.y = 1.34f;
        }
        else
        {
            target = min;
            pos.y = 2.3f;
        }
        gameObject.transform.position = pos;
    }
}
