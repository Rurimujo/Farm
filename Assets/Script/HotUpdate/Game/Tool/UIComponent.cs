﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

namespace MyFrameworkCore
{
    /// <summary>
    /// 使其能在Inspector面板显示，并且可以被赋予相应值
    /// </summary>
    [Serializable]
    public class ReferenceData
    {
        public string key;
        public Object gameObject;
    }

    /// <summary>
    /// 继承IComparer对比器，Ordinal会使用序号排序规则比较字符串，因为是byte级别的比较，所以准确性和性能都不错
    /// </summary>
    public class ReferenceDataComparer : IComparer<ReferenceData>
    {
        public int Compare(ReferenceData x, ReferenceData y)
        {
            return string.Compare(x.key, y.key, StringComparison.Ordinal);
        }
    }

    public class UIComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ReferenceData> data = new List<ReferenceData>();
        private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();//Object并非C#基础中的Object，而是 UnityEngine.Object

#if UNITY_EDITOR
        /// <summary>
        /// 添加新的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Add(string key, Object obj)
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            //根据PropertyPath读取数据
            //如果不知道具体的格式，可以右键用文本编辑器打开一个prefab文件（如Bundles/UI目录中的几个）
            //因为这几个prefab挂载了UIComponent，所以搜索data就能找到存储的数据
            UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
            int i;
            //遍历data，看添加的数据是否存在相同key
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            //不等于data.Count意为已经存在于data List中，直接赋值即可
            if (i != data.Count)
            {
                //根据i的值获取dataProperty，也就是data中的对应ReferenceData，不过在这里，是对Property进行的读取，有点类似json或者xml的节点
                UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
                //对对应节点进行赋值，值为gameobject相对应的fileID
                //fileID独一无二，单对单关系，其他挂载在这个gameobject上的script或组件会保存相对应的fileID
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            else
            {
                //等于则说明key在data中无对应元素，所以得向其插入新的元素
                dataProperty.InsertArrayElementAtIndex(i);
                UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("key").stringValue = key;
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            //应用与更新
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        /// <summary>
        /// 删除元素，知识点与上面的添加相似
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            if (i != data.Count)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
        public void Clear()
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            //根据PropertyPath读取prefab文件中的数据
            //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
            var dataProperty = serializedObject.FindProperty("data");
            dataProperty.ClearArray();
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
        public void Sort()
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            data.Sort(new ReferenceDataComparer());
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
#endif
        public T Get<T>(string key) where T : class
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return dictGo as T;
        }
        public T GetComponent<T>(string key) where T : Component
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return (dictGo as GameObject).GetComponent<T>();
        }
        public Object GetObject(string key)
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return dictGo;
        }

        //OnBeforeSerialize 在将要序列化的时候执行
        public void OnBeforeSerialize()
        {

        }

        //OnAfterDeserialize 在反序列化完成后执行
        public void OnAfterDeserialize()
        {
            dict.Clear();
            foreach (ReferenceData referenceData in data)
            {
                if (!dict.ContainsKey(referenceData.key))
                {
                    dict.Add(referenceData.key, referenceData.gameObject);
                }
            }
        }
    }
}