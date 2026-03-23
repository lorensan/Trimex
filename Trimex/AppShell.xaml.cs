using Trimex.Pages;

namespace Trimex
{
    public partial class AppShell : Shell
    {
        public AppShell(HomePage homePage)
        {
            InitializeComponent();

            Items.Add(new ShellContent
            {
                Route = nameof(HomePage),
                Title = "Trimex",
                Content = homePage
            });
        }
    }
}
