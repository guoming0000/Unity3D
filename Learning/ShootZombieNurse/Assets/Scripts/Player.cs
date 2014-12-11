using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //组件
    public Transform m_transform;
    CharacterController m_ch;

    public int m_life = 100;//生命值
    float m_movSpeed = 5.0f;//角色移动速度
    float m_gravity = 2.0f;//重力

    Transform m_camTransform;//摄像机Transform
    Vector3 m_camRot;//摄像机旋转角度
    float m_camHeight = 1.4f;//摄像机高度

    Transform m_muzzlepoint;//枪口transform
    public LayerMask m_layer;//射击时，射线能射到的碰撞层
    public Transform m_fx;//射中目标后的粒子效果
    public AudioClip m_audio;//射击间隔时间计时器
    float m_shootTimer = 0;//射击间隔时间计时器

    // Use this for initialization
    void Start()
    {
        //获取组件
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();

        m_camTransform = Camera.main.transform;//获取摄像机
        //设置摄像头初始位置
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;

        //锁定鼠标
        Screen.lockCursor = true;

        m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //如果生命为0，什么也不做
        if (m_life <= 0)
        {
            return;
        }
        Control();

        //更新射击间隔时间
        m_shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1f;
            this.audio.PlayOneShot(m_audio);//射击音效
            GameManager.Instance.SetAmmo(1);//减少弹药，更新弹药UI
            RaycastHit info;//RaycastHit用来保存射线的探测结果

            //从muzzlepoint的位置，向摄像机面向的正方向射出一条射线
            //涉嫌只能与m_layer所指定的层碰撞
            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if (hit)
            {
                //如果射中了tag为enemy的游戏体
                //if (info.transform.CompareTag("enemy") == 0)
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                }
                //在射中的地方释放一个粒子效果，飙血效果
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }
    }

    void Control()
    {
        float xm = 0, ym = 0, zm = 0;
        ym -= m_gravity * Time.deltaTime;//重力运动

        //获取鼠标移动距离
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        //旋转摄像头
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        //使主角的面向方向与摄像机一致
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;

        //上下左右运动
        if (Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }
        //移动
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));

        //使摄像机的位置与主角一致
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;
        GameManager.Instance.SetLife(m_life);//更新UI

        //如果生命为0，取消鼠标锁定
        if (m_life <= 0)
        {
            Screen.lockCursor = false;
        }
    }
}
