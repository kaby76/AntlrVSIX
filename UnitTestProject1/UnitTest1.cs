using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            }
            document.Changed = true;
            _ = ParserDetailsFactory.Create(document);
            _ = LanguageServer.Module.Compile();
            return document;
        }

        [TestMethod]
        public void TestIndexQuickInfo()
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

        [TestMethod]
        public void TestIndexQuickInfo2()
        {
            var cwd = Directory.GetCurrentDirectory();
            Document lexer_doc = CheckDoc("../../../../LanguageServer/ANTLRv4Lexer.g4");
            Document document = CheckDoc("../../../../LanguageServer/ANTLRv4Parser.g4");
            // Position at the "grammarSpec" rule, beginning of LHS symbol.
            // All lines and columns are zero based in LSP.
            int line = 45;
            int character = 0;
            int index = LanguageServer.Module.GetIndex(line, character, document);
            (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
            if (back.Item1 != line || back.Item2 != character) throw new Exception();
            QuickInfo quick_info = LanguageServer.Module.GetQuickInfo(index, document);
            if (quick_info == null) throw new Exception();
            if (quick_info.Range.Start.Value != 1994 || quick_info.Range.End.Value != 2005) throw new Exception();
        }

        [TestMethod]
        public void TestFindDef()
        {
            var cwd = Directory.GetCurrentDirectory();
            Document lexer_doc = CheckDoc("../../../../LanguageServer/ANTLRv4Lexer.g4");
            Document document = CheckDoc("../../../../LanguageServer/ANTLRv4Parser.g4");
            // Position at the "grammarSpec" rule, beginning of RHS symbol "grammarDecl".
            // All lines and columns are zero based in LSP.
            int line = 46;
            int character = 18;
            int index = LanguageServer.Module.GetIndex(line, character, document);
            (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
            if (back.Item1 != line || back.Item2 != character) throw new Exception();
            IList<Location> found = LanguageServer.Module.FindDefs(index, document);
            List<object> locations = new List<object>();
            if (found.Count != 1) throw new Exception();
            if (found.First().Range.Start.Value != 2084 || found.First().Range.End.Value != 2094) throw new Exception();
        }

        [TestMethod]
        public void TestFindAllRefs()
        {
            var cwd = Directory.GetCurrentDirectory();
            Document document = CheckDoc("../../../../corpus-for-codebuff/A.g4");
            // Position at the "grammarSpec" rule, beginning of RHS symbol "grammarDecl".
            // All lines and columns are zero based in LSP.
            int line = 3;
            int character = 6;
            int index = LanguageServer.Module.GetIndex(line, character, document);
            (int, int) back = LanguageServer.Module.GetLineColumn(index, document);
            if (back.Item1 != line || back.Item2 != character) throw new Exception();
            var found = LanguageServer.Module.FindRefsAndDefs(index, document).ToList();
            List<object> locations = new List<object>();
  //          if (found.Count != 1) throw new Exception();
  //          if (found.First().Range.Start.Value != 2084 || found.First().Range.End.Value != 2094) throw new Exception();
        }
    }
}
