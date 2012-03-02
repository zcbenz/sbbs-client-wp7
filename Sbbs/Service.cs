using System;
using System.Net;
using System.Windows;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace sbbs_client_wp7.Sbbs
{
    // 集合类型
    using TopicCollection = ObservableCollection<TopicViewModel>;

    // 回调函数参数
    public struct ServiceArg<T>
    {
        public Action<T, bool, string> callback;
    }

    public class Service
    {
        // API前缀和后缀
        private const string apiBase = "http://bbs.seu.edu.cn/napi/";
        private const string apiPost = ".json";

        // 用户认证Token
        public string Token { get; set; }

        // 登录换取Token
        public void Login(string username, string password, Action<string, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "token" + apiPost);

            wc.DownloadStringCompleted += DownloadedAndParse<string, TokenResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<string>() { callback = callback });
        }

        // 获取十大
        public void Topten(Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "hot/topten" + apiPost);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { callback = callback });
        }

        // 下载完成后分析JSON数据然后调用回调函数
        // C为返回类型，比如TopicCollection
        // R为JSON的Response类型
        void DownloadedAndParse<C, R>(object sender, DownloadStringCompletedEventArgs e)
            where R : Response, IResponse<C>
        {
            // 用户传入参数
            ServiceArg<C> arg = (ServiceArg<C>)e.UserState;
            // 检查网络错误
            if (e.Error != null)
            {
                arg.callback(default(C), false, "网络连接错误");
                return;
            }

            // 读取下载的字符串
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(e.Result)))
            {
                // 转换成JSON
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(R));
                R result = serializer.ReadObject(stream) as R;
                if (!result.success)
                {
                    // result.error表示有错误
                    arg.callback(default(C), false, result.error);
                }
                else
                {
                    arg.callback(result.Root, true, null);
                }
            }
        }
    }
}
