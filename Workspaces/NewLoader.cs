//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.CodeAnalysis.MSBuild;

//namespace Workspaces
//{
//    public class NewLoader
//    {
//        public static async Task<Workspace> Loader(string msbuild_file)
//        {
//            if (Path.GetExtension(msbuild_file) == ".csproj")
//            {
//                var workspace = MSBuildWorkspace.Create();
//                var project = await workspace.OpenProjectAsync("MyProject.csproj");
//                var compilation = await project.GetCompilationAsync();
//            } else if (Path.GetExtension(msbuild_file) == ".sln")
//            {
//                var workspace = MSBuildWorkspace.Create();
//                var solution = await workspace.OpenSolutionAsync("MySolution.sln");
//                foreach (var project in solution.Projects)
//                {
//                    var compilation = await project.GetCompilationAsync();
//                }
//            }
//            return null;
//        }
//    }
//}
