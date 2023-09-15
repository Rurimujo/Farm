using System.Collections.Generic;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    数据管理扩展
-----------------------*/

namespace MyFrameworkCore
{
    public static class DataExpansion
    {
        public static T GetDataOne<T>(this int id) where T : class, IData
        {
            return DataManager.Instance.GetDataOne<T>(id);
        }
        public static List<IData> GetDataList<T>(this object obj) where T : class, IData
        {
            return DataManager.Instance.GetDataList<T>();
        }
    }
}
