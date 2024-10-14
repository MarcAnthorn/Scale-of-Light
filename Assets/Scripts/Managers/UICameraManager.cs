using UnityEngine;

public class UICameraManager : MonoBehaviour
{
    private static UICameraManager instance;

    void Awake()
    {
        // 检查是否已有一个实例存在
        if (instance == null)
        {
            // 如果没有实例，将当前实例设置为唯一实例
            instance = this;

            // 保证当前对象不被销毁
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果已经有一个实例存在，销毁当前重复的UICamera
            Destroy(gameObject);
        }
    }
}
