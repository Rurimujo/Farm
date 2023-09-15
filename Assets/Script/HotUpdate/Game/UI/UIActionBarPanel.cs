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
    快捷栏面板
-----------------------*/

namespace MyFrameworkCore
{
    public class UIActionBarPanel : UIBase
    {

        public GameObject T_BagBtn { get; set; }
        public GameObject T_ActionSlotGroup { get; set; }


        private List<SlotUI> ActionBarSlotUIList;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Normal, EUIMode.Normal);

            UIComponent uiComponent = panelGameObject.GetComponent<UIComponent>();

            T_ActionSlotGroup = uiComponent.Get<GameObject>("T_ActionSlotGroup");
            T_BagBtn = uiComponent.Get<GameObject>("T_BagBtn");

            int slotAmount = T_ActionSlotGroup.transform.childCount - 1;
            InventorySystem.Instance.CreatItemDicArray(ConfigInventory.ActionBar,slotAmount);

            ActionBarSlotUIList = new List<SlotUI>();
            for (int i = 0;i< slotAmount; i++)
            {
                SlotUI slotUI = T_ActionSlotGroup.GetChildComponent<SlotUI>(i);
                slotUI.slotIndex = i;
                slotUI.configInventoryKey = ConfigInventory.ActionBar;
                ActionBarSlotUIList.Add(slotUI);
            }

            ButtonOnClickAddListener(T_BagBtn, T_BagButtonListener);
        }

        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigInventory.ActionBar.AddEventListener<InventoryItem[]>(RefreshItem);
            InventoryItem[] playerBagItems = InventorySystem.Instance.GetItemDicArray(ConfigInventory.ActionBar);
            ConfigInventory.ActionBar.EventTrigger(playerBagItems);
        }

        public override void UIOnDestroy()
        {
            base.UIOnDestroy();
            ConfigInventory.ActionBar.RemoveEventListener<InventoryItem[]>(RefreshItem);
        }
        private void RefreshItem(InventoryItem[] inventoryItems)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].itemAmount > 0)
                {
                    ItemDetails itemDetails = InventorySystem.Instance.GetItem(inventoryItems[i].itemID);
                    ActionBarSlotUIList[i].UpdateSlot(itemDetails, inventoryItems[i].itemAmount).Forget();
                }
                else ActionBarSlotUIList[i].UpdateEmptySlot();
            }
        }
        private void T_BagButtonListener(GameObject go)
        {
            bool bagOpened = UIManager.Instance.IsShow(ConfigUIPanel.UIPlayerBagPanel);
            RDebug.Log(bagOpened);
            if (bagOpened)
            {
                CloseOtherUIForm(ConfigUIPanel.UIPlayerBagPanel);
            }
            else
            {
                OpenUIForm<UIPlayerBagPanel>(ConfigUIPanel.UIPlayerBagPanel);
            }
        }//背包按钮监听
    }
}