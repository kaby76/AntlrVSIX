using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.GrammarDescription
{
    public class Project
    {
        string _name;
        string _ffn;
        List<Document> _documents = new List<Document>();
        Dictionary<string, string> _properties = new Dictionary<string, string>();
        Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();
        Func<string, object, string> _get_property;
        object _get_property_data;

        public Project(string name, string ffn, Func<string, object, string> get_property, object get_property_data)
        {
            _get_property = get_property;
            _get_property_data = get_property_data;
            _name = name;
            _ffn = ffn;
        }

        public Document AddDocument(Document doc)
        {
            _documents.Add(doc);
            return doc;
        }

        public void AddProperty(string name)
        {
            _properties[name] = null;
            _lazy_evaluated[name] = false;
        }

        public void AddProperty(string name, string value)
        {
            _properties[name] = value;
            _lazy_evaluated[name] = true;
        }

        public string GetProperty(string name)
        {
            _lazy_evaluated.TryGetValue(name, out bool evaluated);
            string result;
            if (!evaluated)
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

        public IEnumerable<Document> Documents
        {
            get { return _documents; }
        }
    }
}
