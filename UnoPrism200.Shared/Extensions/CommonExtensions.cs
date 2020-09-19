using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UnoPrism200.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        ///     하위 오브젝트 목록 반환
        /// </summary>
        public static IList<T> FindChilds<T>(this DependencyObject depObj) where T
            : DependencyObject
        {
            if (depObj == null) return null;
            IList<T> returnList = new List<T>();

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T tChild)
                {
                    returnList.Add(tChild);
                    if (VisualTreeHelper.GetChildrenCount(tChild) <= 0) continue;
                    var list = FindChilds<T>(tChild);
                    if (list != null && list.Any()) list.ForEach(t => returnList.Add(t));
                }
                else
                {
                    var list = FindChilds<T>(child);
                    if (list != null && list.Any()) list.ForEach(t => returnList.Add(t));
                }
            }

            return returnList.Any() ? returnList : null;
        }

        /// <summary>
        ///     부모 찾아서 반환하기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject depObj) where T : DependencyObject
        {
            while (true)
            {
                var element = depObj as FrameworkElement;
                if (element?.Parent == null) return default(T);
                if (element.Parent is T variable) return variable;
                depObj = element.Parent;
            }
        }
    }
}
