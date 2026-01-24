using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Extensions
{
    public class PasswordBoxExtend
    {
        public static string GetPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(PwdProperty);
        }

        public static void SetPwd(DependencyObject obj, string value)
        {
            obj.SetValue(PwdProperty, value);
        }

        public static readonly DependencyProperty PwdProperty =
            DependencyProperty.RegisterAttached("Pwd", typeof(string),
                typeof(PasswordBoxExtend), new PropertyMetadata("", OnPasswordChanged));

        /// <summary>
        /// パスワード変更時処理
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pwdBox = d as PasswordBox;
            string newPwd = e.NewValue as string;
            if (pwdBox != null && pwdBox.Password != newPwd)
            {
                pwdBox.Password = newPwd;
            }
        }

    }
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PasswordBoxExtend.GetPwd(passwordBox);
            if (passwordBox != null && passwordBox.Password != password)
            {
                PasswordBoxExtend.SetPwd(passwordBox, passwordBox.Password);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }
    }
}
