// Generated from ANTLRv4Lexer.g4 by ANTLR 4.6
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ANTLRv4Lexer extends LexerAdaptor {
	static { RuntimeMetaData.checkVersion("4.6", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		TOKEN_REF=1, RULE_REF=2, LEXER_CHAR_SET=3, DOC_COMMENT=4, BLOCK_COMMENT=5, 
		LINE_COMMENT=6, BEGIN_ARG_ACTION=7, OPTIONS=8, TOKENS=9, CHANNELS=10, 
		IMPORT=11, FRAGMENT=12, LEXER=13, PARSER=14, GRAMMAR=15, PROTECTED=16, 
		PUBLIC=17, PRIVATE=18, RETURNS=19, LOCALS=20, THROWS=21, CATCH=22, FINALLY=23, 
		MODE=24, COLON=25, COLONCOLON=26, COMMA=27, SEMI=28, LPAREN=29, RPAREN=30, 
		RARROW=31, LT=32, GT=33, ASSIGN=34, QUESTION=35, STAR=36, PLUS=37, PLUS_ASSIGN=38, 
		OR=39, DOLLAR=40, DOT=41, RANGE=42, AT=43, POUND=44, NOT=45, RBRACE=46, 
		INT=47, STRING_LITERAL=48, UNTERMINATED_STRING_LITERAL=49, WS=50, ACTION=51, 
		ERRCHAR=52, ARG_ACTION=53, UNTERMINATED_ARG_ACTION=54, UNTERMINATED_CHAR_SET=55;
	public static final int ArgAction = 1;
	public static final int LexerCharSet = 2;
	public static String[] modeNames = {
		"DEFAULT_MODE", "ArgAction", "LexerCharSet"
	};

	public static final String[] ruleNames = {
		"DOC_COMMENT", "BLOCK_COMMENT", "LINE_COMMENT", "BEGIN_ARG_ACTION", "OPTIONS", 
		"TOKENS", "CHANNELS", "IMPORT", "FRAGMENT", "LEXER", "PARSER", "GRAMMAR", 
		"PROTECTED", "PUBLIC", "PRIVATE", "RETURNS", "LOCALS", "THROWS", "CATCH", 
		"FINALLY", "MODE", "COLON", "COLONCOLON", "COMMA", "SEMI", "LPAREN", "RPAREN", 
		"RARROW", "LT", "GT", "ASSIGN", "QUESTION", "STAR", "PLUS", "PLUS_ASSIGN", 
		"OR", "DOLLAR", "DOT", "RANGE", "AT", "POUND", "NOT", "RBRACE", "RULE_REF", 
		"TOKEN_REF", "NameChar", "NameStartChar", "INT", "STRING_LITERAL", "UNTERMINATED_STRING_LITERAL", 
		"ESC_SEQ", "UNICODE_ESC", "HEX_DIGIT", "WS", "ACTION", "ACTION_ESCAPE", 
		"ACTION_STRING_LITERAL", "ACTION_CHAR_LITERAL", "ERRCHAR", "NESTED_ARG_ACTION", 
		"ARG_ACTION_ESCAPE", "ARG_ACTION_STRING_LITERAL", "ARG_ACTION_CHAR_LITERAL", 
		"ARG_ACTION", "UNTERMINATED_ARG_ACTION", "ARG_ACTION_CHAR", "LEXER_CHAR_SET_BODY", 
		"LEXER_CHAR_SET", "UNTERMINATED_CHAR_SET"
	};

	private static final String[] _LITERAL_NAMES = {
		null, null, null, null, null, null, null, null, null, null, null, "'import'", 
		"'fragment'", "'lexer'", "'parser'", "'grammar'", "'protected'", "'public'", 
		"'private'", "'returns'", "'locals'", "'throws'", "'catch'", "'finally'", 
		"'mode'", "':'", "'::'", "','", "';'", "'('", "')'", "'->'", "'<'", "'>'", 
		"'='", "'?'", "'*'", "'+'", "'+='", "'|'", "'$'", "'.'", "'..'", "'@'", 
		"'#'", "'~'", "'}'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "TOKEN_REF", "RULE_REF", "LEXER_CHAR_SET", "DOC_COMMENT", "BLOCK_COMMENT", 
		"LINE_COMMENT", "BEGIN_ARG_ACTION", "OPTIONS", "TOKENS", "CHANNELS", "IMPORT", 
		"FRAGMENT", "LEXER", "PARSER", "GRAMMAR", "PROTECTED", "PUBLIC", "PRIVATE", 
		"RETURNS", "LOCALS", "THROWS", "CATCH", "FINALLY", "MODE", "COLON", "COLONCOLON", 
		"COMMA", "SEMI", "LPAREN", "RPAREN", "RARROW", "LT", "GT", "ASSIGN", "QUESTION", 
		"STAR", "PLUS", "PLUS_ASSIGN", "OR", "DOLLAR", "DOT", "RANGE", "AT", "POUND", 
		"NOT", "RBRACE", "INT", "STRING_LITERAL", "UNTERMINATED_STRING_LITERAL", 
		"WS", "ACTION", "ERRCHAR", "ARG_ACTION", "UNTERMINATED_ARG_ACTION", "UNTERMINATED_CHAR_SET"
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


	public ANTLRv4Lexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "ANTLRv4Lexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	@Override
	public void action(RuleContext _localctx, int ruleIndex, int actionIndex) {
		switch (ruleIndex) {
		case 3:
			BEGIN_ARG_ACTION_action((RuleContext)_localctx, actionIndex);
			break;
		}
	}
	private void BEGIN_ARG_ACTION_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 0:
			handleBeginArgument();
			break;
		}
	}

	public static final String _serializedATN =
		"\3\u0430\ud6d1\u8206\uad2d\u4417\uaef1\u8d80\uaadd\29\u0243\b\1\b\1\b"+
		"\1\4\2\t\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n"+
		"\t\n\4\13\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21"+
		"\4\22\t\22\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30"+
		"\4\31\t\31\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37"+
		"\4 \t \4!\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t"+
		"*\4+\t+\4,\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63"+
		"\4\64\t\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t"+
		"<\4=\t=\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\4F\tF\3\2\3\2"+
		"\3\2\3\2\3\2\7\2\u0095\n\2\f\2\16\2\u0098\13\2\3\2\3\2\3\2\5\2\u009d\n"+
		"\2\3\3\3\3\3\3\3\3\7\3\u00a3\n\3\f\3\16\3\u00a6\13\3\3\3\3\3\3\3\5\3\u00ab"+
		"\n\3\3\3\3\3\3\4\3\4\3\4\3\4\7\4\u00b3\n\4\f\4\16\4\u00b6\13\4\3\4\3\4"+
		"\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\3\6\7\6\u00c6\n\6\f\6\16"+
		"\6\u00c9\13\6\3\6\3\6\3\7\3\7\3\7\3\7\3\7\3\7\3\7\3\7\7\7\u00d5\n\7\f"+
		"\7\16\7\u00d8\13\7\3\7\3\7\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\7\b"+
		"\u00e6\n\b\f\b\16\b\u00e9\13\b\3\b\3\b\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\n"+
		"\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\n\3\13\3\13\3\13\3\13\3\13\3\13\3\f\3\f"+
		"\3\f\3\f\3\f\3\f\3\f\3\r\3\r\3\r\3\r\3\r\3\r\3\r\3\r\3\16\3\16\3\16\3"+
		"\16\3\16\3\16\3\16\3\16\3\16\3\16\3\17\3\17\3\17\3\17\3\17\3\17\3\17\3"+
		"\20\3\20\3\20\3\20\3\20\3\20\3\20\3\20\3\21\3\21\3\21\3\21\3\21\3\21\3"+
		"\21\3\21\3\22\3\22\3\22\3\22\3\22\3\22\3\22\3\23\3\23\3\23\3\23\3\23\3"+
		"\23\3\23\3\24\3\24\3\24\3\24\3\24\3\24\3\25\3\25\3\25\3\25\3\25\3\25\3"+
		"\25\3\25\3\26\3\26\3\26\3\26\3\26\3\27\3\27\3\30\3\30\3\30\3\31\3\31\3"+
		"\32\3\32\3\33\3\33\3\34\3\34\3\35\3\35\3\35\3\36\3\36\3\37\3\37\3 \3 "+
		"\3!\3!\3\"\3\"\3#\3#\3$\3$\3$\3%\3%\3&\3&\3\'\3\'\3(\3(\3(\3)\3)\3*\3"+
		"*\3+\3+\3,\3,\3-\3-\7-\u0186\n-\f-\16-\u0189\13-\3.\3.\7.\u018d\n.\f."+
		"\16.\u0190\13.\3/\3/\5/\u0194\n/\3\60\3\60\3\61\6\61\u0199\n\61\r\61\16"+
		"\61\u019a\3\62\3\62\3\62\7\62\u01a0\n\62\f\62\16\62\u01a3\13\62\3\62\3"+
		"\62\3\63\3\63\3\63\7\63\u01aa\n\63\f\63\16\63\u01ad\13\63\3\64\3\64\3"+
		"\64\3\64\3\64\5\64\u01b4\n\64\3\65\3\65\3\65\3\65\3\65\5\65\u01bb\n\65"+
		"\5\65\u01bd\n\65\5\65\u01bf\n\65\5\65\u01c1\n\65\3\66\3\66\3\67\6\67\u01c6"+
		"\n\67\r\67\16\67\u01c7\3\67\3\67\38\38\38\38\38\38\38\38\38\78\u01d5\n"+
		"8\f8\168\u01d8\138\38\38\38\38\38\38\78\u01e0\n8\f8\168\u01e3\138\38\7"+
		"8\u01e6\n8\f8\168\u01e9\138\38\58\u01ec\n8\39\39\39\3:\3:\3:\7:\u01f4"+
		"\n:\f:\16:\u01f7\13:\3:\3:\3;\3;\3;\7;\u01fe\n;\f;\16;\u0201\13;\3;\3"+
		";\3<\3<\3<\3<\3=\3=\3=\3=\3=\3>\3>\3>\3>\3>\3?\3?\3?\3?\7?\u0217\n?\f"+
		"?\16?\u021a\13?\3?\3?\3?\3?\3@\3@\3@\3@\3@\5@\u0225\n@\3@\3@\3A\3A\3A"+
		"\3A\3B\3B\3B\3B\3C\3C\3C\3C\3D\3D\3D\5D\u0238\nD\3D\3D\3E\3E\3E\3E\3F"+
		"\3F\3F\3F\6\u0096\u00a4\u01d6\u01e7\2G\5\6\7\7\t\b\13\t\r\n\17\13\21\f"+
		"\23\r\25\16\27\17\31\20\33\21\35\22\37\23!\24#\25%\26\'\27)\30+\31-\32"+
		"/\33\61\34\63\35\65\36\67\379 ;!=\"?#A$C%E&G\'I(K)M*O+Q,S-U.W/Y\60[\4"+
		"]\3_\2a\2c\61e\62g\63i\2k\2m\2o\64q\65s\2u\2w\2y\66{\2}\2\177\2\u0081"+
		"\2\u0083\67\u00858\u0087\2\u0089\2\u008b\5\u008d9\5\2\3\4\21\4\2\f\f\17"+
		"\17\5\2\13\f\16\17\"\"\3\2c|\6\2\62;C\\aac|\3\2C\\\7\2\62;aa\u00b9\u00b9"+
		"\u0302\u0371\u2041\u2042\17\2C\\c|\u00c2\u00d8\u00da\u00f8\u00fa\u0301"+
		"\u0372\u037f\u0381\u2001\u200e\u200f\u2072\u2191\u2c02\u2ff1\u3003\ud801"+
		"\uf902\ufdd1\ufdf2\uffff\3\2\62;\6\2\f\f\17\17))^^\n\2$$))^^ddhhppttv"+
		"v\5\2\62;CHch\3\3\177\177\4\2$$^^\4\2))^^\3\2^_\u0261\2\5\3\2\2\2\2\7"+
		"\3\2\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2"+
		"\2\2\23\3\2\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33\3\2\2\2\2"+
		"\35\3\2\2\2\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2\2\2\2\'\3\2\2\2"+
		"\2)\3\2\2\2\2+\3\2\2\2\2-\3\2\2\2\2/\3\2\2\2\2\61\3\2\2\2\2\63\3\2\2\2"+
		"\2\65\3\2\2\2\2\67\3\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3\2\2\2\2?\3\2\2\2"+
		"\2A\3\2\2\2\2C\3\2\2\2\2E\3\2\2\2\2G\3\2\2\2\2I\3\2\2\2\2K\3\2\2\2\2M"+
		"\3\2\2\2\2O\3\2\2\2\2Q\3\2\2\2\2S\3\2\2\2\2U\3\2\2\2\2W\3\2\2\2\2Y\3\2"+
		"\2\2\2[\3\2\2\2\2]\3\2\2\2\2c\3\2\2\2\2e\3\2\2\2\2g\3\2\2\2\2o\3\2\2\2"+
		"\2q\3\2\2\2\2y\3\2\2\2\3{\3\2\2\2\3}\3\2\2\2\3\177\3\2\2\2\3\u0081\3\2"+
		"\2\2\3\u0083\3\2\2\2\3\u0085\3\2\2\2\3\u0087\3\2\2\2\4\u0089\3\2\2\2\4"+
		"\u008b\3\2\2\2\4\u008d\3\2\2\2\5\u008f\3\2\2\2\7\u009e\3\2\2\2\t\u00ae"+
		"\3\2\2\2\13\u00b9\3\2\2\2\r\u00bc\3\2\2\2\17\u00cc\3\2\2\2\21\u00db\3"+
		"\2\2\2\23\u00ec\3\2\2\2\25\u00f3\3\2\2\2\27\u00fc\3\2\2\2\31\u0102\3\2"+
		"\2\2\33\u0109\3\2\2\2\35\u0111\3\2\2\2\37\u011b\3\2\2\2!\u0122\3\2\2\2"+
		"#\u012a\3\2\2\2%\u0132\3\2\2\2\'\u0139\3\2\2\2)\u0140\3\2\2\2+\u0146\3"+
		"\2\2\2-\u014e\3\2\2\2/\u0153\3\2\2\2\61\u0155\3\2\2\2\63\u0158\3\2\2\2"+
		"\65\u015a\3\2\2\2\67\u015c\3\2\2\29\u015e\3\2\2\2;\u0160\3\2\2\2=\u0163"+
		"\3\2\2\2?\u0165\3\2\2\2A\u0167\3\2\2\2C\u0169\3\2\2\2E\u016b\3\2\2\2G"+
		"\u016d\3\2\2\2I\u016f\3\2\2\2K\u0172\3\2\2\2M\u0174\3\2\2\2O\u0176\3\2"+
		"\2\2Q\u0178\3\2\2\2S\u017b\3\2\2\2U\u017d\3\2\2\2W\u017f\3\2\2\2Y\u0181"+
		"\3\2\2\2[\u0183\3\2\2\2]\u018a\3\2\2\2_\u0193\3\2\2\2a\u0195\3\2\2\2c"+
		"\u0198\3\2\2\2e\u019c\3\2\2\2g\u01a6\3\2\2\2i\u01ae\3\2\2\2k\u01b5\3\2"+
		"\2\2m\u01c2\3\2\2\2o\u01c5\3\2\2\2q\u01cb\3\2\2\2s\u01ed\3\2\2\2u\u01f0"+
		"\3\2\2\2w\u01fa\3\2\2\2y\u0204\3\2\2\2{\u0208\3\2\2\2}\u020d\3\2\2\2\177"+
		"\u0212\3\2\2\2\u0081\u0224\3\2\2\2\u0083\u0228\3\2\2\2\u0085\u022c\3\2"+
		"\2\2\u0087\u0230\3\2\2\2\u0089\u0237\3\2\2\2\u008b\u023b\3\2\2\2\u008d"+
		"\u023f\3\2\2\2\u008f\u0090\7\61\2\2\u0090\u0091\7,\2\2\u0091\u0092\7,"+
		"\2\2\u0092\u0096\3\2\2\2\u0093\u0095\13\2\2\2\u0094\u0093\3\2\2\2\u0095"+
		"\u0098\3\2\2\2\u0096\u0097\3\2\2\2\u0096\u0094\3\2\2\2\u0097\u009c\3\2"+
		"\2\2\u0098\u0096\3\2\2\2\u0099\u009a\7,\2\2\u009a\u009d\7\61\2\2\u009b"+
		"\u009d\7\2\2\3\u009c\u0099\3\2\2\2\u009c\u009b\3\2\2\2\u009d\6\3\2\2\2"+
		"\u009e\u009f\7\61\2\2\u009f\u00a0\7,\2\2\u00a0\u00a4\3\2\2\2\u00a1\u00a3"+
		"\13\2\2\2\u00a2\u00a1\3\2\2\2\u00a3\u00a6\3\2\2\2\u00a4\u00a5\3\2\2\2"+
		"\u00a4\u00a2\3\2\2\2\u00a5\u00aa\3\2\2\2\u00a6\u00a4\3\2\2\2\u00a7\u00a8"+
		"\7,\2\2\u00a8\u00ab\7\61\2\2\u00a9\u00ab\7\2\2\3\u00aa\u00a7\3\2\2\2\u00aa"+
		"\u00a9\3\2\2\2\u00ab\u00ac\3\2\2\2\u00ac\u00ad\b\3\2\2\u00ad\b\3\2\2\2"+
		"\u00ae\u00af\7\61\2\2\u00af\u00b0\7\61\2\2\u00b0\u00b4\3\2\2\2\u00b1\u00b3"+
		"\n\2\2\2\u00b2\u00b1\3\2\2\2\u00b3\u00b6\3\2\2\2\u00b4\u00b2\3\2\2\2\u00b4"+
		"\u00b5\3\2\2\2\u00b5\u00b7\3\2\2\2\u00b6\u00b4\3\2\2\2\u00b7\u00b8\b\4"+
		"\2\2\u00b8\n\3\2\2\2\u00b9\u00ba\7]\2\2\u00ba\u00bb\b\5\3\2\u00bb\f\3"+
		"\2\2\2\u00bc\u00bd\7q\2\2\u00bd\u00be\7r\2\2\u00be\u00bf\7v\2\2\u00bf"+
		"\u00c0\7k\2\2\u00c0\u00c1\7q\2\2\u00c1\u00c2\7p\2\2\u00c2\u00c3\7u\2\2"+
		"\u00c3\u00c7\3\2\2\2\u00c4\u00c6\t\3\2\2\u00c5\u00c4\3\2\2\2\u00c6\u00c9"+
		"\3\2\2\2\u00c7\u00c5\3\2\2\2\u00c7\u00c8\3\2\2\2\u00c8\u00ca\3\2\2\2\u00c9"+
		"\u00c7\3\2\2\2\u00ca\u00cb\7}\2\2\u00cb\16\3\2\2\2\u00cc\u00cd\7v\2\2"+
		"\u00cd\u00ce\7q\2\2\u00ce\u00cf\7m\2\2\u00cf\u00d0\7g\2\2\u00d0\u00d1"+
		"\7p\2\2\u00d1\u00d2\7u\2\2\u00d2\u00d6\3\2\2\2\u00d3\u00d5\t\3\2\2\u00d4"+
		"\u00d3\3\2\2\2\u00d5\u00d8\3\2\2\2\u00d6\u00d4\3\2\2\2\u00d6\u00d7\3\2"+
		"\2\2\u00d7\u00d9\3\2\2\2\u00d8\u00d6\3\2\2\2\u00d9\u00da\7}\2\2\u00da"+
		"\20\3\2\2\2\u00db\u00dc\7e\2\2\u00dc\u00dd\7j\2\2\u00dd\u00de\7c\2\2\u00de"+
		"\u00df\7p\2\2\u00df\u00e0\7p\2\2\u00e0\u00e1\7g\2\2\u00e1\u00e2\7n\2\2"+
		"\u00e2\u00e3\7u\2\2\u00e3\u00e7\3\2\2\2\u00e4\u00e6\t\3\2\2\u00e5\u00e4"+
		"\3\2\2\2\u00e6\u00e9\3\2\2\2\u00e7\u00e5\3\2\2\2\u00e7\u00e8\3\2\2\2\u00e8"+
		"\u00ea\3\2\2\2\u00e9\u00e7\3\2\2\2\u00ea\u00eb\7}\2\2\u00eb\22\3\2\2\2"+
		"\u00ec\u00ed\7k\2\2\u00ed\u00ee\7o\2\2\u00ee\u00ef\7r\2\2\u00ef\u00f0"+
		"\7q\2\2\u00f0\u00f1\7t\2\2\u00f1\u00f2\7v\2\2\u00f2\24\3\2\2\2\u00f3\u00f4"+
		"\7h\2\2\u00f4\u00f5\7t\2\2\u00f5\u00f6\7c\2\2\u00f6\u00f7\7i\2\2\u00f7"+
		"\u00f8\7o\2\2\u00f8\u00f9\7g\2\2\u00f9\u00fa\7p\2\2\u00fa\u00fb\7v\2\2"+
		"\u00fb\26\3\2\2\2\u00fc\u00fd\7n\2\2\u00fd\u00fe\7g\2\2\u00fe\u00ff\7"+
		"z\2\2\u00ff\u0100\7g\2\2\u0100\u0101\7t\2\2\u0101\30\3\2\2\2\u0102\u0103"+
		"\7r\2\2\u0103\u0104\7c\2\2\u0104\u0105\7t\2\2\u0105\u0106\7u\2\2\u0106"+
		"\u0107\7g\2\2\u0107\u0108\7t\2\2\u0108\32\3\2\2\2\u0109\u010a\7i\2\2\u010a"+
		"\u010b\7t\2\2\u010b\u010c\7c\2\2\u010c\u010d\7o\2\2\u010d\u010e\7o\2\2"+
		"\u010e\u010f\7c\2\2\u010f\u0110\7t\2\2\u0110\34\3\2\2\2\u0111\u0112\7"+
		"r\2\2\u0112\u0113\7t\2\2\u0113\u0114\7q\2\2\u0114\u0115\7v\2\2\u0115\u0116"+
		"\7g\2\2\u0116\u0117\7e\2\2\u0117\u0118\7v\2\2\u0118\u0119\7g\2\2\u0119"+
		"\u011a\7f\2\2\u011a\36\3\2\2\2\u011b\u011c\7r\2\2\u011c\u011d\7w\2\2\u011d"+
		"\u011e\7d\2\2\u011e\u011f\7n\2\2\u011f\u0120\7k\2\2\u0120\u0121\7e\2\2"+
		"\u0121 \3\2\2\2\u0122\u0123\7r\2\2\u0123\u0124\7t\2\2\u0124\u0125\7k\2"+
		"\2\u0125\u0126\7x\2\2\u0126\u0127\7c\2\2\u0127\u0128\7v\2\2\u0128\u0129"+
		"\7g\2\2\u0129\"\3\2\2\2\u012a\u012b\7t\2\2\u012b\u012c\7g\2\2\u012c\u012d"+
		"\7v\2\2\u012d\u012e\7w\2\2\u012e\u012f\7t\2\2\u012f\u0130\7p\2\2\u0130"+
		"\u0131\7u\2\2\u0131$\3\2\2\2\u0132\u0133\7n\2\2\u0133\u0134\7q\2\2\u0134"+
		"\u0135\7e\2\2\u0135\u0136\7c\2\2\u0136\u0137\7n\2\2\u0137\u0138\7u\2\2"+
		"\u0138&\3\2\2\2\u0139\u013a\7v\2\2\u013a\u013b\7j\2\2\u013b\u013c\7t\2"+
		"\2\u013c\u013d\7q\2\2\u013d\u013e\7y\2\2\u013e\u013f\7u\2\2\u013f(\3\2"+
		"\2\2\u0140\u0141\7e\2\2\u0141\u0142\7c\2\2\u0142\u0143\7v\2\2\u0143\u0144"+
		"\7e\2\2\u0144\u0145\7j\2\2\u0145*\3\2\2\2\u0146\u0147\7h\2\2\u0147\u0148"+
		"\7k\2\2\u0148\u0149\7p\2\2\u0149\u014a\7c\2\2\u014a\u014b\7n\2\2\u014b"+
		"\u014c\7n\2\2\u014c\u014d\7{\2\2\u014d,\3\2\2\2\u014e\u014f\7o\2\2\u014f"+
		"\u0150\7q\2\2\u0150\u0151\7f\2\2\u0151\u0152\7g\2\2\u0152.\3\2\2\2\u0153"+
		"\u0154\7<\2\2\u0154\60\3\2\2\2\u0155\u0156\7<\2\2\u0156\u0157\7<\2\2\u0157"+
		"\62\3\2\2\2\u0158\u0159\7.\2\2\u0159\64\3\2\2\2\u015a\u015b\7=\2\2\u015b"+
		"\66\3\2\2\2\u015c\u015d\7*\2\2\u015d8\3\2\2\2\u015e\u015f\7+\2\2\u015f"+
		":\3\2\2\2\u0160\u0161\7/\2\2\u0161\u0162\7@\2\2\u0162<\3\2\2\2\u0163\u0164"+
		"\7>\2\2\u0164>\3\2\2\2\u0165\u0166\7@\2\2\u0166@\3\2\2\2\u0167\u0168\7"+
		"?\2\2\u0168B\3\2\2\2\u0169\u016a\7A\2\2\u016aD\3\2\2\2\u016b\u016c\7,"+
		"\2\2\u016cF\3\2\2\2\u016d\u016e\7-\2\2\u016eH\3\2\2\2\u016f\u0170\7-\2"+
		"\2\u0170\u0171\7?\2\2\u0171J\3\2\2\2\u0172\u0173\7~\2\2\u0173L\3\2\2\2"+
		"\u0174\u0175\7&\2\2\u0175N\3\2\2\2\u0176\u0177\7\60\2\2\u0177P\3\2\2\2"+
		"\u0178\u0179\7\60\2\2\u0179\u017a\7\60\2\2\u017aR\3\2\2\2\u017b\u017c"+
		"\7B\2\2\u017cT\3\2\2\2\u017d\u017e\7%\2\2\u017eV\3\2\2\2\u017f\u0180\7"+
		"\u0080\2\2\u0180X\3\2\2\2\u0181\u0182\7\177\2\2\u0182Z\3\2\2\2\u0183\u0187"+
		"\t\4\2\2\u0184\u0186\t\5\2\2\u0185\u0184\3\2\2\2\u0186\u0189\3\2\2\2\u0187"+
		"\u0185\3\2\2\2\u0187\u0188\3\2\2\2\u0188\\\3\2\2\2\u0189\u0187\3\2\2\2"+
		"\u018a\u018e\t\6\2\2\u018b\u018d\t\5\2\2\u018c\u018b\3\2\2\2\u018d\u0190"+
		"\3\2\2\2\u018e\u018c\3\2\2\2\u018e\u018f\3\2\2\2\u018f^\3\2\2\2\u0190"+
		"\u018e\3\2\2\2\u0191\u0194\5a\60\2\u0192\u0194\t\7\2\2\u0193\u0191\3\2"+
		"\2\2\u0193\u0192\3\2\2\2\u0194`\3\2\2\2\u0195\u0196\t\b\2\2\u0196b\3\2"+
		"\2\2\u0197\u0199\t\t\2\2\u0198\u0197\3\2\2\2\u0199\u019a\3\2\2\2\u019a"+
		"\u0198\3\2\2\2\u019a\u019b\3\2\2\2\u019bd\3\2\2\2\u019c\u01a1\7)\2\2\u019d"+
		"\u01a0\5i\64\2\u019e\u01a0\n\n\2\2\u019f\u019d\3\2\2\2\u019f\u019e\3\2"+
		"\2\2\u01a0\u01a3\3\2\2\2\u01a1\u019f\3\2\2\2\u01a1\u01a2\3\2\2\2\u01a2"+
		"\u01a4\3\2\2\2\u01a3\u01a1\3\2\2\2\u01a4\u01a5\7)\2\2\u01a5f\3\2\2\2\u01a6"+
		"\u01ab\7)\2\2\u01a7\u01aa\5i\64\2\u01a8\u01aa\n\n\2\2\u01a9\u01a7\3\2"+
		"\2\2\u01a9\u01a8\3\2\2\2\u01aa\u01ad\3\2\2\2\u01ab\u01a9\3\2\2\2\u01ab"+
		"\u01ac\3\2\2\2\u01ach\3\2\2\2\u01ad\u01ab\3\2\2\2\u01ae\u01b3\7^\2\2\u01af"+
		"\u01b4\t\13\2\2\u01b0\u01b4\5k\65\2\u01b1\u01b4\13\2\2\2\u01b2\u01b4\7"+
		"\2\2\3\u01b3\u01af\3\2\2\2\u01b3\u01b0\3\2\2\2\u01b3\u01b1\3\2\2\2\u01b3"+
		"\u01b2\3\2\2\2\u01b4j\3\2\2\2\u01b5\u01c0\7w\2\2\u01b6\u01be\5m\66\2\u01b7"+
		"\u01bc\5m\66\2\u01b8\u01ba\5m\66\2\u01b9\u01bb\5m\66\2\u01ba\u01b9\3\2"+
		"\2\2\u01ba\u01bb\3\2\2\2\u01bb\u01bd\3\2\2\2\u01bc\u01b8\3\2\2\2\u01bc"+
		"\u01bd\3\2\2\2\u01bd\u01bf\3\2\2\2\u01be\u01b7\3\2\2\2\u01be\u01bf\3\2"+
		"\2\2\u01bf\u01c1\3\2\2\2\u01c0\u01b6\3\2\2\2\u01c0\u01c1\3\2\2\2\u01c1"+
		"l\3\2\2\2\u01c2\u01c3\t\f\2\2\u01c3n\3\2\2\2\u01c4\u01c6\t\3\2\2\u01c5"+
		"\u01c4\3\2\2\2\u01c6\u01c7\3\2\2\2\u01c7\u01c5\3\2\2\2\u01c7\u01c8\3\2"+
		"\2\2\u01c8\u01c9\3\2\2\2\u01c9\u01ca\b\67\2\2\u01cap\3\2\2\2\u01cb\u01e7"+
		"\7}\2\2\u01cc\u01e6\5q8\2\u01cd\u01e6\5s9\2\u01ce\u01e6\5u:\2\u01cf\u01e6"+
		"\5w;\2\u01d0\u01d1\7\61\2\2\u01d1\u01d2\7,\2\2\u01d2\u01d6\3\2\2\2\u01d3"+
		"\u01d5\13\2\2\2\u01d4\u01d3\3\2\2\2\u01d5\u01d8\3\2\2\2\u01d6\u01d7\3"+
		"\2\2\2\u01d6\u01d4\3\2\2\2\u01d7\u01d9\3\2\2\2\u01d8\u01d6\3\2\2\2\u01d9"+
		"\u01da\7,\2\2\u01da\u01e6\7\61\2\2\u01db\u01dc\7\61\2\2\u01dc\u01dd\7"+
		"\61\2\2\u01dd\u01e1\3\2\2\2\u01de\u01e0\n\2\2\2\u01df\u01de\3\2\2\2\u01e0"+
		"\u01e3\3\2\2\2\u01e1\u01df\3\2\2\2\u01e1\u01e2\3\2\2\2\u01e2\u01e6\3\2"+
		"\2\2\u01e3\u01e1\3\2\2\2\u01e4\u01e6\13\2\2\2\u01e5\u01cc\3\2\2\2\u01e5"+
		"\u01cd\3\2\2\2\u01e5\u01ce\3\2\2\2\u01e5\u01cf\3\2\2\2\u01e5\u01d0\3\2"+
		"\2\2\u01e5\u01db\3\2\2\2\u01e5\u01e4\3\2\2\2\u01e6\u01e9\3\2\2\2\u01e7"+
		"\u01e8\3\2\2\2\u01e7\u01e5\3\2\2\2\u01e8\u01eb\3\2\2\2\u01e9\u01e7\3\2"+
		"\2\2\u01ea\u01ec\t\r\2\2\u01eb\u01ea\3\2\2\2\u01ecr\3\2\2\2\u01ed\u01ee"+
		"\7^\2\2\u01ee\u01ef\13\2\2\2\u01eft\3\2\2\2\u01f0\u01f5\7$\2\2\u01f1\u01f4"+
		"\5s9\2\u01f2\u01f4\n\16\2\2\u01f3\u01f1\3\2\2\2\u01f3\u01f2\3\2\2\2\u01f4"+
		"\u01f7\3\2\2\2\u01f5\u01f3\3\2\2\2\u01f5\u01f6\3\2\2\2\u01f6\u01f8\3\2"+
		"\2\2\u01f7\u01f5\3\2\2\2\u01f8\u01f9\7$\2\2\u01f9v\3\2\2\2\u01fa\u01ff"+
		"\7)\2\2\u01fb\u01fe\5s9\2\u01fc\u01fe\n\17\2\2\u01fd\u01fb\3\2\2\2\u01fd"+
		"\u01fc\3\2\2\2\u01fe\u0201\3\2\2\2\u01ff\u01fd\3\2\2\2\u01ff\u0200\3\2"+
		"\2\2\u0200\u0202\3\2\2\2\u0201\u01ff\3\2\2\2\u0202\u0203\7)\2\2\u0203"+
		"x\3\2\2\2\u0204\u0205\13\2\2\2\u0205\u0206\3\2\2\2\u0206\u0207\b<\2\2"+
		"\u0207z\3\2\2\2\u0208\u0209\7]\2\2\u0209\u020a\3\2\2\2\u020a\u020b\b="+
		"\4\2\u020b\u020c\b=\5\2\u020c|\3\2\2\2\u020d\u020e\7^\2\2\u020e\u020f"+
		"\13\2\2\2\u020f\u0210\3\2\2\2\u0210\u0211\b>\4\2\u0211~\3\2\2\2\u0212"+
		"\u0218\7$\2\2\u0213\u0214\7^\2\2\u0214\u0217\13\2\2\2\u0215\u0217\n\16"+
		"\2\2\u0216\u0213\3\2\2\2\u0216\u0215\3\2\2\2\u0217\u021a\3\2\2\2\u0218"+
		"\u0216\3\2\2\2\u0218\u0219\3\2\2\2\u0219\u021b\3\2\2\2\u021a\u0218\3\2"+
		"\2\2\u021b\u021c\7$\2\2\u021c\u021d\3\2\2\2\u021d\u021e\b?\4\2\u021e\u0080"+
		"\3\2\2\2\u021f\u0220\7$\2\2\u0220\u0221\7^\2\2\u0221\u0225\13\2\2\2\u0222"+
		"\u0223\n\16\2\2\u0223\u0225\7$\2\2\u0224\u021f\3\2\2\2\u0224\u0222\3\2"+
		"\2\2\u0225\u0226\3\2\2\2\u0226\u0227\b@\4\2\u0227\u0082\3\2\2\2\u0228"+
		"\u0229\7_\2\2\u0229\u022a\3\2\2\2\u022a\u022b\bA\6\2\u022b\u0084\3\2\2"+
		"\2\u022c\u022d\7\2\2\3\u022d\u022e\3\2\2\2\u022e\u022f\bB\6\2\u022f\u0086"+
		"\3\2\2\2\u0230\u0231\13\2\2\2\u0231\u0232\3\2\2\2\u0232\u0233\bC\4\2\u0233"+
		"\u0088\3\2\2\2\u0234\u0238\n\20\2\2\u0235\u0236\7^\2\2\u0236\u0238\13"+
		"\2\2\2\u0237\u0234\3\2\2\2\u0237\u0235\3\2\2\2\u0238\u0239\3\2\2\2\u0239"+
		"\u023a\bD\4\2\u023a\u008a\3\2\2\2\u023b\u023c\7_\2\2\u023c\u023d\3\2\2"+
		"\2\u023d\u023e\bE\6\2\u023e\u008c\3\2\2\2\u023f\u0240\7\2\2\3\u0240\u0241"+
		"\3\2\2\2\u0241\u0242\bF\6\2\u0242\u008e\3\2\2\2(\2\3\4\u0096\u009c\u00a4"+
		"\u00aa\u00b4\u00c7\u00d6\u00e7\u0187\u018e\u0193\u019a\u019f\u01a1\u01a9"+
		"\u01ab\u01b3\u01ba\u01bc\u01be\u01c0\u01c7\u01d6\u01e1\u01e5\u01e7\u01eb"+
		"\u01f3\u01f5\u01fd\u01ff\u0216\u0218\u0224\u0237\7\2\3\2\3\5\2\5\2\2\7"+
		"\3\2\6\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}