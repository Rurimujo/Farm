using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    数据结构类
-----------------------*/
namespace MyFrameworkCore
{
    public class ClassData
    {
        private static string DATA_CLASS_PATH = $"{Application.dataPath}/Script/HotUpdate/Game/Config/ExcelClass/";//Assets/HotUpdate/GameMain/Config

        /// <summary>
        /// 通过Excel数据生成脚本文件
        /// </summary>
        public static void CreateScript(string filePath, string[][] data)
        {
            StringBuilder sb = new StringBuilder();
            string className = new FileInfo(filePath).Name.Split('.')[0];
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using MyFrameworkCore;");
            sb.AppendLine("using System;\n\t");
            sb.AppendLine($"[Serializable]");
            sb.AppendLine($"public class {className} : IData");
            sb.AppendLine("{");
            string[] filedTypeArray = data[(int)RowType.FIELD_TYPE];
            string[] filedNameArray = data[(int)RowType.FIELD_NAME];
            for (int i = 0; i < filedTypeArray.Length; ++i)
            {
                sb.AppendLine($"\tpublic {filedTypeArray[i].PadRight(10, ' ')}\t{filedNameArray[i]};");
            }
            sb.AppendLine($"    public int GetId()\r\n    {{\r\n\t\treturn {data[0][0]};\r\n    }}");
            sb.AppendLine("}");
            DATA_CLASS_PATH.GenerateDirectory();
            string path = $"{DATA_CLASS_PATH}/{className}.cs";
            File.Delete(path);
            File.WriteAllText(path, sb.ToString());
            AssetDatabase.Refresh();
        }
    }
}
