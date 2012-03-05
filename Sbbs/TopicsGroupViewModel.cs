using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace sbbs_client_wp7.Sbbs
{
    [DataContract]
    public class TopicsGroupViewModel : ObservableCollection<TopicViewModel>
    {
        [DataMember(Name = "description")]
        public string Title { get; set; }
                        
        [DataMember(Name = "topics")]
        public ObservableCollection<TopicViewModel> Topics { get; set; }

        public bool HasItems
        {
            get
            {
                return Count != 0;
            }

            private set
            {
            }
        }

        // 将ObservableCollection的全部元素操作转向Topics
    }
}
