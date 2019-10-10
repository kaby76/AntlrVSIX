using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace Workspaces
{
    public class Workspace
    {
        IVsSolution _ide_object;
        uint _id;
        static Workspace _instance;
        string _name;
        string _ffn;
        List<Project> _projects = new List<Project>();

        public static void Initialize(IVsSolution ide_object, string name, string ffn)
        {
            if (_instance != null) return;
            var i = Instance;
            i._ide_object = ide_object;
            i._name = name;
            i._ffn = ffn;
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

        public IEnumerable<Project> Projects
        {
            get { return _projects; }
        }

        public Project AddProject(Project project)
        {
            if (project == null) throw new Exception();
            _projects.Add(project);
            return project;
        }

        public Document FindDocumentFullName(string ffn)
        {
            foreach (Project proj in _projects)
                foreach (var doc in proj.Documents)
                    if (doc.FullPath.ToLower() == ffn.ToLower())
                        return doc;
            return null;
        }

        public Project FindProject(string ffn, string name)
        {
            foreach (Project proj in _projects)
                if (proj.FullPath.ToLower() == ffn.ToLower() && proj.Name.ToLower() == name.ToLower())
                    return proj;
            return null;
        }
    }
}
