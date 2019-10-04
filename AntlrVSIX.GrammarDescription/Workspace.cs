using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.GrammarDescription
{
    public class Workspace
    {
        static Workspace _instance;
        string _name;
        string _ffn;
        List<Project> _projects = new List<Project>();
        Func<string, object, string> _get_property;
        object _get_property_data;

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

        public IEnumerable<Project> Projects
        {
            get { return _projects; }
        }

        public Project AddProject(Project project)
        {
            _projects.Add(project);
            return project;
        }

        public Document FindDocumentFullName(string ffn)
        {
            foreach (Project proj in _projects)
                foreach (var doc in proj.Documents)
                    if (doc.FullPath == ffn)
                        return doc;
            if (System.IO.File.Exists(ffn))
            {
                foreach (var proj in _projects)
                    if (proj.Name == "Miscellaneous Files")
                    {
                        var doc = new Document(ffn, null, null);
                        proj.AddDocument(doc);
                        return doc;
                    }
            }
            return null;
        }
    }
}
