public static class DownloadProgressHelper
{
    public async static Task<string?> DownloadFileWithProgressBarAsync(this HttpClient httpClient, string url, string downloadPath)
    {
        using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
        {
            var totalBytes = 0L;

            try
            {
                response.EnsureSuccessStatusCode();
                totalBytes = response.Content.Headers.ContentLength ?? throw new Exception("Returned Success status code but no content");
            }
            catch (Exception)
            {
                return null;
            }

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var buffer = new byte[8192];
                var totalRead = 0L;
                var moreToRead = true;
                var totalMegaBytes = (double)totalBytes / 1024 / 1024;

                do
                {
                    var read = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                    totalRead += read;
                    var totalMegaBytesRead = (double)totalRead / 1024 / 1024;
                    
                    await fileStream.WriteAsync(buffer, 0, read);

                    var downloadPercent = (double)totalRead / totalBytes * 100;
                    Console.Write($"\rDownload Progress: {downloadPercent:F3}% [{totalMegaBytesRead:F2}MB / {totalMegaBytes:F2}MB]");

                    moreToRead = read > 0;
                } while (moreToRead);
            }
        }

        Console.WriteLine();
        return downloadPath;
    }
}