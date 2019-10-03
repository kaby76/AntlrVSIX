using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.IO;

namespace AntlrVSIX.GrammarDescription
{
    public class Document
    {
        string _name;
        string _ffn;
        string _contents;
        bool _changed_contents;
        Dictionary<string, string> _properties = new Dictionary<string, string>();

        public Document(string ffn)
        {
            _name = ffn;
            _changed_contents = false;
            _ffn = ffn;
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
        }

        public string GetProperty(string name)
        {
            _properties.TryGetValue(name, out string result);
            return result;
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
    }
}
