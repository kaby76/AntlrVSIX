using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.IO;
using System;


namespace AntlrVSIX.GrammarDescription
{
    public class Document
    {
        string _name;
        string _ffn;
        string _contents;
        bool _changed_contents;
        Dictionary<string, string> _properties = new Dictionary<string, string>();
        Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();
        Func<string, object, string> _get_property;
        object _get_property_data;

        public Document(string ffn, Func<string, object, string> get_property, object get_property_data)
        {
            _get_property = get_property;
            _get_property_data = get_property_data;
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
            if (_get_property != null && !evaluated)
            {
                result = _get_property(name, _get_property_data);
                _properties[name] = result;
            }
            else
            {
                _properties.TryGetValue(name, out string r);
                result = r;
            }
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
