using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using Glue;
using Ceras;
using System.Linq;
using UnityEditor;

namespace Glue
{
	public class GlueConnector : MonoBehaviour
	{
		// Expose settings for UI
		public Settings Settings = DataPool.Settings;

		private UdpClient _socket;
		private IPEndPoint _endpoint;

		private float _currentFPS = 0;
		private int _frameCounter = 0;
		private System.DateTime _timeAtReceivedFrame;
		private System.DateTime _timeAtLateUpdate;

		private CerasSerializer _cerasSerializer;
		private CerasSerializer _cerasDeserializer;

		private byte[] _cerasSendBuffer = null;
		private byte[] _receivedData = null;
		private int _sendAmount;

		private ulong _lastFrameFrameCounter = 0;

		public delegate void OnReceiveFrame();
		public static OnReceiveFrame onReceiveFrame;

		private DataPool DataPool = DataPool.Instance;

		void OnEnable()
		{
			CerasSerializer.ClearGenericCaches();
			Application.runInBackground = true;
			SetupSerializer();
			StartStreaming();
		}

		void Update()
		{
			if (DataPool.Settings.OverrideVSync)
			{
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = DataPool.Settings.ForcedFPS;
			}
		}

		void LateUpdate()
		{
			SendBackFrame();
			_timeAtLateUpdate = System.DateTime.Now;
			_frameCounter++;
			DataPool.SendFrame.Counter = (ulong)_frameCounter;
			DataPool.Diagnostics.Framecount = _frameCounter;
			DataPool.Diagnostics.ReceiveFrameDifference = DataPool.ReceivedFrame.Counter - _lastFrameFrameCounter;
			DataPool.Diagnostics.TimeBetweenReceivedAndLateUpdate_ms = (int)(_timeAtLateUpdate - _timeAtReceivedFrame).TotalMilliseconds;
			_currentFPS = 1f / Time.unscaledDeltaTime;
			DataPool.Diagnostics.FPS = (int)_currentFPS;
		}

		void OnDisable()
		{
			// According to
			// https://stackoverflow.com/questions/24312265/should-a-udpclient-be-disposed-of
			// You should use .Close() rather than .Dispose()
			_socket.Close();
			CerasSerializer.ClearGenericCaches();
			DataPool.ReceivedFrame.Clear();
			DataPool.SendFrame.Clear();
		}

		private void SetupSerializer()
		{
			var config = new SerializerConfig();
			_cerasSerializer = new CerasSerializer(config);
			_cerasDeserializer = new CerasSerializer(config);
		}

		private void StartStreaming()
		{
			if (DataPool.Settings.IsVerbose)
				Debug.Log("GlueConnector: Started Streaming");
			try
			{
				_cerasSendBuffer = null;
				_endpoint = new IPEndPoint(IPAddress.Any, DataPool.Settings.PortToListenOn);
				_socket = new UdpClient();
				_socket.Client.SetSocketOption(SocketOptionLevel.Socket,
											   SocketOptionName.ReuseAddress,
											   true);
				_socket.Client.Bind(_endpoint);
				_socket.BeginReceive(new AsyncCallback(ReceiveFrame), _socket);
			}
			catch (SocketException se)
			{
				if (DataPool.Settings.IsVerbose)
					Debug.Log($"EXCEPTION: {se.Message}");
			}
		}

		private void ReceiveFrame(IAsyncResult result)
		{
			if (!_socket.Client.Connected)
				return;
			try
			{
				_timeAtReceivedFrame = System.DateTime.Now;
				_lastFrameFrameCounter = DataPool.ReceivedFrame.Counter;
				var receivedData = _socket.EndReceive(result, ref _endpoint);
				// System.IO.File.WriteAllBytes("ceras.receiveframe.bin", receivedData);
				_cerasDeserializer.Deserialize<Frame>(ref DataPool.ReceivedFrame, receivedData);
				Debug.Log(DataPool.ReceivedFrame.ToString());
				DataPool.Diagnostics.SocketReceiveBufferSize = receivedData.Length;
				DataPool.Diagnostics.SocketAvailable = _socket.Available;
				if (onReceiveFrame != null)
					onReceiveFrame();
				_socket.BeginReceive(new AsyncCallback(ReceiveFrame), _socket);
			}
			catch (SocketException se)
			{
				if (DataPool.Settings.IsVerbose)
					Debug.Log($"EXCEPTION: {se.Message}");
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		private void SendBackFrame()
		{
			try
			{
				_sendAmount = _cerasSerializer.Serialize<Frame>(DataPool.SendFrame, ref _cerasSendBuffer);
				_socket.Send(_cerasSendBuffer, _sendAmount, DataPool.Settings.IPOfReceiver, DataPool.Settings.PortOfUnityReceiver);
			}
			catch (SocketException se)
			{
				if (DataPool.Settings.IsVerbose)
					Debug.Log($"EXCEPTION: {se.Message}");
			}
			catch (IndexOutOfRangeException ioe)
			{
				Debug.Log(ioe.Message);
				Debug.Log(ioe.StackTrace);
			}
			catch (Exception e)
			{
				if (DataPool.Settings.IsVerbose)
					Debug.Log($"EXCEPTION: {e.Message}");
			}
		}
	}
}
