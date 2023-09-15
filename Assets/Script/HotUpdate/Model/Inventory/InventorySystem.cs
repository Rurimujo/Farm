using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    物品管理系统
-----------------------*/

namespace MyFrameworkCore
{
    public class InventorySystem : SingletonInit<InventorySystem>, ICore
    {
        public Dictionary<string, List<InventoryItem>> ItemDicList; //物品字典列表 两本字典的KEY请不要重复
        public Dictionary<string, InventoryItem[]> ItemDicArray; //物品字典数组   两本字典的KEY请不要重复

        public void ICroeInit()
        {
            ItemDicArray = new Dictionary<string, InventoryItem[]>();
            ItemDicList = new Dictionary<string, List<InventoryItem>>();
        }
        public void Init()
        {

        }

        #region ItemDicList字典操作

        /// <summary> 创建ItemDicList </summary>
        public void CreatItemDicList(string key)
        {
            ItemDicList.Add(key, new List<InventoryItem>());
        }

        /// <summary> 给字典的列表加Item </summary>
        public bool AddItemDicList(string key, Item item)//捡东西可以设置ture
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            if (itemList == null)
            {
                RDebug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicListRecord()方法创建");
                return false;
            }

            int sameIndex = GetSameItemIndexItemDicList(key, item.itemID);//存在相同物品的下标
            int spaceIndex = GetSameIndexDicList(key);//空位下标

            if(sameIndex != -1) //有相同物品
            {
                InventoryItem inventoryItem = new InventoryItem() { itemID = item.itemID, itemAmount = item.itemAmount + itemList[sameIndex].itemAmount };
                itemList[sameIndex] = inventoryItem;
            }
            else
            {
                if (spaceIndex != -1) itemList[spaceIndex] = new InventoryItem { itemID = item.itemID, itemAmount = item.itemAmount };
                else
                {
                    RDebug.Log($"{key}已满");
                    return false;
                }
            }
            return true;
            
        }

        /// <summary> 是否存在相同物品ID,有返回对应下标,没有返回-1 </summary>
        private int GetSameItemIndexItemDicList(string key, int ID)
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList?.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }

        /// <summary> 检查是否有空位,即是否有ID为0的,有返回下标,没有返回-1 </summary>
        private int GetSameIndexDicList(string key)
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> itemList);
            for (int i = 0; i < itemList.Count; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == 0)
                    return i;
            }
            return -1;
        }

        /// <summary> 获取字典对应的列表 </summary>
        public List<InventoryItem> GetItemDicList(string key)
        {
            ItemDicList.TryGetValue(key, out List<InventoryItem> inventoryItemList);
            return inventoryItemList;
        }

        #endregion

        #region ItemDicArray字典操作

        /// <summary> 创建字典数组 </summary>
        public void CreatItemDicArray(string key, int count)
        {
            ItemDicArray.Add(key, new InventoryItem[count]);
        }

        /// <summary> 给字典的数组加Item </summary>
        public bool AddItemDicArray(string key, Item item)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null)
            {
                RDebug.Error($"添加{key}失败,字典中没有包含{key}的索引,请调用CreatItemDicArrayRecord()方法创建");
                return false;
            }

            int sameIndex = GetSameItemIndexArray(key, item.itemID);   //是否存在这个物品-1 没有 其他表示有
            int spaceIndex = GetSpaceItemIndexArray(key);               //是否有空位
            if (sameIndex != -1) //有相同物品
            {
                InventoryItem inventoryItem = new InventoryItem() { itemID = item.itemID, itemAmount = item.itemAmount + inventoryItemArray[sameIndex].itemAmount };
                inventoryItemArray[sameIndex] = inventoryItem;
            }
            else
            {
                if (spaceIndex != -1) inventoryItemArray[spaceIndex] = new InventoryItem { itemID = item.itemID, itemAmount = item.itemAmount };
                else
                {
                    RDebug.Log($"{key}已满");
                    return false;
                }
            }

            //更新物品UI 呼叫事件中心,执行委托的代码
            key.EventTrigger(ItemDicArray[key]);//这里的在比如背包页面那边开启的时候监听
            return true;
        }

        /// <summary> 减少物品数量 </summary>
        public bool ReduceItemDicArray(string key, int itemID, int reduceAmount)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null)
            {
                RDebug.Log($"没找到对应数组{key}");
                return false;
            }

            int sameIndex = GetSameItemIndexArray(key, itemID);

            if (sameIndex == -1) //没有物品
            {
                RDebug.Log($"没有找到物品ID{itemID}");
                return false;
            }
            else
            {
                int amount = inventoryItemArray[sameIndex].itemAmount - reduceAmount;
                if (amount > 0)
                {
                    inventoryItemArray[sameIndex] = new InventoryItem { itemID = inventoryItemArray[sameIndex].itemID, itemAmount = amount };
                }
                else
                {
                    inventoryItemArray[sameIndex] = new InventoryItem();//移除物品
                }
            }

            key.EventTrigger(inventoryItemArray);
            return true;
        }

        /// <summary> 移除物品 </summary>
        public bool RemoveItemDicArray(string key, int itemID)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            if (inventoryItemArray == null)
            {
                RDebug.Log($"没找到对应数组{key}");
                return false;
            }
            int sameIndex = GetSameItemIndexArray(key, itemID);
            if (sameIndex == -1) //没有物品
            {
                RDebug.Log($"没有找到物品ID{itemID}");
                return false;
            }
            else
            {
                inventoryItemArray[sameIndex] = new InventoryItem();//移除物品
            }

            key.EventTrigger(inventoryItemArray);
            return true;
        }

        /// <summary> 交换物品 </summary>
        public bool ChangeItemDicArray(string oldKey, string newKey, int oldIndex, int newIndex)
        {
            ItemDicArray.TryGetValue(oldKey, out InventoryItem[] oldInventoryItemArray);
            if (oldInventoryItemArray == null)
            {
                RDebug.Error($"老的{oldKey}是空的,请先创建");
                return false;
            }
            ItemDicArray.TryGetValue(newKey, out InventoryItem[] newInventoryItemArray);
            if (newInventoryItemArray == null)
            {
                RDebug.Error($"新的{oldKey}是空的,请先创建");
                return false;
            }
            InventoryItem currentItem = oldInventoryItemArray[oldIndex];
            InventoryItem targetItem = newInventoryItemArray[newIndex];
            //数据交换
            if (targetItem.itemID != 0)//交换的目标有物品的情况下
            {
                oldInventoryItemArray[oldIndex] = targetItem;
                newInventoryItemArray[newIndex] = currentItem;
            }
            else
            {
                oldInventoryItemArray[oldIndex] = targetItem;
                newInventoryItemArray[newIndex] = currentItem;// new InventoryItem();
            }

            //if (oldInventoryItemArray[oldIndex].itemID == newInventoryItemArray[newIndex].itemID)//说明拖拽放下的是同一个物体
            //{
            //    oldInventoryItemArray[oldIndex].itemAmount += newInventoryItemArray[newIndex].itemAmount;
            //    newInventoryItemArray[newIndex].itemID = 0;
            //    newInventoryItemArray[newIndex].itemAmount = 0;
            //}
            oldKey.EventTrigger(ItemDicArray[oldKey]);//这里的在比如背包页面那边开启的时候监听
            newKey.EventTrigger(ItemDicArray[newKey]);//这里的在比如背包页面那边开启的时候监听
            return true;
        }

        /// <summary> 获取字典对应的数组 </summary>
        public InventoryItem[] GetItemDicArray(string key)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] inventoryItemArray);
            return inventoryItemArray;
        }

        /// <summary> 是否存在相同物品ID,有返回对应下标,没有返回-1 </summary>
        private int GetSameItemIndexArray(string key, int ID)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] itemList);
            for (int i = 0; i < itemList?.Length; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == ID)
                    return i;
            }
            return -1;
        }

        /// <summary> 检查是否有空位,即是否有ID为0的,有返回下标,没有返回-1 </summary>
        private int GetSpaceItemIndexArray(string key)
        {
            ItemDicArray.TryGetValue(key, out InventoryItem[] itemList);
            for (int i = 0; i < itemList?.Length; i++)
            {
                InventoryItem inventoryItem = itemList[i];
                if (inventoryItem.itemID == 0)
                    return i;
            }
            return -1;
        }

        #endregion

        public ItemDetails GetItem(int id)
        {
            return id.GetDataOne<ItemDetails>();
        }
    }
}