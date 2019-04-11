using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class ODDownloadList : ODSingletonMono<ODDownloadList>
{
	public delegate void DownloadFinish(long statusCode, string error = null); // declare delegate type
	public delegate void Downloading(); // declare delegate type
	public DownloadFinish downloadFinish;
	public Downloading downloading;
	private ODDownload downloader;
	public List<string> downloadList;
	public float progress;
	public float totalBytesDownload;
	public bool isDone = false;
	void Awake()
	{
		downloader = ODDownload.Instance;
	}

	public void Download()
	{
		StartCoroutine(_Download());
	}

	public IEnumerator _Download()
	{
		progress = 0;
		isDone = false;
		bool isError = false;
		totalBytesDownload = 0;

		for (int i = 0; i < downloadList.Count; i++)
		{
			float currentDownloadBytes = 0;

			if (isError)
			{
				break;
			}

			downloader.downloadError = (long statusCode, string error) => {
				Debug.Log("Error:" + statusCode + " => " + error);
				isError = true;
				if (downloadFinish != null)
				{
					downloadFinish(statusCode, error);
				}
			};

			downloader.downloadBytes = (ulong downloadBytes) => {			
				currentDownloadBytes = downloadBytes - currentDownloadBytes;
				totalBytesDownload += currentDownloadBytes;
				progress = (float)((float)i / downloadList.Count) + (downloader.progress / (float)downloadList.Count);
				
				if (downloading != null)
				{
					downloading();
				}
			};
			
			yield return StartCoroutine(downloader._Download(downloadList[i]));
		}

		if (isError == false)
		{
			if (downloadFinish != null)
			{
				downloadFinish(200);
			}
		}

		isDone = true;
		yield return null;
	}
}
