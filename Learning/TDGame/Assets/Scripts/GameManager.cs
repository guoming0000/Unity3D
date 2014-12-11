using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool m_debug = false;
    public ArrayList m_PathNode;

    [HideInInspector]
    public int m_wave = 1;//波数不在Inspector窗口显示
    public int m_life = 10;//生命
    public int m_point = 10;//点数

    //文字
    GUIText m_txt_wave;
    GUIText m_txt_life;
    GUIText m_txt_point;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //获取文字控件
        m_txt_wave = this.transform.FindChild("txt_wave").GetComponent<GUIText>();
        m_txt_life = this.transform.FindChild("txt_life").GetComponent<GUIText>();
        m_txt_point = this.transform.FindChild("txt_point").GetComponent<GUIText>();

        //初始化文字
        m_txt_wave.text = "<color=red>wave</color>" + m_wave;
        m_txt_life.text = "<color=red>life</color>" + m_life;
        m_txt_point.text = "<color=red>point</color>" + m_point;

        //将所有场景中的路点装入m_PathNode
        BuildPath();
    }

    // Update is called once per frame
    void Update()
    {
        bool press = Input.GetMouseButton(0);//鼠标操作
        float mx = Input.GetAxis("Mouse X");//获得鼠标移动距离
        float my = Input.GetAxis("Mouse Y");

        GameCamera.Instance.Control(press, mx, my);
    }

    //更新波数
    public void SetWave(int wave)
    {
        m_wave = wave;
        m_txt_wave.text = "<color=red>wave</color>" + m_wave;
    }

    //更新生命
    public void SetDamage(int damage)
    {
        m_life -= damage;
        m_txt_life.text = "<color=red>life</color>" + m_life;
    }

    //更新点数
    public void SetPoint(int point)
    {
        m_point += point;
        m_txt_point.text = "<color=red>point</color>" + m_point;
    }

    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNode = new ArrayList();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");
        for (int i = 0; i < objs.Length; i++) 
        {
            PathNode node = objs[i].GetComponent<PathNode>();
            m_PathNode.Add(node);
        }
    }

    void OnDrawGizmos()
    {
        if(!m_debug || m_PathNode==null)
        {
            return;
        }

        Gizmos.color = Color.blue;
        
        foreach(PathNode node in m_PathNode)
        {
            if(node.m_next != null)
            {
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }
}
