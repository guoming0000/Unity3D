using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/EnemySpawn")]
public class EnemySpawn : MonoBehaviour {

    public Transform m_enemy;//敌人的Prefab
    protected float m_timer = 5;//生成敌人的间隔--5s
    protected Transform m_transform;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        m_timer -= Time.deltaTime;
        if(m_timer <= 0)
        {
            m_timer = Random.value * 15.0f;//Random.value产生0.0~1.0之间的随机数
            if(m_timer < 5)//最小产生时间间隔是5s，最大是15s
            {
                m_timer = 5;
            }
            Instantiate(m_enemy, m_transform.position, Quaternion.identity);
        }
	}

    //让敌人生成器在场景中以图标形式显示
    void OnDrawGizmos()
    {
        //这里使用transform而不能使用m_transform，因为没有被复制。只有transform表示是自己
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }
}

