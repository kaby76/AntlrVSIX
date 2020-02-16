using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer
{
    class Goto
    {
        void main()
        {
//            // Get the symbol name as a string.
//            var symbol_name = symbol.Name;
//            var capitalized_symbol_name = Capitalized(symbol_name);

//            // Parse all the C# files in the solution.
//            Dictionary<string, SyntaxTree> trees = new Dictionary<string, SyntaxTree>();
//            foreach (var item in DteExtensions.SolutionFiles(application))
//            {
//                string file_name = item.Name;
//                if (file_name != null)
//                {
//                    string prefix = file_name.TrimSuffix(".cs");
//                    if (prefix == file_name) continue;
//                    try
//                    {
//                        object prop = item.Properties.Item("FullPath").Value;
//                        string ffn = (string)prop;
//                        StreamReader sr = new StreamReader(ffn);
//                        string code = sr.ReadToEnd();
//                        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
//                        trees[ffn] = tree;
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }
//            }

//            // Find all occurrences of visitor class.
//            List<ClassDeclarationSyntax> found_class = new List<ClassDeclarationSyntax>();
//            string class_file_path = null;
//            try
//            {
//                foreach (var kvp in trees)
//                {
//                    var file_name = kvp.Key;
//                    var tree = kvp.Value;

//                    // Look for IParseTreeListener or IParseTreeVisitor classes.
//                    var root = (CompilationUnitSyntax)tree.GetRoot();
//                    if (root == null) continue;
//                    foreach (var nm in root.Members)
//                    {
//                        var namespace_member = nm as NamespaceDeclarationSyntax;
//                        if (namespace_member == null) continue;
//                        foreach (var cm in namespace_member.Members)
//                        {
//                            var class_member = cm as ClassDeclarationSyntax;
//                            if (class_member == null) continue;
//                            var bls = class_member.BaseList;
//                            if (bls == null) continue;
//                            var types = bls.Types;
//                            Regex reg = new Regex("[<].+[>]");
//                            foreach (var type in types)
//                            {
//                                var s = type.ToString();
//                                s = reg.Replace(s, "");
//                                if (s.ToString() == listener_baseclass_name)
//                                {
//                                    // Found the right class.
//                                    found_class.Add(class_member);
//                                    throw new Exception();
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }

//            if (found_class.Count == 0)
//            {
//                if (!Options.Option.GetBoolean("GenerateVisitorListener"))
//                    return;

//                // Look in grammar directory for any C# files.
//                string name_space = null;
//                string ffn = Path.GetFullPath(g4_file_path);
//                ffn = Path.GetDirectoryName(ffn);
//                foreach (var i in DteExtensions.SolutionFiles(application))
//                {
//                    string file_name = i.Name;
//                    if (file_name != null)
//                    {
//                        string prefix = file_name.TrimSuffix(".cs");
//                        if (prefix == file_name) continue;
//                        try
//                        {
//                            object prop = i.Properties.Item("FullPath").Value;
//                            string ffncs = (string)prop;
//                            // Look for namespace.
//                            var t = trees[ffncs];
//                            if (t == null) continue;
//                            var root = t.GetCompilationUnitRoot();
//                            foreach (var nm in root.Members)
//                            {
//                                var namespace_member = nm as NamespaceDeclarationSyntax;
//                                if (namespace_member == null) continue;
//                                name_space = namespace_member.Name.ToString();
//                                break;
//                            }
//                        }
//                        catch (Exception)
//                        {
//                        }
//                    }
//                }
//                if (name_space == null) name_space = "Generated";

//                // Create class.
//                string clazz = visitor ? $@"
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace {name_space}
//{{
//    class {listener_class_name}<Result> : {listener_baseclass_name}<Result>
//    {{
//        //public override Result VisitA([NotNull] A3Parser.AContext context)
//        //{{
//        //  return VisitChildren(context);
//        //}}
//    }}
//}}
//"
//                : $@"
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace {name_space}
//{{
//    class {listener_class_name} : {listener_baseclass_name}
//    {{
//        //public override void EnterA(A3Parser.AContext context)
//        //{{
//        //    base.EnterA(context);
//        //}}
//        //public override void ExitA(A3Parser.AContext context)
//        //{{
//        //    base.ExitA(context);
//        //}}
//    }}
//}}
//";

//                class_file_path = ffn + Path.DirectorySeparatorChar + listener_class_name + ".cs";
//                System.IO.File.WriteAllText(class_file_path, clazz);
//                var item = ProjectHelpers.GetSelectedItem();
//                string folder = FindFolder(item);
//                if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
//                    return;
//                var file = new FileInfo(class_file_path);
//                var selectedItem = Workspaces.Workspace.Instance.FindDocument(class_file_path);
//                if (selectedItem == null)
//                {
//                    //var selectedProject = item as Project;
//                    //Project project = selectedItem?.ContainingProject ?? selectedProject ?? null;
//                    //var projectItem = project.AddFileToProject(file);
//                }
//                // Redo parse.
//                try
//                {
//                    StreamReader sr = new StreamReader(class_file_path);
//                    string code = sr.ReadToEnd();
//                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
//                    trees[class_file_path] = tree;
//                }
//                catch (Exception)
//                {
//                }
//                // Redo find class.
//                try
//                {
//                    var tree = trees[class_file_path];
//                    var save = class_file_path;
//                    class_file_path = null;
//                    // Look for IParseTreeListener or IParseTreeVisitor classes.
//                    var root = (CompilationUnitSyntax)tree.GetRoot();
//                    foreach (var nm in root.Members)
//                    {
//                        var namespace_member = nm as NamespaceDeclarationSyntax;
//                        if (namespace_member == null) continue;
//                        foreach (var cm in namespace_member.Members)
//                        {
//                            var class_member = cm as ClassDeclarationSyntax;
//                            if (class_member == null) continue;
//                            var bls = class_member.BaseList;
//                            if (bls == null) continue;
//                            var types = bls.Types;
//                            Regex reg = new Regex("[<].+[>]");
//                            foreach (var type in types)
//                            {
//                                var s = type.ToString();
//                                s = reg.Replace(s, "");
//                                if (s.ToString() == listener_baseclass_name)
//                                {
//                                    // Found the right class.
//                                    found_class.Add(class_member);
//                                    throw new Exception();
//                                }
//                            }
//                        }
//                    }
//                }
//                catch
//                {
//                }
//            }

//            // Look for enter or exit method for symbol.
//            MethodDeclarationSyntax found_member = null;
//            bool ctl = true; //CtrlKeyState.GetStateForView(grammar_view).Enabled;
//            var capitalized_member_name = "";
//            if (visitor) capitalized_member_name = "Visit" + capitalized_symbol_name;
//            else if (ctl) capitalized_member_name = "Exit" + capitalized_symbol_name;
//            else capitalized_member_name = "Enter" + capitalized_symbol_name;
//            var capitalized_grammar_name = Capitalized(grammar_name);
//            try
//            {
//                foreach (var fc in found_class)
//                {
//                    foreach (var me in fc.Members)
//                    {
//                        var method_member = me as MethodDeclarationSyntax;
//                        if (method_member == null) continue;
//                        if (method_member.Identifier.ValueText.ToLower() == capitalized_member_name.ToLower())
//                        {
//                            found_member = method_member;
//                            throw new Exception();
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }
//            if (found_member == null)
//            {
//                if (!Options.Option.GetBoolean("GenerateVisitorListener"))
//                    return;

//                // Find point for edit.
//                var fc = found_class.First();
//                var here = fc.OpenBraceToken;
//                var spn = here.FullSpan;
//                var end = spn.End;

//                IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
//                if (vstv == null)
//                {
//                    IVsTextViewExtensions.ShowFrame(class_file_path);
//                    vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
//                }
//                IWpfTextView wpftv = vstv.GetIWpfTextView();
//                if (wpftv == null) return;
//                ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;

//                var res = vstv.GetBuffer(out IVsTextLines ppBuffer);

//                var nss = new SnapshotSpan(cc, spn.End + 1, 0);
//                var txt_span = nss.Span;

//                int line_number;
//                int colum_number;
//                vstv.GetLineAndColumn(txt_span.Start, out line_number, out colum_number);
//                res = ppBuffer.CreateEditPoint(line_number, colum_number, out object ppEditPoint);
//                EditPoint editPoint = ppEditPoint as EditPoint;
//                // Create class.
//                string member = visitor ? $@"
//public override Result {capitalized_member_name}([NotNull] {capitalized_grammar_name}Parser.{capitalized_symbol_name}Context context)
//{{
//    return VisitChildren(context);
//}}
//"
//                    : $@"
//public override void {capitalized_member_name}({capitalized_grammar_name}Parser.{capitalized_symbol_name}Context context)
//{{
//    base.{capitalized_member_name}(context);
//}}
//";
//                editPoint.Insert(member);
//                // Redo parse.
//                try
//                {
//                    vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
//                    if (vstv == null)
//                    {
//                        IVsTextViewExtensions.ShowFrame(class_file_path);
//                        vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
//                    }
//                    var text_buffer = vstv.GetITextBuffer();
//                    var code = text_buffer.GetBufferText();
//                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
//                    trees[class_file_path] = tree;
//                }
//                catch (Exception)
//                {
//                }
//                // Redo find class.
//                try
//                {
//                    var tree = trees[class_file_path];
//                    var save = class_file_path;
//                    class_file_path = null;
//                    // Look for IParseTreeListener or IParseTreeVisitor classes.
//                    var root = (CompilationUnitSyntax)tree.GetRoot();
//                    foreach (var nm in root.Members)
//                    {
//                        var namespace_member = nm as NamespaceDeclarationSyntax;
//                        if (namespace_member == null) continue;
//                        foreach (var cm in namespace_member.Members)
//                        {
//                            var class_member = cm as ClassDeclarationSyntax;
//                            if (class_member == null) continue;
//                            var bls = class_member.BaseList;
//                            if (bls == null) continue;
//                            var types = bls.Types;
//                            Regex reg = new Regex("[<].+[>]");
//                            foreach (var type in types)
//                            {
//                                var s = type.ToString();
//                                s = reg.Replace(s, "");
//                                if (s.ToString() == listener_baseclass_name)
//                                {
//                                    // Found the right class.
//                                    found_class.Add(class_member);
//                                    class_file_path = save;
//                                    throw new Exception();
//                                }
//                            }
//                        }
//                    }
//                }
//                catch
//                {
//                }
//                try
//                {
//                    foreach (var fcc in found_class)
//                    {
//                        foreach (var me in fcc.Members)
//                        {
//                            var method_member = me as MethodDeclarationSyntax;
//                            if (method_member == null) continue;
//                            if (method_member.Identifier.ValueText.ToLower() == capitalized_member_name.ToLower())
//                            {
//                                found_member = method_member;
//                                throw new Exception();
//                            }
//                        }
//                    }
//                }
//                catch
//                {
//                }
//            }
        }
        //private static string FindFolder(object item)
        //{
        //    if (item == null)
        //        return null;
        //    DTE application = Workspaces.Help.GetApplication();
        //    if (application == null) return "";
        //    if (application.ActiveWindow is Window2 window && window.Type == vsWindowType.vsWindowTypeDocument)
        //    {
        //        // if a document is active, use the document's containing directory
        //        var doc = application.ActiveDocument;
        //        if (doc != null && !string.IsNullOrEmpty(doc.FullName))
        //        {
        //            Workspaces.Document docItem = Workspaces.Workspace.Instance.FindDocument(doc.FullName);
        //            if (docItem != null)
        //            {
        //                string fileName = docItem.FullPath;
        //                if (System.IO.File.Exists(fileName))
        //                    return Path.GetDirectoryName(fileName);
        //            }
        //        }
        //    }

        //    string folder = null;

        //    var projectItem = item as ProjectItem;
        //    if (projectItem != null && "{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}" == projectItem.Kind) //Constants.vsProjectItemKindVirtualFolder
        //    {
        //        ProjectItems items = projectItem.ProjectItems;
        //        foreach (ProjectItem it in items)
        //        {
        //            if (System.IO.File.Exists(it.FileNames[1]))
        //            {
        //                folder = Path.GetDirectoryName(it.FileNames[1]);
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var project = item as Project;
        //        if (projectItem != null)
        //        {
        //            string fileName = projectItem.FileNames[1];

        //            if (System.IO.File.Exists(fileName))
        //            {
        //                folder = Path.GetDirectoryName(fileName);
        //            }
        //            else
        //            {
        //                folder = fileName;
        //            }
        //        }
        //        else if (project != null)
        //        {
        //            folder = project.GetRootFolder();
        //        }
        //    }
        //    return folder;
        //}

        //string Capitalized(string s)
        //{
        //    if (string.IsNullOrEmpty(s))
        //    {
        //        return string.Empty;
        //    }
        //    return char.ToUpper(s[0]) + s.Substring(1);
        //}
    }
}
