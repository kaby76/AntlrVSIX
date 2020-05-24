namespace LspAntlr
{
    public class StringValue
    {
        public StringValue(string s)
        {
            Ffn = s;
            _bison = System.IO.Path.GetFileName(Ffn);
            _antlr = System.IO.Path.GetFileName(Ffn).Replace(".y", ".g4");
        }
        //public string FFN { get { return _ffn; } set { _ffn = value; } }
        public string Bison { get => _bison; set => _bison = value; }
        public string Antlr { get => _antlr; set => _antlr = value; }
        public string Ffn { get => ffn; set => ffn = value; }

        private string ffn;
        private string _antlr;
        private string _bison;
    }
}
