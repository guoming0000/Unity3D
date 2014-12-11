using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Transform m_transform;//Transform组件
    Animator m_animator;//动画组件
    Player m_player;//主角
    NavMeshAgent m_agent;//寻路组件
    float m_movSpeed = 0.5f;//角色移动速度
    float m_rotSpeed = 120;//角色旋转速度，当敌人进攻主角时，它始终旋转到面向主角的角度
    float m_timer = 2;//计时器。用来计算时间，比如待机一定时间，每隔一定时间更新寻路
    int m_life = 5;//敌人的生命值

    // Use this for initialization
    void Start()
    {
        m_transform = this.transform;//获取组件
        m_animator = this.GetComponent<Animator>();//获取动画组件
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获得主角
        m_agent = GetComponent<NavMeshAgent>();//获得寻路组件
        m_agent.SetDestination(m_player.m_transform.position);//设置寻路目标----都往主角那里走
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player.m_life <= 0)
        {
            return;
        }

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);//获取当前动画状态

        //如果处于待机状态  ///m_animator.IsInTransition(0)是否是过渡状态，如果是，什么都不做
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_animator.IsInTransition(0))
        {
            m_animator.SetBool("idle", false);

            //待机一定时间
            m_timer -= Time.deltaTime;
            if (m_timer > 0)
            {
                return;
            }

            //如果距离主角小于1.5米，进入攻击动画状态
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5f)
            {
                m_animator.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;//重置定时器
                m_agent.SetDestination(m_player.m_transform.position);//设置寻路目标
                m_animator.SetBool("run", true);
            }
        }

        //如果处于跑步状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_animator.IsInTransition(0))
        {
            m_animator.SetBool("run", false);

            //每隔1秒重新定位主角的位置
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }

            //追向主角
            MoveTo();

            //如果距离主角小于1.5米，向主角攻击
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= 1.5f)
            {
                m_agent.ResetPath();//停止寻路
                m_animator.SetBool("attack", true);//进入攻击状态
            }
        }

        //如果处于攻击状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.attack") && !m_animator.IsInTransition(0))
        {
            //面向主角
            RotateTo();
            m_animator.SetBool("attack", false);

            //如果动画播完，重新进入待机状态
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_animator.SetBool("idle", true);
                m_timer = 2;//重置计时器

                m_player.OnDamage(1);//更新主角生命
            }
        }

        //如果处于死亡状态
        if(stateInfo.nameHash == Animator.StringToHash("Base Layer.death") && !m_animator.IsInTransition(0))
        {
            //当播放完成死亡动画
            if(stateInfo.normalizedTime>=1.0f)
            {
                GameManager.Instance.SetScore(100);//加分
                Destroy(this.gameObject);//销毁自身
            }
        }
    }

    //寻路移动
    void MoveTo()
    {
        float dist = m_movSpeed * Time.deltaTime;
        m_agent.Move(m_transform.TransformDirection((new Vector3(0, 0, dist))));
    }

    //转向目标点
    void RotateTo()
    {
        Vector3 old_angle = m_transform.eulerAngles;//当前角度

        //获得面向主角的角度
        m_transform.LookAt(m_player.m_transform);
        float target = m_transform.eulerAngles.y;

        //转向主角
        float dist = m_rotSpeed * Time.deltaTime;
        //MoveTowardsAngle是一个实用的函数，它的作用是基于旋转角度，计算出当前角度转向目标角度的旋转角度
        float angle = Mathf.MoveTowardsAngle(old_angle.y, target, dist);
        m_transform.eulerAngles = new Vector3(0, angle, 0);
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;

        //如果生命为0，取消鼠标锁定
        if (m_life <= 0)
        {
            m_animator.SetBool("death", true);
        }
    }
}
