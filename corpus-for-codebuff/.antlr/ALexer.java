// Generated from c:\Users\kenne\Documents\AntlrVSIX\corpus-for-codebuff\A.g4 by ANTLR 4.7.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ALexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.7.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, INT=2, WS=3;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	public static final String[] ruleNames = {
		"T__0", "INT", "WS"
	};

	private static final String[] _LITERAL_NAMES = {
		null, "'*'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, null, "INT", "WS"
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


	public ALexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "A.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\5\27\b\1\4\2\t\2"+
		"\4\3\t\3\4\4\t\4\3\2\3\2\3\3\6\3\r\n\3\r\3\16\3\16\3\4\6\4\22\n\4\r\4"+
		"\16\4\23\3\4\3\4\2\2\5\3\3\5\4\7\5\3\2\4\3\2\62;\4\2\13\f\"\"\2\30\2\3"+
		"\3\2\2\2\2\5\3\2\2\2\2\7\3\2\2\2\3\t\3\2\2\2\5\f\3\2\2\2\7\21\3\2\2\2"+
		"\t\n\7,\2\2\n\4\3\2\2\2\13\r\t\2\2\2\f\13\3\2\2\2\r\16\3\2\2\2\16\f\3"+
		"\2\2\2\16\17\3\2\2\2\17\6\3\2\2\2\20\22\t\3\2\2\21\20\3\2\2\2\22\23\3"+
		"\2\2\2\23\21\3\2\2\2\23\24\3\2\2\2\24\25\3\2\2\2\25\26\b\4\2\2\26\b\3"+
		"\2\2\2\5\2\16\23\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}