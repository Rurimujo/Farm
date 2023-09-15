
/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    日志接口
-----------------------*/

namespace MyFrameworkCore
{
    public interface ILogger
    {
        public void Log(string msg, LogCoLor LogCoLor = LogCoLor.None);//普通信息
        public void Warn(string msg);                                  //警告
        public void Error(string msg);                                 //异常错误
    }
}
