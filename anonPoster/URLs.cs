namespace anonPoster {
    static class URLs {
        public const string mainStream      = "https://stream1.cybergame.tv/live/fiuu.m3u8";
        public const string mainStreamRtmp  = "rtmp://stream1.cybergame.tv:2936/live/fiuu";
        public const string bomjStream      = "rtmp://stream1.cybergame.tv:2936/live/thanksyiiifor240p";
        public const string youTube         = "https://www.youtube.com/channel/UCh4B8kA4xl7KS4TTgRHIePA/videos";

        public const string radio           = "https://anon.fm";
        public const string radioSong       = radio + "/song/";
        public const string radioState      = radio + "/state.txt";
        public const string radioFeedback   = radio + "/feedback";

        public static string RadioCaptcha(int CaptchaID) {
            return $"{radioFeedback}/{CaptchaID}.gif";
        }
        public static string RadioCover(int CoverID) {
            return $"{radio}/tmp/image-cover-{CoverID}.jpg";
        }

        public static string Googel(string query) {
            return $"https://google.com/search?q={query}";
        }

        public const string selfUpdate = "https://github.com/AHOHNMYC/anonPoster/releases/latest";
    }
}
