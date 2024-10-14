using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//由于既有可能是键盘信息作为检测输入的对象，也有可能是鼠标信息作为检测对象，因此使用数据结构类存储按键信息的容器，也是字典的value；
public class InputInfo
{
    //使用成员枚举来让外部进行选择具体到底是什么键位
    public enum E_EnterType
    {
        Keyboard,
        Mouse,
    }
    public E_EnterType enterType;

    public enum E_KeyType
    {
        Up,
        Down,
        Press,
        None,
    }
    public E_KeyType keyType;

    public KeyCode key; //存储按键类型；
    public KeyCode key2;    //双按键需求的第二key
    public int mouseId; //存储鼠标键类型；

    //实现两种构造函数重载；
    public InputInfo(InputInfo.E_EnterType _enterType, InputInfo.E_KeyType _keyType, KeyCode _key)
    {
        enterType = _enterType;
        keyType = _keyType;
        key = _key;
    }

    public InputInfo(InputInfo.E_EnterType _enterType, InputInfo.E_KeyType _keyType, int _mouseId)
    {
        enterType = _enterType;
        keyType = _keyType;
        mouseId = _mouseId;
    }

    public InputInfo(InputInfo.E_EnterType _enterType,InputInfo.E_KeyType _keyType, KeyCode _key1,KeyCode _key2)
    {
        enterType = _enterType;
        keyType = _keyType;
        key = _key1;
        key2 = _key2;
    }

}


public class InputManager : SingletonBaseManager<InputManager>
{
    private bool isDetect = true;
    private bool isToFetchKey = false;
    public void ToDetectInputOrNot(bool isDetect_)
    {
        isDetect = isDetect_;
    }

    //使用字典来存储事件名和键位的对应
    private Dictionary<string, InputInfo> inputDic = new Dictionary<string, InputInfo>();
    //存储双键位的字典和事件
    private  Dictionary<string,InputInfo> inputDoubleDic = new Dictionary<string, InputInfo>();
    //eventName是链接事件和字典中存储的键位的桥梁；
    //触发事件和查找该事件的键位都是依靠它

    //特别注意：在外界不调用InputManager.Instance之前
    //InputManager的构造函数不会被调用；换言之，就不能实现事件的发布；
    private InputManager()
    {
        MonoManager.Instance.AddUpdateListener(InputUpdate);
    }


    private UnityAction<InputInfo> action;
    //提供给外部的方法：检测当前键盘输入，并且传入委托来获取该键所在的数据结构类InputInfo;
    public void FetchKeyNowDown(UnityAction<InputInfo> _action)
    {
        action = _action;
        MonoManager.Instance.StartCoroutine(WaitForOneFrame()); //利用协程，等一帧再开启检测；
    }


    private IEnumerator WaitForOneFrame()
    {
        yield return 0; //yield return后填什么都会最起码等上一帧；
        isToFetchKey = true;
    }
    private void InputUpdate()
    {
        //开启获取当前输入键位检测再检测
        if (isToFetchKey)
        {
            if (Input.anyKeyDown)    //检测到有键位被按下
            {
                //通过遍历的方式，实现当前键位的读取
                //拓展：Enum类中的GetValues方法：
                //该方法是一个封装在Enum类中的方法，参数类型是Type，要求传入目标枚举类的type；返回值是数组，存储了该枚举中的所有成员；

                InputInfo info = null;
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))   //遍历判断到底是谁被按下了；
                    {
                        info = new InputInfo(InputInfo.E_EnterType.Keyboard, InputInfo.E_KeyType.Down, key);
                        break;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        info = new InputInfo(InputInfo.E_EnterType.Mouse, InputInfo.E_KeyType.Down, i);
                        break;
                    }
                }
                //定点调用通过FetchKeyNowDown方法传入的委托
                action?.Invoke(info);
                //获取完毕，关闭获取遍历；
                isToFetchKey = false;
            }
        }

        if (!isDetect)
            return;

        InputInfo temp;
        foreach (string eventName in inputDic.Keys)
        {
            temp = inputDic[eventName];
            if (temp.enterType == InputInfo.E_EnterType.Mouse)
            {
                switch (temp.keyType)
                {
                    case InputInfo.E_KeyType.Up:
                        if (Input.GetMouseButtonUp(temp.mouseId))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.Down:
                        if (Input.GetMouseButtonDown(temp.mouseId))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.Press:
                        if (Input.GetMouseButton(temp.mouseId))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.None:
                        if(!Input.anyKey)
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                }

            }

            else if (temp.enterType == InputInfo.E_EnterType.Keyboard)
            {
                switch (temp.keyType)
                {
                    case InputInfo.E_KeyType.Up:
                        if (Input.GetKeyUp(temp.key))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.Down:
                        if (Input.GetKeyDown(temp.key))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.Press:
                        if (Input.GetKey(temp.key))
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                    case InputInfo.E_KeyType.None:
                        if (!Input.anyKey)
                            EventHub.Instance.EventTrigger(eventName);
                        break;
                }
            }
        }

        InputInfo temp2;
        foreach (string eventName in inputDoubleDic.Keys)
        {
            temp2 = inputDoubleDic[eventName];
            switch (temp2.keyType)
            {
                case InputInfo.E_KeyType.Up:
                    if (Input.GetKey(temp2.key) && Input.GetKeyUp(temp2.key2))
                        EventHub.Instance.EventTrigger(eventName);
                    break;
                case InputInfo.E_KeyType.Down:
                    if (Input.GetKey(temp2.key) && Input.GetKeyDown(temp2.key2))
                        EventHub.Instance.EventTrigger(eventName);
                    break;
                case InputInfo.E_KeyType.Press:
                    if (Input.GetKey(temp2.key) &&  Input.GetKey(temp2.key2))
                        EventHub.Instance.EventTrigger(eventName);
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            EventHub.Instance.EventTrigger("GetAxisHorizontal", Input.GetAxisRaw("Horizontal"));
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            EventHub.Instance.EventTrigger("GetAxisVertical", Input.GetAxisRaw("Vertical"));


    }


    //提供给外部的改键方法：改为键盘检测
    public void SwitchToBoardKey(string eventName, InputInfo.E_KeyType type, KeyCode code)
    {
        if (!inputDic.ContainsKey(eventName))
            inputDic.Add(eventName, new InputInfo(InputInfo.E_EnterType.Keyboard, type, code));
        else
        {
            //强制将类型置为KeyBoard
            inputDic[eventName].enterType = InputInfo.E_EnterType.Keyboard;
            inputDic[eventName].keyType = type;
            inputDic[eventName].key = code;
        }
    }

    //重载一个双键位事件的检测方法;默认第一个键位传入的是Press类型的KeyType
    public void SwitchToBoardKey(string eventName, InputInfo.E_KeyType type2,KeyCode code1,KeyCode code2)
    {
        if (!inputDoubleDic.ContainsKey(eventName))
            inputDoubleDic.Add(eventName, new InputInfo(InputInfo.E_EnterType.Keyboard, type2, code1, code2));
        else
        {
            //强制将类型置为KeyBoard
            inputDoubleDic[eventName].enterType = InputInfo.E_EnterType.Keyboard;
            inputDoubleDic[eventName].keyType = type2;
            inputDoubleDic[eventName].key = code1;
            inputDoubleDic[eventName].key2 = code2;

        }

    }

    //改为鼠标检测
    public void SwitchToMouse(string eventName, InputInfo.E_KeyType type, int mouseId)
    {
        if (!inputDic.ContainsKey(eventName))
            inputDic.Add(eventName, new InputInfo(InputInfo.E_EnterType.Mouse, type, mouseId));
        else
        {
            inputDic[eventName].enterType = InputInfo.E_EnterType.Mouse;
            inputDic[eventName].keyType = type;
            inputDic[eventName].mouseId = mouseId;
        }
    }

  

    //移除监听
    public void RemoveEventInfo(string eventName)
    {
        if (inputDic.ContainsKey(eventName))
            inputDic.Remove(eventName);
    }

    public void RemoveEventInfoInDouble(string eventName)
    {
        if (inputDoubleDic.ContainsKey(eventName))
            inputDoubleDic.Remove(eventName);
    }


}

