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

        public Project(string name, string ffn)
        {
            _name = name;
            _ffn = ffn;
        }

        public Document AddDocument(Document doc)
        {
            _documents.Add(doc);
            return doc;
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

        public IEnumerable<Document> Documents
        {
            get { return _documents; }
        }
    }
}
