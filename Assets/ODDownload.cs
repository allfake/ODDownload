using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ODDownload : MonoBehaviour {

	public delegate void DownloadBytes(ulong downloadBytes); // declare delegate type
	public delegate void DownloadCompleted(); // declare delegate type
	public delegate void DownloadSuccess(byte[] result); // declare delegate type
	public delegate void DownloadError(long statusCode, string error); // declare delegate type
	public DownloadBytes downloadBytes;
	public DownloadSuccess downloadFinish;
	public DownloadError downloadError;
	public DownloadCompleted downloadCompleted;
	public float fileSize;
	public float progress;
	public bool useCache = false;
	public string resultText;
	public byte[] result;
	public bool isSuccess = false;
	public bool isDone = false;
	void Start()
	{
		// Download();
	}
	public string full_path;
	public void Download()
	{
		StartCoroutine(_Download(full_path));
	}
	public IEnumerator _Download(string url)
	{
		fileSize = 0;
		progress = 0;
		resultText = "";
		result = new byte[] {};
		isSuccess = false;
		isDone = false;
		
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return null;
			AsyncOperation asyncOperation;
            // Request and wait for the desired page.
			DownloadHandler downloadHandler = new DownloadHandlerBuffer();
			webRequest.downloadHandler = downloadHandler;
            asyncOperation = webRequest.SendWebRequest();
			
			while(!asyncOperation.isDone)
			{
				UpdateProgress(webRequest);
			}
			
			UpdateProgress(webRequest);

			//string[] pages = url.Split('/');
			//int page = pages.Length - 1;

			if (webRequest.isNetworkError) {
				//Debug.Log(pages[page] + ": Error: " + webRequest.error);
				if (downloadError != null)
				{
					downloadError(webRequest.responseCode, webRequest.error);
				}
			}
			else
			{
				//Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
				if (downloadFinish != null)
				{
					downloadFinish(webRequest.downloadHandler.data);
				}
				result = webRequest.downloadHandler.data;
				resultText = webRequest.downloadHandler.text;
				isSuccess = true;
			}

			if (downloadCompleted != null)
			{
				downloadCompleted();
			}

			isDone = true;
			webRequest.Dispose();
        }
	}

	private void UpdateProgress(UnityWebRequest webRequest)
	{
		string fileSizeString = webRequest.GetResponseHeader("Content-Length");

		float.TryParse(fileSizeString, out fileSize);

		if (string.IsNullOrEmpty(fileSizeString) == false)
		{
		}

		if (downloadBytes != null)
		{
			downloadBytes(webRequest.downloadedBytes);
		}

		progress = webRequest.downloadProgress;
	}
}
