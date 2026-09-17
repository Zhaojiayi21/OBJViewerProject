using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjParser
{
    // 【作业核心】public 顶点列表，可以直接访问所有顶点
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();
    public List<int> triangles = new List<int>();

    public void Parse(string objText)
    {
        vertices.Clear();
        uvs.Clear();
        normals.Clear();
        triangles.Clear();

        List<Vector3> tempVerts = new List<Vector3>();
        List<Vector2> tempUvs = new List<Vector2>();
        List<Vector3> tempNormals = new List<Vector3>();

        string[] lines = objText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        foreach (string line in lines)
        {
            string trimLine = line.Trim();
            if (string.IsNullOrEmpty(trimLine) || trimLine.StartsWith("#"))
                continue;

            string[] parts = trimLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v":
                    tempVerts.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                    break;
                case "vt":
                    tempUvs.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                    break;
                case "vn":
                    tempNormals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                    break;
                case "f":
                    // 解析三角面三个顶点
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] indices = parts[i].Split('/');
                        int vIdx = int.Parse(indices[0]) - 1;
                        vertices.Add(tempVerts[vIdx]);

                        if (indices.Length >= 2 && !string.IsNullOrEmpty(indices[1]))
                        {
                            int uvIdx = int.Parse(indices[1]) - 1;
                            uvs.Add(tempUvs[uvIdx]);
                        }
                        else
                        {
                            uvs.Add(Vector2.zero);
                        }

                        if (indices.Length >= 3 && !string.IsNullOrEmpty(indices[2]))
                        {
                            int nIdx = int.Parse(indices[2]) - 1;
                            normals.Add(tempNormals[nIdx]);
                        }
                        else
                        {
                            normals.Add(Vector3.up);
                        }
                        triangles.Add(vertices.Count - 1);
                    }
                    break;
            }
        }
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        return mesh;
    }
}
