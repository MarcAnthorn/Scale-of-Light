using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBaseAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //实行动态创建对象 动态挂载
                GameObject obj = new GameObject();  //创建挂载的场景对象；
                obj.name = typeof(T).Name;          //获取T脚本的类名，并将其作为对象的名称，方便在场景中明确识别这是挂载了什么脚本的对象；
                instance = obj.AddComponent<T>();   //挂载并且返回T类型的脚本组件，并被instance引用；
                DontDestroyOnLoad(obj);             //并且，指定过场景的时候不移除挂载的对象；维护单例模式的唯一性；
            }
  
            return instance;
        }
    }
}
