using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace ModemRestart
{

    public partial class MainPage : ContentPage
    {
        public string UserName = "";
        public string Password = "";

        public string[] ipArry = { "192.168.1.1", "192.168.1.2" };

        public MainPage()
        {
            InitializeComponent();
            _PickerIp.ItemsSource = ipArry;
        }

        private async void _ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_EntryKullaniciAdi.Text) || string.IsNullOrEmpty(_EntrySifre.Text))
            {
                await DisplayAlert("Uyarı", "Gerekli bilgileri eksiksiz doldurunuz!", "Ok");
                return;
            }
            var selectedIp = _PickerIp.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedIp))
            {
                selectedIp = "http://192.168.1.1/";
            }
            else
            {
                selectedIp = "http://" + selectedIp.Trim() + "/";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"{selectedIp}login/login-page.cgi");
            request.Method = "POST";
            request.ContentType = "text/html";
            request.CookieContainer = new CookieContainer();
            request.UserAgent = "-";
            var postdata = $"AuthName={_EntryKullaniciAdi.Text.Trim()}" + $"&AuthPassword={_EntrySifre.Text.Trim()}";
            var data = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = data.Length;
            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var source = new StreamReader(response.GetResponseStream()).ReadToEnd();
            CookieContainer cookieContainer = new CookieContainer();
            foreach (Cookie item in response.Cookies)
            {
                cookieContainer.Add(item);
            }
            if (source.IndexOf("top.location=") != -1)
            {
                //bağlantı başarılı
                //await Loading("İşlem gerçekleştiriliyor...");

                Task t1 = Task.Run(() =>
                {
                    using (UserDialogs.Instance.Loading("İşlem gerçekleştiriliyor...", null, null, true, MaskType.Black))
                    {
                        //Task.Delay(TimeSpan.FromMilliseconds(30000));
                        Thread.Sleep(30000);
                    }
                });

                Task t2 = Task.Run(() =>
                    {
                        HttpWebRequest request2 = (HttpWebRequest)HttpWebRequest.Create($"{selectedIp}pages/tabFW/reboot-rebootpost.cgi");
                        request2.Method = "GET";
                        request2.ContentType = "text/html";
                        request2.CookieContainer = cookieContainer;
                        var response2 = (HttpWebResponse)request2.GetResponse();
                        source = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                    });
            }
            else
            {
                await DisplayAlert("Başarısız İşlem", "Modem ile bağlantı kurulamadı.Bilgileri kontrol ediniz", "Ok");
            }
        }

        //private async Task Loading(string message, int time = 30000)
        //{

        //}
    }
}
