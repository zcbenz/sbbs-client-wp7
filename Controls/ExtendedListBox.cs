﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Collections;

namespace sbbs_client_wp7
{
    public class ExtendedListBox : ListBox
    {
        protected bool _isBouncy = false;
        private bool alreadyHookedScrollEvents = false;

        TextBlock LoadMoreText;

        // 属性：是否完全载入
        public bool IsFullyLoaded
        {
            get { return (bool)GetValue(IsFullyLoadedProperty); }
            set { SetValue(IsFullyLoadedProperty, value); }
        }

        public static readonly DependencyProperty IsFullyLoadedProperty =
            DependencyProperty.Register("IsFullyLoaded", typeof(bool), typeof(ExtendedListBox), new PropertyMetadata(false));
        
        // 事件：拖到边界处
        public delegate void OnNextPage(object sender, NextPageEventArgs e);
        public event OnNextPage NextPage;

        public ExtendedListBox()
        {
            this.Loaded += new RoutedEventHandler(ListBox_Loaded);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            LoadMoreText = (TextBlock)GetTemplateChild("LoadMoreText");
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollBar sb = null;
            ScrollViewer sv = null;
            if (alreadyHookedScrollEvents)
                return;
 
            alreadyHookedScrollEvents = true;
            this.AddHandler(ExtendedListBox.ManipulationCompletedEvent, (EventHandler<ManipulationCompletedEventArgs>)LB_ManipulationCompleted, true);
            sb = (ScrollBar)FindElementRecursive(this, typeof(ScrollBar));
            sv = (ScrollViewer)FindElementRecursive(this, typeof(ScrollViewer));
 
            if (sv != null)
            {
                // Visual States are always on the first child of the control template 
                FrameworkElement element = VisualTreeHelper.GetChild(sv, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup vgroup = FindVisualState(element, "VerticalCompression");
                    VisualStateGroup hgroup = FindVisualState(element, "HorizontalCompression"); 
                    if (vgroup != null)
                        vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(vgroup_CurrentStateChanging);
                    if (hgroup != null)
                        hgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(hgroup_CurrentStateChanging);
                }
            }
 
        }

        private void hgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == "CompressionLeft")
                _isBouncy = true;
            else if (e.NewState.Name == "CompressionRight")
                _isBouncy = true;
            else if (e.NewState.Name == "NoHorizontalCompression")
                _isBouncy = false;
        }

        private void vgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == "CompressionTop")
            {
                _isBouncy = true;
            }
            else if (e.NewState.Name == "CompressionBottom")
            {
                _isBouncy = true;
                if (NextPage != null && !IsFullyLoaded)
                    NextPage(this, new NextPageEventArgs());
            }
            else if (e.NewState.Name == "NoVerticalCompression")
                _isBouncy = false;
        }

        private void LB_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (_isBouncy)
                _isBouncy = false;
        }

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                    {
                        return element as UIElement;
                    }
                    else
                    {
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                    }
                }
            }
            return returnElement;
        }

        private VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            foreach (VisualStateGroup group in groups)
                if (group.Name == name)
                    return group;

            return null;
        }
    }

    public class NextPageEventArgs : EventArgs
    {
    }
}
