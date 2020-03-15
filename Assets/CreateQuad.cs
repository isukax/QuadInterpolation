using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateQuad : MonoBehaviour
{
    public bool doExtraInterpolation;
    public Color mColor;

    Vector4 extraColorData;

    Vector4 ToV4(Color col)
    {
        return new Vector4(col.r, col.b, col.b, col.a);
    }

    void Update()
    {
        Mesh mesh = new Mesh();
        
        mesh.vertices = new Vector3[]
        {
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, 1, 0),
        };

        mesh.triangles = new[] { 0, 2, 1, 0, 3, 2 };

        // Diagonal
        mesh.colors = new Color[]
        {
            mColor,
            //new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 1),
            new Color(1, 0, 0, 1),
            new Color(1, 1, 1, 1),
        };

        //mesh.colors = new Color[]
        //{
        //    //new Color(1, 1, 1, 1),
        //    new Color(1, 1, 1, 1),
        //    mColor,
        //    new Color(1, 1, 1, 1),
        //    new Color(1, 0, 0, 1),
        //};

        // Calculate extra data for interpolation
        extraColorData = -ToV4(mesh.colors[0]) + ToV4(mesh.colors[1]) - ToV4(mesh.colors[2]) + ToV4(mesh.colors[3]);

        // Set barycentric per vertex
        mesh.uv2 = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GetComponent<MeshFilter>().mesh = mesh;

        Material mat = GetComponent<Renderer>().material;
        mat.SetVector("_ExtraColorData", extraColorData);
        if (doExtraInterpolation)
        {
            mat.EnableKeyword("DO_INTERP");
        }
        else
        {
            mat.DisableKeyword("DO_INTERP");
        }
    }
}
