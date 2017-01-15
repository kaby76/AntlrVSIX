import java.io.IOException;
import org.antlr.v4.runtime.ANTLRFileStream;
import org.antlr.v4.runtime.ANTLRInputStream;
import org.antlr.v4.runtime.CommonTokenStream;
import org.antlr.v4.runtime.tree.ParseTree;
import org.antlr.v4.runtime.tree.ParseTreeWalker;
public class Test {
    public static void main(String[] args) throws IOException {
	ANTLRInputStream input = new ANTLRFileStream("test.g4");
	ANTLRv4Lexer lexer = new ANTLRv4Lexer(input);
	CommonTokenStream tokes = new CommonTokenStream(lexer);
	ANTLRv4Parser parser = new ANTLRv4Parser(tokes);
	ParseTree tree = parser.grammarSpec();
	String str = tree.toStringTree(parser);
	System.out.println(str);
    }
}
