using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    世界物品脚本,挂到WorldItem预制体上
-----------------------*/

namespace MyFrameworkCore
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        public int itemAmount;
        public ItemDetails itemDatails;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D coll;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemID != 0) LoadItem(itemID, itemAmount).Forget();
        }
        public async UniTaskVoid LoadItem(int itemID,int itemAmount)
        {
            this.itemID = itemID;
            this.itemAmount = itemAmount;
            this.itemDatails = InventorySystem.Instance.GetItem(itemID);
            if (itemDatails == null)
            {
                RDebug.Log("没找到itemDatails");
                return;
            }
            string itemIconName = itemDatails.itemIconName;
            spriteRenderer.sprite = await ResourceManager.Instance.LoadAsyncUniTask<Sprite>(itemIconName);
            Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
            coll.size = newSize;//设置尺寸
            coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);//设置他的偏移，根据中心点
        }

        /// <summary> Item触发检测 </summary>
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(ConfigTag.TagPlayer) && InventorySystem.Instance.AddItemDicArray(ConfigInventory.PalayerBag, this)) 
            { 
                Destroy(gameObject);
            }
        }
    }
}