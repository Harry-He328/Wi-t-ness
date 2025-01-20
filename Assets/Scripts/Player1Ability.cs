using UnityEngine;

public class Player1Ability : MonoBehaviour
{
    public Transform playerTransform; // 角色的Transform组件
    public GameObject pickedObject;  // 当前被吸附的物体
    public LayerMask objectLayer;     // 物体所在的图层
    public KeyCode pickKeyCode = KeyCode.X; // 吸附物体的按键
    public float pickUpHeight = 1.0f; // 吸附时物体相对于角色的垂直偏移量
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
                    pickedObject.transform.localPosition = new Vector3(0, pickUpHeight, 0);
                }
            }
            else if (pickedObject != null)
            {
                //将吸附的物体放下
                //Vector3 dropPosition = playerTransform.position + new Vector3(0, pickUpHeight, 0);
                //pickedObject.transform.position = dropPosition;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }

    // 绘制射线
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