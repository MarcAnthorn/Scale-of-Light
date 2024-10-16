using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    //使用一个全局静态结构体，作为存储数据的容器；
    private static SaveData _saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public LevelsLockData LockData;
    }

    //实现一个方法：返回一个存储文件的路径，用于到时候从JSON中读取数据
    public static string FetchSaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    //处理存储的数据的方法，通过传入数据结构体的的方式拉取数据
    public static void HandleSaveData()
    {
        //从GameManager中调取数据；GameManager是挂载型的单例模式；Save应当在关卡通关的时候被调用从而实现数据的存储；
    }
    

    //因为没有继承Mono，使用静态方法来执行存储和加载数据
    public static void Save()
    {

    }
}
