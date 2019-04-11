using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ODApiRequest : ODSingletonMono<ODApiRequest>
{
	[System.Serializable]
	public class APIOD
	{
		public string version;
		[SerializeField]
		public List<ApiAssetContent> requireAssets;
	}

	[System.Serializable]
	public class ApiAssetContent
	{
		public string url;
		public string size;
	}

	public string APIUrl = "";
	private ODDownload downloader;
	private ODDownloadList downloaderList;
	public float totalFilesize = 0;
	private List<string> contentList = new List<string>();
	private APIOD apiData;

	private void Awake()
	{
		downloader = ODDownload.Instance;
		downloaderList = ODDownloadList.Instance;
	}
	void Start () 
	{
		StartCoroutine(DownloadApi());
	}

	IEnumerator DownloadContent()
	{
		downloaderList.downloadList = contentList;
		downloaderList.downloading = () => {
			Debug.Log(downloaderList.totalBytesDownload + "/" + totalFilesize + " : " + downloaderList.progress * 100 + "%");
		};

		downloaderList.downloadFinish = (long statusCode, string error) => {
			Debug.Log(downloaderList.totalBytesDownload + "/" + totalFilesize + " : " + downloaderList.progress * 100 + "%");
		};

		yield return StartCoroutine(downloaderList._Download());
	}

	IEnumerator DownloadApi()
	{
		yield return StartCoroutine(downloader._Download(APIUrl));

		if (downloader.isSuccess)
		{
			totalFilesize = 0;
			apiData = JsonUtility.FromJson<APIOD>(downloader.resultText);

			for (int i = 0; i < apiData.requireAssets.Count; i++)
			{
				ApiAssetContent assetContent = apiData.requireAssets[i];
				contentList.Add(assetContent.url);
				totalFilesize += float.Parse(assetContent.size);
			}
		}
	}
}
