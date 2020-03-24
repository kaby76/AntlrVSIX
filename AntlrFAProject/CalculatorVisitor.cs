// Template generated code from Antlr4BuildTasks.Template v 3.0
namespace $safeprojectname$
{
    using System;
    using Antlr4.Runtime.Misc;

    class CalculatorVisitor : arithmeticBaseVisitor<double>
    {
        public override double VisitAtom([NotNull] arithmeticParser.AtomContext context)
        {
            var text = context.GetText();
            var v = Double.Parse(text);
            return v;
        }

        public override double VisitExpression(arithmeticParser.ExpressionContext context)
        {
            var first = context.GetChild(0);
            var is_e_op_e = first.GetType() == typeof(arithmeticParser.ExpressionContext);
            var is_lparen = first.GetText() == "(";
            if (is_e_op_e)
            {
                var left = VisitExpression((arithmeticParser.ExpressionContext)context.left);
                var right = VisitExpression((arithmeticParser.ExpressionContext)context.right);
                var op = context.op ?? throw new Exception();
                switch (op.Text)
                {
                    case "+":
                        return left + right;
                    case "-":
                        return left - right;
                    case "/":
                        return left / right;
                    case "*":
                        return left * right;
                    default:
                        throw new Exception();
                }
            }
            else if (is_lparen)
            {
                return VisitExpression((arithmeticParser.ExpressionContext)context.evalue);
            }
            else
            {
                var o = (arithmeticParser.ExpressionContext)context.op;
                var v = VisitAtom((arithmeticParser.AtomContext)context.avalue);
                return v;
            }
        }

        public override double VisitFile(arithmeticParser.FileContext context)
        {
            for (int i = 0; i < context.ChildCount; ++i)
            {
                var c = context.GetChild(i);
                if (c as arithmeticParser.ExpressionContext != null)
                {
                    var v = Visit(c);
                    System.Console.WriteLine(v);
                }
            }
            return -1;
        }
    }
}
