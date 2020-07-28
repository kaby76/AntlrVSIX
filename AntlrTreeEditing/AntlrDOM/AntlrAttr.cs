namespace AntlrTreeEditing.AntlrDOM
{
    using org.w3c.dom;

    public class AntlrAttr : AntlrNode, Attr
    {
        public string Prefix { get; set; }
        public object Name { get; set; }
        public string Value { get; set; }
        public Node OwnerElement { get; set; }
        public TypeInfo SchemaTypeInfo { get; set; }
    }
}
