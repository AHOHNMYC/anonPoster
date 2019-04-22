namespace anonPoster {
    static class URLs {
        public const string mainStream      = "https://stream1.cybergame.tv/live/fiuu.m3u8";
        public const string mainStreamInfo  = "https://api.cybergame.tv/p/statusv2/?channel=fiuu";
        public const string mainStreamRtmp  = "rtmp://stream1.cybergame.tv:2936/live/fiuu";
        public const string bomjStream      = "rtmp://stream1.cybergame.tv:2936/live/thanksyiiifor240p";
        public const string youTube         = "https://www.youtube.com/channel/UCh4B8kA4xl7KS4TTgRHIePA/videos";

        public const string radio           = "https://anon.fm";
        public const string radioSong       = radio + "/song/";
        public const string radioState      = radio + "/state.txt";
        public const string radioSched      = radio + "/shed.js";
        public const string radioFeedback   = radio + "/feedback";

        public const string audio           = "http://anon.fm:8000";
        public const string audioSSL        = radio + "/streams";
        public const string audio192        = "/radio";
        public const string audio64         = "/radio-low";
        public const string audio12         = "/radio.aac";


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
