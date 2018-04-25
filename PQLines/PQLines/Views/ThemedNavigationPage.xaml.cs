using Xamarin.Forms;

namespace PQLines.Views
{
    public partial class ThemedNavigationPage : NavigationPage
    {
        // The constructor parameters are required below because, unlike Content Pages when we "new up" Navigation Pages,
        // we have to pass in a content page (in our case, ThemedContentPage) as the argument into Xamarin.Forms.NavigationPage
        // Refer to method "CreateThemedNavPage<TViewModel>(TViewModel viewModel)" in ViewFactory.cs
        public ThemedNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}