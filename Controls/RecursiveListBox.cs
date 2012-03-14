using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Collections.Specialized;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;

namespace CustomControls
{
    using ElementType = Sbbs.BoardViewModel;
    using CollectionType = ObservableCollection<Sbbs.BoardViewModel>;
    public class RecursiveListBox : ListBox
    {
        // 浏览历史
        private Stack<CollectionType> history = new Stack<CollectionType>();

        // 叶子结点点击事件
        public delegate void OnLeafItemTap(object sender, SelectionChangedEventArgs e);
        public event OnLeafItemTap LeafItemTap;

        // 捕捉选择事件
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // 清除选择
                SelectedIndex = -1;

                ElementType board = e.AddedItems[0] as ElementType;
                if (board.Leaf && LeafItemTap != null)
                {
                    // 叶子结点直接触发事件
                    LeafItemTap(sender, e);
                }
                else if (!board.Leaf)
                {
                    // 非叶子结点切换视角
                    if (board.EnglishName == "..")
                    {
                        ItemsSource = history.Pop() as CollectionType;
                    }
                    else
                    {
                        history.Push(ItemsSource as CollectionType);
                        ItemsSource = board.Boards;
                    }
                }
            }
        }

        public RecursiveListBox()
        {
            DefaultStyleKey = typeof(RecursiveListBox);
            SelectionChanged += ListBox_SelectionChanged;
        }
    }
}
