using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.MsgEvents;

namespace WPFClient.Views
{
    /// <summary>
    /// DeviceInfo.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfo : UserControl
    {
        private readonly IEventAggregator Aggregator;
        public DeviceInfo(IEventAggregator _aggregator)
        {
            InitializeComponent();
            Aggregator = _aggregator;
            Aggregator.GetEvent<MsgEvent>().Subscribe(Sub);
        }

        private void Sub(String obj)
        {
            SubmitBar.MessageQueue.Enqueue(obj);
        }
    }
}
