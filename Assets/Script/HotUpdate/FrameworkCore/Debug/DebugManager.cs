using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    日志模块
-----------------------*/



namespace MyFrameworkCore
{
    public class DebugManager : SingletonInit<DebugManager>,ICore
    {
        public void ICroeInit()
        {
            //主动日志
            RDebug.InitSettings(new LogConfig()
            {
                enableSave = true,
                loggerType = LoggerType.Unity,
#if !UNITY_EDITOR
                //savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
#endif
                savePath = $"{Application.dataPath}/LogOut/ActiveLog/",
                saveName = "Debug主动输出日志.txt",
            });
        }
        public void Init()
        {
            RDebug.Log("日志模块初始化完毕!");
        }
    }
}
