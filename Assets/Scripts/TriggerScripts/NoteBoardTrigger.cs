using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBoardTrigger : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    private GameObject note;
    private SpriteRenderer noteRenderer;

    [SerializeField]
    private Transform noteTarget;

    private bool isUILocked = true;
    private void Update()
    {
       if(!isUILocked)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Guide UI is triggered!");
                UIManager.Instance.ShowPanel<GuidePanel>();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                UIManager.Instance.HidePanel<GuidePanel>();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MediumSize"))
        {
        
            //显示提示性note:
            note = PoolManager.Instance.SpawnFromPool("Note");
            note.transform.position = this.transform.position + offset;
            noteRenderer = note.GetComponent<SpriteRenderer>();

            LeanTween.value(note, EnableUpdateAlpha, 0f, 1f, 0.4f);
            isUILocked = false;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MediumSize"))
        {
            UIManager.Instance.HidePanel<GuidePanel>();
            LeanTween.value(note, DisableUpdateAlpha, 1f, 0f, 0.4f).setOnComplete(() =>
            {
                //隐藏提示性note:
                PoolManager.Instance.ReturnToPool("Note", note);
            });

            isUILocked = true;
            
        }
    }

    // 生成时更新 alpha 值的回调函数
    private void EnableUpdateAlpha(float alphaValue)
    {
        Color newColor = noteRenderer.color;
        newColor.a = alphaValue;  // 修改 alpha 值
        noteRenderer.color = newColor;  // 应用新的颜色
    }

    // 失活时更新 alpha 值的回调函数
    private void DisableUpdateAlpha(float alphaValue)
    {
        Color newColor = noteRenderer.color;
        newColor.a = alphaValue; 
        noteRenderer.color = newColor;  
    }


}
