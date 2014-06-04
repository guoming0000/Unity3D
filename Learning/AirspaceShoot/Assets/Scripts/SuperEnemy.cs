using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/SuperEnemy")]
public class SuperEnemy : Enemy {

    public Transform m_rocket;
    protected float m_fireTimer = 2;//发射时间间隔
    protected Transform m_player;//主角的飞船

    void Awake()//继承自MonoBehaviour，他会在游戏体实例化时执行一次，并先于Start方法。
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");//找到用户控制的飞机
        if(obj != null)//如果用户飞机还活着
        {
            m_player = obj.transform;
        }
    }
    protected override void UpdateMove()
    {
        m_fireTimer -= Time.deltaTime;
        if(m_fireTimer < 0)
        {
            m_fireTimer = 2;
            if(m_player != null)
            {
                Vector3 relativePos = m_transform.position - m_player.position;
                Instantiate(m_rocket, m_transform.position, Quaternion.LookRotation(relativePos));
            }
        }
        m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));//前进
    }
}
