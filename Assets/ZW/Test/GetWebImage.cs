using UnityEngine;
using System.Collections;

public class GetWebImage : MonoBehaviour
{

    string url = "http://img.hb.aicdn.com/240136a8caf6ae05d38f2f57d596aec10c44d1ff112df-4XaoQJ_fw580";
    public Material material;

    IEnumerator Start()
    {
        WWW www = new WWW(url);
        yield return www;
        material = new Material(Shader.Find("Unlit/Transparent Cutout"));
        material.mainTexture = www.texture;

        Split(4, 4);
    }

    /// <summary>
    /// 分割图片
    /// </summary>
    void Split(int width, int height)
    {

        float x = -400;
        float y = -200;

        for (int i = 0; i < width; i++)
        {
            for (int n = 0; n < height; n++)
            {
                GameObject obj = CreateObj();
                SetUv(obj.GetComponent<MeshFilter>().mesh);
                obj.transform.localScale = new Vector3(material.mainTexture.width / width, material.mainTexture.height / height, 1);

                x += 100;
                obj.transform.parent = transform.parent;
                obj.transform.localPosition = new Vector3(x, y, 1);
            }
            x = -400;
            y += 200;
        }

    }

    /// <summary>
    /// 创建对象
    /// </summary>
    private GameObject CreateObj()
    {
        GameObject obj = new GameObject();
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        return obj;
    }

    /// <summary>
    /// 贴图
    /// </summary>
    void SetUv(Mesh mesh)
    {
        //定点坐标 
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, 0, 0);

        mesh.vertices = vertices;

        //三角形连线 
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;

        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.triangles = triangles;

        //设置uv坐标
        Vector2 uvPostion0 = new Vector2(0, 0);
        Vector2 uvPostion1 = new Vector2(0, 0.5f);
        Vector2 uvPostion2 = new Vector2(0.5f, 0.5f);
        Vector2 uvPostion3 = new Vector2(0.5f, 0);

        mesh.uv = new Vector2[] { uvPostion0, uvPostion1, uvPostion2, uvPostion3 };
    }

}