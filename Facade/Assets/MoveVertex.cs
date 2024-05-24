using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVertex : MonoBehaviour
{
    public Vector3[] vertices;
    public int vertex_number;
    public Mesh mesh;

    Vector3 offset;
    float mouseZ;

    public void SetDot(int vertex_number_,Mesh mesh_, Vector3[] vector3_)
    {
        vertex_number = vertex_number_;
        mesh = mesh_;
        vertices = vector3_;
    }

    public void Update()
    {
        if (mesh != null )
        {
            vertices[vertex_number] = transform.position;
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    private void OnMouseDown()
    {
        mouseZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; 
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZ;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
