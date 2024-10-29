using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BasicBallLogic : MonoBehaviour
{
    [SerializeField]
    [Range(1, 5)]
    private int speed;

    //[SerializeField]
    //[Range(0, 2)]
    //private float jumpHeight;

    [SerializeField]
    [Range(0, 4f)]
    [Tooltip("Jump height")]
    private float height;

    [SerializeField]
    [Range(0, 2f)]
    private float jumpTime;

    //空中停滞
    [SerializeField]
    [Range(0, 1f)]
    private float stagnationTime;

    [SerializeField]
    public bool ifInputDetect = false;
    [SerializeField]
    private bool ifLand = true;


    //跳跃需要的力；
    [SerializeField]
    private Vector3 jumpForce;
    float g = 9.81f; // 重力加速度

    //由于重写状态机是不能通过行为脚本实现对应的逻辑的，因此只能在输入读取的时候进行音效的播放；


    public bool ifPipeEnterUnlocked = false;

    [SerializeField]
    [Range(0, 5)]
    private int gravity;
    public int Gravity => gravity;


    private Animator animatorNowControlled;

    private bool jumpSoundLock = true;

    private bool jumpForceLock = false;

    private Rigidbody2D Rigidbody { get; set; }
    //[SerializeField]
    //[Range(0, 5f)]
    //private float gravityScale;


    private void Awake()
    {
        jumpSoundLock = true;
        Rigidbody = this.GetComponent<Rigidbody2D>();
        //计算对象需要的力的大小；
        jumpForce = new Vector3(0, Rigidbody.mass * Mathf.Sqrt(2 * g * Rigidbody.gravityScale * (height + 1)), 0);
        if (this.gameObject.CompareTag("MaxSize"))
            ifInputDetect = true;
        else
            ifInputDetect = false;

       
    }

    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<bool>("UnlockOrLockPipeEnter", UnlockOrLockPipeEnter);
        EventHub.Instance.AddEventListener<Animator>("FetchAnimatorNowControlled", FetchAnimatorNowControlled);
        
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<bool>("UnlockOrLockPipeEnter", UnlockOrLockPipeEnter);
        EventHub.Instance.RemoveEventListener<Animator>("FetchAnimatorNowControlled", FetchAnimatorNowControlled);
    }

    private void Update()
    {
        InputDetect();
    }

    private void FixedUpdate()
    {
        
    }

    private void InputDetect()
    {
        if (!Input.anyKeyDown && !Input.anyKey && animatorNowControlled != null)
        {
            animatorNowControlled.SetBool("isIdle", true);
            animatorNowControlled.SetBool("isMove", false);
        }

        if (ifInputDetect && !MoveController.isInputLockedStatic && animatorNowControlled != null)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                animatorNowControlled.SetBool("isIdle", false);
                animatorNowControlled.SetBool("isMove", true);

                if (Input.GetKeyDown(KeyCode.Space) && ifLand)
                {

                    ifLand = false;
                    animatorNowControlled.SetBool("isLand", false);
                    animatorNowControlled.SetTrigger("triggerDashJump");
                    Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                    jumpForceLock = true;
                    LeanTween.delayedCall(0.2f, () =>
                    {
                        jumpForceLock = false;
                    });

                }

                if (Input.GetKey(KeyCode.A))
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                    this.transform.Translate(speed * Time.deltaTime * Vector3.right);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                    this.transform.Translate(speed * Time.deltaTime * Vector3.right);            
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && ifLand)
            {

                //起跳之后如果碰见球，isLand就会重置，因此需要注意这点：
                ifLand = false;
                animatorNowControlled.SetBool("isLand", false);
                animatorNowControlled.SetTrigger("triggerJump");
                Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                jumpForceLock = true;
                LeanTween.delayedCall(0.2f, () =>
                {
                    jumpForceLock = false;
                });

            }

            //执行进管道逻辑；
            if (ifPipeEnterUnlocked && Input.GetKeyDown(KeyCode.Q))
                EventHub.Instance.EventTrigger<Transform>("PipeTriggerEnter", this.gameObject.transform);

            if(Input.GetKeyDown(KeyCode.I))
            {
                //问题出在这里：虽然确实只有一个中球响应了分裂的输入，但是他这个事件发布直接激活了所有的中球分裂事件；
                //导致场上的所有的中球都执行了分裂的逻辑！
                //修复的方法：通过全局唯一的当前操作对象，进行分裂方法的选择执行；
                Debug.Log("Divide Executed");
                if (this.gameObject.CompareTag("MaxSize"))
                    EventHub.Instance.EventTrigger("MaxDivide");
                else if(this.gameObject.CompareTag("MediumSize"))
                    EventHub.Instance.EventTrigger("MediumDivide");
            }

        }
    }

    public void UnlockOrLockPipeEnter(bool _lock)
    {
        ifPipeEnterUnlocked = _lock;
        Debug.Log("PipeEntryUnlocked");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
        {
            if(!jumpSoundLock)
            {
                SoundEffectManager.Instance.PlaySoundEffect("Sound/SlimeSound/JumpLand");
            }
            jumpSoundLock = false;
            if(!jumpForceLock)
                ifLand = true;
            animatorNowControlled.SetBool("isLand", true);

        }

   
    }
    public void CancelOrResumeJump(bool _ifLand)
    {
        ifLand = _ifLand;
    }

    public void FetchAnimatorNowControlled(Animator _animator)
    {
        animatorNowControlled = _animator;
    }
}