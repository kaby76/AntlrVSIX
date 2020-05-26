namespace Workspaces
{
    using System.Collections.Generic;

    public class Project : Container
    {
        public uint _id;
        public int _hash;
        private string _canonical_name;
        private string _name;
        private string _ffn;
        private readonly List<Container> _contents = new List<Container>();
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _lazy_evaluated = new Dictionary<string, bool>();

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string CanonicalName
        {
            get => _canonical_name;
            set => _canonical_name = value;
        }

        public string FullPath
        {
            get => _ffn;
            set => _ffn = value;
        }

        public Project(string canonical_name, string name, string ffn)
        {
            _canonical_name = canonical_name;
            _name = name;
            _ffn = ffn;
        }

        public Document AddDocument(Document doc)
        {
            if (doc == null)
            {
                throw new System.Exception("Trying to add null document.");
            }

            _contents.Add(doc);
            doc.Parent = this;
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
            _lazy_evaluated.TryGetValue(name, out _);
            string result = null;
            return result;
        }

        public override Project FindProject(string ffn)
        {
            if (ffn == null && FullPath == null)
            {
                return null;
            }

            if (FullPath.ToLower() == ffn.ToLower())
            {
                return this;
            }

            foreach (Container proj in _contents)
            {
                Project found = proj.FindProject(ffn);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public override Project FindProject(string canonical_name, string name, string ffn)
        {
            if (CanonicalName == canonical_name &&
                Name == name &&
                FullPath.ToLower() == ffn.ToLower())
            {
                return this;
            }

            foreach (Container proj in _contents)
            {
                Project found = proj.FindProject(canonical_name, name, ffn);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public override Document FindDocument(string ffn)
        {
            foreach (Container doc in _contents)
            {
                Document found = doc.FindDocument(ffn);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public IEnumerable<Container> Children => _contents;
        public override Container AddChild(Container doc)
        {
            if (doc == null)
            {
                throw new System.Exception("Trying to add null document.");
            }

            _contents.Add(doc);
            doc.Parent = this;
            return doc;
        }


    }
}
