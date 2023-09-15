using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// 初始化资源包
/// </summary>
namespace MyFrameworkCore
{
	internal class FsmInitialize : IStateNode
	{
		private StateMachine _machine;

		void IStateNode.OnCreate(StateMachine machine)
		{
			_machine = machine;
		}
		void IStateNode.OnEnter()
		{
			Debug.Log("初始化资源包");
			PatchManager.Instance.LoadingText.text = "初始化资源包";
            // 创建默认的资源包
            InitPackage().Forget();
            //UniSingleton.StartCoroutine(InitPackage());
        }
        private async UniTaskVoid InitPackage()
        {
            await UniTask.Delay(300);

            var playMode = PatchManager.Instance.PlayMode;

            // 创建默认的资源包
            string packageName = "PC";
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                package = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(package);
            }

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (playMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (playMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (playMode == EPlayMode.HostPlayMode)
            {
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // WebGL运行模式
            if (playMode == EPlayMode.WebPlayMode)
            {
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                var createParameters = new WebPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            await initializationOperation;
            if (initializationOperation.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"资源包初始化失败：{initializationOperation.Error}");
                return;
            }
            Debug.Log("资源包初始化成功！");
            _machine.ChangeState<FsmUpdateVersion>();
        }
        //旧协程
        //private IEnumerator InitPackage()
        //{
        //	yield return new WaitForSeconds(0.3f);

        //	var playMode = PatchManager.Instance.PlayMode;

        //	// 创建默认的资源包
        //	string packageName = "PC";
        //	var package = YooAssets.TryGetPackage(packageName);
        //	if (package == null)
        //	{
        //		package = YooAssets.CreatePackage(packageName);
        //		YooAssets.SetDefaultPackage(package);
        //	}

        //	// 编辑器下的模拟模式
        //	InitializationOperation initializationOperation = null;
        //	if (playMode == EPlayMode.EditorSimulateMode)
        //	{
        //		var createParameters = new EditorSimulateModeParameters();
        //		createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
        //		initializationOperation = package.InitializeAsync(createParameters);
        //	}

        //	// 单机运行模式
        //	if (playMode == EPlayMode.OfflinePlayMode)
        //	{
        //		var createParameters = new OfflinePlayModeParameters();
        //		createParameters.DecryptionServices = new GameDecryptionServices();
        //		initializationOperation = package.InitializeAsync(createParameters);
        //	}

        //	// 联机运行模式
        //	if (playMode == EPlayMode.HostPlayMode)
        //	{
        //		string defaultHostServer = GetHostServerURL();
        //		string fallbackHostServer = GetHostServerURL();
        //		var createParameters = new HostPlayModeParameters();
        //		createParameters.DecryptionServices = new GameDecryptionServices();
        //		createParameters.QueryServices = new GameQueryServices();
        //		createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
        //		initializationOperation = package.InitializeAsync(createParameters);
        //	}

        //	// WebGL运行模式
        //	if (playMode == EPlayMode.WebPlayMode)
        //	{
        //		string defaultHostServer = GetHostServerURL();
        //		string fallbackHostServer = GetHostServerURL();
        //		var createParameters = new WebPlayModeParameters();
        //		createParameters.DecryptionServices = new GameDecryptionServices();
        //		createParameters.QueryServices = new GameQueryServices();
        //		createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
        //		initializationOperation = package.InitializeAsync(createParameters);
        //	}

        //	yield return initializationOperation;
        //	if (initializationOperation.Status != EOperationStatus.Succeed)
        //	{
        //		Debug.LogError($"资源包初始化失败：{initializationOperation.Error}");
        //		yield break;
        //	}
        //	Debug.Log("资源包初始化成功！");
        //	_machine.ChangeState<FsmUpdateVersion>();
        //}

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private string GetHostServerURL()
		{
			//string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
			string hostServerIP = "http://127.0.0.1";
			string appVersion = "v1.0";

#if UNITY_EDITOR
			if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
				return $"{hostServerIP}/CDN/Android/{appVersion}";
			else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
				return $"{hostServerIP}/CDN/IPhone/{appVersion}";
			else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
				return $"{hostServerIP}/CDN/WebGL/{appVersion}";
			else
				return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		else
			return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
		}


		/// <summary>
		/// 远端资源地址查询服务类
		/// </summary>
		private class RemoteServices : IRemoteServices
		{
			private readonly string _defaultHostServer;
			private readonly string _fallbackHostServer;

			public RemoteServices(string defaultHostServer, string fallbackHostServer)
			{
				_defaultHostServer = defaultHostServer;
				_fallbackHostServer = fallbackHostServer;
			}
			string IRemoteServices.GetRemoteFallbackURL(string fileName)
			{
				return $"{_defaultHostServer}/{fileName}";
			}
			string IRemoteServices.GetRemoteMainURL(string fileName)
			{
				return $"{_fallbackHostServer}/{fileName}";
			}
		}

		/// <summary>
		/// 资源文件解密服务类
		/// </summary>
		private class GameDecryptionServices : IDecryptionServices
		{
			public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
			{
				return 32;
			}

			public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
			{
				throw new NotImplementedException();
			}

			public Stream LoadFromStream(DecryptFileInfo fileInfo)
			{
				BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				return bundleStream;
			}

			public uint GetManagedReadBufferSize()
			{
				return 1024;
			}
		}
	}
}