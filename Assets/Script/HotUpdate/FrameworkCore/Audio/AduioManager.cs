using System.Collections.Generic;
using UnityEngine;


/*--------�ű�����-----------
�������䣺
    2605218730@qq.com
����:
    �����޳�
����:
    ��Ƶģ��
-----------------------*/

namespace MyFrameworkCore
{
    public class AduioManager : SingletonInit<AduioManager>, ICore
    {
        private Dictionary<string, AudioClip> AudioClipDic; //��Ч�б�
        private GameObject soundObj = null;                 //��Ч��������
        private float bkValue = 1;                          //�������ִ�С
        public AudioSource bkMusic;                         //�����������
        private float soundValue = 1;                       //��Ч��С
        private List<AudioSource> soundList;                //��Ч�б�
        public void ICroeInit()
        {
            AudioClipDic = new Dictionary<string, AudioClip>();
            soundList = new List<AudioSource>();
        }
        public void Init()
        {
            if (soundObj == null)
                soundObj = new GameObject("Aduio");

            bkMusic = soundObj.AddComponent<AudioSource>();
            GameObject.DontDestroyOnLoad(soundObj);
            RDebug.Log("��Ƶģ���ʼ���ɹ�!");
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public void OnAudioClipDicInit(float bkValue, float soundValue,
           AudioSource bkMusic, AudioSource soundMusic)
        {
            this.bkValue = bkValue;
            this.soundValue = soundValue;
            //PlayBkMusic()
        }

        /// <summary>
        /// �����Ч�Ƿ񲥷Ž���
        /// </summary>
        public void ChackSoundOpenOver()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    GameObject.Destroy(soundList[i]);
                    soundList.RemoveAt(i);
                }
            }
        }

        //***********************��������***********************
        /// <summary>
        /// ���ű�������
        /// </summary>
        /// <param name="name"></param>
        public void PlayBkMusic(string name, AudioClip clip)
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        }

        /// <summary>
        /// ��ͣ��������
        /// </summary>
        public void PauseBKMusic()
        {
            bkMusic?.Pause();
        }

        /// <summary>
        /// ֹͣ��������
        /// </summary>
        public void StopBKMusic()
        {
            bkMusic?.Stop();
        }

        /// <summary>
        /// �ı䱳������ ������С
        /// </summary>
        /// <param name="v"></param>
        public void ChangeBKValue(float v)
        {
            bkValue = v;
            if (bkMusic == null)
                return;
            bkMusic.volume = bkValue;
        }


        //***********************��Ч����***********************
        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLoop"></param>
        /// <param name="clip"></param>
        public void PlaySound(string name, bool isLoop, AudioClip clip)
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
        }

        /// <summary>
        /// �ı���Ч������С
        /// </summary>
        /// <param name="value"></param>
        public void ChangeSoundValue(float value)
        {
            soundValue = value;
            for (int i = 0; i < soundList.Count; ++i)
                soundList[i].volume = value;
        }

        /// <summary>
        /// ֹͣ��Ч
        /// </summary>
        public void StopSound(AudioSource source)
        {
            if (soundList.Contains(source))
            {
                soundList.Remove(source);
                source.Stop();
                GameObject.Destroy(source);
            }
        }
    }
}
