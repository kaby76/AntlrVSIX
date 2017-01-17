// Generated from C:\Users\Ken\Documents\Visual Studio 2015\Projects\VSIXProject1\VSIXProject1\Grammar\ANTLRv4Parser.g4 by ANTLR 4.1
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link ANTLRv4Parser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface ANTLRv4ParserVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#grammarSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGrammarSpec(@NotNull ANTLRv4Parser.GrammarSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#grammarType}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGrammarType(@NotNull ANTLRv4Parser.GrammarTypeContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#localsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLocalsSpec(@NotNull ANTLRv4Parser.LocalsSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#notSet}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNotSet(@NotNull ANTLRv4Parser.NotSetContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#rules}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRules(@NotNull ANTLRv4Parser.RulesContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#idList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIdList(@NotNull ANTLRv4Parser.IdListContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#exceptionGroup}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExceptionGroup(@NotNull ANTLRv4Parser.ExceptionGroupContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#rulePrequel}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRulePrequel(@NotNull ANTLRv4Parser.RulePrequelContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#throwsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitThrowsSpec(@NotNull ANTLRv4Parser.ThrowsSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledElement(@NotNull ANTLRv4Parser.LabeledElementContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#action}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAction(@NotNull ANTLRv4Parser.ActionContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#block}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlock(@NotNull ANTLRv4Parser.BlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#prequelConstruct}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrequelConstruct(@NotNull ANTLRv4Parser.PrequelConstructContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#setElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSetElement(@NotNull ANTLRv4Parser.SetElementContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#id}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitId(@NotNull ANTLRv4Parser.IdContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#finallyClause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFinallyClause(@NotNull ANTLRv4Parser.FinallyClauseContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommandExpr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommandExpr(@NotNull ANTLRv4Parser.LexerCommandExprContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAltList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAltList(@NotNull ANTLRv4Parser.LexerAltListContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAtom}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAtom(@NotNull ANTLRv4Parser.LexerAtomContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#channelsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitChannelsSpec(@NotNull ANTLRv4Parser.ChannelsSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#element}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElement(@NotNull ANTLRv4Parser.ElementContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAlt}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAlt(@NotNull ANTLRv4Parser.LexerAltContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommand}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommand(@NotNull ANTLRv4Parser.LexerCommandContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerElement(@NotNull ANTLRv4Parser.LexerElementContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ebnfSuffix}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEbnfSuffix(@NotNull ANTLRv4Parser.EbnfSuffixContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#actionScopeName}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitActionScopeName(@NotNull ANTLRv4Parser.ActionScopeNameContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleAltList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleAltList(@NotNull ANTLRv4Parser.RuleAltListContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleAction}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleAction(@NotNull ANTLRv4Parser.RuleActionContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleModifier}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleModifier(@NotNull ANTLRv4Parser.RuleModifierContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#delegateGrammars}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDelegateGrammars(@NotNull ANTLRv4Parser.DelegateGrammarsContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ebnf}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEbnf(@NotNull ANTLRv4Parser.EbnfContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#exceptionHandler}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExceptionHandler(@NotNull ANTLRv4Parser.ExceptionHandlerContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#actionBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitActionBlock(@NotNull ANTLRv4Parser.ActionBlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOption(@NotNull ANTLRv4Parser.OptionContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleModifiers}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleModifiers(@NotNull ANTLRv4Parser.RuleModifiersContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommands}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommands(@NotNull ANTLRv4Parser.LexerCommandsContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#parserRuleSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParserRuleSpec(@NotNull ANTLRv4Parser.ParserRuleSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledLexerElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledLexerElement(@NotNull ANTLRv4Parser.LabeledLexerElementContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleBlock(@NotNull ANTLRv4Parser.RuleBlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#range}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRange(@NotNull ANTLRv4Parser.RangeContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#argActionBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArgActionBlock(@NotNull ANTLRv4Parser.ArgActionBlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#delegateGrammar}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDelegateGrammar(@NotNull ANTLRv4Parser.DelegateGrammarContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#optionsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOptionsSpec(@NotNull ANTLRv4Parser.OptionsSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleSpec(@NotNull ANTLRv4Parser.RuleSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleref}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleref(@NotNull ANTLRv4Parser.RulerefContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerBlock(@NotNull ANTLRv4Parser.LexerBlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerRuleSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerRuleSpec(@NotNull ANTLRv4Parser.LexerRuleSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerRuleBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerRuleBlock(@NotNull ANTLRv4Parser.LexerRuleBlockContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#tokensSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTokensSpec(@NotNull ANTLRv4Parser.TokensSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#elementOptions}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElementOptions(@NotNull ANTLRv4Parser.ElementOptionsContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommandName}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommandName(@NotNull ANTLRv4Parser.LexerCommandNameContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#optionValue}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOptionValue(@NotNull ANTLRv4Parser.OptionValueContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleReturns}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleReturns(@NotNull ANTLRv4Parser.RuleReturnsContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#alternative}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlternative(@NotNull ANTLRv4Parser.AlternativeContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#altList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAltList(@NotNull ANTLRv4Parser.AltListContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#terminal}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTerminal(@NotNull ANTLRv4Parser.TerminalContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledAlt}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledAlt(@NotNull ANTLRv4Parser.LabeledAltContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#modeSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitModeSpec(@NotNull ANTLRv4Parser.ModeSpecContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#blockSet}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlockSet(@NotNull ANTLRv4Parser.BlockSetContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#elementOption}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElementOption(@NotNull ANTLRv4Parser.ElementOptionContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#blockSuffix}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlockSuffix(@NotNull ANTLRv4Parser.BlockSuffixContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#atom}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAtom(@NotNull ANTLRv4Parser.AtomContext ctx);

	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerElements}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerElements(@NotNull ANTLRv4Parser.LexerElementsContext ctx);
}