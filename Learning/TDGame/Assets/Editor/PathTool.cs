using UnityEngine;
using UnityEditor;
using System.Collections;

public class PathTool : ScriptableObject {
    static PathNode m_parent = null;//父路点

    //菜单SetParent，用来设置父路点，快捷键Ctrl+q
    [MenuItem("PathTool/Set Parent %q")]
    static void SetParent()
    {
        //如果没有选中任何物体，或选择物体数量大于1，返回
        if(!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
        {
            return;
        }

        //如果选中的物体Tag为pathnode
        if(Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            //保存当前选中的路点
            m_parent = Selection.activeGameObject.GetComponent<PathNode>();
        }
    }

    //菜单SetNextChild，用来设置子路点，快捷键Ctrl+w
    [MenuItem("PathTool/Set Next %w")]
    static void SetNextChild()
    {
        if (!Selection.activeGameObject || m_parent == null || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1) 
        {
            return;
        }

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent.SetNext(Selection.activeGameObject.GetComponent<PathNode>());
            m_parent = null;
        }
    }

}
