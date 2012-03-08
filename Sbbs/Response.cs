using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Sbbs
{
    // 服务器响应通常会包括一个字段作为返回值，该返回值字段名通常不定，
    // 因此响应DataContract子类需要自己手动定义哪个字段是返回值字段
    public interface IResponse<T>
    {
        T Root { get; }
    }

    // 服务器响应一定会包含的字段
    [DataContract]
    [KnownType(typeof(TopicsResponse))]
    [KnownType(typeof(BoardsResponse))]
    [KnownType(typeof(TokenResponse))]
    public class Response
    {
        [DataMember(Name = "success")]
        public bool success;

        [DataMember(Name = "error")]
        public string error;

        [DataMember(Name = "time")]
        public int time;

        [DataMember(Name = "cost")]
        public int cost;
    }

    // 返回主题集合
    // 符合类型： 十大，版面
    [DataContract]
    public class TopicsResponse : Response, IResponse<ObservableCollection<TopicViewModel>>
    {
        [DataMember(Name = "topics")]
        public ObservableCollection<TopicViewModel> topics;

        public ObservableCollection<TopicViewModel> Root
        {
            get
            {
                return topics;
            }
        }
    }

    // 返回单个主题
    [DataContract]
    public class TopicResponse : Response, IResponse<TopicViewModel>
    {
        [DataMember(Name = "topic")]
        public TopicViewModel topic;

        public TopicViewModel Root
        {
            get
            {
                return topic;
            }
        }
    }
    
    // 返回邮件集合
    [DataContract]
    public class MailsResponse : Response, IResponse<ObservableCollection<TopicViewModel>>
    {
        [DataMember(Name = "mails")]
        public ObservableCollection<TopicViewModel> mails;

        public ObservableCollection<TopicViewModel> Root
        {
            get
            {
                return mails;
            }
        }
    }

    // 返回单封邮件
    [DataContract]
    public class MailResponse : Response, IResponse<TopicViewModel>
    {
        [DataMember(Name = "mail")]
        public TopicViewModel mail;

        public TopicViewModel Root
        {
            get
            {
                return mail;
            }
        }
    }

    // 返回版面集合
    // 符合类型： 收藏夹
    [DataContract]
    public class BoardsResponse : Response, IResponse<ObservableCollection<BoardViewModel>>
    {
        [DataMember(Name = "boards")]
        public ObservableCollection<BoardViewModel> boards;

        public ObservableCollection<BoardViewModel> Root
        {
            get
            {
                return boards;
            }
        }
    }

    // 返回首页热点
    [DataContract]
    public class HotTopicsResponse : Response, IResponse<ObservableCollection<HotTopicsViewModel>>
    {
        [DataMember(Name = "topics")]
        public ObservableCollection<HotTopicsViewModel> topics;

        public ObservableCollection<HotTopicsViewModel> Root
        {
            get
            {
                return topics;
            }
        }
    }

    // 返回用户认证Token
    [DataContract]
    public class TokenResponse : Response, IResponse<string>
    {
        [DataMember(Name = "token")]
        public string token;

        [DataMember(Name = "id")]
        public string id;

        [DataMember(Name = "name")]
        public string name;

        public string Root
        {
            get
            {
                return token;
            }
        }
    }
}
