// Generated from tsql.g4 by ANTLR 4.6
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class tsqlParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.6", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		ADD=1, ALL=2, ALTER=3, AND=4, ANY=5, AS=6, ASC=7, AUTHORIZATION=8, BACKUP=9, 
		BEGIN=10, BETWEEN=11, BREAK=12, BROWSE=13, BULK=14, BY=15, CASCADE=16, 
		CASE=17, CHANGETABLE=18, CHANGES=19, CHECK=20, CHECKPOINT=21, CLOSE=22, 
		CLUSTERED=23, COALESCE=24, COLLATE=25, COLUMN=26, COMMIT=27, COMPUTE=28, 
		CONSTRAINT=29, CONTAINS=30, CONTAINSTABLE=31, CONTINUE=32, CONVERT=33, 
		CREATE=34, CROSS=35, CURRENT=36, CURRENT_DATE=37, CURRENT_TIME=38, CURRENT_TIMESTAMP=39, 
		CURRENT_USER=40, CURSOR=41, DATABASE=42, DBCC=43, DEALLOCATE=44, DECLARE=45, 
		DEFAULT=46, DELETE=47, DENY=48, DESC=49, DISK=50, DISTINCT=51, DISTRIBUTED=52, 
		DOUBLE=53, DROP=54, DUMP=55, ELSE=56, END=57, ERRLVL=58, ESCAPE=59, EXCEPT=60, 
		EXEC=61, EXECUTE=62, EXISTS=63, EXIT=64, EXTERNAL=65, FETCH=66, FILE=67, 
		FILLFACTOR=68, FOR=69, FORCESEEK=70, FOREIGN=71, FREETEXT=72, FREETEXTTABLE=73, 
		FROM=74, FULL=75, FUNCTION=76, GOTO=77, GRANT=78, GROUP=79, HAVING=80, 
		IDENTITY=81, IDENTITYCOL=82, IDENTITY_INSERT=83, IF=84, IN=85, INDEX=86, 
		INNER=87, INSERT=88, INTERSECT=89, INTO=90, IS=91, JOIN=92, KEY=93, KILL=94, 
		LEFT=95, LIKE=96, LINENO=97, LOAD=98, MERGE=99, NATIONAL=100, NOCHECK=101, 
		NONCLUSTERED=102, NOT=103, NULL=104, NULLIF=105, OF=106, OFF=107, OFFSETS=108, 
		ON=109, OPEN=110, OPENDATASOURCE=111, OPENQUERY=112, OPENROWSET=113, OPENXML=114, 
		OPTION=115, OR=116, ORDER=117, OUTER=118, OVER=119, PERCENT=120, PIVOT=121, 
		PLAN=122, PRECISION=123, PRIMARY=124, PRINT=125, PROC=126, PROCEDURE=127, 
		PUBLIC=128, RAISERROR=129, READ=130, READTEXT=131, RECONFIGURE=132, REFERENCES=133, 
		REPLICATION=134, RESTORE=135, RESTRICT=136, RETURN=137, REVERT=138, REVOKE=139, 
		RIGHT=140, ROLLBACK=141, ROWCOUNT=142, ROWGUIDCOL=143, RULE=144, SAVE=145, 
		SCHEMA=146, SECURITYAUDIT=147, SELECT=148, SEMANTICKEYPHRASETABLE=149, 
		SEMANTICSIMILARITYDETAILSTABLE=150, SEMANTICSIMILARITYTABLE=151, SESSION_USER=152, 
		SET=153, SETUSER=154, SHUTDOWN=155, SOME=156, STATISTICS=157, SYSTEM_USER=158, 
		TABLE=159, TABLESAMPLE=160, TEXTSIZE=161, THEN=162, TO=163, TOP=164, TRAN=165, 
		TRANSACTION=166, TRIGGER=167, TRUNCATE=168, TRY_CONVERT=169, TSEQUAL=170, 
		UNION=171, UNIQUE=172, UNPIVOT=173, UPDATE=174, UPDATETEXT=175, USE=176, 
		USER=177, VALUES=178, VARYING=179, VIEW=180, WAITFOR=181, WHEN=182, WHERE=183, 
		WHILE=184, WITH=185, WITHIN=186, WRITETEXT=187, ABSOLUTE=188, APPLY=189, 
		AUTO=190, AVG=191, BASE64=192, BINARY_CHECKSUM=193, CALLER=194, CAST=195, 
		CATCH=196, CHECKSUM=197, CHECKSUM_AGG=198, COMMITTED=199, CONCAT=200, 
		COOKIE=201, COUNT=202, COUNT_BIG=203, DATEADD=204, DATEDIFF=205, DATENAME=206, 
		DATEPART=207, DELAY=208, DELETED=209, DENSE_RANK=210, DISABLE=211, DYNAMIC=212, 
		ENCRYPTION=213, EXPAND=214, FAST=215, FAST_FORWARD=216, FIRST=217, FORCE=218, 
		FORCED=219, FOLLOWING=220, FORWARD_ONLY=221, FULLSCAN=222, GLOBAL=223, 
		GO=224, GROUPING=225, GROUPING_ID=226, HASH=227, INSENSITIVE=228, INSERTED=229, 
		ISOLATION=230, KEEP=231, KEEPFIXED=232, IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX=233, 
		KEYSET=234, LAST=235, LEVEL=236, LOCAL=237, LOCK_ESCALATION=238, LOGIN=239, 
		LOOP=240, MARK=241, MAX=242, MAXDOP=243, MAXRECURSION=244, MIN=245, MIN_ACTIVE_ROWVERSION=246, 
		MODIFY=247, NEXT=248, NAME=249, NOCOUNT=250, NOEXPAND=251, NORECOMPUTE=252, 
		NTILE=253, NUMBER=254, OFFSET=255, ONLY=256, OPTIMISTIC=257, OPTIMIZE=258, 
		OUT=259, OUTPUT=260, OWNER=261, PARAMETERIZATION=262, PARTITION=263, PATH=264, 
		PRECEDING=265, PRIOR=266, RANGE=267, RANK=268, READONLY=269, READ_ONLY=270, 
		RECOMPILE=271, RELATIVE=272, REMOTE=273, REPEATABLE=274, ROBUST=275, ROOT=276, 
		ROW=277, ROWGUID=278, ROWS=279, ROW_NUMBER=280, SAMPLE=281, SCHEMABINDING=282, 
		SCROLL=283, SCROLL_LOCKS=284, SELF=285, SERIALIZABLE=286, SIMPLE=287, 
		SNAPSHOT=288, SPATIAL_WINDOW_MAX_CELLS=289, STATIC=290, STATS_STREAM=291, 
		STDEV=292, STDEVP=293, SUM=294, THROW=295, TIES=296, TIME=297, TRY=298, 
		TYPE=299, TYPE_WARNING=300, UNBOUNDED=301, UNCOMMITTED=302, UNKNOWN=303, 
		USING=304, VAR=305, VARP=306, VIEW_METADATA=307, VIEWS=308, WORK=309, 
		XML=310, XMLNAMESPACES=311, DOLLAR_ACTION=312, SPACE=313, COMMENT=314, 
		LINE_COMMENT=315, DOUBLE_QUOTE_ID=316, SQUARE_BRACKET_ID=317, LOCAL_ID=318, 
		DECIMAL=319, ID=320, STRING=321, BINARY=322, FLOAT=323, REAL=324, EQUAL=325, 
		GREATER=326, LESS=327, EXCLAMATION=328, PLUS_ASSIGN=329, MINUS_ASSIGN=330, 
		MULT_ASSIGN=331, DIV_ASSIGN=332, MOD_ASSIGN=333, AND_ASSIGN=334, XOR_ASSIGN=335, 
		OR_ASSIGN=336, DOT=337, UNDERLINE=338, AT=339, SHARP=340, DOLLAR=341, 
		LR_BRACKET=342, RR_BRACKET=343, COMMA=344, SEMI=345, COLON=346, STAR=347, 
		DIVIDE=348, MODULE=349, PLUS=350, MINUS=351, BIT_NOT=352, BIT_OR=353, 
		BIT_AND=354, BIT_XOR=355;
	public static final int
		RULE_tsql_file = 0, RULE_sql_clause = 1, RULE_dml_clause = 2, RULE_ddl_clause = 3, 
		RULE_cfl_statement = 4, RULE_another_statement = 5, RULE_delete_statement = 6, 
		RULE_insert_statement = 7, RULE_select_statement = 8, RULE_update_statement = 9, 
		RULE_output_clause = 10, RULE_output_dml_list_elem = 11, RULE_output_column_name = 12, 
		RULE_create_index = 13, RULE_create_procedure = 14, RULE_procedure_param = 15, 
		RULE_procedure_option = 16, RULE_create_statistics = 17, RULE_create_table = 18, 
		RULE_create_view = 19, RULE_view_attribute = 20, RULE_alter_table = 21, 
		RULE_alter_database = 22, RULE_database_option = 23, RULE_drop_index = 24, 
		RULE_drop_procedure = 25, RULE_drop_statistics = 26, RULE_drop_table = 27, 
		RULE_drop_view = 28, RULE_rowset_function_limited = 29, RULE_openquery = 30, 
		RULE_opendatasource = 31, RULE_declare_statement = 32, RULE_cursor_statement = 33, 
		RULE_execute_statement = 34, RULE_execute_statement_arg = 35, RULE_execute_var_string = 36, 
		RULE_security_statement = 37, RULE_set_statment = 38, RULE_transaction_statement = 39, 
		RULE_go_statement = 40, RULE_use_statement = 41, RULE_execute_clause = 42, 
		RULE_declare_local = 43, RULE_table_type_definition = 44, RULE_column_def_table_constraint = 45, 
		RULE_column_definition = 46, RULE_column_constraint = 47, RULE_table_constraint = 48, 
		RULE_index_options = 49, RULE_index_option = 50, RULE_declare_cursor = 51, 
		RULE_declare_set_cursor_common = 52, RULE_fetch_cursor = 53, RULE_set_special = 54, 
		RULE_expression = 55, RULE_constant_expression = 56, RULE_subquery = 57, 
		RULE_with_expression = 58, RULE_common_table_expression = 59, RULE_update_elem = 60, 
		RULE_search_condition_list = 61, RULE_search_condition = 62, RULE_search_condition_or = 63, 
		RULE_search_condition_not = 64, RULE_predicate = 65, RULE_query_expression = 66, 
		RULE_union = 67, RULE_query_specification = 68, RULE_order_by_clause = 69, 
		RULE_for_clause = 70, RULE_xml_common_directives = 71, RULE_order_by_expression = 72, 
		RULE_group_by_item = 73, RULE_option_clause = 74, RULE_option = 75, RULE_optimize_for_arg = 76, 
		RULE_select_list = 77, RULE_select_list_elem = 78, RULE_partition_by_clause = 79, 
		RULE_table_source = 80, RULE_table_source_item_joined = 81, RULE_table_source_item = 82, 
		RULE_change_table = 83, RULE_join_part = 84, RULE_table_name_with_hint = 85, 
		RULE_rowset_function = 86, RULE_bulk_option = 87, RULE_derived_table = 88, 
		RULE_function_call = 89, RULE_datepart = 90, RULE_as_table_alias = 91, 
		RULE_table_alias = 92, RULE_with_table_hints = 93, RULE_insert_with_table_hints = 94, 
		RULE_table_hint = 95, RULE_index_column_name = 96, RULE_index_value = 97, 
		RULE_column_alias_list = 98, RULE_column_alias = 99, RULE_expression_list = 100, 
		RULE_case_expr = 101, RULE_ranking_windowed_function = 102, RULE_aggregate_windowed_function = 103, 
		RULE_all_distinct_expression = 104, RULE_over_clause = 105, RULE_row_or_range_clause = 106, 
		RULE_window_frame_extent = 107, RULE_window_frame_bound = 108, RULE_window_frame_preceding = 109, 
		RULE_window_frame_following = 110, RULE_full_table_name = 111, RULE_table_name = 112, 
		RULE_view_name = 113, RULE_func_proc_name = 114, RULE_ddl_object = 115, 
		RULE_full_column_name = 116, RULE_column_name_list = 117, RULE_column_name = 118, 
		RULE_cursor_name = 119, RULE_on_off = 120, RULE_clustered = 121, RULE_null_notnull = 122, 
		RULE_scalar_function_name = 123, RULE_data_type = 124, RULE_default_value = 125, 
		RULE_constant = 126, RULE_number = 127, RULE_sign = 128, RULE_id = 129, 
		RULE_simple_id = 130, RULE_comparison_operator = 131, RULE_assignment_operator = 132;
	public static final String[] ruleNames = {
		"tsql_file", "sql_clause", "dml_clause", "ddl_clause", "cfl_statement", 
		"another_statement", "delete_statement", "insert_statement", "select_statement", 
		"update_statement", "output_clause", "output_dml_list_elem", "output_column_name", 
		"create_index", "create_procedure", "procedure_param", "procedure_option", 
		"create_statistics", "create_table", "create_view", "view_attribute", 
		"alter_table", "alter_database", "database_option", "drop_index", "drop_procedure", 
		"drop_statistics", "drop_table", "drop_view", "rowset_function_limited", 
		"openquery", "opendatasource", "declare_statement", "cursor_statement", 
		"execute_statement", "execute_statement_arg", "execute_var_string", "security_statement", 
		"set_statment", "transaction_statement", "go_statement", "use_statement", 
		"execute_clause", "declare_local", "table_type_definition", "column_def_table_constraint", 
		"column_definition", "column_constraint", "table_constraint", "index_options", 
		"index_option", "declare_cursor", "declare_set_cursor_common", "fetch_cursor", 
		"set_special", "expression", "constant_expression", "subquery", "with_expression", 
		"common_table_expression", "update_elem", "search_condition_list", "search_condition", 
		"search_condition_or", "search_condition_not", "predicate", "query_expression", 
		"union", "query_specification", "order_by_clause", "for_clause", "xml_common_directives", 
		"order_by_expression", "group_by_item", "option_clause", "option", "optimize_for_arg", 
		"select_list", "select_list_elem", "partition_by_clause", "table_source", 
		"table_source_item_joined", "table_source_item", "change_table", "join_part", 
		"table_name_with_hint", "rowset_function", "bulk_option", "derived_table", 
		"function_call", "datepart", "as_table_alias", "table_alias", "with_table_hints", 
		"insert_with_table_hints", "table_hint", "index_column_name", "index_value", 
		"column_alias_list", "column_alias", "expression_list", "case_expr", "ranking_windowed_function", 
		"aggregate_windowed_function", "all_distinct_expression", "over_clause", 
		"row_or_range_clause", "window_frame_extent", "window_frame_bound", "window_frame_preceding", 
		"window_frame_following", "full_table_name", "table_name", "view_name", 
		"func_proc_name", "ddl_object", "full_column_name", "column_name_list", 
		"column_name", "cursor_name", "on_off", "clustered", "null_notnull", "scalar_function_name", 
		"data_type", "default_value", "constant", "number", "sign", "id", "simple_id", 
		"comparison_operator", "assignment_operator"
	};

	private static final String[] _LITERAL_NAMES = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, "'='", "'>'", "'<'", "'!'", "'+='", "'-='", "'*='", "'/='", "'%='", 
		"'&='", "'^='", "'|='", "'.'", "'_'", "'@'", "'#'", "'$'", "'('", "')'", 
		"','", "';'", "':'", "'*'", "'/'", "'%'", "'+'", "'-'", "'~'", "'|'", 
		"'&'", "'^'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", 
		"BACKUP", "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", 
		"CASE", "CHANGETABLE", "CHANGES", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", 
		"COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", 
		"CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT", 
		"CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", 
		"DATABASE", "DBCC", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", 
		"DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMP", "ELSE", 
		"END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", 
		"EXTERNAL", "FETCH", "FILE", "FILLFACTOR", "FOR", "FORCESEEK", "FOREIGN", 
		"FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GOTO", "GRANT", 
		"GROUP", "HAVING", "IDENTITY", "IDENTITYCOL", "IDENTITY_INSERT", "IF", 
		"IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY", 
		"KILL", "LEFT", "LIKE", "LINENO", "LOAD", "MERGE", "NATIONAL", "NOCHECK", 
		"NONCLUSTERED", "NOT", "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", 
		"OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML", "OPTION", 
		"OR", "ORDER", "OUTER", "OVER", "PERCENT", "PIVOT", "PLAN", "PRECISION", 
		"PRIMARY", "PRINT", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", 
		"READTEXT", "RECONFIGURE", "REFERENCES", "REPLICATION", "RESTORE", "RESTRICT", 
		"RETURN", "REVERT", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", 
		"RULE", "SAVE", "SCHEMA", "SECURITYAUDIT", "SELECT", "SEMANTICKEYPHRASETABLE", 
		"SEMANTICSIMILARITYDETAILSTABLE", "SEMANTICSIMILARITYTABLE", "SESSION_USER", 
		"SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", 
		"TABLESAMPLE", "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", 
		"TRIGGER", "TRUNCATE", "TRY_CONVERT", "TSEQUAL", "UNION", "UNIQUE", "UNPIVOT", 
		"UPDATE", "UPDATETEXT", "USE", "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", 
		"WHEN", "WHERE", "WHILE", "WITH", "WITHIN", "WRITETEXT", "ABSOLUTE", "APPLY", 
		"AUTO", "AVG", "BASE64", "BINARY_CHECKSUM", "CALLER", "CAST", "CATCH", 
		"CHECKSUM", "CHECKSUM_AGG", "COMMITTED", "CONCAT", "COOKIE", "COUNT", 
		"COUNT_BIG", "DATEADD", "DATEDIFF", "DATENAME", "DATEPART", "DELAY", "DELETED", 
		"DENSE_RANK", "DISABLE", "DYNAMIC", "ENCRYPTION", "EXPAND", "FAST", "FAST_FORWARD", 
		"FIRST", "FORCE", "FORCED", "FOLLOWING", "FORWARD_ONLY", "FULLSCAN", "GLOBAL", 
		"GO", "GROUPING", "GROUPING_ID", "HASH", "INSENSITIVE", "INSERTED", "ISOLATION", 
		"KEEP", "KEEPFIXED", "IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX", "KEYSET", 
		"LAST", "LEVEL", "LOCAL", "LOCK_ESCALATION", "LOGIN", "LOOP", "MARK", 
		"MAX", "MAXDOP", "MAXRECURSION", "MIN", "MIN_ACTIVE_ROWVERSION", "MODIFY", 
		"NEXT", "NAME", "NOCOUNT", "NOEXPAND", "NORECOMPUTE", "NTILE", "NUMBER", 
		"OFFSET", "ONLY", "OPTIMISTIC", "OPTIMIZE", "OUT", "OUTPUT", "OWNER", 
		"PARAMETERIZATION", "PARTITION", "PATH", "PRECEDING", "PRIOR", "RANGE", 
		"RANK", "READONLY", "READ_ONLY", "RECOMPILE", "RELATIVE", "REMOTE", "REPEATABLE", 
		"ROBUST", "ROOT", "ROW", "ROWGUID", "ROWS", "ROW_NUMBER", "SAMPLE", "SCHEMABINDING", 
		"SCROLL", "SCROLL_LOCKS", "SELF", "SERIALIZABLE", "SIMPLE", "SNAPSHOT", 
		"SPATIAL_WINDOW_MAX_CELLS", "STATIC", "STATS_STREAM", "STDEV", "STDEVP", 
		"SUM", "THROW", "TIES", "TIME", "TRY", "TYPE", "TYPE_WARNING", "UNBOUNDED", 
		"UNCOMMITTED", "UNKNOWN", "USING", "VAR", "VARP", "VIEW_METADATA", "VIEWS", 
		"WORK", "XML", "XMLNAMESPACES", "DOLLAR_ACTION", "SPACE", "COMMENT", "LINE_COMMENT", 
		"DOUBLE_QUOTE_ID", "SQUARE_BRACKET_ID", "LOCAL_ID", "DECIMAL", "ID", "STRING", 
		"BINARY", "FLOAT", "REAL", "EQUAL", "GREATER", "LESS", "EXCLAMATION", 
		"PLUS_ASSIGN", "MINUS_ASSIGN", "MULT_ASSIGN", "DIV_ASSIGN", "MOD_ASSIGN", 
		"AND_ASSIGN", "XOR_ASSIGN", "OR_ASSIGN", "DOT", "UNDERLINE", "AT", "SHARP", 
		"DOLLAR", "LR_BRACKET", "RR_BRACKET", "COMMA", "SEMI", "COLON", "STAR", 
		"DIVIDE", "MODULE", "PLUS", "MINUS", "BIT_NOT", "BIT_OR", "BIT_AND", "BIT_XOR"
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

	@Override
	public String getGrammarFileName() { return "tsql.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public tsqlParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}
	public static class Tsql_fileContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(tsqlParser.EOF, 0); }
		public List<Sql_clauseContext> sql_clause() {
			return getRuleContexts(Sql_clauseContext.class);
		}
		public Sql_clauseContext sql_clause(int i) {
			return getRuleContext(Sql_clauseContext.class,i);
		}
		public Tsql_fileContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_tsql_file; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTsql_file(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTsql_file(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTsql_file(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Tsql_fileContext tsql_file() throws RecognitionException {
		Tsql_fileContext _localctx = new Tsql_fileContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_tsql_file);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(269);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ALTER) | (1L << BEGIN) | (1L << BREAK) | (1L << CLOSE) | (1L << COMMIT) | (1L << CONTINUE) | (1L << CREATE) | (1L << DEALLOCATE) | (1L << DECLARE) | (1L << DELETE) | (1L << DROP) | (1L << EXEC) | (1L << EXECUTE))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (FETCH - 66)) | (1L << (FORCESEEK - 66)) | (1L << (GOTO - 66)) | (1L << (IF - 66)) | (1L << (INSERT - 66)) | (1L << (OPEN - 66)) | (1L << (PRINT - 66)) | (1L << (RAISERROR - 66)))) != 0) || ((((_la - 137)) & ~0x3f) == 0 && ((1L << (_la - 137)) & ((1L << (RETURN - 137)) | (1L << (REVERT - 137)) | (1L << (ROLLBACK - 137)) | (1L << (SAVE - 137)) | (1L << (SELECT - 137)) | (1L << (SET - 137)) | (1L << (UPDATE - 137)) | (1L << (USE - 137)) | (1L << (WAITFOR - 137)) | (1L << (WHILE - 137)) | (1L << (WITH - 137)) | (1L << (ABSOLUTE - 137)) | (1L << (APPLY - 137)) | (1L << (AUTO - 137)) | (1L << (AVG - 137)) | (1L << (BASE64 - 137)) | (1L << (CALLER - 137)) | (1L << (CAST - 137)) | (1L << (CATCH - 137)) | (1L << (CHECKSUM_AGG - 137)) | (1L << (COMMITTED - 137)) | (1L << (CONCAT - 137)))) != 0) || ((((_la - 201)) & ~0x3f) == 0 && ((1L << (_la - 201)) & ((1L << (COOKIE - 201)) | (1L << (COUNT - 201)) | (1L << (COUNT_BIG - 201)) | (1L << (DELAY - 201)) | (1L << (DELETED - 201)) | (1L << (DENSE_RANK - 201)) | (1L << (DISABLE - 201)) | (1L << (DYNAMIC - 201)) | (1L << (ENCRYPTION - 201)) | (1L << (EXPAND - 201)) | (1L << (FAST - 201)) | (1L << (FAST_FORWARD - 201)) | (1L << (FIRST - 201)) | (1L << (FORCE - 201)) | (1L << (FORCED - 201)) | (1L << (FOLLOWING - 201)) | (1L << (FORWARD_ONLY - 201)) | (1L << (FULLSCAN - 201)) | (1L << (GLOBAL - 201)) | (1L << (GO - 201)) | (1L << (GROUPING - 201)) | (1L << (GROUPING_ID - 201)) | (1L << (HASH - 201)) | (1L << (INSENSITIVE - 201)) | (1L << (INSERTED - 201)) | (1L << (ISOLATION - 201)) | (1L << (KEEP - 201)) | (1L << (KEEPFIXED - 201)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 201)) | (1L << (KEYSET - 201)) | (1L << (LAST - 201)) | (1L << (LEVEL - 201)) | (1L << (LOCAL - 201)) | (1L << (LOCK_ESCALATION - 201)) | (1L << (LOGIN - 201)) | (1L << (LOOP - 201)) | (1L << (MARK - 201)) | (1L << (MAX - 201)) | (1L << (MAXDOP - 201)) | (1L << (MAXRECURSION - 201)) | (1L << (MIN - 201)) | (1L << (MODIFY - 201)) | (1L << (NEXT - 201)) | (1L << (NAME - 201)) | (1L << (NOCOUNT - 201)) | (1L << (NOEXPAND - 201)) | (1L << (NORECOMPUTE - 201)) | (1L << (NTILE - 201)) | (1L << (NUMBER - 201)) | (1L << (OFFSET - 201)) | (1L << (ONLY - 201)) | (1L << (OPTIMISTIC - 201)) | (1L << (OPTIMIZE - 201)) | (1L << (OUT - 201)) | (1L << (OUTPUT - 201)) | (1L << (OWNER - 201)) | (1L << (PARAMETERIZATION - 201)) | (1L << (PARTITION - 201)) | (1L << (PATH - 201)))) != 0) || ((((_la - 265)) & ~0x3f) == 0 && ((1L << (_la - 265)) & ((1L << (PRECEDING - 265)) | (1L << (PRIOR - 265)) | (1L << (RANGE - 265)) | (1L << (RANK - 265)) | (1L << (READONLY - 265)) | (1L << (READ_ONLY - 265)) | (1L << (RECOMPILE - 265)) | (1L << (RELATIVE - 265)) | (1L << (REMOTE - 265)) | (1L << (REPEATABLE - 265)) | (1L << (ROBUST - 265)) | (1L << (ROOT - 265)) | (1L << (ROW - 265)) | (1L << (ROWGUID - 265)) | (1L << (ROWS - 265)) | (1L << (ROW_NUMBER - 265)) | (1L << (SAMPLE - 265)) | (1L << (SCHEMABINDING - 265)) | (1L << (SCROLL - 265)) | (1L << (SCROLL_LOCKS - 265)) | (1L << (SELF - 265)) | (1L << (SERIALIZABLE - 265)) | (1L << (SIMPLE - 265)) | (1L << (SNAPSHOT - 265)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 265)) | (1L << (STATIC - 265)) | (1L << (STATS_STREAM - 265)) | (1L << (STDEV - 265)) | (1L << (STDEVP - 265)) | (1L << (SUM - 265)) | (1L << (THROW - 265)) | (1L << (TIES - 265)) | (1L << (TIME - 265)) | (1L << (TRY - 265)) | (1L << (TYPE - 265)) | (1L << (TYPE_WARNING - 265)) | (1L << (UNBOUNDED - 265)) | (1L << (UNCOMMITTED - 265)) | (1L << (UNKNOWN - 265)) | (1L << (USING - 265)) | (1L << (VAR - 265)) | (1L << (VARP - 265)) | (1L << (VIEW_METADATA - 265)) | (1L << (VIEWS - 265)) | (1L << (WORK - 265)) | (1L << (XML - 265)) | (1L << (XMLNAMESPACES - 265)) | (1L << (DOUBLE_QUOTE_ID - 265)) | (1L << (SQUARE_BRACKET_ID - 265)) | (1L << (ID - 265)))) != 0) || _la==LR_BRACKET) {
				{
				{
				setState(266);
				sql_clause();
				}
				}
				setState(271);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(272);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Sql_clauseContext extends ParserRuleContext {
		public Dml_clauseContext dml_clause() {
			return getRuleContext(Dml_clauseContext.class,0);
		}
		public Ddl_clauseContext ddl_clause() {
			return getRuleContext(Ddl_clauseContext.class,0);
		}
		public Cfl_statementContext cfl_statement() {
			return getRuleContext(Cfl_statementContext.class,0);
		}
		public Another_statementContext another_statement() {
			return getRuleContext(Another_statementContext.class,0);
		}
		public Sql_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sql_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSql_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSql_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSql_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Sql_clauseContext sql_clause() throws RecognitionException {
		Sql_clauseContext _localctx = new Sql_clauseContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_sql_clause);
		try {
			setState(278);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(274);
				dml_clause();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(275);
				ddl_clause();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(276);
				cfl_statement();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(277);
				another_statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Dml_clauseContext extends ParserRuleContext {
		public Delete_statementContext delete_statement() {
			return getRuleContext(Delete_statementContext.class,0);
		}
		public Insert_statementContext insert_statement() {
			return getRuleContext(Insert_statementContext.class,0);
		}
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public Update_statementContext update_statement() {
			return getRuleContext(Update_statementContext.class,0);
		}
		public Dml_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_dml_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDml_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDml_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDml_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Dml_clauseContext dml_clause() throws RecognitionException {
		Dml_clauseContext _localctx = new Dml_clauseContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_dml_clause);
		try {
			setState(284);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(280);
				delete_statement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(281);
				insert_statement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(282);
				select_statement();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(283);
				update_statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Ddl_clauseContext extends ParserRuleContext {
		public Create_indexContext create_index() {
			return getRuleContext(Create_indexContext.class,0);
		}
		public Create_procedureContext create_procedure() {
			return getRuleContext(Create_procedureContext.class,0);
		}
		public Create_statisticsContext create_statistics() {
			return getRuleContext(Create_statisticsContext.class,0);
		}
		public Create_tableContext create_table() {
			return getRuleContext(Create_tableContext.class,0);
		}
		public Create_viewContext create_view() {
			return getRuleContext(Create_viewContext.class,0);
		}
		public Alter_tableContext alter_table() {
			return getRuleContext(Alter_tableContext.class,0);
		}
		public Alter_databaseContext alter_database() {
			return getRuleContext(Alter_databaseContext.class,0);
		}
		public Drop_indexContext drop_index() {
			return getRuleContext(Drop_indexContext.class,0);
		}
		public Drop_procedureContext drop_procedure() {
			return getRuleContext(Drop_procedureContext.class,0);
		}
		public Drop_statisticsContext drop_statistics() {
			return getRuleContext(Drop_statisticsContext.class,0);
		}
		public Drop_tableContext drop_table() {
			return getRuleContext(Drop_tableContext.class,0);
		}
		public Drop_viewContext drop_view() {
			return getRuleContext(Drop_viewContext.class,0);
		}
		public Ddl_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ddl_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDdl_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDdl_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDdl_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Ddl_clauseContext ddl_clause() throws RecognitionException {
		Ddl_clauseContext _localctx = new Ddl_clauseContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_ddl_clause);
		try {
			setState(298);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,3,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(286);
				create_index();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(287);
				create_procedure();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(288);
				create_statistics();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(289);
				create_table();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(290);
				create_view();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(291);
				alter_table();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(292);
				alter_database();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(293);
				drop_index();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(294);
				drop_procedure();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(295);
				drop_statistics();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(296);
				drop_table();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(297);
				drop_view();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Cfl_statementContext extends ParserRuleContext {
		public Cfl_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_cfl_statement; }
	 
		public Cfl_statementContext() { }
		public void copyFrom(Cfl_statementContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class Waitfor_statementContext extends Cfl_statementContext {
		public TerminalNode WAITFOR() { return getToken(tsqlParser.WAITFOR, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode DELAY() { return getToken(tsqlParser.DELAY, 0); }
		public TerminalNode TIME() { return getToken(tsqlParser.TIME, 0); }
		public Waitfor_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWaitfor_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWaitfor_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWaitfor_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Print_statementContext extends Cfl_statementContext {
		public TerminalNode PRINT() { return getToken(tsqlParser.PRINT, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Print_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterPrint_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitPrint_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitPrint_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Continue_statementContext extends Cfl_statementContext {
		public TerminalNode CONTINUE() { return getToken(tsqlParser.CONTINUE, 0); }
		public Continue_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterContinue_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitContinue_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitContinue_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Begin_statementContext extends Cfl_statementContext {
		public TerminalNode BEGIN() { return getToken(tsqlParser.BEGIN, 0); }
		public TerminalNode END() { return getToken(tsqlParser.END, 0); }
		public List<Sql_clauseContext> sql_clause() {
			return getRuleContexts(Sql_clauseContext.class);
		}
		public Sql_clauseContext sql_clause(int i) {
			return getRuleContext(Sql_clauseContext.class,i);
		}
		public Begin_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterBegin_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitBegin_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitBegin_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class If_statementContext extends Cfl_statementContext {
		public TerminalNode IF() { return getToken(tsqlParser.IF, 0); }
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public List<Sql_clauseContext> sql_clause() {
			return getRuleContexts(Sql_clauseContext.class);
		}
		public Sql_clauseContext sql_clause(int i) {
			return getRuleContext(Sql_clauseContext.class,i);
		}
		public TerminalNode ELSE() { return getToken(tsqlParser.ELSE, 0); }
		public If_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterIf_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitIf_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitIf_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Raiseerror_statementContext extends Cfl_statementContext {
		public Token msg;
		public TerminalNode RAISERROR() { return getToken(tsqlParser.RAISERROR, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public List<TerminalNode> LOCAL_ID() { return getTokens(tsqlParser.LOCAL_ID); }
		public TerminalNode LOCAL_ID(int i) {
			return getToken(tsqlParser.LOCAL_ID, i);
		}
		public List<NumberContext> number() {
			return getRuleContexts(NumberContext.class);
		}
		public NumberContext number(int i) {
			return getRuleContext(NumberContext.class,i);
		}
		public List<ConstantContext> constant() {
			return getRuleContexts(ConstantContext.class);
		}
		public ConstantContext constant(int i) {
			return getRuleContext(ConstantContext.class,i);
		}
		public Raiseerror_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterRaiseerror_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitRaiseerror_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitRaiseerror_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Throw_statementContext extends Cfl_statementContext {
		public TerminalNode THROW() { return getToken(tsqlParser.THROW, 0); }
		public List<TerminalNode> DECIMAL() { return getTokens(tsqlParser.DECIMAL); }
		public TerminalNode DECIMAL(int i) {
			return getToken(tsqlParser.DECIMAL, i);
		}
		public List<TerminalNode> LOCAL_ID() { return getTokens(tsqlParser.LOCAL_ID); }
		public TerminalNode LOCAL_ID(int i) {
			return getToken(tsqlParser.LOCAL_ID, i);
		}
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public Throw_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterThrow_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitThrow_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitThrow_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Try_catch_statementContext extends Cfl_statementContext {
		public List<TerminalNode> BEGIN() { return getTokens(tsqlParser.BEGIN); }
		public TerminalNode BEGIN(int i) {
			return getToken(tsqlParser.BEGIN, i);
		}
		public List<TerminalNode> TRY() { return getTokens(tsqlParser.TRY); }
		public TerminalNode TRY(int i) {
			return getToken(tsqlParser.TRY, i);
		}
		public List<TerminalNode> END() { return getTokens(tsqlParser.END); }
		public TerminalNode END(int i) {
			return getToken(tsqlParser.END, i);
		}
		public List<TerminalNode> CATCH() { return getTokens(tsqlParser.CATCH); }
		public TerminalNode CATCH(int i) {
			return getToken(tsqlParser.CATCH, i);
		}
		public List<Sql_clauseContext> sql_clause() {
			return getRuleContexts(Sql_clauseContext.class);
		}
		public Sql_clauseContext sql_clause(int i) {
			return getRuleContext(Sql_clauseContext.class,i);
		}
		public Try_catch_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTry_catch_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTry_catch_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTry_catch_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class While_statementContext extends Cfl_statementContext {
		public TerminalNode WHILE() { return getToken(tsqlParser.WHILE, 0); }
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public Sql_clauseContext sql_clause() {
			return getRuleContext(Sql_clauseContext.class,0);
		}
		public TerminalNode BREAK() { return getToken(tsqlParser.BREAK, 0); }
		public TerminalNode CONTINUE() { return getToken(tsqlParser.CONTINUE, 0); }
		public While_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWhile_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWhile_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWhile_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Break_statementContext extends Cfl_statementContext {
		public TerminalNode BREAK() { return getToken(tsqlParser.BREAK, 0); }
		public Break_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterBreak_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitBreak_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitBreak_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Goto_statementContext extends Cfl_statementContext {
		public TerminalNode GOTO() { return getToken(tsqlParser.GOTO, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Goto_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterGoto_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitGoto_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitGoto_statement(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Return_statementContext extends Cfl_statementContext {
		public TerminalNode RETURN() { return getToken(tsqlParser.RETURN, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Return_statementContext(Cfl_statementContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterReturn_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitReturn_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitReturn_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Cfl_statementContext cfl_statement() throws RecognitionException {
		Cfl_statementContext _localctx = new Cfl_statementContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_cfl_statement);
		int _la;
		try {
			setState(443);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,33,_ctx) ) {
			case 1:
				_localctx = new Begin_statementContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(300);
				match(BEGIN);
				setState(302);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMI) {
					{
					setState(301);
					match(SEMI);
					}
				}

				setState(307);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ALTER) | (1L << BEGIN) | (1L << BREAK) | (1L << CLOSE) | (1L << COMMIT) | (1L << CONTINUE) | (1L << CREATE) | (1L << DEALLOCATE) | (1L << DECLARE) | (1L << DELETE) | (1L << DROP) | (1L << EXEC) | (1L << EXECUTE))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (FETCH - 66)) | (1L << (FORCESEEK - 66)) | (1L << (GOTO - 66)) | (1L << (IF - 66)) | (1L << (INSERT - 66)) | (1L << (OPEN - 66)) | (1L << (PRINT - 66)) | (1L << (RAISERROR - 66)))) != 0) || ((((_la - 137)) & ~0x3f) == 0 && ((1L << (_la - 137)) & ((1L << (RETURN - 137)) | (1L << (REVERT - 137)) | (1L << (ROLLBACK - 137)) | (1L << (SAVE - 137)) | (1L << (SELECT - 137)) | (1L << (SET - 137)) | (1L << (UPDATE - 137)) | (1L << (USE - 137)) | (1L << (WAITFOR - 137)) | (1L << (WHILE - 137)) | (1L << (WITH - 137)) | (1L << (ABSOLUTE - 137)) | (1L << (APPLY - 137)) | (1L << (AUTO - 137)) | (1L << (AVG - 137)) | (1L << (BASE64 - 137)) | (1L << (CALLER - 137)) | (1L << (CAST - 137)) | (1L << (CATCH - 137)) | (1L << (CHECKSUM_AGG - 137)) | (1L << (COMMITTED - 137)) | (1L << (CONCAT - 137)))) != 0) || ((((_la - 201)) & ~0x3f) == 0 && ((1L << (_la - 201)) & ((1L << (COOKIE - 201)) | (1L << (COUNT - 201)) | (1L << (COUNT_BIG - 201)) | (1L << (DELAY - 201)) | (1L << (DELETED - 201)) | (1L << (DENSE_RANK - 201)) | (1L << (DISABLE - 201)) | (1L << (DYNAMIC - 201)) | (1L << (ENCRYPTION - 201)) | (1L << (EXPAND - 201)) | (1L << (FAST - 201)) | (1L << (FAST_FORWARD - 201)) | (1L << (FIRST - 201)) | (1L << (FORCE - 201)) | (1L << (FORCED - 201)) | (1L << (FOLLOWING - 201)) | (1L << (FORWARD_ONLY - 201)) | (1L << (FULLSCAN - 201)) | (1L << (GLOBAL - 201)) | (1L << (GO - 201)) | (1L << (GROUPING - 201)) | (1L << (GROUPING_ID - 201)) | (1L << (HASH - 201)) | (1L << (INSENSITIVE - 201)) | (1L << (INSERTED - 201)) | (1L << (ISOLATION - 201)) | (1L << (KEEP - 201)) | (1L << (KEEPFIXED - 201)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 201)) | (1L << (KEYSET - 201)) | (1L << (LAST - 201)) | (1L << (LEVEL - 201)) | (1L << (LOCAL - 201)) | (1L << (LOCK_ESCALATION - 201)) | (1L << (LOGIN - 201)) | (1L << (LOOP - 201)) | (1L << (MARK - 201)) | (1L << (MAX - 201)) | (1L << (MAXDOP - 201)) | (1L << (MAXRECURSION - 201)) | (1L << (MIN - 201)) | (1L << (MODIFY - 201)) | (1L << (NEXT - 201)) | (1L << (NAME - 201)) | (1L << (NOCOUNT - 201)) | (1L << (NOEXPAND - 201)) | (1L << (NORECOMPUTE - 201)) | (1L << (NTILE - 201)) | (1L << (NUMBER - 201)) | (1L << (OFFSET - 201)) | (1L << (ONLY - 201)) | (1L << (OPTIMISTIC - 201)) | (1L << (OPTIMIZE - 201)) | (1L << (OUT - 201)) | (1L << (OUTPUT - 201)) | (1L << (OWNER - 201)) | (1L << (PARAMETERIZATION - 201)) | (1L << (PARTITION - 201)) | (1L << (PATH - 201)))) != 0) || ((((_la - 265)) & ~0x3f) == 0 && ((1L << (_la - 265)) & ((1L << (PRECEDING - 265)) | (1L << (PRIOR - 265)) | (1L << (RANGE - 265)) | (1L << (RANK - 265)) | (1L << (READONLY - 265)) | (1L << (READ_ONLY - 265)) | (1L << (RECOMPILE - 265)) | (1L << (RELATIVE - 265)) | (1L << (REMOTE - 265)) | (1L << (REPEATABLE - 265)) | (1L << (ROBUST - 265)) | (1L << (ROOT - 265)) | (1L << (ROW - 265)) | (1L << (ROWGUID - 265)) | (1L << (ROWS - 265)) | (1L << (ROW_NUMBER - 265)) | (1L << (SAMPLE - 265)) | (1L << (SCHEMABINDING - 265)) | (1L << (SCROLL - 265)) | (1L << (SCROLL_LOCKS - 265)) | (1L << (SELF - 265)) | (1L << (SERIALIZABLE - 265)) | (1L << (SIMPLE - 265)) | (1L << (SNAPSHOT - 265)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 265)) | (1L << (STATIC - 265)) | (1L << (STATS_STREAM - 265)) | (1L << (STDEV - 265)) | (1L << (STDEVP - 265)) | (1L << (SUM - 265)) | (1L << (THROW - 265)) | (1L << (TIES - 265)) | (1L << (TIME - 265)) | (1L << (TRY - 265)) | (1L << (TYPE - 265)) | (1L << (TYPE_WARNING - 265)) | (1L << (UNBOUNDED - 265)) | (1L << (UNCOMMITTED - 265)) | (1L << (UNKNOWN - 265)) | (1L << (USING - 265)) | (1L << (VAR - 265)) | (1L << (VARP - 265)) | (1L << (VIEW_METADATA - 265)) | (1L << (VIEWS - 265)) | (1L << (WORK - 265)) | (1L << (XML - 265)) | (1L << (XMLNAMESPACES - 265)) | (1L << (DOUBLE_QUOTE_ID - 265)) | (1L << (SQUARE_BRACKET_ID - 265)) | (1L << (ID - 265)))) != 0) || _la==LR_BRACKET) {
					{
					{
					setState(304);
					sql_clause();
					}
					}
					setState(309);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(310);
				match(END);
				setState(312);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
				case 1:
					{
					setState(311);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				_localctx = new Break_statementContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(314);
				match(BREAK);
				setState(316);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
				case 1:
					{
					setState(315);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 3:
				_localctx = new Continue_statementContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(318);
				match(CONTINUE);
				setState(320);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
				case 1:
					{
					setState(319);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 4:
				_localctx = new Goto_statementContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(322);
				match(GOTO);
				setState(323);
				id();
				setState(325);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
				case 1:
					{
					setState(324);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 5:
				_localctx = new Goto_statementContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(327);
				id();
				setState(328);
				match(COLON);
				setState(330);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,10,_ctx) ) {
				case 1:
					{
					setState(329);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 6:
				_localctx = new If_statementContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(332);
				match(IF);
				setState(333);
				search_condition();
				setState(334);
				sql_clause();
				setState(337);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
				case 1:
					{
					setState(335);
					match(ELSE);
					setState(336);
					sql_clause();
					}
					break;
				}
				setState(340);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
				case 1:
					{
					setState(339);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 7:
				_localctx = new Return_statementContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(342);
				match(RETURN);
				setState(344);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
				case 1:
					{
					setState(343);
					expression(0);
					}
					break;
				}
				setState(347);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,14,_ctx) ) {
				case 1:
					{
					setState(346);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 8:
				_localctx = new Throw_statementContext(_localctx);
				enterOuterAlt(_localctx, 8);
				{
				setState(349);
				match(THROW);
				setState(355);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LOCAL_ID || _la==DECIMAL) {
					{
					setState(350);
					_la = _input.LA(1);
					if ( !(_la==LOCAL_ID || _la==DECIMAL) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(351);
					match(COMMA);
					setState(352);
					_la = _input.LA(1);
					if ( !(_la==LOCAL_ID || _la==STRING) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(353);
					match(COMMA);
					setState(354);
					_la = _input.LA(1);
					if ( !(_la==LOCAL_ID || _la==DECIMAL) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
				}

				setState(358);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,16,_ctx) ) {
				case 1:
					{
					setState(357);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 9:
				_localctx = new Try_catch_statementContext(_localctx);
				enterOuterAlt(_localctx, 9);
				{
				setState(360);
				match(BEGIN);
				setState(361);
				match(TRY);
				setState(363);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMI) {
					{
					setState(362);
					match(SEMI);
					}
				}

				setState(368);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ALTER) | (1L << BEGIN) | (1L << BREAK) | (1L << CLOSE) | (1L << COMMIT) | (1L << CONTINUE) | (1L << CREATE) | (1L << DEALLOCATE) | (1L << DECLARE) | (1L << DELETE) | (1L << DROP) | (1L << EXEC) | (1L << EXECUTE))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (FETCH - 66)) | (1L << (FORCESEEK - 66)) | (1L << (GOTO - 66)) | (1L << (IF - 66)) | (1L << (INSERT - 66)) | (1L << (OPEN - 66)) | (1L << (PRINT - 66)) | (1L << (RAISERROR - 66)))) != 0) || ((((_la - 137)) & ~0x3f) == 0 && ((1L << (_la - 137)) & ((1L << (RETURN - 137)) | (1L << (REVERT - 137)) | (1L << (ROLLBACK - 137)) | (1L << (SAVE - 137)) | (1L << (SELECT - 137)) | (1L << (SET - 137)) | (1L << (UPDATE - 137)) | (1L << (USE - 137)) | (1L << (WAITFOR - 137)) | (1L << (WHILE - 137)) | (1L << (WITH - 137)) | (1L << (ABSOLUTE - 137)) | (1L << (APPLY - 137)) | (1L << (AUTO - 137)) | (1L << (AVG - 137)) | (1L << (BASE64 - 137)) | (1L << (CALLER - 137)) | (1L << (CAST - 137)) | (1L << (CATCH - 137)) | (1L << (CHECKSUM_AGG - 137)) | (1L << (COMMITTED - 137)) | (1L << (CONCAT - 137)))) != 0) || ((((_la - 201)) & ~0x3f) == 0 && ((1L << (_la - 201)) & ((1L << (COOKIE - 201)) | (1L << (COUNT - 201)) | (1L << (COUNT_BIG - 201)) | (1L << (DELAY - 201)) | (1L << (DELETED - 201)) | (1L << (DENSE_RANK - 201)) | (1L << (DISABLE - 201)) | (1L << (DYNAMIC - 201)) | (1L << (ENCRYPTION - 201)) | (1L << (EXPAND - 201)) | (1L << (FAST - 201)) | (1L << (FAST_FORWARD - 201)) | (1L << (FIRST - 201)) | (1L << (FORCE - 201)) | (1L << (FORCED - 201)) | (1L << (FOLLOWING - 201)) | (1L << (FORWARD_ONLY - 201)) | (1L << (FULLSCAN - 201)) | (1L << (GLOBAL - 201)) | (1L << (GO - 201)) | (1L << (GROUPING - 201)) | (1L << (GROUPING_ID - 201)) | (1L << (HASH - 201)) | (1L << (INSENSITIVE - 201)) | (1L << (INSERTED - 201)) | (1L << (ISOLATION - 201)) | (1L << (KEEP - 201)) | (1L << (KEEPFIXED - 201)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 201)) | (1L << (KEYSET - 201)) | (1L << (LAST - 201)) | (1L << (LEVEL - 201)) | (1L << (LOCAL - 201)) | (1L << (LOCK_ESCALATION - 201)) | (1L << (LOGIN - 201)) | (1L << (LOOP - 201)) | (1L << (MARK - 201)) | (1L << (MAX - 201)) | (1L << (MAXDOP - 201)) | (1L << (MAXRECURSION - 201)) | (1L << (MIN - 201)) | (1L << (MODIFY - 201)) | (1L << (NEXT - 201)) | (1L << (NAME - 201)) | (1L << (NOCOUNT - 201)) | (1L << (NOEXPAND - 201)) | (1L << (NORECOMPUTE - 201)) | (1L << (NTILE - 201)) | (1L << (NUMBER - 201)) | (1L << (OFFSET - 201)) | (1L << (ONLY - 201)) | (1L << (OPTIMISTIC - 201)) | (1L << (OPTIMIZE - 201)) | (1L << (OUT - 201)) | (1L << (OUTPUT - 201)) | (1L << (OWNER - 201)) | (1L << (PARAMETERIZATION - 201)) | (1L << (PARTITION - 201)) | (1L << (PATH - 201)))) != 0) || ((((_la - 265)) & ~0x3f) == 0 && ((1L << (_la - 265)) & ((1L << (PRECEDING - 265)) | (1L << (PRIOR - 265)) | (1L << (RANGE - 265)) | (1L << (RANK - 265)) | (1L << (READONLY - 265)) | (1L << (READ_ONLY - 265)) | (1L << (RECOMPILE - 265)) | (1L << (RELATIVE - 265)) | (1L << (REMOTE - 265)) | (1L << (REPEATABLE - 265)) | (1L << (ROBUST - 265)) | (1L << (ROOT - 265)) | (1L << (ROW - 265)) | (1L << (ROWGUID - 265)) | (1L << (ROWS - 265)) | (1L << (ROW_NUMBER - 265)) | (1L << (SAMPLE - 265)) | (1L << (SCHEMABINDING - 265)) | (1L << (SCROLL - 265)) | (1L << (SCROLL_LOCKS - 265)) | (1L << (SELF - 265)) | (1L << (SERIALIZABLE - 265)) | (1L << (SIMPLE - 265)) | (1L << (SNAPSHOT - 265)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 265)) | (1L << (STATIC - 265)) | (1L << (STATS_STREAM - 265)) | (1L << (STDEV - 265)) | (1L << (STDEVP - 265)) | (1L << (SUM - 265)) | (1L << (THROW - 265)) | (1L << (TIES - 265)) | (1L << (TIME - 265)) | (1L << (TRY - 265)) | (1L << (TYPE - 265)) | (1L << (TYPE_WARNING - 265)) | (1L << (UNBOUNDED - 265)) | (1L << (UNCOMMITTED - 265)) | (1L << (UNKNOWN - 265)) | (1L << (USING - 265)) | (1L << (VAR - 265)) | (1L << (VARP - 265)) | (1L << (VIEW_METADATA - 265)) | (1L << (VIEWS - 265)) | (1L << (WORK - 265)) | (1L << (XML - 265)) | (1L << (XMLNAMESPACES - 265)) | (1L << (DOUBLE_QUOTE_ID - 265)) | (1L << (SQUARE_BRACKET_ID - 265)) | (1L << (ID - 265)))) != 0) || _la==LR_BRACKET) {
					{
					{
					setState(365);
					sql_clause();
					}
					}
					setState(370);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(371);
				match(END);
				setState(372);
				match(TRY);
				setState(374);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMI) {
					{
					setState(373);
					match(SEMI);
					}
				}

				setState(376);
				match(BEGIN);
				setState(377);
				match(CATCH);
				setState(379);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SEMI) {
					{
					setState(378);
					match(SEMI);
					}
				}

				setState(384);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ALTER) | (1L << BEGIN) | (1L << BREAK) | (1L << CLOSE) | (1L << COMMIT) | (1L << CONTINUE) | (1L << CREATE) | (1L << DEALLOCATE) | (1L << DECLARE) | (1L << DELETE) | (1L << DROP) | (1L << EXEC) | (1L << EXECUTE))) != 0) || ((((_la - 66)) & ~0x3f) == 0 && ((1L << (_la - 66)) & ((1L << (FETCH - 66)) | (1L << (FORCESEEK - 66)) | (1L << (GOTO - 66)) | (1L << (IF - 66)) | (1L << (INSERT - 66)) | (1L << (OPEN - 66)) | (1L << (PRINT - 66)) | (1L << (RAISERROR - 66)))) != 0) || ((((_la - 137)) & ~0x3f) == 0 && ((1L << (_la - 137)) & ((1L << (RETURN - 137)) | (1L << (REVERT - 137)) | (1L << (ROLLBACK - 137)) | (1L << (SAVE - 137)) | (1L << (SELECT - 137)) | (1L << (SET - 137)) | (1L << (UPDATE - 137)) | (1L << (USE - 137)) | (1L << (WAITFOR - 137)) | (1L << (WHILE - 137)) | (1L << (WITH - 137)) | (1L << (ABSOLUTE - 137)) | (1L << (APPLY - 137)) | (1L << (AUTO - 137)) | (1L << (AVG - 137)) | (1L << (BASE64 - 137)) | (1L << (CALLER - 137)) | (1L << (CAST - 137)) | (1L << (CATCH - 137)) | (1L << (CHECKSUM_AGG - 137)) | (1L << (COMMITTED - 137)) | (1L << (CONCAT - 137)))) != 0) || ((((_la - 201)) & ~0x3f) == 0 && ((1L << (_la - 201)) & ((1L << (COOKIE - 201)) | (1L << (COUNT - 201)) | (1L << (COUNT_BIG - 201)) | (1L << (DELAY - 201)) | (1L << (DELETED - 201)) | (1L << (DENSE_RANK - 201)) | (1L << (DISABLE - 201)) | (1L << (DYNAMIC - 201)) | (1L << (ENCRYPTION - 201)) | (1L << (EXPAND - 201)) | (1L << (FAST - 201)) | (1L << (FAST_FORWARD - 201)) | (1L << (FIRST - 201)) | (1L << (FORCE - 201)) | (1L << (FORCED - 201)) | (1L << (FOLLOWING - 201)) | (1L << (FORWARD_ONLY - 201)) | (1L << (FULLSCAN - 201)) | (1L << (GLOBAL - 201)) | (1L << (GO - 201)) | (1L << (GROUPING - 201)) | (1L << (GROUPING_ID - 201)) | (1L << (HASH - 201)) | (1L << (INSENSITIVE - 201)) | (1L << (INSERTED - 201)) | (1L << (ISOLATION - 201)) | (1L << (KEEP - 201)) | (1L << (KEEPFIXED - 201)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 201)) | (1L << (KEYSET - 201)) | (1L << (LAST - 201)) | (1L << (LEVEL - 201)) | (1L << (LOCAL - 201)) | (1L << (LOCK_ESCALATION - 201)) | (1L << (LOGIN - 201)) | (1L << (LOOP - 201)) | (1L << (MARK - 201)) | (1L << (MAX - 201)) | (1L << (MAXDOP - 201)) | (1L << (MAXRECURSION - 201)) | (1L << (MIN - 201)) | (1L << (MODIFY - 201)) | (1L << (NEXT - 201)) | (1L << (NAME - 201)) | (1L << (NOCOUNT - 201)) | (1L << (NOEXPAND - 201)) | (1L << (NORECOMPUTE - 201)) | (1L << (NTILE - 201)) | (1L << (NUMBER - 201)) | (1L << (OFFSET - 201)) | (1L << (ONLY - 201)) | (1L << (OPTIMISTIC - 201)) | (1L << (OPTIMIZE - 201)) | (1L << (OUT - 201)) | (1L << (OUTPUT - 201)) | (1L << (OWNER - 201)) | (1L << (PARAMETERIZATION - 201)) | (1L << (PARTITION - 201)) | (1L << (PATH - 201)))) != 0) || ((((_la - 265)) & ~0x3f) == 0 && ((1L << (_la - 265)) & ((1L << (PRECEDING - 265)) | (1L << (PRIOR - 265)) | (1L << (RANGE - 265)) | (1L << (RANK - 265)) | (1L << (READONLY - 265)) | (1L << (READ_ONLY - 265)) | (1L << (RECOMPILE - 265)) | (1L << (RELATIVE - 265)) | (1L << (REMOTE - 265)) | (1L << (REPEATABLE - 265)) | (1L << (ROBUST - 265)) | (1L << (ROOT - 265)) | (1L << (ROW - 265)) | (1L << (ROWGUID - 265)) | (1L << (ROWS - 265)) | (1L << (ROW_NUMBER - 265)) | (1L << (SAMPLE - 265)) | (1L << (SCHEMABINDING - 265)) | (1L << (SCROLL - 265)) | (1L << (SCROLL_LOCKS - 265)) | (1L << (SELF - 265)) | (1L << (SERIALIZABLE - 265)) | (1L << (SIMPLE - 265)) | (1L << (SNAPSHOT - 265)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 265)) | (1L << (STATIC - 265)) | (1L << (STATS_STREAM - 265)) | (1L << (STDEV - 265)) | (1L << (STDEVP - 265)) | (1L << (SUM - 265)) | (1L << (THROW - 265)) | (1L << (TIES - 265)) | (1L << (TIME - 265)) | (1L << (TRY - 265)) | (1L << (TYPE - 265)) | (1L << (TYPE_WARNING - 265)) | (1L << (UNBOUNDED - 265)) | (1L << (UNCOMMITTED - 265)) | (1L << (UNKNOWN - 265)) | (1L << (USING - 265)) | (1L << (VAR - 265)) | (1L << (VARP - 265)) | (1L << (VIEW_METADATA - 265)) | (1L << (VIEWS - 265)) | (1L << (WORK - 265)) | (1L << (XML - 265)) | (1L << (XMLNAMESPACES - 265)) | (1L << (DOUBLE_QUOTE_ID - 265)) | (1L << (SQUARE_BRACKET_ID - 265)) | (1L << (ID - 265)))) != 0) || _la==LR_BRACKET) {
					{
					{
					setState(381);
					sql_clause();
					}
					}
					setState(386);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(387);
				match(END);
				setState(388);
				match(CATCH);
				setState(390);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,22,_ctx) ) {
				case 1:
					{
					setState(389);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 10:
				_localctx = new Waitfor_statementContext(_localctx);
				enterOuterAlt(_localctx, 10);
				{
				setState(392);
				match(WAITFOR);
				setState(393);
				_la = _input.LA(1);
				if ( !(_la==DELAY || _la==TIME) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(394);
				expression(0);
				setState(396);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,23,_ctx) ) {
				case 1:
					{
					setState(395);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 11:
				_localctx = new While_statementContext(_localctx);
				enterOuterAlt(_localctx, 11);
				{
				setState(398);
				match(WHILE);
				setState(399);
				search_condition();
				setState(409);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,26,_ctx) ) {
				case 1:
					{
					setState(400);
					sql_clause();
					}
					break;
				case 2:
					{
					setState(401);
					match(BREAK);
					setState(403);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,24,_ctx) ) {
					case 1:
						{
						setState(402);
						match(SEMI);
						}
						break;
					}
					}
					break;
				case 3:
					{
					setState(405);
					match(CONTINUE);
					setState(407);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,25,_ctx) ) {
					case 1:
						{
						setState(406);
						match(SEMI);
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case 12:
				_localctx = new Print_statementContext(_localctx);
				enterOuterAlt(_localctx, 12);
				{
				setState(411);
				match(PRINT);
				setState(412);
				expression(0);
				setState(414);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,27,_ctx) ) {
				case 1:
					{
					setState(413);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 13:
				_localctx = new Raiseerror_statementContext(_localctx);
				enterOuterAlt(_localctx, 13);
				{
				setState(416);
				match(RAISERROR);
				setState(417);
				match(LR_BRACKET);
				setState(418);
				((Raiseerror_statementContext)_localctx).msg = _input.LT(1);
				_la = _input.LA(1);
				if ( !(((((_la - 318)) & ~0x3f) == 0 && ((1L << (_la - 318)) & ((1L << (LOCAL_ID - 318)) | (1L << (DECIMAL - 318)) | (1L << (STRING - 318)))) != 0)) ) {
					((Raiseerror_statementContext)_localctx).msg = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(419);
				match(COMMA);
				setState(422);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case DECIMAL:
				case PLUS:
				case MINUS:
					{
					setState(420);
					number();
					}
					break;
				case LOCAL_ID:
					{
					setState(421);
					match(LOCAL_ID);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(424);
				match(COMMA);
				setState(427);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case DECIMAL:
				case PLUS:
				case MINUS:
					{
					setState(425);
					number();
					}
					break;
				case LOCAL_ID:
					{
					setState(426);
					match(LOCAL_ID);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(436);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(429);
					match(COMMA);
					setState(432);
					_errHandler.sync(this);
					switch (_input.LA(1)) {
					case DECIMAL:
					case STRING:
					case BINARY:
					case FLOAT:
					case REAL:
					case DOLLAR:
					case PLUS:
					case MINUS:
						{
						setState(430);
						constant();
						}
						break;
					case LOCAL_ID:
						{
						setState(431);
						match(LOCAL_ID);
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					}
					}
					setState(438);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(439);
				match(RR_BRACKET);
				setState(441);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,32,_ctx) ) {
				case 1:
					{
					setState(440);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Another_statementContext extends ParserRuleContext {
		public Declare_statementContext declare_statement() {
			return getRuleContext(Declare_statementContext.class,0);
		}
		public Cursor_statementContext cursor_statement() {
			return getRuleContext(Cursor_statementContext.class,0);
		}
		public Execute_statementContext execute_statement() {
			return getRuleContext(Execute_statementContext.class,0);
		}
		public Security_statementContext security_statement() {
			return getRuleContext(Security_statementContext.class,0);
		}
		public Set_statmentContext set_statment() {
			return getRuleContext(Set_statmentContext.class,0);
		}
		public Transaction_statementContext transaction_statement() {
			return getRuleContext(Transaction_statementContext.class,0);
		}
		public Go_statementContext go_statement() {
			return getRuleContext(Go_statementContext.class,0);
		}
		public Use_statementContext use_statement() {
			return getRuleContext(Use_statementContext.class,0);
		}
		public Another_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_another_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAnother_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAnother_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAnother_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Another_statementContext another_statement() throws RecognitionException {
		Another_statementContext _localctx = new Another_statementContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_another_statement);
		try {
			setState(453);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,34,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(445);
				declare_statement();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(446);
				cursor_statement();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(447);
				execute_statement();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(448);
				security_statement();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(449);
				set_statment();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(450);
				transaction_statement();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(451);
				go_statement();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(452);
				use_statement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Delete_statementContext extends ParserRuleContext {
		public Token cursor_var;
		public TerminalNode DELETE() { return getToken(tsqlParser.DELETE, 0); }
		public Table_aliasContext table_alias() {
			return getRuleContext(Table_aliasContext.class,0);
		}
		public Ddl_objectContext ddl_object() {
			return getRuleContext(Ddl_objectContext.class,0);
		}
		public Rowset_function_limitedContext rowset_function_limited() {
			return getRuleContext(Rowset_function_limitedContext.class,0);
		}
		public With_expressionContext with_expression() {
			return getRuleContext(With_expressionContext.class,0);
		}
		public TerminalNode TOP() { return getToken(tsqlParser.TOP, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public List<TerminalNode> FROM() { return getTokens(tsqlParser.FROM); }
		public TerminalNode FROM(int i) {
			return getToken(tsqlParser.FROM, i);
		}
		public With_table_hintsContext with_table_hints() {
			return getRuleContext(With_table_hintsContext.class,0);
		}
		public Output_clauseContext output_clause() {
			return getRuleContext(Output_clauseContext.class,0);
		}
		public List<Table_sourceContext> table_source() {
			return getRuleContexts(Table_sourceContext.class);
		}
		public Table_sourceContext table_source(int i) {
			return getRuleContext(Table_sourceContext.class,i);
		}
		public TerminalNode WHERE() { return getToken(tsqlParser.WHERE, 0); }
		public For_clauseContext for_clause() {
			return getRuleContext(For_clauseContext.class,0);
		}
		public Option_clauseContext option_clause() {
			return getRuleContext(Option_clauseContext.class,0);
		}
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public TerminalNode CURRENT() { return getToken(tsqlParser.CURRENT, 0); }
		public TerminalNode OF() { return getToken(tsqlParser.OF, 0); }
		public TerminalNode PERCENT() { return getToken(tsqlParser.PERCENT, 0); }
		public Cursor_nameContext cursor_name() {
			return getRuleContext(Cursor_nameContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public Delete_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_delete_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDelete_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDelete_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDelete_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Delete_statementContext delete_statement() throws RecognitionException {
		Delete_statementContext _localctx = new Delete_statementContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_delete_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(456);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(455);
				with_expression();
				}
			}

			setState(458);
			match(DELETE);
			setState(466);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TOP) {
				{
				setState(459);
				match(TOP);
				setState(460);
				match(LR_BRACKET);
				setState(461);
				expression(0);
				setState(462);
				match(RR_BRACKET);
				setState(464);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PERCENT) {
					{
					setState(463);
					match(PERCENT);
					}
				}

				}
			}

			setState(469);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FROM) {
				{
				setState(468);
				match(FROM);
				}
			}

			setState(474);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
			case 1:
				{
				setState(471);
				table_alias();
				}
				break;
			case 2:
				{
				setState(472);
				ddl_object();
				}
				break;
			case 3:
				{
				setState(473);
				rowset_function_limited();
				}
				break;
			}
			setState(477);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,40,_ctx) ) {
			case 1:
				{
				setState(476);
				with_table_hints();
				}
				break;
			}
			setState(480);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,41,_ctx) ) {
			case 1:
				{
				setState(479);
				output_clause();
				}
				break;
			}
			setState(491);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FROM) {
				{
				setState(482);
				match(FROM);
				setState(483);
				table_source();
				setState(488);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(484);
					match(COMMA);
					setState(485);
					table_source();
					}
					}
					setState(490);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(506);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WHERE) {
				{
				setState(493);
				match(WHERE);
				setState(504);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case CASE:
				case COALESCE:
				case CONVERT:
				case CURRENT_TIMESTAMP:
				case CURRENT_USER:
				case DEFAULT:
				case EXISTS:
				case FORCESEEK:
				case IDENTITY:
				case LEFT:
				case NOT:
				case NULL:
				case NULLIF:
				case RIGHT:
				case SESSION_USER:
				case SYSTEM_USER:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case BINARY_CHECKSUM:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DATEADD:
				case DATEDIFF:
				case DATENAME:
				case DATEPART:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MIN_ACTIVE_ROWVERSION:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case LOCAL_ID:
				case DECIMAL:
				case ID:
				case STRING:
				case BINARY:
				case FLOAT:
				case REAL:
				case DOLLAR:
				case LR_BRACKET:
				case PLUS:
				case MINUS:
				case BIT_NOT:
					{
					setState(494);
					search_condition();
					}
					break;
				case CURRENT:
					{
					setState(495);
					match(CURRENT);
					setState(496);
					match(OF);
					setState(502);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,45,_ctx) ) {
					case 1:
						{
						setState(498);
						_errHandler.sync(this);
						switch ( getInterpreter().adaptivePredict(_input,44,_ctx) ) {
						case 1:
							{
							setState(497);
							match(GLOBAL);
							}
							break;
						}
						setState(500);
						cursor_name();
						}
						break;
					case 2:
						{
						setState(501);
						((Delete_statementContext)_localctx).cursor_var = match(LOCAL_ID);
						}
						break;
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
			}

			setState(509);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FOR) {
				{
				setState(508);
				for_clause();
				}
			}

			setState(512);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==OPTION) {
				{
				setState(511);
				option_clause();
				}
			}

			setState(515);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,50,_ctx) ) {
			case 1:
				{
				setState(514);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Insert_statementContext extends ParserRuleContext {
		public TerminalNode INSERT() { return getToken(tsqlParser.INSERT, 0); }
		public Ddl_objectContext ddl_object() {
			return getRuleContext(Ddl_objectContext.class,0);
		}
		public Rowset_function_limitedContext rowset_function_limited() {
			return getRuleContext(Rowset_function_limitedContext.class,0);
		}
		public TerminalNode VALUES() { return getToken(tsqlParser.VALUES, 0); }
		public List<Expression_listContext> expression_list() {
			return getRuleContexts(Expression_listContext.class);
		}
		public Expression_listContext expression_list(int i) {
			return getRuleContext(Expression_listContext.class,i);
		}
		public Derived_tableContext derived_table() {
			return getRuleContext(Derived_tableContext.class,0);
		}
		public Execute_statementContext execute_statement() {
			return getRuleContext(Execute_statementContext.class,0);
		}
		public TerminalNode DEFAULT() { return getToken(tsqlParser.DEFAULT, 0); }
		public With_expressionContext with_expression() {
			return getRuleContext(With_expressionContext.class,0);
		}
		public TerminalNode TOP() { return getToken(tsqlParser.TOP, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode INTO() { return getToken(tsqlParser.INTO, 0); }
		public Insert_with_table_hintsContext insert_with_table_hints() {
			return getRuleContext(Insert_with_table_hintsContext.class,0);
		}
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public Output_clauseContext output_clause() {
			return getRuleContext(Output_clauseContext.class,0);
		}
		public For_clauseContext for_clause() {
			return getRuleContext(For_clauseContext.class,0);
		}
		public Option_clauseContext option_clause() {
			return getRuleContext(Option_clauseContext.class,0);
		}
		public TerminalNode PERCENT() { return getToken(tsqlParser.PERCENT, 0); }
		public Insert_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_insert_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterInsert_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitInsert_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitInsert_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Insert_statementContext insert_statement() throws RecognitionException {
		Insert_statementContext _localctx = new Insert_statementContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_insert_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(518);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(517);
				with_expression();
				}
			}

			setState(520);
			match(INSERT);
			setState(528);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TOP) {
				{
				setState(521);
				match(TOP);
				setState(522);
				match(LR_BRACKET);
				setState(523);
				expression(0);
				setState(524);
				match(RR_BRACKET);
				setState(526);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PERCENT) {
					{
					setState(525);
					match(PERCENT);
					}
				}

				}
			}

			setState(531);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==INTO) {
				{
				setState(530);
				match(INTO);
				}
			}

			setState(535);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case LOCAL_ID:
			case ID:
				{
				setState(533);
				ddl_object();
				}
				break;
			case OPENDATASOURCE:
			case OPENQUERY:
				{
				setState(534);
				rowset_function_limited();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(538);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,56,_ctx) ) {
			case 1:
				{
				setState(537);
				insert_with_table_hints();
				}
				break;
			}
			setState(544);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,57,_ctx) ) {
			case 1:
				{
				setState(540);
				match(LR_BRACKET);
				setState(541);
				column_name_list();
				setState(542);
				match(RR_BRACKET);
				}
				break;
			}
			setState(547);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==OUTPUT) {
				{
				setState(546);
				output_clause();
				}
			}

			setState(567);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case VALUES:
				{
				setState(549);
				match(VALUES);
				setState(550);
				match(LR_BRACKET);
				setState(551);
				expression_list();
				setState(552);
				match(RR_BRACKET);
				setState(560);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(553);
					match(COMMA);
					setState(554);
					match(LR_BRACKET);
					setState(555);
					expression_list();
					setState(556);
					match(RR_BRACKET);
					}
					}
					setState(562);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case SELECT:
			case WITH:
			case LR_BRACKET:
				{
				setState(563);
				derived_table();
				}
				break;
			case EXEC:
			case EXECUTE:
				{
				setState(564);
				execute_statement();
				}
				break;
			case DEFAULT:
				{
				setState(565);
				match(DEFAULT);
				setState(566);
				match(VALUES);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(570);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FOR) {
				{
				setState(569);
				for_clause();
				}
			}

			setState(573);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==OPTION) {
				{
				setState(572);
				option_clause();
				}
			}

			setState(576);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,63,_ctx) ) {
			case 1:
				{
				setState(575);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Select_statementContext extends ParserRuleContext {
		public Query_expressionContext query_expression() {
			return getRuleContext(Query_expressionContext.class,0);
		}
		public With_expressionContext with_expression() {
			return getRuleContext(With_expressionContext.class,0);
		}
		public Order_by_clauseContext order_by_clause() {
			return getRuleContext(Order_by_clauseContext.class,0);
		}
		public For_clauseContext for_clause() {
			return getRuleContext(For_clauseContext.class,0);
		}
		public Option_clauseContext option_clause() {
			return getRuleContext(Option_clauseContext.class,0);
		}
		public Select_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_select_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSelect_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSelect_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSelect_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Select_statementContext select_statement() throws RecognitionException {
		Select_statementContext _localctx = new Select_statementContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_select_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(579);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(578);
				with_expression();
				}
			}

			setState(581);
			query_expression();
			setState(583);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,65,_ctx) ) {
			case 1:
				{
				setState(582);
				order_by_clause();
				}
				break;
			}
			setState(586);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,66,_ctx) ) {
			case 1:
				{
				setState(585);
				for_clause();
				}
				break;
			}
			setState(589);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,67,_ctx) ) {
			case 1:
				{
				setState(588);
				option_clause();
				}
				break;
			}
			setState(592);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,68,_ctx) ) {
			case 1:
				{
				setState(591);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Update_statementContext extends ParserRuleContext {
		public Token cursor_var;
		public TerminalNode UPDATE() { return getToken(tsqlParser.UPDATE, 0); }
		public TerminalNode SET() { return getToken(tsqlParser.SET, 0); }
		public List<Update_elemContext> update_elem() {
			return getRuleContexts(Update_elemContext.class);
		}
		public Update_elemContext update_elem(int i) {
			return getRuleContext(Update_elemContext.class,i);
		}
		public Ddl_objectContext ddl_object() {
			return getRuleContext(Ddl_objectContext.class,0);
		}
		public Rowset_function_limitedContext rowset_function_limited() {
			return getRuleContext(Rowset_function_limitedContext.class,0);
		}
		public With_expressionContext with_expression() {
			return getRuleContext(With_expressionContext.class,0);
		}
		public TerminalNode TOP() { return getToken(tsqlParser.TOP, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public With_table_hintsContext with_table_hints() {
			return getRuleContext(With_table_hintsContext.class,0);
		}
		public Output_clauseContext output_clause() {
			return getRuleContext(Output_clauseContext.class,0);
		}
		public TerminalNode FROM() { return getToken(tsqlParser.FROM, 0); }
		public List<Table_sourceContext> table_source() {
			return getRuleContexts(Table_sourceContext.class);
		}
		public Table_sourceContext table_source(int i) {
			return getRuleContext(Table_sourceContext.class,i);
		}
		public TerminalNode WHERE() { return getToken(tsqlParser.WHERE, 0); }
		public For_clauseContext for_clause() {
			return getRuleContext(For_clauseContext.class,0);
		}
		public Option_clauseContext option_clause() {
			return getRuleContext(Option_clauseContext.class,0);
		}
		public Search_condition_listContext search_condition_list() {
			return getRuleContext(Search_condition_listContext.class,0);
		}
		public TerminalNode CURRENT() { return getToken(tsqlParser.CURRENT, 0); }
		public TerminalNode OF() { return getToken(tsqlParser.OF, 0); }
		public TerminalNode PERCENT() { return getToken(tsqlParser.PERCENT, 0); }
		public Cursor_nameContext cursor_name() {
			return getRuleContext(Cursor_nameContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public Update_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_update_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterUpdate_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitUpdate_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitUpdate_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Update_statementContext update_statement() throws RecognitionException {
		Update_statementContext _localctx = new Update_statementContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_update_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(595);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(594);
				with_expression();
				}
			}

			setState(597);
			match(UPDATE);
			setState(605);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TOP) {
				{
				setState(598);
				match(TOP);
				setState(599);
				match(LR_BRACKET);
				setState(600);
				expression(0);
				setState(601);
				match(RR_BRACKET);
				setState(603);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PERCENT) {
					{
					setState(602);
					match(PERCENT);
					}
				}

				}
			}

			setState(609);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case LOCAL_ID:
			case ID:
				{
				setState(607);
				ddl_object();
				}
				break;
			case OPENDATASOURCE:
			case OPENQUERY:
				{
				setState(608);
				rowset_function_limited();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(612);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH || _la==LR_BRACKET) {
				{
				setState(611);
				with_table_hints();
				}
			}

			setState(614);
			match(SET);
			setState(615);
			update_elem();
			setState(620);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(616);
				match(COMMA);
				setState(617);
				update_elem();
				}
				}
				setState(622);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(624);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,75,_ctx) ) {
			case 1:
				{
				setState(623);
				output_clause();
				}
				break;
			}
			setState(635);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FROM) {
				{
				setState(626);
				match(FROM);
				setState(627);
				table_source();
				setState(632);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(628);
					match(COMMA);
					setState(629);
					table_source();
					}
					}
					setState(634);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(650);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WHERE) {
				{
				setState(637);
				match(WHERE);
				setState(648);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case CASE:
				case COALESCE:
				case CONVERT:
				case CURRENT_TIMESTAMP:
				case CURRENT_USER:
				case DEFAULT:
				case EXISTS:
				case FORCESEEK:
				case IDENTITY:
				case LEFT:
				case NOT:
				case NULL:
				case NULLIF:
				case RIGHT:
				case SESSION_USER:
				case SYSTEM_USER:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case BINARY_CHECKSUM:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DATEADD:
				case DATEDIFF:
				case DATENAME:
				case DATEPART:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MIN_ACTIVE_ROWVERSION:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case LOCAL_ID:
				case DECIMAL:
				case ID:
				case STRING:
				case BINARY:
				case FLOAT:
				case REAL:
				case DOLLAR:
				case LR_BRACKET:
				case PLUS:
				case MINUS:
				case BIT_NOT:
					{
					setState(638);
					search_condition_list();
					}
					break;
				case CURRENT:
					{
					setState(639);
					match(CURRENT);
					setState(640);
					match(OF);
					setState(646);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,79,_ctx) ) {
					case 1:
						{
						setState(642);
						_errHandler.sync(this);
						switch ( getInterpreter().adaptivePredict(_input,78,_ctx) ) {
						case 1:
							{
							setState(641);
							match(GLOBAL);
							}
							break;
						}
						setState(644);
						cursor_name();
						}
						break;
					case 2:
						{
						setState(645);
						((Update_statementContext)_localctx).cursor_var = match(LOCAL_ID);
						}
						break;
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
			}

			setState(653);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FOR) {
				{
				setState(652);
				for_clause();
				}
			}

			setState(656);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==OPTION) {
				{
				setState(655);
				option_clause();
				}
			}

			setState(659);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,84,_ctx) ) {
			case 1:
				{
				setState(658);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Output_clauseContext extends ParserRuleContext {
		public TerminalNode OUTPUT() { return getToken(tsqlParser.OUTPUT, 0); }
		public List<Output_dml_list_elemContext> output_dml_list_elem() {
			return getRuleContexts(Output_dml_list_elemContext.class);
		}
		public Output_dml_list_elemContext output_dml_list_elem(int i) {
			return getRuleContext(Output_dml_list_elemContext.class,i);
		}
		public TerminalNode INTO() { return getToken(tsqlParser.INTO, 0); }
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public Output_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_output_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOutput_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOutput_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOutput_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Output_clauseContext output_clause() throws RecognitionException {
		Output_clauseContext _localctx = new Output_clauseContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_output_clause);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(661);
			match(OUTPUT);
			setState(662);
			output_dml_list_elem();
			setState(667);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(663);
				match(COMMA);
				setState(664);
				output_dml_list_elem();
				}
				}
				setState(669);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(681);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==INTO) {
				{
				setState(670);
				match(INTO);
				setState(673);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case LOCAL_ID:
					{
					setState(671);
					match(LOCAL_ID);
					}
					break;
				case FORCESEEK:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case ID:
					{
					setState(672);
					table_name();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(679);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,87,_ctx) ) {
				case 1:
					{
					setState(675);
					match(LR_BRACKET);
					setState(676);
					column_name_list();
					setState(677);
					match(RR_BRACKET);
					}
					break;
				}
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Output_dml_list_elemContext extends ParserRuleContext {
		public Output_column_nameContext output_column_name() {
			return getRuleContext(Output_column_nameContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Column_aliasContext column_alias() {
			return getRuleContext(Column_aliasContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Output_dml_list_elemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_output_dml_list_elem; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOutput_dml_list_elem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOutput_dml_list_elem(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOutput_dml_list_elem(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Output_dml_list_elemContext output_dml_list_elem() throws RecognitionException {
		Output_dml_list_elemContext _localctx = new Output_dml_list_elemContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_output_dml_list_elem);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(685);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,89,_ctx) ) {
			case 1:
				{
				setState(683);
				output_column_name();
				}
				break;
			case 2:
				{
				setState(684);
				expression(0);
				}
				break;
			}
			setState(691);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,91,_ctx) ) {
			case 1:
				{
				setState(688);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==AS) {
					{
					setState(687);
					match(AS);
					}
				}

				setState(690);
				column_alias();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Output_column_nameContext extends ParserRuleContext {
		public TerminalNode DELETED() { return getToken(tsqlParser.DELETED, 0); }
		public TerminalNode INSERTED() { return getToken(tsqlParser.INSERTED, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public Column_nameContext column_name() {
			return getRuleContext(Column_nameContext.class,0);
		}
		public TerminalNode DOLLAR_ACTION() { return getToken(tsqlParser.DOLLAR_ACTION, 0); }
		public Output_column_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_output_column_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOutput_column_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOutput_column_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOutput_column_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Output_column_nameContext output_column_name() throws RecognitionException {
		Output_column_nameContext _localctx = new Output_column_nameContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_output_column_name);
		try {
			setState(704);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(696);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,92,_ctx) ) {
				case 1:
					{
					setState(693);
					match(DELETED);
					}
					break;
				case 2:
					{
					setState(694);
					match(INSERTED);
					}
					break;
				case 3:
					{
					setState(695);
					table_name();
					}
					break;
				}
				setState(698);
				match(DOT);
				setState(701);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case STAR:
					{
					setState(699);
					match(STAR);
					}
					break;
				case FORCESEEK:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case ID:
					{
					setState(700);
					column_name();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			case DOLLAR_ACTION:
				enterOuterAlt(_localctx, 2);
				{
				setState(703);
				match(DOLLAR_ACTION);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Create_indexContext extends ParserRuleContext {
		public IdContext name;
		public TerminalNode CREATE() { return getToken(tsqlParser.CREATE, 0); }
		public TerminalNode INDEX() { return getToken(tsqlParser.INDEX, 0); }
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public Table_name_with_hintContext table_name_with_hint() {
			return getRuleContext(Table_name_with_hintContext.class,0);
		}
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode UNIQUE() { return getToken(tsqlParser.UNIQUE, 0); }
		public ClusteredContext clustered() {
			return getRuleContext(ClusteredContext.class,0);
		}
		public Create_indexContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_create_index; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCreate_index(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCreate_index(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCreate_index(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Create_indexContext create_index() throws RecognitionException {
		Create_indexContext _localctx = new Create_indexContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_create_index);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(706);
			match(CREATE);
			setState(708);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==UNIQUE) {
				{
				setState(707);
				match(UNIQUE);
				}
			}

			setState(711);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CLUSTERED || _la==NONCLUSTERED) {
				{
				setState(710);
				clustered();
				}
			}

			setState(713);
			match(INDEX);
			setState(714);
			((Create_indexContext)_localctx).name = id();
			setState(715);
			match(ON);
			setState(716);
			table_name_with_hint();
			setState(717);
			match(LR_BRACKET);
			setState(718);
			column_name_list();
			setState(719);
			match(RR_BRACKET);
			setState(721);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,97,_ctx) ) {
			case 1:
				{
				setState(720);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Create_procedureContext extends ParserRuleContext {
		public TerminalNode CREATE() { return getToken(tsqlParser.CREATE, 0); }
		public Func_proc_nameContext func_proc_name() {
			return getRuleContext(Func_proc_nameContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public TerminalNode PROC() { return getToken(tsqlParser.PROC, 0); }
		public TerminalNode PROCEDURE() { return getToken(tsqlParser.PROCEDURE, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public List<Procedure_paramContext> procedure_param() {
			return getRuleContexts(Procedure_paramContext.class);
		}
		public Procedure_paramContext procedure_param(int i) {
			return getRuleContext(Procedure_paramContext.class,i);
		}
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public List<Procedure_optionContext> procedure_option() {
			return getRuleContexts(Procedure_optionContext.class);
		}
		public Procedure_optionContext procedure_option(int i) {
			return getRuleContext(Procedure_optionContext.class,i);
		}
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode REPLICATION() { return getToken(tsqlParser.REPLICATION, 0); }
		public List<Sql_clauseContext> sql_clause() {
			return getRuleContexts(Sql_clauseContext.class);
		}
		public Sql_clauseContext sql_clause(int i) {
			return getRuleContext(Sql_clauseContext.class,i);
		}
		public Create_procedureContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_create_procedure; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCreate_procedure(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCreate_procedure(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCreate_procedure(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Create_procedureContext create_procedure() throws RecognitionException {
		Create_procedureContext _localctx = new Create_procedureContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_create_procedure);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(723);
			match(CREATE);
			setState(724);
			_la = _input.LA(1);
			if ( !(_la==PROC || _la==PROCEDURE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(725);
			func_proc_name();
			setState(728);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SEMI) {
				{
				setState(726);
				match(SEMI);
				setState(727);
				match(DECIMAL);
				}
			}

			setState(744);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LOCAL_ID || _la==LR_BRACKET) {
				{
				setState(731);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LR_BRACKET) {
					{
					setState(730);
					match(LR_BRACKET);
					}
				}

				setState(733);
				procedure_param();
				setState(738);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(734);
					match(COMMA);
					setState(735);
					procedure_param();
					}
					}
					setState(740);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(742);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==RR_BRACKET) {
					{
					setState(741);
					match(RR_BRACKET);
					}
				}

				}
			}

			setState(755);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(746);
				match(WITH);
				setState(747);
				procedure_option();
				setState(752);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(748);
					match(COMMA);
					setState(749);
					procedure_option();
					}
					}
					setState(754);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(759);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FOR) {
				{
				setState(757);
				match(FOR);
				setState(758);
				match(REPLICATION);
				}
			}

			setState(761);
			match(AS);
			setState(763); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(762);
					sql_clause();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(765); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,106,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Procedure_paramContext extends ParserRuleContext {
		public Default_valueContext default_val;
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Data_typeContext data_type() {
			return getRuleContext(Data_typeContext.class,0);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public TerminalNode VARYING() { return getToken(tsqlParser.VARYING, 0); }
		public Default_valueContext default_value() {
			return getRuleContext(Default_valueContext.class,0);
		}
		public TerminalNode OUT() { return getToken(tsqlParser.OUT, 0); }
		public TerminalNode OUTPUT() { return getToken(tsqlParser.OUTPUT, 0); }
		public TerminalNode READONLY() { return getToken(tsqlParser.READONLY, 0); }
		public Procedure_paramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_procedure_param; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterProcedure_param(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitProcedure_param(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitProcedure_param(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Procedure_paramContext procedure_param() throws RecognitionException {
		Procedure_paramContext _localctx = new Procedure_paramContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_procedure_param);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(767);
			match(LOCAL_ID);
			setState(771);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,107,_ctx) ) {
			case 1:
				{
				setState(768);
				id();
				setState(769);
				match(DOT);
				}
				break;
			}
			setState(774);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AS) {
				{
				setState(773);
				match(AS);
				}
			}

			setState(776);
			data_type();
			setState(778);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==VARYING) {
				{
				setState(777);
				match(VARYING);
				}
			}

			setState(782);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==EQUAL) {
				{
				setState(780);
				match(EQUAL);
				setState(781);
				((Procedure_paramContext)_localctx).default_val = default_value();
				}
			}

			setState(785);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (((((_la - 259)) & ~0x3f) == 0 && ((1L << (_la - 259)) & ((1L << (OUT - 259)) | (1L << (OUTPUT - 259)) | (1L << (READONLY - 259)))) != 0)) {
				{
				setState(784);
				_la = _input.LA(1);
				if ( !(((((_la - 259)) & ~0x3f) == 0 && ((1L << (_la - 259)) & ((1L << (OUT - 259)) | (1L << (OUTPUT - 259)) | (1L << (READONLY - 259)))) != 0)) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Procedure_optionContext extends ParserRuleContext {
		public TerminalNode ENCRYPTION() { return getToken(tsqlParser.ENCRYPTION, 0); }
		public TerminalNode RECOMPILE() { return getToken(tsqlParser.RECOMPILE, 0); }
		public Execute_clauseContext execute_clause() {
			return getRuleContext(Execute_clauseContext.class,0);
		}
		public Procedure_optionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_procedure_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterProcedure_option(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitProcedure_option(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitProcedure_option(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Procedure_optionContext procedure_option() throws RecognitionException {
		Procedure_optionContext _localctx = new Procedure_optionContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_procedure_option);
		try {
			setState(790);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ENCRYPTION:
				enterOuterAlt(_localctx, 1);
				{
				setState(787);
				match(ENCRYPTION);
				}
				break;
			case RECOMPILE:
				enterOuterAlt(_localctx, 2);
				{
				setState(788);
				match(RECOMPILE);
				}
				break;
			case EXEC:
			case EXECUTE:
				enterOuterAlt(_localctx, 3);
				{
				setState(789);
				execute_clause();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Create_statisticsContext extends ParserRuleContext {
		public On_offContext INCREMENTAL;
		public TerminalNode CREATE() { return getToken(tsqlParser.CREATE, 0); }
		public TerminalNode STATISTICS() { return getToken(tsqlParser.STATISTICS, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public Table_name_with_hintContext table_name_with_hint() {
			return getRuleContext(Table_name_with_hintContext.class,0);
		}
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public TerminalNode FULLSCAN() { return getToken(tsqlParser.FULLSCAN, 0); }
		public TerminalNode SAMPLE() { return getToken(tsqlParser.SAMPLE, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode STATS_STREAM() { return getToken(tsqlParser.STATS_STREAM, 0); }
		public TerminalNode PERCENT() { return getToken(tsqlParser.PERCENT, 0); }
		public TerminalNode ROWS() { return getToken(tsqlParser.ROWS, 0); }
		public TerminalNode NORECOMPUTE() { return getToken(tsqlParser.NORECOMPUTE, 0); }
		public On_offContext on_off() {
			return getRuleContext(On_offContext.class,0);
		}
		public Create_statisticsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_create_statistics; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCreate_statistics(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCreate_statistics(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCreate_statistics(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Create_statisticsContext create_statistics() throws RecognitionException {
		Create_statisticsContext _localctx = new Create_statisticsContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_create_statistics);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(792);
			match(CREATE);
			setState(793);
			match(STATISTICS);
			setState(794);
			id();
			setState(795);
			match(ON);
			setState(796);
			table_name_with_hint();
			setState(797);
			match(LR_BRACKET);
			setState(798);
			column_name_list();
			setState(799);
			match(RR_BRACKET);
			setState(816);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,116,_ctx) ) {
			case 1:
				{
				setState(800);
				match(WITH);
				setState(806);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case FULLSCAN:
					{
					setState(801);
					match(FULLSCAN);
					}
					break;
				case SAMPLE:
					{
					setState(802);
					match(SAMPLE);
					setState(803);
					match(DECIMAL);
					setState(804);
					_la = _input.LA(1);
					if ( !(_la==PERCENT || _la==ROWS) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
					break;
				case STATS_STREAM:
					{
					setState(805);
					match(STATS_STREAM);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(810);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,114,_ctx) ) {
				case 1:
					{
					setState(808);
					match(COMMA);
					setState(809);
					match(NORECOMPUTE);
					}
					break;
				}
				setState(814);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(812);
					match(COMMA);
					setState(813);
					((Create_statisticsContext)_localctx).INCREMENTAL = on_off();
					}
				}

				}
				break;
			}
			setState(819);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,117,_ctx) ) {
			case 1:
				{
				setState(818);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Create_tableContext extends ParserRuleContext {
		public TerminalNode CREATE() { return getToken(tsqlParser.CREATE, 0); }
		public TerminalNode TABLE() { return getToken(tsqlParser.TABLE, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public List<Column_def_table_constraintContext> column_def_table_constraint() {
			return getRuleContexts(Column_def_table_constraintContext.class);
		}
		public Column_def_table_constraintContext column_def_table_constraint(int i) {
			return getRuleContext(Column_def_table_constraintContext.class,i);
		}
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode DEFAULT() { return getToken(tsqlParser.DEFAULT, 0); }
		public Create_tableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_create_table; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCreate_table(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCreate_table(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCreate_table(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Create_tableContext create_table() throws RecognitionException {
		Create_tableContext _localctx = new Create_tableContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_create_table);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(821);
			match(CREATE);
			setState(822);
			match(TABLE);
			setState(823);
			table_name();
			setState(824);
			match(LR_BRACKET);
			setState(825);
			column_def_table_constraint();
			setState(832);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,119,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(827);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==COMMA) {
						{
						setState(826);
						match(COMMA);
						}
					}

					setState(829);
					column_def_table_constraint();
					}
					} 
				}
				setState(834);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,119,_ctx);
			}
			setState(836);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(835);
				match(COMMA);
				}
			}

			setState(838);
			match(RR_BRACKET);
			setState(842);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ON:
				{
				setState(839);
				match(ON);
				setState(840);
				id();
				}
				break;
			case DEFAULT:
				{
				setState(841);
				match(DEFAULT);
				}
				break;
			case EOF:
			case ALTER:
			case BEGIN:
			case BREAK:
			case CLOSE:
			case COMMIT:
			case CONTINUE:
			case CREATE:
			case DEALLOCATE:
			case DECLARE:
			case DELETE:
			case DROP:
			case ELSE:
			case END:
			case EXEC:
			case EXECUTE:
			case FETCH:
			case FORCESEEK:
			case GOTO:
			case IF:
			case INSERT:
			case OPEN:
			case PRINT:
			case RAISERROR:
			case RETURN:
			case REVERT:
			case ROLLBACK:
			case SAVE:
			case SELECT:
			case SET:
			case UPDATE:
			case USE:
			case WAITFOR:
			case WHILE:
			case WITH:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
			case LR_BRACKET:
			case SEMI:
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(845);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,122,_ctx) ) {
			case 1:
				{
				setState(844);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Create_viewContext extends ParserRuleContext {
		public TerminalNode CREATE() { return getToken(tsqlParser.CREATE, 0); }
		public TerminalNode VIEW() { return getToken(tsqlParser.VIEW, 0); }
		public View_nameContext view_name() {
			return getRuleContext(View_nameContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public List<Column_nameContext> column_name() {
			return getRuleContexts(Column_nameContext.class);
		}
		public Column_nameContext column_name(int i) {
			return getRuleContext(Column_nameContext.class,i);
		}
		public List<TerminalNode> WITH() { return getTokens(tsqlParser.WITH); }
		public TerminalNode WITH(int i) {
			return getToken(tsqlParser.WITH, i);
		}
		public List<View_attributeContext> view_attribute() {
			return getRuleContexts(View_attributeContext.class);
		}
		public View_attributeContext view_attribute(int i) {
			return getRuleContext(View_attributeContext.class,i);
		}
		public TerminalNode CHECK() { return getToken(tsqlParser.CHECK, 0); }
		public TerminalNode OPTION() { return getToken(tsqlParser.OPTION, 0); }
		public Create_viewContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_create_view; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCreate_view(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCreate_view(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCreate_view(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Create_viewContext create_view() throws RecognitionException {
		Create_viewContext _localctx = new Create_viewContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_create_view);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(847);
			match(CREATE);
			setState(848);
			match(VIEW);
			setState(849);
			view_name();
			setState(861);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LR_BRACKET) {
				{
				setState(850);
				match(LR_BRACKET);
				setState(851);
				column_name();
				setState(856);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(852);
					match(COMMA);
					setState(853);
					column_name();
					}
					}
					setState(858);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(859);
				match(RR_BRACKET);
				}
			}

			setState(872);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(863);
				match(WITH);
				setState(864);
				view_attribute();
				setState(869);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(865);
					match(COMMA);
					setState(866);
					view_attribute();
					}
					}
					setState(871);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(874);
			match(AS);
			setState(875);
			select_statement();
			setState(879);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,127,_ctx) ) {
			case 1:
				{
				setState(876);
				match(WITH);
				setState(877);
				match(CHECK);
				setState(878);
				match(OPTION);
				}
				break;
			}
			setState(882);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,128,_ctx) ) {
			case 1:
				{
				setState(881);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class View_attributeContext extends ParserRuleContext {
		public TerminalNode ENCRYPTION() { return getToken(tsqlParser.ENCRYPTION, 0); }
		public TerminalNode SCHEMABINDING() { return getToken(tsqlParser.SCHEMABINDING, 0); }
		public TerminalNode VIEW_METADATA() { return getToken(tsqlParser.VIEW_METADATA, 0); }
		public View_attributeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_view_attribute; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterView_attribute(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitView_attribute(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitView_attribute(this);
			else return visitor.visitChildren(this);
		}
	}

	public final View_attributeContext view_attribute() throws RecognitionException {
		View_attributeContext _localctx = new View_attributeContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_view_attribute);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(884);
			_la = _input.LA(1);
			if ( !(_la==ENCRYPTION || _la==SCHEMABINDING || _la==VIEW_METADATA) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Alter_tableContext extends ParserRuleContext {
		public TerminalNode ALTER() { return getToken(tsqlParser.ALTER, 0); }
		public List<TerminalNode> TABLE() { return getTokens(tsqlParser.TABLE); }
		public TerminalNode TABLE(int i) {
			return getToken(tsqlParser.TABLE, i);
		}
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public TerminalNode SET() { return getToken(tsqlParser.SET, 0); }
		public TerminalNode LOCK_ESCALATION() { return getToken(tsqlParser.LOCK_ESCALATION, 0); }
		public TerminalNode AUTO() { return getToken(tsqlParser.AUTO, 0); }
		public TerminalNode DISABLE() { return getToken(tsqlParser.DISABLE, 0); }
		public TerminalNode ADD() { return getToken(tsqlParser.ADD, 0); }
		public Column_def_table_constraintContext column_def_table_constraint() {
			return getRuleContext(Column_def_table_constraintContext.class,0);
		}
		public Alter_tableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_alter_table; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAlter_table(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAlter_table(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAlter_table(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Alter_tableContext alter_table() throws RecognitionException {
		Alter_tableContext _localctx = new Alter_tableContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_alter_table);
		int _la;
		try {
			setState(906);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,131,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(886);
				match(ALTER);
				setState(887);
				match(TABLE);
				setState(888);
				table_name();
				setState(889);
				match(SET);
				setState(890);
				match(LR_BRACKET);
				setState(891);
				match(LOCK_ESCALATION);
				setState(892);
				match(EQUAL);
				setState(893);
				_la = _input.LA(1);
				if ( !(((((_la - 159)) & ~0x3f) == 0 && ((1L << (_la - 159)) & ((1L << (TABLE - 159)) | (1L << (AUTO - 159)) | (1L << (DISABLE - 159)))) != 0)) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(894);
				match(RR_BRACKET);
				setState(896);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,129,_ctx) ) {
				case 1:
					{
					setState(895);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(898);
				match(ALTER);
				setState(899);
				match(TABLE);
				setState(900);
				table_name();
				setState(901);
				match(ADD);
				setState(902);
				column_def_table_constraint();
				setState(904);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,130,_ctx) ) {
				case 1:
					{
					setState(903);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Alter_databaseContext extends ParserRuleContext {
		public IdContext database;
		public IdContext new_name;
		public IdContext collation;
		public TerminalNode ALTER() { return getToken(tsqlParser.ALTER, 0); }
		public TerminalNode DATABASE() { return getToken(tsqlParser.DATABASE, 0); }
		public TerminalNode CURRENT() { return getToken(tsqlParser.CURRENT, 0); }
		public TerminalNode MODIFY() { return getToken(tsqlParser.MODIFY, 0); }
		public TerminalNode NAME() { return getToken(tsqlParser.NAME, 0); }
		public TerminalNode COLLATE() { return getToken(tsqlParser.COLLATE, 0); }
		public TerminalNode SET() { return getToken(tsqlParser.SET, 0); }
		public Database_optionContext database_option() {
			return getRuleContext(Database_optionContext.class,0);
		}
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Alter_databaseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_alter_database; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAlter_database(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAlter_database(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAlter_database(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Alter_databaseContext alter_database() throws RecognitionException {
		Alter_databaseContext _localctx = new Alter_databaseContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_alter_database);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(908);
			match(ALTER);
			setState(909);
			match(DATABASE);
			setState(912);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				{
				setState(910);
				((Alter_databaseContext)_localctx).database = id();
				}
				break;
			case CURRENT:
				{
				setState(911);
				match(CURRENT);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(922);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case MODIFY:
				{
				setState(914);
				match(MODIFY);
				setState(915);
				match(NAME);
				setState(916);
				match(EQUAL);
				setState(917);
				((Alter_databaseContext)_localctx).new_name = id();
				}
				break;
			case COLLATE:
				{
				setState(918);
				match(COLLATE);
				setState(919);
				((Alter_databaseContext)_localctx).collation = id();
				}
				break;
			case SET:
				{
				setState(920);
				match(SET);
				setState(921);
				database_option();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(925);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,134,_ctx) ) {
			case 1:
				{
				setState(924);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Database_optionContext extends ParserRuleContext {
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public TerminalNode FULL() { return getToken(tsqlParser.FULL, 0); }
		public Database_optionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_database_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDatabase_option(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDatabase_option(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDatabase_option(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Database_optionContext database_option() throws RecognitionException {
		Database_optionContext _localctx = new Database_optionContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_database_option);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(927);
			id();
			setState(930);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,135,_ctx) ) {
			case 1:
				{
				setState(928);
				id();
				}
				break;
			case 2:
				{
				setState(929);
				match(FULL);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Drop_indexContext extends ParserRuleContext {
		public IdContext name;
		public TerminalNode DROP() { return getToken(tsqlParser.DROP, 0); }
		public TerminalNode INDEX() { return getToken(tsqlParser.INDEX, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode IF() { return getToken(tsqlParser.IF, 0); }
		public TerminalNode EXISTS() { return getToken(tsqlParser.EXISTS, 0); }
		public Drop_indexContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_drop_index; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDrop_index(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDrop_index(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDrop_index(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Drop_indexContext drop_index() throws RecognitionException {
		Drop_indexContext _localctx = new Drop_indexContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_drop_index);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(932);
			match(DROP);
			setState(933);
			match(INDEX);
			setState(936);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IF) {
				{
				setState(934);
				match(IF);
				setState(935);
				match(EXISTS);
				}
			}

			setState(938);
			((Drop_indexContext)_localctx).name = id();
			setState(940);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,137,_ctx) ) {
			case 1:
				{
				setState(939);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Drop_procedureContext extends ParserRuleContext {
		public TerminalNode DROP() { return getToken(tsqlParser.DROP, 0); }
		public TerminalNode PROCEDURE() { return getToken(tsqlParser.PROCEDURE, 0); }
		public Func_proc_nameContext func_proc_name() {
			return getRuleContext(Func_proc_nameContext.class,0);
		}
		public TerminalNode IF() { return getToken(tsqlParser.IF, 0); }
		public TerminalNode EXISTS() { return getToken(tsqlParser.EXISTS, 0); }
		public Drop_procedureContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_drop_procedure; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDrop_procedure(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDrop_procedure(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDrop_procedure(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Drop_procedureContext drop_procedure() throws RecognitionException {
		Drop_procedureContext _localctx = new Drop_procedureContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_drop_procedure);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(942);
			match(DROP);
			setState(943);
			match(PROCEDURE);
			setState(946);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IF) {
				{
				setState(944);
				match(IF);
				setState(945);
				match(EXISTS);
				}
			}

			setState(948);
			func_proc_name();
			setState(950);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,139,_ctx) ) {
			case 1:
				{
				setState(949);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Drop_statisticsContext extends ParserRuleContext {
		public IdContext name;
		public TerminalNode DROP() { return getToken(tsqlParser.DROP, 0); }
		public TerminalNode STATISTICS() { return getToken(tsqlParser.STATISTICS, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public Drop_statisticsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_drop_statistics; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDrop_statistics(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDrop_statistics(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDrop_statistics(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Drop_statisticsContext drop_statistics() throws RecognitionException {
		Drop_statisticsContext _localctx = new Drop_statisticsContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_drop_statistics);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(952);
			match(DROP);
			setState(953);
			match(STATISTICS);
			setState(957);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,140,_ctx) ) {
			case 1:
				{
				setState(954);
				table_name();
				setState(955);
				match(DOT);
				}
				break;
			}
			setState(959);
			((Drop_statisticsContext)_localctx).name = id();
			setState(960);
			match(SEMI);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Drop_tableContext extends ParserRuleContext {
		public TerminalNode DROP() { return getToken(tsqlParser.DROP, 0); }
		public TerminalNode TABLE() { return getToken(tsqlParser.TABLE, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public TerminalNode IF() { return getToken(tsqlParser.IF, 0); }
		public TerminalNode EXISTS() { return getToken(tsqlParser.EXISTS, 0); }
		public Drop_tableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_drop_table; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDrop_table(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDrop_table(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDrop_table(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Drop_tableContext drop_table() throws RecognitionException {
		Drop_tableContext _localctx = new Drop_tableContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_drop_table);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(962);
			match(DROP);
			setState(963);
			match(TABLE);
			setState(966);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IF) {
				{
				setState(964);
				match(IF);
				setState(965);
				match(EXISTS);
				}
			}

			setState(968);
			table_name();
			setState(970);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,142,_ctx) ) {
			case 1:
				{
				setState(969);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Drop_viewContext extends ParserRuleContext {
		public TerminalNode DROP() { return getToken(tsqlParser.DROP, 0); }
		public TerminalNode VIEW() { return getToken(tsqlParser.VIEW, 0); }
		public List<View_nameContext> view_name() {
			return getRuleContexts(View_nameContext.class);
		}
		public View_nameContext view_name(int i) {
			return getRuleContext(View_nameContext.class,i);
		}
		public TerminalNode IF() { return getToken(tsqlParser.IF, 0); }
		public TerminalNode EXISTS() { return getToken(tsqlParser.EXISTS, 0); }
		public Drop_viewContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_drop_view; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDrop_view(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDrop_view(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDrop_view(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Drop_viewContext drop_view() throws RecognitionException {
		Drop_viewContext _localctx = new Drop_viewContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_drop_view);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(972);
			match(DROP);
			setState(973);
			match(VIEW);
			setState(976);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==IF) {
				{
				setState(974);
				match(IF);
				setState(975);
				match(EXISTS);
				}
			}

			setState(978);
			view_name();
			setState(983);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(979);
				match(COMMA);
				setState(980);
				view_name();
				}
				}
				setState(985);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(987);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,145,_ctx) ) {
			case 1:
				{
				setState(986);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Rowset_function_limitedContext extends ParserRuleContext {
		public OpenqueryContext openquery() {
			return getRuleContext(OpenqueryContext.class,0);
		}
		public OpendatasourceContext opendatasource() {
			return getRuleContext(OpendatasourceContext.class,0);
		}
		public Rowset_function_limitedContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rowset_function_limited; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterRowset_function_limited(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitRowset_function_limited(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitRowset_function_limited(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Rowset_function_limitedContext rowset_function_limited() throws RecognitionException {
		Rowset_function_limitedContext _localctx = new Rowset_function_limitedContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_rowset_function_limited);
		try {
			setState(991);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case OPENQUERY:
				enterOuterAlt(_localctx, 1);
				{
				setState(989);
				openquery();
				}
				break;
			case OPENDATASOURCE:
				enterOuterAlt(_localctx, 2);
				{
				setState(990);
				opendatasource();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OpenqueryContext extends ParserRuleContext {
		public IdContext linked_server;
		public Token query;
		public TerminalNode OPENQUERY() { return getToken(tsqlParser.OPENQUERY, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public OpenqueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_openquery; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOpenquery(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOpenquery(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOpenquery(this);
			else return visitor.visitChildren(this);
		}
	}

	public final OpenqueryContext openquery() throws RecognitionException {
		OpenqueryContext _localctx = new OpenqueryContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_openquery);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(993);
			match(OPENQUERY);
			setState(994);
			match(LR_BRACKET);
			setState(995);
			((OpenqueryContext)_localctx).linked_server = id();
			setState(996);
			match(COMMA);
			setState(997);
			((OpenqueryContext)_localctx).query = match(STRING);
			setState(998);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OpendatasourceContext extends ParserRuleContext {
		public Token provider;
		public Token init;
		public IdContext database;
		public IdContext scheme;
		public IdContext table;
		public TerminalNode OPENDATASOURCE() { return getToken(tsqlParser.OPENDATASOURCE, 0); }
		public List<TerminalNode> STRING() { return getTokens(tsqlParser.STRING); }
		public TerminalNode STRING(int i) {
			return getToken(tsqlParser.STRING, i);
		}
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public OpendatasourceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_opendatasource; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOpendatasource(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOpendatasource(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOpendatasource(this);
			else return visitor.visitChildren(this);
		}
	}

	public final OpendatasourceContext opendatasource() throws RecognitionException {
		OpendatasourceContext _localctx = new OpendatasourceContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_opendatasource);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1000);
			match(OPENDATASOURCE);
			setState(1001);
			match(LR_BRACKET);
			setState(1002);
			((OpendatasourceContext)_localctx).provider = match(STRING);
			setState(1003);
			match(COMMA);
			setState(1004);
			((OpendatasourceContext)_localctx).init = match(STRING);
			setState(1005);
			match(RR_BRACKET);
			setState(1006);
			match(DOT);
			setState(1008);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
				{
				setState(1007);
				((OpendatasourceContext)_localctx).database = id();
				}
			}

			setState(1010);
			match(DOT);
			setState(1012);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
				{
				setState(1011);
				((OpendatasourceContext)_localctx).scheme = id();
				}
			}

			setState(1014);
			match(DOT);
			{
			setState(1015);
			((OpendatasourceContext)_localctx).table = id();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Declare_statementContext extends ParserRuleContext {
		public TerminalNode DECLARE() { return getToken(tsqlParser.DECLARE, 0); }
		public List<Declare_localContext> declare_local() {
			return getRuleContexts(Declare_localContext.class);
		}
		public Declare_localContext declare_local(int i) {
			return getRuleContext(Declare_localContext.class,i);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Table_type_definitionContext table_type_definition() {
			return getRuleContext(Table_type_definitionContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Declare_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declare_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDeclare_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDeclare_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDeclare_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Declare_statementContext declare_statement() throws RecognitionException {
		Declare_statementContext _localctx = new Declare_statementContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_declare_statement);
		int _la;
		try {
			setState(1038);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,153,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1017);
				match(DECLARE);
				setState(1018);
				declare_local();
				setState(1023);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1019);
					match(COMMA);
					setState(1020);
					declare_local();
					}
					}
					setState(1025);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(1027);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,150,_ctx) ) {
				case 1:
					{
					setState(1026);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1029);
				match(DECLARE);
				setState(1030);
				match(LOCAL_ID);
				setState(1032);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==AS) {
					{
					setState(1031);
					match(AS);
					}
				}

				setState(1034);
				table_type_definition();
				setState(1036);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,152,_ctx) ) {
				case 1:
					{
					setState(1035);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Cursor_statementContext extends ParserRuleContext {
		public TerminalNode CLOSE() { return getToken(tsqlParser.CLOSE, 0); }
		public Cursor_nameContext cursor_name() {
			return getRuleContext(Cursor_nameContext.class,0);
		}
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public TerminalNode DEALLOCATE() { return getToken(tsqlParser.DEALLOCATE, 0); }
		public Declare_cursorContext declare_cursor() {
			return getRuleContext(Declare_cursorContext.class,0);
		}
		public Fetch_cursorContext fetch_cursor() {
			return getRuleContext(Fetch_cursorContext.class,0);
		}
		public TerminalNode OPEN() { return getToken(tsqlParser.OPEN, 0); }
		public Cursor_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_cursor_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCursor_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCursor_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCursor_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Cursor_statementContext cursor_statement() throws RecognitionException {
		Cursor_statementContext _localctx = new Cursor_statementContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_cursor_statement);
		try {
			setState(1066);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CLOSE:
				enterOuterAlt(_localctx, 1);
				{
				setState(1040);
				match(CLOSE);
				setState(1042);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,154,_ctx) ) {
				case 1:
					{
					setState(1041);
					match(GLOBAL);
					}
					break;
				}
				setState(1044);
				cursor_name();
				setState(1046);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,155,_ctx) ) {
				case 1:
					{
					setState(1045);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case DEALLOCATE:
				enterOuterAlt(_localctx, 2);
				{
				setState(1048);
				match(DEALLOCATE);
				setState(1050);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,156,_ctx) ) {
				case 1:
					{
					setState(1049);
					match(GLOBAL);
					}
					break;
				}
				setState(1052);
				cursor_name();
				setState(1054);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,157,_ctx) ) {
				case 1:
					{
					setState(1053);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case DECLARE:
				enterOuterAlt(_localctx, 3);
				{
				setState(1056);
				declare_cursor();
				}
				break;
			case FETCH:
				enterOuterAlt(_localctx, 4);
				{
				setState(1057);
				fetch_cursor();
				}
				break;
			case OPEN:
				enterOuterAlt(_localctx, 5);
				{
				setState(1058);
				match(OPEN);
				setState(1060);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,158,_ctx) ) {
				case 1:
					{
					setState(1059);
					match(GLOBAL);
					}
					break;
				}
				setState(1062);
				cursor_name();
				setState(1064);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,159,_ctx) ) {
				case 1:
					{
					setState(1063);
					match(SEMI);
					}
					break;
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute_statementContext extends ParserRuleContext {
		public Token return_status;
		public Func_proc_nameContext func_proc_name() {
			return getRuleContext(Func_proc_nameContext.class,0);
		}
		public TerminalNode EXEC() { return getToken(tsqlParser.EXEC, 0); }
		public TerminalNode EXECUTE() { return getToken(tsqlParser.EXECUTE, 0); }
		public List<Execute_statement_argContext> execute_statement_arg() {
			return getRuleContexts(Execute_statement_argContext.class);
		}
		public Execute_statement_argContext execute_statement_arg(int i) {
			return getRuleContext(Execute_statement_argContext.class,i);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public List<Execute_var_stringContext> execute_var_string() {
			return getRuleContexts(Execute_var_stringContext.class);
		}
		public Execute_var_stringContext execute_var_string(int i) {
			return getRuleContext(Execute_var_stringContext.class,i);
		}
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public TerminalNode LOGIN() { return getToken(tsqlParser.LOGIN, 0); }
		public TerminalNode USER() { return getToken(tsqlParser.USER, 0); }
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Execute_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterExecute_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitExecute_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitExecute_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Execute_statementContext execute_statement() throws RecognitionException {
		Execute_statementContext _localctx = new Execute_statementContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_execute_statement);
		int _la;
		try {
			setState(1109);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,169,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1068);
				_la = _input.LA(1);
				if ( !(_la==EXEC || _la==EXECUTE) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1071);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LOCAL_ID) {
					{
					setState(1069);
					((Execute_statementContext)_localctx).return_status = match(LOCAL_ID);
					setState(1070);
					match(EQUAL);
					}
				}

				setState(1073);
				func_proc_name();
				setState(1082);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==DEFAULT || _la==NULL || ((((_la - 318)) & ~0x3f) == 0 && ((1L << (_la - 318)) & ((1L << (LOCAL_ID - 318)) | (1L << (DECIMAL - 318)) | (1L << (STRING - 318)) | (1L << (BINARY - 318)) | (1L << (FLOAT - 318)) | (1L << (REAL - 318)) | (1L << (DOLLAR - 318)) | (1L << (PLUS - 318)) | (1L << (MINUS - 318)))) != 0)) {
					{
					setState(1074);
					execute_statement_arg();
					setState(1079);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==COMMA) {
						{
						{
						setState(1075);
						match(COMMA);
						setState(1076);
						execute_statement_arg();
						}
						}
						setState(1081);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
				}

				setState(1085);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,164,_ctx) ) {
				case 1:
					{
					setState(1084);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1087);
				_la = _input.LA(1);
				if ( !(_la==EXEC || _la==EXECUTE) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1088);
				match(LR_BRACKET);
				setState(1089);
				execute_var_string();
				setState(1094);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==PLUS) {
					{
					{
					setState(1090);
					match(PLUS);
					setState(1091);
					execute_var_string();
					}
					}
					setState(1096);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(1097);
				match(RR_BRACKET);
				setState(1104);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,167,_ctx) ) {
				case 1:
					{
					setState(1099);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==AS) {
						{
						setState(1098);
						match(AS);
						}
					}

					setState(1101);
					_la = _input.LA(1);
					if ( !(_la==USER || _la==LOGIN) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(1102);
					match(EQUAL);
					setState(1103);
					match(STRING);
					}
					break;
				}
				setState(1107);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,168,_ctx) ) {
				case 1:
					{
					setState(1106);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute_statement_argContext extends ParserRuleContext {
		public Token parameter;
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public List<TerminalNode> LOCAL_ID() { return getTokens(tsqlParser.LOCAL_ID); }
		public TerminalNode LOCAL_ID(int i) {
			return getToken(tsqlParser.LOCAL_ID, i);
		}
		public TerminalNode DEFAULT() { return getToken(tsqlParser.DEFAULT, 0); }
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public TerminalNode OUTPUT() { return getToken(tsqlParser.OUTPUT, 0); }
		public TerminalNode OUT() { return getToken(tsqlParser.OUT, 0); }
		public Execute_statement_argContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute_statement_arg; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterExecute_statement_arg(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitExecute_statement_arg(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitExecute_statement_arg(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Execute_statement_argContext execute_statement_arg() throws RecognitionException {
		Execute_statement_argContext _localctx = new Execute_statement_argContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_execute_statement_arg);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1113);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,170,_ctx) ) {
			case 1:
				{
				setState(1111);
				((Execute_statement_argContext)_localctx).parameter = match(LOCAL_ID);
				setState(1112);
				match(EQUAL);
				}
				break;
			}
			setState(1122);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case DECIMAL:
			case STRING:
			case BINARY:
			case FLOAT:
			case REAL:
			case DOLLAR:
			case PLUS:
			case MINUS:
				{
				setState(1115);
				constant();
				}
				break;
			case LOCAL_ID:
				{
				setState(1116);
				match(LOCAL_ID);
				setState(1118);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,171,_ctx) ) {
				case 1:
					{
					setState(1117);
					_la = _input.LA(1);
					if ( !(_la==OUT || _la==OUTPUT) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
					break;
				}
				}
				break;
			case DEFAULT:
				{
				setState(1120);
				match(DEFAULT);
				}
				break;
			case NULL:
				{
				setState(1121);
				match(NULL);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute_var_stringContext extends ParserRuleContext {
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public Execute_var_stringContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute_var_string; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterExecute_var_string(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitExecute_var_string(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitExecute_var_string(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Execute_var_stringContext execute_var_string() throws RecognitionException {
		Execute_var_stringContext _localctx = new Execute_var_stringContext(_ctx, getState());
		enterRule(_localctx, 72, RULE_execute_var_string);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1124);
			_la = _input.LA(1);
			if ( !(_la==LOCAL_ID || _la==STRING) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Security_statementContext extends ParserRuleContext {
		public Execute_clauseContext execute_clause() {
			return getRuleContext(Execute_clauseContext.class,0);
		}
		public TerminalNode REVERT() { return getToken(tsqlParser.REVERT, 0); }
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public TerminalNode COOKIE() { return getToken(tsqlParser.COOKIE, 0); }
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Security_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_security_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSecurity_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSecurity_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSecurity_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Security_statementContext security_statement() throws RecognitionException {
		Security_statementContext _localctx = new Security_statementContext(_ctx, getState());
		enterRule(_localctx, 74, RULE_security_statement);
		try {
			setState(1142);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case EXEC:
			case EXECUTE:
				enterOuterAlt(_localctx, 1);
				{
				setState(1126);
				execute_clause();
				setState(1128);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,173,_ctx) ) {
				case 1:
					{
					setState(1127);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case REVERT:
				enterOuterAlt(_localctx, 2);
				{
				setState(1130);
				match(REVERT);
				setState(1137);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,174,_ctx) ) {
				case 1:
					{
					setState(1131);
					match(LR_BRACKET);
					setState(1132);
					match(WITH);
					setState(1133);
					match(COOKIE);
					setState(1134);
					match(EQUAL);
					setState(1135);
					match(LOCAL_ID);
					setState(1136);
					match(RR_BRACKET);
					}
					break;
				}
				setState(1140);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,175,_ctx) ) {
				case 1:
					{
					setState(1139);
					match(SEMI);
					}
					break;
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Set_statmentContext extends ParserRuleContext {
		public IdContext member_name;
		public TerminalNode SET() { return getToken(tsqlParser.SET, 0); }
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Assignment_operatorContext assignment_operator() {
			return getRuleContext(Assignment_operatorContext.class,0);
		}
		public TerminalNode CURSOR() { return getToken(tsqlParser.CURSOR, 0); }
		public Declare_set_cursor_commonContext declare_set_cursor_common() {
			return getRuleContext(Declare_set_cursor_commonContext.class,0);
		}
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode READ() { return getToken(tsqlParser.READ, 0); }
		public TerminalNode ONLY() { return getToken(tsqlParser.ONLY, 0); }
		public TerminalNode UPDATE() { return getToken(tsqlParser.UPDATE, 0); }
		public TerminalNode OF() { return getToken(tsqlParser.OF, 0); }
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public Set_specialContext set_special() {
			return getRuleContext(Set_specialContext.class,0);
		}
		public Set_statmentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_set_statment; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSet_statment(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSet_statment(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSet_statment(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Set_statmentContext set_statment() throws RecognitionException {
		Set_statmentContext _localctx = new Set_statmentContext(_ctx, getState());
		enterRule(_localctx, 76, RULE_set_statment);
		int _la;
		try {
			setState(1183);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,184,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1144);
				match(SET);
				setState(1145);
				match(LOCAL_ID);
				setState(1148);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==DOT) {
					{
					setState(1146);
					match(DOT);
					setState(1147);
					((Set_statmentContext)_localctx).member_name = id();
					}
				}

				setState(1150);
				match(EQUAL);
				setState(1151);
				expression(0);
				setState(1153);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,178,_ctx) ) {
				case 1:
					{
					setState(1152);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1155);
				match(SET);
				setState(1156);
				match(LOCAL_ID);
				setState(1157);
				assignment_operator();
				setState(1158);
				expression(0);
				setState(1160);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,179,_ctx) ) {
				case 1:
					{
					setState(1159);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1162);
				match(SET);
				setState(1163);
				match(LOCAL_ID);
				setState(1164);
				match(EQUAL);
				setState(1165);
				match(CURSOR);
				setState(1166);
				declare_set_cursor_common();
				setState(1177);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FOR) {
					{
					setState(1167);
					match(FOR);
					setState(1175);
					_errHandler.sync(this);
					switch (_input.LA(1)) {
					case READ:
						{
						setState(1168);
						match(READ);
						setState(1169);
						match(ONLY);
						}
						break;
					case UPDATE:
						{
						setState(1170);
						match(UPDATE);
						setState(1173);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if (_la==OF) {
							{
							setState(1171);
							match(OF);
							setState(1172);
							column_name_list();
							}
						}

						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					}
				}

				setState(1180);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,183,_ctx) ) {
				case 1:
					{
					setState(1179);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(1182);
				set_special();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Transaction_statementContext extends ParserRuleContext {
		public Token DELAYED_DURABILITY;
		public TerminalNode BEGIN() { return getToken(tsqlParser.BEGIN, 0); }
		public TerminalNode DISTRIBUTED() { return getToken(tsqlParser.DISTRIBUTED, 0); }
		public TerminalNode TRAN() { return getToken(tsqlParser.TRAN, 0); }
		public TerminalNode TRANSACTION() { return getToken(tsqlParser.TRANSACTION, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public TerminalNode MARK() { return getToken(tsqlParser.MARK, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public TerminalNode COMMIT() { return getToken(tsqlParser.COMMIT, 0); }
		public TerminalNode OFF() { return getToken(tsqlParser.OFF, 0); }
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public TerminalNode WORK() { return getToken(tsqlParser.WORK, 0); }
		public TerminalNode ROLLBACK() { return getToken(tsqlParser.ROLLBACK, 0); }
		public TerminalNode SAVE() { return getToken(tsqlParser.SAVE, 0); }
		public Transaction_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_transaction_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTransaction_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTransaction_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTransaction_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Transaction_statementContext transaction_statement() throws RecognitionException {
		Transaction_statementContext _localctx = new Transaction_statementContext(_ctx, getState());
		enterRule(_localctx, 78, RULE_transaction_statement);
		int _la;
		try {
			setState(1260);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,203,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1185);
				match(BEGIN);
				setState(1186);
				match(DISTRIBUTED);
				setState(1187);
				_la = _input.LA(1);
				if ( !(_la==TRAN || _la==TRANSACTION) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1190);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,185,_ctx) ) {
				case 1:
					{
					setState(1188);
					id();
					}
					break;
				case 2:
					{
					setState(1189);
					match(LOCAL_ID);
					}
					break;
				}
				setState(1193);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,186,_ctx) ) {
				case 1:
					{
					setState(1192);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1195);
				match(BEGIN);
				setState(1196);
				_la = _input.LA(1);
				if ( !(_la==TRAN || _la==TRANSACTION) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1206);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,189,_ctx) ) {
				case 1:
					{
					setState(1199);
					_errHandler.sync(this);
					switch (_input.LA(1)) {
					case FORCESEEK:
					case ABSOLUTE:
					case APPLY:
					case AUTO:
					case AVG:
					case BASE64:
					case CALLER:
					case CAST:
					case CATCH:
					case CHECKSUM_AGG:
					case COMMITTED:
					case CONCAT:
					case COOKIE:
					case COUNT:
					case COUNT_BIG:
					case DELAY:
					case DELETED:
					case DENSE_RANK:
					case DISABLE:
					case DYNAMIC:
					case ENCRYPTION:
					case EXPAND:
					case FAST:
					case FAST_FORWARD:
					case FIRST:
					case FORCE:
					case FORCED:
					case FOLLOWING:
					case FORWARD_ONLY:
					case FULLSCAN:
					case GLOBAL:
					case GO:
					case GROUPING:
					case GROUPING_ID:
					case HASH:
					case INSENSITIVE:
					case INSERTED:
					case ISOLATION:
					case KEEP:
					case KEEPFIXED:
					case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
					case KEYSET:
					case LAST:
					case LEVEL:
					case LOCAL:
					case LOCK_ESCALATION:
					case LOGIN:
					case LOOP:
					case MARK:
					case MAX:
					case MAXDOP:
					case MAXRECURSION:
					case MIN:
					case MODIFY:
					case NEXT:
					case NAME:
					case NOCOUNT:
					case NOEXPAND:
					case NORECOMPUTE:
					case NTILE:
					case NUMBER:
					case OFFSET:
					case ONLY:
					case OPTIMISTIC:
					case OPTIMIZE:
					case OUT:
					case OUTPUT:
					case OWNER:
					case PARAMETERIZATION:
					case PARTITION:
					case PATH:
					case PRECEDING:
					case PRIOR:
					case RANGE:
					case RANK:
					case READONLY:
					case READ_ONLY:
					case RECOMPILE:
					case RELATIVE:
					case REMOTE:
					case REPEATABLE:
					case ROBUST:
					case ROOT:
					case ROW:
					case ROWGUID:
					case ROWS:
					case ROW_NUMBER:
					case SAMPLE:
					case SCHEMABINDING:
					case SCROLL:
					case SCROLL_LOCKS:
					case SELF:
					case SERIALIZABLE:
					case SIMPLE:
					case SNAPSHOT:
					case SPATIAL_WINDOW_MAX_CELLS:
					case STATIC:
					case STATS_STREAM:
					case STDEV:
					case STDEVP:
					case SUM:
					case THROW:
					case TIES:
					case TIME:
					case TRY:
					case TYPE:
					case TYPE_WARNING:
					case UNBOUNDED:
					case UNCOMMITTED:
					case UNKNOWN:
					case USING:
					case VAR:
					case VARP:
					case VIEW_METADATA:
					case VIEWS:
					case WORK:
					case XML:
					case XMLNAMESPACES:
					case DOUBLE_QUOTE_ID:
					case SQUARE_BRACKET_ID:
					case ID:
						{
						setState(1197);
						id();
						}
						break;
					case LOCAL_ID:
						{
						setState(1198);
						match(LOCAL_ID);
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(1204);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,188,_ctx) ) {
					case 1:
						{
						setState(1201);
						match(WITH);
						setState(1202);
						match(MARK);
						setState(1203);
						match(STRING);
						}
						break;
					}
					}
					break;
				}
				setState(1209);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,190,_ctx) ) {
				case 1:
					{
					setState(1208);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1211);
				match(COMMIT);
				setState(1212);
				_la = _input.LA(1);
				if ( !(_la==TRAN || _la==TRANSACTION) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1223);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,193,_ctx) ) {
				case 1:
					{
					setState(1215);
					_errHandler.sync(this);
					switch (_input.LA(1)) {
					case FORCESEEK:
					case ABSOLUTE:
					case APPLY:
					case AUTO:
					case AVG:
					case BASE64:
					case CALLER:
					case CAST:
					case CATCH:
					case CHECKSUM_AGG:
					case COMMITTED:
					case CONCAT:
					case COOKIE:
					case COUNT:
					case COUNT_BIG:
					case DELAY:
					case DELETED:
					case DENSE_RANK:
					case DISABLE:
					case DYNAMIC:
					case ENCRYPTION:
					case EXPAND:
					case FAST:
					case FAST_FORWARD:
					case FIRST:
					case FORCE:
					case FORCED:
					case FOLLOWING:
					case FORWARD_ONLY:
					case FULLSCAN:
					case GLOBAL:
					case GO:
					case GROUPING:
					case GROUPING_ID:
					case HASH:
					case INSENSITIVE:
					case INSERTED:
					case ISOLATION:
					case KEEP:
					case KEEPFIXED:
					case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
					case KEYSET:
					case LAST:
					case LEVEL:
					case LOCAL:
					case LOCK_ESCALATION:
					case LOGIN:
					case LOOP:
					case MARK:
					case MAX:
					case MAXDOP:
					case MAXRECURSION:
					case MIN:
					case MODIFY:
					case NEXT:
					case NAME:
					case NOCOUNT:
					case NOEXPAND:
					case NORECOMPUTE:
					case NTILE:
					case NUMBER:
					case OFFSET:
					case ONLY:
					case OPTIMISTIC:
					case OPTIMIZE:
					case OUT:
					case OUTPUT:
					case OWNER:
					case PARAMETERIZATION:
					case PARTITION:
					case PATH:
					case PRECEDING:
					case PRIOR:
					case RANGE:
					case RANK:
					case READONLY:
					case READ_ONLY:
					case RECOMPILE:
					case RELATIVE:
					case REMOTE:
					case REPEATABLE:
					case ROBUST:
					case ROOT:
					case ROW:
					case ROWGUID:
					case ROWS:
					case ROW_NUMBER:
					case SAMPLE:
					case SCHEMABINDING:
					case SCROLL:
					case SCROLL_LOCKS:
					case SELF:
					case SERIALIZABLE:
					case SIMPLE:
					case SNAPSHOT:
					case SPATIAL_WINDOW_MAX_CELLS:
					case STATIC:
					case STATS_STREAM:
					case STDEV:
					case STDEVP:
					case SUM:
					case THROW:
					case TIES:
					case TIME:
					case TRY:
					case TYPE:
					case TYPE_WARNING:
					case UNBOUNDED:
					case UNCOMMITTED:
					case UNKNOWN:
					case USING:
					case VAR:
					case VARP:
					case VIEW_METADATA:
					case VIEWS:
					case WORK:
					case XML:
					case XMLNAMESPACES:
					case DOUBLE_QUOTE_ID:
					case SQUARE_BRACKET_ID:
					case ID:
						{
						setState(1213);
						id();
						}
						break;
					case LOCAL_ID:
						{
						setState(1214);
						match(LOCAL_ID);
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(1221);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,192,_ctx) ) {
					case 1:
						{
						setState(1217);
						match(WITH);
						setState(1218);
						match(LR_BRACKET);
						setState(1219);
						((Transaction_statementContext)_localctx).DELAYED_DURABILITY = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==OFF || _la==ON) ) {
							((Transaction_statementContext)_localctx).DELAYED_DURABILITY = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(1220);
						match(RR_BRACKET);
						}
						break;
					}
					}
					break;
				}
				setState(1226);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,194,_ctx) ) {
				case 1:
					{
					setState(1225);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(1228);
				match(COMMIT);
				setState(1230);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,195,_ctx) ) {
				case 1:
					{
					setState(1229);
					match(WORK);
					}
					break;
				}
				setState(1233);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,196,_ctx) ) {
				case 1:
					{
					setState(1232);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(1235);
				match(ROLLBACK);
				setState(1236);
				_la = _input.LA(1);
				if ( !(_la==TRAN || _la==TRANSACTION) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1239);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,197,_ctx) ) {
				case 1:
					{
					setState(1237);
					id();
					}
					break;
				case 2:
					{
					setState(1238);
					match(LOCAL_ID);
					}
					break;
				}
				setState(1242);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,198,_ctx) ) {
				case 1:
					{
					setState(1241);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(1244);
				match(ROLLBACK);
				setState(1246);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,199,_ctx) ) {
				case 1:
					{
					setState(1245);
					match(WORK);
					}
					break;
				}
				setState(1249);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,200,_ctx) ) {
				case 1:
					{
					setState(1248);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(1251);
				match(SAVE);
				setState(1252);
				_la = _input.LA(1);
				if ( !(_la==TRAN || _la==TRANSACTION) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1255);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,201,_ctx) ) {
				case 1:
					{
					setState(1253);
					id();
					}
					break;
				case 2:
					{
					setState(1254);
					match(LOCAL_ID);
					}
					break;
				}
				setState(1258);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,202,_ctx) ) {
				case 1:
					{
					setState(1257);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Go_statementContext extends ParserRuleContext {
		public Token count;
		public TerminalNode GO() { return getToken(tsqlParser.GO, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public Go_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_go_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterGo_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitGo_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitGo_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Go_statementContext go_statement() throws RecognitionException {
		Go_statementContext _localctx = new Go_statementContext(_ctx, getState());
		enterRule(_localctx, 80, RULE_go_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1262);
			match(GO);
			setState(1264);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==DECIMAL) {
				{
				setState(1263);
				((Go_statementContext)_localctx).count = match(DECIMAL);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Use_statementContext extends ParserRuleContext {
		public IdContext database;
		public TerminalNode USE() { return getToken(tsqlParser.USE, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Use_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_use_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterUse_statement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitUse_statement(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitUse_statement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Use_statementContext use_statement() throws RecognitionException {
		Use_statementContext _localctx = new Use_statementContext(_ctx, getState());
		enterRule(_localctx, 82, RULE_use_statement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1266);
			match(USE);
			setState(1267);
			((Use_statementContext)_localctx).database = id();
			setState(1269);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,205,_ctx) ) {
			case 1:
				{
				setState(1268);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute_clauseContext extends ParserRuleContext {
		public Token clause;
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public TerminalNode EXEC() { return getToken(tsqlParser.EXEC, 0); }
		public TerminalNode EXECUTE() { return getToken(tsqlParser.EXECUTE, 0); }
		public TerminalNode CALLER() { return getToken(tsqlParser.CALLER, 0); }
		public TerminalNode SELF() { return getToken(tsqlParser.SELF, 0); }
		public TerminalNode OWNER() { return getToken(tsqlParser.OWNER, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public Execute_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterExecute_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitExecute_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitExecute_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Execute_clauseContext execute_clause() throws RecognitionException {
		Execute_clauseContext _localctx = new Execute_clauseContext(_ctx, getState());
		enterRule(_localctx, 84, RULE_execute_clause);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1271);
			_la = _input.LA(1);
			if ( !(_la==EXEC || _la==EXECUTE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(1272);
			match(AS);
			setState(1273);
			((Execute_clauseContext)_localctx).clause = _input.LT(1);
			_la = _input.LA(1);
			if ( !(_la==CALLER || ((((_la - 261)) & ~0x3f) == 0 && ((1L << (_la - 261)) & ((1L << (OWNER - 261)) | (1L << (SELF - 261)) | (1L << (STRING - 261)))) != 0)) ) {
				((Execute_clauseContext)_localctx).clause = (Token)_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Declare_localContext extends ParserRuleContext {
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Data_typeContext data_type() {
			return getRuleContext(Data_typeContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Declare_localContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declare_local; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDeclare_local(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDeclare_local(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDeclare_local(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Declare_localContext declare_local() throws RecognitionException {
		Declare_localContext _localctx = new Declare_localContext(_ctx, getState());
		enterRule(_localctx, 86, RULE_declare_local);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1275);
			match(LOCAL_ID);
			setState(1277);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AS) {
				{
				setState(1276);
				match(AS);
				}
			}

			setState(1279);
			data_type();
			setState(1282);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==EQUAL) {
				{
				setState(1280);
				match(EQUAL);
				setState(1281);
				expression(0);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_type_definitionContext extends ParserRuleContext {
		public TerminalNode TABLE() { return getToken(tsqlParser.TABLE, 0); }
		public List<Column_def_table_constraintContext> column_def_table_constraint() {
			return getRuleContexts(Column_def_table_constraintContext.class);
		}
		public Column_def_table_constraintContext column_def_table_constraint(int i) {
			return getRuleContext(Column_def_table_constraintContext.class,i);
		}
		public Table_type_definitionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_type_definition; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_type_definition(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_type_definition(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_type_definition(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_type_definitionContext table_type_definition() throws RecognitionException {
		Table_type_definitionContext _localctx = new Table_type_definitionContext(_ctx, getState());
		enterRule(_localctx, 88, RULE_table_type_definition);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1284);
			match(TABLE);
			setState(1285);
			match(LR_BRACKET);
			setState(1286);
			column_def_table_constraint();
			setState(1293);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CHECK || _la==CONSTRAINT || _la==FORCESEEK || _la==PRIMARY || ((((_la - 172)) & ~0x3f) == 0 && ((1L << (_la - 172)) & ((1L << (UNIQUE - 172)) | (1L << (ABSOLUTE - 172)) | (1L << (APPLY - 172)) | (1L << (AUTO - 172)) | (1L << (AVG - 172)) | (1L << (BASE64 - 172)) | (1L << (CALLER - 172)) | (1L << (CAST - 172)) | (1L << (CATCH - 172)) | (1L << (CHECKSUM_AGG - 172)) | (1L << (COMMITTED - 172)) | (1L << (CONCAT - 172)) | (1L << (COOKIE - 172)) | (1L << (COUNT - 172)) | (1L << (COUNT_BIG - 172)) | (1L << (DELAY - 172)) | (1L << (DELETED - 172)) | (1L << (DENSE_RANK - 172)) | (1L << (DISABLE - 172)) | (1L << (DYNAMIC - 172)) | (1L << (ENCRYPTION - 172)) | (1L << (EXPAND - 172)) | (1L << (FAST - 172)) | (1L << (FAST_FORWARD - 172)) | (1L << (FIRST - 172)) | (1L << (FORCE - 172)) | (1L << (FORCED - 172)) | (1L << (FOLLOWING - 172)) | (1L << (FORWARD_ONLY - 172)) | (1L << (FULLSCAN - 172)) | (1L << (GLOBAL - 172)) | (1L << (GO - 172)) | (1L << (GROUPING - 172)) | (1L << (GROUPING_ID - 172)) | (1L << (HASH - 172)) | (1L << (INSENSITIVE - 172)) | (1L << (INSERTED - 172)) | (1L << (ISOLATION - 172)) | (1L << (KEEP - 172)) | (1L << (KEEPFIXED - 172)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 172)) | (1L << (KEYSET - 172)) | (1L << (LAST - 172)))) != 0) || ((((_la - 236)) & ~0x3f) == 0 && ((1L << (_la - 236)) & ((1L << (LEVEL - 236)) | (1L << (LOCAL - 236)) | (1L << (LOCK_ESCALATION - 236)) | (1L << (LOGIN - 236)) | (1L << (LOOP - 236)) | (1L << (MARK - 236)) | (1L << (MAX - 236)) | (1L << (MAXDOP - 236)) | (1L << (MAXRECURSION - 236)) | (1L << (MIN - 236)) | (1L << (MODIFY - 236)) | (1L << (NEXT - 236)) | (1L << (NAME - 236)) | (1L << (NOCOUNT - 236)) | (1L << (NOEXPAND - 236)) | (1L << (NORECOMPUTE - 236)) | (1L << (NTILE - 236)) | (1L << (NUMBER - 236)) | (1L << (OFFSET - 236)) | (1L << (ONLY - 236)) | (1L << (OPTIMISTIC - 236)) | (1L << (OPTIMIZE - 236)) | (1L << (OUT - 236)) | (1L << (OUTPUT - 236)) | (1L << (OWNER - 236)) | (1L << (PARAMETERIZATION - 236)) | (1L << (PARTITION - 236)) | (1L << (PATH - 236)) | (1L << (PRECEDING - 236)) | (1L << (PRIOR - 236)) | (1L << (RANGE - 236)) | (1L << (RANK - 236)) | (1L << (READONLY - 236)) | (1L << (READ_ONLY - 236)) | (1L << (RECOMPILE - 236)) | (1L << (RELATIVE - 236)) | (1L << (REMOTE - 236)) | (1L << (REPEATABLE - 236)) | (1L << (ROBUST - 236)) | (1L << (ROOT - 236)) | (1L << (ROW - 236)) | (1L << (ROWGUID - 236)) | (1L << (ROWS - 236)) | (1L << (ROW_NUMBER - 236)) | (1L << (SAMPLE - 236)) | (1L << (SCHEMABINDING - 236)) | (1L << (SCROLL - 236)) | (1L << (SCROLL_LOCKS - 236)) | (1L << (SELF - 236)) | (1L << (SERIALIZABLE - 236)) | (1L << (SIMPLE - 236)) | (1L << (SNAPSHOT - 236)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 236)) | (1L << (STATIC - 236)) | (1L << (STATS_STREAM - 236)) | (1L << (STDEV - 236)) | (1L << (STDEVP - 236)) | (1L << (SUM - 236)) | (1L << (THROW - 236)) | (1L << (TIES - 236)) | (1L << (TIME - 236)) | (1L << (TRY - 236)) | (1L << (TYPE - 236)))) != 0) || ((((_la - 300)) & ~0x3f) == 0 && ((1L << (_la - 300)) & ((1L << (TYPE_WARNING - 300)) | (1L << (UNBOUNDED - 300)) | (1L << (UNCOMMITTED - 300)) | (1L << (UNKNOWN - 300)) | (1L << (USING - 300)) | (1L << (VAR - 300)) | (1L << (VARP - 300)) | (1L << (VIEW_METADATA - 300)) | (1L << (VIEWS - 300)) | (1L << (WORK - 300)) | (1L << (XML - 300)) | (1L << (XMLNAMESPACES - 300)) | (1L << (DOUBLE_QUOTE_ID - 300)) | (1L << (SQUARE_BRACKET_ID - 300)) | (1L << (ID - 300)) | (1L << (COMMA - 300)))) != 0)) {
				{
				{
				setState(1288);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(1287);
					match(COMMA);
					}
				}

				setState(1290);
				column_def_table_constraint();
				}
				}
				setState(1295);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1296);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_def_table_constraintContext extends ParserRuleContext {
		public Column_definitionContext column_definition() {
			return getRuleContext(Column_definitionContext.class,0);
		}
		public Table_constraintContext table_constraint() {
			return getRuleContext(Table_constraintContext.class,0);
		}
		public Column_def_table_constraintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_def_table_constraint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_def_table_constraint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_def_table_constraint(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_def_table_constraint(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_def_table_constraintContext column_def_table_constraint() throws RecognitionException {
		Column_def_table_constraintContext _localctx = new Column_def_table_constraintContext(_ctx, getState());
		enterRule(_localctx, 90, RULE_column_def_table_constraint);
		try {
			setState(1300);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(1298);
				column_definition();
				}
				break;
			case CHECK:
			case CONSTRAINT:
			case PRIMARY:
			case UNIQUE:
				enterOuterAlt(_localctx, 2);
				{
				setState(1299);
				table_constraint();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_definitionContext extends ParserRuleContext {
		public IdContext constraint;
		public Token seed;
		public Token increment;
		public Column_nameContext column_name() {
			return getRuleContext(Column_nameContext.class,0);
		}
		public Data_typeContext data_type() {
			return getRuleContext(Data_typeContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode COLLATE() { return getToken(tsqlParser.COLLATE, 0); }
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Null_notnullContext null_notnull() {
			return getRuleContext(Null_notnullContext.class,0);
		}
		public TerminalNode DEFAULT() { return getToken(tsqlParser.DEFAULT, 0); }
		public Constant_expressionContext constant_expression() {
			return getRuleContext(Constant_expressionContext.class,0);
		}
		public TerminalNode IDENTITY() { return getToken(tsqlParser.IDENTITY, 0); }
		public TerminalNode ROWGUIDCOL() { return getToken(tsqlParser.ROWGUIDCOL, 0); }
		public List<Column_constraintContext> column_constraint() {
			return getRuleContexts(Column_constraintContext.class);
		}
		public Column_constraintContext column_constraint(int i) {
			return getRuleContext(Column_constraintContext.class,i);
		}
		public TerminalNode CONSTRAINT() { return getToken(tsqlParser.CONSTRAINT, 0); }
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public TerminalNode VALUES() { return getToken(tsqlParser.VALUES, 0); }
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode REPLICATION() { return getToken(tsqlParser.REPLICATION, 0); }
		public List<TerminalNode> DECIMAL() { return getTokens(tsqlParser.DECIMAL); }
		public TerminalNode DECIMAL(int i) {
			return getToken(tsqlParser.DECIMAL, i);
		}
		public Column_definitionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_definition; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_definition(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_definition(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_definition(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_definitionContext column_definition() throws RecognitionException {
		Column_definitionContext _localctx = new Column_definitionContext(_ctx, getState());
		enterRule(_localctx, 92, RULE_column_definition);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1302);
			column_name();
			setState(1306);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				{
				setState(1303);
				data_type();
				}
				break;
			case AS:
				{
				setState(1304);
				match(AS);
				setState(1305);
				expression(0);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(1310);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLLATE) {
				{
				setState(1308);
				match(COLLATE);
				setState(1309);
				id();
				}
			}

			setState(1313);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,213,_ctx) ) {
			case 1:
				{
				setState(1312);
				null_notnull();
				}
				break;
			}
			setState(1338);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,218,_ctx) ) {
			case 1:
				{
				setState(1317);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CONSTRAINT) {
					{
					setState(1315);
					match(CONSTRAINT);
					setState(1316);
					((Column_definitionContext)_localctx).constraint = id();
					}
				}

				setState(1319);
				match(DEFAULT);
				setState(1320);
				constant_expression();
				setState(1323);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,215,_ctx) ) {
				case 1:
					{
					setState(1321);
					match(WITH);
					setState(1322);
					match(VALUES);
					}
					break;
				}
				}
				break;
			case 2:
				{
				setState(1325);
				match(IDENTITY);
				setState(1331);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,216,_ctx) ) {
				case 1:
					{
					setState(1326);
					match(LR_BRACKET);
					setState(1327);
					((Column_definitionContext)_localctx).seed = match(DECIMAL);
					setState(1328);
					match(COMMA);
					setState(1329);
					((Column_definitionContext)_localctx).increment = match(DECIMAL);
					setState(1330);
					match(RR_BRACKET);
					}
					break;
				}
				setState(1336);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,217,_ctx) ) {
				case 1:
					{
					setState(1333);
					match(NOT);
					setState(1334);
					match(FOR);
					setState(1335);
					match(REPLICATION);
					}
					break;
				}
				}
				break;
			}
			setState(1341);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ROWGUIDCOL) {
				{
				setState(1340);
				match(ROWGUIDCOL);
				}
			}

			setState(1346);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,220,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1343);
					column_constraint();
					}
					} 
				}
				setState(1348);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,220,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_constraintContext extends ParserRuleContext {
		public TerminalNode CHECK() { return getToken(tsqlParser.CHECK, 0); }
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public TerminalNode CONSTRAINT() { return getToken(tsqlParser.CONSTRAINT, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Null_notnullContext null_notnull() {
			return getRuleContext(Null_notnullContext.class,0);
		}
		public TerminalNode PRIMARY() { return getToken(tsqlParser.PRIMARY, 0); }
		public TerminalNode KEY() { return getToken(tsqlParser.KEY, 0); }
		public TerminalNode UNIQUE() { return getToken(tsqlParser.UNIQUE, 0); }
		public ClusteredContext clustered() {
			return getRuleContext(ClusteredContext.class,0);
		}
		public Index_optionsContext index_options() {
			return getRuleContext(Index_optionsContext.class,0);
		}
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode REPLICATION() { return getToken(tsqlParser.REPLICATION, 0); }
		public Column_constraintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_constraint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_constraint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_constraint(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_constraint(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_constraintContext column_constraint() throws RecognitionException {
		Column_constraintContext _localctx = new Column_constraintContext(_ctx, getState());
		enterRule(_localctx, 94, RULE_column_constraint);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1351);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CONSTRAINT) {
				{
				setState(1349);
				match(CONSTRAINT);
				setState(1350);
				id();
				}
			}

			setState(1354);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NOT || _la==NULL) {
				{
				setState(1353);
				null_notnull();
				}
			}

			setState(1377);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case PRIMARY:
			case UNIQUE:
				{
				setState(1359);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case PRIMARY:
					{
					setState(1356);
					match(PRIMARY);
					setState(1357);
					match(KEY);
					}
					break;
				case UNIQUE:
					{
					setState(1358);
					match(UNIQUE);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1362);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CLUSTERED || _la==NONCLUSTERED) {
					{
					setState(1361);
					clustered();
					}
				}

				setState(1365);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,225,_ctx) ) {
				case 1:
					{
					setState(1364);
					index_options();
					}
					break;
				}
				}
				break;
			case CHECK:
				{
				setState(1367);
				match(CHECK);
				setState(1371);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NOT) {
					{
					setState(1368);
					match(NOT);
					setState(1369);
					match(FOR);
					setState(1370);
					match(REPLICATION);
					}
				}

				setState(1373);
				match(LR_BRACKET);
				setState(1374);
				search_condition();
				setState(1375);
				match(RR_BRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_constraintContext extends ParserRuleContext {
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public TerminalNode CHECK() { return getToken(tsqlParser.CHECK, 0); }
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public TerminalNode CONSTRAINT() { return getToken(tsqlParser.CONSTRAINT, 0); }
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public TerminalNode PRIMARY() { return getToken(tsqlParser.PRIMARY, 0); }
		public TerminalNode KEY() { return getToken(tsqlParser.KEY, 0); }
		public TerminalNode UNIQUE() { return getToken(tsqlParser.UNIQUE, 0); }
		public ClusteredContext clustered() {
			return getRuleContext(ClusteredContext.class,0);
		}
		public Index_optionsContext index_options() {
			return getRuleContext(Index_optionsContext.class,0);
		}
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode REPLICATION() { return getToken(tsqlParser.REPLICATION, 0); }
		public Table_constraintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_constraint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_constraint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_constraint(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_constraint(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_constraintContext table_constraint() throws RecognitionException {
		Table_constraintContext _localctx = new Table_constraintContext(_ctx, getState());
		enterRule(_localctx, 96, RULE_table_constraint);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1381);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CONSTRAINT) {
				{
				setState(1379);
				match(CONSTRAINT);
				setState(1380);
				id();
				}
			}

			setState(1411);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case PRIMARY:
			case UNIQUE:
				{
				setState(1386);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case PRIMARY:
					{
					setState(1383);
					match(PRIMARY);
					setState(1384);
					match(KEY);
					}
					break;
				case UNIQUE:
					{
					setState(1385);
					match(UNIQUE);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1389);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CLUSTERED || _la==NONCLUSTERED) {
					{
					setState(1388);
					clustered();
					}
				}

				setState(1391);
				match(LR_BRACKET);
				setState(1392);
				column_name_list();
				setState(1393);
				match(RR_BRACKET);
				setState(1395);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,231,_ctx) ) {
				case 1:
					{
					setState(1394);
					index_options();
					}
					break;
				}
				setState(1399);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ON) {
					{
					setState(1397);
					match(ON);
					setState(1398);
					id();
					}
				}

				}
				break;
			case CHECK:
				{
				setState(1401);
				match(CHECK);
				setState(1405);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NOT) {
					{
					setState(1402);
					match(NOT);
					setState(1403);
					match(FOR);
					setState(1404);
					match(REPLICATION);
					}
				}

				setState(1407);
				match(LR_BRACKET);
				setState(1408);
				search_condition();
				setState(1409);
				match(RR_BRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Index_optionsContext extends ParserRuleContext {
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public List<Index_optionContext> index_option() {
			return getRuleContexts(Index_optionContext.class);
		}
		public Index_optionContext index_option(int i) {
			return getRuleContext(Index_optionContext.class,i);
		}
		public Index_optionsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_index_options; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterIndex_options(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitIndex_options(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitIndex_options(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Index_optionsContext index_options() throws RecognitionException {
		Index_optionsContext _localctx = new Index_optionsContext(_ctx, getState());
		enterRule(_localctx, 98, RULE_index_options);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1413);
			match(WITH);
			setState(1414);
			match(LR_BRACKET);
			setState(1415);
			index_option();
			setState(1420);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1416);
				match(COMMA);
				setState(1417);
				index_option();
				}
				}
				setState(1422);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1423);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Index_optionContext extends ParserRuleContext {
		public List<Simple_idContext> simple_id() {
			return getRuleContexts(Simple_idContext.class);
		}
		public Simple_idContext simple_id(int i) {
			return getRuleContext(Simple_idContext.class,i);
		}
		public On_offContext on_off() {
			return getRuleContext(On_offContext.class,0);
		}
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public Index_optionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_index_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterIndex_option(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitIndex_option(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitIndex_option(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Index_optionContext index_option() throws RecognitionException {
		Index_optionContext _localctx = new Index_optionContext(_ctx, getState());
		enterRule(_localctx, 100, RULE_index_option);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1425);
			simple_id();
			setState(1426);
			match(EQUAL);
			setState(1430);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case ID:
				{
				setState(1427);
				simple_id();
				}
				break;
			case OFF:
			case ON:
				{
				setState(1428);
				on_off();
				}
				break;
			case DECIMAL:
				{
				setState(1429);
				match(DECIMAL);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Declare_cursorContext extends ParserRuleContext {
		public TerminalNode DECLARE() { return getToken(tsqlParser.DECLARE, 0); }
		public Cursor_nameContext cursor_name() {
			return getRuleContext(Cursor_nameContext.class,0);
		}
		public TerminalNode CURSOR() { return getToken(tsqlParser.CURSOR, 0); }
		public List<TerminalNode> FOR() { return getTokens(tsqlParser.FOR); }
		public TerminalNode FOR(int i) {
			return getToken(tsqlParser.FOR, i);
		}
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public TerminalNode INSENSITIVE() { return getToken(tsqlParser.INSENSITIVE, 0); }
		public TerminalNode SCROLL() { return getToken(tsqlParser.SCROLL, 0); }
		public TerminalNode READ() { return getToken(tsqlParser.READ, 0); }
		public TerminalNode ONLY() { return getToken(tsqlParser.ONLY, 0); }
		public TerminalNode UPDATE() { return getToken(tsqlParser.UPDATE, 0); }
		public TerminalNode OF() { return getToken(tsqlParser.OF, 0); }
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public Declare_set_cursor_commonContext declare_set_cursor_common() {
			return getRuleContext(Declare_set_cursor_commonContext.class,0);
		}
		public Declare_cursorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declare_cursor; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDeclare_cursor(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDeclare_cursor(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDeclare_cursor(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Declare_cursorContext declare_cursor() throws RecognitionException {
		Declare_cursorContext _localctx = new Declare_cursorContext(_ctx, getState());
		enterRule(_localctx, 102, RULE_declare_cursor);
		int _la;
		try {
			setState(1477);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,246,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1432);
				match(DECLARE);
				setState(1433);
				cursor_name();
				setState(1434);
				match(CURSOR);
				setState(1436);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,237,_ctx) ) {
				case 1:
					{
					setState(1435);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1438);
				match(DECLARE);
				setState(1439);
				cursor_name();
				setState(1441);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==INSENSITIVE) {
					{
					setState(1440);
					match(INSENSITIVE);
					}
				}

				setState(1444);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==SCROLL) {
					{
					setState(1443);
					match(SCROLL);
					}
				}

				setState(1446);
				match(CURSOR);
				setState(1447);
				match(FOR);
				setState(1448);
				select_statement();
				setState(1457);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FOR) {
					{
					setState(1449);
					match(FOR);
					setState(1455);
					_errHandler.sync(this);
					switch (_input.LA(1)) {
					case READ:
						{
						setState(1450);
						match(READ);
						setState(1451);
						match(ONLY);
						}
						break;
					case UPDATE:
						{
						setState(1452);
						match(UPDATE);
						}
						break;
					case OF:
						{
						{
						setState(1453);
						match(OF);
						setState(1454);
						column_name_list();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					}
				}

				setState(1460);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,242,_ctx) ) {
				case 1:
					{
					setState(1459);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1462);
				match(DECLARE);
				setState(1463);
				cursor_name();
				setState(1464);
				match(CURSOR);
				setState(1465);
				declare_set_cursor_common();
				setState(1472);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FOR) {
					{
					setState(1466);
					match(FOR);
					setState(1467);
					match(UPDATE);
					setState(1470);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==OF) {
						{
						setState(1468);
						match(OF);
						setState(1469);
						column_name_list();
						}
					}

					}
				}

				setState(1475);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,245,_ctx) ) {
				case 1:
					{
					setState(1474);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Declare_set_cursor_commonContext extends ParserRuleContext {
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public TerminalNode TYPE_WARNING() { return getToken(tsqlParser.TYPE_WARNING, 0); }
		public TerminalNode LOCAL() { return getToken(tsqlParser.LOCAL, 0); }
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public TerminalNode FORWARD_ONLY() { return getToken(tsqlParser.FORWARD_ONLY, 0); }
		public TerminalNode SCROLL() { return getToken(tsqlParser.SCROLL, 0); }
		public TerminalNode STATIC() { return getToken(tsqlParser.STATIC, 0); }
		public TerminalNode KEYSET() { return getToken(tsqlParser.KEYSET, 0); }
		public TerminalNode DYNAMIC() { return getToken(tsqlParser.DYNAMIC, 0); }
		public TerminalNode FAST_FORWARD() { return getToken(tsqlParser.FAST_FORWARD, 0); }
		public TerminalNode READ_ONLY() { return getToken(tsqlParser.READ_ONLY, 0); }
		public TerminalNode SCROLL_LOCKS() { return getToken(tsqlParser.SCROLL_LOCKS, 0); }
		public TerminalNode OPTIMISTIC() { return getToken(tsqlParser.OPTIMISTIC, 0); }
		public Declare_set_cursor_commonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declare_set_cursor_common; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDeclare_set_cursor_common(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDeclare_set_cursor_common(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDeclare_set_cursor_common(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Declare_set_cursor_commonContext declare_set_cursor_common() throws RecognitionException {
		Declare_set_cursor_commonContext _localctx = new Declare_set_cursor_commonContext(_ctx, getState());
		enterRule(_localctx, 104, RULE_declare_set_cursor_common);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1480);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==GLOBAL || _la==LOCAL) {
				{
				setState(1479);
				_la = _input.LA(1);
				if ( !(_la==GLOBAL || _la==LOCAL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(1483);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FORWARD_ONLY || _la==SCROLL) {
				{
				setState(1482);
				_la = _input.LA(1);
				if ( !(_la==FORWARD_ONLY || _la==SCROLL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(1486);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (((((_la - 212)) & ~0x3f) == 0 && ((1L << (_la - 212)) & ((1L << (DYNAMIC - 212)) | (1L << (FAST_FORWARD - 212)) | (1L << (KEYSET - 212)))) != 0) || _la==STATIC) {
				{
				setState(1485);
				_la = _input.LA(1);
				if ( !(((((_la - 212)) & ~0x3f) == 0 && ((1L << (_la - 212)) & ((1L << (DYNAMIC - 212)) | (1L << (FAST_FORWARD - 212)) | (1L << (KEYSET - 212)))) != 0) || _la==STATIC) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(1489);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (((((_la - 257)) & ~0x3f) == 0 && ((1L << (_la - 257)) & ((1L << (OPTIMISTIC - 257)) | (1L << (READ_ONLY - 257)) | (1L << (SCROLL_LOCKS - 257)))) != 0)) {
				{
				setState(1488);
				_la = _input.LA(1);
				if ( !(((((_la - 257)) & ~0x3f) == 0 && ((1L << (_la - 257)) & ((1L << (OPTIMISTIC - 257)) | (1L << (READ_ONLY - 257)) | (1L << (SCROLL_LOCKS - 257)))) != 0)) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(1492);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TYPE_WARNING) {
				{
				setState(1491);
				match(TYPE_WARNING);
				}
			}

			setState(1494);
			match(FOR);
			setState(1495);
			select_statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Fetch_cursorContext extends ParserRuleContext {
		public TerminalNode FETCH() { return getToken(tsqlParser.FETCH, 0); }
		public Cursor_nameContext cursor_name() {
			return getRuleContext(Cursor_nameContext.class,0);
		}
		public TerminalNode FROM() { return getToken(tsqlParser.FROM, 0); }
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public TerminalNode INTO() { return getToken(tsqlParser.INTO, 0); }
		public List<TerminalNode> LOCAL_ID() { return getTokens(tsqlParser.LOCAL_ID); }
		public TerminalNode LOCAL_ID(int i) {
			return getToken(tsqlParser.LOCAL_ID, i);
		}
		public TerminalNode NEXT() { return getToken(tsqlParser.NEXT, 0); }
		public TerminalNode PRIOR() { return getToken(tsqlParser.PRIOR, 0); }
		public TerminalNode FIRST() { return getToken(tsqlParser.FIRST, 0); }
		public TerminalNode LAST() { return getToken(tsqlParser.LAST, 0); }
		public TerminalNode ABSOLUTE() { return getToken(tsqlParser.ABSOLUTE, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode RELATIVE() { return getToken(tsqlParser.RELATIVE, 0); }
		public Fetch_cursorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fetch_cursor; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFetch_cursor(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFetch_cursor(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFetch_cursor(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Fetch_cursorContext fetch_cursor() throws RecognitionException {
		Fetch_cursorContext _localctx = new Fetch_cursorContext(_ctx, getState());
		enterRule(_localctx, 106, RULE_fetch_cursor);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1497);
			match(FETCH);
			setState(1509);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,253,_ctx) ) {
			case 1:
				{
				setState(1506);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case NEXT:
					{
					setState(1498);
					match(NEXT);
					}
					break;
				case PRIOR:
					{
					setState(1499);
					match(PRIOR);
					}
					break;
				case FIRST:
					{
					setState(1500);
					match(FIRST);
					}
					break;
				case LAST:
					{
					setState(1501);
					match(LAST);
					}
					break;
				case ABSOLUTE:
					{
					setState(1502);
					match(ABSOLUTE);
					setState(1503);
					expression(0);
					}
					break;
				case RELATIVE:
					{
					setState(1504);
					match(RELATIVE);
					setState(1505);
					expression(0);
					}
					break;
				case FROM:
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1508);
				match(FROM);
				}
				break;
			}
			setState(1512);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,254,_ctx) ) {
			case 1:
				{
				setState(1511);
				match(GLOBAL);
				}
				break;
			}
			setState(1514);
			cursor_name();
			setState(1524);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==INTO) {
				{
				setState(1515);
				match(INTO);
				setState(1516);
				match(LOCAL_ID);
				setState(1521);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1517);
					match(COMMA);
					setState(1518);
					match(LOCAL_ID);
					}
					}
					setState(1523);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(1527);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,257,_ctx) ) {
			case 1:
				{
				setState(1526);
				match(SEMI);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Set_specialContext extends ParserRuleContext {
		public TerminalNode SET() { return getToken(tsqlParser.SET, 0); }
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public On_offContext on_off() {
			return getRuleContext(On_offContext.class,0);
		}
		public TerminalNode TRANSACTION() { return getToken(tsqlParser.TRANSACTION, 0); }
		public TerminalNode ISOLATION() { return getToken(tsqlParser.ISOLATION, 0); }
		public TerminalNode LEVEL() { return getToken(tsqlParser.LEVEL, 0); }
		public TerminalNode READ() { return getToken(tsqlParser.READ, 0); }
		public TerminalNode UNCOMMITTED() { return getToken(tsqlParser.UNCOMMITTED, 0); }
		public TerminalNode COMMITTED() { return getToken(tsqlParser.COMMITTED, 0); }
		public TerminalNode REPEATABLE() { return getToken(tsqlParser.REPEATABLE, 0); }
		public TerminalNode SNAPSHOT() { return getToken(tsqlParser.SNAPSHOT, 0); }
		public TerminalNode SERIALIZABLE() { return getToken(tsqlParser.SERIALIZABLE, 0); }
		public TerminalNode IDENTITY_INSERT() { return getToken(tsqlParser.IDENTITY_INSERT, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public Set_specialContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_set_special; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSet_special(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSet_special(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSet_special(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Set_specialContext set_special() throws RecognitionException {
		Set_specialContext _localctx = new Set_specialContext(_ctx, getState());
		enterRule(_localctx, 108, RULE_set_special);
		try {
			setState(1564);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,263,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1529);
				match(SET);
				setState(1530);
				id();
				setState(1535);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case FORCESEEK:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case ID:
					{
					setState(1531);
					id();
					}
					break;
				case DECIMAL:
				case STRING:
				case BINARY:
				case FLOAT:
				case REAL:
				case DOLLAR:
				case PLUS:
				case MINUS:
					{
					setState(1532);
					constant();
					}
					break;
				case LOCAL_ID:
					{
					setState(1533);
					match(LOCAL_ID);
					}
					break;
				case OFF:
				case ON:
					{
					setState(1534);
					on_off();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1538);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,259,_ctx) ) {
				case 1:
					{
					setState(1537);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1540);
				match(SET);
				setState(1541);
				match(TRANSACTION);
				setState(1542);
				match(ISOLATION);
				setState(1543);
				match(LEVEL);
				setState(1552);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,260,_ctx) ) {
				case 1:
					{
					setState(1544);
					match(READ);
					setState(1545);
					match(UNCOMMITTED);
					}
					break;
				case 2:
					{
					setState(1546);
					match(READ);
					setState(1547);
					match(COMMITTED);
					}
					break;
				case 3:
					{
					setState(1548);
					match(REPEATABLE);
					setState(1549);
					match(READ);
					}
					break;
				case 4:
					{
					setState(1550);
					match(SNAPSHOT);
					}
					break;
				case 5:
					{
					setState(1551);
					match(SERIALIZABLE);
					}
					break;
				}
				setState(1555);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,261,_ctx) ) {
				case 1:
					{
					setState(1554);
					match(SEMI);
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1557);
				match(SET);
				setState(1558);
				match(IDENTITY_INSERT);
				setState(1559);
				table_name();
				setState(1560);
				on_off();
				setState(1562);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,262,_ctx) ) {
				case 1:
					{
					setState(1561);
					match(SEMI);
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionContext extends ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	 
		public ExpressionContext() { }
		public void copyFrom(ExpressionContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class Binary_operator_expressionContext extends ExpressionContext {
		public Token op;
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public Comparison_operatorContext comparison_operator() {
			return getRuleContext(Comparison_operatorContext.class,0);
		}
		public Binary_operator_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterBinary_operator_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitBinary_operator_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitBinary_operator_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Primitive_expressionContext extends ExpressionContext {
		public TerminalNode DEFAULT() { return getToken(tsqlParser.DEFAULT, 0); }
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public Primitive_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterPrimitive_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitPrimitive_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitPrimitive_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Bracket_expressionContext extends ExpressionContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Bracket_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterBracket_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitBracket_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitBracket_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Unary_operator_expressionContext extends ExpressionContext {
		public Token op;
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Unary_operator_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterUnary_operator_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitUnary_operator_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitUnary_operator_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Function_call_expressionContext extends ExpressionContext {
		public Function_callContext function_call() {
			return getRuleContext(Function_callContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode COLLATE() { return getToken(tsqlParser.COLLATE, 0); }
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Function_call_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFunction_call_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFunction_call_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFunction_call_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Case_expressionContext extends ExpressionContext {
		public Case_exprContext case_expr() {
			return getRuleContext(Case_exprContext.class,0);
		}
		public Case_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCase_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCase_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCase_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Column_ref_expressionContext extends ExpressionContext {
		public Full_column_nameContext full_column_name() {
			return getRuleContext(Full_column_nameContext.class,0);
		}
		public Column_ref_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_ref_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_ref_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_ref_expression(this);
			else return visitor.visitChildren(this);
		}
	}
	public static class Subquery_expressionContext extends ExpressionContext {
		public SubqueryContext subquery() {
			return getRuleContext(SubqueryContext.class,0);
		}
		public Subquery_expressionContext(ExpressionContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSubquery_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSubquery_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSubquery_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 110;
		enterRecursionRule(_localctx, 110, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1586);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,264,_ctx) ) {
			case 1:
				{
				_localctx = new Primitive_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(1567);
				match(DEFAULT);
				}
				break;
			case 2:
				{
				_localctx = new Primitive_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1568);
				match(NULL);
				}
				break;
			case 3:
				{
				_localctx = new Primitive_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1569);
				match(LOCAL_ID);
				}
				break;
			case 4:
				{
				_localctx = new Primitive_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1570);
				constant();
				}
				break;
			case 5:
				{
				_localctx = new Function_call_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1571);
				function_call();
				}
				break;
			case 6:
				{
				_localctx = new Case_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1572);
				case_expr();
				}
				break;
			case 7:
				{
				_localctx = new Column_ref_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1573);
				full_column_name();
				}
				break;
			case 8:
				{
				_localctx = new Bracket_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1574);
				match(LR_BRACKET);
				setState(1575);
				expression(0);
				setState(1576);
				match(RR_BRACKET);
				}
				break;
			case 9:
				{
				_localctx = new Subquery_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1578);
				match(LR_BRACKET);
				setState(1579);
				subquery();
				setState(1580);
				match(RR_BRACKET);
				}
				break;
			case 10:
				{
				_localctx = new Unary_operator_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1582);
				match(BIT_NOT);
				setState(1583);
				expression(5);
				}
				break;
			case 11:
				{
				_localctx = new Unary_operator_expressionContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(1584);
				((Unary_operator_expressionContext)_localctx).op = _input.LT(1);
				_la = _input.LA(1);
				if ( !(_la==PLUS || _la==MINUS) ) {
					((Unary_operator_expressionContext)_localctx).op = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1585);
				expression(3);
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(1603);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,266,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(1601);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,265,_ctx) ) {
					case 1:
						{
						_localctx = new Binary_operator_expressionContext(new ExpressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(1588);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(1589);
						((Binary_operator_expressionContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !(((((_la - 347)) & ~0x3f) == 0 && ((1L << (_la - 347)) & ((1L << (STAR - 347)) | (1L << (DIVIDE - 347)) | (1L << (MODULE - 347)))) != 0)) ) {
							((Binary_operator_expressionContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(1590);
						expression(5);
						}
						break;
					case 2:
						{
						_localctx = new Binary_operator_expressionContext(new ExpressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(1591);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(1592);
						((Binary_operator_expressionContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !(((((_la - 350)) & ~0x3f) == 0 && ((1L << (_la - 350)) & ((1L << (PLUS - 350)) | (1L << (MINUS - 350)) | (1L << (BIT_OR - 350)) | (1L << (BIT_AND - 350)) | (1L << (BIT_XOR - 350)))) != 0)) ) {
							((Binary_operator_expressionContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(1593);
						expression(3);
						}
						break;
					case 3:
						{
						_localctx = new Binary_operator_expressionContext(new ExpressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(1594);
						if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
						setState(1595);
						comparison_operator();
						setState(1596);
						expression(2);
						}
						break;
					case 4:
						{
						_localctx = new Function_call_expressionContext(new ExpressionContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(1598);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(1599);
						match(COLLATE);
						setState(1600);
						id();
						}
						break;
					}
					} 
				}
				setState(1605);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,266,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class Constant_expressionContext extends ParserRuleContext {
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public Function_callContext function_call() {
			return getRuleContext(Function_callContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Constant_expressionContext constant_expression() {
			return getRuleContext(Constant_expressionContext.class,0);
		}
		public Constant_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constant_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterConstant_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitConstant_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitConstant_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Constant_expressionContext constant_expression() throws RecognitionException {
		Constant_expressionContext _localctx = new Constant_expressionContext(_ctx, getState());
		enterRule(_localctx, 112, RULE_constant_expression);
		try {
			setState(1614);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NULL:
				enterOuterAlt(_localctx, 1);
				{
				setState(1606);
				match(NULL);
				}
				break;
			case DECIMAL:
			case STRING:
			case BINARY:
			case FLOAT:
			case REAL:
			case DOLLAR:
			case PLUS:
			case MINUS:
				enterOuterAlt(_localctx, 2);
				{
				setState(1607);
				constant();
				}
				break;
			case COALESCE:
			case CONVERT:
			case CURRENT_TIMESTAMP:
			case CURRENT_USER:
			case FORCESEEK:
			case IDENTITY:
			case LEFT:
			case NULLIF:
			case RIGHT:
			case SESSION_USER:
			case SYSTEM_USER:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case BINARY_CHECKSUM:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DATEADD:
			case DATEDIFF:
			case DATENAME:
			case DATEPART:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MIN_ACTIVE_ROWVERSION:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 3);
				{
				setState(1608);
				function_call();
				}
				break;
			case LOCAL_ID:
				enterOuterAlt(_localctx, 4);
				{
				setState(1609);
				match(LOCAL_ID);
				}
				break;
			case LR_BRACKET:
				enterOuterAlt(_localctx, 5);
				{
				setState(1610);
				match(LR_BRACKET);
				setState(1611);
				constant_expression();
				setState(1612);
				match(RR_BRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SubqueryContext extends ParserRuleContext {
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public SubqueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_subquery; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSubquery(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSubquery(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSubquery(this);
			else return visitor.visitChildren(this);
		}
	}

	public final SubqueryContext subquery() throws RecognitionException {
		SubqueryContext _localctx = new SubqueryContext(_ctx, getState());
		enterRule(_localctx, 114, RULE_subquery);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1616);
			select_statement();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class With_expressionContext extends ParserRuleContext {
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public List<Common_table_expressionContext> common_table_expression() {
			return getRuleContexts(Common_table_expressionContext.class);
		}
		public Common_table_expressionContext common_table_expression(int i) {
			return getRuleContext(Common_table_expressionContext.class,i);
		}
		public TerminalNode XMLNAMESPACES() { return getToken(tsqlParser.XMLNAMESPACES, 0); }
		public With_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_with_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWith_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWith_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWith_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final With_expressionContext with_expression() throws RecognitionException {
		With_expressionContext _localctx = new With_expressionContext(_ctx, getState());
		enterRule(_localctx, 116, RULE_with_expression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1618);
			match(WITH);
			setState(1621);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,268,_ctx) ) {
			case 1:
				{
				setState(1619);
				match(XMLNAMESPACES);
				setState(1620);
				match(COMMA);
				}
				break;
			}
			setState(1623);
			common_table_expression();
			setState(1628);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1624);
				match(COMMA);
				setState(1625);
				common_table_expression();
				}
				}
				setState(1630);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Common_table_expressionContext extends ParserRuleContext {
		public IdContext expression_name;
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Select_statementContext select_statement() {
			return getRuleContext(Select_statementContext.class,0);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Column_name_listContext column_name_list() {
			return getRuleContext(Column_name_listContext.class,0);
		}
		public Common_table_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_common_table_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCommon_table_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCommon_table_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCommon_table_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Common_table_expressionContext common_table_expression() throws RecognitionException {
		Common_table_expressionContext _localctx = new Common_table_expressionContext(_ctx, getState());
		enterRule(_localctx, 118, RULE_common_table_expression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1631);
			((Common_table_expressionContext)_localctx).expression_name = id();
			setState(1636);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LR_BRACKET) {
				{
				setState(1632);
				match(LR_BRACKET);
				setState(1633);
				column_name_list();
				setState(1634);
				match(RR_BRACKET);
				}
			}

			setState(1638);
			match(AS);
			setState(1639);
			match(LR_BRACKET);
			setState(1640);
			select_statement();
			setState(1641);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Update_elemContext extends ParserRuleContext {
		public IdContext udt_column_name;
		public IdContext method_name;
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Full_column_nameContext full_column_name() {
			return getRuleContext(Full_column_nameContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Assignment_operatorContext assignment_operator() {
			return getRuleContext(Assignment_operatorContext.class,0);
		}
		public Expression_listContext expression_list() {
			return getRuleContext(Expression_listContext.class,0);
		}
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Update_elemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_update_elem; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterUpdate_elem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitUpdate_elem(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitUpdate_elem(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Update_elemContext update_elem() throws RecognitionException {
		Update_elemContext _localctx = new Update_elemContext(_ctx, getState());
		enterRule(_localctx, 120, RULE_update_elem);
		try {
			setState(1659);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,273,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1645);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case FORCESEEK:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case ID:
					{
					setState(1643);
					full_column_name();
					}
					break;
				case LOCAL_ID:
					{
					setState(1644);
					match(LOCAL_ID);
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1649);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case EQUAL:
					{
					setState(1647);
					match(EQUAL);
					}
					break;
				case PLUS_ASSIGN:
				case MINUS_ASSIGN:
				case MULT_ASSIGN:
				case DIV_ASSIGN:
				case MOD_ASSIGN:
				case AND_ASSIGN:
				case XOR_ASSIGN:
				case OR_ASSIGN:
					{
					setState(1648);
					assignment_operator();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(1651);
				expression(0);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1652);
				((Update_elemContext)_localctx).udt_column_name = id();
				setState(1653);
				match(DOT);
				setState(1654);
				((Update_elemContext)_localctx).method_name = id();
				setState(1655);
				match(LR_BRACKET);
				setState(1656);
				expression_list();
				setState(1657);
				match(RR_BRACKET);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Search_condition_listContext extends ParserRuleContext {
		public List<Search_conditionContext> search_condition() {
			return getRuleContexts(Search_conditionContext.class);
		}
		public Search_conditionContext search_condition(int i) {
			return getRuleContext(Search_conditionContext.class,i);
		}
		public Search_condition_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_search_condition_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSearch_condition_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSearch_condition_list(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSearch_condition_list(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Search_condition_listContext search_condition_list() throws RecognitionException {
		Search_condition_listContext _localctx = new Search_condition_listContext(_ctx, getState());
		enterRule(_localctx, 122, RULE_search_condition_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1661);
			search_condition();
			setState(1666);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1662);
				match(COMMA);
				setState(1663);
				search_condition();
				}
				}
				setState(1668);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Search_conditionContext extends ParserRuleContext {
		public List<Search_condition_orContext> search_condition_or() {
			return getRuleContexts(Search_condition_orContext.class);
		}
		public Search_condition_orContext search_condition_or(int i) {
			return getRuleContext(Search_condition_orContext.class,i);
		}
		public List<TerminalNode> AND() { return getTokens(tsqlParser.AND); }
		public TerminalNode AND(int i) {
			return getToken(tsqlParser.AND, i);
		}
		public Search_conditionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_search_condition; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSearch_condition(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSearch_condition(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSearch_condition(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Search_conditionContext search_condition() throws RecognitionException {
		Search_conditionContext _localctx = new Search_conditionContext(_ctx, getState());
		enterRule(_localctx, 124, RULE_search_condition);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1669);
			search_condition_or();
			setState(1674);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==AND) {
				{
				{
				setState(1670);
				match(AND);
				setState(1671);
				search_condition_or();
				}
				}
				setState(1676);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Search_condition_orContext extends ParserRuleContext {
		public List<Search_condition_notContext> search_condition_not() {
			return getRuleContexts(Search_condition_notContext.class);
		}
		public Search_condition_notContext search_condition_not(int i) {
			return getRuleContext(Search_condition_notContext.class,i);
		}
		public List<TerminalNode> OR() { return getTokens(tsqlParser.OR); }
		public TerminalNode OR(int i) {
			return getToken(tsqlParser.OR, i);
		}
		public Search_condition_orContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_search_condition_or; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSearch_condition_or(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSearch_condition_or(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSearch_condition_or(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Search_condition_orContext search_condition_or() throws RecognitionException {
		Search_condition_orContext _localctx = new Search_condition_orContext(_ctx, getState());
		enterRule(_localctx, 126, RULE_search_condition_or);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1677);
			search_condition_not();
			setState(1682);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==OR) {
				{
				{
				setState(1678);
				match(OR);
				setState(1679);
				search_condition_not();
				}
				}
				setState(1684);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Search_condition_notContext extends ParserRuleContext {
		public PredicateContext predicate() {
			return getRuleContext(PredicateContext.class,0);
		}
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public Search_condition_notContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_search_condition_not; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSearch_condition_not(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSearch_condition_not(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSearch_condition_not(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Search_condition_notContext search_condition_not() throws RecognitionException {
		Search_condition_notContext _localctx = new Search_condition_notContext(_ctx, getState());
		enterRule(_localctx, 128, RULE_search_condition_not);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1686);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NOT) {
				{
				setState(1685);
				match(NOT);
				}
			}

			setState(1688);
			predicate();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PredicateContext extends ParserRuleContext {
		public TerminalNode EXISTS() { return getToken(tsqlParser.EXISTS, 0); }
		public SubqueryContext subquery() {
			return getRuleContext(SubqueryContext.class,0);
		}
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public Comparison_operatorContext comparison_operator() {
			return getRuleContext(Comparison_operatorContext.class,0);
		}
		public TerminalNode ALL() { return getToken(tsqlParser.ALL, 0); }
		public TerminalNode SOME() { return getToken(tsqlParser.SOME, 0); }
		public TerminalNode ANY() { return getToken(tsqlParser.ANY, 0); }
		public TerminalNode BETWEEN() { return getToken(tsqlParser.BETWEEN, 0); }
		public TerminalNode AND() { return getToken(tsqlParser.AND, 0); }
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public TerminalNode IN() { return getToken(tsqlParser.IN, 0); }
		public Expression_listContext expression_list() {
			return getRuleContext(Expression_listContext.class,0);
		}
		public TerminalNode LIKE() { return getToken(tsqlParser.LIKE, 0); }
		public TerminalNode ESCAPE() { return getToken(tsqlParser.ESCAPE, 0); }
		public TerminalNode IS() { return getToken(tsqlParser.IS, 0); }
		public Null_notnullContext null_notnull() {
			return getRuleContext(Null_notnullContext.class,0);
		}
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public PredicateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_predicate; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterPredicate(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitPredicate(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitPredicate(this);
			else return visitor.visitChildren(this);
		}
	}

	public final PredicateContext predicate() throws RecognitionException {
		PredicateContext _localctx = new PredicateContext(_ctx, getState());
		enterRule(_localctx, 130, RULE_predicate);
		int _la;
		try {
			setState(1745);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,283,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1690);
				match(EXISTS);
				setState(1691);
				match(LR_BRACKET);
				setState(1692);
				subquery();
				setState(1693);
				match(RR_BRACKET);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1695);
				expression(0);
				setState(1696);
				comparison_operator();
				setState(1697);
				expression(0);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1699);
				expression(0);
				setState(1700);
				comparison_operator();
				setState(1701);
				_la = _input.LA(1);
				if ( !(_la==ALL || _la==ANY || _la==SOME) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1702);
				match(LR_BRACKET);
				setState(1703);
				subquery();
				setState(1704);
				match(RR_BRACKET);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(1706);
				expression(0);
				setState(1708);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NOT) {
					{
					setState(1707);
					match(NOT);
					}
				}

				setState(1710);
				match(BETWEEN);
				setState(1711);
				expression(0);
				setState(1712);
				match(AND);
				setState(1713);
				expression(0);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(1715);
				expression(0);
				setState(1717);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NOT) {
					{
					setState(1716);
					match(NOT);
					}
				}

				setState(1719);
				match(IN);
				setState(1720);
				match(LR_BRACKET);
				setState(1723);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,280,_ctx) ) {
				case 1:
					{
					setState(1721);
					subquery();
					}
					break;
				case 2:
					{
					setState(1722);
					expression_list();
					}
					break;
				}
				setState(1725);
				match(RR_BRACKET);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(1727);
				expression(0);
				setState(1729);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NOT) {
					{
					setState(1728);
					match(NOT);
					}
				}

				setState(1731);
				match(LIKE);
				setState(1732);
				expression(0);
				setState(1735);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ESCAPE) {
					{
					setState(1733);
					match(ESCAPE);
					setState(1734);
					expression(0);
					}
				}

				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(1737);
				expression(0);
				setState(1738);
				match(IS);
				setState(1739);
				null_notnull();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(1741);
				match(LR_BRACKET);
				setState(1742);
				search_condition();
				setState(1743);
				match(RR_BRACKET);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Query_expressionContext extends ParserRuleContext {
		public Query_specificationContext query_specification() {
			return getRuleContext(Query_specificationContext.class,0);
		}
		public Query_expressionContext query_expression() {
			return getRuleContext(Query_expressionContext.class,0);
		}
		public List<UnionContext> union() {
			return getRuleContexts(UnionContext.class);
		}
		public UnionContext union(int i) {
			return getRuleContext(UnionContext.class,i);
		}
		public Query_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterQuery_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitQuery_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitQuery_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Query_expressionContext query_expression() throws RecognitionException {
		Query_expressionContext _localctx = new Query_expressionContext(_ctx, getState());
		enterRule(_localctx, 132, RULE_query_expression);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1752);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SELECT:
				{
				setState(1747);
				query_specification();
				}
				break;
			case LR_BRACKET:
				{
				setState(1748);
				match(LR_BRACKET);
				setState(1749);
				query_expression();
				setState(1750);
				match(RR_BRACKET);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(1757);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,285,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1754);
					union();
					}
					} 
				}
				setState(1759);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,285,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class UnionContext extends ParserRuleContext {
		public TerminalNode UNION() { return getToken(tsqlParser.UNION, 0); }
		public TerminalNode EXCEPT() { return getToken(tsqlParser.EXCEPT, 0); }
		public TerminalNode INTERSECT() { return getToken(tsqlParser.INTERSECT, 0); }
		public Query_specificationContext query_specification() {
			return getRuleContext(Query_specificationContext.class,0);
		}
		public TerminalNode ALL() { return getToken(tsqlParser.ALL, 0); }
		public List<Query_expressionContext> query_expression() {
			return getRuleContexts(Query_expressionContext.class);
		}
		public Query_expressionContext query_expression(int i) {
			return getRuleContext(Query_expressionContext.class,i);
		}
		public UnionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_union; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterUnion(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitUnion(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitUnion(this);
			else return visitor.visitChildren(this);
		}
	}

	public final UnionContext union() throws RecognitionException {
		UnionContext _localctx = new UnionContext(_ctx, getState());
		enterRule(_localctx, 134, RULE_union);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1766);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case UNION:
				{
				setState(1760);
				match(UNION);
				setState(1762);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ALL) {
					{
					setState(1761);
					match(ALL);
					}
				}

				}
				break;
			case EXCEPT:
				{
				setState(1764);
				match(EXCEPT);
				}
				break;
			case INTERSECT:
				{
				setState(1765);
				match(INTERSECT);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(1777);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SELECT:
				{
				setState(1768);
				query_specification();
				}
				break;
			case LR_BRACKET:
				{
				setState(1773); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(1769);
						match(LR_BRACKET);
						setState(1770);
						query_expression();
						setState(1771);
						match(RR_BRACKET);
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(1775); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,288,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Query_specificationContext extends ParserRuleContext {
		public Table_nameContext into_table;
		public Search_conditionContext where;
		public Search_conditionContext having;
		public TerminalNode SELECT() { return getToken(tsqlParser.SELECT, 0); }
		public Select_listContext select_list() {
			return getRuleContext(Select_listContext.class,0);
		}
		public TerminalNode TOP() { return getToken(tsqlParser.TOP, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode INTO() { return getToken(tsqlParser.INTO, 0); }
		public TerminalNode FROM() { return getToken(tsqlParser.FROM, 0); }
		public List<Table_sourceContext> table_source() {
			return getRuleContexts(Table_sourceContext.class);
		}
		public Table_sourceContext table_source(int i) {
			return getRuleContext(Table_sourceContext.class,i);
		}
		public TerminalNode WHERE() { return getToken(tsqlParser.WHERE, 0); }
		public TerminalNode GROUP() { return getToken(tsqlParser.GROUP, 0); }
		public TerminalNode BY() { return getToken(tsqlParser.BY, 0); }
		public List<Group_by_itemContext> group_by_item() {
			return getRuleContexts(Group_by_itemContext.class);
		}
		public Group_by_itemContext group_by_item(int i) {
			return getRuleContext(Group_by_itemContext.class,i);
		}
		public TerminalNode HAVING() { return getToken(tsqlParser.HAVING, 0); }
		public TerminalNode ALL() { return getToken(tsqlParser.ALL, 0); }
		public TerminalNode DISTINCT() { return getToken(tsqlParser.DISTINCT, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public List<Search_conditionContext> search_condition() {
			return getRuleContexts(Search_conditionContext.class);
		}
		public Search_conditionContext search_condition(int i) {
			return getRuleContext(Search_conditionContext.class,i);
		}
		public TerminalNode PERCENT() { return getToken(tsqlParser.PERCENT, 0); }
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public TerminalNode TIES() { return getToken(tsqlParser.TIES, 0); }
		public Query_specificationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query_specification; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterQuery_specification(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitQuery_specification(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitQuery_specification(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Query_specificationContext query_specification() throws RecognitionException {
		Query_specificationContext _localctx = new Query_specificationContext(_ctx, getState());
		enterRule(_localctx, 136, RULE_query_specification);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1779);
			match(SELECT);
			setState(1781);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ALL || _la==DISTINCT) {
				{
				setState(1780);
				_la = _input.LA(1);
				if ( !(_la==ALL || _la==DISTINCT) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(1792);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TOP) {
				{
				setState(1783);
				match(TOP);
				setState(1784);
				expression(0);
				setState(1786);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PERCENT) {
					{
					setState(1785);
					match(PERCENT);
					}
				}

				setState(1790);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==WITH) {
					{
					setState(1788);
					match(WITH);
					setState(1789);
					match(TIES);
					}
				}

				}
			}

			setState(1794);
			select_list();
			setState(1797);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==INTO) {
				{
				setState(1795);
				match(INTO);
				setState(1796);
				((Query_specificationContext)_localctx).into_table = table_name();
				}
			}

			setState(1808);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==FROM) {
				{
				setState(1799);
				match(FROM);
				setState(1800);
				table_source();
				setState(1805);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,295,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(1801);
						match(COMMA);
						setState(1802);
						table_source();
						}
						} 
					}
					setState(1807);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,295,_ctx);
				}
				}
			}

			setState(1812);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,297,_ctx) ) {
			case 1:
				{
				setState(1810);
				match(WHERE);
				setState(1811);
				((Query_specificationContext)_localctx).where = search_condition();
				}
				break;
			}
			setState(1824);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,299,_ctx) ) {
			case 1:
				{
				setState(1814);
				match(GROUP);
				setState(1815);
				match(BY);
				setState(1816);
				group_by_item();
				setState(1821);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,298,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(1817);
						match(COMMA);
						setState(1818);
						group_by_item();
						}
						} 
					}
					setState(1823);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,298,_ctx);
				}
				}
				break;
			}
			setState(1828);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,300,_ctx) ) {
			case 1:
				{
				setState(1826);
				match(HAVING);
				setState(1827);
				((Query_specificationContext)_localctx).having = search_condition();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Order_by_clauseContext extends ParserRuleContext {
		public TerminalNode ORDER() { return getToken(tsqlParser.ORDER, 0); }
		public TerminalNode BY() { return getToken(tsqlParser.BY, 0); }
		public List<Order_by_expressionContext> order_by_expression() {
			return getRuleContexts(Order_by_expressionContext.class);
		}
		public Order_by_expressionContext order_by_expression(int i) {
			return getRuleContext(Order_by_expressionContext.class,i);
		}
		public TerminalNode OFFSET() { return getToken(tsqlParser.OFFSET, 0); }
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public List<TerminalNode> ROW() { return getTokens(tsqlParser.ROW); }
		public TerminalNode ROW(int i) {
			return getToken(tsqlParser.ROW, i);
		}
		public List<TerminalNode> ROWS() { return getTokens(tsqlParser.ROWS); }
		public TerminalNode ROWS(int i) {
			return getToken(tsqlParser.ROWS, i);
		}
		public TerminalNode FETCH() { return getToken(tsqlParser.FETCH, 0); }
		public TerminalNode ONLY() { return getToken(tsqlParser.ONLY, 0); }
		public TerminalNode FIRST() { return getToken(tsqlParser.FIRST, 0); }
		public TerminalNode NEXT() { return getToken(tsqlParser.NEXT, 0); }
		public Order_by_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_order_by_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOrder_by_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOrder_by_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOrder_by_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Order_by_clauseContext order_by_clause() throws RecognitionException {
		Order_by_clauseContext _localctx = new Order_by_clauseContext(_ctx, getState());
		enterRule(_localctx, 138, RULE_order_by_clause);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1830);
			match(ORDER);
			setState(1831);
			match(BY);
			setState(1832);
			order_by_expression();
			setState(1837);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,301,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1833);
					match(COMMA);
					setState(1834);
					order_by_expression();
					}
					} 
				}
				setState(1839);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,301,_ctx);
			}
			setState(1851);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,303,_ctx) ) {
			case 1:
				{
				setState(1840);
				match(OFFSET);
				setState(1841);
				expression(0);
				setState(1842);
				_la = _input.LA(1);
				if ( !(_la==ROW || _la==ROWS) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1849);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,302,_ctx) ) {
				case 1:
					{
					setState(1843);
					match(FETCH);
					setState(1844);
					_la = _input.LA(1);
					if ( !(_la==FIRST || _la==NEXT) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(1845);
					expression(0);
					setState(1846);
					_la = _input.LA(1);
					if ( !(_la==ROW || _la==ROWS) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(1847);
					match(ONLY);
					}
					break;
				}
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class For_clauseContext extends ParserRuleContext {
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public TerminalNode BROWSE() { return getToken(tsqlParser.BROWSE, 0); }
		public TerminalNode XML() { return getToken(tsqlParser.XML, 0); }
		public TerminalNode AUTO() { return getToken(tsqlParser.AUTO, 0); }
		public Xml_common_directivesContext xml_common_directives() {
			return getRuleContext(Xml_common_directivesContext.class,0);
		}
		public TerminalNode PATH() { return getToken(tsqlParser.PATH, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public For_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_for_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFor_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFor_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFor_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final For_clauseContext for_clause() throws RecognitionException {
		For_clauseContext _localctx = new For_clauseContext(_ctx, getState());
		enterRule(_localctx, 140, RULE_for_clause);
		try {
			setState(1872);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,307,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1853);
				match(FOR);
				setState(1854);
				match(BROWSE);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1855);
				match(FOR);
				setState(1856);
				match(XML);
				setState(1857);
				match(AUTO);
				setState(1859);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,304,_ctx) ) {
				case 1:
					{
					setState(1858);
					xml_common_directives();
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1861);
				match(FOR);
				setState(1862);
				match(XML);
				setState(1863);
				match(PATH);
				setState(1867);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,305,_ctx) ) {
				case 1:
					{
					setState(1864);
					match(LR_BRACKET);
					setState(1865);
					match(STRING);
					setState(1866);
					match(RR_BRACKET);
					}
					break;
				}
				setState(1870);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,306,_ctx) ) {
				case 1:
					{
					setState(1869);
					xml_common_directives();
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Xml_common_directivesContext extends ParserRuleContext {
		public TerminalNode BINARY() { return getToken(tsqlParser.BINARY, 0); }
		public TerminalNode BASE64() { return getToken(tsqlParser.BASE64, 0); }
		public TerminalNode TYPE() { return getToken(tsqlParser.TYPE, 0); }
		public TerminalNode ROOT() { return getToken(tsqlParser.ROOT, 0); }
		public Xml_common_directivesContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_xml_common_directives; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterXml_common_directives(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitXml_common_directives(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitXml_common_directives(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Xml_common_directivesContext xml_common_directives() throws RecognitionException {
		Xml_common_directivesContext _localctx = new Xml_common_directivesContext(_ctx, getState());
		enterRule(_localctx, 142, RULE_xml_common_directives);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1874);
			match(COMMA);
			setState(1879);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case BINARY:
				{
				setState(1875);
				match(BINARY);
				setState(1876);
				match(BASE64);
				}
				break;
			case TYPE:
				{
				setState(1877);
				match(TYPE);
				}
				break;
			case ROOT:
				{
				setState(1878);
				match(ROOT);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Order_by_expressionContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode ASC() { return getToken(tsqlParser.ASC, 0); }
		public TerminalNode DESC() { return getToken(tsqlParser.DESC, 0); }
		public Order_by_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_order_by_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOrder_by_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOrder_by_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOrder_by_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Order_by_expressionContext order_by_expression() throws RecognitionException {
		Order_by_expressionContext _localctx = new Order_by_expressionContext(_ctx, getState());
		enterRule(_localctx, 144, RULE_order_by_expression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1881);
			expression(0);
			setState(1883);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ASC || _la==DESC) {
				{
				setState(1882);
				_la = _input.LA(1);
				if ( !(_la==ASC || _la==DESC) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Group_by_itemContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public Group_by_itemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_group_by_item; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterGroup_by_item(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitGroup_by_item(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitGroup_by_item(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Group_by_itemContext group_by_item() throws RecognitionException {
		Group_by_itemContext _localctx = new Group_by_itemContext(_ctx, getState());
		enterRule(_localctx, 146, RULE_group_by_item);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1885);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Option_clauseContext extends ParserRuleContext {
		public TerminalNode OPTION() { return getToken(tsqlParser.OPTION, 0); }
		public List<OptionContext> option() {
			return getRuleContexts(OptionContext.class);
		}
		public OptionContext option(int i) {
			return getRuleContext(OptionContext.class,i);
		}
		public Option_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_option_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOption_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOption_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOption_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Option_clauseContext option_clause() throws RecognitionException {
		Option_clauseContext _localctx = new Option_clauseContext(_ctx, getState());
		enterRule(_localctx, 148, RULE_option_clause);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1887);
			match(OPTION);
			setState(1888);
			match(LR_BRACKET);
			setState(1889);
			option();
			setState(1894);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(1890);
				match(COMMA);
				setState(1891);
				option();
				}
				}
				setState(1896);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(1897);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class OptionContext extends ParserRuleContext {
		public Token number_rows;
		public Token number_of_processors;
		public Token number_recursion;
		public TerminalNode FAST() { return getToken(tsqlParser.FAST, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode GROUP() { return getToken(tsqlParser.GROUP, 0); }
		public TerminalNode HASH() { return getToken(tsqlParser.HASH, 0); }
		public TerminalNode ORDER() { return getToken(tsqlParser.ORDER, 0); }
		public TerminalNode UNION() { return getToken(tsqlParser.UNION, 0); }
		public TerminalNode MERGE() { return getToken(tsqlParser.MERGE, 0); }
		public TerminalNode CONCAT() { return getToken(tsqlParser.CONCAT, 0); }
		public TerminalNode JOIN() { return getToken(tsqlParser.JOIN, 0); }
		public TerminalNode LOOP() { return getToken(tsqlParser.LOOP, 0); }
		public TerminalNode EXPAND() { return getToken(tsqlParser.EXPAND, 0); }
		public TerminalNode VIEWS() { return getToken(tsqlParser.VIEWS, 0); }
		public TerminalNode FORCE() { return getToken(tsqlParser.FORCE, 0); }
		public TerminalNode IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX() { return getToken(tsqlParser.IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX, 0); }
		public TerminalNode KEEP() { return getToken(tsqlParser.KEEP, 0); }
		public TerminalNode PLAN() { return getToken(tsqlParser.PLAN, 0); }
		public TerminalNode KEEPFIXED() { return getToken(tsqlParser.KEEPFIXED, 0); }
		public TerminalNode MAXDOP() { return getToken(tsqlParser.MAXDOP, 0); }
		public TerminalNode MAXRECURSION() { return getToken(tsqlParser.MAXRECURSION, 0); }
		public TerminalNode OPTIMIZE() { return getToken(tsqlParser.OPTIMIZE, 0); }
		public TerminalNode FOR() { return getToken(tsqlParser.FOR, 0); }
		public List<Optimize_for_argContext> optimize_for_arg() {
			return getRuleContexts(Optimize_for_argContext.class);
		}
		public Optimize_for_argContext optimize_for_arg(int i) {
			return getRuleContext(Optimize_for_argContext.class,i);
		}
		public TerminalNode UNKNOWN() { return getToken(tsqlParser.UNKNOWN, 0); }
		public TerminalNode PARAMETERIZATION() { return getToken(tsqlParser.PARAMETERIZATION, 0); }
		public TerminalNode SIMPLE() { return getToken(tsqlParser.SIMPLE, 0); }
		public TerminalNode FORCED() { return getToken(tsqlParser.FORCED, 0); }
		public TerminalNode RECOMPILE() { return getToken(tsqlParser.RECOMPILE, 0); }
		public TerminalNode ROBUST() { return getToken(tsqlParser.ROBUST, 0); }
		public TerminalNode USE() { return getToken(tsqlParser.USE, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public OptionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOption(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOption(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOption(this);
			else return visitor.visitChildren(this);
		}
	}

	public final OptionContext option() throws RecognitionException {
		OptionContext _localctx = new OptionContext(_ctx, getState());
		enterRule(_localctx, 150, RULE_option);
		int _la;
		try {
			setState(1944);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,312,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1899);
				match(FAST);
				setState(1900);
				((OptionContext)_localctx).number_rows = match(DECIMAL);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1901);
				_la = _input.LA(1);
				if ( !(_la==ORDER || _la==HASH) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1902);
				match(GROUP);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1903);
				_la = _input.LA(1);
				if ( !(_la==MERGE || _la==CONCAT || _la==HASH) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1904);
				match(UNION);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(1905);
				_la = _input.LA(1);
				if ( !(_la==MERGE || _la==HASH || _la==LOOP) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(1906);
				match(JOIN);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(1907);
				match(EXPAND);
				setState(1908);
				match(VIEWS);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(1909);
				match(FORCE);
				setState(1910);
				match(ORDER);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(1911);
				match(IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX);
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(1912);
				match(KEEP);
				setState(1913);
				match(PLAN);
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(1914);
				match(KEEPFIXED);
				setState(1915);
				match(PLAN);
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(1916);
				match(MAXDOP);
				setState(1917);
				((OptionContext)_localctx).number_of_processors = match(DECIMAL);
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(1918);
				match(MAXRECURSION);
				setState(1919);
				((OptionContext)_localctx).number_recursion = match(DECIMAL);
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(1920);
				match(OPTIMIZE);
				setState(1921);
				match(FOR);
				setState(1922);
				match(LR_BRACKET);
				setState(1923);
				optimize_for_arg();
				setState(1928);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(1924);
					match(COMMA);
					setState(1925);
					optimize_for_arg();
					}
					}
					setState(1930);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(1931);
				match(RR_BRACKET);
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(1933);
				match(OPTIMIZE);
				setState(1934);
				match(FOR);
				setState(1935);
				match(UNKNOWN);
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(1936);
				match(PARAMETERIZATION);
				setState(1937);
				_la = _input.LA(1);
				if ( !(_la==FORCED || _la==SIMPLE) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(1938);
				match(RECOMPILE);
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(1939);
				match(ROBUST);
				setState(1940);
				match(PLAN);
				}
				break;
			case 17:
				enterOuterAlt(_localctx, 17);
				{
				setState(1941);
				match(USE);
				setState(1942);
				match(PLAN);
				setState(1943);
				match(STRING);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Optimize_for_argContext extends ParserRuleContext {
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public TerminalNode UNKNOWN() { return getToken(tsqlParser.UNKNOWN, 0); }
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public Optimize_for_argContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optimize_for_arg; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOptimize_for_arg(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOptimize_for_arg(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOptimize_for_arg(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Optimize_for_argContext optimize_for_arg() throws RecognitionException {
		Optimize_for_argContext _localctx = new Optimize_for_argContext(_ctx, getState());
		enterRule(_localctx, 152, RULE_optimize_for_arg);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1946);
			match(LOCAL_ID);
			setState(1950);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case UNKNOWN:
				{
				setState(1947);
				match(UNKNOWN);
				}
				break;
			case EQUAL:
				{
				setState(1948);
				match(EQUAL);
				setState(1949);
				constant();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Select_listContext extends ParserRuleContext {
		public List<Select_list_elemContext> select_list_elem() {
			return getRuleContexts(Select_list_elemContext.class);
		}
		public Select_list_elemContext select_list_elem(int i) {
			return getRuleContext(Select_list_elemContext.class,i);
		}
		public Select_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_select_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSelect_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSelect_list(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSelect_list(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Select_listContext select_list() throws RecognitionException {
		Select_listContext _localctx = new Select_listContext(_ctx, getState());
		enterRule(_localctx, 154, RULE_select_list);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1952);
			select_list_elem();
			setState(1957);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,314,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1953);
					match(COMMA);
					setState(1954);
					select_list_elem();
					}
					} 
				}
				setState(1959);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,314,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Select_list_elemContext extends ParserRuleContext {
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public TerminalNode IDENTITY() { return getToken(tsqlParser.IDENTITY, 0); }
		public TerminalNode ROWGUID() { return getToken(tsqlParser.ROWGUID, 0); }
		public Column_aliasContext column_alias() {
			return getRuleContext(Column_aliasContext.class,0);
		}
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Select_list_elemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_select_list_elem; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSelect_list_elem(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSelect_list_elem(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSelect_list_elem(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Select_list_elemContext select_list_elem() throws RecognitionException {
		Select_list_elemContext _localctx = new Select_list_elemContext(_ctx, getState());
		enterRule(_localctx, 156, RULE_select_list_elem);
		int _la;
		try {
			setState(1981);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,319,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1963);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
					{
					setState(1960);
					table_name();
					setState(1961);
					match(DOT);
					}
				}

				setState(1968);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case STAR:
					{
					setState(1965);
					match(STAR);
					}
					break;
				case DOLLAR:
					{
					setState(1966);
					match(DOLLAR);
					setState(1967);
					_la = _input.LA(1);
					if ( !(_la==IDENTITY || _la==ROWGUID) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1970);
				column_alias();
				setState(1971);
				match(EQUAL);
				setState(1972);
				expression(0);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(1974);
				expression(0);
				setState(1979);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,318,_ctx) ) {
				case 1:
					{
					setState(1976);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==AS) {
						{
						setState(1975);
						match(AS);
						}
					}

					setState(1978);
					column_alias();
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Partition_by_clauseContext extends ParserRuleContext {
		public TerminalNode PARTITION() { return getToken(tsqlParser.PARTITION, 0); }
		public TerminalNode BY() { return getToken(tsqlParser.BY, 0); }
		public Expression_listContext expression_list() {
			return getRuleContext(Expression_listContext.class,0);
		}
		public Partition_by_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_partition_by_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterPartition_by_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitPartition_by_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitPartition_by_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Partition_by_clauseContext partition_by_clause() throws RecognitionException {
		Partition_by_clauseContext _localctx = new Partition_by_clauseContext(_ctx, getState());
		enterRule(_localctx, 158, RULE_partition_by_clause);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(1983);
			match(PARTITION);
			setState(1984);
			match(BY);
			setState(1985);
			expression_list();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_sourceContext extends ParserRuleContext {
		public Table_source_item_joinedContext table_source_item_joined() {
			return getRuleContext(Table_source_item_joinedContext.class,0);
		}
		public Table_sourceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_source; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_source(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_source(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_source(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_sourceContext table_source() throws RecognitionException {
		Table_sourceContext _localctx = new Table_sourceContext(_ctx, getState());
		enterRule(_localctx, 160, RULE_table_source);
		try {
			setState(1992);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,320,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(1987);
				table_source_item_joined();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(1988);
				match(LR_BRACKET);
				setState(1989);
				table_source_item_joined();
				setState(1990);
				match(RR_BRACKET);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_source_item_joinedContext extends ParserRuleContext {
		public Table_source_itemContext table_source_item() {
			return getRuleContext(Table_source_itemContext.class,0);
		}
		public List<Join_partContext> join_part() {
			return getRuleContexts(Join_partContext.class);
		}
		public Join_partContext join_part(int i) {
			return getRuleContext(Join_partContext.class,i);
		}
		public Table_source_item_joinedContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_source_item_joined; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_source_item_joined(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_source_item_joined(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_source_item_joined(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_source_item_joinedContext table_source_item_joined() throws RecognitionException {
		Table_source_item_joinedContext _localctx = new Table_source_item_joinedContext(_ctx, getState());
		enterRule(_localctx, 162, RULE_table_source_item_joined);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(1994);
			table_source_item();
			setState(1998);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,321,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(1995);
					join_part();
					}
					} 
				}
				setState(2000);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,321,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_source_itemContext extends ParserRuleContext {
		public Table_name_with_hintContext table_name_with_hint() {
			return getRuleContext(Table_name_with_hintContext.class,0);
		}
		public As_table_aliasContext as_table_alias() {
			return getRuleContext(As_table_aliasContext.class,0);
		}
		public Rowset_functionContext rowset_function() {
			return getRuleContext(Rowset_functionContext.class,0);
		}
		public Derived_tableContext derived_table() {
			return getRuleContext(Derived_tableContext.class,0);
		}
		public Column_alias_listContext column_alias_list() {
			return getRuleContext(Column_alias_listContext.class,0);
		}
		public Change_tableContext change_table() {
			return getRuleContext(Change_tableContext.class,0);
		}
		public Function_callContext function_call() {
			return getRuleContext(Function_callContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Table_source_itemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_source_item; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_source_item(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_source_item(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_source_item(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_source_itemContext table_source_item() throws RecognitionException {
		Table_source_itemContext _localctx = new Table_source_itemContext(_ctx, getState());
		enterRule(_localctx, 164, RULE_table_source_item);
		try {
			setState(2036);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,330,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2001);
				table_name_with_hint();
				setState(2003);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,322,_ctx) ) {
				case 1:
					{
					setState(2002);
					as_table_alias();
					}
					break;
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2005);
				rowset_function();
				setState(2007);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,323,_ctx) ) {
				case 1:
					{
					setState(2006);
					as_table_alias();
					}
					break;
				}
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(2009);
				derived_table();
				setState(2014);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,325,_ctx) ) {
				case 1:
					{
					setState(2010);
					as_table_alias();
					setState(2012);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,324,_ctx) ) {
					case 1:
						{
						setState(2011);
						column_alias_list();
						}
						break;
					}
					}
					break;
				}
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(2016);
				change_table();
				setState(2017);
				as_table_alias();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(2019);
				function_call();
				setState(2021);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,326,_ctx) ) {
				case 1:
					{
					setState(2020);
					as_table_alias();
					}
					break;
				}
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(2023);
				match(LOCAL_ID);
				setState(2025);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,327,_ctx) ) {
				case 1:
					{
					setState(2024);
					as_table_alias();
					}
					break;
				}
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(2027);
				match(LOCAL_ID);
				setState(2028);
				match(DOT);
				setState(2029);
				function_call();
				setState(2034);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,329,_ctx) ) {
				case 1:
					{
					setState(2030);
					as_table_alias();
					setState(2032);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,328,_ctx) ) {
					case 1:
						{
						setState(2031);
						column_alias_list();
						}
						break;
					}
					}
					break;
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Change_tableContext extends ParserRuleContext {
		public TerminalNode CHANGETABLE() { return getToken(tsqlParser.CHANGETABLE, 0); }
		public TerminalNode CHANGES() { return getToken(tsqlParser.CHANGES, 0); }
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Change_tableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_change_table; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterChange_table(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitChange_table(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitChange_table(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Change_tableContext change_table() throws RecognitionException {
		Change_tableContext _localctx = new Change_tableContext(_ctx, getState());
		enterRule(_localctx, 166, RULE_change_table);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2038);
			match(CHANGETABLE);
			setState(2039);
			match(LR_BRACKET);
			setState(2040);
			match(CHANGES);
			setState(2041);
			table_name();
			setState(2042);
			match(COMMA);
			setState(2043);
			_la = _input.LA(1);
			if ( !(_la==NULL || _la==LOCAL_ID || _la==DECIMAL) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(2044);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Join_partContext extends ParserRuleContext {
		public Token join_type;
		public Token join_hint;
		public TerminalNode JOIN() { return getToken(tsqlParser.JOIN, 0); }
		public Table_sourceContext table_source() {
			return getRuleContext(Table_sourceContext.class,0);
		}
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public Search_conditionContext search_condition() {
			return getRuleContext(Search_conditionContext.class,0);
		}
		public TerminalNode LEFT() { return getToken(tsqlParser.LEFT, 0); }
		public TerminalNode RIGHT() { return getToken(tsqlParser.RIGHT, 0); }
		public TerminalNode FULL() { return getToken(tsqlParser.FULL, 0); }
		public TerminalNode INNER() { return getToken(tsqlParser.INNER, 0); }
		public TerminalNode OUTER() { return getToken(tsqlParser.OUTER, 0); }
		public TerminalNode LOOP() { return getToken(tsqlParser.LOOP, 0); }
		public TerminalNode HASH() { return getToken(tsqlParser.HASH, 0); }
		public TerminalNode MERGE() { return getToken(tsqlParser.MERGE, 0); }
		public TerminalNode REMOTE() { return getToken(tsqlParser.REMOTE, 0); }
		public TerminalNode CROSS() { return getToken(tsqlParser.CROSS, 0); }
		public TerminalNode APPLY() { return getToken(tsqlParser.APPLY, 0); }
		public Join_partContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_join_part; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterJoin_part(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitJoin_part(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitJoin_part(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Join_partContext join_part() throws RecognitionException {
		Join_partContext _localctx = new Join_partContext(_ctx, getState());
		enterRule(_localctx, 168, RULE_join_part);
		int _la;
		try {
			setState(2072);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,335,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2053);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case INNER:
				case JOIN:
				case MERGE:
				case HASH:
				case LOOP:
				case REMOTE:
					{
					setState(2047);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==INNER) {
						{
						setState(2046);
						match(INNER);
						}
					}

					}
					break;
				case FULL:
				case LEFT:
				case RIGHT:
					{
					setState(2049);
					((Join_partContext)_localctx).join_type = _input.LT(1);
					_la = _input.LA(1);
					if ( !(_la==FULL || _la==LEFT || _la==RIGHT) ) {
						((Join_partContext)_localctx).join_type = (Token)_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					setState(2051);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==OUTER) {
						{
						setState(2050);
						match(OUTER);
						}
					}

					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(2056);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==MERGE || ((((_la - 227)) & ~0x3f) == 0 && ((1L << (_la - 227)) & ((1L << (HASH - 227)) | (1L << (LOOP - 227)) | (1L << (REMOTE - 227)))) != 0)) {
					{
					setState(2055);
					((Join_partContext)_localctx).join_hint = _input.LT(1);
					_la = _input.LA(1);
					if ( !(_la==MERGE || ((((_la - 227)) & ~0x3f) == 0 && ((1L << (_la - 227)) & ((1L << (HASH - 227)) | (1L << (LOOP - 227)) | (1L << (REMOTE - 227)))) != 0)) ) {
						((Join_partContext)_localctx).join_hint = (Token)_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
				}

				setState(2058);
				match(JOIN);
				setState(2059);
				table_source();
				setState(2060);
				match(ON);
				setState(2061);
				search_condition();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2063);
				match(CROSS);
				setState(2064);
				match(JOIN);
				setState(2065);
				table_source();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(2066);
				match(CROSS);
				setState(2067);
				match(APPLY);
				setState(2068);
				table_source();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(2069);
				match(OUTER);
				setState(2070);
				match(APPLY);
				setState(2071);
				table_source();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_name_with_hintContext extends ParserRuleContext {
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public With_table_hintsContext with_table_hints() {
			return getRuleContext(With_table_hintsContext.class,0);
		}
		public Table_name_with_hintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_name_with_hint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_name_with_hint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_name_with_hint(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_name_with_hint(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_name_with_hintContext table_name_with_hint() throws RecognitionException {
		Table_name_with_hintContext _localctx = new Table_name_with_hintContext(_ctx, getState());
		enterRule(_localctx, 170, RULE_table_name_with_hint);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2074);
			table_name();
			setState(2076);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,336,_ctx) ) {
			case 1:
				{
				setState(2075);
				with_table_hints();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Rowset_functionContext extends ParserRuleContext {
		public Token data_file;
		public TerminalNode OPENROWSET() { return getToken(tsqlParser.OPENROWSET, 0); }
		public TerminalNode BULK() { return getToken(tsqlParser.BULK, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public List<Bulk_optionContext> bulk_option() {
			return getRuleContexts(Bulk_optionContext.class);
		}
		public Bulk_optionContext bulk_option(int i) {
			return getRuleContext(Bulk_optionContext.class,i);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Rowset_functionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rowset_function; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterRowset_function(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitRowset_function(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitRowset_function(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Rowset_functionContext rowset_function() throws RecognitionException {
		Rowset_functionContext _localctx = new Rowset_functionContext(_ctx, getState());
		enterRule(_localctx, 172, RULE_rowset_function);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2078);
			match(OPENROWSET);
			setState(2079);
			match(LR_BRACKET);
			setState(2080);
			match(BULK);
			setState(2081);
			((Rowset_functionContext)_localctx).data_file = match(STRING);
			setState(2082);
			match(COMMA);
			setState(2092);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,338,_ctx) ) {
			case 1:
				{
				setState(2083);
				bulk_option();
				setState(2088);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(2084);
					match(COMMA);
					setState(2085);
					bulk_option();
					}
					}
					setState(2090);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case 2:
				{
				setState(2091);
				id();
				}
				break;
			}
			setState(2094);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Bulk_optionContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public Bulk_optionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bulk_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterBulk_option(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitBulk_option(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitBulk_option(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Bulk_optionContext bulk_option() throws RecognitionException {
		Bulk_optionContext _localctx = new Bulk_optionContext(_ctx, getState());
		enterRule(_localctx, 174, RULE_bulk_option);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2096);
			id();
			setState(2097);
			match(EQUAL);
			setState(2098);
			_la = _input.LA(1);
			if ( !(_la==DECIMAL || _la==STRING) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Derived_tableContext extends ParserRuleContext {
		public SubqueryContext subquery() {
			return getRuleContext(SubqueryContext.class,0);
		}
		public Derived_tableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_derived_table; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDerived_table(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDerived_table(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDerived_table(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Derived_tableContext derived_table() throws RecognitionException {
		Derived_tableContext _localctx = new Derived_tableContext(_ctx, getState());
		enterRule(_localctx, 176, RULE_derived_table);
		try {
			setState(2105);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,339,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2100);
				subquery();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2101);
				match(LR_BRACKET);
				setState(2102);
				subquery();
				setState(2103);
				match(RR_BRACKET);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Function_callContext extends ParserRuleContext {
		public ExpressionContext style;
		public Token seed;
		public Token increment;
		public Ranking_windowed_functionContext ranking_windowed_function() {
			return getRuleContext(Ranking_windowed_functionContext.class,0);
		}
		public Aggregate_windowed_functionContext aggregate_windowed_function() {
			return getRuleContext(Aggregate_windowed_functionContext.class,0);
		}
		public Scalar_function_nameContext scalar_function_name() {
			return getRuleContext(Scalar_function_nameContext.class,0);
		}
		public Expression_listContext expression_list() {
			return getRuleContext(Expression_listContext.class,0);
		}
		public TerminalNode BINARY_CHECKSUM() { return getToken(tsqlParser.BINARY_CHECKSUM, 0); }
		public TerminalNode CAST() { return getToken(tsqlParser.CAST, 0); }
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public Data_typeContext data_type() {
			return getRuleContext(Data_typeContext.class,0);
		}
		public TerminalNode CONVERT() { return getToken(tsqlParser.CONVERT, 0); }
		public TerminalNode CHECKSUM() { return getToken(tsqlParser.CHECKSUM, 0); }
		public TerminalNode COALESCE() { return getToken(tsqlParser.COALESCE, 0); }
		public TerminalNode CURRENT_TIMESTAMP() { return getToken(tsqlParser.CURRENT_TIMESTAMP, 0); }
		public TerminalNode CURRENT_USER() { return getToken(tsqlParser.CURRENT_USER, 0); }
		public TerminalNode DATEADD() { return getToken(tsqlParser.DATEADD, 0); }
		public DatepartContext datepart() {
			return getRuleContext(DatepartContext.class,0);
		}
		public TerminalNode DATEDIFF() { return getToken(tsqlParser.DATEDIFF, 0); }
		public TerminalNode DATENAME() { return getToken(tsqlParser.DATENAME, 0); }
		public TerminalNode DATEPART() { return getToken(tsqlParser.DATEPART, 0); }
		public TerminalNode IDENTITY() { return getToken(tsqlParser.IDENTITY, 0); }
		public List<TerminalNode> DECIMAL() { return getTokens(tsqlParser.DECIMAL); }
		public TerminalNode DECIMAL(int i) {
			return getToken(tsqlParser.DECIMAL, i);
		}
		public TerminalNode MIN_ACTIVE_ROWVERSION() { return getToken(tsqlParser.MIN_ACTIVE_ROWVERSION, 0); }
		public TerminalNode NULLIF() { return getToken(tsqlParser.NULLIF, 0); }
		public TerminalNode SESSION_USER() { return getToken(tsqlParser.SESSION_USER, 0); }
		public TerminalNode SYSTEM_USER() { return getToken(tsqlParser.SYSTEM_USER, 0); }
		public Function_callContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_function_call; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFunction_call(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFunction_call(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFunction_call(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Function_callContext function_call() throws RecognitionException {
		Function_callContext _localctx = new Function_callContext(_ctx, getState());
		enterRule(_localctx, 178, RULE_function_call);
		int _la;
		try {
			setState(2204);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,344,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2107);
				ranking_windowed_function();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2108);
				aggregate_windowed_function();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(2109);
				scalar_function_name();
				setState(2110);
				match(LR_BRACKET);
				setState(2112);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << CASE) | (1L << COALESCE) | (1L << CONVERT) | (1L << CURRENT_TIMESTAMP) | (1L << CURRENT_USER) | (1L << DEFAULT))) != 0) || ((((_la - 70)) & ~0x3f) == 0 && ((1L << (_la - 70)) & ((1L << (FORCESEEK - 70)) | (1L << (IDENTITY - 70)) | (1L << (LEFT - 70)) | (1L << (NULL - 70)) | (1L << (NULLIF - 70)))) != 0) || ((((_la - 140)) & ~0x3f) == 0 && ((1L << (_la - 140)) & ((1L << (RIGHT - 140)) | (1L << (SESSION_USER - 140)) | (1L << (SYSTEM_USER - 140)) | (1L << (ABSOLUTE - 140)) | (1L << (APPLY - 140)) | (1L << (AUTO - 140)) | (1L << (AVG - 140)) | (1L << (BASE64 - 140)) | (1L << (BINARY_CHECKSUM - 140)) | (1L << (CALLER - 140)) | (1L << (CAST - 140)) | (1L << (CATCH - 140)) | (1L << (CHECKSUM - 140)) | (1L << (CHECKSUM_AGG - 140)) | (1L << (COMMITTED - 140)) | (1L << (CONCAT - 140)) | (1L << (COOKIE - 140)) | (1L << (COUNT - 140)) | (1L << (COUNT_BIG - 140)))) != 0) || ((((_la - 204)) & ~0x3f) == 0 && ((1L << (_la - 204)) & ((1L << (DATEADD - 204)) | (1L << (DATEDIFF - 204)) | (1L << (DATENAME - 204)) | (1L << (DATEPART - 204)) | (1L << (DELAY - 204)) | (1L << (DELETED - 204)) | (1L << (DENSE_RANK - 204)) | (1L << (DISABLE - 204)) | (1L << (DYNAMIC - 204)) | (1L << (ENCRYPTION - 204)) | (1L << (EXPAND - 204)) | (1L << (FAST - 204)) | (1L << (FAST_FORWARD - 204)) | (1L << (FIRST - 204)) | (1L << (FORCE - 204)) | (1L << (FORCED - 204)) | (1L << (FOLLOWING - 204)) | (1L << (FORWARD_ONLY - 204)) | (1L << (FULLSCAN - 204)) | (1L << (GLOBAL - 204)) | (1L << (GO - 204)) | (1L << (GROUPING - 204)) | (1L << (GROUPING_ID - 204)) | (1L << (HASH - 204)) | (1L << (INSENSITIVE - 204)) | (1L << (INSERTED - 204)) | (1L << (ISOLATION - 204)) | (1L << (KEEP - 204)) | (1L << (KEEPFIXED - 204)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 204)) | (1L << (KEYSET - 204)) | (1L << (LAST - 204)) | (1L << (LEVEL - 204)) | (1L << (LOCAL - 204)) | (1L << (LOCK_ESCALATION - 204)) | (1L << (LOGIN - 204)) | (1L << (LOOP - 204)) | (1L << (MARK - 204)) | (1L << (MAX - 204)) | (1L << (MAXDOP - 204)) | (1L << (MAXRECURSION - 204)) | (1L << (MIN - 204)) | (1L << (MIN_ACTIVE_ROWVERSION - 204)) | (1L << (MODIFY - 204)) | (1L << (NEXT - 204)) | (1L << (NAME - 204)) | (1L << (NOCOUNT - 204)) | (1L << (NOEXPAND - 204)) | (1L << (NORECOMPUTE - 204)) | (1L << (NTILE - 204)) | (1L << (NUMBER - 204)) | (1L << (OFFSET - 204)) | (1L << (ONLY - 204)) | (1L << (OPTIMISTIC - 204)) | (1L << (OPTIMIZE - 204)) | (1L << (OUT - 204)) | (1L << (OUTPUT - 204)) | (1L << (OWNER - 204)) | (1L << (PARAMETERIZATION - 204)) | (1L << (PARTITION - 204)) | (1L << (PATH - 204)) | (1L << (PRECEDING - 204)) | (1L << (PRIOR - 204)) | (1L << (RANGE - 204)))) != 0) || ((((_la - 268)) & ~0x3f) == 0 && ((1L << (_la - 268)) & ((1L << (RANK - 268)) | (1L << (READONLY - 268)) | (1L << (READ_ONLY - 268)) | (1L << (RECOMPILE - 268)) | (1L << (RELATIVE - 268)) | (1L << (REMOTE - 268)) | (1L << (REPEATABLE - 268)) | (1L << (ROBUST - 268)) | (1L << (ROOT - 268)) | (1L << (ROW - 268)) | (1L << (ROWGUID - 268)) | (1L << (ROWS - 268)) | (1L << (ROW_NUMBER - 268)) | (1L << (SAMPLE - 268)) | (1L << (SCHEMABINDING - 268)) | (1L << (SCROLL - 268)) | (1L << (SCROLL_LOCKS - 268)) | (1L << (SELF - 268)) | (1L << (SERIALIZABLE - 268)) | (1L << (SIMPLE - 268)) | (1L << (SNAPSHOT - 268)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 268)) | (1L << (STATIC - 268)) | (1L << (STATS_STREAM - 268)) | (1L << (STDEV - 268)) | (1L << (STDEVP - 268)) | (1L << (SUM - 268)) | (1L << (THROW - 268)) | (1L << (TIES - 268)) | (1L << (TIME - 268)) | (1L << (TRY - 268)) | (1L << (TYPE - 268)) | (1L << (TYPE_WARNING - 268)) | (1L << (UNBOUNDED - 268)) | (1L << (UNCOMMITTED - 268)) | (1L << (UNKNOWN - 268)) | (1L << (USING - 268)) | (1L << (VAR - 268)) | (1L << (VARP - 268)) | (1L << (VIEW_METADATA - 268)) | (1L << (VIEWS - 268)) | (1L << (WORK - 268)) | (1L << (XML - 268)) | (1L << (XMLNAMESPACES - 268)) | (1L << (DOUBLE_QUOTE_ID - 268)) | (1L << (SQUARE_BRACKET_ID - 268)) | (1L << (LOCAL_ID - 268)) | (1L << (DECIMAL - 268)) | (1L << (ID - 268)) | (1L << (STRING - 268)) | (1L << (BINARY - 268)) | (1L << (FLOAT - 268)) | (1L << (REAL - 268)))) != 0) || ((((_la - 341)) & ~0x3f) == 0 && ((1L << (_la - 341)) & ((1L << (DOLLAR - 341)) | (1L << (LR_BRACKET - 341)) | (1L << (PLUS - 341)) | (1L << (MINUS - 341)) | (1L << (BIT_NOT - 341)))) != 0)) {
					{
					setState(2111);
					expression_list();
					}
				}

				setState(2114);
				match(RR_BRACKET);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(2116);
				match(BINARY_CHECKSUM);
				setState(2117);
				match(LR_BRACKET);
				setState(2118);
				match(STAR);
				setState(2119);
				match(RR_BRACKET);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(2120);
				match(CAST);
				setState(2121);
				match(LR_BRACKET);
				setState(2122);
				expression(0);
				setState(2123);
				match(AS);
				setState(2124);
				data_type();
				setState(2125);
				match(RR_BRACKET);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(2127);
				match(CONVERT);
				setState(2128);
				match(LR_BRACKET);
				setState(2129);
				data_type();
				setState(2130);
				match(COMMA);
				setState(2131);
				expression(0);
				setState(2134);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(2132);
					match(COMMA);
					setState(2133);
					((Function_callContext)_localctx).style = expression(0);
					}
				}

				setState(2136);
				match(RR_BRACKET);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(2138);
				match(CHECKSUM);
				setState(2139);
				match(LR_BRACKET);
				setState(2140);
				match(STAR);
				setState(2141);
				match(RR_BRACKET);
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(2142);
				match(COALESCE);
				setState(2143);
				match(LR_BRACKET);
				setState(2144);
				expression_list();
				setState(2145);
				match(RR_BRACKET);
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(2147);
				match(CURRENT_TIMESTAMP);
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(2148);
				match(CURRENT_USER);
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(2149);
				match(DATEADD);
				setState(2150);
				match(LR_BRACKET);
				setState(2151);
				datepart();
				setState(2152);
				match(COMMA);
				setState(2153);
				expression(0);
				setState(2154);
				match(COMMA);
				setState(2155);
				expression(0);
				setState(2156);
				match(RR_BRACKET);
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(2158);
				match(DATEDIFF);
				setState(2159);
				match(LR_BRACKET);
				setState(2160);
				datepart();
				setState(2161);
				match(COMMA);
				setState(2162);
				expression(0);
				setState(2163);
				match(COMMA);
				setState(2164);
				expression(0);
				setState(2165);
				match(RR_BRACKET);
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(2167);
				match(DATENAME);
				setState(2168);
				match(LR_BRACKET);
				setState(2169);
				datepart();
				setState(2170);
				match(COMMA);
				setState(2171);
				expression(0);
				setState(2172);
				match(RR_BRACKET);
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(2174);
				match(DATEPART);
				setState(2175);
				match(LR_BRACKET);
				setState(2176);
				datepart();
				setState(2177);
				match(COMMA);
				setState(2178);
				expression(0);
				setState(2179);
				match(RR_BRACKET);
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(2181);
				match(IDENTITY);
				setState(2182);
				match(LR_BRACKET);
				setState(2183);
				data_type();
				setState(2186);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,342,_ctx) ) {
				case 1:
					{
					setState(2184);
					match(COMMA);
					setState(2185);
					((Function_callContext)_localctx).seed = match(DECIMAL);
					}
					break;
				}
				setState(2190);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(2188);
					match(COMMA);
					setState(2189);
					((Function_callContext)_localctx).increment = match(DECIMAL);
					}
				}

				setState(2192);
				match(RR_BRACKET);
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(2194);
				match(MIN_ACTIVE_ROWVERSION);
				}
				break;
			case 17:
				enterOuterAlt(_localctx, 17);
				{
				setState(2195);
				match(NULLIF);
				setState(2196);
				match(LR_BRACKET);
				setState(2197);
				expression(0);
				setState(2198);
				match(COMMA);
				setState(2199);
				expression(0);
				setState(2200);
				match(RR_BRACKET);
				}
				break;
			case 18:
				enterOuterAlt(_localctx, 18);
				{
				setState(2202);
				match(SESSION_USER);
				}
				break;
			case 19:
				enterOuterAlt(_localctx, 19);
				{
				setState(2203);
				match(SYSTEM_USER);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DatepartContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(tsqlParser.ID, 0); }
		public DatepartContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_datepart; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDatepart(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDatepart(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDatepart(this);
			else return visitor.visitChildren(this);
		}
	}

	public final DatepartContext datepart() throws RecognitionException {
		DatepartContext _localctx = new DatepartContext(_ctx, getState());
		enterRule(_localctx, 180, RULE_datepart);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2206);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class As_table_aliasContext extends ParserRuleContext {
		public Table_aliasContext table_alias() {
			return getRuleContext(Table_aliasContext.class,0);
		}
		public TerminalNode AS() { return getToken(tsqlParser.AS, 0); }
		public As_table_aliasContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_as_table_alias; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAs_table_alias(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAs_table_alias(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAs_table_alias(this);
			else return visitor.visitChildren(this);
		}
	}

	public final As_table_aliasContext as_table_alias() throws RecognitionException {
		As_table_aliasContext _localctx = new As_table_aliasContext(_ctx, getState());
		enterRule(_localctx, 182, RULE_as_table_alias);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2209);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AS) {
				{
				setState(2208);
				match(AS);
				}
			}

			setState(2211);
			table_alias();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_aliasContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public With_table_hintsContext with_table_hints() {
			return getRuleContext(With_table_hintsContext.class,0);
		}
		public Table_aliasContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_alias; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_alias(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_alias(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_alias(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_aliasContext table_alias() throws RecognitionException {
		Table_aliasContext _localctx = new Table_aliasContext(_ctx, getState());
		enterRule(_localctx, 184, RULE_table_alias);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2213);
			id();
			setState(2215);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,346,_ctx) ) {
			case 1:
				{
				setState(2214);
				with_table_hints();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class With_table_hintsContext extends ParserRuleContext {
		public List<Table_hintContext> table_hint() {
			return getRuleContexts(Table_hintContext.class);
		}
		public Table_hintContext table_hint(int i) {
			return getRuleContext(Table_hintContext.class,i);
		}
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public With_table_hintsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_with_table_hints; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWith_table_hints(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWith_table_hints(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWith_table_hints(this);
			else return visitor.visitChildren(this);
		}
	}

	public final With_table_hintsContext with_table_hints() throws RecognitionException {
		With_table_hintsContext _localctx = new With_table_hintsContext(_ctx, getState());
		enterRule(_localctx, 186, RULE_with_table_hints);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2218);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WITH) {
				{
				setState(2217);
				match(WITH);
				}
			}

			setState(2220);
			match(LR_BRACKET);
			setState(2221);
			table_hint();
			setState(2226);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(2222);
				match(COMMA);
				setState(2223);
				table_hint();
				}
				}
				setState(2228);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(2229);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Insert_with_table_hintsContext extends ParserRuleContext {
		public TerminalNode WITH() { return getToken(tsqlParser.WITH, 0); }
		public List<Table_hintContext> table_hint() {
			return getRuleContexts(Table_hintContext.class);
		}
		public Table_hintContext table_hint(int i) {
			return getRuleContext(Table_hintContext.class,i);
		}
		public Insert_with_table_hintsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_insert_with_table_hints; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterInsert_with_table_hints(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitInsert_with_table_hints(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitInsert_with_table_hints(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Insert_with_table_hintsContext insert_with_table_hints() throws RecognitionException {
		Insert_with_table_hintsContext _localctx = new Insert_with_table_hintsContext(_ctx, getState());
		enterRule(_localctx, 188, RULE_insert_with_table_hints);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2231);
			match(WITH);
			setState(2232);
			match(LR_BRACKET);
			setState(2233);
			table_hint();
			setState(2238);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(2234);
				match(COMMA);
				setState(2235);
				table_hint();
				}
				}
				setState(2240);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(2241);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_hintContext extends ParserRuleContext {
		public TerminalNode NOEXPAND() { return getToken(tsqlParser.NOEXPAND, 0); }
		public TerminalNode INDEX() { return getToken(tsqlParser.INDEX, 0); }
		public List<Index_valueContext> index_value() {
			return getRuleContexts(Index_valueContext.class);
		}
		public Index_valueContext index_value(int i) {
			return getRuleContext(Index_valueContext.class,i);
		}
		public TerminalNode FORCESEEK() { return getToken(tsqlParser.FORCESEEK, 0); }
		public TerminalNode SERIALIZABLE() { return getToken(tsqlParser.SERIALIZABLE, 0); }
		public TerminalNode SNAPSHOT() { return getToken(tsqlParser.SNAPSHOT, 0); }
		public TerminalNode SPATIAL_WINDOW_MAX_CELLS() { return getToken(tsqlParser.SPATIAL_WINDOW_MAX_CELLS, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode ID() { return getToken(tsqlParser.ID, 0); }
		public List<Index_column_nameContext> index_column_name() {
			return getRuleContexts(Index_column_nameContext.class);
		}
		public Index_column_nameContext index_column_name(int i) {
			return getRuleContext(Index_column_nameContext.class,i);
		}
		public Table_hintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_hint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_hint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_hint(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_hint(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_hintContext table_hint() throws RecognitionException {
		Table_hintContext _localctx = new Table_hintContext(_ctx, getState());
		enterRule(_localctx, 190, RULE_table_hint);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2244);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NOEXPAND) {
				{
				setState(2243);
				match(NOEXPAND);
				}
			}

			setState(2284);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,354,_ctx) ) {
			case 1:
				{
				setState(2246);
				match(INDEX);
				setState(2247);
				match(LR_BRACKET);
				setState(2248);
				index_value();
				setState(2253);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==COMMA) {
					{
					{
					setState(2249);
					match(COMMA);
					setState(2250);
					index_value();
					}
					}
					setState(2255);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(2256);
				match(RR_BRACKET);
				}
				break;
			case 2:
				{
				setState(2258);
				match(INDEX);
				setState(2259);
				match(EQUAL);
				setState(2260);
				index_value();
				}
				break;
			case 3:
				{
				setState(2261);
				match(FORCESEEK);
				setState(2276);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==LR_BRACKET) {
					{
					setState(2262);
					match(LR_BRACKET);
					setState(2263);
					index_value();
					setState(2264);
					match(LR_BRACKET);
					setState(2265);
					index_column_name();
					setState(2270);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==COMMA) {
						{
						{
						setState(2266);
						match(COMMA);
						setState(2267);
						index_column_name();
						}
						}
						setState(2272);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					setState(2273);
					match(RR_BRACKET);
					setState(2274);
					match(RR_BRACKET);
					}
				}

				}
				break;
			case 4:
				{
				setState(2278);
				match(SERIALIZABLE);
				}
				break;
			case 5:
				{
				setState(2279);
				match(SNAPSHOT);
				}
				break;
			case 6:
				{
				setState(2280);
				match(SPATIAL_WINDOW_MAX_CELLS);
				setState(2281);
				match(EQUAL);
				setState(2282);
				match(DECIMAL);
				}
				break;
			case 7:
				{
				setState(2283);
				match(ID);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Index_column_nameContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(tsqlParser.ID, 0); }
		public Index_column_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_index_column_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterIndex_column_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitIndex_column_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitIndex_column_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Index_column_nameContext index_column_name() throws RecognitionException {
		Index_column_nameContext _localctx = new Index_column_nameContext(_ctx, getState());
		enterRule(_localctx, 192, RULE_index_column_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2286);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Index_valueContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(tsqlParser.ID, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public Index_valueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_index_value; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterIndex_value(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitIndex_value(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitIndex_value(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Index_valueContext index_value() throws RecognitionException {
		Index_valueContext _localctx = new Index_valueContext(_ctx, getState());
		enterRule(_localctx, 194, RULE_index_value);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2288);
			_la = _input.LA(1);
			if ( !(_la==DECIMAL || _la==ID) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_alias_listContext extends ParserRuleContext {
		public List<Column_aliasContext> column_alias() {
			return getRuleContexts(Column_aliasContext.class);
		}
		public Column_aliasContext column_alias(int i) {
			return getRuleContext(Column_aliasContext.class,i);
		}
		public Column_alias_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_alias_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_alias_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_alias_list(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_alias_list(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_alias_listContext column_alias_list() throws RecognitionException {
		Column_alias_listContext _localctx = new Column_alias_listContext(_ctx, getState());
		enterRule(_localctx, 196, RULE_column_alias_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2290);
			match(LR_BRACKET);
			setState(2291);
			column_alias();
			setState(2296);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(2292);
				match(COMMA);
				setState(2293);
				column_alias();
				}
				}
				setState(2298);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(2299);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_aliasContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public Column_aliasContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_alias; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_alias(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_alias(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_alias(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_aliasContext column_alias() throws RecognitionException {
		Column_aliasContext _localctx = new Column_aliasContext(_ctx, getState());
		enterRule(_localctx, 198, RULE_column_alias);
		try {
			setState(2303);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(2301);
				id();
				}
				break;
			case STRING:
				enterOuterAlt(_localctx, 2);
				{
				setState(2302);
				match(STRING);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Expression_listContext extends ParserRuleContext {
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public Expression_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterExpression_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitExpression_list(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitExpression_list(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Expression_listContext expression_list() throws RecognitionException {
		Expression_listContext _localctx = new Expression_listContext(_ctx, getState());
		enterRule(_localctx, 200, RULE_expression_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2305);
			expression(0);
			setState(2310);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(2306);
				match(COMMA);
				setState(2307);
				expression(0);
				}
				}
				setState(2312);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Case_exprContext extends ParserRuleContext {
		public TerminalNode CASE() { return getToken(tsqlParser.CASE, 0); }
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public TerminalNode END() { return getToken(tsqlParser.END, 0); }
		public List<TerminalNode> WHEN() { return getTokens(tsqlParser.WHEN); }
		public TerminalNode WHEN(int i) {
			return getToken(tsqlParser.WHEN, i);
		}
		public List<TerminalNode> THEN() { return getTokens(tsqlParser.THEN); }
		public TerminalNode THEN(int i) {
			return getToken(tsqlParser.THEN, i);
		}
		public TerminalNode ELSE() { return getToken(tsqlParser.ELSE, 0); }
		public List<Search_conditionContext> search_condition() {
			return getRuleContexts(Search_conditionContext.class);
		}
		public Search_conditionContext search_condition(int i) {
			return getRuleContext(Search_conditionContext.class,i);
		}
		public Case_exprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_case_expr; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCase_expr(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCase_expr(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCase_expr(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Case_exprContext case_expr() throws RecognitionException {
		Case_exprContext _localctx = new Case_exprContext(_ctx, getState());
		enterRule(_localctx, 202, RULE_case_expr);
		int _la;
		try {
			setState(2346);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,362,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2313);
				match(CASE);
				setState(2314);
				expression(0);
				setState(2320); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(2315);
					match(WHEN);
					setState(2316);
					expression(0);
					setState(2317);
					match(THEN);
					setState(2318);
					expression(0);
					}
					}
					setState(2322); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( _la==WHEN );
				setState(2326);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ELSE) {
					{
					setState(2324);
					match(ELSE);
					setState(2325);
					expression(0);
					}
				}

				setState(2328);
				match(END);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2330);
				match(CASE);
				setState(2336); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(2331);
					match(WHEN);
					setState(2332);
					search_condition();
					setState(2333);
					match(THEN);
					setState(2334);
					expression(0);
					}
					}
					setState(2338); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( _la==WHEN );
				setState(2342);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==ELSE) {
					{
					setState(2340);
					match(ELSE);
					setState(2341);
					expression(0);
					}
				}

				setState(2344);
				match(END);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Ranking_windowed_functionContext extends ParserRuleContext {
		public TerminalNode RANK() { return getToken(tsqlParser.RANK, 0); }
		public Over_clauseContext over_clause() {
			return getRuleContext(Over_clauseContext.class,0);
		}
		public TerminalNode DENSE_RANK() { return getToken(tsqlParser.DENSE_RANK, 0); }
		public TerminalNode NTILE() { return getToken(tsqlParser.NTILE, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode ROW_NUMBER() { return getToken(tsqlParser.ROW_NUMBER, 0); }
		public Ranking_windowed_functionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ranking_windowed_function; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterRanking_windowed_function(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitRanking_windowed_function(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitRanking_windowed_function(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Ranking_windowed_functionContext ranking_windowed_function() throws RecognitionException {
		Ranking_windowed_functionContext _localctx = new Ranking_windowed_functionContext(_ctx, getState());
		enterRule(_localctx, 204, RULE_ranking_windowed_function);
		try {
			setState(2366);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case RANK:
				enterOuterAlt(_localctx, 1);
				{
				setState(2348);
				match(RANK);
				setState(2349);
				match(LR_BRACKET);
				setState(2350);
				match(RR_BRACKET);
				setState(2351);
				over_clause();
				}
				break;
			case DENSE_RANK:
				enterOuterAlt(_localctx, 2);
				{
				setState(2352);
				match(DENSE_RANK);
				setState(2353);
				match(LR_BRACKET);
				setState(2354);
				match(RR_BRACKET);
				setState(2355);
				over_clause();
				}
				break;
			case NTILE:
				enterOuterAlt(_localctx, 3);
				{
				setState(2356);
				match(NTILE);
				setState(2357);
				match(LR_BRACKET);
				setState(2358);
				expression(0);
				setState(2359);
				match(RR_BRACKET);
				setState(2360);
				over_clause();
				}
				break;
			case ROW_NUMBER:
				enterOuterAlt(_localctx, 4);
				{
				setState(2362);
				match(ROW_NUMBER);
				setState(2363);
				match(LR_BRACKET);
				setState(2364);
				match(RR_BRACKET);
				setState(2365);
				over_clause();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Aggregate_windowed_functionContext extends ParserRuleContext {
		public TerminalNode AVG() { return getToken(tsqlParser.AVG, 0); }
		public All_distinct_expressionContext all_distinct_expression() {
			return getRuleContext(All_distinct_expressionContext.class,0);
		}
		public Over_clauseContext over_clause() {
			return getRuleContext(Over_clauseContext.class,0);
		}
		public TerminalNode CHECKSUM_AGG() { return getToken(tsqlParser.CHECKSUM_AGG, 0); }
		public TerminalNode GROUPING() { return getToken(tsqlParser.GROUPING, 0); }
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode GROUPING_ID() { return getToken(tsqlParser.GROUPING_ID, 0); }
		public Expression_listContext expression_list() {
			return getRuleContext(Expression_listContext.class,0);
		}
		public TerminalNode MAX() { return getToken(tsqlParser.MAX, 0); }
		public TerminalNode MIN() { return getToken(tsqlParser.MIN, 0); }
		public TerminalNode SUM() { return getToken(tsqlParser.SUM, 0); }
		public TerminalNode STDEV() { return getToken(tsqlParser.STDEV, 0); }
		public TerminalNode STDEVP() { return getToken(tsqlParser.STDEVP, 0); }
		public TerminalNode VAR() { return getToken(tsqlParser.VAR, 0); }
		public TerminalNode VARP() { return getToken(tsqlParser.VARP, 0); }
		public TerminalNode COUNT() { return getToken(tsqlParser.COUNT, 0); }
		public TerminalNode COUNT_BIG() { return getToken(tsqlParser.COUNT_BIG, 0); }
		public Aggregate_windowed_functionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregate_windowed_function; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAggregate_windowed_function(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAggregate_windowed_function(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAggregate_windowed_function(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Aggregate_windowed_functionContext aggregate_windowed_function() throws RecognitionException {
		Aggregate_windowed_functionContext _localctx = new Aggregate_windowed_functionContext(_ctx, getState());
		enterRule(_localctx, 206, RULE_aggregate_windowed_function);
		try {
			setState(2459);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case AVG:
				enterOuterAlt(_localctx, 1);
				{
				setState(2368);
				match(AVG);
				setState(2369);
				match(LR_BRACKET);
				setState(2370);
				all_distinct_expression();
				setState(2371);
				match(RR_BRACKET);
				setState(2373);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,364,_ctx) ) {
				case 1:
					{
					setState(2372);
					over_clause();
					}
					break;
				}
				}
				break;
			case CHECKSUM_AGG:
				enterOuterAlt(_localctx, 2);
				{
				setState(2375);
				match(CHECKSUM_AGG);
				setState(2376);
				match(LR_BRACKET);
				setState(2377);
				all_distinct_expression();
				setState(2378);
				match(RR_BRACKET);
				}
				break;
			case GROUPING:
				enterOuterAlt(_localctx, 3);
				{
				setState(2380);
				match(GROUPING);
				setState(2381);
				match(LR_BRACKET);
				setState(2382);
				expression(0);
				setState(2383);
				match(RR_BRACKET);
				}
				break;
			case GROUPING_ID:
				enterOuterAlt(_localctx, 4);
				{
				setState(2385);
				match(GROUPING_ID);
				setState(2386);
				match(LR_BRACKET);
				setState(2387);
				expression_list();
				setState(2388);
				match(RR_BRACKET);
				}
				break;
			case MAX:
				enterOuterAlt(_localctx, 5);
				{
				setState(2390);
				match(MAX);
				setState(2391);
				match(LR_BRACKET);
				setState(2392);
				all_distinct_expression();
				setState(2393);
				match(RR_BRACKET);
				setState(2395);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,365,_ctx) ) {
				case 1:
					{
					setState(2394);
					over_clause();
					}
					break;
				}
				}
				break;
			case MIN:
				enterOuterAlt(_localctx, 6);
				{
				setState(2397);
				match(MIN);
				setState(2398);
				match(LR_BRACKET);
				setState(2399);
				all_distinct_expression();
				setState(2400);
				match(RR_BRACKET);
				setState(2402);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,366,_ctx) ) {
				case 1:
					{
					setState(2401);
					over_clause();
					}
					break;
				}
				}
				break;
			case SUM:
				enterOuterAlt(_localctx, 7);
				{
				setState(2404);
				match(SUM);
				setState(2405);
				match(LR_BRACKET);
				setState(2406);
				all_distinct_expression();
				setState(2407);
				match(RR_BRACKET);
				setState(2409);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,367,_ctx) ) {
				case 1:
					{
					setState(2408);
					over_clause();
					}
					break;
				}
				}
				break;
			case STDEV:
				enterOuterAlt(_localctx, 8);
				{
				setState(2411);
				match(STDEV);
				setState(2412);
				match(LR_BRACKET);
				setState(2413);
				all_distinct_expression();
				setState(2414);
				match(RR_BRACKET);
				setState(2416);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,368,_ctx) ) {
				case 1:
					{
					setState(2415);
					over_clause();
					}
					break;
				}
				}
				break;
			case STDEVP:
				enterOuterAlt(_localctx, 9);
				{
				setState(2418);
				match(STDEVP);
				setState(2419);
				match(LR_BRACKET);
				setState(2420);
				all_distinct_expression();
				setState(2421);
				match(RR_BRACKET);
				setState(2423);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,369,_ctx) ) {
				case 1:
					{
					setState(2422);
					over_clause();
					}
					break;
				}
				}
				break;
			case VAR:
				enterOuterAlt(_localctx, 10);
				{
				setState(2425);
				match(VAR);
				setState(2426);
				match(LR_BRACKET);
				setState(2427);
				all_distinct_expression();
				setState(2428);
				match(RR_BRACKET);
				setState(2430);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,370,_ctx) ) {
				case 1:
					{
					setState(2429);
					over_clause();
					}
					break;
				}
				}
				break;
			case VARP:
				enterOuterAlt(_localctx, 11);
				{
				setState(2432);
				match(VARP);
				setState(2433);
				match(LR_BRACKET);
				setState(2434);
				all_distinct_expression();
				setState(2435);
				match(RR_BRACKET);
				setState(2437);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,371,_ctx) ) {
				case 1:
					{
					setState(2436);
					over_clause();
					}
					break;
				}
				}
				break;
			case COUNT:
				enterOuterAlt(_localctx, 12);
				{
				setState(2439);
				match(COUNT);
				setState(2440);
				match(LR_BRACKET);
				setState(2443);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case STAR:
					{
					setState(2441);
					match(STAR);
					}
					break;
				case ALL:
				case CASE:
				case COALESCE:
				case CONVERT:
				case CURRENT_TIMESTAMP:
				case CURRENT_USER:
				case DEFAULT:
				case DISTINCT:
				case FORCESEEK:
				case IDENTITY:
				case LEFT:
				case NULL:
				case NULLIF:
				case RIGHT:
				case SESSION_USER:
				case SYSTEM_USER:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case BINARY_CHECKSUM:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DATEADD:
				case DATEDIFF:
				case DATENAME:
				case DATEPART:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MIN_ACTIVE_ROWVERSION:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case LOCAL_ID:
				case DECIMAL:
				case ID:
				case STRING:
				case BINARY:
				case FLOAT:
				case REAL:
				case DOLLAR:
				case LR_BRACKET:
				case PLUS:
				case MINUS:
				case BIT_NOT:
					{
					setState(2442);
					all_distinct_expression();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(2445);
				match(RR_BRACKET);
				setState(2447);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,373,_ctx) ) {
				case 1:
					{
					setState(2446);
					over_clause();
					}
					break;
				}
				}
				break;
			case COUNT_BIG:
				enterOuterAlt(_localctx, 13);
				{
				setState(2449);
				match(COUNT_BIG);
				setState(2450);
				match(LR_BRACKET);
				setState(2453);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case STAR:
					{
					setState(2451);
					match(STAR);
					}
					break;
				case ALL:
				case CASE:
				case COALESCE:
				case CONVERT:
				case CURRENT_TIMESTAMP:
				case CURRENT_USER:
				case DEFAULT:
				case DISTINCT:
				case FORCESEEK:
				case IDENTITY:
				case LEFT:
				case NULL:
				case NULLIF:
				case RIGHT:
				case SESSION_USER:
				case SYSTEM_USER:
				case ABSOLUTE:
				case APPLY:
				case AUTO:
				case AVG:
				case BASE64:
				case BINARY_CHECKSUM:
				case CALLER:
				case CAST:
				case CATCH:
				case CHECKSUM:
				case CHECKSUM_AGG:
				case COMMITTED:
				case CONCAT:
				case COOKIE:
				case COUNT:
				case COUNT_BIG:
				case DATEADD:
				case DATEDIFF:
				case DATENAME:
				case DATEPART:
				case DELAY:
				case DELETED:
				case DENSE_RANK:
				case DISABLE:
				case DYNAMIC:
				case ENCRYPTION:
				case EXPAND:
				case FAST:
				case FAST_FORWARD:
				case FIRST:
				case FORCE:
				case FORCED:
				case FOLLOWING:
				case FORWARD_ONLY:
				case FULLSCAN:
				case GLOBAL:
				case GO:
				case GROUPING:
				case GROUPING_ID:
				case HASH:
				case INSENSITIVE:
				case INSERTED:
				case ISOLATION:
				case KEEP:
				case KEEPFIXED:
				case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
				case KEYSET:
				case LAST:
				case LEVEL:
				case LOCAL:
				case LOCK_ESCALATION:
				case LOGIN:
				case LOOP:
				case MARK:
				case MAX:
				case MAXDOP:
				case MAXRECURSION:
				case MIN:
				case MIN_ACTIVE_ROWVERSION:
				case MODIFY:
				case NEXT:
				case NAME:
				case NOCOUNT:
				case NOEXPAND:
				case NORECOMPUTE:
				case NTILE:
				case NUMBER:
				case OFFSET:
				case ONLY:
				case OPTIMISTIC:
				case OPTIMIZE:
				case OUT:
				case OUTPUT:
				case OWNER:
				case PARAMETERIZATION:
				case PARTITION:
				case PATH:
				case PRECEDING:
				case PRIOR:
				case RANGE:
				case RANK:
				case READONLY:
				case READ_ONLY:
				case RECOMPILE:
				case RELATIVE:
				case REMOTE:
				case REPEATABLE:
				case ROBUST:
				case ROOT:
				case ROW:
				case ROWGUID:
				case ROWS:
				case ROW_NUMBER:
				case SAMPLE:
				case SCHEMABINDING:
				case SCROLL:
				case SCROLL_LOCKS:
				case SELF:
				case SERIALIZABLE:
				case SIMPLE:
				case SNAPSHOT:
				case SPATIAL_WINDOW_MAX_CELLS:
				case STATIC:
				case STATS_STREAM:
				case STDEV:
				case STDEVP:
				case SUM:
				case THROW:
				case TIES:
				case TIME:
				case TRY:
				case TYPE:
				case TYPE_WARNING:
				case UNBOUNDED:
				case UNCOMMITTED:
				case UNKNOWN:
				case USING:
				case VAR:
				case VARP:
				case VIEW_METADATA:
				case VIEWS:
				case WORK:
				case XML:
				case XMLNAMESPACES:
				case DOUBLE_QUOTE_ID:
				case SQUARE_BRACKET_ID:
				case LOCAL_ID:
				case DECIMAL:
				case ID:
				case STRING:
				case BINARY:
				case FLOAT:
				case REAL:
				case DOLLAR:
				case LR_BRACKET:
				case PLUS:
				case MINUS:
				case BIT_NOT:
					{
					setState(2452);
					all_distinct_expression();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(2455);
				match(RR_BRACKET);
				setState(2457);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,375,_ctx) ) {
				case 1:
					{
					setState(2456);
					over_clause();
					}
					break;
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class All_distinct_expressionContext extends ParserRuleContext {
		public ExpressionContext expression() {
			return getRuleContext(ExpressionContext.class,0);
		}
		public TerminalNode ALL() { return getToken(tsqlParser.ALL, 0); }
		public TerminalNode DISTINCT() { return getToken(tsqlParser.DISTINCT, 0); }
		public All_distinct_expressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_all_distinct_expression; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAll_distinct_expression(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAll_distinct_expression(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAll_distinct_expression(this);
			else return visitor.visitChildren(this);
		}
	}

	public final All_distinct_expressionContext all_distinct_expression() throws RecognitionException {
		All_distinct_expressionContext _localctx = new All_distinct_expressionContext(_ctx, getState());
		enterRule(_localctx, 208, RULE_all_distinct_expression);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2462);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ALL || _la==DISTINCT) {
				{
				setState(2461);
				_la = _input.LA(1);
				if ( !(_la==ALL || _la==DISTINCT) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
			}

			setState(2464);
			expression(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Over_clauseContext extends ParserRuleContext {
		public TerminalNode OVER() { return getToken(tsqlParser.OVER, 0); }
		public Partition_by_clauseContext partition_by_clause() {
			return getRuleContext(Partition_by_clauseContext.class,0);
		}
		public Order_by_clauseContext order_by_clause() {
			return getRuleContext(Order_by_clauseContext.class,0);
		}
		public Row_or_range_clauseContext row_or_range_clause() {
			return getRuleContext(Row_or_range_clauseContext.class,0);
		}
		public Over_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_over_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOver_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOver_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOver_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Over_clauseContext over_clause() throws RecognitionException {
		Over_clauseContext _localctx = new Over_clauseContext(_ctx, getState());
		enterRule(_localctx, 210, RULE_over_clause);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2466);
			match(OVER);
			setState(2467);
			match(LR_BRACKET);
			setState(2469);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==PARTITION) {
				{
				setState(2468);
				partition_by_clause();
				}
			}

			setState(2472);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ORDER) {
				{
				setState(2471);
				order_by_clause();
				}
			}

			setState(2475);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==RANGE || _la==ROWS) {
				{
				setState(2474);
				row_or_range_clause();
				}
			}

			setState(2477);
			match(RR_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Row_or_range_clauseContext extends ParserRuleContext {
		public Window_frame_extentContext window_frame_extent() {
			return getRuleContext(Window_frame_extentContext.class,0);
		}
		public TerminalNode ROWS() { return getToken(tsqlParser.ROWS, 0); }
		public TerminalNode RANGE() { return getToken(tsqlParser.RANGE, 0); }
		public Row_or_range_clauseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_row_or_range_clause; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterRow_or_range_clause(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitRow_or_range_clause(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitRow_or_range_clause(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Row_or_range_clauseContext row_or_range_clause() throws RecognitionException {
		Row_or_range_clauseContext _localctx = new Row_or_range_clauseContext(_ctx, getState());
		enterRule(_localctx, 212, RULE_row_or_range_clause);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2479);
			_la = _input.LA(1);
			if ( !(_la==RANGE || _la==ROWS) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(2480);
			window_frame_extent();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Window_frame_extentContext extends ParserRuleContext {
		public Window_frame_precedingContext window_frame_preceding() {
			return getRuleContext(Window_frame_precedingContext.class,0);
		}
		public TerminalNode BETWEEN() { return getToken(tsqlParser.BETWEEN, 0); }
		public List<Window_frame_boundContext> window_frame_bound() {
			return getRuleContexts(Window_frame_boundContext.class);
		}
		public Window_frame_boundContext window_frame_bound(int i) {
			return getRuleContext(Window_frame_boundContext.class,i);
		}
		public TerminalNode AND() { return getToken(tsqlParser.AND, 0); }
		public Window_frame_extentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_window_frame_extent; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWindow_frame_extent(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWindow_frame_extent(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWindow_frame_extent(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Window_frame_extentContext window_frame_extent() throws RecognitionException {
		Window_frame_extentContext _localctx = new Window_frame_extentContext(_ctx, getState());
		enterRule(_localctx, 214, RULE_window_frame_extent);
		try {
			setState(2488);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CURRENT:
			case UNBOUNDED:
			case DECIMAL:
				enterOuterAlt(_localctx, 1);
				{
				setState(2482);
				window_frame_preceding();
				}
				break;
			case BETWEEN:
				enterOuterAlt(_localctx, 2);
				{
				setState(2483);
				match(BETWEEN);
				setState(2484);
				window_frame_bound();
				setState(2485);
				match(AND);
				setState(2486);
				window_frame_bound();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Window_frame_boundContext extends ParserRuleContext {
		public Window_frame_precedingContext window_frame_preceding() {
			return getRuleContext(Window_frame_precedingContext.class,0);
		}
		public Window_frame_followingContext window_frame_following() {
			return getRuleContext(Window_frame_followingContext.class,0);
		}
		public Window_frame_boundContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_window_frame_bound; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWindow_frame_bound(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWindow_frame_bound(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWindow_frame_bound(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Window_frame_boundContext window_frame_bound() throws RecognitionException {
		Window_frame_boundContext _localctx = new Window_frame_boundContext(_ctx, getState());
		enterRule(_localctx, 216, RULE_window_frame_bound);
		try {
			setState(2492);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,382,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2490);
				window_frame_preceding();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2491);
				window_frame_following();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Window_frame_precedingContext extends ParserRuleContext {
		public TerminalNode UNBOUNDED() { return getToken(tsqlParser.UNBOUNDED, 0); }
		public TerminalNode PRECEDING() { return getToken(tsqlParser.PRECEDING, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public TerminalNode CURRENT() { return getToken(tsqlParser.CURRENT, 0); }
		public TerminalNode ROW() { return getToken(tsqlParser.ROW, 0); }
		public Window_frame_precedingContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_window_frame_preceding; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWindow_frame_preceding(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWindow_frame_preceding(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWindow_frame_preceding(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Window_frame_precedingContext window_frame_preceding() throws RecognitionException {
		Window_frame_precedingContext _localctx = new Window_frame_precedingContext(_ctx, getState());
		enterRule(_localctx, 218, RULE_window_frame_preceding);
		try {
			setState(2500);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case UNBOUNDED:
				enterOuterAlt(_localctx, 1);
				{
				setState(2494);
				match(UNBOUNDED);
				setState(2495);
				match(PRECEDING);
				}
				break;
			case DECIMAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(2496);
				match(DECIMAL);
				setState(2497);
				match(PRECEDING);
				}
				break;
			case CURRENT:
				enterOuterAlt(_localctx, 3);
				{
				setState(2498);
				match(CURRENT);
				setState(2499);
				match(ROW);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Window_frame_followingContext extends ParserRuleContext {
		public TerminalNode UNBOUNDED() { return getToken(tsqlParser.UNBOUNDED, 0); }
		public TerminalNode FOLLOWING() { return getToken(tsqlParser.FOLLOWING, 0); }
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public Window_frame_followingContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_window_frame_following; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterWindow_frame_following(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitWindow_frame_following(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitWindow_frame_following(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Window_frame_followingContext window_frame_following() throws RecognitionException {
		Window_frame_followingContext _localctx = new Window_frame_followingContext(_ctx, getState());
		enterRule(_localctx, 220, RULE_window_frame_following);
		try {
			setState(2506);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case UNBOUNDED:
				enterOuterAlt(_localctx, 1);
				{
				setState(2502);
				match(UNBOUNDED);
				setState(2503);
				match(FOLLOWING);
				}
				break;
			case DECIMAL:
				enterOuterAlt(_localctx, 2);
				{
				setState(2504);
				match(DECIMAL);
				setState(2505);
				match(FOLLOWING);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Full_table_nameContext extends ParserRuleContext {
		public IdContext server;
		public IdContext database;
		public IdContext schema;
		public IdContext table;
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Full_table_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_full_table_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFull_table_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFull_table_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFull_table_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Full_table_nameContext full_table_name() throws RecognitionException {
		Full_table_nameContext _localctx = new Full_table_nameContext(_ctx, getState());
		enterRule(_localctx, 222, RULE_full_table_name);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2525);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,386,_ctx) ) {
			case 1:
				{
				setState(2508);
				((Full_table_nameContext)_localctx).server = id();
				setState(2509);
				match(DOT);
				setState(2510);
				((Full_table_nameContext)_localctx).database = id();
				setState(2511);
				match(DOT);
				setState(2512);
				((Full_table_nameContext)_localctx).schema = id();
				setState(2513);
				match(DOT);
				}
				break;
			case 2:
				{
				setState(2515);
				((Full_table_nameContext)_localctx).database = id();
				setState(2516);
				match(DOT);
				setState(2518);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
					{
					setState(2517);
					((Full_table_nameContext)_localctx).schema = id();
					}
				}

				setState(2520);
				match(DOT);
				}
				break;
			case 3:
				{
				setState(2522);
				((Full_table_nameContext)_localctx).schema = id();
				setState(2523);
				match(DOT);
				}
				break;
			}
			setState(2527);
			((Full_table_nameContext)_localctx).table = id();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Table_nameContext extends ParserRuleContext {
		public IdContext database;
		public IdContext schema;
		public IdContext table;
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Table_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_table_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterTable_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitTable_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitTable_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Table_nameContext table_name() throws RecognitionException {
		Table_nameContext _localctx = new Table_nameContext(_ctx, getState());
		enterRule(_localctx, 224, RULE_table_name);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2539);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,388,_ctx) ) {
			case 1:
				{
				setState(2529);
				((Table_nameContext)_localctx).database = id();
				setState(2530);
				match(DOT);
				setState(2532);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
					{
					setState(2531);
					((Table_nameContext)_localctx).schema = id();
					}
				}

				setState(2534);
				match(DOT);
				}
				break;
			case 2:
				{
				setState(2536);
				((Table_nameContext)_localctx).schema = id();
				setState(2537);
				match(DOT);
				}
				break;
			}
			setState(2541);
			((Table_nameContext)_localctx).table = id();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class View_nameContext extends ParserRuleContext {
		public IdContext schema;
		public IdContext view;
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public View_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_view_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterView_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitView_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitView_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final View_nameContext view_name() throws RecognitionException {
		View_nameContext _localctx = new View_nameContext(_ctx, getState());
		enterRule(_localctx, 226, RULE_view_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2546);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,389,_ctx) ) {
			case 1:
				{
				setState(2543);
				((View_nameContext)_localctx).schema = id();
				setState(2544);
				match(DOT);
				}
				break;
			}
			setState(2548);
			((View_nameContext)_localctx).view = id();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Func_proc_nameContext extends ParserRuleContext {
		public IdContext database;
		public IdContext schema;
		public IdContext procedure;
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public Func_proc_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_func_proc_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFunc_proc_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFunc_proc_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFunc_proc_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Func_proc_nameContext func_proc_name() throws RecognitionException {
		Func_proc_nameContext _localctx = new Func_proc_nameContext(_ctx, getState());
		enterRule(_localctx, 228, RULE_func_proc_name);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2560);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,391,_ctx) ) {
			case 1:
				{
				setState(2550);
				((Func_proc_nameContext)_localctx).database = id();
				setState(2551);
				match(DOT);
				setState(2553);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || ((((_la - 316)) & ~0x3f) == 0 && ((1L << (_la - 316)) & ((1L << (DOUBLE_QUOTE_ID - 316)) | (1L << (SQUARE_BRACKET_ID - 316)) | (1L << (ID - 316)))) != 0)) {
					{
					setState(2552);
					((Func_proc_nameContext)_localctx).schema = id();
					}
				}

				setState(2555);
				match(DOT);
				}
				break;
			case 2:
				{
				{
				setState(2557);
				((Func_proc_nameContext)_localctx).schema = id();
				}
				setState(2558);
				match(DOT);
				}
				break;
			}
			setState(2562);
			((Func_proc_nameContext)_localctx).procedure = id();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Ddl_objectContext extends ParserRuleContext {
		public Full_table_nameContext full_table_name() {
			return getRuleContext(Full_table_nameContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Ddl_objectContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_ddl_object; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDdl_object(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDdl_object(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDdl_object(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Ddl_objectContext ddl_object() throws RecognitionException {
		Ddl_objectContext _localctx = new Ddl_objectContext(_ctx, getState());
		enterRule(_localctx, 230, RULE_ddl_object);
		try {
			setState(2566);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(2564);
				full_table_name();
				}
				break;
			case LOCAL_ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(2565);
				match(LOCAL_ID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Full_column_nameContext extends ParserRuleContext {
		public Column_nameContext column_name() {
			return getRuleContext(Column_nameContext.class,0);
		}
		public Table_nameContext table_name() {
			return getRuleContext(Table_nameContext.class,0);
		}
		public Full_column_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_full_column_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterFull_column_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitFull_column_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitFull_column_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Full_column_nameContext full_column_name() throws RecognitionException {
		Full_column_nameContext _localctx = new Full_column_nameContext(_ctx, getState());
		enterRule(_localctx, 232, RULE_full_column_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2571);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,393,_ctx) ) {
			case 1:
				{
				setState(2568);
				table_name();
				setState(2569);
				match(DOT);
				}
				break;
			}
			setState(2573);
			column_name();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_name_listContext extends ParserRuleContext {
		public List<Column_nameContext> column_name() {
			return getRuleContexts(Column_nameContext.class);
		}
		public Column_nameContext column_name(int i) {
			return getRuleContext(Column_nameContext.class,i);
		}
		public Column_name_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_name_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_name_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_name_list(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_name_list(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_name_listContext column_name_list() throws RecognitionException {
		Column_name_listContext _localctx = new Column_name_listContext(_ctx, getState());
		enterRule(_localctx, 234, RULE_column_name_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2575);
			column_name();
			setState(2580);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(2576);
				match(COMMA);
				setState(2577);
				column_name();
				}
				}
				setState(2582);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Column_nameContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public Column_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_column_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterColumn_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitColumn_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitColumn_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Column_nameContext column_name() throws RecognitionException {
		Column_nameContext _localctx = new Column_nameContext(_ctx, getState());
		enterRule(_localctx, 236, RULE_column_name);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2583);
			id();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Cursor_nameContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode LOCAL_ID() { return getToken(tsqlParser.LOCAL_ID, 0); }
		public Cursor_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_cursor_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterCursor_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitCursor_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitCursor_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Cursor_nameContext cursor_name() throws RecognitionException {
		Cursor_nameContext _localctx = new Cursor_nameContext(_ctx, getState());
		enterRule(_localctx, 238, RULE_cursor_name);
		try {
			setState(2587);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(2585);
				id();
				}
				break;
			case LOCAL_ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(2586);
				match(LOCAL_ID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class On_offContext extends ParserRuleContext {
		public TerminalNode ON() { return getToken(tsqlParser.ON, 0); }
		public TerminalNode OFF() { return getToken(tsqlParser.OFF, 0); }
		public On_offContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_on_off; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterOn_off(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitOn_off(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitOn_off(this);
			else return visitor.visitChildren(this);
		}
	}

	public final On_offContext on_off() throws RecognitionException {
		On_offContext _localctx = new On_offContext(_ctx, getState());
		enterRule(_localctx, 240, RULE_on_off);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2589);
			_la = _input.LA(1);
			if ( !(_la==OFF || _la==ON) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ClusteredContext extends ParserRuleContext {
		public TerminalNode CLUSTERED() { return getToken(tsqlParser.CLUSTERED, 0); }
		public TerminalNode NONCLUSTERED() { return getToken(tsqlParser.NONCLUSTERED, 0); }
		public ClusteredContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_clustered; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterClustered(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitClustered(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitClustered(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ClusteredContext clustered() throws RecognitionException {
		ClusteredContext _localctx = new ClusteredContext(_ctx, getState());
		enterRule(_localctx, 242, RULE_clustered);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2591);
			_la = _input.LA(1);
			if ( !(_la==CLUSTERED || _la==NONCLUSTERED) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Null_notnullContext extends ParserRuleContext {
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public TerminalNode NOT() { return getToken(tsqlParser.NOT, 0); }
		public Null_notnullContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_null_notnull; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterNull_notnull(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitNull_notnull(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitNull_notnull(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Null_notnullContext null_notnull() throws RecognitionException {
		Null_notnullContext _localctx = new Null_notnullContext(_ctx, getState());
		enterRule(_localctx, 244, RULE_null_notnull);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2594);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NOT) {
				{
				setState(2593);
				match(NOT);
				}
			}

			setState(2596);
			match(NULL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Scalar_function_nameContext extends ParserRuleContext {
		public Func_proc_nameContext func_proc_name() {
			return getRuleContext(Func_proc_nameContext.class,0);
		}
		public TerminalNode RIGHT() { return getToken(tsqlParser.RIGHT, 0); }
		public TerminalNode LEFT() { return getToken(tsqlParser.LEFT, 0); }
		public TerminalNode BINARY_CHECKSUM() { return getToken(tsqlParser.BINARY_CHECKSUM, 0); }
		public TerminalNode CHECKSUM() { return getToken(tsqlParser.CHECKSUM, 0); }
		public Scalar_function_nameContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_scalar_function_name; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterScalar_function_name(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitScalar_function_name(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitScalar_function_name(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Scalar_function_nameContext scalar_function_name() throws RecognitionException {
		Scalar_function_nameContext _localctx = new Scalar_function_nameContext(_ctx, getState());
		enterRule(_localctx, 246, RULE_scalar_function_name);
		try {
			setState(2603);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case DOUBLE_QUOTE_ID:
			case SQUARE_BRACKET_ID:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(2598);
				func_proc_name();
				}
				break;
			case RIGHT:
				enterOuterAlt(_localctx, 2);
				{
				setState(2599);
				match(RIGHT);
				}
				break;
			case LEFT:
				enterOuterAlt(_localctx, 3);
				{
				setState(2600);
				match(LEFT);
				}
				break;
			case BINARY_CHECKSUM:
				enterOuterAlt(_localctx, 4);
				{
				setState(2601);
				match(BINARY_CHECKSUM);
				}
				break;
			case CHECKSUM:
				enterOuterAlt(_localctx, 5);
				{
				setState(2602);
				match(CHECKSUM);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Data_typeContext extends ParserRuleContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TerminalNode IDENTITY() { return getToken(tsqlParser.IDENTITY, 0); }
		public List<TerminalNode> DECIMAL() { return getTokens(tsqlParser.DECIMAL); }
		public TerminalNode DECIMAL(int i) {
			return getToken(tsqlParser.DECIMAL, i);
		}
		public TerminalNode MAX() { return getToken(tsqlParser.MAX, 0); }
		public Data_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_data_type; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterData_type(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitData_type(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitData_type(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Data_typeContext data_type() throws RecognitionException {
		Data_typeContext _localctx = new Data_typeContext(_ctx, getState());
		enterRule(_localctx, 248, RULE_data_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2605);
			id();
			setState(2607);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,398,_ctx) ) {
			case 1:
				{
				setState(2606);
				match(IDENTITY);
				}
				break;
			}
			setState(2616);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,400,_ctx) ) {
			case 1:
				{
				setState(2609);
				match(LR_BRACKET);
				setState(2610);
				_la = _input.LA(1);
				if ( !(_la==MAX || _la==DECIMAL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(2613);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==COMMA) {
					{
					setState(2611);
					match(COMMA);
					setState(2612);
					match(DECIMAL);
					}
				}

				setState(2615);
				match(RR_BRACKET);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Default_valueContext extends ParserRuleContext {
		public TerminalNode NULL() { return getToken(tsqlParser.NULL, 0); }
		public ConstantContext constant() {
			return getRuleContext(ConstantContext.class,0);
		}
		public Default_valueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_default_value; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterDefault_value(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitDefault_value(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitDefault_value(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Default_valueContext default_value() throws RecognitionException {
		Default_valueContext _localctx = new Default_valueContext(_ctx, getState());
		enterRule(_localctx, 250, RULE_default_value);
		try {
			setState(2620);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NULL:
				enterOuterAlt(_localctx, 1);
				{
				setState(2618);
				match(NULL);
				}
				break;
			case DECIMAL:
			case STRING:
			case BINARY:
			case FLOAT:
			case REAL:
			case DOLLAR:
			case PLUS:
			case MINUS:
				enterOuterAlt(_localctx, 2);
				{
				setState(2619);
				constant();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ConstantContext extends ParserRuleContext {
		public TerminalNode STRING() { return getToken(tsqlParser.STRING, 0); }
		public TerminalNode BINARY() { return getToken(tsqlParser.BINARY, 0); }
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public TerminalNode REAL() { return getToken(tsqlParser.REAL, 0); }
		public TerminalNode FLOAT() { return getToken(tsqlParser.FLOAT, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public ConstantContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constant; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterConstant(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitConstant(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitConstant(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ConstantContext constant() throws RecognitionException {
		ConstantContext _localctx = new ConstantContext(_ctx, getState());
		enterRule(_localctx, 252, RULE_constant);
		int _la;
		try {
			setState(2634);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,404,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2622);
				match(STRING);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2623);
				match(BINARY);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(2624);
				number();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(2626);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PLUS || _la==MINUS) {
					{
					setState(2625);
					sign();
					}
				}

				setState(2628);
				_la = _input.LA(1);
				if ( !(_la==FLOAT || _la==REAL) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(2630);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==PLUS || _la==MINUS) {
					{
					setState(2629);
					sign();
					}
				}

				setState(2632);
				match(DOLLAR);
				setState(2633);
				_la = _input.LA(1);
				if ( !(_la==DECIMAL || _la==FLOAT) ) {
				_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NumberContext extends ParserRuleContext {
		public TerminalNode DECIMAL() { return getToken(tsqlParser.DECIMAL, 0); }
		public SignContext sign() {
			return getRuleContext(SignContext.class,0);
		}
		public NumberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_number; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterNumber(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitNumber(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitNumber(this);
			else return visitor.visitChildren(this);
		}
	}

	public final NumberContext number() throws RecognitionException {
		NumberContext _localctx = new NumberContext(_ctx, getState());
		enterRule(_localctx, 254, RULE_number);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2637);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==PLUS || _la==MINUS) {
				{
				setState(2636);
				sign();
				}
			}

			setState(2639);
			match(DECIMAL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class SignContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(tsqlParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(tsqlParser.MINUS, 0); }
		public SignContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_sign; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSign(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSign(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSign(this);
			else return visitor.visitChildren(this);
		}
	}

	public final SignContext sign() throws RecognitionException {
		SignContext _localctx = new SignContext(_ctx, getState());
		enterRule(_localctx, 256, RULE_sign);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2641);
			_la = _input.LA(1);
			if ( !(_la==PLUS || _la==MINUS) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IdContext extends ParserRuleContext {
		public Simple_idContext simple_id() {
			return getRuleContext(Simple_idContext.class,0);
		}
		public TerminalNode DOUBLE_QUOTE_ID() { return getToken(tsqlParser.DOUBLE_QUOTE_ID, 0); }
		public TerminalNode SQUARE_BRACKET_ID() { return getToken(tsqlParser.SQUARE_BRACKET_ID, 0); }
		public IdContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_id; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterId(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitId(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitId(this);
			else return visitor.visitChildren(this);
		}
	}

	public final IdContext id() throws RecognitionException {
		IdContext _localctx = new IdContext(_ctx, getState());
		enterRule(_localctx, 258, RULE_id);
		try {
			setState(2646);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FORCESEEK:
			case ABSOLUTE:
			case APPLY:
			case AUTO:
			case AVG:
			case BASE64:
			case CALLER:
			case CAST:
			case CATCH:
			case CHECKSUM_AGG:
			case COMMITTED:
			case CONCAT:
			case COOKIE:
			case COUNT:
			case COUNT_BIG:
			case DELAY:
			case DELETED:
			case DENSE_RANK:
			case DISABLE:
			case DYNAMIC:
			case ENCRYPTION:
			case EXPAND:
			case FAST:
			case FAST_FORWARD:
			case FIRST:
			case FORCE:
			case FORCED:
			case FOLLOWING:
			case FORWARD_ONLY:
			case FULLSCAN:
			case GLOBAL:
			case GO:
			case GROUPING:
			case GROUPING_ID:
			case HASH:
			case INSENSITIVE:
			case INSERTED:
			case ISOLATION:
			case KEEP:
			case KEEPFIXED:
			case IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX:
			case KEYSET:
			case LAST:
			case LEVEL:
			case LOCAL:
			case LOCK_ESCALATION:
			case LOGIN:
			case LOOP:
			case MARK:
			case MAX:
			case MAXDOP:
			case MAXRECURSION:
			case MIN:
			case MODIFY:
			case NEXT:
			case NAME:
			case NOCOUNT:
			case NOEXPAND:
			case NORECOMPUTE:
			case NTILE:
			case NUMBER:
			case OFFSET:
			case ONLY:
			case OPTIMISTIC:
			case OPTIMIZE:
			case OUT:
			case OUTPUT:
			case OWNER:
			case PARAMETERIZATION:
			case PARTITION:
			case PATH:
			case PRECEDING:
			case PRIOR:
			case RANGE:
			case RANK:
			case READONLY:
			case READ_ONLY:
			case RECOMPILE:
			case RELATIVE:
			case REMOTE:
			case REPEATABLE:
			case ROBUST:
			case ROOT:
			case ROW:
			case ROWGUID:
			case ROWS:
			case ROW_NUMBER:
			case SAMPLE:
			case SCHEMABINDING:
			case SCROLL:
			case SCROLL_LOCKS:
			case SELF:
			case SERIALIZABLE:
			case SIMPLE:
			case SNAPSHOT:
			case SPATIAL_WINDOW_MAX_CELLS:
			case STATIC:
			case STATS_STREAM:
			case STDEV:
			case STDEVP:
			case SUM:
			case THROW:
			case TIES:
			case TIME:
			case TRY:
			case TYPE:
			case TYPE_WARNING:
			case UNBOUNDED:
			case UNCOMMITTED:
			case UNKNOWN:
			case USING:
			case VAR:
			case VARP:
			case VIEW_METADATA:
			case VIEWS:
			case WORK:
			case XML:
			case XMLNAMESPACES:
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(2643);
				simple_id();
				}
				break;
			case DOUBLE_QUOTE_ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(2644);
				match(DOUBLE_QUOTE_ID);
				}
				break;
			case SQUARE_BRACKET_ID:
				enterOuterAlt(_localctx, 3);
				{
				setState(2645);
				match(SQUARE_BRACKET_ID);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Simple_idContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(tsqlParser.ID, 0); }
		public TerminalNode ABSOLUTE() { return getToken(tsqlParser.ABSOLUTE, 0); }
		public TerminalNode APPLY() { return getToken(tsqlParser.APPLY, 0); }
		public TerminalNode AUTO() { return getToken(tsqlParser.AUTO, 0); }
		public TerminalNode AVG() { return getToken(tsqlParser.AVG, 0); }
		public TerminalNode BASE64() { return getToken(tsqlParser.BASE64, 0); }
		public TerminalNode CALLER() { return getToken(tsqlParser.CALLER, 0); }
		public TerminalNode CAST() { return getToken(tsqlParser.CAST, 0); }
		public TerminalNode CATCH() { return getToken(tsqlParser.CATCH, 0); }
		public TerminalNode CHECKSUM_AGG() { return getToken(tsqlParser.CHECKSUM_AGG, 0); }
		public TerminalNode COMMITTED() { return getToken(tsqlParser.COMMITTED, 0); }
		public TerminalNode CONCAT() { return getToken(tsqlParser.CONCAT, 0); }
		public TerminalNode COOKIE() { return getToken(tsqlParser.COOKIE, 0); }
		public TerminalNode COUNT() { return getToken(tsqlParser.COUNT, 0); }
		public TerminalNode COUNT_BIG() { return getToken(tsqlParser.COUNT_BIG, 0); }
		public TerminalNode DELAY() { return getToken(tsqlParser.DELAY, 0); }
		public TerminalNode DELETED() { return getToken(tsqlParser.DELETED, 0); }
		public TerminalNode DENSE_RANK() { return getToken(tsqlParser.DENSE_RANK, 0); }
		public TerminalNode DISABLE() { return getToken(tsqlParser.DISABLE, 0); }
		public TerminalNode DYNAMIC() { return getToken(tsqlParser.DYNAMIC, 0); }
		public TerminalNode ENCRYPTION() { return getToken(tsqlParser.ENCRYPTION, 0); }
		public TerminalNode EXPAND() { return getToken(tsqlParser.EXPAND, 0); }
		public TerminalNode FAST() { return getToken(tsqlParser.FAST, 0); }
		public TerminalNode FAST_FORWARD() { return getToken(tsqlParser.FAST_FORWARD, 0); }
		public TerminalNode FIRST() { return getToken(tsqlParser.FIRST, 0); }
		public TerminalNode FOLLOWING() { return getToken(tsqlParser.FOLLOWING, 0); }
		public TerminalNode FORCE() { return getToken(tsqlParser.FORCE, 0); }
		public TerminalNode FORCESEEK() { return getToken(tsqlParser.FORCESEEK, 0); }
		public TerminalNode FORWARD_ONLY() { return getToken(tsqlParser.FORWARD_ONLY, 0); }
		public TerminalNode FULLSCAN() { return getToken(tsqlParser.FULLSCAN, 0); }
		public TerminalNode GLOBAL() { return getToken(tsqlParser.GLOBAL, 0); }
		public TerminalNode GO() { return getToken(tsqlParser.GO, 0); }
		public TerminalNode GROUPING() { return getToken(tsqlParser.GROUPING, 0); }
		public TerminalNode GROUPING_ID() { return getToken(tsqlParser.GROUPING_ID, 0); }
		public TerminalNode HASH() { return getToken(tsqlParser.HASH, 0); }
		public TerminalNode INSENSITIVE() { return getToken(tsqlParser.INSENSITIVE, 0); }
		public TerminalNode INSERTED() { return getToken(tsqlParser.INSERTED, 0); }
		public TerminalNode ISOLATION() { return getToken(tsqlParser.ISOLATION, 0); }
		public TerminalNode KEEP() { return getToken(tsqlParser.KEEP, 0); }
		public TerminalNode KEEPFIXED() { return getToken(tsqlParser.KEEPFIXED, 0); }
		public TerminalNode FORCED() { return getToken(tsqlParser.FORCED, 0); }
		public TerminalNode KEYSET() { return getToken(tsqlParser.KEYSET, 0); }
		public TerminalNode IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX() { return getToken(tsqlParser.IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX, 0); }
		public TerminalNode LAST() { return getToken(tsqlParser.LAST, 0); }
		public TerminalNode LEVEL() { return getToken(tsqlParser.LEVEL, 0); }
		public TerminalNode LOCAL() { return getToken(tsqlParser.LOCAL, 0); }
		public TerminalNode LOCK_ESCALATION() { return getToken(tsqlParser.LOCK_ESCALATION, 0); }
		public TerminalNode LOGIN() { return getToken(tsqlParser.LOGIN, 0); }
		public TerminalNode LOOP() { return getToken(tsqlParser.LOOP, 0); }
		public TerminalNode MARK() { return getToken(tsqlParser.MARK, 0); }
		public TerminalNode MAX() { return getToken(tsqlParser.MAX, 0); }
		public TerminalNode MAXDOP() { return getToken(tsqlParser.MAXDOP, 0); }
		public TerminalNode MAXRECURSION() { return getToken(tsqlParser.MAXRECURSION, 0); }
		public TerminalNode MIN() { return getToken(tsqlParser.MIN, 0); }
		public TerminalNode MODIFY() { return getToken(tsqlParser.MODIFY, 0); }
		public TerminalNode NAME() { return getToken(tsqlParser.NAME, 0); }
		public TerminalNode NEXT() { return getToken(tsqlParser.NEXT, 0); }
		public TerminalNode NOCOUNT() { return getToken(tsqlParser.NOCOUNT, 0); }
		public TerminalNode NOEXPAND() { return getToken(tsqlParser.NOEXPAND, 0); }
		public TerminalNode NORECOMPUTE() { return getToken(tsqlParser.NORECOMPUTE, 0); }
		public TerminalNode NTILE() { return getToken(tsqlParser.NTILE, 0); }
		public TerminalNode NUMBER() { return getToken(tsqlParser.NUMBER, 0); }
		public TerminalNode OFFSET() { return getToken(tsqlParser.OFFSET, 0); }
		public TerminalNode ONLY() { return getToken(tsqlParser.ONLY, 0); }
		public TerminalNode OPTIMISTIC() { return getToken(tsqlParser.OPTIMISTIC, 0); }
		public TerminalNode OPTIMIZE() { return getToken(tsqlParser.OPTIMIZE, 0); }
		public TerminalNode OUT() { return getToken(tsqlParser.OUT, 0); }
		public TerminalNode OUTPUT() { return getToken(tsqlParser.OUTPUT, 0); }
		public TerminalNode OWNER() { return getToken(tsqlParser.OWNER, 0); }
		public TerminalNode PARAMETERIZATION() { return getToken(tsqlParser.PARAMETERIZATION, 0); }
		public TerminalNode PARTITION() { return getToken(tsqlParser.PARTITION, 0); }
		public TerminalNode PATH() { return getToken(tsqlParser.PATH, 0); }
		public TerminalNode PRECEDING() { return getToken(tsqlParser.PRECEDING, 0); }
		public TerminalNode PRIOR() { return getToken(tsqlParser.PRIOR, 0); }
		public TerminalNode RANGE() { return getToken(tsqlParser.RANGE, 0); }
		public TerminalNode RANK() { return getToken(tsqlParser.RANK, 0); }
		public TerminalNode READONLY() { return getToken(tsqlParser.READONLY, 0); }
		public TerminalNode READ_ONLY() { return getToken(tsqlParser.READ_ONLY, 0); }
		public TerminalNode RECOMPILE() { return getToken(tsqlParser.RECOMPILE, 0); }
		public TerminalNode RELATIVE() { return getToken(tsqlParser.RELATIVE, 0); }
		public TerminalNode REMOTE() { return getToken(tsqlParser.REMOTE, 0); }
		public TerminalNode REPEATABLE() { return getToken(tsqlParser.REPEATABLE, 0); }
		public TerminalNode ROBUST() { return getToken(tsqlParser.ROBUST, 0); }
		public TerminalNode ROOT() { return getToken(tsqlParser.ROOT, 0); }
		public TerminalNode ROW() { return getToken(tsqlParser.ROW, 0); }
		public TerminalNode ROWGUID() { return getToken(tsqlParser.ROWGUID, 0); }
		public TerminalNode ROWS() { return getToken(tsqlParser.ROWS, 0); }
		public TerminalNode ROW_NUMBER() { return getToken(tsqlParser.ROW_NUMBER, 0); }
		public TerminalNode SAMPLE() { return getToken(tsqlParser.SAMPLE, 0); }
		public TerminalNode SCHEMABINDING() { return getToken(tsqlParser.SCHEMABINDING, 0); }
		public TerminalNode SCROLL() { return getToken(tsqlParser.SCROLL, 0); }
		public TerminalNode SCROLL_LOCKS() { return getToken(tsqlParser.SCROLL_LOCKS, 0); }
		public TerminalNode SELF() { return getToken(tsqlParser.SELF, 0); }
		public TerminalNode SERIALIZABLE() { return getToken(tsqlParser.SERIALIZABLE, 0); }
		public TerminalNode SIMPLE() { return getToken(tsqlParser.SIMPLE, 0); }
		public TerminalNode SNAPSHOT() { return getToken(tsqlParser.SNAPSHOT, 0); }
		public TerminalNode SPATIAL_WINDOW_MAX_CELLS() { return getToken(tsqlParser.SPATIAL_WINDOW_MAX_CELLS, 0); }
		public TerminalNode STATIC() { return getToken(tsqlParser.STATIC, 0); }
		public TerminalNode STATS_STREAM() { return getToken(tsqlParser.STATS_STREAM, 0); }
		public TerminalNode STDEV() { return getToken(tsqlParser.STDEV, 0); }
		public TerminalNode STDEVP() { return getToken(tsqlParser.STDEVP, 0); }
		public TerminalNode SUM() { return getToken(tsqlParser.SUM, 0); }
		public TerminalNode THROW() { return getToken(tsqlParser.THROW, 0); }
		public TerminalNode TIES() { return getToken(tsqlParser.TIES, 0); }
		public TerminalNode TIME() { return getToken(tsqlParser.TIME, 0); }
		public TerminalNode TRY() { return getToken(tsqlParser.TRY, 0); }
		public TerminalNode TYPE() { return getToken(tsqlParser.TYPE, 0); }
		public TerminalNode TYPE_WARNING() { return getToken(tsqlParser.TYPE_WARNING, 0); }
		public TerminalNode UNBOUNDED() { return getToken(tsqlParser.UNBOUNDED, 0); }
		public TerminalNode UNCOMMITTED() { return getToken(tsqlParser.UNCOMMITTED, 0); }
		public TerminalNode UNKNOWN() { return getToken(tsqlParser.UNKNOWN, 0); }
		public TerminalNode USING() { return getToken(tsqlParser.USING, 0); }
		public TerminalNode VAR() { return getToken(tsqlParser.VAR, 0); }
		public TerminalNode VARP() { return getToken(tsqlParser.VARP, 0); }
		public TerminalNode VIEW_METADATA() { return getToken(tsqlParser.VIEW_METADATA, 0); }
		public TerminalNode VIEWS() { return getToken(tsqlParser.VIEWS, 0); }
		public TerminalNode WORK() { return getToken(tsqlParser.WORK, 0); }
		public TerminalNode XML() { return getToken(tsqlParser.XML, 0); }
		public TerminalNode XMLNAMESPACES() { return getToken(tsqlParser.XMLNAMESPACES, 0); }
		public Simple_idContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_simple_id; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterSimple_id(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitSimple_id(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitSimple_id(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Simple_idContext simple_id() throws RecognitionException {
		Simple_idContext _localctx = new Simple_idContext(_ctx, getState());
		enterRule(_localctx, 260, RULE_simple_id);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2648);
			_la = _input.LA(1);
			if ( !(_la==FORCESEEK || ((((_la - 188)) & ~0x3f) == 0 && ((1L << (_la - 188)) & ((1L << (ABSOLUTE - 188)) | (1L << (APPLY - 188)) | (1L << (AUTO - 188)) | (1L << (AVG - 188)) | (1L << (BASE64 - 188)) | (1L << (CALLER - 188)) | (1L << (CAST - 188)) | (1L << (CATCH - 188)) | (1L << (CHECKSUM_AGG - 188)) | (1L << (COMMITTED - 188)) | (1L << (CONCAT - 188)) | (1L << (COOKIE - 188)) | (1L << (COUNT - 188)) | (1L << (COUNT_BIG - 188)) | (1L << (DELAY - 188)) | (1L << (DELETED - 188)) | (1L << (DENSE_RANK - 188)) | (1L << (DISABLE - 188)) | (1L << (DYNAMIC - 188)) | (1L << (ENCRYPTION - 188)) | (1L << (EXPAND - 188)) | (1L << (FAST - 188)) | (1L << (FAST_FORWARD - 188)) | (1L << (FIRST - 188)) | (1L << (FORCE - 188)) | (1L << (FORCED - 188)) | (1L << (FOLLOWING - 188)) | (1L << (FORWARD_ONLY - 188)) | (1L << (FULLSCAN - 188)) | (1L << (GLOBAL - 188)) | (1L << (GO - 188)) | (1L << (GROUPING - 188)) | (1L << (GROUPING_ID - 188)) | (1L << (HASH - 188)) | (1L << (INSENSITIVE - 188)) | (1L << (INSERTED - 188)) | (1L << (ISOLATION - 188)) | (1L << (KEEP - 188)) | (1L << (KEEPFIXED - 188)) | (1L << (IGNORE_NONCLUSTERED_COLUMNSTORE_INDEX - 188)) | (1L << (KEYSET - 188)) | (1L << (LAST - 188)) | (1L << (LEVEL - 188)) | (1L << (LOCAL - 188)) | (1L << (LOCK_ESCALATION - 188)) | (1L << (LOGIN - 188)) | (1L << (LOOP - 188)) | (1L << (MARK - 188)) | (1L << (MAX - 188)) | (1L << (MAXDOP - 188)) | (1L << (MAXRECURSION - 188)) | (1L << (MIN - 188)) | (1L << (MODIFY - 188)) | (1L << (NEXT - 188)) | (1L << (NAME - 188)) | (1L << (NOCOUNT - 188)) | (1L << (NOEXPAND - 188)))) != 0) || ((((_la - 252)) & ~0x3f) == 0 && ((1L << (_la - 252)) & ((1L << (NORECOMPUTE - 252)) | (1L << (NTILE - 252)) | (1L << (NUMBER - 252)) | (1L << (OFFSET - 252)) | (1L << (ONLY - 252)) | (1L << (OPTIMISTIC - 252)) | (1L << (OPTIMIZE - 252)) | (1L << (OUT - 252)) | (1L << (OUTPUT - 252)) | (1L << (OWNER - 252)) | (1L << (PARAMETERIZATION - 252)) | (1L << (PARTITION - 252)) | (1L << (PATH - 252)) | (1L << (PRECEDING - 252)) | (1L << (PRIOR - 252)) | (1L << (RANGE - 252)) | (1L << (RANK - 252)) | (1L << (READONLY - 252)) | (1L << (READ_ONLY - 252)) | (1L << (RECOMPILE - 252)) | (1L << (RELATIVE - 252)) | (1L << (REMOTE - 252)) | (1L << (REPEATABLE - 252)) | (1L << (ROBUST - 252)) | (1L << (ROOT - 252)) | (1L << (ROW - 252)) | (1L << (ROWGUID - 252)) | (1L << (ROWS - 252)) | (1L << (ROW_NUMBER - 252)) | (1L << (SAMPLE - 252)) | (1L << (SCHEMABINDING - 252)) | (1L << (SCROLL - 252)) | (1L << (SCROLL_LOCKS - 252)) | (1L << (SELF - 252)) | (1L << (SERIALIZABLE - 252)) | (1L << (SIMPLE - 252)) | (1L << (SNAPSHOT - 252)) | (1L << (SPATIAL_WINDOW_MAX_CELLS - 252)) | (1L << (STATIC - 252)) | (1L << (STATS_STREAM - 252)) | (1L << (STDEV - 252)) | (1L << (STDEVP - 252)) | (1L << (SUM - 252)) | (1L << (THROW - 252)) | (1L << (TIES - 252)) | (1L << (TIME - 252)) | (1L << (TRY - 252)) | (1L << (TYPE - 252)) | (1L << (TYPE_WARNING - 252)) | (1L << (UNBOUNDED - 252)) | (1L << (UNCOMMITTED - 252)) | (1L << (UNKNOWN - 252)) | (1L << (USING - 252)) | (1L << (VAR - 252)) | (1L << (VARP - 252)) | (1L << (VIEW_METADATA - 252)) | (1L << (VIEWS - 252)) | (1L << (WORK - 252)) | (1L << (XML - 252)) | (1L << (XMLNAMESPACES - 252)))) != 0) || _la==ID) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Comparison_operatorContext extends ParserRuleContext {
		public Comparison_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_comparison_operator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterComparison_operator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitComparison_operator(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitComparison_operator(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Comparison_operatorContext comparison_operator() throws RecognitionException {
		Comparison_operatorContext _localctx = new Comparison_operatorContext(_ctx, getState());
		enterRule(_localctx, 262, RULE_comparison_operator);
		try {
			setState(2665);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,407,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(2650);
				match(EQUAL);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(2651);
				match(GREATER);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(2652);
				match(LESS);
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(2653);
				match(LESS);
				setState(2654);
				match(EQUAL);
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(2655);
				match(GREATER);
				setState(2656);
				match(EQUAL);
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(2657);
				match(LESS);
				setState(2658);
				match(GREATER);
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(2659);
				match(EXCLAMATION);
				setState(2660);
				match(EQUAL);
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(2661);
				match(EXCLAMATION);
				setState(2662);
				match(GREATER);
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(2663);
				match(EXCLAMATION);
				setState(2664);
				match(LESS);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Assignment_operatorContext extends ParserRuleContext {
		public Assignment_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_assignment_operator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).enterAssignment_operator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof tsqlListener ) ((tsqlListener)listener).exitAssignment_operator(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof tsqlVisitor ) return ((tsqlVisitor<? extends T>)visitor).visitAssignment_operator(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Assignment_operatorContext assignment_operator() throws RecognitionException {
		Assignment_operatorContext _localctx = new Assignment_operatorContext(_ctx, getState());
		enterRule(_localctx, 264, RULE_assignment_operator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(2667);
			_la = _input.LA(1);
			if ( !(((((_la - 329)) & ~0x3f) == 0 && ((1L << (_la - 329)) & ((1L << (PLUS_ASSIGN - 329)) | (1L << (MINUS_ASSIGN - 329)) | (1L << (MULT_ASSIGN - 329)) | (1L << (DIV_ASSIGN - 329)) | (1L << (MOD_ASSIGN - 329)) | (1L << (AND_ASSIGN - 329)) | (1L << (XOR_ASSIGN - 329)) | (1L << (OR_ASSIGN - 329)))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 55:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 4);
		case 1:
			return precpred(_ctx, 2);
		case 2:
			return precpred(_ctx, 1);
		case 3:
			return precpred(_ctx, 10);
		}
		return true;
	}

	private static final int _serializedATNSegments = 2;
	private static final String _serializedATNSegment0 =
		"\3\u0430\ud6d1\u8206\uad2d\u4417\uaef1\u8d80\uaadd\3\u0165\u0a70\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\4\61\t\61\4\62\t\62\4\63\t\63\4\64\t"+
		"\64\4\65\t\65\4\66\t\66\4\67\t\67\48\t8\49\t9\4:\t:\4;\t;\4<\t<\4=\t="+
		"\4>\t>\4?\t?\4@\t@\4A\tA\4B\tB\4C\tC\4D\tD\4E\tE\4F\tF\4G\tG\4H\tH\4I"+
		"\tI\4J\tJ\4K\tK\4L\tL\4M\tM\4N\tN\4O\tO\4P\tP\4Q\tQ\4R\tR\4S\tS\4T\tT"+
		"\4U\tU\4V\tV\4W\tW\4X\tX\4Y\tY\4Z\tZ\4[\t[\4\\\t\\\4]\t]\4^\t^\4_\t_\4"+
		"`\t`\4a\ta\4b\tb\4c\tc\4d\td\4e\te\4f\tf\4g\tg\4h\th\4i\ti\4j\tj\4k\t"+
		"k\4l\tl\4m\tm\4n\tn\4o\to\4p\tp\4q\tq\4r\tr\4s\ts\4t\tt\4u\tu\4v\tv\4"+
		"w\tw\4x\tx\4y\ty\4z\tz\4{\t{\4|\t|\4}\t}\4~\t~\4\177\t\177\4\u0080\t\u0080"+
		"\4\u0081\t\u0081\4\u0082\t\u0082\4\u0083\t\u0083\4\u0084\t\u0084\4\u0085"+
		"\t\u0085\4\u0086\t\u0086\3\2\7\2\u010e\n\2\f\2\16\2\u0111\13\2\3\2\3\2"+
		"\3\3\3\3\3\3\3\3\5\3\u0119\n\3\3\4\3\4\3\4\3\4\5\4\u011f\n\4\3\5\3\5\3"+
		"\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\5\5\u012d\n\5\3\6\3\6\5\6\u0131"+
		"\n\6\3\6\7\6\u0134\n\6\f\6\16\6\u0137\13\6\3\6\3\6\5\6\u013b\n\6\3\6\3"+
		"\6\5\6\u013f\n\6\3\6\3\6\5\6\u0143\n\6\3\6\3\6\3\6\5\6\u0148\n\6\3\6\3"+
		"\6\3\6\5\6\u014d\n\6\3\6\3\6\3\6\3\6\3\6\5\6\u0154\n\6\3\6\5\6\u0157\n"+
		"\6\3\6\3\6\5\6\u015b\n\6\3\6\5\6\u015e\n\6\3\6\3\6\3\6\3\6\3\6\3\6\5\6"+
		"\u0166\n\6\3\6\5\6\u0169\n\6\3\6\3\6\3\6\5\6\u016e\n\6\3\6\7\6\u0171\n"+
		"\6\f\6\16\6\u0174\13\6\3\6\3\6\3\6\5\6\u0179\n\6\3\6\3\6\3\6\5\6\u017e"+
		"\n\6\3\6\7\6\u0181\n\6\f\6\16\6\u0184\13\6\3\6\3\6\3\6\5\6\u0189\n\6\3"+
		"\6\3\6\3\6\3\6\5\6\u018f\n\6\3\6\3\6\3\6\3\6\3\6\5\6\u0196\n\6\3\6\3\6"+
		"\5\6\u019a\n\6\5\6\u019c\n\6\3\6\3\6\3\6\5\6\u01a1\n\6\3\6\3\6\3\6\3\6"+
		"\3\6\3\6\5\6\u01a9\n\6\3\6\3\6\3\6\5\6\u01ae\n\6\3\6\3\6\3\6\5\6\u01b3"+
		"\n\6\7\6\u01b5\n\6\f\6\16\6\u01b8\13\6\3\6\3\6\5\6\u01bc\n\6\5\6\u01be"+
		"\n\6\3\7\3\7\3\7\3\7\3\7\3\7\3\7\3\7\5\7\u01c8\n\7\3\b\5\b\u01cb\n\b\3"+
		"\b\3\b\3\b\3\b\3\b\3\b\5\b\u01d3\n\b\5\b\u01d5\n\b\3\b\5\b\u01d8\n\b\3"+
		"\b\3\b\3\b\5\b\u01dd\n\b\3\b\5\b\u01e0\n\b\3\b\5\b\u01e3\n\b\3\b\3\b\3"+
		"\b\3\b\7\b\u01e9\n\b\f\b\16\b\u01ec\13\b\5\b\u01ee\n\b\3\b\3\b\3\b\3\b"+
		"\3\b\5\b\u01f5\n\b\3\b\3\b\5\b\u01f9\n\b\5\b\u01fb\n\b\5\b\u01fd\n\b\3"+
		"\b\5\b\u0200\n\b\3\b\5\b\u0203\n\b\3\b\5\b\u0206\n\b\3\t\5\t\u0209\n\t"+
		"\3\t\3\t\3\t\3\t\3\t\3\t\5\t\u0211\n\t\5\t\u0213\n\t\3\t\5\t\u0216\n\t"+
		"\3\t\3\t\5\t\u021a\n\t\3\t\5\t\u021d\n\t\3\t\3\t\3\t\3\t\5\t\u0223\n\t"+
		"\3\t\5\t\u0226\n\t\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\t\3\t\7\t\u0231\n\t\f"+
		"\t\16\t\u0234\13\t\3\t\3\t\3\t\3\t\5\t\u023a\n\t\3\t\5\t\u023d\n\t\3\t"+
		"\5\t\u0240\n\t\3\t\5\t\u0243\n\t\3\n\5\n\u0246\n\n\3\n\3\n\5\n\u024a\n"+
		"\n\3\n\5\n\u024d\n\n\3\n\5\n\u0250\n\n\3\n\5\n\u0253\n\n\3\13\5\13\u0256"+
		"\n\13\3\13\3\13\3\13\3\13\3\13\3\13\5\13\u025e\n\13\5\13\u0260\n\13\3"+
		"\13\3\13\5\13\u0264\n\13\3\13\5\13\u0267\n\13\3\13\3\13\3\13\3\13\7\13"+
		"\u026d\n\13\f\13\16\13\u0270\13\13\3\13\5\13\u0273\n\13\3\13\3\13\3\13"+
		"\3\13\7\13\u0279\n\13\f\13\16\13\u027c\13\13\5\13\u027e\n\13\3\13\3\13"+
		"\3\13\3\13\3\13\5\13\u0285\n\13\3\13\3\13\5\13\u0289\n\13\5\13\u028b\n"+
		"\13\5\13\u028d\n\13\3\13\5\13\u0290\n\13\3\13\5\13\u0293\n\13\3\13\5\13"+
		"\u0296\n\13\3\f\3\f\3\f\3\f\7\f\u029c\n\f\f\f\16\f\u029f\13\f\3\f\3\f"+
		"\3\f\5\f\u02a4\n\f\3\f\3\f\3\f\3\f\5\f\u02aa\n\f\5\f\u02ac\n\f\3\r\3\r"+
		"\5\r\u02b0\n\r\3\r\5\r\u02b3\n\r\3\r\5\r\u02b6\n\r\3\16\3\16\3\16\5\16"+
		"\u02bb\n\16\3\16\3\16\3\16\5\16\u02c0\n\16\3\16\5\16\u02c3\n\16\3\17\3"+
		"\17\5\17\u02c7\n\17\3\17\5\17\u02ca\n\17\3\17\3\17\3\17\3\17\3\17\3\17"+
		"\3\17\3\17\5\17\u02d4\n\17\3\20\3\20\3\20\3\20\3\20\5\20\u02db\n\20\3"+
		"\20\5\20\u02de\n\20\3\20\3\20\3\20\7\20\u02e3\n\20\f\20\16\20\u02e6\13"+
		"\20\3\20\5\20\u02e9\n\20\5\20\u02eb\n\20\3\20\3\20\3\20\3\20\7\20\u02f1"+
		"\n\20\f\20\16\20\u02f4\13\20\5\20\u02f6\n\20\3\20\3\20\5\20\u02fa\n\20"+
		"\3\20\3\20\6\20\u02fe\n\20\r\20\16\20\u02ff\3\21\3\21\3\21\3\21\5\21\u0306"+
		"\n\21\3\21\5\21\u0309\n\21\3\21\3\21\5\21\u030d\n\21\3\21\3\21\5\21\u0311"+
		"\n\21\3\21\5\21\u0314\n\21\3\22\3\22\3\22\5\22\u0319\n\22\3\23\3\23\3"+
		"\23\3\23\3\23\3\23\3\23\3\23\3\23\3\23\3\23\3\23\3\23\3\23\5\23\u0329"+
		"\n\23\3\23\3\23\5\23\u032d\n\23\3\23\3\23\5\23\u0331\n\23\5\23\u0333\n"+
		"\23\3\23\5\23\u0336\n\23\3\24\3\24\3\24\3\24\3\24\3\24\5\24\u033e\n\24"+
		"\3\24\7\24\u0341\n\24\f\24\16\24\u0344\13\24\3\24\5\24\u0347\n\24\3\24"+
		"\3\24\3\24\3\24\5\24\u034d\n\24\3\24\5\24\u0350\n\24\3\25\3\25\3\25\3"+
		"\25\3\25\3\25\3\25\7\25\u0359\n\25\f\25\16\25\u035c\13\25\3\25\3\25\5"+
		"\25\u0360\n\25\3\25\3\25\3\25\3\25\7\25\u0366\n\25\f\25\16\25\u0369\13"+
		"\25\5\25\u036b\n\25\3\25\3\25\3\25\3\25\3\25\5\25\u0372\n\25\3\25\5\25"+
		"\u0375\n\25\3\26\3\26\3\27\3\27\3\27\3\27\3\27\3\27\3\27\3\27\3\27\3\27"+
		"\5\27\u0383\n\27\3\27\3\27\3\27\3\27\3\27\3\27\5\27\u038b\n\27\5\27\u038d"+
		"\n\27\3\30\3\30\3\30\3\30\5\30\u0393\n\30\3\30\3\30\3\30\3\30\3\30\3\30"+
		"\3\30\3\30\5\30\u039d\n\30\3\30\5\30\u03a0\n\30\3\31\3\31\3\31\5\31\u03a5"+
		"\n\31\3\32\3\32\3\32\3\32\5\32\u03ab\n\32\3\32\3\32\5\32\u03af\n\32\3"+
		"\33\3\33\3\33\3\33\5\33\u03b5\n\33\3\33\3\33\5\33\u03b9\n\33\3\34\3\34"+
		"\3\34\3\34\3\34\5\34\u03c0\n\34\3\34\3\34\3\34\3\35\3\35\3\35\3\35\5\35"+
		"\u03c9\n\35\3\35\3\35\5\35\u03cd\n\35\3\36\3\36\3\36\3\36\5\36\u03d3\n"+
		"\36\3\36\3\36\3\36\7\36\u03d8\n\36\f\36\16\36\u03db\13\36\3\36\5\36\u03de"+
		"\n\36\3\37\3\37\5\37\u03e2\n\37\3 \3 \3 \3 \3 \3 \3 \3!\3!\3!\3!\3!\3"+
		"!\3!\3!\5!\u03f3\n!\3!\3!\5!\u03f7\n!\3!\3!\3!\3\"\3\"\3\"\3\"\7\"\u0400"+
		"\n\"\f\"\16\"\u0403\13\"\3\"\5\"\u0406\n\"\3\"\3\"\3\"\5\"\u040b\n\"\3"+
		"\"\3\"\5\"\u040f\n\"\5\"\u0411\n\"\3#\3#\5#\u0415\n#\3#\3#\5#\u0419\n"+
		"#\3#\3#\5#\u041d\n#\3#\3#\5#\u0421\n#\3#\3#\3#\3#\5#\u0427\n#\3#\3#\5"+
		"#\u042b\n#\5#\u042d\n#\3$\3$\3$\5$\u0432\n$\3$\3$\3$\3$\7$\u0438\n$\f"+
		"$\16$\u043b\13$\5$\u043d\n$\3$\5$\u0440\n$\3$\3$\3$\3$\3$\7$\u0447\n$"+
		"\f$\16$\u044a\13$\3$\3$\5$\u044e\n$\3$\3$\3$\5$\u0453\n$\3$\5$\u0456\n"+
		"$\5$\u0458\n$\3%\3%\5%\u045c\n%\3%\3%\3%\5%\u0461\n%\3%\3%\5%\u0465\n"+
		"%\3&\3&\3\'\3\'\5\'\u046b\n\'\3\'\3\'\3\'\3\'\3\'\3\'\3\'\5\'\u0474\n"+
		"\'\3\'\5\'\u0477\n\'\5\'\u0479\n\'\3(\3(\3(\3(\5(\u047f\n(\3(\3(\3(\5"+
		"(\u0484\n(\3(\3(\3(\3(\3(\5(\u048b\n(\3(\3(\3(\3(\3(\3(\3(\3(\3(\3(\3"+
		"(\5(\u0498\n(\5(\u049a\n(\5(\u049c\n(\3(\5(\u049f\n(\3(\5(\u04a2\n(\3"+
		")\3)\3)\3)\3)\5)\u04a9\n)\3)\5)\u04ac\n)\3)\3)\3)\3)\5)\u04b2\n)\3)\3"+
		")\3)\5)\u04b7\n)\5)\u04b9\n)\3)\5)\u04bc\n)\3)\3)\3)\3)\5)\u04c2\n)\3"+
		")\3)\3)\3)\5)\u04c8\n)\5)\u04ca\n)\3)\5)\u04cd\n)\3)\3)\5)\u04d1\n)\3"+
		")\5)\u04d4\n)\3)\3)\3)\3)\5)\u04da\n)\3)\5)\u04dd\n)\3)\3)\5)\u04e1\n"+
		")\3)\5)\u04e4\n)\3)\3)\3)\3)\5)\u04ea\n)\3)\5)\u04ed\n)\5)\u04ef\n)\3"+
		"*\3*\5*\u04f3\n*\3+\3+\3+\5+\u04f8\n+\3,\3,\3,\3,\3-\3-\5-\u0500\n-\3"+
		"-\3-\3-\5-\u0505\n-\3.\3.\3.\3.\5.\u050b\n.\3.\7.\u050e\n.\f.\16.\u0511"+
		"\13.\3.\3.\3/\3/\5/\u0517\n/\3\60\3\60\3\60\3\60\5\60\u051d\n\60\3\60"+
		"\3\60\5\60\u0521\n\60\3\60\5\60\u0524\n\60\3\60\3\60\5\60\u0528\n\60\3"+
		"\60\3\60\3\60\3\60\5\60\u052e\n\60\3\60\3\60\3\60\3\60\3\60\3\60\5\60"+
		"\u0536\n\60\3\60\3\60\3\60\5\60\u053b\n\60\5\60\u053d\n\60\3\60\5\60\u0540"+
		"\n\60\3\60\7\60\u0543\n\60\f\60\16\60\u0546\13\60\3\61\3\61\5\61\u054a"+
		"\n\61\3\61\5\61\u054d\n\61\3\61\3\61\3\61\5\61\u0552\n\61\3\61\5\61\u0555"+
		"\n\61\3\61\5\61\u0558\n\61\3\61\3\61\3\61\3\61\5\61\u055e\n\61\3\61\3"+
		"\61\3\61\3\61\5\61\u0564\n\61\3\62\3\62\5\62\u0568\n\62\3\62\3\62\3\62"+
		"\5\62\u056d\n\62\3\62\5\62\u0570\n\62\3\62\3\62\3\62\3\62\5\62\u0576\n"+
		"\62\3\62\3\62\5\62\u057a\n\62\3\62\3\62\3\62\3\62\5\62\u0580\n\62\3\62"+
		"\3\62\3\62\3\62\5\62\u0586\n\62\3\63\3\63\3\63\3\63\3\63\7\63\u058d\n"+
		"\63\f\63\16\63\u0590\13\63\3\63\3\63\3\64\3\64\3\64\3\64\3\64\5\64\u0599"+
		"\n\64\3\65\3\65\3\65\3\65\5\65\u059f\n\65\3\65\3\65\3\65\5\65\u05a4\n"+
		"\65\3\65\5\65\u05a7\n\65\3\65\3\65\3\65\3\65\3\65\3\65\3\65\3\65\3\65"+
		"\5\65\u05b2\n\65\5\65\u05b4\n\65\3\65\5\65\u05b7\n\65\3\65\3\65\3\65\3"+
		"\65\3\65\3\65\3\65\3\65\5\65\u05c1\n\65\5\65\u05c3\n\65\3\65\5\65\u05c6"+
		"\n\65\5\65\u05c8\n\65\3\66\5\66\u05cb\n\66\3\66\5\66\u05ce\n\66\3\66\5"+
		"\66\u05d1\n\66\3\66\5\66\u05d4\n\66\3\66\5\66\u05d7\n\66\3\66\3\66\3\66"+
		"\3\67\3\67\3\67\3\67\3\67\3\67\3\67\3\67\3\67\5\67\u05e5\n\67\3\67\5\67"+
		"\u05e8\n\67\3\67\5\67\u05eb\n\67\3\67\3\67\3\67\3\67\3\67\7\67\u05f2\n"+
		"\67\f\67\16\67\u05f5\13\67\5\67\u05f7\n\67\3\67\5\67\u05fa\n\67\38\38"+
		"\38\38\38\38\58\u0602\n8\38\58\u0605\n8\38\38\38\38\38\38\38\38\38\38"+
		"\38\38\58\u0613\n8\38\58\u0616\n8\38\38\38\38\38\58\u061d\n8\58\u061f"+
		"\n8\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\39\59\u0635"+
		"\n9\39\39\39\39\39\39\39\39\39\39\39\39\39\79\u0644\n9\f9\169\u0647\13"+
		"9\3:\3:\3:\3:\3:\3:\3:\3:\5:\u0651\n:\3;\3;\3<\3<\3<\5<\u0658\n<\3<\3"+
		"<\3<\7<\u065d\n<\f<\16<\u0660\13<\3=\3=\3=\3=\3=\5=\u0667\n=\3=\3=\3="+
		"\3=\3=\3>\3>\5>\u0670\n>\3>\3>\5>\u0674\n>\3>\3>\3>\3>\3>\3>\3>\3>\5>"+
		"\u067e\n>\3?\3?\3?\7?\u0683\n?\f?\16?\u0686\13?\3@\3@\3@\7@\u068b\n@\f"+
		"@\16@\u068e\13@\3A\3A\3A\7A\u0693\nA\fA\16A\u0696\13A\3B\5B\u0699\nB\3"+
		"B\3B\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\3C\5C\u06af\n"+
		"C\3C\3C\3C\3C\3C\3C\3C\5C\u06b8\nC\3C\3C\3C\3C\5C\u06be\nC\3C\3C\3C\3"+
		"C\5C\u06c4\nC\3C\3C\3C\3C\5C\u06ca\nC\3C\3C\3C\3C\3C\3C\3C\3C\5C\u06d4"+
		"\nC\3D\3D\3D\3D\3D\5D\u06db\nD\3D\7D\u06de\nD\fD\16D\u06e1\13D\3E\3E\5"+
		"E\u06e5\nE\3E\3E\5E\u06e9\nE\3E\3E\3E\3E\3E\6E\u06f0\nE\rE\16E\u06f1\5"+
		"E\u06f4\nE\3F\3F\5F\u06f8\nF\3F\3F\3F\5F\u06fd\nF\3F\3F\5F\u0701\nF\5"+
		"F\u0703\nF\3F\3F\3F\5F\u0708\nF\3F\3F\3F\3F\7F\u070e\nF\fF\16F\u0711\13"+
		"F\5F\u0713\nF\3F\3F\5F\u0717\nF\3F\3F\3F\3F\3F\7F\u071e\nF\fF\16F\u0721"+
		"\13F\5F\u0723\nF\3F\3F\5F\u0727\nF\3G\3G\3G\3G\3G\7G\u072e\nG\fG\16G\u0731"+
		"\13G\3G\3G\3G\3G\3G\3G\3G\3G\3G\5G\u073c\nG\5G\u073e\nG\3H\3H\3H\3H\3"+
		"H\3H\5H\u0746\nH\3H\3H\3H\3H\3H\3H\5H\u074e\nH\3H\5H\u0751\nH\5H\u0753"+
		"\nH\3I\3I\3I\3I\3I\5I\u075a\nI\3J\3J\5J\u075e\nJ\3K\3K\3L\3L\3L\3L\3L"+
		"\7L\u0767\nL\fL\16L\u076a\13L\3L\3L\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3"+
		"M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\7M\u0789\nM\fM\16M\u078c"+
		"\13M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\3M\5M\u079b\nM\3N\3N\3N\3N\5"+
		"N\u07a1\nN\3O\3O\3O\7O\u07a6\nO\fO\16O\u07a9\13O\3P\3P\3P\5P\u07ae\nP"+
		"\3P\3P\3P\5P\u07b3\nP\3P\3P\3P\3P\3P\3P\5P\u07bb\nP\3P\5P\u07be\nP\5P"+
		"\u07c0\nP\3Q\3Q\3Q\3Q\3R\3R\3R\3R\3R\5R\u07cb\nR\3S\3S\7S\u07cf\nS\fS"+
		"\16S\u07d2\13S\3T\3T\5T\u07d6\nT\3T\3T\5T\u07da\nT\3T\3T\3T\5T\u07df\n"+
		"T\5T\u07e1\nT\3T\3T\3T\3T\3T\5T\u07e8\nT\3T\3T\5T\u07ec\nT\3T\3T\3T\3"+
		"T\3T\5T\u07f3\nT\5T\u07f5\nT\5T\u07f7\nT\3U\3U\3U\3U\3U\3U\3U\3U\3V\5"+
		"V\u0802\nV\3V\3V\5V\u0806\nV\5V\u0808\nV\3V\5V\u080b\nV\3V\3V\3V\3V\3"+
		"V\3V\3V\3V\3V\3V\3V\3V\3V\3V\5V\u081b\nV\3W\3W\5W\u081f\nW\3X\3X\3X\3"+
		"X\3X\3X\3X\3X\7X\u0829\nX\fX\16X\u082c\13X\3X\5X\u082f\nX\3X\3X\3Y\3Y"+
		"\3Y\3Y\3Z\3Z\3Z\3Z\3Z\5Z\u083c\nZ\3[\3[\3[\3[\3[\5[\u0843\n[\3[\3[\3["+
		"\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\5[\u0859\n[\3[\3["+
		"\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3["+
		"\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3["+
		"\3[\3[\5[\u088d\n[\3[\3[\5[\u0891\n[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3[\3["+
		"\3[\5[\u089f\n[\3\\\3\\\3]\5]\u08a4\n]\3]\3]\3^\3^\5^\u08aa\n^\3_\5_\u08ad"+
		"\n_\3_\3_\3_\3_\7_\u08b3\n_\f_\16_\u08b6\13_\3_\3_\3`\3`\3`\3`\3`\7`\u08bf"+
		"\n`\f`\16`\u08c2\13`\3`\3`\3a\5a\u08c7\na\3a\3a\3a\3a\3a\7a\u08ce\na\f"+
		"a\16a\u08d1\13a\3a\3a\3a\3a\3a\3a\3a\3a\3a\3a\3a\3a\7a\u08df\na\fa\16"+
		"a\u08e2\13a\3a\3a\3a\5a\u08e7\na\3a\3a\3a\3a\3a\3a\5a\u08ef\na\3b\3b\3"+
		"c\3c\3d\3d\3d\3d\7d\u08f9\nd\fd\16d\u08fc\13d\3d\3d\3e\3e\5e\u0902\ne"+
		"\3f\3f\3f\7f\u0907\nf\ff\16f\u090a\13f\3g\3g\3g\3g\3g\3g\3g\6g\u0913\n"+
		"g\rg\16g\u0914\3g\3g\5g\u0919\ng\3g\3g\3g\3g\3g\3g\3g\3g\6g\u0923\ng\r"+
		"g\16g\u0924\3g\3g\5g\u0929\ng\3g\3g\5g\u092d\ng\3h\3h\3h\3h\3h\3h\3h\3"+
		"h\3h\3h\3h\3h\3h\3h\3h\3h\3h\3h\5h\u0941\nh\3i\3i\3i\3i\3i\5i\u0948\n"+
		"i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\3i\5i\u095e"+
		"\ni\3i\3i\3i\3i\3i\5i\u0965\ni\3i\3i\3i\3i\3i\5i\u096c\ni\3i\3i\3i\3i"+
		"\3i\5i\u0973\ni\3i\3i\3i\3i\3i\5i\u097a\ni\3i\3i\3i\3i\3i\5i\u0981\ni"+
		"\3i\3i\3i\3i\3i\5i\u0988\ni\3i\3i\3i\3i\5i\u098e\ni\3i\3i\5i\u0992\ni"+
		"\3i\3i\3i\3i\5i\u0998\ni\3i\3i\5i\u099c\ni\5i\u099e\ni\3j\5j\u09a1\nj"+
		"\3j\3j\3k\3k\3k\5k\u09a8\nk\3k\5k\u09ab\nk\3k\5k\u09ae\nk\3k\3k\3l\3l"+
		"\3l\3m\3m\3m\3m\3m\3m\5m\u09bb\nm\3n\3n\5n\u09bf\nn\3o\3o\3o\3o\3o\3o"+
		"\5o\u09c7\no\3p\3p\3p\3p\5p\u09cd\np\3q\3q\3q\3q\3q\3q\3q\3q\3q\3q\5q"+
		"\u09d9\nq\3q\3q\3q\3q\3q\5q\u09e0\nq\3q\3q\3r\3r\3r\5r\u09e7\nr\3r\3r"+
		"\3r\3r\3r\5r\u09ee\nr\3r\3r\3s\3s\3s\5s\u09f5\ns\3s\3s\3t\3t\3t\5t\u09fc"+
		"\nt\3t\3t\3t\3t\3t\5t\u0a03\nt\3t\3t\3u\3u\5u\u0a09\nu\3v\3v\3v\5v\u0a0e"+
		"\nv\3v\3v\3w\3w\3w\7w\u0a15\nw\fw\16w\u0a18\13w\3x\3x\3y\3y\5y\u0a1e\n"+
		"y\3z\3z\3{\3{\3|\5|\u0a25\n|\3|\3|\3}\3}\3}\3}\3}\5}\u0a2e\n}\3~\3~\5"+
		"~\u0a32\n~\3~\3~\3~\3~\5~\u0a38\n~\3~\5~\u0a3b\n~\3\177\3\177\5\177\u0a3f"+
		"\n\177\3\u0080\3\u0080\3\u0080\3\u0080\5\u0080\u0a45\n\u0080\3\u0080\3"+
		"\u0080\5\u0080\u0a49\n\u0080\3\u0080\3\u0080\5\u0080\u0a4d\n\u0080\3\u0081"+
		"\5\u0081\u0a50\n\u0081\3\u0081\3\u0081\3\u0082\3\u0082\3\u0083\3\u0083"+
		"\3\u0083\5\u0083\u0a59\n\u0083\3\u0084\3\u0084\3\u0085\3\u0085\3\u0085"+
		"\3\u0085\3\u0085\3\u0085\3\u0085\3\u0085\3\u0085\3\u0085\3\u0085\3\u0085"+
		"\3\u0085\3\u0085\3\u0085\5\u0085\u0a6c\n\u0085\3\u0086\3\u0086\3\u0086"+
		"\2\3p\u0087\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \"$&(*,.\60\62\64\66"+
		"8:<>@BDFHJLNPRTVXZ\\^`bdfhjlnprtvxz|~\u0080\u0082\u0084\u0086\u0088\u008a"+
		"\u008c\u008e\u0090\u0092\u0094\u0096\u0098\u009a\u009c\u009e\u00a0\u00a2"+
		"\u00a4\u00a6\u00a8\u00aa\u00ac\u00ae\u00b0\u00b2\u00b4\u00b6\u00b8\u00ba"+
		"\u00bc\u00be\u00c0\u00c2\u00c4\u00c6\u00c8\u00ca\u00cc\u00ce\u00d0\u00d2"+
		"\u00d4\u00d6\u00d8\u00da\u00dc\u00de\u00e0\u00e2\u00e4\u00e6\u00e8\u00ea"+
		"\u00ec\u00ee\u00f0\u00f2\u00f4\u00f6\u00f8\u00fa\u00fc\u00fe\u0100\u0102"+
		"\u0104\u0106\u0108\u010a\2.\3\2\u0140\u0141\4\2\u0140\u0140\u0143\u0143"+
		"\4\2\u00d2\u00d2\u012b\u012b\4\2\u0140\u0141\u0143\u0143\3\2\u0080\u0081"+
		"\4\2\u0105\u0106\u010f\u010f\4\2zz\u0119\u0119\5\2\u00d7\u00d7\u011c\u011c"+
		"\u0135\u0135\5\2\u00a1\u00a1\u00c0\u00c0\u00d5\u00d5\3\2?@\4\2\u00b3\u00b3"+
		"\u00f1\u00f1\3\2\u0105\u0106\3\2\u00a7\u00a8\4\2mmoo\6\2\u00c4\u00c4\u0107"+
		"\u0107\u011f\u011f\u0143\u0143\4\2\u00e1\u00e1\u00ef\u00ef\4\2\u00df\u00df"+
		"\u011d\u011d\6\2\u00d6\u00d6\u00da\u00da\u00ec\u00ec\u0124\u0124\5\2\u0103"+
		"\u0103\u0110\u0110\u011e\u011e\3\2\u0160\u0161\3\2\u015d\u015f\4\2\u0160"+
		"\u0161\u0163\u0165\5\2\4\4\7\7\u009e\u009e\4\2\4\4\65\65\4\2\u0117\u0117"+
		"\u0119\u0119\4\2\u00db\u00db\u00fa\u00fa\4\2\t\t\63\63\4\2ww\u00e5\u00e5"+
		"\5\2ee\u00ca\u00ca\u00e5\u00e5\5\2ee\u00e5\u00e5\u00f2\u00f2\4\2\u00dd"+
		"\u00dd\u0121\u0121\4\2SS\u0118\u0118\4\2jj\u0140\u0141\5\2MMaa\u008e\u008e"+
		"\6\2ee\u00e5\u00e5\u00f2\u00f2\u0113\u0113\4\2\u0141\u0141\u0143\u0143"+
		"\3\2\u0141\u0142\4\2\u010d\u010d\u0119\u0119\4\2\31\31hh\4\2\u00f4\u00f4"+
		"\u0141\u0141\3\2\u0145\u0146\4\2\u0141\u0141\u0145\u0145\t\2HH\u00be\u00c2"+
		"\u00c4\u00c6\u00c8\u00cd\u00d2\u00f7\u00f9\u0139\u0142\u0142\3\2\u014b"+
		"\u0152\u0c2e\2\u010f\3\2\2\2\4\u0118\3\2\2\2\6\u011e\3\2\2\2\b\u012c\3"+
		"\2\2\2\n\u01bd\3\2\2\2\f\u01c7\3\2\2\2\16\u01ca\3\2\2\2\20\u0208\3\2\2"+
		"\2\22\u0245\3\2\2\2\24\u0255\3\2\2\2\26\u0297\3\2\2\2\30\u02af\3\2\2\2"+
		"\32\u02c2\3\2\2\2\34\u02c4\3\2\2\2\36\u02d5\3\2\2\2 \u0301\3\2\2\2\"\u0318"+
		"\3\2\2\2$\u031a\3\2\2\2&\u0337\3\2\2\2(\u0351\3\2\2\2*\u0376\3\2\2\2,"+
		"\u038c\3\2\2\2.\u038e\3\2\2\2\60\u03a1\3\2\2\2\62\u03a6\3\2\2\2\64\u03b0"+
		"\3\2\2\2\66\u03ba\3\2\2\28\u03c4\3\2\2\2:\u03ce\3\2\2\2<\u03e1\3\2\2\2"+
		">\u03e3\3\2\2\2@\u03ea\3\2\2\2B\u0410\3\2\2\2D\u042c\3\2\2\2F\u0457\3"+
		"\2\2\2H\u045b\3\2\2\2J\u0466\3\2\2\2L\u0478\3\2\2\2N\u04a1\3\2\2\2P\u04ee"+
		"\3\2\2\2R\u04f0\3\2\2\2T\u04f4\3\2\2\2V\u04f9\3\2\2\2X\u04fd\3\2\2\2Z"+
		"\u0506\3\2\2\2\\\u0516\3\2\2\2^\u0518\3\2\2\2`\u0549\3\2\2\2b\u0567\3"+
		"\2\2\2d\u0587\3\2\2\2f\u0593\3\2\2\2h\u05c7\3\2\2\2j\u05ca\3\2\2\2l\u05db"+
		"\3\2\2\2n\u061e\3\2\2\2p\u0634\3\2\2\2r\u0650\3\2\2\2t\u0652\3\2\2\2v"+
		"\u0654\3\2\2\2x\u0661\3\2\2\2z\u067d\3\2\2\2|\u067f\3\2\2\2~\u0687\3\2"+
		"\2\2\u0080\u068f\3\2\2\2\u0082\u0698\3\2\2\2\u0084\u06d3\3\2\2\2\u0086"+
		"\u06da\3\2\2\2\u0088\u06e8\3\2\2\2\u008a\u06f5\3\2\2\2\u008c\u0728\3\2"+
		"\2\2\u008e\u0752\3\2\2\2\u0090\u0754\3\2\2\2\u0092\u075b\3\2\2\2\u0094"+
		"\u075f\3\2\2\2\u0096\u0761\3\2\2\2\u0098\u079a\3\2\2\2\u009a\u079c\3\2"+
		"\2\2\u009c\u07a2\3\2\2\2\u009e\u07bf\3\2\2\2\u00a0\u07c1\3\2\2\2\u00a2"+
		"\u07ca\3\2\2\2\u00a4\u07cc\3\2\2\2\u00a6\u07f6\3\2\2\2\u00a8\u07f8\3\2"+
		"\2\2\u00aa\u081a\3\2\2\2\u00ac\u081c\3\2\2\2\u00ae\u0820\3\2\2\2\u00b0"+
		"\u0832\3\2\2\2\u00b2\u083b\3\2\2\2\u00b4\u089e\3\2\2\2\u00b6\u08a0\3\2"+
		"\2\2\u00b8\u08a3\3\2\2\2\u00ba\u08a7\3\2\2\2\u00bc\u08ac\3\2\2\2\u00be"+
		"\u08b9\3\2\2\2\u00c0\u08c6\3\2\2\2\u00c2\u08f0\3\2\2\2\u00c4\u08f2\3\2"+
		"\2\2\u00c6\u08f4\3\2\2\2\u00c8\u0901\3\2\2\2\u00ca\u0903\3\2\2\2\u00cc"+
		"\u092c\3\2\2\2\u00ce\u0940\3\2\2\2\u00d0\u099d\3\2\2\2\u00d2\u09a0\3\2"+
		"\2\2\u00d4\u09a4\3\2\2\2\u00d6\u09b1\3\2\2\2\u00d8\u09ba\3\2\2\2\u00da"+
		"\u09be\3\2\2\2\u00dc\u09c6\3\2\2\2\u00de\u09cc\3\2\2\2\u00e0\u09df\3\2"+
		"\2\2\u00e2\u09ed\3\2\2\2\u00e4\u09f4\3\2\2\2\u00e6\u0a02\3\2\2\2\u00e8"+
		"\u0a08\3\2\2\2\u00ea\u0a0d\3\2\2\2\u00ec\u0a11\3\2\2\2\u00ee\u0a19\3\2"+
		"\2\2\u00f0\u0a1d\3\2\2\2\u00f2\u0a1f\3\2\2\2\u00f4\u0a21\3\2\2\2\u00f6"+
		"\u0a24\3\2\2\2\u00f8\u0a2d\3\2\2\2\u00fa\u0a2f\3\2\2\2\u00fc\u0a3e\3\2"+
		"\2\2\u00fe\u0a4c\3\2\2\2\u0100\u0a4f\3\2\2\2\u0102\u0a53\3\2\2\2\u0104"+
		"\u0a58\3\2\2\2\u0106\u0a5a\3\2\2\2\u0108\u0a6b\3\2\2\2\u010a\u0a6d\3\2"+
		"\2\2\u010c\u010e\5\4\3\2\u010d\u010c\3\2\2\2\u010e\u0111\3\2\2\2\u010f"+
		"\u010d\3\2\2\2\u010f\u0110\3\2\2\2\u0110\u0112\3\2\2\2\u0111\u010f\3\2"+
		"\2\2\u0112\u0113\7\2\2\3\u0113\3\3\2\2\2\u0114\u0119\5\6\4\2\u0115\u0119"+
		"\5\b\5\2\u0116\u0119\5\n\6\2\u0117\u0119\5\f\7\2\u0118\u0114\3\2\2\2\u0118"+
		"\u0115\3\2\2\2\u0118\u0116\3\2\2\2\u0118\u0117\3\2\2\2\u0119\5\3\2\2\2"+
		"\u011a\u011f\5\16\b\2\u011b\u011f\5\20\t\2\u011c\u011f\5\22\n\2\u011d"+
		"\u011f\5\24\13\2\u011e\u011a\3\2\2\2\u011e\u011b\3\2\2\2\u011e\u011c\3"+
		"\2\2\2\u011e\u011d\3\2\2\2\u011f\7\3\2\2\2\u0120\u012d\5\34\17\2\u0121"+
		"\u012d\5\36\20\2\u0122\u012d\5$\23\2\u0123\u012d\5&\24\2\u0124\u012d\5"+
		"(\25\2\u0125\u012d\5,\27\2\u0126\u012d\5.\30\2\u0127\u012d\5\62\32\2\u0128"+
		"\u012d\5\64\33\2\u0129\u012d\5\66\34\2\u012a\u012d\58\35\2\u012b\u012d"+
		"\5:\36\2\u012c\u0120\3\2\2\2\u012c\u0121\3\2\2\2\u012c\u0122\3\2\2\2\u012c"+
		"\u0123\3\2\2\2\u012c\u0124\3\2\2\2\u012c\u0125\3\2\2\2\u012c\u0126\3\2"+
		"\2\2\u012c\u0127\3\2\2\2\u012c\u0128\3\2\2\2\u012c\u0129\3\2\2\2\u012c"+
		"\u012a\3\2\2\2\u012c\u012b\3\2\2\2\u012d\t\3\2\2\2\u012e\u0130\7\f\2\2"+
		"\u012f\u0131\7\u015b\2\2\u0130\u012f\3\2\2\2\u0130\u0131\3\2\2\2\u0131"+
		"\u0135\3\2\2\2\u0132\u0134\5\4\3\2\u0133\u0132\3\2\2\2\u0134\u0137\3\2"+
		"\2\2\u0135\u0133\3\2\2\2\u0135\u0136\3\2\2\2\u0136\u0138\3\2\2\2\u0137"+
		"\u0135\3\2\2\2\u0138\u013a\7;\2\2\u0139\u013b\7\u015b\2\2\u013a\u0139"+
		"\3\2\2\2\u013a\u013b\3\2\2\2\u013b\u01be\3\2\2\2\u013c\u013e\7\16\2\2"+
		"\u013d\u013f\7\u015b\2\2\u013e\u013d\3\2\2\2\u013e\u013f\3\2\2\2\u013f"+
		"\u01be\3\2\2\2\u0140\u0142\7\"\2\2\u0141\u0143\7\u015b\2\2\u0142\u0141"+
		"\3\2\2\2\u0142\u0143\3\2\2\2\u0143\u01be\3\2\2\2\u0144\u0145\7O\2\2\u0145"+
		"\u0147\5\u0104\u0083\2\u0146\u0148\7\u015b\2\2\u0147\u0146\3\2\2\2\u0147"+
		"\u0148\3\2\2\2\u0148\u01be\3\2\2\2\u0149\u014a\5\u0104\u0083\2\u014a\u014c"+
		"\7\u015c\2\2\u014b\u014d\7\u015b\2\2\u014c\u014b\3\2\2\2\u014c\u014d\3"+
		"\2\2\2\u014d\u01be\3\2\2\2\u014e\u014f\7V\2\2\u014f\u0150\5~@\2\u0150"+
		"\u0153\5\4\3\2\u0151\u0152\7:\2\2\u0152\u0154\5\4\3\2\u0153\u0151\3\2"+
		"\2\2\u0153\u0154\3\2\2\2\u0154\u0156\3\2\2\2\u0155\u0157\7\u015b\2\2\u0156"+
		"\u0155\3\2\2\2\u0156\u0157\3\2\2\2\u0157\u01be\3\2\2\2\u0158\u015a\7\u008b"+
		"\2\2\u0159\u015b\5p9\2\u015a\u0159\3\2\2\2\u015a\u015b\3\2\2\2\u015b\u015d"+
		"\3\2\2\2\u015c\u015e\7\u015b\2\2\u015d\u015c\3\2\2\2\u015d\u015e\3\2\2"+
		"\2\u015e\u01be\3\2\2\2\u015f\u0165\7\u0129\2\2\u0160\u0161\t\2\2\2\u0161"+
		"\u0162\7\u015a\2\2\u0162\u0163\t\3\2\2\u0163\u0164\7\u015a\2\2\u0164\u0166"+
		"\t\2\2\2\u0165\u0160\3\2\2\2\u0165\u0166\3\2\2\2\u0166\u0168\3\2\2\2\u0167"+
		"\u0169\7\u015b\2\2\u0168\u0167\3\2\2\2\u0168\u0169\3\2\2\2\u0169\u01be"+
		"\3\2\2\2\u016a\u016b\7\f\2\2\u016b\u016d\7\u012c\2\2\u016c\u016e\7\u015b"+
		"\2\2\u016d\u016c\3\2\2\2\u016d\u016e\3\2\2\2\u016e\u0172\3\2\2\2\u016f"+
		"\u0171\5\4\3\2\u0170\u016f\3\2\2\2\u0171\u0174\3\2\2\2\u0172\u0170\3\2"+
		"\2\2\u0172\u0173\3\2\2\2\u0173\u0175\3\2\2\2\u0174\u0172\3\2\2\2\u0175"+
		"\u0176\7;\2\2\u0176\u0178\7\u012c\2\2\u0177\u0179\7\u015b\2\2\u0178\u0177"+
		"\3\2\2\2\u0178\u0179\3\2\2\2\u0179\u017a\3\2\2\2\u017a\u017b\7\f\2\2\u017b"+
		"\u017d\7\u00c6\2\2\u017c\u017e\7\u015b\2\2\u017d\u017c\3\2\2\2\u017d\u017e"+
		"\3\2\2\2\u017e\u0182\3\2\2\2\u017f\u0181\5\4\3\2\u0180\u017f\3\2\2\2\u0181"+
		"\u0184\3\2\2\2\u0182\u0180\3\2\2\2\u0182\u0183\3\2\2\2\u0183\u0185\3\2"+
		"\2\2\u0184\u0182\3\2\2\2\u0185\u0186\7;\2\2\u0186\u0188\7\u00c6\2\2\u0187"+
		"\u0189\7\u015b\2\2\u0188\u0187\3\2\2\2\u0188\u0189\3\2\2\2\u0189\u01be"+
		"\3\2\2\2\u018a\u018b\7\u00b7\2\2\u018b\u018c\t\4\2\2\u018c\u018e\5p9\2"+
		"\u018d\u018f\7\u015b\2\2\u018e\u018d\3\2\2\2\u018e\u018f\3\2\2\2\u018f"+
		"\u01be\3\2\2\2\u0190\u0191\7\u00ba\2\2\u0191\u019b\5~@\2\u0192\u019c\5"+
		"\4\3\2\u0193\u0195\7\16\2\2\u0194\u0196\7\u015b\2\2\u0195\u0194\3\2\2"+
		"\2\u0195\u0196\3\2\2\2\u0196\u019c\3\2\2\2\u0197\u0199\7\"\2\2\u0198\u019a"+
		"\7\u015b\2\2\u0199\u0198\3\2\2\2\u0199\u019a\3\2\2\2\u019a\u019c\3\2\2"+
		"\2\u019b\u0192\3\2\2\2\u019b\u0193\3\2\2\2\u019b\u0197\3\2\2\2\u019c\u01be"+
		"\3\2\2\2\u019d\u019e\7\177\2\2\u019e\u01a0\5p9\2\u019f\u01a1\7\u015b\2"+
		"\2\u01a0\u019f\3\2\2\2\u01a0\u01a1\3\2\2\2\u01a1\u01be\3\2\2\2\u01a2\u01a3"+
		"\7\u0083\2\2\u01a3\u01a4\7\u0158\2\2\u01a4\u01a5\t\5\2\2\u01a5\u01a8\7"+
		"\u015a\2\2\u01a6\u01a9\5\u0100\u0081\2\u01a7\u01a9\7\u0140\2\2\u01a8\u01a6"+
		"\3\2\2\2\u01a8\u01a7\3\2\2\2\u01a9\u01aa\3\2\2\2\u01aa\u01ad\7\u015a\2"+
		"\2\u01ab\u01ae\5\u0100\u0081\2\u01ac\u01ae\7\u0140\2\2\u01ad\u01ab\3\2"+
		"\2\2\u01ad\u01ac\3\2\2\2\u01ae\u01b6\3\2\2\2\u01af\u01b2\7\u015a\2\2\u01b0"+
		"\u01b3\5\u00fe\u0080\2\u01b1\u01b3\7\u0140\2\2\u01b2\u01b0\3\2\2\2\u01b2"+
		"\u01b1\3\2\2\2\u01b3\u01b5\3\2\2\2\u01b4\u01af\3\2\2\2\u01b5\u01b8\3\2"+
		"\2\2\u01b6\u01b4\3\2\2\2\u01b6\u01b7\3\2\2\2\u01b7\u01b9\3\2\2\2\u01b8"+
		"\u01b6\3\2\2\2\u01b9\u01bb\7\u0159\2\2\u01ba\u01bc\7\u015b\2\2\u01bb\u01ba"+
		"\3\2\2\2\u01bb\u01bc\3\2\2\2\u01bc\u01be\3\2\2\2\u01bd\u012e\3\2\2\2\u01bd"+
		"\u013c\3\2\2\2\u01bd\u0140\3\2\2\2\u01bd\u0144\3\2\2\2\u01bd\u0149\3\2"+
		"\2\2\u01bd\u014e\3\2\2\2\u01bd\u0158\3\2\2\2\u01bd\u015f\3\2\2\2\u01bd"+
		"\u016a\3\2\2\2\u01bd\u018a\3\2\2\2\u01bd\u0190\3\2\2\2\u01bd\u019d\3\2"+
		"\2\2\u01bd\u01a2\3\2\2\2\u01be\13\3\2\2\2\u01bf\u01c8\5B\"\2\u01c0\u01c8"+
		"\5D#\2\u01c1\u01c8\5F$\2\u01c2\u01c8\5L\'\2\u01c3\u01c8\5N(\2\u01c4\u01c8"+
		"\5P)\2\u01c5\u01c8\5R*\2\u01c6\u01c8\5T+\2\u01c7\u01bf\3\2\2\2\u01c7\u01c0"+
		"\3\2\2\2\u01c7\u01c1\3\2\2\2\u01c7\u01c2\3\2\2\2\u01c7\u01c3\3\2\2\2\u01c7"+
		"\u01c4\3\2\2\2\u01c7\u01c5\3\2\2\2\u01c7\u01c6\3\2\2\2\u01c8\r\3\2\2\2"+
		"\u01c9\u01cb\5v<\2\u01ca\u01c9\3\2\2\2\u01ca\u01cb\3\2\2\2\u01cb\u01cc"+
		"\3\2\2\2\u01cc\u01d4\7\61\2\2\u01cd\u01ce\7\u00a6\2\2\u01ce\u01cf\7\u0158"+
		"\2\2\u01cf\u01d0\5p9\2\u01d0\u01d2\7\u0159\2\2\u01d1\u01d3\7z\2\2\u01d2"+
		"\u01d1\3\2\2\2\u01d2\u01d3\3\2\2\2\u01d3\u01d5\3\2\2\2\u01d4\u01cd\3\2"+
		"\2\2\u01d4\u01d5\3\2\2\2\u01d5\u01d7\3\2\2\2\u01d6\u01d8\7L\2\2\u01d7"+
		"\u01d6\3\2\2\2\u01d7\u01d8\3\2\2\2\u01d8\u01dc\3\2\2\2\u01d9\u01dd\5\u00ba"+
		"^\2\u01da\u01dd\5\u00e8u\2\u01db\u01dd\5<\37\2\u01dc\u01d9\3\2\2\2\u01dc"+
		"\u01da\3\2\2\2\u01dc\u01db\3\2\2\2\u01dd\u01df\3\2\2\2\u01de\u01e0\5\u00bc"+
		"_\2\u01df\u01de\3\2\2\2\u01df\u01e0\3\2\2\2\u01e0\u01e2\3\2\2\2\u01e1"+
		"\u01e3\5\26\f\2\u01e2\u01e1\3\2\2\2\u01e2\u01e3\3\2\2\2\u01e3\u01ed\3"+
		"\2\2\2\u01e4\u01e5\7L\2\2\u01e5\u01ea\5\u00a2R\2\u01e6\u01e7\7\u015a\2"+
		"\2\u01e7\u01e9\5\u00a2R\2\u01e8\u01e6\3\2\2\2\u01e9\u01ec\3\2\2\2\u01ea"+
		"\u01e8\3\2\2\2\u01ea\u01eb\3\2\2\2\u01eb\u01ee\3\2\2\2\u01ec\u01ea\3\2"+
		"\2\2\u01ed\u01e4\3\2\2\2\u01ed\u01ee\3\2\2\2\u01ee\u01fc\3\2\2\2\u01ef"+
		"\u01fa\7\u00b9\2\2\u01f0\u01fb\5~@\2\u01f1\u01f2\7&\2\2\u01f2\u01f8\7"+
		"l\2\2\u01f3\u01f5\7\u00e1\2\2\u01f4\u01f3\3\2\2\2\u01f4\u01f5\3\2\2\2"+
		"\u01f5\u01f6\3\2\2\2\u01f6\u01f9\5\u00f0y\2\u01f7\u01f9\7\u0140\2\2\u01f8"+
		"\u01f4\3\2\2\2\u01f8\u01f7\3\2\2\2\u01f9\u01fb\3\2\2\2\u01fa\u01f0\3\2"+
		"\2\2\u01fa\u01f1\3\2\2\2\u01fb\u01fd\3\2\2\2\u01fc\u01ef\3\2\2\2\u01fc"+
		"\u01fd\3\2\2\2\u01fd\u01ff\3\2\2\2\u01fe\u0200\5\u008eH\2\u01ff\u01fe"+
		"\3\2\2\2\u01ff\u0200\3\2\2\2\u0200\u0202\3\2\2\2\u0201\u0203\5\u0096L"+
		"\2\u0202\u0201\3\2\2\2\u0202\u0203\3\2\2\2\u0203\u0205\3\2\2\2\u0204\u0206"+
		"\7\u015b\2\2\u0205\u0204\3\2\2\2\u0205\u0206\3\2\2\2\u0206\17\3\2\2\2"+
		"\u0207\u0209\5v<\2\u0208\u0207\3\2\2\2\u0208\u0209\3\2\2\2\u0209\u020a"+
		"\3\2\2\2\u020a\u0212\7Z\2\2\u020b\u020c\7\u00a6\2\2\u020c\u020d\7\u0158"+
		"\2\2\u020d\u020e\5p9\2\u020e\u0210\7\u0159\2\2\u020f\u0211\7z\2\2\u0210"+
		"\u020f\3\2\2\2\u0210\u0211\3\2\2\2\u0211\u0213\3\2\2\2\u0212\u020b\3\2"+
		"\2\2\u0212\u0213\3\2\2\2\u0213\u0215\3\2\2\2\u0214\u0216\7\\\2\2\u0215"+
		"\u0214\3\2\2\2\u0215\u0216\3\2\2\2\u0216\u0219\3\2\2\2\u0217\u021a\5\u00e8"+
		"u\2\u0218\u021a\5<\37\2\u0219\u0217\3\2\2\2\u0219\u0218\3\2\2\2\u021a"+
		"\u021c\3\2\2\2\u021b\u021d\5\u00be`\2\u021c\u021b\3\2\2\2\u021c\u021d"+
		"\3\2\2\2\u021d\u0222\3\2\2\2\u021e\u021f\7\u0158\2\2\u021f\u0220\5\u00ec"+
		"w\2\u0220\u0221\7\u0159\2\2\u0221\u0223\3\2\2\2\u0222\u021e\3\2\2\2\u0222"+
		"\u0223\3\2\2\2\u0223\u0225\3\2\2\2\u0224\u0226\5\26\f\2\u0225\u0224\3"+
		"\2\2\2\u0225\u0226\3\2\2\2\u0226\u0239\3\2\2\2\u0227\u0228\7\u00b4\2\2"+
		"\u0228\u0229\7\u0158\2\2\u0229\u022a\5\u00caf\2\u022a\u0232\7\u0159\2"+
		"\2\u022b\u022c\7\u015a\2\2\u022c\u022d\7\u0158\2\2\u022d\u022e\5\u00ca"+
		"f\2\u022e\u022f\7\u0159\2\2\u022f\u0231\3\2\2\2\u0230\u022b\3\2\2\2\u0231"+
		"\u0234\3\2\2\2\u0232\u0230\3\2\2\2\u0232\u0233\3\2\2\2\u0233\u023a\3\2"+
		"\2\2\u0234\u0232\3\2\2\2\u0235\u023a\5\u00b2Z\2\u0236\u023a\5F$\2\u0237"+
		"\u0238\7\60\2\2\u0238\u023a\7\u00b4\2\2\u0239\u0227\3\2\2\2\u0239\u0235"+
		"\3\2\2\2\u0239\u0236\3\2\2\2\u0239\u0237\3\2\2\2\u023a\u023c\3\2\2\2\u023b"+
		"\u023d\5\u008eH\2\u023c\u023b\3\2\2\2\u023c\u023d\3\2\2\2\u023d\u023f"+
		"\3\2\2\2\u023e\u0240\5\u0096L\2\u023f\u023e\3\2\2\2\u023f\u0240\3\2\2"+
		"\2\u0240\u0242\3\2\2\2\u0241\u0243\7\u015b\2\2\u0242\u0241\3\2\2\2\u0242"+
		"\u0243\3\2\2\2\u0243\21\3\2\2\2\u0244\u0246\5v<\2\u0245\u0244\3\2\2\2"+
		"\u0245\u0246\3\2\2\2\u0246\u0247\3\2\2\2\u0247\u0249\5\u0086D\2\u0248"+
		"\u024a\5\u008cG\2\u0249\u0248\3\2\2\2\u0249\u024a\3\2\2\2\u024a\u024c"+
		"\3\2\2\2\u024b\u024d\5\u008eH\2\u024c\u024b\3\2\2\2\u024c\u024d\3\2\2"+
		"\2\u024d\u024f\3\2\2\2\u024e\u0250\5\u0096L\2\u024f\u024e\3\2\2\2\u024f"+
		"\u0250\3\2\2\2\u0250\u0252\3\2\2\2\u0251\u0253\7\u015b\2\2\u0252\u0251"+
		"\3\2\2\2\u0252\u0253\3\2\2\2\u0253\23\3\2\2\2\u0254\u0256\5v<\2\u0255"+
		"\u0254\3\2\2\2\u0255\u0256\3\2\2\2\u0256\u0257\3\2\2\2\u0257\u025f\7\u00b0"+
		"\2\2\u0258\u0259\7\u00a6\2\2\u0259\u025a\7\u0158\2\2\u025a\u025b\5p9\2"+
		"\u025b\u025d\7\u0159\2\2\u025c\u025e\7z\2\2\u025d\u025c\3\2\2\2\u025d"+
		"\u025e\3\2\2\2\u025e\u0260\3\2\2\2\u025f\u0258\3\2\2\2\u025f\u0260\3\2"+
		"\2\2\u0260\u0263\3\2\2\2\u0261\u0264\5\u00e8u\2\u0262\u0264\5<\37\2\u0263"+
		"\u0261\3\2\2\2\u0263\u0262\3\2\2\2\u0264\u0266\3\2\2\2\u0265\u0267\5\u00bc"+
		"_\2\u0266\u0265\3\2\2\2\u0266\u0267\3\2\2\2\u0267\u0268\3\2\2\2\u0268"+
		"\u0269\7\u009b\2\2\u0269\u026e\5z>\2\u026a\u026b\7\u015a\2\2\u026b\u026d"+
		"\5z>\2\u026c\u026a\3\2\2\2\u026d\u0270\3\2\2\2\u026e\u026c\3\2\2\2\u026e"+
		"\u026f\3\2\2\2\u026f\u0272\3\2\2\2\u0270\u026e\3\2\2\2\u0271\u0273\5\26"+
		"\f\2\u0272\u0271\3\2\2\2\u0272\u0273\3\2\2\2\u0273\u027d\3\2\2\2\u0274"+
		"\u0275\7L\2\2\u0275\u027a\5\u00a2R\2\u0276\u0277\7\u015a\2\2\u0277\u0279"+
		"\5\u00a2R\2\u0278\u0276\3\2\2\2\u0279\u027c\3\2\2\2\u027a\u0278\3\2\2"+
		"\2\u027a\u027b\3\2\2\2\u027b\u027e\3\2\2\2\u027c\u027a\3\2\2\2\u027d\u0274"+
		"\3\2\2\2\u027d\u027e\3\2\2\2\u027e\u028c\3\2\2\2\u027f\u028a\7\u00b9\2"+
		"\2\u0280\u028b\5|?\2\u0281\u0282\7&\2\2\u0282\u0288\7l\2\2\u0283\u0285"+
		"\7\u00e1\2\2\u0284\u0283\3\2\2\2\u0284\u0285\3\2\2\2\u0285\u0286\3\2\2"+
		"\2\u0286\u0289\5\u00f0y\2\u0287\u0289\7\u0140\2\2\u0288\u0284\3\2\2\2"+
		"\u0288\u0287\3\2\2\2\u0289\u028b\3\2\2\2\u028a\u0280\3\2\2\2\u028a\u0281"+
		"\3\2\2\2\u028b\u028d\3\2\2\2\u028c\u027f\3\2\2\2\u028c\u028d\3\2\2\2\u028d"+
		"\u028f\3\2\2\2\u028e\u0290\5\u008eH\2\u028f\u028e\3\2\2\2\u028f\u0290"+
		"\3\2\2\2\u0290\u0292\3\2\2\2\u0291\u0293\5\u0096L\2\u0292\u0291\3\2\2"+
		"\2\u0292\u0293\3\2\2\2\u0293\u0295\3\2\2\2\u0294\u0296\7\u015b\2\2\u0295"+
		"\u0294\3\2\2\2\u0295\u0296\3\2\2\2\u0296\25\3\2\2\2\u0297\u0298\7\u0106"+
		"\2\2\u0298\u029d\5\30\r\2\u0299\u029a\7\u015a\2\2\u029a\u029c\5\30\r\2"+
		"\u029b\u0299\3\2\2\2\u029c\u029f\3\2\2\2\u029d\u029b\3\2\2\2\u029d\u029e"+
		"\3\2\2\2\u029e\u02ab\3\2\2\2\u029f\u029d\3\2\2\2\u02a0\u02a3\7\\\2\2\u02a1"+
		"\u02a4\7\u0140\2\2\u02a2\u02a4\5\u00e2r\2\u02a3\u02a1\3\2\2\2\u02a3\u02a2"+
		"\3\2\2\2\u02a4\u02a9\3\2\2\2\u02a5\u02a6\7\u0158\2\2\u02a6\u02a7\5\u00ec"+
		"w\2\u02a7\u02a8\7\u0159\2\2\u02a8\u02aa\3\2\2\2\u02a9\u02a5\3\2\2\2\u02a9"+
		"\u02aa\3\2\2\2\u02aa\u02ac\3\2\2\2\u02ab\u02a0\3\2\2\2\u02ab\u02ac\3\2"+
		"\2\2\u02ac\27\3\2\2\2\u02ad\u02b0\5\32\16\2\u02ae\u02b0\5p9\2\u02af\u02ad"+
		"\3\2\2\2\u02af\u02ae\3\2\2\2\u02b0\u02b5\3\2\2\2\u02b1\u02b3\7\b\2\2\u02b2"+
		"\u02b1\3\2\2\2\u02b2\u02b3\3\2\2\2\u02b3\u02b4\3\2\2\2\u02b4\u02b6\5\u00c8"+
		"e\2\u02b5\u02b2\3\2\2\2\u02b5\u02b6\3\2\2\2\u02b6\31\3\2\2\2\u02b7\u02bb"+
		"\7\u00d3\2\2\u02b8\u02bb\7\u00e7\2\2\u02b9\u02bb\5\u00e2r\2\u02ba\u02b7"+
		"\3\2\2\2\u02ba\u02b8\3\2\2\2\u02ba\u02b9\3\2\2\2\u02bb\u02bc\3\2\2\2\u02bc"+
		"\u02bf\7\u0153\2\2\u02bd\u02c0\7\u015d\2\2\u02be\u02c0\5\u00eex\2\u02bf"+
		"\u02bd\3\2\2\2\u02bf\u02be\3\2\2\2\u02c0\u02c3\3\2\2\2\u02c1\u02c3\7\u013a"+
		"\2\2\u02c2\u02ba\3\2\2\2\u02c2\u02c1\3\2\2\2\u02c3\33\3\2\2\2\u02c4\u02c6"+
		"\7$\2\2\u02c5\u02c7\7\u00ae\2\2\u02c6\u02c5\3\2\2\2\u02c6\u02c7\3\2\2"+
		"\2\u02c7\u02c9\3\2\2\2\u02c8\u02ca\5\u00f4{\2\u02c9\u02c8\3\2\2\2\u02c9"+
		"\u02ca\3\2\2\2\u02ca\u02cb\3\2\2\2\u02cb\u02cc\7X\2\2\u02cc\u02cd\5\u0104"+
		"\u0083\2\u02cd\u02ce\7o\2\2\u02ce\u02cf\5\u00acW\2\u02cf\u02d0\7\u0158"+
		"\2\2\u02d0\u02d1\5\u00ecw\2\u02d1\u02d3\7\u0159\2\2\u02d2\u02d4\7\u015b"+
		"\2\2\u02d3\u02d2\3\2\2\2\u02d3\u02d4\3\2\2\2\u02d4\35\3\2\2\2\u02d5\u02d6"+
		"\7$\2\2\u02d6\u02d7\t\6\2\2\u02d7\u02da\5\u00e6t\2\u02d8\u02d9\7\u015b"+
		"\2\2\u02d9\u02db\7\u0141\2\2\u02da\u02d8\3\2\2\2\u02da\u02db\3\2\2\2\u02db"+
		"\u02ea\3\2\2\2\u02dc\u02de\7\u0158\2\2\u02dd\u02dc\3\2\2\2\u02dd\u02de"+
		"\3\2\2\2\u02de\u02df\3\2\2\2\u02df\u02e4\5 \21\2\u02e0\u02e1\7\u015a\2"+
		"\2\u02e1\u02e3\5 \21\2\u02e2\u02e0\3\2\2\2\u02e3\u02e6\3\2\2\2\u02e4\u02e2"+
		"\3\2\2\2\u02e4\u02e5\3\2\2\2\u02e5\u02e8\3\2\2\2\u02e6\u02e4\3\2\2\2\u02e7"+
		"\u02e9\7\u0159\2\2\u02e8\u02e7\3\2\2\2\u02e8\u02e9\3\2\2\2\u02e9\u02eb"+
		"\3\2\2\2\u02ea\u02dd\3\2\2\2\u02ea\u02eb\3\2\2\2\u02eb\u02f5\3\2\2\2\u02ec"+
		"\u02ed\7\u00bb\2\2\u02ed\u02f2\5\"\22\2\u02ee\u02ef\7\u015a\2\2\u02ef"+
		"\u02f1\5\"\22\2\u02f0\u02ee\3\2\2\2\u02f1\u02f4\3\2\2\2\u02f2\u02f0\3"+
		"\2\2\2\u02f2\u02f3\3\2\2\2\u02f3\u02f6\3\2\2\2\u02f4\u02f2\3\2\2\2\u02f5"+
		"\u02ec\3\2\2\2\u02f5\u02f6\3\2\2\2\u02f6\u02f9\3\2\2\2\u02f7\u02f8\7G"+
		"\2\2\u02f8\u02fa\7\u0088\2\2\u02f9\u02f7\3\2\2\2\u02f9\u02fa\3\2\2\2\u02fa"+
		"\u02fb\3\2\2\2\u02fb\u02fd\7\b\2\2\u02fc\u02fe\5\4\3\2\u02fd\u02fc\3\2"+
		"\2\2\u02fe\u02ff\3\2\2\2\u02ff\u02fd\3\2\2\2\u02ff\u0300\3\2\2\2\u0300"+
		"\37\3\2\2\2\u0301\u0305\7\u0140\2\2\u0302\u0303\5\u0104\u0083\2\u0303"+
		"\u0304\7\u0153\2\2\u0304\u0306\3\2\2\2\u0305\u0302\3\2\2\2\u0305\u0306"+
		"\3\2\2\2\u0306\u0308\3\2\2\2\u0307\u0309\7\b\2\2\u0308\u0307\3\2\2\2\u0308"+
		"\u0309\3\2\2\2\u0309\u030a\3\2\2\2\u030a\u030c\5\u00fa~\2\u030b\u030d"+
		"\7\u00b5\2\2\u030c\u030b\3\2\2\2\u030c\u030d\3\2\2\2\u030d\u0310\3\2\2"+
		"\2\u030e\u030f\7\u0147\2\2\u030f\u0311\5\u00fc\177\2\u0310\u030e\3\2\2"+
		"\2\u0310\u0311\3\2\2\2\u0311\u0313\3\2\2\2\u0312\u0314\t\7\2\2\u0313\u0312"+
		"\3\2\2\2\u0313\u0314\3\2\2\2\u0314!\3\2\2\2\u0315\u0319\7\u00d7\2\2\u0316"+
		"\u0319\7\u0111\2\2\u0317\u0319\5V,\2\u0318\u0315\3\2\2\2\u0318\u0316\3"+
		"\2\2\2\u0318\u0317\3\2\2\2\u0319#\3\2\2\2\u031a\u031b\7$\2\2\u031b\u031c"+
		"\7\u009f\2\2\u031c\u031d\5\u0104\u0083\2\u031d\u031e\7o\2\2\u031e\u031f"+
		"\5\u00acW\2\u031f\u0320\7\u0158\2\2\u0320\u0321\5\u00ecw\2\u0321\u0332"+
		"\7\u0159\2\2\u0322\u0328\7\u00bb\2\2\u0323\u0329\7\u00e0\2\2\u0324\u0325"+
		"\7\u011b\2\2\u0325\u0326\7\u0141\2\2\u0326\u0329\t\b\2\2\u0327\u0329\7"+
		"\u0125\2\2\u0328\u0323\3\2\2\2\u0328\u0324\3\2\2\2\u0328\u0327\3\2\2\2"+
		"\u0329\u032c\3\2\2\2\u032a\u032b\7\u015a\2\2\u032b\u032d\7\u00fe\2\2\u032c"+
		"\u032a\3\2\2\2\u032c\u032d\3\2\2\2\u032d\u0330\3\2\2\2\u032e\u032f\7\u015a"+
		"\2\2\u032f\u0331\5\u00f2z\2\u0330\u032e\3\2\2\2\u0330\u0331\3\2\2\2\u0331"+
		"\u0333\3\2\2\2\u0332\u0322\3\2\2\2\u0332\u0333\3\2\2\2\u0333\u0335\3\2"+
		"\2\2\u0334\u0336\7\u015b\2\2\u0335\u0334\3\2\2\2\u0335\u0336\3\2\2\2\u0336"+
		"%\3\2\2\2\u0337\u0338\7$\2\2\u0338\u0339\7\u00a1\2\2\u0339\u033a\5\u00e2"+
		"r\2\u033a\u033b\7\u0158\2\2\u033b\u0342\5\\/\2\u033c\u033e\7\u015a\2\2"+
		"\u033d\u033c\3\2\2\2\u033d\u033e\3\2\2\2\u033e\u033f\3\2\2\2\u033f\u0341"+
		"\5\\/\2\u0340\u033d\3\2\2\2\u0341\u0344\3\2\2\2\u0342\u0340\3\2\2\2\u0342"+
		"\u0343\3\2\2\2\u0343\u0346\3\2\2\2\u0344\u0342\3\2\2\2\u0345\u0347\7\u015a"+
		"\2\2\u0346\u0345\3\2\2\2\u0346\u0347\3\2\2\2\u0347\u0348\3\2\2\2\u0348"+
		"\u034c\7\u0159\2\2\u0349\u034a\7o\2\2\u034a\u034d\5\u0104\u0083\2\u034b"+
		"\u034d\7\60\2\2\u034c\u0349\3\2\2\2\u034c\u034b\3\2\2\2\u034c\u034d\3"+
		"\2\2\2\u034d\u034f\3\2\2\2\u034e\u0350\7\u015b\2\2\u034f\u034e\3\2\2\2"+
		"\u034f\u0350\3\2\2\2\u0350\'\3\2\2\2\u0351\u0352\7$\2\2\u0352\u0353\7"+
		"\u00b6\2\2\u0353\u035f\5\u00e4s\2\u0354\u0355\7\u0158\2\2\u0355\u035a"+
		"\5\u00eex\2\u0356\u0357\7\u015a\2\2\u0357\u0359\5\u00eex\2\u0358\u0356"+
		"\3\2\2\2\u0359\u035c\3\2\2\2\u035a\u0358\3\2\2\2\u035a\u035b\3\2\2\2\u035b"+
		"\u035d\3\2\2\2\u035c\u035a\3\2\2\2\u035d\u035e\7\u0159\2\2\u035e\u0360"+
		"\3\2\2\2\u035f\u0354\3\2\2\2\u035f\u0360\3\2\2\2\u0360\u036a\3\2\2\2\u0361"+
		"\u0362\7\u00bb\2\2\u0362\u0367\5*\26\2\u0363\u0364\7\u015a\2\2\u0364\u0366"+
		"\5*\26\2\u0365\u0363\3\2\2\2\u0366\u0369\3\2\2\2\u0367\u0365\3\2\2\2\u0367"+
		"\u0368\3\2\2\2\u0368\u036b\3\2\2\2\u0369\u0367\3\2\2\2\u036a\u0361\3\2"+
		"\2\2\u036a\u036b\3\2\2\2\u036b\u036c\3\2\2\2\u036c\u036d\7\b\2\2\u036d"+
		"\u0371\5\22\n\2\u036e\u036f\7\u00bb\2\2\u036f\u0370\7\26\2\2\u0370\u0372"+
		"\7u\2\2\u0371\u036e\3\2\2\2\u0371\u0372\3\2\2\2\u0372\u0374\3\2\2\2\u0373"+
		"\u0375\7\u015b\2\2\u0374\u0373\3\2\2\2\u0374\u0375\3\2\2\2\u0375)\3\2"+
		"\2\2\u0376\u0377\t\t\2\2\u0377+\3\2\2\2\u0378\u0379\7\5\2\2\u0379\u037a"+
		"\7\u00a1\2\2\u037a\u037b\5\u00e2r\2\u037b\u037c\7\u009b\2\2\u037c\u037d"+
		"\7\u0158\2\2\u037d\u037e\7\u00f0\2\2\u037e\u037f\7\u0147\2\2\u037f\u0380"+
		"\t\n\2\2\u0380\u0382\7\u0159\2\2\u0381\u0383\7\u015b\2\2\u0382\u0381\3"+
		"\2\2\2\u0382\u0383\3\2\2\2\u0383\u038d\3\2\2\2\u0384\u0385\7\5\2\2\u0385"+
		"\u0386\7\u00a1\2\2\u0386\u0387\5\u00e2r\2\u0387\u0388\7\3\2\2\u0388\u038a"+
		"\5\\/\2\u0389\u038b\7\u015b\2\2\u038a\u0389\3\2\2\2\u038a\u038b\3\2\2"+
		"\2\u038b\u038d\3\2\2\2\u038c\u0378\3\2\2\2\u038c\u0384\3\2\2\2\u038d-"+
		"\3\2\2\2\u038e\u038f\7\5\2\2\u038f\u0392\7,\2\2\u0390\u0393\5\u0104\u0083"+
		"\2\u0391\u0393\7&\2\2\u0392\u0390\3\2\2\2\u0392\u0391\3\2\2\2\u0393\u039c"+
		"\3\2\2\2\u0394\u0395\7\u00f9\2\2\u0395\u0396\7\u00fb\2\2\u0396\u0397\7"+
		"\u0147\2\2\u0397\u039d\5\u0104\u0083\2\u0398\u0399\7\33\2\2\u0399\u039d"+
		"\5\u0104\u0083\2\u039a\u039b\7\u009b\2\2\u039b\u039d\5\60\31\2\u039c\u0394"+
		"\3\2\2\2\u039c\u0398\3\2\2\2\u039c\u039a\3\2\2\2\u039d\u039f\3\2\2\2\u039e"+
		"\u03a0\7\u015b\2\2\u039f\u039e\3\2\2\2\u039f\u03a0\3\2\2\2\u03a0/\3\2"+
		"\2\2\u03a1\u03a4\5\u0104\u0083\2\u03a2\u03a5\5\u0104\u0083\2\u03a3\u03a5"+
		"\7M\2\2\u03a4\u03a2\3\2\2\2\u03a4\u03a3\3\2\2\2\u03a4\u03a5\3\2\2\2\u03a5"+
		"\61\3\2\2\2\u03a6\u03a7\78\2\2\u03a7\u03aa\7X\2\2\u03a8\u03a9\7V\2\2\u03a9"+
		"\u03ab\7A\2\2\u03aa\u03a8\3\2\2\2\u03aa\u03ab\3\2\2\2\u03ab\u03ac\3\2"+
		"\2\2\u03ac\u03ae\5\u0104\u0083\2\u03ad\u03af\7\u015b\2\2\u03ae\u03ad\3"+
		"\2\2\2\u03ae\u03af\3\2\2\2\u03af\63\3\2\2\2\u03b0\u03b1\78\2\2\u03b1\u03b4"+
		"\7\u0081\2\2\u03b2\u03b3\7V\2\2\u03b3\u03b5\7A\2\2\u03b4\u03b2\3\2\2\2"+
		"\u03b4\u03b5\3\2\2\2\u03b5\u03b6\3\2\2\2\u03b6\u03b8\5\u00e6t\2\u03b7"+
		"\u03b9\7\u015b\2\2\u03b8\u03b7\3\2\2\2\u03b8\u03b9\3\2\2\2\u03b9\65\3"+
		"\2\2\2\u03ba\u03bb\78\2\2\u03bb\u03bf\7\u009f\2\2\u03bc\u03bd\5\u00e2"+
		"r\2\u03bd\u03be\7\u0153\2\2\u03be\u03c0\3\2\2\2\u03bf\u03bc\3\2\2\2\u03bf"+
		"\u03c0\3\2\2\2\u03c0\u03c1\3\2\2\2\u03c1\u03c2\5\u0104\u0083\2\u03c2\u03c3"+
		"\7\u015b\2\2\u03c3\67\3\2\2\2\u03c4\u03c5\78\2\2\u03c5\u03c8\7\u00a1\2"+
		"\2\u03c6\u03c7\7V\2\2\u03c7\u03c9\7A\2\2\u03c8\u03c6\3\2\2\2\u03c8\u03c9"+
		"\3\2\2\2\u03c9\u03ca\3\2\2\2\u03ca\u03cc\5\u00e2r\2\u03cb\u03cd\7\u015b"+
		"\2\2\u03cc\u03cb\3\2\2\2\u03cc\u03cd\3\2\2\2\u03cd9\3\2\2\2\u03ce\u03cf"+
		"\78\2\2\u03cf\u03d2\7\u00b6\2\2\u03d0\u03d1\7V\2\2\u03d1\u03d3\7A\2\2"+
		"\u03d2\u03d0\3\2\2\2\u03d2\u03d3\3\2\2\2\u03d3\u03d4\3\2\2\2\u03d4\u03d9"+
		"\5\u00e4s\2\u03d5\u03d6\7\u015a\2\2\u03d6\u03d8\5\u00e4s\2\u03d7\u03d5"+
		"\3\2\2\2\u03d8\u03db\3\2\2\2\u03d9\u03d7\3\2\2\2\u03d9\u03da\3\2\2\2\u03da"+
		"\u03dd\3\2\2\2\u03db\u03d9\3\2\2\2\u03dc\u03de\7\u015b\2\2\u03dd\u03dc"+
		"\3\2\2\2\u03dd\u03de\3\2\2\2\u03de;\3\2\2\2\u03df\u03e2\5> \2\u03e0\u03e2"+
		"\5@!\2\u03e1\u03df\3\2\2\2\u03e1\u03e0\3\2\2\2\u03e2=\3\2\2\2\u03e3\u03e4"+
		"\7r\2\2\u03e4\u03e5\7\u0158\2\2\u03e5\u03e6\5\u0104\u0083\2\u03e6\u03e7"+
		"\7\u015a\2\2\u03e7\u03e8\7\u0143\2\2\u03e8\u03e9\7\u0159\2\2\u03e9?\3"+
		"\2\2\2\u03ea\u03eb\7q\2\2\u03eb\u03ec\7\u0158\2\2\u03ec\u03ed\7\u0143"+
		"\2\2\u03ed\u03ee\7\u015a\2\2\u03ee\u03ef\7\u0143\2\2\u03ef\u03f0\7\u0159"+
		"\2\2\u03f0\u03f2\7\u0153\2\2\u03f1\u03f3\5\u0104\u0083\2\u03f2\u03f1\3"+
		"\2\2\2\u03f2\u03f3\3\2\2\2\u03f3\u03f4\3\2\2\2\u03f4\u03f6\7\u0153\2\2"+
		"\u03f5\u03f7\5\u0104\u0083\2\u03f6\u03f5\3\2\2\2\u03f6\u03f7\3\2\2\2\u03f7"+
		"\u03f8\3\2\2\2\u03f8\u03f9\7\u0153\2\2\u03f9\u03fa\5\u0104\u0083\2\u03fa"+
		"A\3\2\2\2\u03fb\u03fc\7/\2\2\u03fc\u0401\5X-\2\u03fd\u03fe\7\u015a\2\2"+
		"\u03fe\u0400\5X-\2\u03ff\u03fd\3\2\2\2\u0400\u0403\3\2\2\2\u0401\u03ff"+
		"\3\2\2\2\u0401\u0402\3\2\2\2\u0402\u0405\3\2\2\2\u0403\u0401\3\2\2\2\u0404"+
		"\u0406\7\u015b\2\2\u0405\u0404\3\2\2\2\u0405\u0406\3\2\2\2\u0406\u0411"+
		"\3\2\2\2\u0407\u0408\7/\2\2\u0408\u040a\7\u0140\2\2\u0409\u040b\7\b\2"+
		"\2\u040a\u0409\3\2\2\2\u040a\u040b\3\2\2\2\u040b\u040c\3\2\2\2\u040c\u040e"+
		"\5Z.\2\u040d\u040f\7\u015b\2\2\u040e\u040d\3\2\2\2\u040e\u040f\3\2\2\2"+
		"\u040f\u0411\3\2\2\2\u0410\u03fb\3\2\2\2\u0410\u0407\3\2\2\2\u0411C\3"+
		"\2\2\2\u0412\u0414\7\30\2\2\u0413\u0415\7\u00e1\2\2\u0414\u0413\3\2\2"+
		"\2\u0414\u0415\3\2\2\2\u0415\u0416\3\2\2\2\u0416\u0418\5\u00f0y\2\u0417"+
		"\u0419\7\u015b\2\2\u0418\u0417\3\2\2\2\u0418\u0419\3\2\2\2\u0419\u042d"+
		"\3\2\2\2\u041a\u041c\7.\2\2\u041b\u041d\7\u00e1\2\2\u041c\u041b\3\2\2"+
		"\2\u041c\u041d\3\2\2\2\u041d\u041e\3\2\2\2\u041e\u0420\5\u00f0y\2\u041f"+
		"\u0421\7\u015b\2\2\u0420\u041f\3\2\2\2\u0420\u0421\3\2\2\2\u0421\u042d"+
		"\3\2\2\2\u0422\u042d\5h\65\2\u0423\u042d\5l\67\2\u0424\u0426\7p\2\2\u0425"+
		"\u0427\7\u00e1\2\2\u0426\u0425\3\2\2\2\u0426\u0427\3\2\2\2\u0427\u0428"+
		"\3\2\2\2\u0428\u042a\5\u00f0y\2\u0429\u042b\7\u015b\2\2\u042a\u0429\3"+
		"\2\2\2\u042a\u042b\3\2\2\2\u042b\u042d\3\2\2\2\u042c\u0412\3\2\2\2\u042c"+
		"\u041a\3\2\2\2\u042c\u0422\3\2\2\2\u042c\u0423\3\2\2\2\u042c\u0424\3\2"+
		"\2\2\u042dE\3\2\2\2\u042e\u0431\t\13\2\2\u042f\u0430\7\u0140\2\2\u0430"+
		"\u0432\7\u0147\2\2\u0431\u042f\3\2\2\2\u0431\u0432\3\2\2\2\u0432\u0433"+
		"\3\2\2\2\u0433\u043c\5\u00e6t\2\u0434\u0439\5H%\2\u0435\u0436\7\u015a"+
		"\2\2\u0436\u0438\5H%\2\u0437\u0435\3\2\2\2\u0438\u043b\3\2\2\2\u0439\u0437"+
		"\3\2\2\2\u0439\u043a\3\2\2\2\u043a\u043d\3\2\2\2\u043b\u0439\3\2\2\2\u043c"+
		"\u0434\3\2\2\2\u043c\u043d\3\2\2\2\u043d\u043f\3\2\2\2\u043e\u0440\7\u015b"+
		"\2\2\u043f\u043e\3\2\2\2\u043f\u0440\3\2\2\2\u0440\u0458\3\2\2\2\u0441"+
		"\u0442\t\13\2\2\u0442\u0443\7\u0158\2\2\u0443\u0448\5J&\2\u0444\u0445"+
		"\7\u0160\2\2\u0445\u0447\5J&\2\u0446\u0444\3\2\2\2\u0447\u044a\3\2\2\2"+
		"\u0448\u0446\3\2\2\2\u0448\u0449\3\2\2\2\u0449\u044b\3\2\2\2\u044a\u0448"+
		"\3\2\2\2\u044b\u0452\7\u0159\2\2\u044c\u044e\7\b\2\2\u044d\u044c\3\2\2"+
		"\2\u044d\u044e\3\2\2\2\u044e\u044f\3\2\2\2\u044f\u0450\t\f\2\2\u0450\u0451"+
		"\7\u0147\2\2\u0451\u0453\7\u0143\2\2\u0452\u044d\3\2\2\2\u0452\u0453\3"+
		"\2\2\2\u0453\u0455\3\2\2\2\u0454\u0456\7\u015b\2\2\u0455\u0454\3\2\2\2"+
		"\u0455\u0456\3\2\2\2\u0456\u0458\3\2\2\2\u0457\u042e\3\2\2\2\u0457\u0441"+
		"\3\2\2\2\u0458G\3\2\2\2\u0459\u045a\7\u0140\2\2\u045a\u045c\7\u0147\2"+
		"\2\u045b\u0459\3\2\2\2\u045b\u045c\3\2\2\2\u045c\u0464\3\2\2\2\u045d\u0465"+
		"\5\u00fe\u0080\2\u045e\u0460\7\u0140\2\2\u045f\u0461\t\r\2\2\u0460\u045f"+
		"\3\2\2\2\u0460\u0461\3\2\2\2\u0461\u0465\3\2\2\2\u0462\u0465\7\60\2\2"+
		"\u0463\u0465\7j\2\2\u0464\u045d\3\2\2\2\u0464\u045e\3\2\2\2\u0464\u0462"+
		"\3\2\2\2\u0464\u0463\3\2\2\2\u0465I\3\2\2\2\u0466\u0467\t\3\2\2\u0467"+
		"K\3\2\2\2\u0468\u046a\5V,\2\u0469\u046b\7\u015b\2\2\u046a\u0469\3\2\2"+
		"\2\u046a\u046b\3\2\2\2\u046b\u0479\3\2\2\2\u046c\u0473\7\u008c\2\2\u046d"+
		"\u046e\7\u0158\2\2\u046e\u046f\7\u00bb\2\2\u046f\u0470\7\u00cb\2\2\u0470"+
		"\u0471\7\u0147\2\2\u0471\u0472\7\u0140\2\2\u0472\u0474\7\u0159\2\2\u0473"+
		"\u046d\3\2\2\2\u0473\u0474\3\2\2\2\u0474\u0476\3\2\2\2\u0475\u0477\7\u015b"+
		"\2\2\u0476\u0475\3\2\2\2\u0476\u0477\3\2\2\2\u0477\u0479\3\2\2\2\u0478"+
		"\u0468\3\2\2\2\u0478\u046c\3\2\2\2\u0479M\3\2\2\2\u047a\u047b\7\u009b"+
		"\2\2\u047b\u047e\7\u0140\2\2\u047c\u047d\7\u0153\2\2\u047d\u047f\5\u0104"+
		"\u0083\2\u047e\u047c\3\2\2\2\u047e\u047f\3\2\2\2\u047f\u0480\3\2\2\2\u0480"+
		"\u0481\7\u0147\2\2\u0481\u0483\5p9\2\u0482\u0484\7\u015b\2\2\u0483\u0482"+
		"\3\2\2\2\u0483\u0484\3\2\2\2\u0484\u04a2\3\2\2\2\u0485\u0486\7\u009b\2"+
		"\2\u0486\u0487\7\u0140\2\2\u0487\u0488\5\u010a\u0086\2\u0488\u048a\5p"+
		"9\2\u0489\u048b\7\u015b\2\2\u048a\u0489\3\2\2\2\u048a\u048b\3\2\2\2\u048b"+
		"\u04a2\3\2\2\2\u048c\u048d\7\u009b\2\2\u048d\u048e\7\u0140\2\2\u048e\u048f"+
		"\7\u0147\2\2\u048f\u0490\7+\2\2\u0490\u049b\5j\66\2\u0491\u0499\7G\2\2"+
		"\u0492\u0493\7\u0084\2\2\u0493\u049a\7\u0102\2\2\u0494\u0497\7\u00b0\2"+
		"\2\u0495\u0496\7l\2\2\u0496\u0498\5\u00ecw\2\u0497\u0495\3\2\2\2\u0497"+
		"\u0498\3\2\2\2\u0498\u049a\3\2\2\2\u0499\u0492\3\2\2\2\u0499\u0494\3\2"+
		"\2\2\u049a\u049c\3\2\2\2\u049b\u0491\3\2\2\2\u049b\u049c\3\2\2\2\u049c"+
		"\u049e\3\2\2\2\u049d\u049f\7\u015b\2\2\u049e\u049d\3\2\2\2\u049e\u049f"+
		"\3\2\2\2\u049f\u04a2\3\2\2\2\u04a0\u04a2\5n8\2\u04a1\u047a\3\2\2\2\u04a1"+
		"\u0485\3\2\2\2\u04a1\u048c\3\2\2\2\u04a1\u04a0\3\2\2\2\u04a2O\3\2\2\2"+
		"\u04a3\u04a4\7\f\2\2\u04a4\u04a5\7\66\2\2\u04a5\u04a8\t\16\2\2\u04a6\u04a9"+
		"\5\u0104\u0083\2\u04a7\u04a9\7\u0140\2\2\u04a8\u04a6\3\2\2\2\u04a8\u04a7"+
		"\3\2\2\2\u04a8\u04a9\3\2\2\2\u04a9\u04ab\3\2\2\2\u04aa\u04ac\7\u015b\2"+
		"\2\u04ab\u04aa\3\2\2\2\u04ab\u04ac\3\2\2\2\u04ac\u04ef\3\2\2\2\u04ad\u04ae"+
		"\7\f\2\2\u04ae\u04b8\t\16\2\2\u04af\u04b2\5\u0104\u0083\2\u04b0\u04b2"+
		"\7\u0140\2\2\u04b1\u04af\3\2\2\2\u04b1\u04b0\3\2\2\2\u04b2\u04b6\3\2\2"+
		"\2\u04b3\u04b4\7\u00bb\2\2\u04b4\u04b5\7\u00f3\2\2\u04b5\u04b7\7\u0143"+
		"\2\2\u04b6\u04b3\3\2\2\2\u04b6\u04b7\3\2\2\2\u04b7\u04b9\3\2\2\2\u04b8"+
		"\u04b1\3\2\2\2\u04b8\u04b9\3\2\2\2\u04b9\u04bb\3\2\2\2\u04ba\u04bc\7\u015b"+
		"\2\2\u04bb\u04ba\3\2\2\2\u04bb\u04bc\3\2\2\2\u04bc\u04ef\3\2\2\2\u04bd"+
		"\u04be\7\35\2\2\u04be\u04c9\t\16\2\2\u04bf\u04c2\5\u0104\u0083\2\u04c0"+
		"\u04c2\7\u0140\2\2\u04c1\u04bf\3\2\2\2\u04c1\u04c0\3\2\2\2\u04c2\u04c7"+
		"\3\2\2\2\u04c3\u04c4\7\u00bb\2\2\u04c4\u04c5\7\u0158\2\2\u04c5\u04c6\t"+
		"\17\2\2\u04c6\u04c8\7\u0159\2\2\u04c7\u04c3\3\2\2\2\u04c7\u04c8\3\2\2"+
		"\2\u04c8\u04ca\3\2\2\2\u04c9\u04c1\3\2\2\2\u04c9\u04ca\3\2\2\2\u04ca\u04cc"+
		"\3\2\2\2\u04cb\u04cd\7\u015b\2\2\u04cc\u04cb\3\2\2\2\u04cc\u04cd\3\2\2"+
		"\2\u04cd\u04ef\3\2\2\2\u04ce\u04d0\7\35\2\2\u04cf\u04d1\7\u0137\2\2\u04d0"+
		"\u04cf\3\2\2\2\u04d0\u04d1\3\2\2\2\u04d1\u04d3\3\2\2\2\u04d2\u04d4\7\u015b"+
		"\2\2\u04d3\u04d2\3\2\2\2\u04d3\u04d4\3\2\2\2\u04d4\u04ef\3\2\2\2\u04d5"+
		"\u04d6\7\u008f\2\2\u04d6\u04d9\t\16\2\2\u04d7\u04da\5\u0104\u0083\2\u04d8"+
		"\u04da\7\u0140\2\2\u04d9\u04d7\3\2\2\2\u04d9\u04d8\3\2\2\2\u04d9\u04da"+
		"\3\2\2\2\u04da\u04dc\3\2\2\2\u04db\u04dd\7\u015b\2\2\u04dc\u04db\3\2\2"+
		"\2\u04dc\u04dd\3\2\2\2\u04dd\u04ef\3\2\2\2\u04de\u04e0\7\u008f\2\2\u04df"+
		"\u04e1\7\u0137\2\2\u04e0\u04df\3\2\2\2\u04e0\u04e1\3\2\2\2\u04e1\u04e3"+
		"\3\2\2\2\u04e2\u04e4\7\u015b\2\2\u04e3\u04e2\3\2\2\2\u04e3\u04e4\3\2\2"+
		"\2\u04e4\u04ef\3\2\2\2\u04e5\u04e6\7\u0093\2\2\u04e6\u04e9\t\16\2\2\u04e7"+
		"\u04ea\5\u0104\u0083\2\u04e8\u04ea\7\u0140\2\2\u04e9\u04e7\3\2\2\2\u04e9"+
		"\u04e8\3\2\2\2\u04e9\u04ea\3\2\2\2\u04ea\u04ec\3\2\2\2\u04eb\u04ed\7\u015b"+
		"\2\2\u04ec\u04eb\3\2\2\2\u04ec\u04ed\3\2\2\2\u04ed\u04ef\3\2\2\2\u04ee"+
		"\u04a3\3\2\2\2\u04ee\u04ad\3\2\2\2\u04ee\u04bd\3\2\2\2\u04ee\u04ce\3\2"+
		"\2\2\u04ee\u04d5\3\2\2\2\u04ee\u04de\3\2\2\2\u04ee\u04e5\3\2\2\2\u04ef"+
		"Q\3\2\2\2\u04f0\u04f2\7\u00e2\2\2\u04f1\u04f3\7\u0141\2\2\u04f2\u04f1"+
		"\3\2\2\2\u04f2\u04f3\3\2\2\2\u04f3S\3\2\2\2\u04f4\u04f5\7\u00b2\2\2\u04f5"+
		"\u04f7\5\u0104\u0083\2\u04f6\u04f8\7\u015b\2\2\u04f7\u04f6\3\2\2\2\u04f7"+
		"\u04f8\3\2\2\2\u04f8U\3\2\2\2\u04f9\u04fa\t\13\2\2\u04fa\u04fb\7\b\2\2"+
		"\u04fb\u04fc\t\20\2\2\u04fcW\3\2\2\2\u04fd\u04ff\7\u0140\2\2\u04fe\u0500"+
		"\7\b\2\2\u04ff\u04fe\3\2\2\2\u04ff\u0500\3\2\2\2\u0500\u0501\3\2\2\2\u0501"+
		"\u0504\5\u00fa~\2\u0502\u0503\7\u0147\2\2\u0503\u0505\5p9\2\u0504\u0502"+
		"\3\2\2\2\u0504\u0505\3\2\2\2\u0505Y\3\2\2\2\u0506\u0507\7\u00a1\2\2\u0507"+
		"\u0508\7\u0158\2\2\u0508\u050f\5\\/\2\u0509\u050b\7\u015a\2\2\u050a\u0509"+
		"\3\2\2\2\u050a\u050b\3\2\2\2\u050b\u050c\3\2\2\2\u050c\u050e\5\\/\2\u050d"+
		"\u050a\3\2\2\2\u050e\u0511\3\2\2\2\u050f\u050d\3\2\2\2\u050f\u0510\3\2"+
		"\2\2\u0510\u0512\3\2\2\2\u0511\u050f\3\2\2\2\u0512\u0513\7\u0159\2\2\u0513"+
		"[\3\2\2\2\u0514\u0517\5^\60\2\u0515\u0517\5b\62\2\u0516\u0514\3\2\2\2"+
		"\u0516\u0515\3\2\2\2\u0517]\3\2\2\2\u0518\u051c\5\u00eex\2\u0519\u051d"+
		"\5\u00fa~\2\u051a\u051b\7\b\2\2\u051b\u051d\5p9\2\u051c\u0519\3\2\2\2"+
		"\u051c\u051a\3\2\2\2\u051d\u0520\3\2\2\2\u051e\u051f\7\33\2\2\u051f\u0521"+
		"\5\u0104\u0083\2\u0520\u051e\3\2\2\2\u0520\u0521\3\2\2\2\u0521\u0523\3"+
		"\2\2\2\u0522\u0524\5\u00f6|\2\u0523\u0522\3\2\2\2\u0523\u0524\3\2\2\2"+
		"\u0524\u053c\3\2\2\2\u0525\u0526\7\37\2\2\u0526\u0528\5\u0104\u0083\2"+
		"\u0527\u0525\3\2\2\2\u0527\u0528\3\2\2\2\u0528\u0529\3\2\2\2\u0529\u052a"+
		"\7\60\2\2\u052a\u052d\5r:\2\u052b\u052c\7\u00bb\2\2\u052c\u052e\7\u00b4"+
		"\2\2\u052d\u052b\3\2\2\2\u052d\u052e\3\2\2\2\u052e\u053d\3\2\2\2\u052f"+
		"\u0535\7S\2\2\u0530\u0531\7\u0158\2\2\u0531\u0532\7\u0141\2\2\u0532\u0533"+
		"\7\u015a\2\2\u0533\u0534\7\u0141\2\2\u0534\u0536\7\u0159\2\2\u0535\u0530"+
		"\3\2\2\2\u0535\u0536\3\2\2\2\u0536\u053a\3\2\2\2\u0537\u0538\7i\2\2\u0538"+
		"\u0539\7G\2\2\u0539\u053b\7\u0088\2\2\u053a\u0537\3\2\2\2\u053a\u053b"+
		"\3\2\2\2\u053b\u053d\3\2\2\2\u053c\u0527\3\2\2\2\u053c\u052f\3\2\2\2\u053c"+
		"\u053d\3\2\2\2\u053d\u053f\3\2\2\2\u053e\u0540\7\u0091\2\2\u053f\u053e"+
		"\3\2\2\2\u053f\u0540\3\2\2\2\u0540\u0544\3\2\2\2\u0541\u0543\5`\61\2\u0542"+
		"\u0541\3\2\2\2\u0543\u0546\3\2\2\2\u0544\u0542\3\2\2\2\u0544\u0545\3\2"+
		"\2\2\u0545_\3\2\2\2\u0546\u0544\3\2\2\2\u0547\u0548\7\37\2\2\u0548\u054a"+
		"\5\u0104\u0083\2\u0549\u0547\3\2\2\2\u0549\u054a\3\2\2\2\u054a\u054c\3"+
		"\2\2\2\u054b\u054d\5\u00f6|\2\u054c\u054b\3\2\2\2\u054c\u054d\3\2\2\2"+
		"\u054d\u0563\3\2\2\2\u054e\u054f\7~\2\2\u054f\u0552\7_\2\2\u0550\u0552"+
		"\7\u00ae\2\2\u0551\u054e\3\2\2\2\u0551\u0550\3\2\2\2\u0552\u0554\3\2\2"+
		"\2\u0553\u0555\5\u00f4{\2\u0554\u0553\3\2\2\2\u0554\u0555\3\2\2\2\u0555"+
		"\u0557\3\2\2\2\u0556\u0558\5d\63\2\u0557\u0556\3\2\2\2\u0557\u0558\3\2"+
		"\2\2\u0558\u0564\3\2\2\2\u0559\u055d\7\26\2\2\u055a\u055b\7i\2\2\u055b"+
		"\u055c\7G\2\2\u055c\u055e\7\u0088\2\2\u055d\u055a\3\2\2\2\u055d\u055e"+
		"\3\2\2\2\u055e\u055f\3\2\2\2\u055f\u0560\7\u0158\2\2\u0560\u0561\5~@\2"+
		"\u0561\u0562\7\u0159\2\2\u0562\u0564\3\2\2\2\u0563\u0551\3\2\2\2\u0563"+
		"\u0559\3\2\2\2\u0564a\3\2\2\2\u0565\u0566\7\37\2\2\u0566\u0568\5\u0104"+
		"\u0083\2\u0567\u0565\3\2\2\2\u0567\u0568\3\2\2\2\u0568\u0585\3\2\2\2\u0569"+
		"\u056a\7~\2\2\u056a\u056d\7_\2\2\u056b\u056d\7\u00ae\2\2\u056c\u0569\3"+
		"\2\2\2\u056c\u056b\3\2\2\2\u056d\u056f\3\2\2\2\u056e\u0570\5\u00f4{\2"+
		"\u056f\u056e\3\2\2\2\u056f\u0570\3\2\2\2\u0570\u0571\3\2\2\2\u0571\u0572"+
		"\7\u0158\2\2\u0572\u0573\5\u00ecw\2\u0573\u0575\7\u0159\2\2\u0574\u0576"+
		"\5d\63\2\u0575\u0574\3\2\2\2\u0575\u0576\3\2\2\2\u0576\u0579\3\2\2\2\u0577"+
		"\u0578\7o\2\2\u0578\u057a\5\u0104\u0083\2\u0579\u0577\3\2\2\2\u0579\u057a"+
		"\3\2\2\2\u057a\u0586\3\2\2\2\u057b\u057f\7\26\2\2\u057c\u057d\7i\2\2\u057d"+
		"\u057e\7G\2\2\u057e\u0580\7\u0088\2\2\u057f\u057c\3\2\2\2\u057f\u0580"+
		"\3\2\2\2\u0580\u0581\3\2\2\2\u0581\u0582\7\u0158\2\2\u0582\u0583\5~@\2"+
		"\u0583\u0584\7\u0159\2\2\u0584\u0586\3\2\2\2\u0585\u056c\3\2\2\2\u0585"+
		"\u057b\3\2\2\2\u0586c\3\2\2\2\u0587\u0588\7\u00bb\2\2\u0588\u0589\7\u0158"+
		"\2\2\u0589\u058e\5f\64\2\u058a\u058b\7\u015a\2\2\u058b\u058d\5f\64\2\u058c"+
		"\u058a\3\2\2\2\u058d\u0590\3\2\2\2\u058e\u058c\3\2\2\2\u058e\u058f\3\2"+
		"\2\2\u058f\u0591\3\2\2\2\u0590\u058e\3\2\2\2\u0591\u0592\7\u0159\2\2\u0592"+
		"e\3\2\2\2\u0593\u0594\5\u0106\u0084\2\u0594\u0598\7\u0147\2\2\u0595\u0599"+
		"\5\u0106\u0084\2\u0596\u0599\5\u00f2z\2\u0597\u0599\7\u0141\2\2\u0598"+
		"\u0595\3\2\2\2\u0598\u0596\3\2\2\2\u0598\u0597\3\2\2\2\u0599g\3\2\2\2"+
		"\u059a\u059b\7/\2\2\u059b\u059c\5\u00f0y\2\u059c\u059e\7+\2\2\u059d\u059f"+
		"\7\u015b\2\2\u059e\u059d\3\2\2\2\u059e\u059f\3\2\2\2\u059f\u05c8\3\2\2"+
		"\2\u05a0\u05a1\7/\2\2\u05a1\u05a3\5\u00f0y\2\u05a2\u05a4\7\u00e6\2\2\u05a3"+
		"\u05a2\3\2\2\2\u05a3\u05a4\3\2\2\2\u05a4\u05a6\3\2\2\2\u05a5\u05a7\7\u011d"+
		"\2\2\u05a6\u05a5\3\2\2\2\u05a6\u05a7\3\2\2\2\u05a7\u05a8\3\2\2\2\u05a8"+
		"\u05a9\7+\2\2\u05a9\u05aa\7G\2\2\u05aa\u05b3\5\22\n\2\u05ab\u05b1\7G\2"+
		"\2\u05ac\u05ad\7\u0084\2\2\u05ad\u05b2\7\u0102\2\2\u05ae\u05b2\7\u00b0"+
		"\2\2\u05af\u05b0\7l\2\2\u05b0\u05b2\5\u00ecw\2\u05b1\u05ac\3\2\2\2\u05b1"+
		"\u05ae\3\2\2\2\u05b1\u05af\3\2\2\2\u05b2\u05b4\3\2\2\2\u05b3\u05ab\3\2"+
		"\2\2\u05b3\u05b4\3\2\2\2\u05b4\u05b6\3\2\2\2\u05b5\u05b7\7\u015b\2\2\u05b6"+
		"\u05b5\3\2\2\2\u05b6\u05b7\3\2\2\2\u05b7\u05c8\3\2\2\2\u05b8\u05b9\7/"+
		"\2\2\u05b9\u05ba\5\u00f0y\2\u05ba\u05bb\7+\2\2\u05bb\u05c2\5j\66\2\u05bc"+
		"\u05bd\7G\2\2\u05bd\u05c0\7\u00b0\2\2\u05be\u05bf\7l\2\2\u05bf\u05c1\5"+
		"\u00ecw\2\u05c0\u05be\3\2\2\2\u05c0\u05c1\3\2\2\2\u05c1\u05c3\3\2\2\2"+
		"\u05c2\u05bc\3\2\2\2\u05c2\u05c3\3\2\2\2\u05c3\u05c5\3\2\2\2\u05c4\u05c6"+
		"\7\u015b\2\2\u05c5\u05c4\3\2\2\2\u05c5\u05c6\3\2\2\2\u05c6\u05c8\3\2\2"+
		"\2\u05c7\u059a\3\2\2\2\u05c7\u05a0\3\2\2\2\u05c7\u05b8\3\2\2\2\u05c8i"+
		"\3\2\2\2\u05c9\u05cb\t\21\2\2\u05ca\u05c9\3\2\2\2\u05ca\u05cb\3\2\2\2"+
		"\u05cb\u05cd\3\2\2\2\u05cc\u05ce\t\22\2\2\u05cd\u05cc\3\2\2\2\u05cd\u05ce"+
		"\3\2\2\2\u05ce\u05d0\3\2\2\2\u05cf\u05d1\t\23\2\2\u05d0\u05cf\3\2\2\2"+
		"\u05d0\u05d1\3\2\2\2\u05d1\u05d3\3\2\2\2\u05d2\u05d4\t\24\2\2\u05d3\u05d2"+
		"\3\2\2\2\u05d3\u05d4\3\2\2\2\u05d4\u05d6\3\2\2\2\u05d5\u05d7\7\u012e\2"+
		"\2\u05d6\u05d5\3\2\2\2\u05d6\u05d7\3\2\2\2\u05d7\u05d8\3\2\2\2\u05d8\u05d9"+
		"\7G\2\2\u05d9\u05da\5\22\n\2\u05dak\3\2\2\2\u05db\u05e7\7D\2\2\u05dc\u05e5"+
		"\7\u00fa\2\2\u05dd\u05e5\7\u010c\2\2\u05de\u05e5\7\u00db\2\2\u05df\u05e5"+
		"\7\u00ed\2\2\u05e0\u05e1\7\u00be\2\2\u05e1\u05e5\5p9\2\u05e2\u05e3\7\u0112"+
		"\2\2\u05e3\u05e5\5p9\2\u05e4\u05dc\3\2\2\2\u05e4\u05dd\3\2\2\2\u05e4\u05de"+
		"\3\2\2\2\u05e4\u05df\3\2\2\2\u05e4\u05e0\3\2\2\2\u05e4\u05e2\3\2\2\2\u05e4"+
		"\u05e5\3\2\2\2\u05e5\u05e6\3\2\2\2\u05e6\u05e8\7L\2\2\u05e7\u05e4\3\2"+
		"\2\2\u05e7\u05e8\3\2\2\2\u05e8\u05ea\3\2\2\2\u05e9\u05eb\7\u00e1\2\2\u05ea"+
		"\u05e9\3\2\2\2\u05ea\u05eb\3\2\2\2\u05eb\u05ec\3\2\2\2\u05ec\u05f6\5\u00f0"+
		"y\2\u05ed\u05ee\7\\\2\2\u05ee\u05f3\7\u0140\2\2\u05ef\u05f0\7\u015a\2"+
		"\2\u05f0\u05f2\7\u0140\2\2\u05f1\u05ef\3\2\2\2\u05f2\u05f5\3\2\2\2\u05f3"+
		"\u05f1\3\2\2\2\u05f3\u05f4\3\2\2\2\u05f4\u05f7\3\2\2\2\u05f5\u05f3\3\2"+
		"\2\2\u05f6\u05ed\3\2\2\2\u05f6\u05f7\3\2\2\2\u05f7\u05f9\3\2\2\2\u05f8"+
		"\u05fa\7\u015b\2\2\u05f9\u05f8\3\2\2\2\u05f9\u05fa\3\2\2\2\u05fam\3\2"+
		"\2\2\u05fb\u05fc\7\u009b\2\2\u05fc\u0601\5\u0104\u0083\2\u05fd\u0602\5"+
		"\u0104\u0083\2\u05fe\u0602\5\u00fe\u0080\2\u05ff\u0602\7\u0140\2\2\u0600"+
		"\u0602\5\u00f2z\2\u0601\u05fd\3\2\2\2\u0601\u05fe\3\2\2\2\u0601\u05ff"+
		"\3\2\2\2\u0601\u0600\3\2\2\2\u0602\u0604\3\2\2\2\u0603\u0605\7\u015b\2"+
		"\2\u0604\u0603\3\2\2\2\u0604\u0605\3\2\2\2\u0605\u061f\3\2\2\2\u0606\u0607"+
		"\7\u009b\2\2\u0607\u0608\7\u00a8\2\2\u0608\u0609\7\u00e8\2\2\u0609\u0612"+
		"\7\u00ee\2\2\u060a\u060b\7\u0084\2\2\u060b\u0613\7\u0130\2\2\u060c\u060d"+
		"\7\u0084\2\2\u060d\u0613\7\u00c9\2\2\u060e\u060f\7\u0114\2\2\u060f\u0613"+
		"\7\u0084\2\2\u0610\u0613\7\u0122\2\2\u0611\u0613\7\u0120\2\2\u0612\u060a"+
		"\3\2\2\2\u0612\u060c\3\2\2\2\u0612\u060e\3\2\2\2\u0612\u0610\3\2\2\2\u0612"+
		"\u0611\3\2\2\2\u0613\u0615\3\2\2\2\u0614\u0616\7\u015b\2\2\u0615\u0614"+
		"\3\2\2\2\u0615\u0616\3\2\2\2\u0616\u061f\3\2\2\2\u0617\u0618\7\u009b\2"+
		"\2\u0618\u0619\7U\2\2\u0619\u061a\5\u00e2r\2\u061a\u061c\5\u00f2z\2\u061b"+
		"\u061d\7\u015b\2\2\u061c\u061b\3\2\2\2\u061c\u061d\3\2\2\2\u061d\u061f"+
		"\3\2\2\2\u061e\u05fb\3\2\2\2\u061e\u0606\3\2\2\2\u061e\u0617\3\2\2\2\u061f"+
		"o\3\2\2\2\u0620\u0621\b9\1\2\u0621\u0635\7\60\2\2\u0622\u0635\7j\2\2\u0623"+
		"\u0635\7\u0140\2\2\u0624\u0635\5\u00fe\u0080\2\u0625\u0635\5\u00b4[\2"+
		"\u0626\u0635\5\u00ccg\2\u0627\u0635\5\u00eav\2\u0628\u0629\7\u0158\2\2"+
		"\u0629\u062a\5p9\2\u062a\u062b\7\u0159\2\2\u062b\u0635\3\2\2\2\u062c\u062d"+
		"\7\u0158\2\2\u062d\u062e\5t;\2\u062e\u062f\7\u0159\2\2\u062f\u0635\3\2"+
		"\2\2\u0630\u0631\7\u0162\2\2\u0631\u0635\5p9\7\u0632\u0633\t\25\2\2\u0633"+
		"\u0635\5p9\5\u0634\u0620\3\2\2\2\u0634\u0622\3\2\2\2\u0634\u0623\3\2\2"+
		"\2\u0634\u0624\3\2\2\2\u0634\u0625\3\2\2\2\u0634\u0626\3\2\2\2\u0634\u0627"+
		"\3\2\2\2\u0634\u0628\3\2\2\2\u0634\u062c\3\2\2\2\u0634\u0630\3\2\2\2\u0634"+
		"\u0632\3\2\2\2\u0635\u0645\3\2\2\2\u0636\u0637\f\6\2\2\u0637\u0638\t\26"+
		"\2\2\u0638\u0644\5p9\7\u0639\u063a\f\4\2\2\u063a\u063b\t\27\2\2\u063b"+
		"\u0644\5p9\5\u063c\u063d\f\3\2\2\u063d\u063e\5\u0108\u0085\2\u063e\u063f"+
		"\5p9\4\u063f\u0644\3\2\2\2\u0640\u0641\f\f\2\2\u0641\u0642\7\33\2\2\u0642"+
		"\u0644\5\u0104\u0083\2\u0643\u0636\3\2\2\2\u0643\u0639\3\2\2\2\u0643\u063c"+
		"\3\2\2\2\u0643\u0640\3\2\2\2\u0644\u0647\3\2\2\2\u0645\u0643\3\2\2\2\u0645"+
		"\u0646\3\2\2\2\u0646q\3\2\2\2\u0647\u0645\3\2\2\2\u0648\u0651\7j\2\2\u0649"+
		"\u0651\5\u00fe\u0080\2\u064a\u0651\5\u00b4[\2\u064b\u0651\7\u0140\2\2"+
		"\u064c\u064d\7\u0158\2\2\u064d\u064e\5r:\2\u064e\u064f\7\u0159\2\2\u064f"+
		"\u0651\3\2\2\2\u0650\u0648\3\2\2\2\u0650\u0649\3\2\2\2\u0650\u064a\3\2"+
		"\2\2\u0650\u064b\3\2\2\2\u0650\u064c\3\2\2\2\u0651s\3\2\2\2\u0652\u0653"+
		"\5\22\n\2\u0653u\3\2\2\2\u0654\u0657\7\u00bb\2\2\u0655\u0656\7\u0139\2"+
		"\2\u0656\u0658\7\u015a\2\2\u0657\u0655\3\2\2\2\u0657\u0658\3\2\2\2\u0658"+
		"\u0659\3\2\2\2\u0659\u065e\5x=\2\u065a\u065b\7\u015a\2\2\u065b\u065d\5"+
		"x=\2\u065c\u065a\3\2\2\2\u065d\u0660\3\2\2\2\u065e\u065c\3\2\2\2\u065e"+
		"\u065f\3\2\2\2\u065fw\3\2\2\2\u0660\u065e\3\2\2\2\u0661\u0666\5\u0104"+
		"\u0083\2\u0662\u0663\7\u0158\2\2\u0663\u0664\5\u00ecw\2\u0664\u0665\7"+
		"\u0159\2\2\u0665\u0667\3\2\2\2\u0666\u0662\3\2\2\2\u0666\u0667\3\2\2\2"+
		"\u0667\u0668\3\2\2\2\u0668\u0669\7\b\2\2\u0669\u066a\7\u0158\2\2\u066a"+
		"\u066b\5\22\n\2\u066b\u066c\7\u0159\2\2\u066cy\3\2\2\2\u066d\u0670\5\u00ea"+
		"v\2\u066e\u0670\7\u0140\2\2\u066f\u066d\3\2\2\2\u066f\u066e\3\2\2\2\u0670"+
		"\u0673\3\2\2\2\u0671\u0674\7\u0147\2\2\u0672\u0674\5\u010a\u0086\2\u0673"+
		"\u0671\3\2\2\2\u0673\u0672\3\2\2\2\u0674\u0675\3\2\2\2\u0675\u067e\5p"+
		"9\2\u0676\u0677\5\u0104\u0083\2\u0677\u0678\7\u0153\2\2\u0678\u0679\5"+
		"\u0104\u0083\2\u0679\u067a\7\u0158\2\2\u067a\u067b\5\u00caf\2\u067b\u067c"+
		"\7\u0159\2\2\u067c\u067e\3\2\2\2\u067d\u066f\3\2\2\2\u067d\u0676\3\2\2"+
		"\2\u067e{\3\2\2\2\u067f\u0684\5~@\2\u0680\u0681\7\u015a\2\2\u0681\u0683"+
		"\5~@\2\u0682\u0680\3\2\2\2\u0683\u0686\3\2\2\2\u0684\u0682\3\2\2\2\u0684"+
		"\u0685\3\2\2\2\u0685}\3\2\2\2\u0686\u0684\3\2\2\2\u0687\u068c\5\u0080"+
		"A\2\u0688\u0689\7\6\2\2\u0689\u068b\5\u0080A\2\u068a\u0688\3\2\2\2\u068b"+
		"\u068e\3\2\2\2\u068c\u068a\3\2\2\2\u068c\u068d\3\2\2\2\u068d\177\3\2\2"+
		"\2\u068e\u068c\3\2\2\2\u068f\u0694\5\u0082B\2\u0690\u0691\7v\2\2\u0691"+
		"\u0693\5\u0082B\2\u0692\u0690\3\2\2\2\u0693\u0696\3\2\2\2\u0694\u0692"+
		"\3\2\2\2\u0694\u0695\3\2\2\2\u0695\u0081\3\2\2\2\u0696\u0694\3\2\2\2\u0697"+
		"\u0699\7i\2\2\u0698\u0697\3\2\2\2\u0698\u0699\3\2\2\2\u0699\u069a\3\2"+
		"\2\2\u069a\u069b\5\u0084C\2\u069b\u0083\3\2\2\2\u069c\u069d\7A\2\2\u069d"+
		"\u069e\7\u0158\2\2\u069e\u069f\5t;\2\u069f\u06a0\7\u0159\2\2\u06a0\u06d4"+
		"\3\2\2\2\u06a1\u06a2\5p9\2\u06a2\u06a3\5\u0108\u0085\2\u06a3\u06a4\5p"+
		"9\2\u06a4\u06d4\3\2\2\2\u06a5\u06a6\5p9\2\u06a6\u06a7\5\u0108\u0085\2"+
		"\u06a7\u06a8\t\30\2\2\u06a8\u06a9\7\u0158\2\2\u06a9\u06aa\5t;\2\u06aa"+
		"\u06ab\7\u0159\2\2\u06ab\u06d4\3\2\2\2\u06ac\u06ae\5p9\2\u06ad\u06af\7"+
		"i\2\2\u06ae\u06ad\3\2\2\2\u06ae\u06af\3\2\2\2\u06af\u06b0\3\2\2\2\u06b0"+
		"\u06b1\7\r\2\2\u06b1\u06b2\5p9\2\u06b2\u06b3\7\6\2\2\u06b3\u06b4\5p9\2"+
		"\u06b4\u06d4\3\2\2\2\u06b5\u06b7\5p9\2\u06b6\u06b8\7i\2\2\u06b7\u06b6"+
		"\3\2\2\2\u06b7\u06b8\3\2\2\2\u06b8\u06b9\3\2\2\2\u06b9\u06ba\7W\2\2\u06ba"+
		"\u06bd\7\u0158\2\2\u06bb\u06be\5t;\2\u06bc\u06be\5\u00caf\2\u06bd\u06bb"+
		"\3\2\2\2\u06bd\u06bc\3\2\2\2\u06be\u06bf\3\2\2\2\u06bf\u06c0\7\u0159\2"+
		"\2\u06c0\u06d4\3\2\2\2\u06c1\u06c3\5p9\2\u06c2\u06c4\7i\2\2\u06c3\u06c2"+
		"\3\2\2\2\u06c3\u06c4\3\2\2\2\u06c4\u06c5\3\2\2\2\u06c5\u06c6\7b\2\2\u06c6"+
		"\u06c9\5p9\2\u06c7\u06c8\7=\2\2\u06c8\u06ca\5p9\2\u06c9\u06c7\3\2\2\2"+
		"\u06c9\u06ca\3\2\2\2\u06ca\u06d4\3\2\2\2\u06cb\u06cc\5p9\2\u06cc\u06cd"+
		"\7]\2\2\u06cd\u06ce\5\u00f6|\2\u06ce\u06d4\3\2\2\2\u06cf\u06d0\7\u0158"+
		"\2\2\u06d0\u06d1\5~@\2\u06d1\u06d2\7\u0159\2\2\u06d2\u06d4\3\2\2\2\u06d3"+
		"\u069c\3\2\2\2\u06d3\u06a1\3\2\2\2\u06d3\u06a5\3\2\2\2\u06d3\u06ac\3\2"+
		"\2\2\u06d3\u06b5\3\2\2\2\u06d3\u06c1\3\2\2\2\u06d3\u06cb\3\2\2\2\u06d3"+
		"\u06cf\3\2\2\2\u06d4\u0085\3\2\2\2\u06d5\u06db\5\u008aF\2\u06d6\u06d7"+
		"\7\u0158\2\2\u06d7\u06d8\5\u0086D\2\u06d8\u06d9\7\u0159\2\2\u06d9\u06db"+
		"\3\2\2\2\u06da\u06d5\3\2\2\2\u06da\u06d6\3\2\2\2\u06db\u06df\3\2\2\2\u06dc"+
		"\u06de\5\u0088E\2\u06dd\u06dc\3\2\2\2\u06de\u06e1\3\2\2\2\u06df\u06dd"+
		"\3\2\2\2\u06df\u06e0\3\2\2\2\u06e0\u0087\3\2\2\2\u06e1\u06df\3\2\2\2\u06e2"+
		"\u06e4\7\u00ad\2\2\u06e3\u06e5\7\4\2\2\u06e4\u06e3\3\2\2\2\u06e4\u06e5"+
		"\3\2\2\2\u06e5\u06e9\3\2\2\2\u06e6\u06e9\7>\2\2\u06e7\u06e9\7[\2\2\u06e8"+
		"\u06e2\3\2\2\2\u06e8\u06e6\3\2\2\2\u06e8\u06e7\3\2\2\2\u06e9\u06f3\3\2"+
		"\2\2\u06ea\u06f4\5\u008aF\2\u06eb\u06ec\7\u0158\2\2\u06ec\u06ed\5\u0086"+
		"D\2\u06ed\u06ee\7\u0159\2\2\u06ee\u06f0\3\2\2\2\u06ef\u06eb\3\2\2\2\u06f0"+
		"\u06f1\3\2\2\2\u06f1\u06ef\3\2\2\2\u06f1\u06f2\3\2\2\2\u06f2\u06f4\3\2"+
		"\2\2\u06f3\u06ea\3\2\2\2\u06f3\u06ef\3\2\2\2\u06f4\u0089\3\2\2\2\u06f5"+
		"\u06f7\7\u0096\2\2\u06f6\u06f8\t\31\2\2\u06f7\u06f6\3\2\2\2\u06f7\u06f8"+
		"\3\2\2\2\u06f8\u0702\3\2\2\2\u06f9\u06fa\7\u00a6\2\2\u06fa\u06fc\5p9\2"+
		"\u06fb\u06fd\7z\2\2\u06fc\u06fb\3\2\2\2\u06fc\u06fd\3\2\2\2\u06fd\u0700"+
		"\3\2\2\2\u06fe\u06ff\7\u00bb\2\2\u06ff\u0701\7\u012a\2\2\u0700\u06fe\3"+
		"\2\2\2\u0700\u0701\3\2\2\2\u0701\u0703\3\2\2\2\u0702\u06f9\3\2\2\2\u0702"+
		"\u0703\3\2\2\2\u0703\u0704\3\2\2\2\u0704\u0707\5\u009cO\2\u0705\u0706"+
		"\7\\\2\2\u0706\u0708\5\u00e2r\2\u0707\u0705\3\2\2\2\u0707\u0708\3\2\2"+
		"\2\u0708\u0712\3\2\2\2\u0709\u070a\7L\2\2\u070a\u070f\5\u00a2R\2\u070b"+
		"\u070c\7\u015a\2\2\u070c\u070e\5\u00a2R\2\u070d\u070b\3\2\2\2\u070e\u0711"+
		"\3\2\2\2\u070f\u070d\3\2\2\2\u070f\u0710\3\2\2\2\u0710\u0713\3\2\2\2\u0711"+
		"\u070f\3\2\2\2\u0712\u0709\3\2\2\2\u0712\u0713\3\2\2\2\u0713\u0716\3\2"+
		"\2\2\u0714\u0715\7\u00b9\2\2\u0715\u0717\5~@\2\u0716\u0714\3\2\2\2\u0716"+
		"\u0717\3\2\2\2\u0717\u0722\3\2\2\2\u0718\u0719\7Q\2\2\u0719\u071a\7\21"+
		"\2\2\u071a\u071f\5\u0094K\2\u071b\u071c\7\u015a\2\2\u071c\u071e\5\u0094"+
		"K\2\u071d\u071b\3\2\2\2\u071e\u0721\3\2\2\2\u071f\u071d\3\2\2\2\u071f"+
		"\u0720\3\2\2\2\u0720\u0723\3\2\2\2\u0721\u071f\3\2\2\2\u0722\u0718\3\2"+
		"\2\2\u0722\u0723\3\2\2\2\u0723\u0726\3\2\2\2\u0724\u0725\7R\2\2\u0725"+
		"\u0727\5~@\2\u0726\u0724\3\2\2\2\u0726\u0727\3\2\2\2\u0727\u008b\3\2\2"+
		"\2\u0728\u0729\7w\2\2\u0729\u072a\7\21\2\2\u072a\u072f\5\u0092J\2\u072b"+
		"\u072c\7\u015a\2\2\u072c\u072e\5\u0092J\2\u072d\u072b\3\2\2\2\u072e\u0731"+
		"\3\2\2\2\u072f\u072d\3\2\2\2\u072f\u0730\3\2\2\2\u0730\u073d\3\2\2\2\u0731"+
		"\u072f\3\2\2\2\u0732\u0733\7\u0101\2\2\u0733\u0734\5p9\2\u0734\u073b\t"+
		"\32\2\2\u0735\u0736\7D\2\2\u0736\u0737\t\33\2\2\u0737\u0738\5p9\2\u0738"+
		"\u0739\t\32\2\2\u0739\u073a\7\u0102\2\2\u073a\u073c\3\2\2\2\u073b\u0735"+
		"\3\2\2\2\u073b\u073c\3\2\2\2\u073c\u073e\3\2\2\2\u073d\u0732\3\2\2\2\u073d"+
		"\u073e\3\2\2\2\u073e\u008d\3\2\2\2\u073f\u0740\7G\2\2\u0740\u0753\7\17"+
		"\2\2\u0741\u0742\7G\2\2\u0742\u0743\7\u0138\2\2\u0743\u0745\7\u00c0\2"+
		"\2\u0744\u0746\5\u0090I\2\u0745\u0744\3\2\2\2\u0745\u0746\3\2\2\2\u0746"+
		"\u0753\3\2\2\2\u0747\u0748\7G\2\2\u0748\u0749\7\u0138\2\2\u0749\u074d"+
		"\7\u010a\2\2\u074a\u074b\7\u0158\2\2\u074b\u074c\7\u0143\2\2\u074c\u074e"+
		"\7\u0159\2\2\u074d\u074a\3\2\2\2\u074d\u074e\3\2\2\2\u074e\u0750\3\2\2"+
		"\2\u074f\u0751\5\u0090I\2\u0750\u074f\3\2\2\2\u0750\u0751\3\2\2\2\u0751"+
		"\u0753\3\2\2\2\u0752\u073f\3\2\2\2\u0752\u0741\3\2\2\2\u0752\u0747\3\2"+
		"\2\2\u0753\u008f\3\2\2\2\u0754\u0759\7\u015a\2\2\u0755\u0756\7\u0144\2"+
		"\2\u0756\u075a\7\u00c2\2\2\u0757\u075a\7\u012d\2\2\u0758\u075a\7\u0116"+
		"\2\2\u0759\u0755\3\2\2\2\u0759\u0757\3\2\2\2\u0759\u0758\3\2\2\2\u075a"+
		"\u0091\3\2\2\2\u075b\u075d\5p9\2\u075c\u075e\t\34\2\2\u075d\u075c\3\2"+
		"\2\2\u075d\u075e\3\2\2\2\u075e\u0093\3\2\2\2\u075f\u0760\5p9\2\u0760\u0095"+
		"\3\2\2\2\u0761\u0762\7u\2\2\u0762\u0763\7\u0158\2\2\u0763\u0768\5\u0098"+
		"M\2\u0764\u0765\7\u015a\2\2\u0765\u0767\5\u0098M\2\u0766\u0764\3\2\2\2"+
		"\u0767\u076a\3\2\2\2\u0768\u0766\3\2\2\2\u0768\u0769\3\2\2\2\u0769\u076b"+
		"\3\2\2\2\u076a\u0768\3\2\2\2\u076b\u076c\7\u0159\2\2\u076c\u0097\3\2\2"+
		"\2\u076d\u076e\7\u00d9\2\2\u076e\u079b\7\u0141\2\2\u076f\u0770\t\35\2"+
		"\2\u0770\u079b\7Q\2\2\u0771\u0772\t\36\2\2\u0772\u079b\7\u00ad\2\2\u0773"+
		"\u0774\t\37\2\2\u0774\u079b\7^\2\2\u0775\u0776\7\u00d8\2\2\u0776\u079b"+
		"\7\u0136\2\2\u0777\u0778\7\u00dc\2\2\u0778\u079b\7w\2\2\u0779\u079b\7"+
		"\u00eb\2\2\u077a\u077b\7\u00e9\2\2\u077b\u079b\7|\2\2\u077c\u077d\7\u00ea"+
		"\2\2\u077d\u079b\7|\2\2\u077e\u077f\7\u00f5\2\2\u077f\u079b\7\u0141\2"+
		"\2\u0780\u0781\7\u00f6\2\2\u0781\u079b\7\u0141\2\2\u0782\u0783\7\u0104"+
		"\2\2\u0783\u0784\7G\2\2\u0784\u0785\7\u0158\2\2\u0785\u078a\5\u009aN\2"+
		"\u0786\u0787\7\u015a\2\2\u0787\u0789\5\u009aN\2\u0788\u0786\3\2\2\2\u0789"+
		"\u078c\3\2\2\2\u078a\u0788\3\2\2\2\u078a\u078b\3\2\2\2\u078b\u078d\3\2"+
		"\2\2\u078c\u078a\3\2\2\2\u078d\u078e\7\u0159\2\2\u078e\u079b\3\2\2\2\u078f"+
		"\u0790\7\u0104\2\2\u0790\u0791\7G\2\2\u0791\u079b\7\u0131\2\2\u0792\u0793"+
		"\7\u0108\2\2\u0793\u079b\t \2\2\u0794\u079b\7\u0111\2\2\u0795\u0796\7"+
		"\u0115\2\2\u0796\u079b\7|\2\2\u0797\u0798\7\u00b2\2\2\u0798\u0799\7|\2"+
		"\2\u0799\u079b\7\u0143\2\2\u079a\u076d\3\2\2\2\u079a\u076f\3\2\2\2\u079a"+
		"\u0771\3\2\2\2\u079a\u0773\3\2\2\2\u079a\u0775\3\2\2\2\u079a\u0777\3\2"+
		"\2\2\u079a\u0779\3\2\2\2\u079a\u077a\3\2\2\2\u079a\u077c\3\2\2\2\u079a"+
		"\u077e\3\2\2\2\u079a\u0780\3\2\2\2\u079a\u0782\3\2\2\2\u079a\u078f\3\2"+
		"\2\2\u079a\u0792\3\2\2\2\u079a\u0794\3\2\2\2\u079a\u0795\3\2\2\2\u079a"+
		"\u0797\3\2\2\2\u079b\u0099\3\2\2\2\u079c\u07a0\7\u0140\2\2\u079d\u07a1"+
		"\7\u0131\2\2\u079e\u079f\7\u0147\2\2\u079f\u07a1\5\u00fe\u0080\2\u07a0"+
		"\u079d\3\2\2\2\u07a0\u079e\3\2\2\2\u07a1\u009b\3\2\2\2\u07a2\u07a7\5\u009e"+
		"P\2\u07a3\u07a4\7\u015a\2\2\u07a4\u07a6\5\u009eP\2\u07a5\u07a3\3\2\2\2"+
		"\u07a6\u07a9\3\2\2\2\u07a7\u07a5\3\2\2\2\u07a7\u07a8\3\2\2\2\u07a8\u009d"+
		"\3\2\2\2\u07a9\u07a7\3\2\2\2\u07aa\u07ab\5\u00e2r\2\u07ab\u07ac\7\u0153"+
		"\2\2\u07ac\u07ae\3\2\2\2\u07ad\u07aa\3\2\2\2\u07ad\u07ae\3\2\2\2\u07ae"+
		"\u07b2\3\2\2\2\u07af\u07b3\7\u015d\2\2\u07b0\u07b1\7\u0157\2\2\u07b1\u07b3"+
		"\t!\2\2\u07b2\u07af\3\2\2\2\u07b2\u07b0\3\2\2\2\u07b3\u07c0\3\2\2\2\u07b4"+
		"\u07b5\5\u00c8e\2\u07b5\u07b6\7\u0147\2\2\u07b6\u07b7\5p9\2\u07b7\u07c0"+
		"\3\2\2\2\u07b8\u07bd\5p9\2\u07b9\u07bb\7\b\2\2\u07ba\u07b9\3\2\2\2\u07ba"+
		"\u07bb\3\2\2\2\u07bb\u07bc\3\2\2\2\u07bc\u07be\5\u00c8e\2\u07bd\u07ba"+
		"\3\2\2\2\u07bd\u07be\3\2\2\2\u07be\u07c0\3\2\2\2\u07bf\u07ad\3\2\2\2\u07bf"+
		"\u07b4\3\2\2\2\u07bf\u07b8\3\2\2\2\u07c0\u009f\3\2\2\2\u07c1\u07c2\7\u0109"+
		"\2\2\u07c2\u07c3\7\21\2\2\u07c3\u07c4\5\u00caf\2\u07c4\u00a1\3\2\2\2\u07c5"+
		"\u07cb\5\u00a4S\2\u07c6\u07c7\7\u0158\2\2\u07c7\u07c8\5\u00a4S\2\u07c8"+
		"\u07c9\7\u0159\2\2\u07c9\u07cb\3\2\2\2\u07ca\u07c5\3\2\2\2\u07ca\u07c6"+
		"\3\2\2\2\u07cb\u00a3\3\2\2\2\u07cc\u07d0\5\u00a6T\2\u07cd\u07cf\5\u00aa"+
		"V\2\u07ce\u07cd\3\2\2\2\u07cf\u07d2\3\2\2\2\u07d0\u07ce\3\2\2\2\u07d0"+
		"\u07d1\3\2\2\2\u07d1\u00a5\3\2\2\2\u07d2\u07d0\3\2\2\2\u07d3\u07d5\5\u00ac"+
		"W\2\u07d4\u07d6\5\u00b8]\2\u07d5\u07d4\3\2\2\2\u07d5\u07d6\3\2\2\2\u07d6"+
		"\u07f7\3\2\2\2\u07d7\u07d9\5\u00aeX\2\u07d8\u07da\5\u00b8]\2\u07d9\u07d8"+
		"\3\2\2\2\u07d9\u07da\3\2\2\2\u07da\u07f7\3\2\2\2\u07db\u07e0\5\u00b2Z"+
		"\2\u07dc\u07de\5\u00b8]\2\u07dd\u07df\5\u00c6d\2\u07de\u07dd\3\2\2\2\u07de"+
		"\u07df\3\2\2\2\u07df\u07e1\3\2\2\2\u07e0\u07dc\3\2\2\2\u07e0\u07e1\3\2"+
		"\2\2\u07e1\u07f7\3\2\2\2\u07e2\u07e3\5\u00a8U\2\u07e3\u07e4\5\u00b8]\2"+
		"\u07e4\u07f7\3\2\2\2\u07e5\u07e7\5\u00b4[\2\u07e6\u07e8\5\u00b8]\2\u07e7"+
		"\u07e6\3\2\2\2\u07e7\u07e8\3\2\2\2\u07e8\u07f7\3\2\2\2\u07e9\u07eb\7\u0140"+
		"\2\2\u07ea\u07ec\5\u00b8]\2\u07eb\u07ea\3\2\2\2\u07eb\u07ec\3\2\2\2\u07ec"+
		"\u07f7\3\2\2\2\u07ed\u07ee\7\u0140\2\2\u07ee\u07ef\7\u0153\2\2\u07ef\u07f4"+
		"\5\u00b4[\2\u07f0\u07f2\5\u00b8]\2\u07f1\u07f3\5\u00c6d\2\u07f2\u07f1"+
		"\3\2\2\2\u07f2\u07f3\3\2\2\2\u07f3\u07f5\3\2\2\2\u07f4\u07f0\3\2\2\2\u07f4"+
		"\u07f5\3\2\2\2\u07f5\u07f7\3\2\2\2\u07f6\u07d3\3\2\2\2\u07f6\u07d7\3\2"+
		"\2\2\u07f6\u07db\3\2\2\2\u07f6\u07e2\3\2\2\2\u07f6\u07e5\3\2\2\2\u07f6"+
		"\u07e9\3\2\2\2\u07f6\u07ed\3\2\2\2\u07f7\u00a7\3\2\2\2\u07f8\u07f9\7\24"+
		"\2\2\u07f9\u07fa\7\u0158\2\2\u07fa\u07fb\7\25\2\2\u07fb\u07fc\5\u00e2"+
		"r\2\u07fc\u07fd\7\u015a\2\2\u07fd\u07fe\t\"\2\2\u07fe\u07ff\7\u0159\2"+
		"\2\u07ff\u00a9\3\2\2\2\u0800\u0802\7Y\2\2\u0801\u0800\3\2\2\2\u0801\u0802"+
		"\3\2\2\2\u0802\u0808\3\2\2\2\u0803\u0805\t#\2\2\u0804\u0806\7x\2\2\u0805"+
		"\u0804\3\2\2\2\u0805\u0806\3\2\2\2\u0806\u0808\3\2\2\2\u0807\u0801\3\2"+
		"\2\2\u0807\u0803\3\2\2\2\u0808\u080a\3\2\2\2\u0809\u080b\t$\2\2\u080a"+
		"\u0809\3\2\2\2\u080a\u080b\3\2\2\2\u080b\u080c\3\2\2\2\u080c\u080d\7^"+
		"\2\2\u080d\u080e\5\u00a2R\2\u080e\u080f\7o\2\2\u080f\u0810\5~@\2\u0810"+
		"\u081b\3\2\2\2\u0811\u0812\7%\2\2\u0812\u0813\7^\2\2\u0813\u081b\5\u00a2"+
		"R\2\u0814\u0815\7%\2\2\u0815\u0816\7\u00bf\2\2\u0816\u081b\5\u00a2R\2"+
		"\u0817\u0818\7x\2\2\u0818\u0819\7\u00bf\2\2\u0819\u081b\5\u00a2R\2\u081a"+
		"\u0807\3\2\2\2\u081a\u0811\3\2\2\2\u081a\u0814\3\2\2\2\u081a\u0817\3\2"+
		"\2\2\u081b\u00ab\3\2\2\2\u081c\u081e\5\u00e2r\2\u081d\u081f\5\u00bc_\2"+
		"\u081e\u081d\3\2\2\2\u081e\u081f\3\2\2\2\u081f\u00ad\3\2\2\2\u0820\u0821"+
		"\7s\2\2\u0821\u0822\7\u0158\2\2\u0822\u0823\7\20\2\2\u0823\u0824\7\u0143"+
		"\2\2\u0824\u082e\7\u015a\2\2\u0825\u082a\5\u00b0Y\2\u0826\u0827\7\u015a"+
		"\2\2\u0827\u0829\5\u00b0Y\2\u0828\u0826\3\2\2\2\u0829\u082c\3\2\2\2\u082a"+
		"\u0828\3\2\2\2\u082a\u082b\3\2\2\2\u082b\u082f\3\2\2\2\u082c\u082a\3\2"+
		"\2\2\u082d\u082f\5\u0104\u0083\2\u082e\u0825\3\2\2\2\u082e\u082d\3\2\2"+
		"\2\u082f\u0830\3\2\2\2\u0830\u0831\7\u0159\2\2\u0831\u00af\3\2\2\2\u0832"+
		"\u0833\5\u0104\u0083\2\u0833\u0834\7\u0147\2\2\u0834\u0835\t%\2\2\u0835"+
		"\u00b1\3\2\2\2\u0836\u083c\5t;\2\u0837\u0838\7\u0158\2\2\u0838\u0839\5"+
		"t;\2\u0839\u083a\7\u0159\2\2\u083a\u083c\3\2\2\2\u083b\u0836\3\2\2\2\u083b"+
		"\u0837\3\2\2\2\u083c\u00b3\3\2\2\2\u083d\u089f\5\u00ceh\2\u083e\u089f"+
		"\5\u00d0i\2\u083f\u0840\5\u00f8}\2\u0840\u0842\7\u0158\2\2\u0841\u0843"+
		"\5\u00caf\2\u0842\u0841\3\2\2\2\u0842\u0843\3\2\2\2\u0843\u0844\3\2\2"+
		"\2\u0844\u0845\7\u0159\2\2\u0845\u089f\3\2\2\2\u0846\u0847\7\u00c3\2\2"+
		"\u0847\u0848\7\u0158\2\2\u0848\u0849\7\u015d\2\2\u0849\u089f\7\u0159\2"+
		"\2\u084a\u084b\7\u00c5\2\2\u084b\u084c\7\u0158\2\2\u084c\u084d\5p9\2\u084d"+
		"\u084e\7\b\2\2\u084e\u084f\5\u00fa~\2\u084f\u0850\7\u0159\2\2\u0850\u089f"+
		"\3\2\2\2\u0851\u0852\7#\2\2\u0852\u0853\7\u0158\2\2\u0853\u0854\5\u00fa"+
		"~\2\u0854\u0855\7\u015a\2\2\u0855\u0858\5p9\2\u0856\u0857\7\u015a\2\2"+
		"\u0857\u0859\5p9\2\u0858\u0856\3\2\2\2\u0858\u0859\3\2\2\2\u0859\u085a"+
		"\3\2\2\2\u085a\u085b\7\u0159\2\2\u085b\u089f\3\2\2\2\u085c\u085d\7\u00c7"+
		"\2\2\u085d\u085e\7\u0158\2\2\u085e\u085f\7\u015d\2\2\u085f\u089f\7\u0159"+
		"\2\2\u0860\u0861\7\32\2\2\u0861\u0862\7\u0158\2\2\u0862\u0863\5\u00ca"+
		"f\2\u0863\u0864\7\u0159\2\2\u0864\u089f\3\2\2\2\u0865\u089f\7)\2\2\u0866"+
		"\u089f\7*\2\2\u0867\u0868\7\u00ce\2\2\u0868\u0869\7\u0158\2\2\u0869\u086a"+
		"\5\u00b6\\\2\u086a\u086b\7\u015a\2\2\u086b\u086c\5p9\2\u086c\u086d\7\u015a"+
		"\2\2\u086d\u086e\5p9\2\u086e\u086f\7\u0159\2\2\u086f\u089f\3\2\2\2\u0870"+
		"\u0871\7\u00cf\2\2\u0871\u0872\7\u0158\2\2\u0872\u0873\5\u00b6\\\2\u0873"+
		"\u0874\7\u015a\2\2\u0874\u0875\5p9\2\u0875\u0876\7\u015a\2\2\u0876\u0877"+
		"\5p9\2\u0877\u0878\7\u0159\2\2\u0878\u089f\3\2\2\2\u0879\u087a\7\u00d0"+
		"\2\2\u087a\u087b\7\u0158\2\2\u087b\u087c\5\u00b6\\\2\u087c\u087d\7\u015a"+
		"\2\2\u087d\u087e\5p9\2\u087e\u087f\7\u0159\2\2\u087f\u089f\3\2\2\2\u0880"+
		"\u0881\7\u00d1\2\2\u0881\u0882\7\u0158\2\2\u0882\u0883\5\u00b6\\\2\u0883"+
		"\u0884\7\u015a\2\2\u0884\u0885\5p9\2\u0885\u0886\7\u0159\2\2\u0886\u089f"+
		"\3\2\2\2\u0887\u0888\7S\2\2\u0888\u0889\7\u0158\2\2\u0889\u088c\5\u00fa"+
		"~\2\u088a\u088b\7\u015a\2\2\u088b\u088d\7\u0141\2\2\u088c\u088a\3\2\2"+
		"\2\u088c\u088d\3\2\2\2\u088d\u0890\3\2\2\2\u088e\u088f\7\u015a\2\2\u088f"+
		"\u0891\7\u0141\2\2\u0890\u088e\3\2\2\2\u0890\u0891\3\2\2\2\u0891\u0892"+
		"\3\2\2\2\u0892\u0893\7\u0159\2\2\u0893\u089f\3\2\2\2\u0894\u089f\7\u00f8"+
		"\2\2\u0895\u0896\7k\2\2\u0896\u0897\7\u0158\2\2\u0897\u0898\5p9\2\u0898"+
		"\u0899\7\u015a\2\2\u0899\u089a\5p9\2\u089a\u089b\7\u0159\2\2\u089b\u089f"+
		"\3\2\2\2\u089c\u089f\7\u009a\2\2\u089d\u089f\7\u00a0\2\2\u089e\u083d\3"+
		"\2\2\2\u089e\u083e\3\2\2\2\u089e\u083f\3\2\2\2\u089e\u0846\3\2\2\2\u089e"+
		"\u084a\3\2\2\2\u089e\u0851\3\2\2\2\u089e\u085c\3\2\2\2\u089e\u0860\3\2"+
		"\2\2\u089e\u0865\3\2\2\2\u089e\u0866\3\2\2\2\u089e\u0867\3\2\2\2\u089e"+
		"\u0870\3\2\2\2\u089e\u0879\3\2\2\2\u089e\u0880\3\2\2\2\u089e\u0887\3\2"+
		"\2\2\u089e\u0894\3\2\2\2\u089e\u0895\3\2\2\2\u089e\u089c\3\2\2\2\u089e"+
		"\u089d\3\2\2\2\u089f\u00b5\3\2\2\2\u08a0\u08a1\7\u0142\2\2\u08a1\u00b7"+
		"\3\2\2\2\u08a2\u08a4\7\b\2\2\u08a3\u08a2\3\2\2\2\u08a3\u08a4\3\2\2\2\u08a4"+
		"\u08a5\3\2\2\2\u08a5\u08a6\5\u00ba^\2\u08a6\u00b9\3\2\2\2\u08a7\u08a9"+
		"\5\u0104\u0083\2\u08a8\u08aa\5\u00bc_\2\u08a9\u08a8\3\2\2\2\u08a9\u08aa"+
		"\3\2\2\2\u08aa\u00bb\3\2\2\2\u08ab\u08ad\7\u00bb\2\2\u08ac\u08ab\3\2\2"+
		"\2\u08ac\u08ad\3\2\2\2\u08ad\u08ae\3\2\2\2\u08ae\u08af\7\u0158\2\2\u08af"+
		"\u08b4\5\u00c0a\2\u08b0\u08b1\7\u015a\2\2\u08b1\u08b3\5\u00c0a\2\u08b2"+
		"\u08b0\3\2\2\2\u08b3\u08b6\3\2\2\2\u08b4\u08b2\3\2\2\2\u08b4\u08b5\3\2"+
		"\2\2\u08b5\u08b7\3\2\2\2\u08b6\u08b4\3\2\2\2\u08b7\u08b8\7\u0159\2\2\u08b8"+
		"\u00bd\3\2\2\2\u08b9\u08ba\7\u00bb\2\2\u08ba\u08bb\7\u0158\2\2\u08bb\u08c0"+
		"\5\u00c0a\2\u08bc\u08bd\7\u015a\2\2\u08bd\u08bf\5\u00c0a\2\u08be\u08bc"+
		"\3\2\2\2\u08bf\u08c2\3\2\2\2\u08c0\u08be\3\2\2\2\u08c0\u08c1\3\2\2\2\u08c1"+
		"\u08c3\3\2\2\2\u08c2\u08c0\3\2\2\2\u08c3\u08c4\7\u0159\2\2\u08c4\u00bf"+
		"\3\2\2\2\u08c5\u08c7\7\u00fd\2\2\u08c6\u08c5\3\2\2\2\u08c6\u08c7\3\2\2"+
		"\2\u08c7\u08ee\3\2\2\2\u08c8\u08c9\7X\2\2\u08c9\u08ca\7\u0158\2\2\u08ca"+
		"\u08cf\5\u00c4c\2\u08cb\u08cc\7\u015a\2\2\u08cc\u08ce\5\u00c4c\2";
	private static final String _serializedATNSegment1 =
		"\u08cd\u08cb\3\2\2\2\u08ce\u08d1\3\2\2\2\u08cf\u08cd\3\2\2\2\u08cf\u08d0"+
		"\3\2\2\2\u08d0\u08d2\3\2\2\2\u08d1\u08cf\3\2\2\2\u08d2\u08d3\7\u0159\2"+
		"\2\u08d3\u08ef\3\2\2\2\u08d4\u08d5\7X\2\2\u08d5\u08d6\7\u0147\2\2\u08d6"+
		"\u08ef\5\u00c4c\2\u08d7\u08e6\7H\2\2\u08d8\u08d9\7\u0158\2\2\u08d9\u08da"+
		"\5\u00c4c\2\u08da\u08db\7\u0158\2\2\u08db\u08e0\5\u00c2b\2\u08dc\u08dd"+
		"\7\u015a\2\2\u08dd\u08df\5\u00c2b\2\u08de\u08dc\3\2\2\2\u08df\u08e2\3"+
		"\2\2\2\u08e0\u08de\3\2\2\2\u08e0\u08e1\3\2\2\2\u08e1\u08e3\3\2\2\2\u08e2"+
		"\u08e0\3\2\2\2\u08e3\u08e4\7\u0159\2\2\u08e4\u08e5\7\u0159\2\2\u08e5\u08e7"+
		"\3\2\2\2\u08e6\u08d8\3\2\2\2\u08e6\u08e7\3\2\2\2\u08e7\u08ef\3\2\2\2\u08e8"+
		"\u08ef\7\u0120\2\2\u08e9\u08ef\7\u0122\2\2\u08ea\u08eb\7\u0123\2\2\u08eb"+
		"\u08ec\7\u0147\2\2\u08ec\u08ef\7\u0141\2\2\u08ed\u08ef\7\u0142\2\2\u08ee"+
		"\u08c8\3\2\2\2\u08ee\u08d4\3\2\2\2\u08ee\u08d7\3\2\2\2\u08ee\u08e8\3\2"+
		"\2\2\u08ee\u08e9\3\2\2\2\u08ee\u08ea\3\2\2\2\u08ee\u08ed\3\2\2\2\u08ee"+
		"\u08ef\3\2\2\2\u08ef\u00c1\3\2\2\2\u08f0\u08f1\7\u0142\2\2\u08f1\u00c3"+
		"\3\2\2\2\u08f2\u08f3\t&\2\2\u08f3\u00c5\3\2\2\2\u08f4\u08f5\7\u0158\2"+
		"\2\u08f5\u08fa\5\u00c8e\2\u08f6\u08f7\7\u015a\2\2\u08f7\u08f9\5\u00c8"+
		"e\2\u08f8\u08f6\3\2\2\2\u08f9\u08fc\3\2\2\2\u08fa\u08f8\3\2\2\2\u08fa"+
		"\u08fb\3\2\2\2\u08fb\u08fd\3\2\2\2\u08fc\u08fa\3\2\2\2\u08fd\u08fe\7\u0159"+
		"\2\2\u08fe\u00c7\3\2\2\2\u08ff\u0902\5\u0104\u0083\2\u0900\u0902\7\u0143"+
		"\2\2\u0901\u08ff\3\2\2\2\u0901\u0900\3\2\2\2\u0902\u00c9\3\2\2\2\u0903"+
		"\u0908\5p9\2\u0904\u0905\7\u015a\2\2\u0905\u0907\5p9\2\u0906\u0904\3\2"+
		"\2\2\u0907\u090a\3\2\2\2\u0908\u0906\3\2\2\2\u0908\u0909\3\2\2\2\u0909"+
		"\u00cb\3\2\2\2\u090a\u0908\3\2\2\2\u090b\u090c\7\23\2\2\u090c\u0912\5"+
		"p9\2\u090d\u090e\7\u00b8\2\2\u090e\u090f\5p9\2\u090f\u0910\7\u00a4\2\2"+
		"\u0910\u0911\5p9\2\u0911\u0913\3\2\2\2\u0912\u090d\3\2\2\2\u0913\u0914"+
		"\3\2\2\2\u0914\u0912\3\2\2\2\u0914\u0915\3\2\2\2\u0915\u0918\3\2\2\2\u0916"+
		"\u0917\7:\2\2\u0917\u0919\5p9\2\u0918\u0916\3\2\2\2\u0918\u0919\3\2\2"+
		"\2\u0919\u091a\3\2\2\2\u091a\u091b\7;\2\2\u091b\u092d\3\2\2\2\u091c\u0922"+
		"\7\23\2\2\u091d\u091e\7\u00b8\2\2\u091e\u091f\5~@\2\u091f\u0920\7\u00a4"+
		"\2\2\u0920\u0921\5p9\2\u0921\u0923\3\2\2\2\u0922\u091d\3\2\2\2\u0923\u0924"+
		"\3\2\2\2\u0924\u0922\3\2\2\2\u0924\u0925\3\2\2\2\u0925\u0928\3\2\2\2\u0926"+
		"\u0927\7:\2\2\u0927\u0929\5p9\2\u0928\u0926\3\2\2\2\u0928\u0929\3\2\2"+
		"\2\u0929\u092a\3\2\2\2\u092a\u092b\7;\2\2\u092b\u092d\3\2\2\2\u092c\u090b"+
		"\3\2\2\2\u092c\u091c\3\2\2\2\u092d\u00cd\3\2\2\2\u092e\u092f\7\u010e\2"+
		"\2\u092f\u0930\7\u0158\2\2\u0930\u0931\7\u0159\2\2\u0931\u0941\5\u00d4"+
		"k\2\u0932\u0933\7\u00d4\2\2\u0933\u0934\7\u0158\2\2\u0934\u0935\7\u0159"+
		"\2\2\u0935\u0941\5\u00d4k\2\u0936\u0937\7\u00ff\2\2\u0937\u0938\7\u0158"+
		"\2\2\u0938\u0939\5p9\2\u0939\u093a\7\u0159\2\2\u093a\u093b\5\u00d4k\2"+
		"\u093b\u0941\3\2\2\2\u093c\u093d\7\u011a\2\2\u093d\u093e\7\u0158\2\2\u093e"+
		"\u093f\7\u0159\2\2\u093f\u0941\5\u00d4k\2\u0940\u092e\3\2\2\2\u0940\u0932"+
		"\3\2\2\2\u0940\u0936\3\2\2\2\u0940\u093c\3\2\2\2\u0941\u00cf\3\2\2\2\u0942"+
		"\u0943\7\u00c1\2\2\u0943\u0944\7\u0158\2\2\u0944\u0945\5\u00d2j\2\u0945"+
		"\u0947\7\u0159\2\2\u0946\u0948\5\u00d4k\2\u0947\u0946\3\2\2\2\u0947\u0948"+
		"\3\2\2\2\u0948\u099e\3\2\2\2\u0949\u094a\7\u00c8\2\2\u094a\u094b\7\u0158"+
		"\2\2\u094b\u094c\5\u00d2j\2\u094c\u094d\7\u0159\2\2\u094d\u099e\3\2\2"+
		"\2\u094e\u094f\7\u00e3\2\2\u094f\u0950\7\u0158\2\2\u0950\u0951\5p9\2\u0951"+
		"\u0952\7\u0159\2\2\u0952\u099e\3\2\2\2\u0953\u0954\7\u00e4\2\2\u0954\u0955"+
		"\7\u0158\2\2\u0955\u0956\5\u00caf\2\u0956\u0957\7\u0159\2\2\u0957\u099e"+
		"\3\2\2\2\u0958\u0959\7\u00f4\2\2\u0959\u095a\7\u0158\2\2\u095a\u095b\5"+
		"\u00d2j\2\u095b\u095d\7\u0159\2\2\u095c\u095e\5\u00d4k\2\u095d\u095c\3"+
		"\2\2\2\u095d\u095e\3\2\2\2\u095e\u099e\3\2\2\2\u095f\u0960\7\u00f7\2\2"+
		"\u0960\u0961\7\u0158\2\2\u0961\u0962\5\u00d2j\2\u0962\u0964\7\u0159\2"+
		"\2\u0963\u0965\5\u00d4k\2\u0964\u0963\3\2\2\2\u0964\u0965\3\2\2\2\u0965"+
		"\u099e\3\2\2\2\u0966\u0967\7\u0128\2\2\u0967\u0968\7\u0158\2\2\u0968\u0969"+
		"\5\u00d2j\2\u0969\u096b\7\u0159\2\2\u096a\u096c\5\u00d4k\2\u096b\u096a"+
		"\3\2\2\2\u096b\u096c\3\2\2\2\u096c\u099e\3\2\2\2\u096d\u096e\7\u0126\2"+
		"\2\u096e\u096f\7\u0158\2\2\u096f\u0970\5\u00d2j\2\u0970\u0972\7\u0159"+
		"\2\2\u0971\u0973\5\u00d4k\2\u0972\u0971\3\2\2\2\u0972\u0973\3\2\2\2\u0973"+
		"\u099e\3\2\2\2\u0974\u0975\7\u0127\2\2\u0975\u0976\7\u0158\2\2\u0976\u0977"+
		"\5\u00d2j\2\u0977\u0979\7\u0159\2\2\u0978\u097a\5\u00d4k\2\u0979\u0978"+
		"\3\2\2\2\u0979\u097a\3\2\2\2\u097a\u099e\3\2\2\2\u097b\u097c\7\u0133\2"+
		"\2\u097c\u097d\7\u0158\2\2\u097d\u097e\5\u00d2j\2\u097e\u0980\7\u0159"+
		"\2\2\u097f\u0981\5\u00d4k\2\u0980\u097f\3\2\2\2\u0980\u0981\3\2\2\2\u0981"+
		"\u099e\3\2\2\2\u0982\u0983\7\u0134\2\2\u0983\u0984\7\u0158\2\2\u0984\u0985"+
		"\5\u00d2j\2\u0985\u0987\7\u0159\2\2\u0986\u0988\5\u00d4k\2\u0987\u0986"+
		"\3\2\2\2\u0987\u0988\3\2\2\2\u0988\u099e\3\2\2\2\u0989\u098a\7\u00cc\2"+
		"\2\u098a\u098d\7\u0158\2\2\u098b\u098e\7\u015d\2\2\u098c\u098e\5\u00d2"+
		"j\2\u098d\u098b\3\2\2\2\u098d\u098c\3\2\2\2\u098e\u098f\3\2\2\2\u098f"+
		"\u0991\7\u0159\2\2\u0990\u0992\5\u00d4k\2\u0991\u0990\3\2\2\2\u0991\u0992"+
		"\3\2\2\2\u0992\u099e\3\2\2\2\u0993\u0994\7\u00cd\2\2\u0994\u0997\7\u0158"+
		"\2\2\u0995\u0998\7\u015d\2\2\u0996\u0998\5\u00d2j\2\u0997\u0995\3\2\2"+
		"\2\u0997\u0996\3\2\2\2\u0998\u0999\3\2\2\2\u0999\u099b\7\u0159\2\2\u099a"+
		"\u099c\5\u00d4k\2\u099b\u099a\3\2\2\2\u099b\u099c\3\2\2\2\u099c\u099e"+
		"\3\2\2\2\u099d\u0942\3\2\2\2\u099d\u0949\3\2\2\2\u099d\u094e\3\2\2\2\u099d"+
		"\u0953\3\2\2\2\u099d\u0958\3\2\2\2\u099d\u095f\3\2\2\2\u099d\u0966\3\2"+
		"\2\2\u099d\u096d\3\2\2\2\u099d\u0974\3\2\2\2\u099d\u097b\3\2\2\2\u099d"+
		"\u0982\3\2\2\2\u099d\u0989\3\2\2\2\u099d\u0993\3\2\2\2\u099e\u00d1\3\2"+
		"\2\2\u099f\u09a1\t\31\2\2\u09a0\u099f\3\2\2\2\u09a0\u09a1\3\2\2\2\u09a1"+
		"\u09a2\3\2\2\2\u09a2\u09a3\5p9\2\u09a3\u00d3\3\2\2\2\u09a4\u09a5\7y\2"+
		"\2\u09a5\u09a7\7\u0158\2\2\u09a6\u09a8\5\u00a0Q\2\u09a7\u09a6\3\2\2\2"+
		"\u09a7\u09a8\3\2\2\2\u09a8\u09aa\3\2\2\2\u09a9\u09ab\5\u008cG\2\u09aa"+
		"\u09a9\3\2\2\2\u09aa\u09ab\3\2\2\2\u09ab\u09ad\3\2\2\2\u09ac\u09ae\5\u00d6"+
		"l\2\u09ad\u09ac\3\2\2\2\u09ad\u09ae\3\2\2\2\u09ae\u09af\3\2\2\2\u09af"+
		"\u09b0\7\u0159\2\2\u09b0\u00d5\3\2\2\2\u09b1\u09b2\t\'\2\2\u09b2\u09b3"+
		"\5\u00d8m\2\u09b3\u00d7\3\2\2\2\u09b4\u09bb\5\u00dco\2\u09b5\u09b6\7\r"+
		"\2\2\u09b6\u09b7\5\u00dan\2\u09b7\u09b8\7\6\2\2\u09b8\u09b9\5\u00dan\2"+
		"\u09b9\u09bb\3\2\2\2\u09ba\u09b4\3\2\2\2\u09ba\u09b5\3\2\2\2\u09bb\u00d9"+
		"\3\2\2\2\u09bc\u09bf\5\u00dco\2\u09bd\u09bf\5\u00dep\2\u09be\u09bc\3\2"+
		"\2\2\u09be\u09bd\3\2\2\2\u09bf\u00db\3\2\2\2\u09c0\u09c1\7\u012f\2\2\u09c1"+
		"\u09c7\7\u010b\2\2\u09c2\u09c3\7\u0141\2\2\u09c3\u09c7\7\u010b\2\2\u09c4"+
		"\u09c5\7&\2\2\u09c5\u09c7\7\u0117\2\2\u09c6\u09c0\3\2\2\2\u09c6\u09c2"+
		"\3\2\2\2\u09c6\u09c4\3\2\2\2\u09c7\u00dd\3\2\2\2\u09c8\u09c9\7\u012f\2"+
		"\2\u09c9\u09cd\7\u00de\2\2\u09ca\u09cb\7\u0141\2\2\u09cb\u09cd\7\u00de"+
		"\2\2\u09cc\u09c8\3\2\2\2\u09cc\u09ca\3\2\2\2\u09cd\u00df\3\2\2\2\u09ce"+
		"\u09cf\5\u0104\u0083\2\u09cf\u09d0\7\u0153\2\2\u09d0\u09d1\5\u0104\u0083"+
		"\2\u09d1\u09d2\7\u0153\2\2\u09d2\u09d3\5\u0104\u0083\2\u09d3\u09d4\7\u0153"+
		"\2\2\u09d4\u09e0\3\2\2\2\u09d5\u09d6\5\u0104\u0083\2\u09d6\u09d8\7\u0153"+
		"\2\2\u09d7\u09d9\5\u0104\u0083\2\u09d8\u09d7\3\2\2\2\u09d8\u09d9\3\2\2"+
		"\2\u09d9\u09da\3\2\2\2\u09da\u09db\7\u0153\2\2\u09db\u09e0\3\2\2\2\u09dc"+
		"\u09dd\5\u0104\u0083\2\u09dd\u09de\7\u0153\2\2\u09de\u09e0\3\2\2\2\u09df"+
		"\u09ce\3\2\2\2\u09df\u09d5\3\2\2\2\u09df\u09dc\3\2\2\2\u09df\u09e0\3\2"+
		"\2\2\u09e0\u09e1\3\2\2\2\u09e1\u09e2\5\u0104\u0083\2\u09e2\u00e1\3\2\2"+
		"\2\u09e3\u09e4\5\u0104\u0083\2\u09e4\u09e6\7\u0153\2\2\u09e5\u09e7\5\u0104"+
		"\u0083\2\u09e6\u09e5\3\2\2\2\u09e6\u09e7\3\2\2\2\u09e7\u09e8\3\2\2\2\u09e8"+
		"\u09e9\7\u0153\2\2\u09e9\u09ee\3\2\2\2\u09ea\u09eb\5\u0104\u0083\2\u09eb"+
		"\u09ec\7\u0153\2\2\u09ec\u09ee\3\2\2\2\u09ed\u09e3\3\2\2\2\u09ed\u09ea"+
		"\3\2\2\2\u09ed\u09ee\3\2\2\2\u09ee\u09ef\3\2\2\2\u09ef\u09f0\5\u0104\u0083"+
		"\2\u09f0\u00e3\3\2\2\2\u09f1\u09f2\5\u0104\u0083\2\u09f2\u09f3\7\u0153"+
		"\2\2\u09f3\u09f5\3\2\2\2\u09f4\u09f1\3\2\2\2\u09f4\u09f5\3\2\2\2\u09f5"+
		"\u09f6\3\2\2\2\u09f6\u09f7\5\u0104\u0083\2\u09f7\u00e5\3\2\2\2\u09f8\u09f9"+
		"\5\u0104\u0083\2\u09f9\u09fb\7\u0153\2\2\u09fa\u09fc\5\u0104\u0083\2\u09fb"+
		"\u09fa\3\2\2\2\u09fb\u09fc\3\2\2\2\u09fc\u09fd\3\2\2\2\u09fd\u09fe\7\u0153"+
		"\2\2\u09fe\u0a03\3\2\2\2\u09ff\u0a00\5\u0104\u0083\2\u0a00\u0a01\7\u0153"+
		"\2\2\u0a01\u0a03\3\2\2\2\u0a02\u09f8\3\2\2\2\u0a02\u09ff\3\2\2\2\u0a02"+
		"\u0a03\3\2\2\2\u0a03\u0a04\3\2\2\2\u0a04\u0a05\5\u0104\u0083\2\u0a05\u00e7"+
		"\3\2\2\2\u0a06\u0a09\5\u00e0q\2\u0a07\u0a09\7\u0140\2\2\u0a08\u0a06\3"+
		"\2\2\2\u0a08\u0a07\3\2\2\2\u0a09\u00e9\3\2\2\2\u0a0a\u0a0b\5\u00e2r\2"+
		"\u0a0b\u0a0c\7\u0153\2\2\u0a0c\u0a0e\3\2\2\2\u0a0d\u0a0a\3\2\2\2\u0a0d"+
		"\u0a0e\3\2\2\2\u0a0e\u0a0f\3\2\2\2\u0a0f\u0a10\5\u00eex\2\u0a10\u00eb"+
		"\3\2\2\2\u0a11\u0a16\5\u00eex\2\u0a12\u0a13\7\u015a\2\2\u0a13\u0a15\5"+
		"\u00eex\2\u0a14\u0a12\3\2\2\2\u0a15\u0a18\3\2\2\2\u0a16\u0a14\3\2\2\2"+
		"\u0a16\u0a17\3\2\2\2\u0a17\u00ed\3\2\2\2\u0a18\u0a16\3\2\2\2\u0a19\u0a1a"+
		"\5\u0104\u0083\2\u0a1a\u00ef\3\2\2\2\u0a1b\u0a1e\5\u0104\u0083\2\u0a1c"+
		"\u0a1e\7\u0140\2\2\u0a1d\u0a1b\3\2\2\2\u0a1d\u0a1c\3\2\2\2\u0a1e\u00f1"+
		"\3\2\2\2\u0a1f\u0a20\t\17\2\2\u0a20\u00f3\3\2\2\2\u0a21\u0a22\t(\2\2\u0a22"+
		"\u00f5\3\2\2\2\u0a23\u0a25\7i\2\2\u0a24\u0a23\3\2\2\2\u0a24\u0a25\3\2"+
		"\2\2\u0a25\u0a26\3\2\2\2\u0a26\u0a27\7j\2\2\u0a27\u00f7\3\2\2\2\u0a28"+
		"\u0a2e\5\u00e6t\2\u0a29\u0a2e\7\u008e\2\2\u0a2a\u0a2e\7a\2\2\u0a2b\u0a2e"+
		"\7\u00c3\2\2\u0a2c\u0a2e\7\u00c7\2\2\u0a2d\u0a28\3\2\2\2\u0a2d\u0a29\3"+
		"\2\2\2\u0a2d\u0a2a\3\2\2\2\u0a2d\u0a2b\3\2\2\2\u0a2d\u0a2c\3\2\2\2\u0a2e"+
		"\u00f9\3\2\2\2\u0a2f\u0a31\5\u0104\u0083\2\u0a30\u0a32\7S\2\2\u0a31\u0a30"+
		"\3\2\2\2\u0a31\u0a32\3\2\2\2\u0a32\u0a3a\3\2\2\2\u0a33\u0a34\7\u0158\2"+
		"\2\u0a34\u0a37\t)\2\2\u0a35\u0a36\7\u015a\2\2\u0a36\u0a38\7\u0141\2\2"+
		"\u0a37\u0a35\3\2\2\2\u0a37\u0a38\3\2\2\2\u0a38\u0a39\3\2\2\2\u0a39\u0a3b"+
		"\7\u0159\2\2\u0a3a\u0a33\3\2\2\2\u0a3a\u0a3b\3\2\2\2\u0a3b\u00fb\3\2\2"+
		"\2\u0a3c\u0a3f\7j\2\2\u0a3d\u0a3f\5\u00fe\u0080\2\u0a3e\u0a3c\3\2\2\2"+
		"\u0a3e\u0a3d\3\2\2\2\u0a3f\u00fd\3\2\2\2\u0a40\u0a4d\7\u0143\2\2\u0a41"+
		"\u0a4d\7\u0144\2\2\u0a42\u0a4d\5\u0100\u0081\2\u0a43\u0a45\5\u0102\u0082"+
		"\2\u0a44\u0a43\3\2\2\2\u0a44\u0a45\3\2\2\2\u0a45\u0a46\3\2\2\2\u0a46\u0a4d"+
		"\t*\2\2\u0a47\u0a49\5\u0102\u0082\2\u0a48\u0a47\3\2\2\2\u0a48\u0a49\3"+
		"\2\2\2\u0a49\u0a4a\3\2\2\2\u0a4a\u0a4b\7\u0157\2\2\u0a4b\u0a4d\t+\2\2"+
		"\u0a4c\u0a40\3\2\2\2\u0a4c\u0a41\3\2\2\2\u0a4c\u0a42\3\2\2\2\u0a4c\u0a44"+
		"\3\2\2\2\u0a4c\u0a48\3\2\2\2\u0a4d\u00ff\3\2\2\2\u0a4e\u0a50\5\u0102\u0082"+
		"\2\u0a4f\u0a4e\3\2\2\2\u0a4f\u0a50\3\2\2\2\u0a50\u0a51\3\2\2\2\u0a51\u0a52"+
		"\7\u0141\2\2\u0a52\u0101\3\2\2\2\u0a53\u0a54\t\25\2\2\u0a54\u0103\3\2"+
		"\2\2\u0a55\u0a59\5\u0106\u0084\2\u0a56\u0a59\7\u013e\2\2\u0a57\u0a59\7"+
		"\u013f\2\2\u0a58\u0a55\3\2\2\2\u0a58\u0a56\3\2\2\2\u0a58\u0a57\3\2\2\2"+
		"\u0a59\u0105\3\2\2\2\u0a5a\u0a5b\t,\2\2\u0a5b\u0107\3\2\2\2\u0a5c\u0a6c"+
		"\7\u0147\2\2\u0a5d\u0a6c\7\u0148\2\2\u0a5e\u0a6c\7\u0149\2\2\u0a5f\u0a60"+
		"\7\u0149\2\2\u0a60\u0a6c\7\u0147\2\2\u0a61\u0a62\7\u0148\2\2\u0a62\u0a6c"+
		"\7\u0147\2\2\u0a63\u0a64\7\u0149\2\2\u0a64\u0a6c\7\u0148\2\2\u0a65\u0a66"+
		"\7\u014a\2\2\u0a66\u0a6c\7\u0147\2\2\u0a67\u0a68\7\u014a\2\2\u0a68\u0a6c"+
		"\7\u0148\2\2\u0a69\u0a6a\7\u014a\2\2\u0a6a\u0a6c\7\u0149\2\2\u0a6b\u0a5c"+
		"\3\2\2\2\u0a6b\u0a5d\3\2\2\2\u0a6b\u0a5e\3\2\2\2\u0a6b\u0a5f\3\2\2\2\u0a6b"+
		"\u0a61\3\2\2\2\u0a6b\u0a63\3\2\2\2\u0a6b\u0a65\3\2\2\2\u0a6b\u0a67\3\2"+
		"\2\2\u0a6b\u0a69\3\2\2\2\u0a6c\u0109\3\2\2\2\u0a6d\u0a6e\t-\2\2\u0a6e"+
		"\u010b\3\2\2\2\u019a\u010f\u0118\u011e\u012c\u0130\u0135\u013a\u013e\u0142"+
		"\u0147\u014c\u0153\u0156\u015a\u015d\u0165\u0168\u016d\u0172\u0178\u017d"+
		"\u0182\u0188\u018e\u0195\u0199\u019b\u01a0\u01a8\u01ad\u01b2\u01b6\u01bb"+
		"\u01bd\u01c7\u01ca\u01d2\u01d4\u01d7\u01dc\u01df\u01e2\u01ea\u01ed\u01f4"+
		"\u01f8\u01fa\u01fc\u01ff\u0202\u0205\u0208\u0210\u0212\u0215\u0219\u021c"+
		"\u0222\u0225\u0232\u0239\u023c\u023f\u0242\u0245\u0249\u024c\u024f\u0252"+
		"\u0255\u025d\u025f\u0263\u0266\u026e\u0272\u027a\u027d\u0284\u0288\u028a"+
		"\u028c\u028f\u0292\u0295\u029d\u02a3\u02a9\u02ab\u02af\u02b2\u02b5\u02ba"+
		"\u02bf\u02c2\u02c6\u02c9\u02d3\u02da\u02dd\u02e4\u02e8\u02ea\u02f2\u02f5"+
		"\u02f9\u02ff\u0305\u0308\u030c\u0310\u0313\u0318\u0328\u032c\u0330\u0332"+
		"\u0335\u033d\u0342\u0346\u034c\u034f\u035a\u035f\u0367\u036a\u0371\u0374"+
		"\u0382\u038a\u038c\u0392\u039c\u039f\u03a4\u03aa\u03ae\u03b4\u03b8\u03bf"+
		"\u03c8\u03cc\u03d2\u03d9\u03dd\u03e1\u03f2\u03f6\u0401\u0405\u040a\u040e"+
		"\u0410\u0414\u0418\u041c\u0420\u0426\u042a\u042c\u0431\u0439\u043c\u043f"+
		"\u0448\u044d\u0452\u0455\u0457\u045b\u0460\u0464\u046a\u0473\u0476\u0478"+
		"\u047e\u0483\u048a\u0497\u0499\u049b\u049e\u04a1\u04a8\u04ab\u04b1\u04b6"+
		"\u04b8\u04bb\u04c1\u04c7\u04c9\u04cc\u04d0\u04d3\u04d9\u04dc\u04e0\u04e3"+
		"\u04e9\u04ec\u04ee\u04f2\u04f7\u04ff\u0504\u050a\u050f\u0516\u051c\u0520"+
		"\u0523\u0527\u052d\u0535\u053a\u053c\u053f\u0544\u0549\u054c\u0551\u0554"+
		"\u0557\u055d\u0563\u0567\u056c\u056f\u0575\u0579\u057f\u0585\u058e\u0598"+
		"\u059e\u05a3\u05a6\u05b1\u05b3\u05b6\u05c0\u05c2\u05c5\u05c7\u05ca\u05cd"+
		"\u05d0\u05d3\u05d6\u05e4\u05e7\u05ea\u05f3\u05f6\u05f9\u0601\u0604\u0612"+
		"\u0615\u061c\u061e\u0634\u0643\u0645\u0650\u0657\u065e\u0666\u066f\u0673"+
		"\u067d\u0684\u068c\u0694\u0698\u06ae\u06b7\u06bd\u06c3\u06c9\u06d3\u06da"+
		"\u06df\u06e4\u06e8\u06f1\u06f3\u06f7\u06fc\u0700\u0702\u0707\u070f\u0712"+
		"\u0716\u071f\u0722\u0726\u072f\u073b\u073d\u0745\u074d\u0750\u0752\u0759"+
		"\u075d\u0768\u078a\u079a\u07a0\u07a7\u07ad\u07b2\u07ba\u07bd\u07bf\u07ca"+
		"\u07d0\u07d5\u07d9\u07de\u07e0\u07e7\u07eb\u07f2\u07f4\u07f6\u0801\u0805"+
		"\u0807\u080a\u081a\u081e\u082a\u082e\u083b\u0842\u0858\u088c\u0890\u089e"+
		"\u08a3\u08a9\u08ac\u08b4\u08c0\u08c6\u08cf\u08e0\u08e6\u08ee\u08fa\u0901"+
		"\u0908\u0914\u0918\u0924\u0928\u092c\u0940\u0947\u095d\u0964\u096b\u0972"+
		"\u0979\u0980\u0987\u098d\u0991\u0997\u099b\u099d\u09a0\u09a7\u09aa\u09ad"+
		"\u09ba\u09be\u09c6\u09cc\u09d8\u09df\u09e6\u09ed\u09f4\u09fb\u0a02\u0a08"+
		"\u0a0d\u0a16\u0a1d\u0a24\u0a2d\u0a31\u0a37\u0a3a\u0a3e\u0a44\u0a48\u0a4c"+
		"\u0a4f\u0a58\u0a6b";
	public static final String _serializedATN = Utils.join(
		new String[] {
			_serializedATNSegment0,
			_serializedATNSegment1
		},
		""
	);
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}