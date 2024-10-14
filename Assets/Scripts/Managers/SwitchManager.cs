using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : SingletonBaseManager<SwitchManager>
{
    private SwitchManager() { }

    public int ballIndex = 0;
    public Queue<GameObject> ballQueue = new Queue<GameObject>();
    private bool ifFind = false;

    //向队列中添加一个MinSizeBall
    public GameObject AddNewMinToQueue(Vector3 _position)
    {
        GameObject newChild = PoolManager.Instance.SpawnFromPool("MinSizeBall", _position, Quaternion.identity);
        ballQueue.Enqueue(newChild);
        //Debug.Log("Now Child Is Enqueued, MinName:" + newChild.gameObject.name);
        ballIndex++;
        return newChild;
    }

    //向队列中添加一个MediumSizeBall
    public GameObject AddNewMediumToQueue(Vector3 _position)
    {
        GameObject newMedium = PoolManager.Instance.SpawnFromPool("MediumSizeBall", _position, Quaternion.identity);
        ballQueue.Enqueue(newMedium);
        //Debug.Log("Now Medium Is Enqueued, MediumName:" + newMedium.gameObject.name);
        ballIndex++;
        return newMedium;
    }

    //向队列中添加一个MaxSizeBall
    public GameObject AddNewMaxToQueue(Vector3 _position)
    {
        GameObject newBase = PoolManager.Instance.SpawnFromPool("MaxSizeBall", _position, Quaternion.identity);
        ballQueue.Enqueue(newBase);
        //Debug.Log("Now Max Is Enqueued, MaxName:" + newBase.gameObject.name);
        ballIndex++;
        return newBase;
    }


    //向队列中添加新的球实例；如果实例已经存在，那么就不会重复添加
    public void PushBallToQueue(GameObject ball)
    {
        if (CheckMembersInQueue(ball))
        {
            Debug.Log("Found Same, Returned");
            return;
        }
        ballIndex++;
        ballQueue.Enqueue(ball);
        //CheckMembersInQueue();

    }

    //从队列中删除指定的球，并且重新调整队列中的顺序；
    public void DeleteBallFromQueue(GameObject _ball)
    {
        ifFind = false;
        int queueCount = ballQueue.Count;
        if (ballQueue.Count == 0)
        {
            Debug.LogError("Ball Queue is Empty");
            return;
        }
        int index = 0;
        foreach(var ball in ballQueue)
        {
            index++;
            if (ball.gameObject.name == _ball.gameObject.name)
            {
                ifFind = true;
                break;
            }
        }
        if (!ifFind)
            return;
        GameObject nowBall;
        for (int i = 1;i < index;i++)
        {
            nowBall = ballQueue.Dequeue();
            ballQueue.Enqueue(nowBall);
        }
        ballQueue.Dequeue();
        for (int i = 0; i < queueCount - index; i++)
        {
            nowBall = ballQueue.Dequeue();
            ballQueue.Enqueue(nowBall);
        }

    }


    //取用当前队列的头部球对象，并且重新排入队尾；
    public GameObject FetchBallInSequence()
    {
        if (ballQueue.Count == 0)
        {
            Debug.LogError("Ball Queue is Empty");
            return null;
        }
        GameObject ball = ballQueue.Dequeue();
        ballQueue.Enqueue(ball);
        return ball;
    }

    //检查队列中是否已经有该对象存在了；
    private bool CheckMembersInQueue(GameObject _ball)
    {
        foreach (var ball in ballQueue)
        {
            if(ball.name == _ball.name)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckMembersInQueue()
    {
        foreach (var ball in ballQueue)
        {
            Debug.Log(ball.name);
        }
    }

}

