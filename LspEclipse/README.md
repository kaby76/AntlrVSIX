# LspEclipse

This is a very simple extension for VS2019 to run the Eclipse LSP server. I provide it because
MS does not seem to provide a generic LSP client extension. It uses
Microsoft.VisualStudio.LanguageServer.Client, which implements the UI for a LSP client in Visual
Studio IDE.

This extension has an options box to set the location and version of the server to download from Eclipse,
and to set the workspace. There isn't much otherwise, as most of the functionality is provided by the VSSDK.

Any questions, please read the code. I can be reached at ken . domino @ gmail . com.
