// Generated from ANTLRv4Parser.g4 by ANTLR 4.6
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
	T visitGrammarSpec(ANTLRv4Parser.GrammarSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#grammarType}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGrammarType(ANTLRv4Parser.GrammarTypeContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#prequelConstruct}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrequelConstruct(ANTLRv4Parser.PrequelConstructContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#optionsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOptionsSpec(ANTLRv4Parser.OptionsSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOption(ANTLRv4Parser.OptionContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#optionValue}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOptionValue(ANTLRv4Parser.OptionValueContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#delegateGrammars}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDelegateGrammars(ANTLRv4Parser.DelegateGrammarsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#delegateGrammar}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDelegateGrammar(ANTLRv4Parser.DelegateGrammarContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#tokensSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTokensSpec(ANTLRv4Parser.TokensSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#channelsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitChannelsSpec(ANTLRv4Parser.ChannelsSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#idList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIdList(ANTLRv4Parser.IdListContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#action}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAction(ANTLRv4Parser.ActionContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#actionScopeName}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitActionScopeName(ANTLRv4Parser.ActionScopeNameContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#modeSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitModeSpec(ANTLRv4Parser.ModeSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#rules}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRules(ANTLRv4Parser.RulesContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleSpec(ANTLRv4Parser.RuleSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#parserRuleSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParserRuleSpec(ANTLRv4Parser.ParserRuleSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#exceptionGroup}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExceptionGroup(ANTLRv4Parser.ExceptionGroupContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#exceptionHandler}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExceptionHandler(ANTLRv4Parser.ExceptionHandlerContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#finallyClause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFinallyClause(ANTLRv4Parser.FinallyClauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#rulePrequel}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRulePrequel(ANTLRv4Parser.RulePrequelContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleReturns}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleReturns(ANTLRv4Parser.RuleReturnsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#throwsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitThrowsSpec(ANTLRv4Parser.ThrowsSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#localsSpec}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLocalsSpec(ANTLRv4Parser.LocalsSpecContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleAction}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleAction(ANTLRv4Parser.RuleActionContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleModifiers}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleModifiers(ANTLRv4Parser.RuleModifiersContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleModifier}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleModifier(ANTLRv4Parser.RuleModifierContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleBlock(ANTLRv4Parser.RuleBlockContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleAltList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleAltList(ANTLRv4Parser.RuleAltListContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledAlt}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledAlt(ANTLRv4Parser.LabeledAltContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerRule}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerRule(ANTLRv4Parser.LexerRuleContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerRuleBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerRuleBlock(ANTLRv4Parser.LexerRuleBlockContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAltList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAltList(ANTLRv4Parser.LexerAltListContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAlt}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAlt(ANTLRv4Parser.LexerAltContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerElements}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerElements(ANTLRv4Parser.LexerElementsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerElement(ANTLRv4Parser.LexerElementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledLexerElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledLexerElement(ANTLRv4Parser.LabeledLexerElementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerBlock}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerBlock(ANTLRv4Parser.LexerBlockContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommands}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommands(ANTLRv4Parser.LexerCommandsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommand}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommand(ANTLRv4Parser.LexerCommandContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommandName}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommandName(ANTLRv4Parser.LexerCommandNameContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerCommandExpr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerCommandExpr(ANTLRv4Parser.LexerCommandExprContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#altList}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAltList(ANTLRv4Parser.AltListContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#alternative}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlternative(ANTLRv4Parser.AlternativeContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#element}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElement(ANTLRv4Parser.ElementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#labeledElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLabeledElement(ANTLRv4Parser.LabeledElementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ebnf}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEbnf(ANTLRv4Parser.EbnfContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#blockSuffix}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlockSuffix(ANTLRv4Parser.BlockSuffixContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ebnfSuffix}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEbnfSuffix(ANTLRv4Parser.EbnfSuffixContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#lexerAtom}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLexerAtom(ANTLRv4Parser.LexerAtomContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#atom}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAtom(ANTLRv4Parser.AtomContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#notSet}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNotSet(ANTLRv4Parser.NotSetContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#blockSet}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlockSet(ANTLRv4Parser.BlockSetContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#setElement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSetElement(ANTLRv4Parser.SetElementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#block}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlock(ANTLRv4Parser.BlockContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#ruleref}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRuleref(ANTLRv4Parser.RulerefContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#range}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRange(ANTLRv4Parser.RangeContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#terminal}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTerminal(ANTLRv4Parser.TerminalContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#elementOptions}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElementOptions(ANTLRv4Parser.ElementOptionsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#elementOption}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElementOption(ANTLRv4Parser.ElementOptionContext ctx);
	/**
	 * Visit a parse tree produced by {@link ANTLRv4Parser#id}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitId(ANTLRv4Parser.IdContext ctx);
}