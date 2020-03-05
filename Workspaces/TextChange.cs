namespace Workspaces
{
    public class TextChange
    {
        private readonly Range _range;
        private readonly string _replacement;

        public TextChange(Range range, string replacement)
        {
            _range = range;
            _replacement = replacement;
        }
    }
}
