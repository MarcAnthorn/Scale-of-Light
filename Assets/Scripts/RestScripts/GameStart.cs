using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStart : MonoBehaviour
{
    private MediumSizeBall initScript;
    // Start is called before the first frame update
    void Start()
    {       
        ////取消Button的默认选中，防止除了鼠标选择之外的键盘输入对Panel的行为造成影响；
        //EventSystem.current.SetSelectedGameObject(null);

        //UIManager.Instance.ShowPanel<GameDurationPanel>();
        Debug.Log("Start Executed");
        GameObject initBall = GameObject.Find("MediumSizeBall0");
        initScript = initBall.GetComponent<MediumSizeBall>();
        initScript.LockDivideOrNot(false);
        EventHub.Instance.EventTrigger<Transform>("SwitchControlled", initBall.transform);


    }

    
}
