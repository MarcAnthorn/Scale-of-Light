using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntryNote : BaseNote
{
    protected void OnEnable()
    {
        EventHub.Instance.AddEventListener<GameObject>("RevealPipeNote", RevealPipeNote);
        EventHub.Instance.AddEventListener("HidePipeNote", HidePipeNote);
       
    }

    private void OnDisable()
    {

        EventHub.Instance.RemoveEventListener<GameObject>("RevealPipeNote", RevealPipeNote);
        EventHub.Instance.RemoveEventListener("HidePipeNote", HidePipeNote);
    }

    protected void HidePipeNote()
    {
        LeanTween.value(this.gameObject, currentColor.a, 0, 0.55f)
         .setOnUpdate((float alpha) =>
         {
             // 在插值过程中更新 SpriteRenderer 的 Alpha 值
             currentColor.a = alpha;
             sr.color = currentColor;
         }).setOnComplete(() =>
         {
             LeanTween.cancel(this.gameObject);
             PoolManager.Instance.ReturnToPool("PipeEntryNote", this.gameObject);
         });
      
    }

    protected void RevealPipeNote(GameObject _note)
    {
        GameObject nowNote = _note;
        target = GameObject.Find("NoteTarget").transform;
        nowNote.transform.position = target.position + spawnOffset;

        sr = nowNote.GetComponent<SpriteRenderer>();
        //重置alpha值为0；
        currentColor = sr.color;
        currentColor.a = 0;
        sr.color = currentColor;

        LeanTween.value(nowNote, currentColor.a, 1, 0.55f)
          .setOnUpdate((float alpha) =>
          {
              // 在插值过程中更新 SpriteRenderer 的 Alpha 值
              currentColor.a = alpha;
              sr.color = currentColor;
          }).setOnComplete(() =>
          {
              this.transform.LeanMoveLocalY(target.position.y, floatTime).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong();
          });
        
    }
}
