using Cysharp.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    淡入淡出面板
-----------------------*/

namespace MyFrameworkCore
{
    public class UIFadePanel : UIBase
    {
        private CanvasGroup fadeCanvasGroup;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fade, EUIMode.Normal);
            fadeCanvasGroup = panelGameObject.GetComponent<CanvasGroup>();
            ConfigEvent.UIFade.AddEventListenerUniTask<float>(Fade);
        }

        /// <summary>loading画面淡入淡出场景</summary>
        public async UniTask Fade(float targetAlpha)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / ConfigSettings.fadeDuretion;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))//Approximately 判断是否大概相似
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                await UniTask.Yield();
            }
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}