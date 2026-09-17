using UnityEngine;
using System.IO;

public class ObjLoader : MonoBehaviour
{
    public string objFilePath = "Assets/Models/cube.obj";

    [Header("生成结果")]
    public GameObject modelGo;
    public ObjParser parser;

    void Start()
    {
        parser = new ObjParser();

        string objText = File.ReadAllText(objFilePath);
        parser.Parse(objText);

        Mesh mesh = parser.GetMesh();

        modelGo = new GameObject("LoadedOBJModel");
        modelGo.transform.localScale = Vector3.one;

        MeshFilter mf = modelGo.AddComponent<MeshFilter>();
        MeshRenderer mr = modelGo.AddComponent<MeshRenderer>();

        mf.mesh = mesh;

        // 使用Unlit双面Shader，保证所有面片都可见
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.SetColor("_Color", Color.gray);
        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        mr.material = mat;

        Debug.Log($"OBJ模型总顶点数量：{parser.vertices.Count}");
        if (parser.vertices.Count > 0)
        {
            Debug.Log($"第0号顶点坐标：{parser.vertices[0]}");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (parser == null || modelGo == null) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < parser.vertices.Count; i++)
        {
            Vector3 worldPos = modelGo.transform.TransformPoint(parser.vertices[i]);
            Gizmos.DrawSphere(worldPos, 0.05f);
        }
    }
}
