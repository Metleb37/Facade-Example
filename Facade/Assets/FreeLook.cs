using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeLook : MonoBehaviour
{
    public Transform Target;
    public float distance;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;

    private float x = 0.0f;
    private float y = 0.0f;
    public float CameraDist = 10;
    public Slider slider;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x + 50;
        y = angles.y;
        distance = 30;

        if (this.GetComponent<Rigidbody>() == true)
            GetComponent<Rigidbody>().freezeRotation = true;

        update_cam();
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Target)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y += Input.GetAxis("Mouse Y") * ySpeed * 0.05f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 position = rotation * new Vector3(0, 0, -distance) + Target.position;

                transform.rotation = rotation;
                transform.position = position;
                distance = CameraDist;

                if (Input.GetKey(KeyCode.W))
                {
                    CameraDist -= Time.deltaTime * 6.5f;
                    CameraDist = Mathf.Clamp(CameraDist, 2, 60);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    CameraDist += Time.deltaTime * 6.5f;
                    CameraDist = Mathf.Clamp(CameraDist, 2, 60);
                }
            }
        }

        //if (Input.mouseScrollDelta.y >= 0.5f)
        //{
        //    CameraDist -= Time.deltaTime * CameraDist * 50;
        //    update_cam();
        //}
        //else if(Input.mouseScrollDelta.y <= -0.5f)
        //{
        //    CameraDist += Time.deltaTime * CameraDist * 50;
        //    update_cam();
        //}
    }

    public void update_cam()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y += Input.GetAxis("Mouse Y") * ySpeed * 0.05f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + Target.position;

        transform.rotation = rotation;
        transform.position = position;
        distance = slider.value;
        CameraDist = slider.value;
    }

    float ClampAngle(float ag, float min, float max)
    {
        if (ag < -360)
            ag += 360;
        if (ag > 360)
            ag -= 360;
        return Mathf.Clamp(ag, min, max);
    }
}
