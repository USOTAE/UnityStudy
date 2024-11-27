using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    public GameObject PlayerModel;
    public Rigidbody2D PlayerRig;
    public Animator PlayerAnimator;

    [Header("PlayerAttribute")]
    public float PlayerSpeed = 1.0f;
    public float JumpForce = 5.0f;
    public float Hp = 100.0f;
    public float HpNow = 100.0f;

    [Header("Dash")]
    [SerializeField] private bool CanDash = true; //能否冲刺
    [SerializeField] private float DashSpeed = 3.0f;  //冲刺时施加的速度
    [SerializeField] private float DashDirection = 1.0f;  //冲刺的方向
    [SerializeField] private float DashKeep = 0.5f;   //冲刺持续时间
    [SerializeField] private float DashInterval = 2.0f;   //冲刺的间隔
    [SerializeField] private bool Dashing = false;    //是否在冲刺状态中

    [Header("Attack")]
    public bool CanAttack = true;
    public bool Attacking = false;
    public float AttackInterval = 1.0f;
    public GameObject AttackBox;
    public GameObject AttackBoxLocation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerJump();
        PlayerDash();
        PlayerAttack();
    }

    public void PlayerDash()
    {
        if(CanDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                CanDash = false;
                Dashing = true;
                PlayerAnimator.SetTrigger("DashStart");
                Invoke("DashEnd", DashKeep);
            }
        }

    }

    public void PlayerMove()
    {
        //获取X轴的输入
        float XInput = Input.GetAxisRaw("Horizontal");


        //检测玩家是否在攻击状态(攻击时不可移动)
        if (Attacking)
        {
            XInput = 0;
        }

        //设置X Y轴速度，其中Y轴速度为自身代表无法在Y轴移动
        if (Dashing)
        {
            PlayerRig.velocity = new Vector2(DashSpeed * DashDirection, 0);
        }
        else
        {
            PlayerRig.velocity = new Vector2(XInput * PlayerSpeed, PlayerRig.velocity.y);
        }
            
        //控制Idle和run的动画混合树状态
        PlayerAnimator.SetFloat("RunBlend", Mathf.Abs(XInput));

        if (XInput > 0)
        {
            //记录最后输入的方向，用来决定冲刺的方向
            DashDirection = 1;

            PlayerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (XInput < 0)
        {
            DashDirection = -1;

            //输入反向反转模型
            PlayerModel.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
    }

    public void PlayerJump()
    {
        if (!Attacking)
        {
            bool Jumping = PlayerAnimator.GetBool("Jumping");

            if (!Jumping && Input.GetKeyDown(KeyCode.Space))
            {
                PlayerRig.velocity = new Vector2(PlayerRig.velocity.x, JumpForce);
            }
        }
    }

    public void PlayerAttack()
    {
        if (CanAttack)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                CanAttack = false;
                PlayerAnimator.SetTrigger("Attack");
                Invoke("AttackEnd", AttackInterval);
            }
        }
    }

    public void DashReStart()
    {
        CanDash = true;
    }

    public void DashEnd()
    {
        Dashing = false;
        PlayerAnimator.SetTrigger("DashEnd");
        Invoke("DashReStart", DashInterval);
    }

    public void AttackEnd()
    {
        CanAttack = true;
    }

    //使用动画事件来判断是否在攻击状态
    public void AttackStartEvent()
    {
        Attacking = true;
    }

    public void AttackEndEvent()
    {
        Attacking = false;
    }
}
