using System;
using System.IO;
using LanguageServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workspaces;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public static Document CheckDoc(string path)
        {
            string file_name = path;
            Document document = Workspaces.Workspace.Instance.FindDocument(file_name);
            if (document == null)
            {
                document = new Workspaces.Document(file_name);
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(file_name))
                    {
                        // Read the stream to a string, and write the string to the console.
                        string str = sr.ReadToEnd();
                        document.Code = str;
                    }
                }
                catch (IOException)
                {
                }
                Project project = Workspaces.Workspace.Instance.FindProject("Misc");
                if (project == null)
                {
                    project = new Project("Misc", "Misc", "Misc");
                    Workspaces.Workspace.Instance.AddChild(project);
                }
                project.AddDocument(document);
                document.Changed = true;
                _ = ParserDetailsFactory.Create(document);
                _ = LanguageServer.Module.Compile();
            }
            return document;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var cwd = Directory.GetCurrentDirectory();
            Document lexer_doc = CheckDoc("../../../../LanguageServer/ANTLRv4Lexer.g4");
            Document document = CheckDoc("../../../../LanguageServer/ANTLRv4Parser.g4");
            int line = 1;
            int character = 1;
            int index = LanguageServer.Module.GetIndex(line, character, document);
            (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
            if (back.Item1 != line || back.Item2 != character) throw new Exception();
            QuickInfo quick_info = LanguageServer.Module.GetQuickInfo(index, document);
            if (quick_info != null) throw new Exception();
        }
    }
}
