using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AntlrVSIX.Classification
{
    public class FontColor
    {
        public readonly Color? Foreground;
        public readonly Color? Background;

        public FontColor(Color? foreground = null, Color? background = null)
        {
            Foreground = foreground;
            Background = background;
        }
    }
}
