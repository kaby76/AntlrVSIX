namespace AntlrTreeEditing.AntlrDOM
{
    using org.w3c.dom;

    public class AntlrText : AntlrNode, Text
    {
        public string Data { get; set; }
    }
}
