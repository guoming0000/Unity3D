﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("MyGame/TitleScreen")]
public class TitleScreen : MonoBehaviour
{
    void OnGUI()
    {
        GUI.skin.label.fontSize = 48;//文字大小
        GUI.skin.label.alignment = TextAnchor.LowerCenter;//UI中心对齐
        GUI.Label(new Rect(0, 30, Screen.width, 100), "太空大战");//显示标题

        //开始游戏按钮
        if (GUI.Button(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.7f, 200, 30), "开始游戏"))
        {
            //开始读取下一关
            Application.LoadLevel("level1");
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
