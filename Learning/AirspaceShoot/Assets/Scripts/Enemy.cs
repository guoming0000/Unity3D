using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Enemy")]
public class Enemy : MonoBehaviour {

    public float m_speed = 1;//速度
    public float m_rotSpeed = 25;//旋转速度
    public float m_life = 5;//生命值
    public int m_point = 10;//敌人分数
    protected float m_timer = 1.5f;
    protected Transform m_transform;

    protected AudioSource m_audio;//声音源组件，用于播放声音
    public Transform m_explosionFX;//爆炸特效

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_audio = this.audio;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMove();
	}

    protected virtual void UpdateMove()
    {
        m_timer -= Time.deltaTime;
        if(m_timer < 0)
        {
            m_timer = 3;
            m_rotSpeed = -m_rotSpeed;//改变旋转方向
        }

       m_transform.Rotate(Vector3.up, m_rotSpeed * Time.deltaTime, Space.World);//旋转方向
       m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
    }

    //派生自MonoBehaviour的函数，在碰撞体互相碰撞时会被触发
    void OnTriggerEnter(Collider other)//另一个人和他发生碰撞
    {
        if(other.tag.CompareTo("PlayerRocket") == 0)//判断是否是子弹
        {
            Rocket rocket = other.GetComponent<Rocket>();//获取碰撞体的Rocket脚本组件，转换成子弹对象
            if(rocket != null)//有时候还会是空的?
            {
                m_life -= rocket.m_power;
                if(m_life <= 0)//生命耗尽，则毁灭自己
                {
                    GameManager.Instance.AddScore(m_point);
                    Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }
        else if(other.tag.CompareTo("Player") == 0)//如果主角飞机碰到敌人
        {
            m_life = 0;
            Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
            Destroy(this.gameObject);//敌人死掉
        }
        else if(other.tag.CompareTo("bound") == 0)
        {
            m_life = 0;
            Destroy(this.gameObject);
        }
    }

}
