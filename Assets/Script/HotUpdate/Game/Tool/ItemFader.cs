using UnityEngine;
using DG.Tweening;

namespace MyFrameworkCore
{
    public class ItemFader : MonoBehaviour
    {
        private SpriteRenderer[] spriteRenderers;

        private void Awake()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var renderer in spriteRenderers) fadeOut(renderer);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            foreach (var renderer in spriteRenderers) fadeIn(renderer);
        }
        /// <summary>
        /// 逐渐恢复颜色
        /// </summary> 
        public void fadeIn(SpriteRenderer spriteRenderer)
        {
            print("逐渐恢复颜色");
            Color TargetColor = new Color(1, 1, 1, 1);
            spriteRenderer.DOColor(TargetColor, ConfigSettings.itemFadeDuretion);
        }

        ///// <summary>
        ///// 逐渐半透明
        ///// </summary>
        public void fadeOut(SpriteRenderer spriteRenderer)
        {
            print("逐渐半透明");
            Color TargetColor = new Color(1, 1, 1, ConfigSettings.targetAlpha);
            spriteRenderer.DOColor(TargetColor, ConfigSettings.itemFadeDuretion);
        }
    }
}