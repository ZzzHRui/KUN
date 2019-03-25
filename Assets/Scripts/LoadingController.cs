using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*异步加载主场景 */
public class LoadingController : MonoBehaviour
{
    AsyncOperation op;
    int progress;  //加载进度
    public Text text;
    bool finished = false;
    float beginTime;

    // Start is called before the first frame update
    void Start()
    {
        beginTime = Time.time;
        StartCoroutine(LoadMainScene());
    }

    // Update is called once per frame
    void Update()
    {
        if(finished && Input.touchCount == 1)  //有触摸
        {
            op.allowSceneActivation = true;
        }
        if(finished)
            return;
        progress = (int)(op.progress * 100.0f);
        if(text != null)
            text.text = progress + "%";
    }

    IEnumerator LoadMainScene()
    {
        op = SceneManager.LoadSceneAsync("Main");
        yield return new WaitForEndOfFrame();
        op.allowSceneActivation = false;
        if(text != null)    
            text.text = "触摸屏幕以继续";
        finished = true;
    }
}
