namespace AntlrVSIX
{
    using System.Windows.Media;

    internal static class Constants
    {
        /* Current version of the AntlrVSIX extension. Note, I don't know
         * how to automatically synchonize a version string from the package
         * manifest (source.extension.vsixmanifest), so the version must also
         * be set in the manifest file.
         * Make sure to change the version in source.extension.vsixmanifest as well!
         */
        public const string Version = "2.0.2";

        /* Invariants of the AntlrVSIX extension.
         */
        public const string LanguageName = "Antlr";
        public const string ContentType = "Antlr";
        public const string TextEditorSettingsRegistryKey = LanguageName;
        public const string FileExtension = ".g4;.g";
        public const string PackageGuidString = "7e37eef9-8cbe-4b10-81f7-66413cd2c9d3";
        public const string guidMenuAndCommandsCmdSet =                       "{1B4BF8E5-B60D-4DF7-95CB-FF3684750363}";
        public const string guidVSPackageCommandCodeWindowContextMenuCmdSet = "{0c1acc31-15ac-417c-86b2-eefdc669e8bf}";
        public const string guidFindAllReferences =                           "{9bd1fb43-7317-4992-8f66-8c2277ac652d}";

        /* Tagging and classification types. */
        public const string ClassificationNameTerminal = "terminal";
        public const string ClassificationNameNonterminal = "nonterminal";
        public const string ClassificationNameComment = "acomment";
        public const string ClassificationNameKeyword = "akeyword";
        public const string ClassificationNameLiteral = "aliteral";

        /* Color scheme for the tagging. */
        public static Color NormalColorTextForegroundTerminal = Colors.Orange;
        public static Color NormalColorTextForegroundNonterminal = Colors.Purple;
        public static Color NormalColorTextForegroundComment = Colors.Green;
        public static Color NormalColorTextForegroundKeyword = Colors.Blue;
        public static Color NormalColorTextForegroundLiteral = Colors.Red;

        public static Color InvertedColorTextForegroundTerminal = Colors.LightYellow;
        public static Color InvertedColorTextForegroundNonterminal = Colors.LightPink;
        public static Color InvertedColorTextForegroundComment = Colors.LightGreen;
        public static Color InvertedColorTextForegroundKeyword = Colors.LightBlue;
        public static Color InvertedColorTextForegroundLiteral = Colors.Red;
    }
}
