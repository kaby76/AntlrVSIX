using System.Collections.Generic;

namespace LanguageServer
{
    public class Compiler
    {
        public static void Compile()
        {
            var ws = Workspaces.Workspace.Instance;
            List<ParserDetails> to_do = new List<ParserDetails>();

            foreach (var project in ws.Projects)
            {
                foreach (var document in project.Documents)
                {
                    string file_name = document.FullPath;
                    if (file_name == null) continue;
                    var gd = LanguageServer.GrammarDescriptionFactory.Create(file_name);
                    if (gd == null) continue;
                    if (!System.IO.File.Exists(file_name)) continue;
                    var item = Workspaces.Workspace.Instance.FindDocumentFullName(file_name);
                    var pd = ParserDetailsFactory.Create(item);
                    if (!pd.Changed) continue;
                    to_do.Add(pd);
                }
            }

            foreach (var pd in to_do)
            {
                pd.Parse();
            }
            foreach (var pd in to_do)
            {
                pd.Pass1();
            }
            foreach (var pd in to_do)
            {
                pd.Pass2();
            }
            foreach (var pd in to_do)
            {
                pd.GatherDefs();
            }
            foreach (var pd in to_do)
            {
                pd.GatherRefs();
            }
        }
    }
}
