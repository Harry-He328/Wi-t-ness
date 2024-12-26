using UnityEngine;

public class Player1Ability : MonoBehaviour
{
    public Transform playerTransform; // 角色的Transform组件
    public GameObject pickedObject;  // 当前被吸附的物体
    public LayerMask objectLayer;     // 物体所在的图层
    public float pickDistance = 1.0f; // 吸附距离
    public KeyCode pickKeyCode = KeyCode.X; // 吸附物体的按键

    void Update()
    {
        // 检查是否按下了吸附按键，并且角色附近有物体
        if (Input.GetKeyDown(pickKeyCode))
        {
            Collider hitCollider = null;
            Vector3 direction = Vector3.forward * pickDistance; // 向上查找物体
            Ray ray = new Ray(playerTransform.position, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, pickDistance, objectLayer))
            {
                hitCollider = hitInfo.collider;
            }

            // 处理吸附和放下逻辑
            if (hitCollider != null)
            {
                if (pickedObject == null)
                {
                    // 吸附物体
                    pickedObject = hitCollider.gameObject;
                    pickedObject.transform.SetParent(playerTransform, false); // 将物体设为角色的子物体
                    pickedObject.transform.localPosition = Vector3.up * 2f; // 调整物体位置到头顶
                }
                else if (pickedObject == hitCollider.gameObject)
                {
                    // 放下物体
                    pickedObject.transform.SetParent(null); // 取消父级关系
                    pickedObject = null; // 重置当前吸附的物体
                }
            }
            else if (pickedObject != null)
            {
                // 如果当前有吸附的物体但附近没有可吸附的物体，则放下物体
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }
}

//射线检测四个方向？还可以让真相原地转的时候不消耗行动点数，放下物体的时候放到前方
