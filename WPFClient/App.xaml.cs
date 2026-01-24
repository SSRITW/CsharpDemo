using Prism.Ioc;
using Prism.Mvvm;
using System.Configuration;
using System.Data;
using System.Windows;
using WPFClient.HttpClients;
using WPFClient.ViewModels;
using WPFClient.Views;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 启动主窗口 
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<Main>();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // dialog登録
            containerRegistry.RegisterDialog<Login,LoginViewModel>();

            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made:Parameters.Of.Type<string>(serviceKey:"webUrl"));
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("Login", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Shutdown();
                    return;
                }
            });
            base.OnInitialized();
        }

        /*protected override IModuleCatalog CreateModuleCatalog()
        {
           //项目内部模块（dll文件）加载
           return new DirectoryModuleCatalog(){ ModulePath=@"./Modules"};
        }*/

        /*protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<项目应用外部模块>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }*/


        // カスタマイズしたViewModelLocatorの設定
        /*protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(SetViewModel);
        }

        private Type? SetViewModel(Type viewType)
        {
            var viewName = viewType.FullName;
            string vmPath = viewName!.Replace("Views", "ViewModels")+"VM";
            return Type.GetType(vmPath);
        }*/

    }

}
