using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/Player")]
public class Player : MonoBehaviour {
	public float m_speed = 1;
    public float m_rocketRate = 0;
    public Transform m_rocket;//子弹
    public float m_life = 3;
    protected Transform m_transform;//自己

    public AudioClip m_shootClip;//声音--这里就是射击的声音
    protected AudioSource m_audio;//声音源组件，用于播放声音
    public Transform m_explosionFX;//爆炸特效

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_audio = this.audio;
	}
	
	// Update is called once per frame
	void Update () {
		//纵向移动距离
		float move_v = 0 ;

		//水平移动距离 
		float move_h = 0;

		//上
		if(Input.GetKey( KeyCode.UpArrow ))
		{
			move_v -= m_speed * Time.deltaTime;
		}

		//下
		if(Input.GetKey( KeyCode.DownArrow ))
		{
			move_v += m_speed * Time.deltaTime;
		}

		//左
		if(Input.GetKey( KeyCode.LeftArrow ))
		{
			move_h += m_speed * Time.deltaTime;
		}

		//右
		if(Input.GetKey( KeyCode.RightArrow ))
		{
			move_h -= m_speed * Time.deltaTime;
		}

        /// 之所以下面的位移增加减少都不符合常理，是因为Translate默认以自身坐标系运动
        /// 这艘飞机的自身坐标的x,y,z都是与世界坐标系相反的！

        //移动
        m_transform.Translate(new Vector3(move_h, 0, move_v));
        m_rocketRate -= Time.deltaTime;
        if(m_rocketRate <= 0)
        {
            m_rocketRate = 0.1f;//每隔0.1s发射一次子弹
            //按空格键或鼠标左键发射子弹
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                //m_transform.Rotate(Vector3.up, m_rotSpeed * Time.deltaTime, Space.World);
                Instantiate(m_rocket, m_transform.position, Quaternion.AngleAxis(200, Vector3.up));
                Instantiate(m_rocket, m_transform.position, Quaternion.AngleAxis(160, Vector3.up));
                Instantiate(m_rocket, m_transform.position, Quaternion.AngleAxis(230, Vector3.up));
                Instantiate(m_rocket, m_transform.position, Quaternion.AngleAxis(130, Vector3.up));
                Instantiate(m_rocket, m_transform.position, m_transform.rotation);
                m_audio.PlayOneShot(m_shootClip);//播放射击音乐
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("PlayerRocket") != 0)//如果没有碰撞到自己发射子弹---也就是碰撞到敌人了
        {
            m_life -= 1;
            if(m_life <= 0)
            {
                Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
