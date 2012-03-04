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
    using BoardCollection = ObservableCollection<BoardViewModel>;

    // 回调函数参数
    public struct ServiceArg<T>
    {
        public Action<T, bool, string> Callback;
    }

    public class Service
    {
        // API前缀和后缀
        private const string apiBase = "http://bbs.seu.edu.cn/napi/";
        private const string apiPost = ".json";

        // 用户认证Token
        public string Token { get; set; }

        // 默认版面模式
        public int BoardMode { get; set; }

        // 登录换取Token
        public void Login(string username, string password, Action<string, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "token" + apiPost + "?user=" + HttpUtility.UrlEncode(username) + "&pass=" + HttpUtility.UrlEncode(password));

            wc.DownloadStringCompleted += DownloadedAndParse<string, TokenResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<string>() { Callback = callback });
        }

        // 获取十大
        public void Topten(Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "hot/topten" + apiPost);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        // 获取收藏夹
        public void Favorates(Action<BoardCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "fav/list" + apiPost + "?up=1&token=" + HttpUtility.UrlEncode(Token));

            wc.DownloadStringCompleted += DownloadedAndParse<BoardCollection, BoardsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<BoardCollection>() { Callback = callback });
        }

        // 获取版面
        public void Board(string board, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            Board(board, BoardMode, start, limit, callback);
        }

        public void Board(string board, int mode, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "board/" + board + apiPost + "?token=" + HttpUtility.UrlEncode(Token) + "&start=" + start + "&limit=" + limit + "&mode=" + mode);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        // 获取话题
        public void Topic(string board, int id, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "topic/" + board + "/" + id + apiPost + "?token=" + HttpUtility.UrlEncode(Token) + "&start=" + start + "&limit=" + limit);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        // 发帖
        public void TopicPost(string board, int reid, string title, string content, Action<TopicViewModel, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(apiBase + "topic/post" + apiPost + "?type=2&token=" + HttpUtility.UrlEncode(Token) + "&board=" + board + "&reid=" + reid
                + "&title=" + HttpUtility.UrlEncode(title) + "&content=" + HttpUtility.UrlEncode(content));

            wc.DownloadStringCompleted += DownloadedAndParse<TopicViewModel, TopicResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicViewModel>() { Callback = callback });
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
                arg.Callback(default(C), false, "网络连接错误");
                return;
            }

            // 读取下载的字符串
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(e.Result)))
            {
                // 转换成JSON
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(R));
                R result = serializer.ReadObject(stream) as R;
                if (result.error != null)
                {
                    // result.error表示有错误
                    arg.Callback(default(C), true, result.error);
                }
                else
                {
                    arg.Callback(result.Root, true, null);
                }
            }
        }
    }
}
