using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//计时器对象数据结构类
public class TimeItem
{
    public bool isRunning = true;          //布尔类型，判断该数据类是否还在参与协程运作；

    public int indexOfItem;          //数据结构类的编号，方便访问

    public float intervalTime;      //单位：毫秒
    public float setIntervalTime;   //单位：毫秒

    public float totalTime;         //单位：毫秒
    public float setTotalTime;

    public UnityAction intervalCallback;    //间隔时间归零，间隔一定时间执行的回调；
    public UnityAction endCallback;         //总计时归零，计时结束执行的回调；

    public TimeItem(float total_, UnityAction endCallback_,int index, float interval_ = 0, UnityAction intervalCallback_ = null)
    {
        setIntervalTime = intervalTime = interval_;
        setTotalTime = totalTime = total_;
        intervalCallback = intervalCallback_;
        endCallback = endCallback_;
        indexOfItem = index;

    }

    public void ResetIntervalTime()
    {
        intervalTime = setIntervalTime;
    }

    public void ResetTotalTime()
    {
        totalTime = setTotalTime;
    }

}



//计时器管理类
public class TimeManager : SingletonBaseManager<TimeManager>
{
    //计时器index记录；
    public int indexsAll;
    public int indexsAllFree;
    private TimeManager()
    {
        //默认起始的计时器index：从0开始
        indexsAll = 0;
        indexsAllFree = 0;
        StartCoroutine();       //执行受到scaleTime影响的协程；
        StartCoroutine(true);   //执行不受到scaleTime影响的协程；
    }

    //声明一个字典来管理所有的计时器对象
    private Dictionary<int, TimeItem> timeDic = new Dictionary<int, TimeItem>();
    private Dictionary<int, TimeItem> timeFreeDic = new Dictionary<int, TimeItem>();

    private List<TimeItem> listToDelete = new List<TimeItem>();
    private List<TimeItem> listFreeToDelete = new List<TimeItem>();

    private const float interval = 0.1f;   //设置协程的执行间隔时间为0.1s，即100ms；
    private Coroutine coroutine;
    private Coroutine coroutineFree;


    public void StartCoroutine(bool isFreeFromScaleTime = false)
    {
        if (isFreeFromScaleTime)
            coroutineFree = MonoManager.Instance.StartCoroutine(FreeCoroutineContent());
        else
            coroutine = MonoManager.Instance.StartCoroutine(CoroutineContent());
    }

    public void StopCoroutine(bool isFreeFromScaleTime = false)
    {
        if (isFreeFromScaleTime)
            MonoManager.Instance.StopCoroutine(coroutineFree);
        else
            MonoManager.Instance.StopCoroutine(coroutine);
    }

    //为了节约性能，我们可以不要让协程每次执行都new一个WaitForSeconds；
    WaitForSeconds waitForSeconds = new WaitForSeconds(interval);
    WaitForSecondsRealtime waitForRealSeconds = new WaitForSecondsRealtime(interval);

    //协程：执行计时；定时执行TimeItem中存储的定时回调；并且会在最终延时时间到的时候执行最终回调，完成延时操作；
    //检测间隔：每0.1s执行一次检测：是否有item需要执行内部回调；
    private IEnumerator CoroutineContent()
    {
        while (true)
        {
            yield return waitForSeconds;
            //遍历所有仍在运作的TimeItem类
            foreach (TimeItem item in timeDic.Values)
            {
                if (!item.isRunning)
                    continue;   //该事件类不参与运作了，那就跳过，直接访问下一个

                if (item.intervalTime != 0)  //可能会有不希望执行间隔委托回调的定时器；那就不要执行；
                {
                    item.intervalTime -= (int)(interval * 1000);      //interval转为毫秒，并且切换为int类型；
                    if (item.intervalTime <= 0)  //说明间隔时间减完了，到时间了，要执行间隔的委托了；
                    {
                        item.intervalCallback?.Invoke();
                        //重置间隔时间，重新开始减；
                        item.ResetIntervalTime();
                    }
                }
                item.totalTime -= (int)(interval * 1000);
                if (item.totalTime <= 0)
                {
                    item.endCallback?.Invoke();
                    //由于仍在进行遍历，所以此时不可以直接将该item从Dictionary中移除；
                    //使用老办法：将所有符合删除条件的item记录入一个待删除列表；
                    listToDelete.Add(item);
                }

            }
            for (int i = 0; i < listToDelete.Count; i++)
            {
                timeDic.Remove(listToDelete[i].indexOfItem);
                listToDelete[i] = null; //置空，防止出现内存泄漏；
            }
            listToDelete.Clear();

        }

    }

    //不受Time.scaleTime影响的协程
    private IEnumerator FreeCoroutineContent()
    {
        while (true)
        {
            yield return waitForRealSeconds;
            //遍历所有仍在运作的TimeItem类
            foreach (TimeItem item in timeFreeDic.Values)
            {
                if (!item.isRunning)
                    continue;   //该事件类不参与运作了，那就跳过，直接访问下一个

                if (item.intervalTime != 0)  //可能会有不希望执行间隔委托回调的定时器；那就不要执行；
                {
                    item.intervalTime -= (int)(interval * 1000);      //interval转为毫秒，并且切换为int类型；
                    if (item.intervalTime <= 0)  //说明间隔时间减完了，到时间了，要执行间隔的委托了；
                    {
                        item.intervalCallback?.Invoke();
                        //重置间隔时间，重新开始减；
                        item.ResetIntervalTime();
                    }
                }
                item.totalTime -= (int)(interval * 1000);
                if (item.totalTime <= 0)
                {
                    item.endCallback?.Invoke();
                    //由于仍在进行遍历，所以此时不可以直接将该item从Dictionary中移除；
                    //使用老办法：将所有符合删除条件的item记录入一个待删除列表；
                    listFreeToDelete.Add(item);
                }

            }
            for (int i = 0; i < listFreeToDelete.Count; i++)
            {
                timeFreeDic.Remove(listFreeToDelete[i].indexOfItem);
                listFreeToDelete[i] = null; //置空，防止出现内存泄漏；
            }
            listFreeToDelete.Clear();

        }

    }

    //方法：添加TimeItem计时器；
    //传入：间隔时间、延时总时间（单位：毫秒）；间隔委托、最终委托；
    //返回该次创建的TimeItem的对应编号，以便删除的时候可以对应得上 
    public int CreateTimeItem(UnityAction endAction_, int totalTime_, bool isFreeFromScaleTime = false, int intervalTime_ = 0,  UnityAction intervalAction_ = null)
    {
        if(!isFreeFromScaleTime)
        {
         //每创建一个计时器，就使index自增；这样每个定时器的index就是唯一的；
            indexsAll++;
            if(!timeDic.ContainsKey(indexsAll))
                timeDic.Add(indexsAll,new TimeItem(totalTime_, endAction_, indexsAll, intervalTime_, intervalAction_));
            return indexsAll;
        }
        else
        {
            //每创建一个计时器，就使index自增；这样每个定时器的index就是唯一的；
            indexsAllFree++;
            if (!timeFreeDic.ContainsKey(indexsAllFree))
                timeFreeDic.Add(indexsAllFree, new TimeItem(totalTime_, endAction_, indexsAll, intervalTime_, intervalAction_));
            return indexsAllFree;
        }
    }


    //删除单个计时器：
    //只是置空计时器对象，不删除，防止此时协程仍然在执行，如果字典仍然在遍历，此时直接从字典中移除，可能会出现问题；
    //将isRunning置为false，可以一定程度防止再次访问该计时器对象的成员；
    public void DeleteTimeItem(int indexTarget,bool isFreeFromScaleTime = false)
    {
        if (!isFreeFromScaleTime)
        {
            if (timeDic.ContainsKey(indexTarget))
            {
                timeDic[indexTarget].isRunning = false;
                timeDic[indexTarget] = null;
            }
        }
        else
        {
            if (timeFreeDic.ContainsKey(indexTarget))
            {
                timeFreeDic[indexTarget].isRunning = false;
                timeFreeDic[indexTarget] = null;
            }
        }

    }

    //方法：重置计时器：
    public void ResetTimeItem(int indexTarget, bool isFreeFromScaleTime = false)
    {
        if (!isFreeFromScaleTime)
        {
            if (timeDic.ContainsKey(indexTarget))
            {
                timeDic[indexTarget].ResetIntervalTime();
                timeDic[indexTarget].ResetTotalTime();
            }
        }
        else
        {
            if (timeFreeDic.ContainsKey(indexTarget))
            {
                timeFreeDic[indexTarget].ResetIntervalTime();
                timeFreeDic[indexTarget].ResetTotalTime();
            }
        }

    }

//开启计时器和停止计时器：(用于暂停后重新开始)
    public void StartTimeItem(int indexTarget, bool isFreeFromScaleTime = false)
    {
        if (!isFreeFromScaleTime)
        {
            if (timeDic.ContainsKey(indexTarget))
                timeDic[indexTarget].isRunning = true;
        }
        else
        {
            if (timeFreeDic.ContainsKey(indexTarget))
                timeFreeDic[indexTarget].isRunning = true;
        }
    }

    public void StopTimeItem(int indexTarget, bool isFreeFromScaleTime = false)
    {
        if (!isFreeFromScaleTime)
        {
            if (timeDic.ContainsKey(indexTarget))
                timeDic[indexTarget].isRunning = false;
        }
        else
        {
            if (timeFreeDic.ContainsKey(indexTarget))
                timeFreeDic[indexTarget].isRunning = false;
        }
    }

}
