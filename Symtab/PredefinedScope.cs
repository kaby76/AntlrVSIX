namespace org.antlr.symtab
{

    /// <summary>
    /// A scope to hold predefined symbols of your language. This could
    ///  be a list of type names like int or methods like print.
    /// </summary>
    public class PredefinedScope : BaseScope
    {
        public override string Name
        {
            get
            {
                return "predefined";
            }
        }
    }

}