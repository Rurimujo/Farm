using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace MyFrameworkCore
{
    public class SlotUI : MonoBehaviour
    {
        public Image slotImage;             //图片
        private TextMeshProUGUI amountText; //数量文本
        private Button button;              //按钮
        public Image slotHightLight;        //高亮

        public bool isSelected;             //是否启用高亮
        public ItemDetails itemDatails;     //物品信息
        public int itemAmount;              //物品数量
        public int slotIndex;               //格子序列号
        public string configInventoryKey;   //属于哪个物品管理类的,也就是InventoryAllManager的ItemDicList或者ItemDicArray的Key

        private void Awake()
        {
            slotImage = gameObject.GetChildComponent<Image>("Image");
            amountText = gameObject.GetChildComponent<TextMeshProUGUI>("AmountText");
            button = GetComponent<Button>();
            slotHightLight = gameObject.GetChildComponent<Image>("HighLight");
        }
        private void Start()
        {
            isSelected = false;
            //if (itemDatails == null) 
            //    UpdateEmptySlot();
        }
        /// <summary> 更新Slot显示 </summary>
        public async UniTask UpdateSlot(ItemDetails itemDetails,int Amount)
        {
            this.itemDatails = itemDetails;
            this.itemAmount = Amount;
            slotImage.sprite = await ResourceExtension.LoadAsyncUniTask<Sprite>(itemDatails.itemIconName);
            amountText.text = Amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;//该组是否可交互（组下的元素是否处于启用状态）。
        }
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }
            itemDatails = null;
            itemAmount = 0;
            amountText.text = string.Empty;
            slotImage.enabled = false;
            button.interactable = false;

        }
    }
}