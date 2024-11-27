using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDetection : MonoBehaviour
{

    public Animator PlayerAnimator;
    
    //碰撞体接触时保持设置Jumping为false，表示可跳跃
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            PlayerAnimator.SetBool("Jumping", false);
        }
    }

    //碰撞体不接触时保持设置Jumping为true，表示不可跳跃
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            PlayerAnimator.SetBool("Jumping", true);
        }
    }
}
