using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.GrammarDescription
{
    public class Compiler
    {
        public static void Compile()
        {
            var ws = AntlrVSIX.GrammarDescription.Workspace.Instance;
            List<ParserDetails> to_do = new List<ParserDetails>();

            foreach (var project in ws.Projects)
            {
                foreach (var document in project.Documents)
                {
                    string file_name = document.FullPath;
                    if (file_name == null) continue;
                    var gd = AntlrVSIX.GrammarDescription.GrammarDescriptionFactory.Create(file_name);
                    if (gd == null) continue;
                    if (!System.IO.File.Exists(file_name)) continue;
                    var item = AntlrVSIX.GrammarDescription.Workspace.Instance.FindDocumentFullName(file_name);
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
