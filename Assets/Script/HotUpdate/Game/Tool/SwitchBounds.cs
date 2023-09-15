using Cinemachine;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    Cinemachine边界工具
-----------------------*/

namespace MyFrameworkCore
{
    public class SwitchBounds : MonoBehaviour
    {
        private void OnEnable()
        {
            ConfigEvent.SwichConfinerShape.AddEventListener(SwitchConfinerShape);
        }
        private void OnDestroy()
        {
            ConfigEvent.SwichConfinerShape.RemoveEventListener(SwitchConfinerShape);
        }
        /// <summary> 切换场景的时候找到限定范围的组件 </summary>
        private void SwitchConfinerShape()
        {
            PolygonCollider2D polygonCollider = GameObject.FindGameObjectWithTag(ConfigTag.TagBoundsConfiner).GetComponent<PolygonCollider2D>();
            CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
            cinemachineConfiner.m_BoundingShape2D = polygonCollider;
            //如果边界形状的点在运行时改变，调用这个函数
            cinemachineConfiner.InvalidatePathCache();
        }
    }
}
