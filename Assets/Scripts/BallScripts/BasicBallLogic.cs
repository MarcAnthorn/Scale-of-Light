using System.Collections;
using System.Collections.Generic;
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




    public bool ifPipeEnterUnlocked = false;

    [SerializeField]
    [Range(0, 5)]
    private int gravity;
    public int Gravity => gravity;

    private Animator animatorMax;


    private Rigidbody2D Rigidbody { get; set; }
    //[SerializeField]
    //[Range(0, 5f)]
    //private float gravityScale;


    private void Awake()
    {
        Rigidbody = this.GetComponent<Rigidbody2D>();
        //计算对象需要的力的大小；
        jumpForce = new Vector3(0, Rigidbody.mass * Mathf.Sqrt(2 * g * Rigidbody.gravityScale * height), 0);
        animatorMax = this.GetComponent<Animator>();
        if (this.gameObject.CompareTag("MaxSize"))
            ifInputDetect = true;
        else
            ifInputDetect = false;

       
    }

    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<bool>("UnlockOrLockPipeEnter", UnlockOrLockPipeEnter);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<bool>("UnlockOrLockPipeEnter", UnlockOrLockPipeEnter);
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
        if (ifInputDetect)
        {
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

            if (Input.GetKeyDown(KeyCode.Space) && ifLand)
            {
                ifLand = false;
                animatorMax.SetBool("isLand", false);
                animatorMax.SetTrigger("triggerJump");

                Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
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
        if (collision.gameObject.CompareTag("Terrain"))
        {
            ifLand = true;
            animatorMax.SetBool("isLand", true);

        }
    }

    public void CancelOrResumeJump(bool _ifLand)
    {
        ifLand = _ifLand;
    }

}