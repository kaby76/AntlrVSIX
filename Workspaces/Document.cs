using System.Diagnostics;

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
        private string _parse_as;

        public Document(string ffn)
        {
            FullPath = Util.GetProperFilePathCapitalization(ffn);
            Changed = false;
        }

        public string ParseAs { get; set; }

        public string FullPath { get; private set; }

        public bool Changed { get; set; }

        public string Code
        {
            get
            {
                if (_contents == null)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(FullPath);
                        _contents = sr.ReadToEnd();
                        Changed = true;
                    }
                    catch {}
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
            if (name is null)
            {
                throw new System.ArgumentNullException(nameof(name));
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
            var normalized_ffn = Util.GetProperFilePathCapitalization(ffn);
            Debug.Assert(this.FullPath == Util.GetProperFilePathCapitalization(this.FullPath));

            if (normalized_ffn == this.FullPath)
            {
                return this;
            }

            return null;
        }
    }
}
