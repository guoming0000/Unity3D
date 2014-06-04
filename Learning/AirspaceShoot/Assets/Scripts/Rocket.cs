using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Rocket")]
public class Rocket : MonoBehaviour {

    public float m_speed = 10;//子弹飞行速度
    public float m_liveTime = 1f;//生存时间
    public float m_power = 1.0f;//威力
    protected Transform m_transform;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;

	}
	
	// Update is called once per frame
	void Update () {
        m_liveTime -= Time.deltaTime;
        if(m_liveTime <= 0){
            Destroy(this.gameObject);
        }
        m_transform.Rotate(new Vector3(0, 0, 1), 11, Space.Self);
        m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("Enemy") != 0)//如果遇到的不是敌人则返回
        {
            return;
        }
        else
        {
            Destroy(this.gameObject);//如果遇到敌人，则自己消失        
        }
    }
}
