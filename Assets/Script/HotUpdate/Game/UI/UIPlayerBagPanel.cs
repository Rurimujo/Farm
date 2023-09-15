using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    背包面板
-----------------------*/

namespace MyFrameworkCore
{
    public class UIPlayerBagPanel : UIBase
    {
        private List<SlotUI> playerBagSlotList;
        public GameObject T_SlotGroup;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Normal, EUIMode.Normal);

            UIComponent uIComponent = panelGameObject.GetComponent<UIComponent>();
            T_SlotGroup = uIComponent.Get<GameObject>("T_SlotGroup");
            playerBagSlotList = new List<SlotUI>();

            InventorySystem.Instance.CreatItemDicArray(ConfigInventory.PalayerBag, T_SlotGroup.transform.childCount);

            for(int i = 0;i < T_SlotGroup.transform.childCount; i++)
            {
                SlotUI slotUI = T_SlotGroup.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i;
                slotUI.configInventoryKey = ConfigInventory.PalayerBag;
                playerBagSlotList.Add(slotUI);
            }
        }

        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.PalayerBag.AddEventListener<InventoryItem[]>(RefreshItem);//注册刷新事件
            RefreshItem(InventorySystem.Instance.GetItemDicArray(ConfigInventory.PalayerBag));
        }

        public override void UIOnDisable()
        {
            base.UIOnDisable();
            ConfigInventory.PalayerBag.RemoveEventListener<InventoryItem[]>(RefreshItem);
        }

        /// <summary> 刷新SlotUI列表 </summary>
        private void RefreshItem(InventoryItem[] inventoryItems)
        {
            for(int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].itemAmount > 0)
                {
                    ItemDetails itemDetails = InventorySystem.Instance.GetItem(inventoryItems[i].itemID);
                    playerBagSlotList[i].UpdateSlot(itemDetails, inventoryItems[i].itemAmount).Forget();
                }
                else playerBagSlotList[i].UpdateEmptySlot();
            }
        }
    }
}