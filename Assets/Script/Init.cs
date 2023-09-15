using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace MyFrameworkCore
{
    public class Init : MonoBehaviour
    {
        /// <summary>
        /// ��Դϵͳ����ģʽ
        /// </summary>
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
        protected void Awake()
        {
            Debug.Log($"��Դϵͳ����ģʽ��{PlayMode}");
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            //��ʼ����Ϸ���
            this.InitFramework();
            //�����Դ����
            this.CheckHotUpdate();
        }

        private void InitFramework()
        {

        }
        private void CheckHotUpdate()
        {
            // ��ʼ������������
            PatchManager.Instance.Run(PlayMode);
        }

    }
}
