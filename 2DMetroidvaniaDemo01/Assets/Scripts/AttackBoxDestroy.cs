using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxDestroy : MonoBehaviour
{

    private float _destroyTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //销毁普通攻击伤害碰撞盒子，gameObject表示销毁自己
        Destroy(gameObject, _destroyTime);
    }
}
