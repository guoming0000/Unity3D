 using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
    public PathNode m_parent;//父路点
    public PathNode m_next;//子路点

    //设置子节点
    public void SetNext(PathNode node)
    {
        if(m_next != null)
        {
            m_next.m_parent = null;
        }
        m_next = node;
        node.m_parent = this;
    }

    //显示路点图标
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Node.tif");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
