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
    public bool CanDash = true;
    public float DashSpeed = 20.0f;
    public float DashDirection = 1.0f;

    public float DashRead = 1.0f;
    public float DashInterval = 2.0f;
    public bool Dashing = false;

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
        PlayerAttack();
    }

    public void PlayerMove()
    {
        //获取X轴的输入
        float XInput = Input.GetAxisRaw("Horizontal");

        if (CanDash)
        {
            //冲刺
            PlayerRig.velocity = new Vector2(DashSpeed * DashDirection, 0);

            CanDash = false;    //后面需要注释
        }
        else
        {
            //检测玩家是否在攻击状态(攻击时不可移动)
            if (Attacking)
            {
                XInput = 0;
            }

            //设置X Y轴速度，其中Y轴速度为自身代表无法在Y轴移动
            PlayerRig.velocity = new Vector2(XInput * PlayerSpeed, PlayerRig.velocity.y);
            
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
        if (Input.GetKeyDown(KeyCode.J))
        {

        }
    }

    public void DashReStart()
    {
        CanDash = true;
    }

    public void TelEnd()
    {
        Dashing = false;
        PlayerAnimator.SetTrigger("DashEnd");
        Invoke("DashReStart", DashInterval);
        //AttackEndEvent();

    }
}
