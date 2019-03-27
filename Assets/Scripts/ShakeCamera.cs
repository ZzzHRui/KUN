using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
	Vector3 shakePos = Vector3.zero;
	float shakeDelay = 0.2f;
	int shakeNum = 4;
	int nowNum = 1;
	float hasLerp = 0.0f;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// if(nowNum > shakeNum)
		// {
		// 	this.gameObject.SetActive(false);//解除此摄像机
		// }
		// float lerp = Time.deltaTime / shakeDelay;
		// hasLerp += lerp;
		// gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, shakePos, hasLerp);
		// if(hasLerp >= 1.0f)
		// {
		// 	GetNewPos();
		// 	nowNum++;
		// 	hasLerp = 0.0f;
		// }


		// if(Time.time-time >= shakingTime){
		// 	time=float.MaxValue;
		// 	this.gameObject.SetActive(false);//解除此摄像机
		// }
		// transform.localPosition -= shakePos;  //回到原点
        // shakePos = Random.insideUnitSphere / T;
        // transform.localPosition += shakePos;
	}

    public void BeginShake()
    {
		nowNum = 1;
		hasLerp = 0.0f;
		GetNewPos();
		gameObject.transform.localPosition = Vector3.zero;
    }

	void GetNewPos()
	{
		float x = nowNum / 2 == 0 ? 0.1f : -0.1f;
		if(nowNum == shakeNum)
			x = 0.0f;
		shakePos = new Vector3(x, 0.0f, 0.0f);
	}
}
