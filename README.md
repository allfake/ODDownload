# ODDownload
wrapper www downloader with delegate

#### ODDownload

Simple downloader
    
    StartCoroutine(_Download(full_path));
---

#### ODApiRequest

Json format should be

    {
        "requireAssets": 
        [
            { "url": "http://localhost:8000/xxxx.bundle", "size":"93024"},
            { "url": "http://localhost:8000/yyyyy.bundle", "size":"93024"},
            { "url": "http://localhost:8000/zzzzz.bundle", "size":"93024"}
        ],
        "version": "1"
    }
