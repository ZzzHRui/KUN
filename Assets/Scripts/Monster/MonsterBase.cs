using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    protected float scale = 1.0f;
    protected int attack = 0;
    protected float speed_down = 0.0f;
    protected float speed_down_min = -1.0f;
    bool hasCollide = false;

    protected void Start()
    {
        gameObject.transform.Rotate(0.0f, 0.0f, 180.0f);
    }

    protected void Update()
    {
        // gameObject.transform.Translate(0.0f, -speed_down * Time.deltaTime, 0.0f);
        gameObject.transform.position += new Vector3(0.0f, -speed_down * Time.deltaTime, 0.0f);
    }
    
    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player" || hasCollide)
            return;
        Game.instance.playerScript.BeAttacked(attack);
        var collider = gameObject.GetComponent<Collider2D>();
        hasCollide = true;
    }
}
