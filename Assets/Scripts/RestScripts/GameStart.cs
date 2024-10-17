using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private MediumSizeBall initScript;
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.ShowPanel<GameDurationPanel>();
        Debug.Log("Start Executed");
        GameObject initBall = GameObject.Find("MediumSizeBall");
        initScript = initBall.GetComponent<MediumSizeBall>();
        initScript.LockDivideOrNot(false);
        EventHub.Instance.EventTrigger<Transform>("SwitchControlled", initBall.transform);


    }

    
}
