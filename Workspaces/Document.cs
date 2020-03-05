namespace Workspaces
{
    using Antlr4.Runtime.Tree;
    using System.Collections.Generic;
    using System.IO;

    public class Document : Container
    {
        private string _contents;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();

        public Document(string ffn)
        {
            FullPath = ffn;
            Changed = false;
        }

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
            if (FullPath.ToLower() == ffn.ToLower())
            {
                return this;
            }

            return null;
        }
    }
}
