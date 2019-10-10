namespace Workspaces
{
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Collections.Generic;
    using System.IO;

    public class Document : Container
    {
        IVsHierarchy _ide_object;
        public int _hash;
        string _name;
        string _ffn;
        string _contents;
        bool _changed_contents;
        Dictionary<string, string> _properties = new Dictionary<string, string>();
        Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();

        public Document(IVsHierarchy ide_object, string ffn, string name)
        {
            _ide_object = ide_object;
            _hash = ide_object.GetHashCode();
            _ffn = ffn;
            _name = name;
            _changed_contents = false;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FullPath
        {
            get { return _ffn; }
            set { _ffn = value; }
        }

        public bool Changed
        {
            get { return _changed_contents; }
            set { _changed_contents = value; }
        }

        public string Code
        {
            get
            {
                if (_contents == null)
                {
                    StreamReader sr = new StreamReader(_ffn);
                    _contents = sr.ReadToEnd();
                    _changed_contents = true;
                }
                return _contents;
            }
            set
            {
                if (_contents == value)
                {
                    return;
                }
                _changed_contents = true;
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
