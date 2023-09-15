
/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    UI配置
-----------------------*/


namespace MyFrameworkCore
{
    /// <summary> UI窗体（位置）类型 </summary>
    public enum EUIType
    {
        /// <summary> 普通窗体 </summary>
        Normal,
        /// <summary> 独立的窗口可移动的 </summary> TODO 等待敲代码
        Mobile,
        /// <summary> 渐变过度窗体 </summary>
        Fade,
    }

    /// <summary> UI窗体的显示类型 </summary>
    public enum EUIMode
    {
        /// <summary> 普通 模式允许多个窗体同时显示 </summary>
        Normal,
        /// <summary> 隐藏其他 一般应用于全局性的窗体 </summary>
        HideOther,
    }

}
