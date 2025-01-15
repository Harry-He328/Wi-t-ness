using UnityEngine;

public class Player1Ability : MonoBehaviour
{
    public Transform playerTransform; // 角色的Transform组件
    public GameObject pickedObject;  // 当前被吸附的物体
    public LayerMask objectLayer;     // 物体所在的图层
    public float pickDistance = 1.0f; // 吸附距离
    public KeyCode pickKeyCode = KeyCode.X; // 吸附物体的按键
    public float pickUpOffset = 2.0f; // 吸附时物体相对于角色的垂直偏移量
    public float dropOffset = 2.0f;   // 放下时物体相对于角色的前方偏移量
    
    private Vector3[] directions = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    void Update()
    {
       PickCube();
    }


    private void PickCube()
    {
        // 检查是否按下了吸附按键
        if (Input.GetKeyDown(pickKeyCode))
        {
            Collider hitCollider = null;
            RaycastHit hitInfo;
 
            // 向前方发射一条射线
            Vector3 forwardDirection = playerTransform.forward;
            Ray ray = new Ray(playerTransform.position, forwardDirection);
 
            if (Physics.Raycast(ray, out hitInfo, pickDistance, objectLayer))
            {
                hitCollider = hitInfo.collider;
            }
 
            // 处理吸附和放下逻辑
            if (hitCollider != null)
            {
                GameObject hitObject = hitCollider.gameObject;
 
                if (pickedObject == null)
                {
                    pickedObject = hitObject;
                    // 将物体设为角色的子物体，并设置其位置
                    pickedObject.transform.SetParent(playerTransform, false);
                    pickedObject.transform.localPosition = new Vector3(0, pickUpOffset, 0);
                }
                else if (pickedObject == hitObject)
                {
                    // 如果当前吸附的物体就是刚刚检测到的物体，则将其放下
                    Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropOffset;
                    pickedObject.transform.position = dropPosition;
                    pickedObject.transform.SetParent(null); 
                    pickedObject = null;
                }

            }
            else if (pickedObject != null)
            {
                // 如果没有检测到任何物体，但当前有吸附的物体，则将其放下
                Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropOffset;
                pickedObject.transform.position = dropPosition;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }
}