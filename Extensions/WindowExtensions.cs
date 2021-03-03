using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeStats.Extensions
{
    static class WindowExtensions
    {
        public static bool IsWindowOpened<T>(out T window) where T : Window
        {
            bool isWindowOpen = false;
            window = null;

            foreach (Window win in Application.Current.Windows)
            {
                if (win is T upcastedWindow)
                {
                    window = upcastedWindow;
                    isWindowOpen = true;
                    break;
                }
            }

            return isWindowOpen;
        }
    }
}
