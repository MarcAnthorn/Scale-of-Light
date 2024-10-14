using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    void Awake()
    {
        // 检查是否已有一个 Canvas 实例存在
        if (instance == null)
        {
            // 如果没有实例，将当前实例设置为唯一实例
            instance = this;

            // 保证当前对象不被销毁
            DontDestroyOnLoad(gameObject); // 保证 Canvas 在场景切换时不被销毁
        }
        else
        {
            // 如果已有一个实例存在，销毁当前重复的 Canvas
            Destroy(gameObject);
        }
    }
}
