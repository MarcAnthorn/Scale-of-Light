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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MediumSize"))
        {
            //当对象进入了再开启分裂锁：
            EventHub.Instance.EventTrigger<bool>("LockDivide", false);
            //显示提示性note:
            note = PoolManager.Instance.SpawnFromPool("Note");
            note.transform.position = this.transform.position + offset;
            noteRenderer = note.GetComponent<SpriteRenderer>();

            LeanTween.value(note, EnableUpdateAlpha, 0f, 1f, 0.4f);


        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            UIManager.Instance.ShowPanel<GuidePanel>();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIManager.Instance.HidePanel<GuidePanel>();
        }
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
