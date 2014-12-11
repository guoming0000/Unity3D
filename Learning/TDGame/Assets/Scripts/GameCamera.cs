using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
    public static GameCamera Instance = null;
    protected float m_distance = 15;//摄像机与地面的距离
    protected Vector3 m_rot = new Vector3(-55, 180, 0);//摄像机的角度
    protected float m_moveSpeed = 60;//摄像机移动速度

    //摄像机的移动值
    protected float m_vx = 0;
    protected float m_vy = 0;

    protected Transform m_transform;//Transform组件
    protected Transform m_cameraPoint;//摄像机的焦点

    void Awake()
    {
        Instance = this;
        m_transform = this.transform;
    }

	// Use this for initialization
	void Start () {
        m_cameraPoint = CameraPoint.Instance.transform;//获取摄像机的焦点
        
        Follow();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //在Update之后执行
    void LateUpdate()
    {
        Follow();
    }

    //摄像机对其到焦点的位置和角度
    void Follow()
    {
        m_transform.position = m_cameraPoint.position;
        m_transform.eulerAngles = m_rot;
        m_transform.Translate(0, 0, m_distance);

        this.transform.LookAt(m_cameraPoint);
    }

    //控制摄像头移动
    public void Control(bool mouse, float mx, float my)
    {
        if(!mouse)
        {
            return;
        }

        m_cameraPoint.Translate(-mx * m_moveSpeed * Time.deltaTime, 0, -my * m_moveSpeed * Time.deltaTime);
    }
}
