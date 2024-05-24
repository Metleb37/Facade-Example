using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FacadeExample : MonoBehaviour
{
    [SerializeField] GameObject dot;
    [SerializeField] Mesh mesh;
    [SerializeField] Vector3[] vertices;
    [SerializeField] List<Vector3> first_vertices = new List<Vector3>();
    [SerializeField] List<Vector3> dot_points = new List<Vector3>();
    [SerializeField] List<GameObject> dot_objects = new List<GameObject>();
    [SerializeField] WholeDots wholeDots = new WholeDots();
    [SerializeField] WholeDotPointPositions wholeDotPointPositions = new WholeDotPointPositions();
    [SerializeField] WholeDotPointPositions firstPositions = new WholeDotPointPositions();
    bool is_shaking;
    int shaker_value;

    [SerializeField] Material red_material, white_material;

    // 0 - 0.635 - 1.27
    private void Start()
    {
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            first_vertices.Add(vertices[i]);
        }
        create_dots();
    }

    void create_dots()
    {
        for (int i = 0;i < vertices.Length; i++)
        {
            GameObject new_dot = Instantiate(dot);
            new_dot.GetComponent<MoveVertex>().SetDot(i, mesh,vertices);
            new_dot.transform.position = vertices[i];

            if (dot_points.Contains(new_dot.transform.position))
            {
                for(int j = 0; j < dot_points.Count; j++)
                {
                    if (dot_points[j] == new_dot.transform.position)
                    {
                        new_dot.transform.SetParent(dot_objects[j].transform);
                        Destroy(new_dot.GetComponent<SphereCollider>());
                        Destroy(new_dot.GetComponent<MeshRenderer>());
                        Destroy(new_dot.GetComponent<MeshFilter>());
                    }
                }
            }
            else
            {
                dot_objects.Add(new_dot);
                dot_points.Add(new_dot.transform.position);
            }
        }

        for (int i = 0; i < dot_objects.Count; i++)
        {
            if (dot_objects[i].transform.position.z == 0)
            {
                wholeDots.dot_parts[0].dots.Add(dot_objects[i]);
                wholeDotPointPositions.dot_parts[0].dots.Add(dot_objects[i].transform.position);
            }
            else if(dot_objects[i].transform.position.z == 0.635f)
            {
                wholeDots.dot_parts[1].dots.Add(dot_objects[i]);
                wholeDotPointPositions.dot_parts[1].dots.Add(dot_objects[i].transform.position);
            }
            else if (dot_objects[i].transform.position.z == 1.27f)
            {
                wholeDots.dot_parts[2].dots.Add(dot_objects[i]);
                wholeDotPointPositions.dot_parts[2].dots.Add(dot_objects[i].transform.position);
            }
        }

        for (int i = 0;i < wholeDotPointPositions.dot_parts.Count;i++)
        {
            for (int j = 0;j < wholeDotPointPositions.dot_parts[i].dots.Count;j++)
            {
                firstPositions.dot_parts[i].dots.Add(wholeDotPointPositions.dot_parts[i].dots[j]);
            }
        }
    }

    void create_dots_part_2()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject new_dot = Instantiate(dot);
            new_dot.GetComponent<MoveVertex>().SetDot(i, mesh, vertices);
            new_dot.transform.position = vertices[i];

            if (dot_points.Contains(new_dot.transform.position))
            {
                for (int j = 0; j < dot_points.Count; j++)
                {
                    if (dot_points[j] == new_dot.transform.position)
                    {
                        new_dot.transform.SetParent(dot_objects[j].transform);
                        Destroy(new_dot.GetComponent<SphereCollider>());
                        Destroy(new_dot.GetComponent<MeshRenderer>());
                        Destroy(new_dot.GetComponent<MeshFilter>());
                    }
                }
            }
            else
            {
                dot_objects.Add(new_dot);
                dot_points.Add(new_dot.transform.position);
            }
        }

        for (int i = 0; i < dot_objects.Count; i++)
        {
            if (dot_objects[i].transform.position.z == 0)
            {
                
                wholeDots.dot_parts[0].dots[i] = dot_objects[i];
                wholeDotPointPositions.dot_parts[0].dots[i] = dot_objects[i].transform.position;
            }
            else if (dot_objects[i].transform.position.z == 0.635f)
            {   
                wholeDots.dot_parts[1].dots[i - 32] = dot_objects[i];
                wholeDotPointPositions.dot_parts[1].dots[i - 32] = dot_objects[i].transform.position;
            }
            else if (dot_objects[i].transform.position.z == 1.27f)
            {
                wholeDots.dot_parts[2].dots[i- 16] = dot_objects[i];
                wholeDotPointPositions.dot_parts[2].dots[i - 16] = dot_objects[i].transform.position;
            }
        }


        for (int i = 0; i < wholeDotPointPositions.dot_parts.Count; i++)
        {
            for (int j = 0; j < wholeDotPointPositions.dot_parts[i].dots.Count; j++)
            {
                firstPositions.dot_parts[i].dots.Add(wholeDotPointPositions.dot_parts[i].dots[j]);
            }
        }
    }

    public void quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (is_shaking)
        {
            SHAKE(shaker_value);
        }
    }

    public void shake_control(int value)
    {
        is_shaking = true;
        shaker_value = value;

        for (int i = 0; i < wholeDots.dot_parts.Count; i++)
        {
            for (int j = 0; j < wholeDots.dot_parts[i].dots.Count; j++)
            {
                wholeDots.dot_parts[i].dots[j].GetComponent<Renderer>().material = white_material;
            }
        }

        for (int i = 0; i < wholeDots.dot_parts[value].dots.Count; i++)
        {
            wholeDots.dot_parts[value].dots[i].GetComponent<Renderer>().material = red_material;
        }
    }

    public void SHAKE(int value)
    {
        for (int i = 0;i < wholeDots.dot_parts[value].dots.Count;i++)
        {
            if (wholeDotPointPositions.dot_parts[value].dots[i] == wholeDots.dot_parts[value].dots[i].transform.position)
            {
                wholeDotPointPositions.dot_parts[value].dots[i] = new_pos_to_go(firstPositions.dot_parts[value].dots[i]);
            }
            else
            {
               wholeDots.dot_parts[value].dots[i].transform.position = 
               Vector3.MoveTowards(wholeDots.dot_parts[value].dots[i].transform.position,
               wholeDotPointPositions.dot_parts[value].dots[i],
               Time.deltaTime);
            }
        }
    }

    Vector3 new_pos_to_go(Vector3 old_pos)
    {
        return new Vector3(
            old_pos.x + UnityEngine.Random.Range(-0.1f, 0.1f),
            old_pos.y + UnityEngine.Random.Range(-0.1f, 0.1f),
            old_pos.z + UnityEngine.Random.Range(-0.1f, 0.1f));
    }

    private void OnApplicationQuit()
    {
        reset_vertices();
    }

    public void reset_vertices()
    {
        is_shaking = false;

        for (int i = 0; i < dot_objects.Count; i++)
        {
            Destroy(dot_objects[i].gameObject);
        }

        dot_objects.Clear();
        dot_points.Clear();

        for (var i = 0; i < first_vertices.Count; i++)
        {
            vertices[i] = first_vertices[i];
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        create_dots_part_2();
    }

    public void reset_scene()
    {
        reset_vertices();
    }

}

[Serializable]
public class DotPoints
{
    public List<GameObject> dots = new List<GameObject>();
}

[Serializable]
public class WholeDots
{
    public List<DotPoints> dot_parts = new List<DotPoints>();
}

[Serializable]
public class DotPointPosition
{
    public List<Vector3> dots = new List<Vector3>();
}

[Serializable]
public class WholeDotPointPositions
{
    public List<DotPointPosition> dot_parts = new List<DotPointPosition>();
}