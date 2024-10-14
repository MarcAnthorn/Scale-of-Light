using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private MaxSizeBall initScript;
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.ShowPanel<GameDurationPanel>();
        Debug.Log("Start Executed");
        GameObject initBall = GameObject.Find("MaxSizeBall");
        initScript = initBall.GetComponent<MaxSizeBall>();
        initScript.LockDivideOrNot(false);


    }

    
}
