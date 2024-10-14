using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStackFix : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    private void Awake()
    {
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    void Start()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            var mainCamData = mainCamera.GetUniversalAdditionalCameraData();

            // 检查 UICamera 是否已经在 Stack 中
            if (!mainCamData.cameraStack.Contains(uiCamera))
            {
                mainCamData.cameraStack.Add(uiCamera);
            }
        }
    }
}

