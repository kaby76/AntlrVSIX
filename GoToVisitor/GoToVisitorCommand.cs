using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AntlrVSIX.GoToVisitor
{
    public class GoToVisitorCommand
    {
        public GoToVisitorCommand()
        {
        }

        private void Compile()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var firstMember = root.Members[0];

            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];

            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];

            var argsParameter = mainDeclaration.ParameterList.Parameters[0];

            var firstParameters = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(methodDeclaration => methodDeclaration.Identifier.ValueText == "Main")
                .Select(methodDeclaration => methodDeclaration.ParameterList.Parameters.First());

            var argsParameter2 = firstParameters.Single();

        }
    }
}
