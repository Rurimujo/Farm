using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    Excel转换工具
-----------------------*/



namespace MyFrameworkCore
{
    /// <summary>
    /// 每行类型
    /// </summary>
    public enum RowType : byte
    {
        FIELD_NAME = 0,//名称
        FIELD_TYPE = 1,//类型
        BEGIN_INDEX = 3//开始行数
    }

    public class ExcelChange : EditorWindow
    {
        /// <summary>
        /// excel文件存放的路径
        /// </summary>
        public static string EXCEL_PATH = Application.dataPath + "/Editor/Excels/";


        [MenuItem("Tool/Excel转换")]//#E
        public static void GenerateExcelInfo()
        {
            IEnumerable<string> paths = Directory.EnumerateFiles(EXCEL_PATH, "*.xlsx");
            foreach (string filePath in paths)
            {
                //读取Excel数据
                string[][] data = filePath.LoadExcel();
                //生成C#文件
                ClassData.CreateScript(filePath, data);
                //生成二进制文件
                BinaryData.CreateByte(filePath, data);
            }
            //刷新Project窗口
            AssetDatabase.Refresh();
        }

        

    }
}
