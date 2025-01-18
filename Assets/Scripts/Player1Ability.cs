using UnityEngine;

public class Player1Ability : MonoBehaviour
{
    public Transform playerTransform; // 角色的Transform组件
    public GameObject pickedObject;  // 当前被吸附的物体
    public LayerMask objectLayer;     // 物体所在的图层
    public KeyCode pickKeyCode = KeyCode.X; // 吸附物体的按键
    public float pickUpHeight = 3.0f; // 吸附时物体相对于角色的垂直偏移量
    public const float pickDistance = 100.0f;

    void Update()
    {
        PickOrDropObject();
    }

    private void PickOrDropObject()
    {
        // 检查是否按下了吸附按键
        if (Input.GetKeyDown(pickKeyCode))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // 使用非常大的距离值进行射线检测
            if (Physics.Raycast(ray, out hitInfo, pickDistance, objectLayer))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                if (pickedObject == null)
                {
                    // 如果当前没有吸附物体，则吸附鼠标指向的物体
                    pickedObject = hitObject;
                    pickedObject.transform.SetParent(playerTransform);
                    pickedObject.transform.localPosition = new Vector3(0, 0, -pickUpHeight);
                }
                else if (pickedObject == hitObject)
                {
                    // 如果当前吸附的物体就是鼠标指向的物体，则将其放置到鼠标指向的位置
                    Vector3 mousePositionWorld = ray.GetPoint(hitInfo.distance);
                    pickedObject.transform.position = mousePositionWorld;
                    pickedObject.transform.SetParent(null);
                    pickedObject = null;
                }
                else
                {
                    if (pickedObject != null)
                    {
                        Vector3 dropPosition = playerTransform.position + new Vector3(0, 0, pickUpHeight); // 临时放置位置
                        pickedObject.transform.position = dropPosition;
                        pickedObject.transform.SetParent(null);
                        pickedObject = null;
                    }
                }
            }
            else if (pickedObject != null)
            {
                // 如果没有检测到鼠标指向任何物体，但当前有吸附的物体，则将其放置回角色身边（或者一个默认位置）
                // 这里我们选择将其放置回角色上方（可选）
                Vector3 dropPosition = playerTransform.position + new Vector3(0, 0, pickUpHeight); // 临时放置位置
                pickedObject.transform.position = dropPosition;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }

    // 使用Gizmos在Scene视图中绘制射线
    private void OnDrawGizmos()
    {
        // 确保在Scene视图中只有在选择了包含这个脚本的GameObject时，才绘制Gizmos
        if (!gameObject.activeInHierarchy || !enabled) return;

        // 获取主相机的位置和鼠标位置
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 mousePosition = Input.mousePosition;

        // 将屏幕坐标转换为世界坐标射线
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // 为了在Gizmos中模拟“无限远”，我们选择一个非常大的值
        Vector3 rayEndPosition = ray.GetPoint(pickDistance);

        // 绘制射线
        Gizmos.color = Color.red; // 设置射线颜色
        Gizmos.DrawRay(cameraPosition, rayEndPosition - cameraPosition);
    }
}