using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

//解决步骤1:基类声明为抽象类
//成为抽象类之后，该类就不能在任何地方被实例化；
//换言之，该语句就永远不能成立，基类的唯一性得到了保证：
//SingletonBaseManager<T> tBase = new SingletonBaseManager<T>();   

public abstract class SingletonBaseManager<T> where T : class  //new()    //注意：由于子类声明了私有的构造函数，我们不得不删除new()这个限制；
{
    //使用反射解决：
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //Type的使用，需要引用命名空间：System
                Type type = typeof(T);
                //BingingFlags需要引用命名空间：System.Reflections
                //下面这一步就是实现获取T类内部的私有构造函数：
                ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                            null,
                                                            Type.EmptyTypes,
                                                            null);
                if (info != null)
                    instance = info.Invoke(null) as T;
                //该语句会返回Object类型的实例化对象；使其转换类型后被引用，就实现了在基类内部的子类的实例化！
                //至此，该问题就完全解决了；
                else
                    Debug.Log("未得到对应的子类无参构造函数");
                //设置一个提示，防止忘记声明私有构造函数；

            }
            return instance;
        }
    }

}
