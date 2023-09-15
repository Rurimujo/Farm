using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    数据管理加载
-----------------------*/

namespace MyFrameworkCore
{
    public class DataManager :SingletonInit<DataManager>, ICore
    {
        private Dictionary<string, List<IData>> bytesDataDic;//数据

        public void ICroeInit()
        {
            bytesDataDic = new Dictionary<string, List<IData>>();
        }
        public void Init()
        {
            //加载数据
            InitData<ItemDetails>(ConfigBytes.BytesItemDetails);
            RDebug.Log("数据初始化完毕");
        }

        public void InitData<T>(string fileName) where T : IData
        {
            RawFileOperationHandle handle = YooAssetLoadExpsion.YooaddetLoadRawFileAsync(fileName);
            byte[] fileData = handle.GetRawFileData();
            List<IData> itemDetailsList = BinaryAnalysis.GetData<T>(fileData);
            if (bytesDataDic.ContainsKey(typeof(T).FullName))
                bytesDataDic[typeof(T).FullName] = itemDetailsList;
            bytesDataDic.Add(typeof(T).FullName, itemDetailsList);
        }


        public T GetDataOne<T>(int id) where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName)) return null;
            IData data = bytesDataDic[typeof(T).FullName].Find(data => { return data.GetId() == id; });
            return data == null ? null : data as T;
        }
        public List<IData> GetDataList<T>() where T : class, IData
        {
            if (!bytesDataDic.ContainsKey(typeof(T).FullName)) return null;
            List<IData> dataListTemp = bytesDataDic[typeof(T).FullName];
            return dataListTemp;
        }
    }
}
