using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace PQLines.Droid.CustomRenderers
{
    // Android has an issue whereby if the user rapidly presses a navigation button (i.e. one of the "blue" buttons) multiple times, before the page navigation animation has a chance to complete,
    // all sorts of strange navigation stack issues occur, such as duplicates of a page being pushed onto the stack, and throwing critical exceptions (i.e. "Parent page must not already have a parent")
    // This class fixes this problem by only capturing a single click of any particular button (and ignoring all subsequent clicks) until the next page is displayed (i.e. indicated by disposal of the previous page's buttons)

    // All button renderers (e.g. ThemedNavigationButtonRenderer) should be using this class, instead of "ButtonRenderer"
    public class ButtonRendererWithNavFix : ButtonRenderer
    {
        private bool _wasAlreadyClicked;

        static ButtonRendererWithNavFix()
        {
            ButtonHadJustBeenDisposed = false;
        }

        public static bool ButtonHadJustBeenDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ButtonHadJustBeenDisposed = true;
        }

        protected void CustomClickHandler(Button button)
        {

            _wasAlreadyClicked = false;


            button.Click += (sender, args) =>
            {
                if (ButtonHadJustBeenDisposed)
                {
                    _wasAlreadyClicked = false;
                }

                if (!(_wasAlreadyClicked))
                {
                    var buttonRenderer = button.Tag as ThemedNavigationButtonRenderer;

                    if (buttonRenderer != null)
                    {
                        _wasAlreadyClicked = true;
                        buttonRenderer.Element.Command.Execute(buttonRenderer.Element.CommandParameter);
                        ButtonHadJustBeenDisposed = false;
                    }
                }
            };
        }
    }
}