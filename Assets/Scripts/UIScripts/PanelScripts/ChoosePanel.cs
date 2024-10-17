using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanel : BasePanel
{

    //[SerializeField]
    //private Button btnLevelOne;
    //[SerializeField]
    //private Button btnLevelTwo;
    //[SerializeField]
    //private Button btnLevelThree;
    //[SerializeField]
    //private Button btnLevelFour;
    //[SerializeField]
    //private Button btnLevelFive;

    //每一次显示ChoosePanel的时候，都会根据存储的JSON中的数据确定哪些关卡解锁；
    //实际上就是读取数据结构体中的存储数据
    private void OnEnable()
    {
        //在OnEnable中读取JSON的数据
    }


    protected override void Init()
    {
        //btnLevelOne.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.HidePanel<ChoosePanel>();
        //    LoadSceneManager.Instance.LoadSceneAsync("GameScene1");
        //});
        //btnLevelTwo.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.HidePanel<ChoosePanel>();
        //    LoadSceneManager.Instance.LoadSceneAsync("GameScene2");
        //});
        //btnLevelThree.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.HidePanel<ChoosePanel>();
        //    LoadSceneManager.Instance.LoadSceneAsync("GameScene3");
        //});
        //btnLevelFour.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.HidePanel<ChoosePanel>();
        //    LoadSceneManager.Instance.LoadSceneAsync("GameScene4");
        //});
        //btnLevelFive.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.HidePanel<ChoosePanel>();
        //    LoadSceneManager.Instance.LoadSceneAsync("GameScene5");
        //});


    }

   
   
  

    //关卡锁的Save应当在关卡通关的时候进行；

    //Load应当就是在ChoosePanel被激活的时候触发；
    //应当使用事件中心进行事件的发布，调用Load方法；
    public void LoadLocks()
    {

    }

}
