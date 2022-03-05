public static class DownloadProgressHelper
{
    public async static Task<string?> DownloadFileWithProgressBarAsync(this HttpClient httpClient, string url, string downloadPath)
    {
        var bufferSize = 8192;

        using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
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
                var buffer = new byte[bufferSize];
                var totalRead = 0L;
                var moreToRead = true;
                var totalMegaBytes = (double)totalBytes / 1024 / 1024;

                do
                {
                    var read = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                    totalRead += read;
                    var totalMegaBytesRead = (double)totalRead / 1024 / 1024;
                    
                    await fileStream.WriteAsync(buffer, 0, read);

                    WriteConsoleProgress(totalMegaBytesRead, totalMegaBytes, read == totalRead);

                    moreToRead = read > 0;
                } while (moreToRead);
            }
        }

        Console.WriteLine("Done");
        return downloadPath;
    }

    public static void WriteConsoleProgress(double currentMB, double totalMB, bool firstLine = false)
    {
        var progressBarLength = 20;
        var downloadPercent = currentMB / totalMB * 100;
        var currentProgress = (int)(downloadPercent / (100 / progressBarLength));

        Console.Write($"\rDownload Progress: {downloadPercent:F3}% [{currentMB:F2}MB / {totalMB:F2}MB]"
                        + $" [{new string('#', currentProgress)}{new string(' ', progressBarLength - currentProgress)}] ");
    }
}