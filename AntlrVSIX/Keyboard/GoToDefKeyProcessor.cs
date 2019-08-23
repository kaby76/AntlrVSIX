namespace AntlrVSIX.Keyboard
{
    using Microsoft.VisualStudio.Text.Editor;
    using System.Windows.Input;

    internal sealed class GoToDefKeyProcessor : KeyProcessor
    {
        private CtrlKeyState _state;

        public GoToDefKeyProcessor(CtrlKeyState state)
        {
            _state = state;
        }

        void UpdateState(KeyEventArgs args)
        {
            _state.Enabled = (args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0 &&
                             (args.KeyboardDevice.Modifiers & ModifierKeys.Shift) == 0;
        }

        public override void PreviewKeyDown(KeyEventArgs args)
        {
            UpdateState(args);
        }

        public override void PreviewKeyUp(KeyEventArgs args)
        {
            UpdateState(args);
        }
    }
}
