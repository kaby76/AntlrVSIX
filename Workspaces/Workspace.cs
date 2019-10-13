namespace Workspaces
{
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Collections.Generic;

    public class Workspace : Container
    {
        IVsSolution _ide_object;
        static Workspace _instance;
        string _name;
        string _ffn;
        List<Container> _contents = new List<Container>();

        public static Workspace Initialize(IVsSolution ide_object, string name, string ffn)
        {
            if (_instance != null) return _instance;
            var i = Instance;
            i._ide_object = ide_object;
            i._name = name;
            i._ffn = ffn;
            return i;
        }

        public static Workspace Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Workspace();
                return _instance;
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FFN
        {
            get { return _ffn; }
            set { _ffn = value; }
        }

        public IEnumerable<Container> Children
        {
            get { return _contents; }
        }

        public override Container AddChild(Container doc)
        {
            _contents.Add(doc);
            doc.Parent = this;
            return doc;
        }

        public override Document FindDocument(string ffn)
        {
            foreach (var doc in _contents)
            {
                var found = doc.FindDocument(ffn);
                if (found != null) return found;
            }
            return null;
        }

        public override Project FindProject(string ffn)
        {
            foreach (var doc in _contents)
            {
                var found = doc.FindProject(ffn);
                if (found != null) return found;
            }
            return null;
        }
    }
}
