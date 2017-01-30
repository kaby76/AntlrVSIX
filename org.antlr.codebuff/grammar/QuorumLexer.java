// Generated from Quorum.g4 by ANTLR 4.6
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class QuorumLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.6", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		OUTPUT=1, ON=2, CREATE=3, CONSTANT=4, ELSE_IF=5, ME=6, UNTIL=7, PUBLIC=8, 
		PRIVATE=9, ALERT=10, DETECT=11, ALWAYS=12, CHECK=13, PARENT=14, BLUEPRINT=15, 
		NATIVE=16, INHERITS=17, CAST=18, INPUT=19, SAY=20, NOW=21, WHILE=22, PACKAGE_NAME=23, 
		TIMES=24, REPEAT=25, ELSE=26, RETURNS=27, RETURN=28, AND=29, OR=30, NULL=31, 
		ACTION=32, COLON=33, INTEGER_KEYWORD=34, NUMBER_KEYWORD=35, TEXT=36, BOOLEAN_KEYWORD=37, 
		USE=38, NOT=39, NOTEQUALS=40, PERIOD=41, COMMA=42, EQUALITY=43, GREATER=44, 
		GREATER_EQUAL=45, LESS=46, LESS_EQUAL=47, PLUS=48, MINUS=49, MULTIPLY=50, 
		DIVIDE=51, MODULO=52, LEFT_SQR_BRACE=53, RIGHT_SQR_BRACE=54, LEFT_PAREN=55, 
		RIGHT_PAREN=56, DOUBLE_QUOTE=57, IF=58, END=59, CLASS=60, BOOLEAN=61, 
		INT=62, DECIMAL=63, ID=64, STRING=65, NEWLINE=66, WS=67, COMMENTS=68;
	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	public static final String[] ruleNames = {
		"OUTPUT", "ON", "CREATE", "CONSTANT", "ELSE_IF", "ME", "UNTIL", "PUBLIC", 
		"PRIVATE", "ALERT", "DETECT", "ALWAYS", "CHECK", "PARENT", "BLUEPRINT", 
		"NATIVE", "INHERITS", "CAST", "INPUT", "SAY", "NOW", "WHILE", "PACKAGE_NAME", 
		"TIMES", "REPEAT", "ELSE", "RETURNS", "RETURN", "AND", "OR", "NULL", "ACTION", 
		"COLON", "INTEGER_KEYWORD", "NUMBER_KEYWORD", "TEXT", "BOOLEAN_KEYWORD", 
		"USE", "NOT", "NOTEQUALS", "PERIOD", "COMMA", "EQUALITY", "GREATER", "GREATER_EQUAL", 
		"LESS", "LESS_EQUAL", "PLUS", "MINUS", "MULTIPLY", "DIVIDE", "MODULO", 
		"LEFT_SQR_BRACE", "RIGHT_SQR_BRACE", "LEFT_PAREN", "RIGHT_PAREN", "DOUBLE_QUOTE", 
		"IF", "END", "CLASS", "BOOLEAN", "INT", "DECIMAL", "ID", "STRING", "NEWLINE", 
		"WS", "COMMENTS"
	};

	private static final String[] _LITERAL_NAMES = {
		null, "'output'", "'on'", "'create'", "'constant'", "'elseif'", "'me'", 
		"'until'", "'public'", "'private'", "'alert'", "'detect'", "'always'", 
		"'check'", "'parent'", "'blueprint'", "'system'", "'is'", "'cast'", "'input'", 
		"'say'", "'now'", "'while'", "'package'", "'times'", "'repeat'", "'else'", 
		"'returns'", "'return'", "'and'", "'or'", "'undefined'", "'action'", "':'", 
		"'integer'", "'number'", "'text'", "'boolean'", "'use'", null, null, "'.'", 
		"','", "'='", "'>'", "'>='", "'<'", "'<='", "'+'", "'-'", "'*'", "'/'", 
		"'mod'", "'['", "']'", "'('", "')'", "'\"'", "'if'", "'end'", "'class'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "OUTPUT", "ON", "CREATE", "CONSTANT", "ELSE_IF", "ME", "UNTIL", 
		"PUBLIC", "PRIVATE", "ALERT", "DETECT", "ALWAYS", "CHECK", "PARENT", "BLUEPRINT", 
		"NATIVE", "INHERITS", "CAST", "INPUT", "SAY", "NOW", "WHILE", "PACKAGE_NAME", 
		"TIMES", "REPEAT", "ELSE", "RETURNS", "RETURN", "AND", "OR", "NULL", "ACTION", 
		"COLON", "INTEGER_KEYWORD", "NUMBER_KEYWORD", "TEXT", "BOOLEAN_KEYWORD", 
		"USE", "NOT", "NOTEQUALS", "PERIOD", "COMMA", "EQUALITY", "GREATER", "GREATER_EQUAL", 
		"LESS", "LESS_EQUAL", "PLUS", "MINUS", "MULTIPLY", "DIVIDE", "MODULO", 
		"LEFT_SQR_BRACE", "RIGHT_SQR_BRACE", "LEFT_PAREN", "RIGHT_PAREN", "DOUBLE_QUOTE", 
		"IF", "END", "CLASS", "BOOLEAN", "INT", "DECIMAL", "ID", "STRING", "NEWLINE", 
		"WS", "COMMENTS"
	};
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public QuorumLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "Quorum.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u0430\ud6d1\u8206\uad2d\u4417\uaef1\u8d80\uaadd\2F\u0210\b\1\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t<\4=\t="+
		"\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\3\2\3\2\3\2\3\2\3\2\3"+
		"\2\3\2\3\3\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5\3\5"+
		"\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\7\3\7\3\7\3\b\3\b\3\b\3\b\3"+
		"\b\3\b\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\13"+
		"\3\13\3\13\3\13\3\13\3\13\3\f\3\f\3\f\3\f\3\f\3\f\3\f\3\r\3\r\3\r\3\r"+
		"\3\r\3\r\3\r\3\16\3\16\3\16\3\16\3\16\3\16\3\17\3\17\3\17\3\17\3\17\3"+
		"\17\3\17\3\20\3\20\3\20\3\20\3\20\3\20\3\20\3\20\3\20\3\20\3\21\3\21\3"+
		"\21\3\21\3\21\3\21\3\21\3\22\3\22\3\22\3\23\3\23\3\23\3\23\3\23\3\24\3"+
		"\24\3\24\3\24\3\24\3\24\3\25\3\25\3\25\3\25\3\26\3\26\3\26\3\26\3\27\3"+
		"\27\3\27\3\27\3\27\3\27\3\30\3\30\3\30\3\30\3\30\3\30\3\30\3\30\3\31\3"+
		"\31\3\31\3\31\3\31\3\31\3\32\3\32\3\32\3\32\3\32\3\32\3\32\3\33\3\33\3"+
		"\33\3\33\3\33\3\34\3\34\3\34\3\34\3\34\3\34\3\34\3\34\3\35\3\35\3\35\3"+
		"\35\3\35\3\35\3\35\3\36\3\36\3\36\3\36\3\37\3\37\3\37\3 \3 \3 \3 \3 \3"+
		" \3 \3 \3 \3 \3!\3!\3!\3!\3!\3!\3!\3\"\3\"\3#\3#\3#\3#\3#\3#\3#\3#\3$"+
		"\3$\3$\3$\3$\3$\3$\3%\3%\3%\3%\3%\3&\3&\3&\3&\3&\3&\3&\3&\3\'\3\'\3\'"+
		"\3\'\3(\3(\3(\3(\3(\3(\5(\u017c\n(\3)\3)\3)\3)\3)\3*\3*\3+\3+\3,\3,\3"+
		"-\3-\3.\3.\3.\3/\3/\3\60\3\60\3\60\3\61\3\61\3\62\3\62\3\63\3\63\3\64"+
		"\3\64\3\65\3\65\3\65\3\65\3\66\3\66\3\67\3\67\38\38\39\39\3:\3:\3;\3;"+
		"\3;\3<\3<\3<\3<\3=\3=\3=\3=\3=\3=\3>\3>\3>\3>\3>\3>\3>\3>\3>\5>\u01bf"+
		"\n>\3?\6?\u01c2\n?\r?\16?\u01c3\3@\6@\u01c7\n@\r@\16@\u01c8\3@\3@\7@\u01cd"+
		"\n@\f@\16@\u01d0\13@\5@\u01d2\n@\3A\3A\7A\u01d6\nA\fA\16A\u01d9\13A\3"+
		"B\3B\7B\u01dd\nB\fB\16B\u01e0\13B\3B\3B\3C\5C\u01e5\nC\3C\3C\3C\3C\3D"+
		"\6D\u01ec\nD\rD\16D\u01ed\3D\3D\3E\3E\3E\3E\7E\u01f6\nE\fE\16E\u01f9\13"+
		"E\3E\5E\u01fc\nE\3E\3E\5E\u0200\nE\3E\3E\3E\3E\7E\u0206\nE\fE\16E\u0209"+
		"\13E\3E\3E\5E\u020d\nE\3E\3E\4\u01de\u0207\2F\3\3\5\4\7\5\t\6\13\7\r\b"+
		"\17\t\21\n\23\13\25\f\27\r\31\16\33\17\35\20\37\21!\22#\23%\24\'\25)\26"+
		"+\27-\30/\31\61\32\63\33\65\34\67\359\36;\37= ?!A\"C#E$G%I&K\'M(O)Q*S"+
		"+U,W-Y.[/]\60_\61a\62c\63e\64g\65i\66k\67m8o9q:s;u<w=y>{?}@\177A\u0081"+
		"B\u0083C\u0085D\u0087E\u0089F\3\2\7\4\2PPpp\4\2C\\c|\6\2\62;C\\aac|\5"+
		"\2\13\f\17\17\"\"\4\2\f\f\17\17\u021e\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3\2"+
		"\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2\2\2"+
		"\23\3\2\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33\3\2\2\2\2\35\3"+
		"\2\2\2\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2\2\2\2\'\3\2\2\2\2)\3"+
		"\2\2\2\2+\3\2\2\2\2-\3\2\2\2\2/\3\2\2\2\2\61\3\2\2\2\2\63\3\2\2\2\2\65"+
		"\3\2\2\2\2\67\3\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3\2\2\2\2?\3\2\2\2\2A\3"+
		"\2\2\2\2C\3\2\2\2\2E\3\2\2\2\2G\3\2\2\2\2I\3\2\2\2\2K\3\2\2\2\2M\3\2\2"+
		"\2\2O\3\2\2\2\2Q\3\2\2\2\2S\3\2\2\2\2U\3\2\2\2\2W\3\2\2\2\2Y\3\2\2\2\2"+
		"[\3\2\2\2\2]\3\2\2\2\2_\3\2\2\2\2a\3\2\2\2\2c\3\2\2\2\2e\3\2\2\2\2g\3"+
		"\2\2\2\2i\3\2\2\2\2k\3\2\2\2\2m\3\2\2\2\2o\3\2\2\2\2q\3\2\2\2\2s\3\2\2"+
		"\2\2u\3\2\2\2\2w\3\2\2\2\2y\3\2\2\2\2{\3\2\2\2\2}\3\2\2\2\2\177\3\2\2"+
		"\2\2\u0081\3\2\2\2\2\u0083\3\2\2\2\2\u0085\3\2\2\2\2\u0087\3\2\2\2\2\u0089"+
		"\3\2\2\2\3\u008b\3\2\2\2\5\u0092\3\2\2\2\7\u0095\3\2\2\2\t\u009c\3\2\2"+
		"\2\13\u00a5\3\2\2\2\r\u00ac\3\2\2\2\17\u00af\3\2\2\2\21\u00b5\3\2\2\2"+
		"\23\u00bc\3\2\2\2\25\u00c4\3\2\2\2\27\u00ca\3\2\2\2\31\u00d1\3\2\2\2\33"+
		"\u00d8\3\2\2\2\35\u00de\3\2\2\2\37\u00e5\3\2\2\2!\u00ef\3\2\2\2#\u00f6"+
		"\3\2\2\2%\u00f9\3\2\2\2\'\u00fe\3\2\2\2)\u0104\3\2\2\2+\u0108\3\2\2\2"+
		"-\u010c\3\2\2\2/\u0112\3\2\2\2\61\u011a\3\2\2\2\63\u0120\3\2\2\2\65\u0127"+
		"\3\2\2\2\67\u012c\3\2\2\29\u0134\3\2\2\2;\u013b\3\2\2\2=\u013f\3\2\2\2"+
		"?\u0142\3\2\2\2A\u014c\3\2\2\2C\u0153\3\2\2\2E\u0155\3\2\2\2G\u015d\3"+
		"\2\2\2I\u0164\3\2\2\2K\u0169\3\2\2\2M\u0171\3\2\2\2O\u017b\3\2\2\2Q\u017d"+
		"\3\2\2\2S\u0182\3\2\2\2U\u0184\3\2\2\2W\u0186\3\2\2\2Y\u0188\3\2\2\2["+
		"\u018a\3\2\2\2]\u018d\3\2\2\2_\u018f\3\2\2\2a\u0192\3\2\2\2c\u0194\3\2"+
		"\2\2e\u0196\3\2\2\2g\u0198\3\2\2\2i\u019a\3\2\2\2k\u019e\3\2\2\2m\u01a0"+
		"\3\2\2\2o\u01a2\3\2\2\2q\u01a4\3\2\2\2s\u01a6\3\2\2\2u\u01a8\3\2\2\2w"+
		"\u01ab\3\2\2\2y\u01af\3\2\2\2{\u01be\3\2\2\2}\u01c1\3\2\2\2\177\u01c6"+
		"\3\2\2\2\u0081\u01d3\3\2\2\2\u0083\u01da\3\2\2\2\u0085\u01e4\3\2\2\2\u0087"+
		"\u01eb\3\2\2\2\u0089\u020c\3\2\2\2\u008b\u008c\7q\2\2\u008c\u008d\7w\2"+
		"\2\u008d\u008e\7v\2\2\u008e\u008f\7r\2\2\u008f\u0090\7w\2\2\u0090\u0091"+
		"\7v\2\2\u0091\4\3\2\2\2\u0092\u0093\7q\2\2\u0093\u0094\7p\2\2\u0094\6"+
		"\3\2\2\2\u0095\u0096\7e\2\2\u0096\u0097\7t\2\2\u0097\u0098\7g\2\2\u0098"+
		"\u0099\7c\2\2\u0099\u009a\7v\2\2\u009a\u009b\7g\2\2\u009b\b\3\2\2\2\u009c"+
		"\u009d\7e\2\2\u009d\u009e\7q\2\2\u009e\u009f\7p\2\2\u009f\u00a0\7u\2\2"+
		"\u00a0\u00a1\7v\2\2\u00a1\u00a2\7c\2\2\u00a2\u00a3\7p\2\2\u00a3\u00a4"+
		"\7v\2\2\u00a4\n\3\2\2\2\u00a5\u00a6\7g\2\2\u00a6\u00a7\7n\2\2\u00a7\u00a8"+
		"\7u\2\2\u00a8\u00a9\7g\2\2\u00a9\u00aa\7k\2\2\u00aa\u00ab\7h\2\2\u00ab"+
		"\f\3\2\2\2\u00ac\u00ad\7o\2\2\u00ad\u00ae\7g\2\2\u00ae\16\3\2\2\2\u00af"+
		"\u00b0\7w\2\2\u00b0\u00b1\7p\2\2\u00b1\u00b2\7v\2\2\u00b2\u00b3\7k\2\2"+
		"\u00b3\u00b4\7n\2\2\u00b4\20\3\2\2\2\u00b5\u00b6\7r\2\2\u00b6\u00b7\7"+
		"w\2\2\u00b7\u00b8\7d\2\2\u00b8\u00b9\7n\2\2\u00b9\u00ba\7k\2\2\u00ba\u00bb"+
		"\7e\2\2\u00bb\22\3\2\2\2\u00bc\u00bd\7r\2\2\u00bd\u00be\7t\2\2\u00be\u00bf"+
		"\7k\2\2\u00bf\u00c0\7x\2\2\u00c0\u00c1\7c\2\2\u00c1\u00c2\7v\2\2\u00c2"+
		"\u00c3\7g\2\2\u00c3\24\3\2\2\2\u00c4\u00c5\7c\2\2\u00c5\u00c6\7n\2\2\u00c6"+
		"\u00c7\7g\2\2\u00c7\u00c8\7t\2\2\u00c8\u00c9\7v\2\2\u00c9\26\3\2\2\2\u00ca"+
		"\u00cb\7f\2\2\u00cb\u00cc\7g\2\2\u00cc\u00cd\7v\2\2\u00cd\u00ce\7g\2\2"+
		"\u00ce\u00cf\7e\2\2\u00cf\u00d0\7v\2\2\u00d0\30\3\2\2\2\u00d1\u00d2\7"+
		"c\2\2\u00d2\u00d3\7n\2\2\u00d3\u00d4\7y\2\2\u00d4\u00d5\7c\2\2\u00d5\u00d6"+
		"\7{\2\2\u00d6\u00d7\7u\2\2\u00d7\32\3\2\2\2\u00d8\u00d9\7e\2\2\u00d9\u00da"+
		"\7j\2\2\u00da\u00db\7g\2\2\u00db\u00dc\7e\2\2\u00dc\u00dd\7m\2\2\u00dd"+
		"\34\3\2\2\2\u00de\u00df\7r\2\2\u00df\u00e0\7c\2\2\u00e0\u00e1\7t\2\2\u00e1"+
		"\u00e2\7g\2\2\u00e2\u00e3\7p\2\2\u00e3\u00e4\7v\2\2\u00e4\36\3\2\2\2\u00e5"+
		"\u00e6\7d\2\2\u00e6\u00e7\7n\2\2\u00e7\u00e8\7w\2\2\u00e8\u00e9\7g\2\2"+
		"\u00e9\u00ea\7r\2\2\u00ea\u00eb\7t\2\2\u00eb\u00ec\7k\2\2\u00ec\u00ed"+
		"\7p\2\2\u00ed\u00ee\7v\2\2\u00ee \3\2\2\2\u00ef\u00f0\7u\2\2\u00f0\u00f1"+
		"\7{\2\2\u00f1\u00f2\7u\2\2\u00f2\u00f3\7v\2\2\u00f3\u00f4\7g\2\2\u00f4"+
		"\u00f5\7o\2\2\u00f5\"\3\2\2\2\u00f6\u00f7\7k\2\2\u00f7\u00f8\7u\2\2\u00f8"+
		"$\3\2\2\2\u00f9\u00fa\7e\2\2\u00fa\u00fb\7c\2\2\u00fb\u00fc\7u\2\2\u00fc"+
		"\u00fd\7v\2\2\u00fd&\3\2\2\2\u00fe\u00ff\7k\2\2\u00ff\u0100\7p\2\2\u0100"+
		"\u0101\7r\2\2\u0101\u0102\7w\2\2\u0102\u0103\7v\2\2\u0103(\3\2\2\2\u0104"+
		"\u0105\7u\2\2\u0105\u0106\7c\2\2\u0106\u0107\7{\2\2\u0107*\3\2\2\2\u0108"+
		"\u0109\7p\2\2\u0109\u010a\7q\2\2\u010a\u010b\7y\2\2\u010b,\3\2\2\2\u010c"+
		"\u010d\7y\2\2\u010d\u010e\7j\2\2\u010e\u010f\7k\2\2\u010f\u0110\7n\2\2"+
		"\u0110\u0111\7g\2\2\u0111.\3\2\2\2\u0112\u0113\7r\2\2\u0113\u0114\7c\2"+
		"\2\u0114\u0115\7e\2\2\u0115\u0116\7m\2\2\u0116\u0117\7c\2\2\u0117\u0118"+
		"\7i\2\2\u0118\u0119\7g\2\2\u0119\60\3\2\2\2\u011a\u011b\7v\2\2\u011b\u011c"+
		"\7k\2\2\u011c\u011d\7o\2\2\u011d\u011e\7g\2\2\u011e\u011f\7u\2\2\u011f"+
		"\62\3\2\2\2\u0120\u0121\7t\2\2\u0121\u0122\7g\2\2\u0122\u0123\7r\2\2\u0123"+
		"\u0124\7g\2\2\u0124\u0125\7c\2\2\u0125\u0126\7v\2\2\u0126\64\3\2\2\2\u0127"+
		"\u0128\7g\2\2\u0128\u0129\7n\2\2\u0129\u012a\7u\2\2\u012a\u012b\7g\2\2"+
		"\u012b\66\3\2\2\2\u012c\u012d\7t\2\2\u012d\u012e\7g\2\2\u012e\u012f\7"+
		"v\2\2\u012f\u0130\7w\2\2\u0130\u0131\7t\2\2\u0131\u0132\7p\2\2\u0132\u0133"+
		"\7u\2\2\u01338\3\2\2\2\u0134\u0135\7t\2\2\u0135\u0136\7g\2\2\u0136\u0137"+
		"\7v\2\2\u0137\u0138\7w\2\2\u0138\u0139\7t\2\2\u0139\u013a\7p\2\2\u013a"+
		":\3\2\2\2\u013b\u013c\7c\2\2\u013c\u013d\7p\2\2\u013d\u013e\7f\2\2\u013e"+
		"<\3\2\2\2\u013f\u0140\7q\2\2\u0140\u0141\7t\2\2\u0141>\3\2\2\2\u0142\u0143"+
		"\7w\2\2\u0143\u0144\7p\2\2\u0144\u0145\7f\2\2\u0145\u0146\7g\2\2\u0146"+
		"\u0147\7h\2\2\u0147\u0148\7k\2\2\u0148\u0149\7p\2\2\u0149\u014a\7g\2\2"+
		"\u014a\u014b\7f\2\2\u014b@\3\2\2\2\u014c\u014d\7c\2\2\u014d\u014e\7e\2"+
		"\2\u014e\u014f\7v\2\2\u014f\u0150\7k\2\2\u0150\u0151\7q\2\2\u0151\u0152"+
		"\7p\2\2\u0152B\3\2\2\2\u0153\u0154\7<\2\2\u0154D\3\2\2\2\u0155\u0156\7"+
		"k\2\2\u0156\u0157\7p\2\2\u0157\u0158\7v\2\2\u0158\u0159\7g\2\2\u0159\u015a"+
		"\7i\2\2\u015a\u015b\7g\2\2\u015b\u015c\7t\2\2\u015cF\3\2\2\2\u015d\u015e"+
		"\7p\2\2\u015e\u015f\7w\2\2\u015f\u0160\7o\2\2\u0160\u0161\7d\2\2\u0161"+
		"\u0162\7g\2\2\u0162\u0163\7t\2\2\u0163H\3\2\2\2\u0164\u0165\7v\2\2\u0165"+
		"\u0166\7g\2\2\u0166\u0167\7z\2\2\u0167\u0168\7v\2\2\u0168J\3\2\2\2\u0169"+
		"\u016a\7d\2\2\u016a\u016b\7q\2\2\u016b\u016c\7q\2\2\u016c\u016d\7n\2\2"+
		"\u016d\u016e\7g\2\2\u016e\u016f\7c\2\2\u016f\u0170\7p\2\2\u0170L\3\2\2"+
		"\2\u0171\u0172\7w\2\2\u0172\u0173\7u\2\2\u0173\u0174\7g\2\2\u0174N\3\2"+
		"\2\2\u0175\u0176\7p\2\2\u0176\u0177\7q\2\2\u0177\u017c\7v\2\2\u0178\u0179"+
		"\7P\2\2\u0179\u017a\7q\2\2\u017a\u017c\7v\2\2\u017b\u0175\3\2\2\2\u017b"+
		"\u0178\3\2\2\2\u017cP\3\2\2\2\u017d\u017e\t\2\2\2\u017e\u017f\7q\2\2\u017f"+
		"\u0180\7v\2\2\u0180\u0181\7?\2\2\u0181R\3\2\2\2\u0182\u0183\7\60\2\2\u0183"+
		"T\3\2\2\2\u0184\u0185\7.\2\2\u0185V\3\2\2\2\u0186\u0187\7?\2\2\u0187X"+
		"\3\2\2\2\u0188\u0189\7@\2\2\u0189Z\3\2\2\2\u018a\u018b\7@\2\2\u018b\u018c"+
		"\7?\2\2\u018c\\\3\2\2\2\u018d\u018e\7>\2\2\u018e^\3\2\2\2\u018f\u0190"+
		"\7>\2\2\u0190\u0191\7?\2\2\u0191`\3\2\2\2\u0192\u0193\7-\2\2\u0193b\3"+
		"\2\2\2\u0194\u0195\7/\2\2\u0195d\3\2\2\2\u0196\u0197\7,\2\2\u0197f\3\2"+
		"\2\2\u0198\u0199\7\61\2\2\u0199h\3\2\2\2\u019a\u019b\7o\2\2\u019b\u019c"+
		"\7q\2\2\u019c\u019d\7f\2\2\u019dj\3\2\2\2\u019e\u019f\7]\2\2\u019fl\3"+
		"\2\2\2\u01a0\u01a1\7_\2\2\u01a1n\3\2\2\2\u01a2\u01a3\7*\2\2\u01a3p\3\2"+
		"\2\2\u01a4\u01a5\7+\2\2\u01a5r\3\2\2\2\u01a6\u01a7\7$\2\2\u01a7t\3\2\2"+
		"\2\u01a8\u01a9\7k\2\2\u01a9\u01aa\7h\2\2\u01aav\3\2\2\2\u01ab\u01ac\7"+
		"g\2\2\u01ac\u01ad\7p\2\2\u01ad\u01ae\7f\2\2\u01aex\3\2\2\2\u01af\u01b0"+
		"\7e\2\2\u01b0\u01b1\7n\2\2\u01b1\u01b2\7c\2\2\u01b2\u01b3\7u\2\2\u01b3"+
		"\u01b4\7u\2\2\u01b4z\3\2\2\2\u01b5\u01b6\7v\2\2\u01b6\u01b7\7t\2\2\u01b7"+
		"\u01b8\7w\2\2\u01b8\u01bf\7g\2\2\u01b9\u01ba\7h\2\2\u01ba\u01bb\7c\2\2"+
		"\u01bb\u01bc\7n\2\2\u01bc\u01bd\7u\2\2\u01bd\u01bf\7g\2\2\u01be\u01b5"+
		"\3\2\2\2\u01be\u01b9\3\2\2\2\u01bf|\3\2\2\2\u01c0\u01c2\4\62;\2\u01c1"+
		"\u01c0\3\2\2\2\u01c2\u01c3\3\2\2\2\u01c3\u01c1\3\2\2\2\u01c3\u01c4\3\2"+
		"\2\2\u01c4~\3\2\2\2\u01c5\u01c7\4\62;\2\u01c6\u01c5\3\2\2\2\u01c7\u01c8"+
		"\3\2\2\2\u01c8\u01c6\3\2\2\2\u01c8\u01c9\3\2\2\2\u01c9\u01d1\3\2\2\2\u01ca"+
		"\u01ce\5S*\2\u01cb\u01cd\4\62;\2\u01cc\u01cb\3\2\2\2\u01cd\u01d0\3\2\2"+
		"\2\u01ce\u01cc\3\2\2\2\u01ce\u01cf\3\2\2\2\u01cf\u01d2\3\2\2\2\u01d0\u01ce"+
		"\3\2\2\2\u01d1\u01ca\3\2\2\2\u01d1\u01d2\3\2\2\2\u01d2\u0080\3\2\2\2\u01d3"+
		"\u01d7\t\3\2\2\u01d4\u01d6\t\4\2\2\u01d5\u01d4\3\2\2\2\u01d6\u01d9\3\2"+
		"\2\2\u01d7\u01d5\3\2\2\2\u01d7\u01d8\3\2\2\2\u01d8\u0082\3\2\2\2\u01d9"+
		"\u01d7\3\2\2\2\u01da\u01de\5s:\2\u01db\u01dd\13\2\2\2\u01dc\u01db\3\2"+
		"\2\2\u01dd\u01e0\3\2\2\2\u01de\u01df\3\2\2\2\u01de\u01dc\3\2\2\2\u01df"+
		"\u01e1\3\2\2\2\u01e0\u01de\3\2\2\2\u01e1\u01e2\5s:\2\u01e2\u0084\3\2\2"+
		"\2\u01e3\u01e5\7\17\2\2\u01e4\u01e3\3\2\2\2\u01e4\u01e5\3\2\2\2\u01e5"+
		"\u01e6\3\2\2\2\u01e6\u01e7\7\f\2\2\u01e7\u01e8\3\2\2\2\u01e8\u01e9\bC"+
		"\2\2\u01e9\u0086\3\2\2\2\u01ea\u01ec\t\5\2\2\u01eb\u01ea\3\2\2\2\u01ec"+
		"\u01ed\3\2\2\2\u01ed\u01eb\3\2\2\2\u01ed\u01ee\3\2\2\2\u01ee\u01ef\3\2"+
		"\2\2\u01ef\u01f0\bD\2\2\u01f0\u0088\3\2\2\2\u01f1\u01f2\7\61\2\2\u01f2"+
		"\u01f3\7\61\2\2\u01f3\u01f7\3\2\2\2\u01f4\u01f6\n\6\2\2\u01f5\u01f4\3"+
		"\2\2\2\u01f6\u01f9\3\2\2\2\u01f7\u01f5\3\2\2\2\u01f7\u01f8\3\2\2\2\u01f8"+
		"\u01ff\3\2\2\2\u01f9\u01f7\3\2\2\2\u01fa\u01fc\7\17\2\2\u01fb\u01fa\3"+
		"\2\2\2\u01fb\u01fc\3\2\2\2\u01fc\u01fd\3\2\2\2\u01fd\u0200\7\f\2\2\u01fe"+
		"\u0200\7\2\2\3\u01ff\u01fb\3\2\2\2\u01ff\u01fe\3\2\2\2\u0200\u020d\3\2"+
		"\2\2\u0201\u0202\7\61\2\2\u0202\u0203\7,\2\2\u0203\u0207\3\2\2\2\u0204"+
		"\u0206\13\2\2\2\u0205\u0204\3\2\2\2\u0206\u0209\3\2\2\2\u0207\u0208\3"+
		"\2\2\2\u0207\u0205\3\2\2\2\u0208\u020a\3\2\2\2\u0209\u0207\3\2\2\2\u020a"+
		"\u020b\7,\2\2\u020b\u020d\7\61\2\2\u020c\u01f1\3\2\2\2\u020c\u0201\3\2"+
		"\2\2\u020d\u020e\3\2\2\2\u020e\u020f\bE\2\2\u020f\u008a\3\2\2\2\22\2\u017b"+
		"\u01be\u01c3\u01c8\u01ce\u01d1\u01d7\u01de\u01e4\u01ed\u01f7\u01fb\u01ff"+
		"\u0207\u020c\3\2\3\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}