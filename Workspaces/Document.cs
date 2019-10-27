namespace Workspaces
{
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Collections.Generic;
    using System.IO;

    public class Document : Container
    {
        IVsHierarchy _ide_object;
        string _contents;
        Dictionary<string, string> _properties = new Dictionary<string, string>();
        Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();

        public Document(string ffn, string name)
        {
            FullPath = ffn;
            Name = name;
            Changed = false;
        }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public bool Changed { get; set; }

        public string Code
        {
            get
            {
                if (_contents == null)
                {
                    StreamReader sr = new StreamReader(FullPath);
                    _contents = sr.ReadToEnd();
                    Changed = true;
                }
                return _contents;
            }
            set
            {
                if (_contents == value)
                {
                    return;
                }
                Changed = true;
                _contents = value;
            }
        }

        public void AddProperty(string name, string value)
        {
            _properties[name] = value;
            _lazy_evaluated[name] = true;
        }

        public void AddProperty(string name)
        {
            _properties[name] = null;
            _lazy_evaluated[name] = false;
        }

        public string GetProperty(string name)
        {
            _lazy_evaluated.TryGetValue(name, out bool evaluated);
            string result;
            if (_ide_object != null && !evaluated)
            {
                object n;
                _ide_object.GetProperty(0, (int)__VSHPROPID.VSHPROPID_Name, out n);
                _properties[name] = null;
            }
            else
            {
                _properties.TryGetValue(name, out string r);
                result = r;
            }
            return null;
        }

        public IParseTree GetParseTree()
        {
            return null;
        }

        public Dictionary<TerminalNodeImpl, int> Refs
        {
            get;
            set;
        }

        public Dictionary<TerminalNodeImpl, int> Defs
        {
            get;
            set;
        }

        public override Document FindDocument(string ffn)
        {
            if (this.FullPath.ToLower() == ffn.ToLower())
                return this;
            return null;
        }
    }
}
