using iTunesLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace iLyric
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            iTunes = new iTunesApp();
        }

        private iTunesApp iTunes;
        private Boolean MusixMatchMode = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            IITTrack track = iTunes.CurrentTrack;
            if(track == null)
            {
                label1.Text = "Disabled";
                textBox1.Text = "Disabled";
                return;
            }
            if (!track.Enabled)
            {
                label1.Text = "Disabled";
                textBox1.Text = "Disabled";
            }

            string trackName = track.Name;
            string trackArtist = track.Artist;
            string trackAlbum = track.Album;

            label2.Text = textBox1.Font.Size.ToString();

            label3.Text = "";

            label1.Text = trackName + " | " + trackArtist + " | " + trackAlbum;
            int duration = track.Duration;
            if (MusixMatchMode)
            {
                Dictionary<string, string> dic = GetMusixMatchLyrics(trackName, trackArtist);
                if (dic != null && dic["instrumental"] == "1")
                {
                    textBox1.Text = "Instrumental";
                    return;
                }
                string lyrics = dic["lyrics"];
                string artist = dic["artist"];
                if (lyrics == null)
                {
                    dic = GetMusixMatchLyrics(trackName);
                    if (dic != null)
                    {
                        lyrics = dic["lyrics"];
                        artist = dic["artist"];
                    }
                }
                if (lyrics != null)
                {
                    textBox1.Text = lyrics;
                    label3.Text = artist;
                }
                else
                {
                    if (dic != null && dic["instrumental"] == "1")
                    {
                        textBox1.Text = "Instrumental";
                    }
                    else
                    {
                        textBox1.Text = "見つかりませんでした。";
                    }
                }
            }
            else
            {
                IITFileOrCDTrack cdtrack = (IITFileOrCDTrack)track;
                textBox1.Text = cdtrack.Lyrics;
            }

            /*
            string loc = cdtrack.Location;
            label1.Text = loc;
            
            string dir = Path.GetDirectoryName(loc);
            string filename = Path.GetFileNameWithoutExtension(loc);

            string lyricsFilePathLRC = dir + "\\" + filename + ".lrc";
            string lyricsFilePathTXT = dir + "\\" + filename + ".txt";

            if (File.Exists(lyricsFilePathLRC))
            {
                // 歌詞ファイルがある
                using (StreamReader r = new StreamReader(lyricsFilePathLRC, Encoding.GetEncoding("shift_jis")))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        Console.WriteLine(line); // [00:00:00]Welcome to ようこそジャパリパーク！
                        // TODO []内を:で切って、分秒ミリ秒？で処理、秒に直してうんちゃら
                    }
                }
            }
            else if (File.Exists(lyricsFilePathTXT))
            {
                // 歌詞ファイルがある
                using (StreamReader r = new StreamReader(lyricsFilePathTXT, Encoding.GetEncoding("shift_jis")))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else { 
                // 歌詞ファイルがない

            }
            */
        }
        private void textbox_caret_control(object sender, EventArgs e)
        {
            if (Cursor.Current != Cursors.Default)
            {
                Cursor.Current = Cursors.Default;
            }
            TextBox textbox = (TextBox)sender;
            textbox.Enabled = false;
            textbox.Enabled = true;
        }
        private void textbox_cursor_control(object sender, EventArgs e)
        {
            if (Cursor.Current != Cursors.Default)
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void textbox_cursor_control(object sender, MouseEventArgs e)
        {
            if (Cursor.Current != Cursors.Default)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string check = null;
        private void timer1_Tick(object sender, EventArgs e)
        {
            IITTrack track = iTunes.CurrentTrack;
            if(track == null)
            {
                label1.Text = "Disabled";
                textBox1.Text = "Disabled";
                return;
            }
            if (!track.Enabled)
            {
                label1.Text = "Disabled";
                textBox1.Text = "Disabled";
                return;
            }

            string trackName = track.Name;
            string trackArtist = track.Artist;
            string trackAlbum = track.Album;

            label1.Text = trackName + " | " + trackArtist + " | " + trackAlbum;
            int duration = track.Duration;
            if (MusixMatchMode)
            {
                if(check == trackName + " | " + trackArtist + " | " + trackAlbum)
                {
                    return;
                }
                check = trackName + " | " + trackArtist + " | " + trackAlbum;
                Dictionary<string, string> dic = GetMusixMatchLyrics(trackName, trackArtist);
                if (dic != null && dic["instrumental"] == "1")
                {
                    textBox1.Text = "Instrumental";
                    return;
                }
                string lyrics = dic["lyrics"];
                string artist = dic["artist"];
                if (lyrics == null)
                {
                    dic = GetMusixMatchLyrics(trackName);
                    if (dic != null)
                    {
                        lyrics = dic["lyrics"];
                        artist = dic["artist"];
                    }
                }
                if (lyrics != null)
                {
                    textBox1.Text = lyrics;
                    label3.Text = artist;
                }
                else
                {
                    if (dic != null && dic["instrumental"] == "1")
                    {
                        textBox1.Text = "Instrumental";
                    }
                    else
                    {
                        textBox1.Text = "見つかりませんでした。";
                        label3.Text = "";
                    }
                }
            }
            else
            {
                IITFileOrCDTrack cdtrack = (IITFileOrCDTrack)track;
                textBox1.Text = cdtrack.Lyrics;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 1f, textBox1.Font.Style);
            label2.Text = textBox1.Font.Size.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 1f, textBox1.Font.Style);
            label2.Text = textBox1.Font.Size.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MusixMatchMode)
            {
                MusixMatchMode = false;
                button3.Text = "MusixMatchMode";
                Console.WriteLine("button3_Click: " + "NOW iTunesLyricsMode");

                label3.Text = "iTunesLyricsMode";

                IITTrack track = iTunes.CurrentTrack;
                if (!track.Enabled)
                {
                    label1.Text = "Disabled";
                    textBox1.Text = "";
                }

                string trackName = track.Name;
                string trackArtist = track.Artist;
                string trackAlbum = track.Album;

                IITFileOrCDTrack cdtrack = (IITFileOrCDTrack)track;
                textBox1.Text = cdtrack.Lyrics;
            }
            else
            {
                MusixMatchMode = true;
                button3.Text = "iTunesLyricsMode";
                Console.WriteLine("button3_Click: " + "NOW MusixMatchMode");

                label3.Text = "MusixMatchMode";

                IITTrack track = iTunes.CurrentTrack;
                if (!track.Enabled)
                {
                    label1.Text = "Disabled";
                    textBox1.Text = "";
                }

                string trackName = track.Name;
                string trackArtist = track.Artist;
                string trackAlbum = track.Album;

                Dictionary<string, string> dic = GetMusixMatchLyrics(trackName, trackArtist);
                if (dic != null && dic["instrumental"] == "1")
                {
                    textBox1.Text = "Instrumental";
                    return;
                }
                string lyrics = dic["lyrics"];
                string artist = dic["artist"];
                if (lyrics == null)
                {
                    dic = GetMusixMatchLyrics(trackName);
                    if (dic != null)
                    {
                        lyrics = dic["lyrics"];
                        artist = dic["artist"];
                    }
                }
                if (lyrics != null)
                {
                    textBox1.Text = lyrics;
                    label3.Text = artist;
                }
                else
                {
                    if (dic != null && dic["instrumental"] == "1")
                    {
                        textBox1.Text = "Instrumental";
                    }
                    else
                    {
                        textBox1.Text = "見つかりませんでした。";
                    }
                }
            }
        }
        private Dictionary<string, string> GetMusixMatchLyrics(String title)
        {
            Console.WriteLine("GetMusixMatchLyrics(title): " + title);
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(
                    "http://api.musixmatch.com/ws/1.1/track.search?q_track=" + HttpUtility.UrlEncode(title) + "&apikey=8b7654870c8395335a30eb19039218f6"
                );
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string text = sr.ReadToEnd();
            Console.WriteLine("GetMusixMatchLyrics(title): " + text);
            JObject obj = JObject.Parse(text);
            JArray track_list = (JArray)obj["message"]["body"]["track_list"];
            if (track_list.Count != 1)
            {
                // ERROR
                return null;
            }
            JObject track = (JObject)track_list[0];
            Dictionary<string, string> r = new Dictionary<string, string>();
            r["lyrics"] = GetMusixMatchLyrics((int)track["track"]["track_id"]);
            r["artist"] = (string)track["track"]["artist_name"];
            r["instrumental"] = (string)track["track"]["instrumental"];
            return r;
        }
        private Dictionary<string, string> GetMusixMatchLyrics(String title, String artist)
        {
            Console.WriteLine("GetMusixMatchLyrics(title, artist): " + title + ", " + artist);
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(
                    "http://api.musixmatch.com/ws/1.1/track.search?q_track=" + HttpUtility.UrlEncode(title) + "&q_track_artist=" + HttpUtility.UrlEncode(artist) + "&apikey=8b7654870c8395335a30eb19039218f6"
                );
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string text = sr.ReadToEnd();
            Console.WriteLine("GetMusixMatchLyrics(title, artist): " + text);
            JObject obj = JObject.Parse(text);
            JArray track_list = (JArray)obj["message"]["body"]["track_list"];
            if (track_list.Count == 0)
            {
                // ERROR
                Dictionary<string, string> r_ = new Dictionary<string, string>();
                r_["lyrics"] = null;
                r_["artist"] = null;
                r_["instrumental"] = "0";
                return r_;
            }
            JObject track = (JObject)track_list[0];
            Dictionary<string, string> r = new Dictionary<string, string>();
            r["lyrics"] = GetMusixMatchLyrics((int)track["track"]["track_id"]);
            r["artist"] = (string)track["track"]["artist_name"];
            r["instrumental"] = (string)track["track"]["instrumental"];
            return r;
        }
        private String GetMusixMatchLyrics(int track_id)
        {
            Console.WriteLine("GetMusixMatchLyrics(track_id): " + track_id);
            WebClient wc = new WebClient();
            wc.Headers.Add("cookie", "x-mxm-user-id=; x-mxm-token-guid=e08e6c63-edd1-4207-86dc-d350cdf7f4bc; mxm-encrypted-token=; AWSELB=55578B011601B1EF8BC274C33F9043CA947F99DCFF6AB1B746DBF1E96A6F2B997493EE03F2DD5F516C3BC8E8DE7FE9C81FF414E8E76CF57330A3F26A0D86825F74794F3C94");
            wc.Headers.Add("cache-control", "no-cache");
            wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.91 Safari/537.36");
            wc.Headers.Add("upgrade-insecure-requests", "1");
            wc.Headers.Add("accept-language", "en-US,en;q=0.8");
            wc.Headers.Add("accept-encoding", "gzip, deflate");
            wc.Headers.Add("dnt", "1");
            Stream stream = wc.OpenRead(
                    "https://apic-desktop.musixmatch.com/ws/1.1/track.lyrics.get?format=json&track_id=" + track_id + "&user_language=ja&f_subtitle_length=0&f_subtitle_length_max_deviation=0&subtitle_format=lrc&app_id=web-desktop-app-v1.0&guid=e08e6c63-edd1-4207-86dc-d350cdf7f4bc&usertoken=1710144894f79b194e5a5866d9e084d48f227d257dcd8438261277"
                );
            GZipStream responseStream = new GZipStream(stream, CompressionMode.Decompress);
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            string text = sr.ReadToEnd();
            Console.WriteLine("GetMusixMatchLyrics(track_id): " + text);
            JObject obj = JObject.Parse(text);
            if(obj["message"]["body"].SelectToken("lyrics") == null){
                return null;
            }
            return obj["message"]["body"]["lyrics"]["lyrics_body"].ToString().Replace("\n", "\r\n");
        }
    }
}
