
/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    Ui面板拓展类
-----------------------*/


namespace MyFrameworkCore
{
    public static class UIManagerExpansion
    {
        public static T ShwoUIPanel<T>(this string uiFormName) where T : UIBase, new()
        {
            return UIManager.Instance.ShwoUIPanel<T>(uiFormName);
        }
        public static void CloseUIPanel(this string uiFormName)
        {
            UIManager.Instance.CloseUIForms(uiFormName);
        }
        public static T GetUIPanl<T>(this string uiFormName) where T : UIBase
        {
            return UIManager.Instance.GetUIPanl<T>(uiFormName);
        }
    }
}
