using UnityEngine;

public class Player1Ability : MonoBehaviour
{
    public Transform playerTransform; // 角色的Transform组件
    public GameObject pickedObject;  // 当前被吸附的物体
    public LayerMask objectLayer;     // 物体所在的图层
    public float pickDistance = 1.0f; // 吸附距离
    public KeyCode pickKeyCode = KeyCode.X; // 吸附物体的按键
    public float pickUpOffset = 1.0f; // 吸附时物体相对于角色的垂直偏移量
    public float dropOffset = 2.0f;   // 放下时物体相对于角色的前方偏移量

    // 方向向量，分别表示前、后、左、右
    private Vector3[] directions = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    void Update()
    {
        // 检查是否按下了吸附按键
        if (Input.GetKeyDown(pickKeyCode))
        {
            Collider closestHitCollider = null;
            float closestDistance = float.MaxValue;

            // 对每个方向进行射线检测
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 direction = directions[i];
                Ray ray = new Ray(playerTransform.position, direction);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, pickDistance, objectLayer))
                {
                    // 如果击中的物体在指定距离内
                    if (Vector3.Dot(hitInfo.point - playerTransform.position, direction.normalized) >= 0 &&
                        Vector3.Distance(hitInfo.point, playerTransform.position) <= pickDistance)
                    {
                        // 如果这是离角色最近的物体，或者如果之前没有检测到任何物体
                        if (Vector3.Distance(hitInfo.point, playerTransform.position) < closestDistance || closestHitCollider == null)
                        {
                            closestHitCollider = hitInfo.collider;
                            closestDistance = Vector3.Distance(hitInfo.point, playerTransform.position);
                        }
                    }
                }
            }

            // 处理吸附和放下逻辑
            if (closestHitCollider != null)
            {
                GameObject hitObject = closestHitCollider.gameObject;

                if (pickedObject == null)
                {
                    // 吸附物体
                    pickedObject = hitObject;
                    // 将物体设为角色的子物体，并设置其位置
                    pickedObject.transform.SetParent(playerTransform, false);
                    // 这里我们简单地将物体放在角色上方一个偏移量的位置
                    pickedObject.transform.localPosition = new Vector3(0, pickUpOffset, 0);
                }
                else if (pickedObject == hitObject)
                {
                    // 放下物体
                    // 使用一个固定的前方偏移量来放下物体
                    Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropOffset;
                    pickedObject.transform.position = dropPosition;
                    pickedObject.transform.SetParent(null); // 取消父级关系
                    pickedObject = null; // 重置当前吸附的物体
                }
            }
            else if (pickedObject != null)
            {
                // 如果当前有吸附的物体但附近没有可吸附的物体，则放下物体
                // 使用之前定义的前方偏移量来放下物体
                Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropOffset;
                pickedObject.transform.position = dropPosition;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }
}