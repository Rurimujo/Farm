using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    UI基类
-----------------------*/


namespace MyFrameworkCore
{
    public class UIBase
    {
        public EUIType type = EUIType.Normal;               //窗口的位置
        public EUIMode mode = EUIMode.Normal;               //窗口显示类型

        public string UIName { get; set; }                        //UI的名称
        public GameObject panelGameObject { get; set; }                //窗口的物体

        /// <summary>初始化方法</summary>
        /// <param name="type">窗口的位置</param>
        /// <param name="mod">窗口显示类型</param>
        /// <param name="lucenyType">窗口的透明度</param>
        protected void InitUIBase(EUIType type, EUIMode mod)
        {
            this.type = type;
            this.mode = mod;
        }

        //生命周期
        public virtual void UIAwake()
        {

        }       //初始化执行
        public virtual void UIUpdate()
        {

        }      //轮询执行
        public virtual void UIOnEnable()
        {
            this.panelGameObject.SetActive(true);
        }    //开启执行
        public virtual void UIOnDisable()
        {
            this.panelGameObject.SetActive(false);
        }   //关闭执行
        public virtual void UIOnDestroy() { }   //销毁执行

        //面板操作
        protected void OpenUIForm<T>(string uiFormName) where T : UIBase, new()
        {
            UIManager.Instance.ShwoUIPanel<T>(uiFormName);
        }
        protected void GetUIForm<T>(string uiFormName) where T : UIBase, new()
        {
            UIManager.Instance.GetUIPanl<T>(uiFormName);
        }
        protected void CloseUIForm()
        {
            int intPosition = -1;
            string strUIFromName = UIName;  // GetType().ToString().Replace("Panel","");             //命名空间+类名 //处理后的UIFrom 名称
            intPosition = strUIFromName.IndexOf('.');
            if (intPosition != -1)
                strUIFromName = strUIFromName.Substring(intPosition + 1);//剪切字符串中“.”之间的部分
            //RDebug.Log($"关闭的界面名称是:{strUIFromName}");
            UIManager.Instance.CloseUIForms(strUIFromName);
        }
        protected void CloseOtherUIForm(string uiFormName)
        {
            UIManager.Instance.CloseUIForms(uiFormName);
        }

        //事件推送

        protected void ButtonOnClickAddListener(GameObject goButton, EventTriggerListener.VoidDelegate delHandle)
        {
            //给按钮注册事件方法
            if (goButton != null)
                EventTriggerListener.Get(goButton).onClick = delHandle;
        }


    }
}
