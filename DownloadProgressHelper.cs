public static class DownloadProgressHelper
{
    public async static Task<string?> DownloadFileWithProgressBarAsync(this HttpClient httpClient, string url, string downloadPath)
    {
        using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return null;
            }

            var totalBytes = response.Content.Headers.ContentLength;

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var buffer = new byte[8192];
                var moreToRead = true;

                do
                {
                    var read = await responseStream.ReadAsync(buffer, 0, buffer.Length);   
                    
                    await fileStream.WriteAsync(buffer, 0, read);

                    moreToRead = read > 0;
                } while (moreToRead);
            }
        }

        return downloadPath;
    }
}