﻿
namespace AntlrVSIX.AggregateTagger
{
    using AntlrVSIX.Package;
    using AntlrVSIX.Tagger;
    using Color = System.Drawing.Color;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;
    using System;

    // Please refer to Language Service and Editor Extension Points,
    // https://msdn.microsoft.com/en-us/library/dd885244.aspx,
    // for information on how this Managed Extensiblility Framework (MEF)
    // extension hooks into Visual Studio 2015.

    [Export(typeof(ITaggerProvider))]
    [ContentType(AntlrVSIX.Constants.ContentType)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class AntlrClassifierProvider : ITaggerProvider
    {
        [Export]
        [Name(AntlrVSIX.Constants.LanguageName)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition AntlrContentType = null;

        [Export]
        [FileExtension(AntlrVSIX.Constants.FileExtension)]
        [ContentType(AntlrVSIX.Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition AntlrFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IClassificationFormatMapService ClassificationFormatMapService = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            // Ensure package is loaded.
            AntlrLanguagePackage package = AntlrLanguagePackage.Instance;
            VSColorTheme.ThemeChanged += UpdateTheme;
            var antlrTagAggregator = aggregatorFactory.CreateTagAggregator<AntlrTokenTag>(buffer);
            return new AntlrClassifier(null, buffer, antlrTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }

        private void UpdateTheme(EventArgs e)
        {
            Color defaultBackground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            Color defaultForeground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);
            var formatMap = ClassificationFormatMapService.GetClassificationFormatMap(category: "code");
            try
            {
                formatMap.BeginBatchUpdate();
            }
            finally
            {
                formatMap.EndBatchUpdate();
            }
        }
    }
}