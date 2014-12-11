using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public PathNode m_currentNode;//路点
    public int m_life = 15;//生命
    public int m_maxLife = 15;//最大生命
    public float m_speed = 2;//移动速度

    //敌人的类型
    public enum TYPE_ID
    {
        GROUND,
        AIR,
    }

    public TYPE_ID m_type = TYPE_ID.GROUND;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RotateTo();
        MoveTo();
	}

    //转向下一个路点
    public void RotateTo()
    {
        float current = this.transform.eulerAngles.y;
        this.transform.LookAt(m_currentNode.transform);
        Vector3 target = this.transform.eulerAngles;
        float next = Mathf.MoveTowardsAngle(current, target.y, 120 * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0, next, 0);
    }

    //向下一个路点移动
    public void MoveTo()
    {
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = m_currentNode.transform.position;

        //距离子路点的距离
        float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
        if (dist < 1.0f)
        {
            if (m_currentNode.m_next == null)
            {
                GameManager.Instance.SetDamage(1);
                Destroy(this.gameObject);
            }
            else
            {
                m_currentNode = m_currentNode.m_next;
            }
        }
        this.transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
    }
}
