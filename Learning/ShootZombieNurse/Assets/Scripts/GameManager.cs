using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    int m_score = 0;//游戏得分
    static int m_histore = 0;//游戏最高得分
    int m_ammo = 100;//弹药数量
    Player m_player;//游戏主角

    //UI文字
    GUIText txt_ammo;
    GUIText txt_hiscore;
    GUIText txt_life;
    GUIText txt_score;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获得主角

        //获得设置的UI文字
        txt_ammo = this.transform.FindChild("GUI_Bullet_Num").GetComponent<GUIText>();
        txt_hiscore = this.transform.FindChild("GUI_History").GetComponent<GUIText>();
        txt_life = this.transform.FindChild("GUI_Health").GetComponent<GUIText>();
        txt_score = this.transform.FindChild("GUI_Score").GetComponent<GUIText>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //如果主角的生命值为零，则游戏失败
        if (m_player.m_life <= 0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;//剧中显示文字
            GUI.skin.label.fontSize = 40;//改变文字大小
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "游戏结束");//显示Game Over

            //显示重新游戏按钮
            GUI.skin.label.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.75f, 300, 40), "再试一次"))
            {
                Application.LoadLevel(Application.loadedLevelName);//重新载入当前关卡
            }

        }
    }

    //更新分数
    public void SetScore(int score)
    {
        m_score += score;
        if (m_score > m_histore)
        {
            m_histore = m_score;
        }
        txt_score.text = "Score " + m_score;
        txt_hiscore.text = "High Score " + m_histore;
    }

    //更新弹药
    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;

        //如果弹药为负数，重新填弹
        if (m_ammo <= 0)
        {
            m_ammo = 100 - m_ammo;
        }
        txt_ammo.text = m_ammo.ToString() + "/100";
    }

    //更新生命
    public void SetLife(int life)
    {
        txt_life.text = life.ToString();
    }

}
