using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net.Security;
using System.Net;

namespace LeagueIconChanger
{
    public class LeagueClient
    {
        private int port;
        private string token;
        public LeagueClient()
        {

        }

        public bool LeagueRunning()
        {
            var leagueProc = GetLCUPath();
            return leagueProc != null;
        }

        public HttpStatusCode applyIcon(string url)
        {
            url = url.Replace("https://cdn.communitydragon.org/latest/profile-icon/", "");
            url = url.Replace(".jpg", "");
            Console.WriteLine(url);
            var sslCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += sslCallback;

                var data = $"{{\"profileIconId\": {url} }}";
                var bytes = Encoding.UTF8.GetBytes(data);
                HttpWebRequest http = WebRequest.CreateHttp($"https://127.0.0.1:{this.port}/lol-summoner/v1/current-summoner/icon");
                http.Headers.Set(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("riot:" + token)));
                http.ContentType = "application/json";
                http.ContentLength = (long)bytes.Length;
                http.Method = "PUT";
                http.GetRequestStream().Write(bytes, 0, bytes.Length);

                HttpWebResponse resp = (HttpWebResponse)http.GetResponse();
                return resp.StatusCode;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback -= sslCallback;
            }
            return HttpStatusCode.NotFound;
        }

        public void getLeagueApi()
        {
            if (!LeagueRunning()) return;

            string path = GetLCUPath();
            if (path == null) return;

            File.Copy(path + "lockfile", path + "tmp");
            var content = File.ReadAllText(path + "tmp", Encoding.UTF8);
            File.Delete(path + "tmp");

            var lockfile = content.Split(':');
            port = int.Parse(lockfile[2]);
            token = lockfile[3];

        }

        public string GetLCUPath()
        {
            var leagueProc = Process.GetProcesses().Where(p => p.ProcessName.Contains("League"));
            foreach(var proc in leagueProc)
            {
                try
                {
                    string cmdline = GetCommandLine(proc);
                    var index = cmdline.IndexOf("--install-directory");
                    if (index == -1) continue;

                    index = cmdline.IndexOf("=", index) + 1;
                    return cmdline.Substring(index, cmdline.IndexOf("\"", index) - index);
                }
                catch(Win32Exception ex) when (ex.HResult == -2146233079)
                {
                    // Do Nothing
                }
                
            }
            return null;
        }

        private string GetCommandLine( Process process)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            using (ManagementObjectCollection objects = searcher.Get())
            {
                return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
            }

        }
    }
}
