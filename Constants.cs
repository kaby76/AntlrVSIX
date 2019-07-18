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
        public const string Version = "2.0";

        /* Invariants of the AntlrVSIX extension.
         */
        public const string LanguageName = "Antlr";
        public const string ContentType = "Antlr";
        public const string TextEditorSettingsRegistryKey = LanguageName;
        public const string FileExtension = ".g4";
        public const string PackageGuidString = "7e37eef9-8cbe-4b10-81f7-66413cd2c9d3";
        public const string guidMenuAndCommandsCmdSet = "{1B4BF8E5-B60D-4DF7-95CB-FF3684750363}";
        public const string guidVSPackageCommandCodeWindowContextMenuCmdSet = "{0c1acc31-15ac-417c-86b2-eefdc669e8bf}";

        /* Tagging and classification types. */
        public const string ClassificationNameTerminal = "terminal";
        public const string ClassificationNameNonterminal = "nonterminal";
        public const string ClassificationNameComment = "acomment";
        public const string ClassificationNameKeyword = "akeyword";
        public const string ClassificationNameLiteral = "aliteral";

        /* Color scheme for the tagging. */
        public static Color ColorTextForegroundTerminal = Colors.Lime;
        public static Color ColorTextForegroundNonterminal = Colors.Purple;
        public static Color ColorTextForegroundComment = Colors.Green;
        public static Color ColorTextForegroundKeyword = Colors.Blue;
        public static Color ColorTextForegroundLiteral = Colors.Red;


        //===================================================
        // The shit below I copied from somewhere, and they
        // are not used. But, they might be useful later, so
        // I including them for food for thought.
        //===================================================

        public const string ProjectImageList = "Microsoft.JImageList.bmp";
        public const string LibraryManagerGuid = "888888e5-b976-4366-9e98-e7bc01f1842c";
        public const string LibraryManagerServiceGuid = "88888859-2f95-416e-9e2b-cac4678e5af7";
        public const string ProjectFactoryGuid = "888888a0-9f3d-457c-b088-3a5042f75d52";
        public const string EditorFactoryGuid = "888888c4-36f9-4453-90aa-29fa4d2e5706";
        public const string ProjectNodeGuid = "8888881a-afb8-42b1-8398-e60d69ee864d";
        public const string GeneralPropertyPageGuid = "888888fd-3c4a-40da-aefb-5ac10f5e8b30";
        public const string DebugPropertyPageGuid = "9A46BC86-34CB-4597-83E5-498E3BDBA20A";
        public const string PublishPropertyPageGuid = "63DF0877-CF53-4975-B200-2B11D669AB00";
        public const string EditorFactoryPromptForEncodingGuid = "CA887E0B-55C6-4AE9-B5CF-A2EEFBA90A3E";
        public const string VirtualEnvPropertiesGuid = "45D3DC23-F419-4744-B55B-B897FAC1F4A2";
        // Do not change below info without re-requesting PLK:
        public const string ProjectSystemPackageGuid = "15490272-3C6B-4129-8E1D-795C8B6D8E9F"; //matches PLK
        //IDs of the icons for product registration (see Resources.resx)
        public const int IconIfForSplashScreen = 300;
        public const int IconIdForAboutBox = 400;
        public const int AddVirtualEnv = 0x4006;
        public const int ActivateVirtualEnv = 0x4007;
        public const int DeactivateVirtualEnv = 0x4008;
        public const int InstallJPackage = 0x4009;
        public const int UninstallJPackage = 0x400A;
        public const int CreateVirtualEnv = 0x400B;
        public const string VirtualEnvItemType = "VirtualEnv";
        public const string VirtualEnvCurrentEnvironment = "VirtualEnvEnabled";
        public const string VirtualEnvInterpreterId = "InterpreterId";
        public const string VirtualEnvInterpreterVersion = "InterpreterVersion";
        // Shows up before references
        public const int VirtualEnvContainerNodeSortPriority = 200;
    }
}
