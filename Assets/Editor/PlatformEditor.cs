using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
    private Platform platform;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        platform = (Platform)target;

        if (!platform.GetNode().leftNode)
        {
            if (GUILayout.Button("Add Left Node"))
            {
                AddNode(Vector3.left);
            }
        }

        if (!platform.GetNode().rightNode)
        {
            if (GUILayout.Button("Add Right Node"))
            {
                AddNode(Vector3.right);
            }
        }

        if (!platform.GetNode().upNode)
        {
            if (GUILayout.Button("Add Up Node"))
            {
                AddNode(Vector3.up);
            }
        }

        if (!platform.GetNode().bottomNode)
        {
            if (GUILayout.Button("Add Bottom Node"))
            {
                AddNode(Vector3.down);
            }
        }

        if (platform.GetNode().platformType == Platform.PlatformNode.PlatformType.BUTTON && !platform.GetComponent<ButtonPlatform>())
        {
            platform.gameObject.AddComponent<ButtonPlatform>();
        }

        if (platform.GetNode().platformType != Platform.PlatformNode.PlatformType.BUTTON && platform.GetComponent<ButtonPlatform>())
        {
            DestroyImmediate(platform.gameObject.GetComponent<ButtonPlatform>());
        }
    }

    void AddNode(Vector3 direction)
    {
        Platform newPlatform = Instantiate(Resources.Load("Platform Node") as GameObject, platform.transform.parent).GetComponent<Platform>();
        newPlatform.transform.localPosition = platform.transform.localPosition + (direction * 2.1f);
        newPlatform.SetConnection(platform, direction * -1);
        platform.SetConnection(newPlatform, direction);

        CheckAround(newPlatform);

        EditorUtility.SetDirty(platform);
    }

    void CheckAround(Platform newPlatform)
    {
        if (!newPlatform.GetNode().rightNode) CheckPlatform(newPlatform, Vector2.right);
        if (!newPlatform.GetNode().leftNode) CheckPlatform(newPlatform, Vector2.left);
        if (!newPlatform.GetNode().upNode) CheckPlatform(newPlatform, Vector2.up);
        if (!newPlatform.GetNode().bottomNode) CheckPlatform(newPlatform, Vector2.down);
    }

    void CheckPlatform(Platform newPlatform, Vector2 direction)
    {
        Collider2D[] node = Physics2D.OverlapCircleAll( (Vector2) newPlatform.transform.position + (direction * 2.1f), 0.1f);

        if (node.Length > 0)
        {
            Platform sideNode = node[node.Length - 1].GetComponent<Platform>();
            if (sideNode != newPlatform)
            {
                newPlatform.SetConnection(sideNode, direction);
                sideNode.SetConnection(newPlatform, direction * -1);
            } 
        }
    }
}
