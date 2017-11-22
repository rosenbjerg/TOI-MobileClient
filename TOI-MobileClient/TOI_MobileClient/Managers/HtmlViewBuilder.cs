using System;
using System.Collections.Generic;
using System.Text;

namespace TOI_MobileClient.Managers
{
    internal static class HtmlViewBuilder
    {
        private const string CSS = "<style>body * {max-width: 100%;}</style>";

        private const string HEADER =
            "<html>" +
            "<head><title>ToI Web view</title>" + CSS + "</head>" + 
            "<body>";

        private const string END = "</body></html>";

        public static string BuildImageView(string url)
        {
            return HEADER + $"<img src='{url}'>" + END;
        }

        public static string BuildAudioView(string url)
        {
            return HEADER + $"<audio src='{url}' controls>Audio is sadly not supported.</audio>" + END;
        }

        public static string BuildVideoView(string url)
        {
            return HEADER + $"<video controls><source src='{url}' type='video/mp4'>Video is sadly not supported.</video>" + END;
        }

        public static string BuildTextView(string url)
        {
            return HEADER + "<p>This is a paragraph that contains text!</p>" + END;
        }
    }
}
