using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//声明一个抽象类
public abstract class EventInfoBaseClass { }


//声明一个子类继承抽象类
//注意：这两步要看懂，为什么不直接声明一个泛型类作为下方字典的value类型，而要声明一个抽象类作为字典的value类型，然后去用里氏替换原则装载这个泛型类？
//本质上是为了保证事件中心模块的唯一性，即全局只有一类事件中心模块；
public class EventInfo<T> : EventInfoBaseClass
{
    public UnityAction<T> action_;

    //构造函数，便于外部实例化时顺带传入监听方法；
    public EventInfo(UnityAction<T> actions)
    {
        action_ += actions;
    }
}


//如果说我就是不需要传参数，那该怎么办？
//方法：同样也声明一个EventInfoBaseClass的子类，该子类的成员是无参数的UnityAction；该子类用于事件发布和订阅方法的重载；
public class EventInfoNoPara : EventInfoBaseClass
{
    public UnityAction action_;

    public EventInfoNoPara(UnityAction action)
    {
        action_ += action;
    }
}

public class EventHub : SingletonBaseManager<EventHub>
{
    private EventHub() { }

    internal void EventTrigger<T>()
    {
        throw new NotImplementedException();
    }

    //如果事件中心模块这样定义，同时字典这样写：
    //public class EventHub<T> : SingletonBaseManager<EventHub<T>>
    //private Dictionary<string, EventInfo<T>> eventDictionary = new Dictionary<string, EventInfo<T>>();
    //实际上就是违背了全局只有一种事件中心模块的规则；
    //因为对于每个传入的T类型的数据，实际上都是一个全新类型的事件中心模块；
    //因此，我们选择使用一个非泛型的抽象类作为字典的value，同时使用里氏替换原则，利用抽象类对象来装载其泛型子类的实例；
    //这样就不会出现应为泛型而被迫违背事件中心的唯一性了；

    private Dictionary<string, EventInfoBaseClass> eventDictionary = new Dictionary<string, EventInfoBaseClass>();

    public void EventTrigger<T>(string eventName, T info)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            (eventDictionary[eventName] as EventInfo<T>).action_?.Invoke(info);      //info被作为参数传入；此时所有订阅者的监听方法都会收到该参数；
        }
    }


    //支持多播委托的事件订阅；
    public void AddEventListener<T>(string eventName, UnityAction<T> function)
    {
        if (eventDictionary.ContainsKey(eventName))
            (eventDictionary[eventName] as EventInfo<T>).action_ += function;  //as父转子实现子类成员委托调用
        else
        {
            eventDictionary.Add(eventName, new EventInfo<T>(function));     //里氏替换父装载子类实例
        }
    }

     //重载：不支持多播委托的事件订阅；
    public void AddEventListenerNotMultuple<T>(string eventName, UnityAction<T> function)
    {
        if (eventDictionary.ContainsKey(eventName) && (eventDictionary[eventName] as EventInfo<T>).action_ == null)
            (eventDictionary[eventName] as EventInfo<T>).action_ += function;  //as父转子实现子类成员委托调用
        else
        {
            eventDictionary.Add(eventName, new EventInfo<T>(function));     //里氏替换父装载子类实例
        }
    }




    public void RemoveEventListener<T>(string eventName, UnityAction<T> function)
    {
        if (eventDictionary.ContainsKey(eventName))
            (eventDictionary[eventName] as EventInfo<T>).action_ -= function;   //根据里氏替换原则，就算父类对象中装载了子类实例，要想访问子类中的成员，也必须要使用as进行转换
    }


    public void ClearListener()
    {
        eventDictionary.Clear();
    }


    public void ClearListener(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary.Remove(eventName);
    }




    //所有订阅事件和发布事件的无参的重载
    public void EventTrigger(string eventName)  //默认为null，这样外部就算没有信息可交流， 不传参，也不会报错；
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            (eventDictionary[eventName] as EventInfoNoPara).action_?.Invoke();      //info被作为参数传入；此时所有订阅者的监听方法都会收到该参数；
        }
    }

    public void AddEventListener(string eventName, UnityAction function)
    {
        if (eventDictionary.ContainsKey(eventName))
            (eventDictionary[eventName] as EventInfoNoPara).action_ += function;  //as父转子实现子类成员委托调用
        else
        {
            eventDictionary.Add(eventName, new EventInfoNoPara(function));     //里氏替换父装载子类实例
        }
    }

    public void RemoveEventListener(string eventName, UnityAction function)
    {
        if (eventDictionary.ContainsKey(eventName))
            (eventDictionary[eventName] as EventInfoNoPara).action_ -= function;   //根据里氏替换原则，就算父类对象中装载了子类实例，要想访问子类中的成员，也必须要使用as进行转换
    }


}



