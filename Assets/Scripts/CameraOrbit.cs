using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Tooltip("环绕中心点，新建空物体放在模型中心")]
    public Transform target;
    public float distance = 5f;
    public float rotateSpeed = 100f;
    public float zoomSpeed = 2f;

    private float rotX;
    private float rotY;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 鼠标左键拖拽旋转视角
        if (Input.GetMouseButton(0))
        {
            rotX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rotY -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            rotY = Mathf.Clamp(rotY, -80, 80);
        }

        // 滚轮缩放
        distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, 1f, 20f);

        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
        Vector3 pos = target.position + rotation * new Vector3(0, 0, -distance);
        transform.rotation = rotation;
        transform.position = pos;
    }
}
