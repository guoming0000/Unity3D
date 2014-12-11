using UnityEngine;
using System.Collections;

public class GridMap : MonoBehaviour
{
    static public GridMap Instance = null;
    public bool m_debug = false;//是否显示场景信息

    public int MapSizeX = 32;//场景x方向大小
    public int MapSizeZ = 32;//场景z方向大小
    public MapData[,] m_map;//二维数组的声明

    void Awake()
    {
        Instance = this;
        this.BuildMap();
    }

    [ContextMenu("BuildMap")]
    public void BuildMap()//创建地图
    {
        m_map = new MapData[MapSizeX, MapSizeZ];//创建二维数组
        for (int i = 0; i < MapSizeX; ++i)
        {
            for (int k = 0; k < MapSizeZ; k++)
            {
                m_map[i, k] = new MapData();
            }
        }

        //获得所有Tag为gridnode的节点
        GameObject[] nodes = (GameObject[])GameObject.FindGameObjectsWithTag("gridnode");
        foreach (GameObject nodeobj in nodes)
        {
            GridNode node = nodeobj.GetComponent<GridNode>();//获得节点
            Vector3 pos = nodeobj.transform.position;
            if ((int)pos.x >= MapSizeX || (int)pos.z >= MapSizeZ)//如果节点的位置超出了场景范围，则忽略
            {
                continue;
            }

            //设置格子的属性
            m_map[(int)pos.x, (int)pos.z].fieldtype = node.m_mapData.fieldtype;
        }
    }

    void OnDrawGizmos()//绘制场景信息
    {
        if (!m_debug || m_map == null)
        {
            return;
        }

        Gizmos.color = Color.blue;//线条的颜色
        float height = 0;

        //绘制网格
        for (int i = 0; i < MapSizeX; i++)
        {
            Gizmos.DrawLine(new Vector3(i, height, 0), new Vector3(i, height, MapSizeZ));
        }
        for (int k = 0; k < MapSizeZ; k++)
        {
            Gizmos.DrawLine(new Vector3(0, height, k), new Vector3(MapSizeX, height, k));
        }

        Gizmos.color = Color.red;//改为红色
        for (int i = 0; i < MapSizeX; i++)
        {
            for (int k = 0; k < MapSizeZ; k++)
            {
                //在不能放置防守区域的方格内绘制红色的方块
                if (m_map[i, k].fieldtype == MapData.FieldTypeID.CanNotStand)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    Gizmos.DrawCube(new Vector3(i + 0.5f, height, k + 0.5f), new Vector3(1, height + 0.1f, 1));
                }
            }
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
