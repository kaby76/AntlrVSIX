// Generated from C:\Users\Ken\Documents\Visual Studio 2015\Projects\VSIXProject1\VSIXProject1\Grammar\ANTLRv4Parser.g4 by ANTLR 4.1
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link ANTLRv4Parser}.
 */
public interface ANTLRv4ParserListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#grammarSpec}.
	 * @param ctx the parse tree
	 */
	void enterGrammarSpec(@NotNull ANTLRv4Parser.GrammarSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#grammarSpec}.
	 * @param ctx the parse tree
	 */
	void exitGrammarSpec(@NotNull ANTLRv4Parser.GrammarSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#grammarType}.
	 * @param ctx the parse tree
	 */
	void enterGrammarType(@NotNull ANTLRv4Parser.GrammarTypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#grammarType}.
	 * @param ctx the parse tree
	 */
	void exitGrammarType(@NotNull ANTLRv4Parser.GrammarTypeContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#localsSpec}.
	 * @param ctx the parse tree
	 */
	void enterLocalsSpec(@NotNull ANTLRv4Parser.LocalsSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#localsSpec}.
	 * @param ctx the parse tree
	 */
	void exitLocalsSpec(@NotNull ANTLRv4Parser.LocalsSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#notSet}.
	 * @param ctx the parse tree
	 */
	void enterNotSet(@NotNull ANTLRv4Parser.NotSetContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#notSet}.
	 * @param ctx the parse tree
	 */
	void exitNotSet(@NotNull ANTLRv4Parser.NotSetContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#rules}.
	 * @param ctx the parse tree
	 */
	void enterRules(@NotNull ANTLRv4Parser.RulesContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#rules}.
	 * @param ctx the parse tree
	 */
	void exitRules(@NotNull ANTLRv4Parser.RulesContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#idList}.
	 * @param ctx the parse tree
	 */
	void enterIdList(@NotNull ANTLRv4Parser.IdListContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#idList}.
	 * @param ctx the parse tree
	 */
	void exitIdList(@NotNull ANTLRv4Parser.IdListContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#exceptionGroup}.
	 * @param ctx the parse tree
	 */
	void enterExceptionGroup(@NotNull ANTLRv4Parser.ExceptionGroupContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#exceptionGroup}.
	 * @param ctx the parse tree
	 */
	void exitExceptionGroup(@NotNull ANTLRv4Parser.ExceptionGroupContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#rulePrequel}.
	 * @param ctx the parse tree
	 */
	void enterRulePrequel(@NotNull ANTLRv4Parser.RulePrequelContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#rulePrequel}.
	 * @param ctx the parse tree
	 */
	void exitRulePrequel(@NotNull ANTLRv4Parser.RulePrequelContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#throwsSpec}.
	 * @param ctx the parse tree
	 */
	void enterThrowsSpec(@NotNull ANTLRv4Parser.ThrowsSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#throwsSpec}.
	 * @param ctx the parse tree
	 */
	void exitThrowsSpec(@NotNull ANTLRv4Parser.ThrowsSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#labeledElement}.
	 * @param ctx the parse tree
	 */
	void enterLabeledElement(@NotNull ANTLRv4Parser.LabeledElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#labeledElement}.
	 * @param ctx the parse tree
	 */
	void exitLabeledElement(@NotNull ANTLRv4Parser.LabeledElementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#action}.
	 * @param ctx the parse tree
	 */
	void enterAction(@NotNull ANTLRv4Parser.ActionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#action}.
	 * @param ctx the parse tree
	 */
	void exitAction(@NotNull ANTLRv4Parser.ActionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#block}.
	 * @param ctx the parse tree
	 */
	void enterBlock(@NotNull ANTLRv4Parser.BlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#block}.
	 * @param ctx the parse tree
	 */
	void exitBlock(@NotNull ANTLRv4Parser.BlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#prequelConstruct}.
	 * @param ctx the parse tree
	 */
	void enterPrequelConstruct(@NotNull ANTLRv4Parser.PrequelConstructContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#prequelConstruct}.
	 * @param ctx the parse tree
	 */
	void exitPrequelConstruct(@NotNull ANTLRv4Parser.PrequelConstructContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#setElement}.
	 * @param ctx the parse tree
	 */
	void enterSetElement(@NotNull ANTLRv4Parser.SetElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#setElement}.
	 * @param ctx the parse tree
	 */
	void exitSetElement(@NotNull ANTLRv4Parser.SetElementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#id}.
	 * @param ctx the parse tree
	 */
	void enterId(@NotNull ANTLRv4Parser.IdContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#id}.
	 * @param ctx the parse tree
	 */
	void exitId(@NotNull ANTLRv4Parser.IdContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#finallyClause}.
	 * @param ctx the parse tree
	 */
	void enterFinallyClause(@NotNull ANTLRv4Parser.FinallyClauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#finallyClause}.
	 * @param ctx the parse tree
	 */
	void exitFinallyClause(@NotNull ANTLRv4Parser.FinallyClauseContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerCommandExpr}.
	 * @param ctx the parse tree
	 */
	void enterLexerCommandExpr(@NotNull ANTLRv4Parser.LexerCommandExprContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerCommandExpr}.
	 * @param ctx the parse tree
	 */
	void exitLexerCommandExpr(@NotNull ANTLRv4Parser.LexerCommandExprContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerAltList}.
	 * @param ctx the parse tree
	 */
	void enterLexerAltList(@NotNull ANTLRv4Parser.LexerAltListContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerAltList}.
	 * @param ctx the parse tree
	 */
	void exitLexerAltList(@NotNull ANTLRv4Parser.LexerAltListContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerAtom}.
	 * @param ctx the parse tree
	 */
	void enterLexerAtom(@NotNull ANTLRv4Parser.LexerAtomContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerAtom}.
	 * @param ctx the parse tree
	 */
	void exitLexerAtom(@NotNull ANTLRv4Parser.LexerAtomContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#channelsSpec}.
	 * @param ctx the parse tree
	 */
	void enterChannelsSpec(@NotNull ANTLRv4Parser.ChannelsSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#channelsSpec}.
	 * @param ctx the parse tree
	 */
	void exitChannelsSpec(@NotNull ANTLRv4Parser.ChannelsSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#element}.
	 * @param ctx the parse tree
	 */
	void enterElement(@NotNull ANTLRv4Parser.ElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#element}.
	 * @param ctx the parse tree
	 */
	void exitElement(@NotNull ANTLRv4Parser.ElementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerAlt}.
	 * @param ctx the parse tree
	 */
	void enterLexerAlt(@NotNull ANTLRv4Parser.LexerAltContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerAlt}.
	 * @param ctx the parse tree
	 */
	void exitLexerAlt(@NotNull ANTLRv4Parser.LexerAltContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerCommand}.
	 * @param ctx the parse tree
	 */
	void enterLexerCommand(@NotNull ANTLRv4Parser.LexerCommandContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerCommand}.
	 * @param ctx the parse tree
	 */
	void exitLexerCommand(@NotNull ANTLRv4Parser.LexerCommandContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerElement}.
	 * @param ctx the parse tree
	 */
	void enterLexerElement(@NotNull ANTLRv4Parser.LexerElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerElement}.
	 * @param ctx the parse tree
	 */
	void exitLexerElement(@NotNull ANTLRv4Parser.LexerElementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ebnfSuffix}.
	 * @param ctx the parse tree
	 */
	void enterEbnfSuffix(@NotNull ANTLRv4Parser.EbnfSuffixContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ebnfSuffix}.
	 * @param ctx the parse tree
	 */
	void exitEbnfSuffix(@NotNull ANTLRv4Parser.EbnfSuffixContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#actionScopeName}.
	 * @param ctx the parse tree
	 */
	void enterActionScopeName(@NotNull ANTLRv4Parser.ActionScopeNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#actionScopeName}.
	 * @param ctx the parse tree
	 */
	void exitActionScopeName(@NotNull ANTLRv4Parser.ActionScopeNameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleAltList}.
	 * @param ctx the parse tree
	 */
	void enterRuleAltList(@NotNull ANTLRv4Parser.RuleAltListContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleAltList}.
	 * @param ctx the parse tree
	 */
	void exitRuleAltList(@NotNull ANTLRv4Parser.RuleAltListContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleAction}.
	 * @param ctx the parse tree
	 */
	void enterRuleAction(@NotNull ANTLRv4Parser.RuleActionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleAction}.
	 * @param ctx the parse tree
	 */
	void exitRuleAction(@NotNull ANTLRv4Parser.RuleActionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleModifier}.
	 * @param ctx the parse tree
	 */
	void enterRuleModifier(@NotNull ANTLRv4Parser.RuleModifierContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleModifier}.
	 * @param ctx the parse tree
	 */
	void exitRuleModifier(@NotNull ANTLRv4Parser.RuleModifierContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#delegateGrammars}.
	 * @param ctx the parse tree
	 */
	void enterDelegateGrammars(@NotNull ANTLRv4Parser.DelegateGrammarsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#delegateGrammars}.
	 * @param ctx the parse tree
	 */
	void exitDelegateGrammars(@NotNull ANTLRv4Parser.DelegateGrammarsContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ebnf}.
	 * @param ctx the parse tree
	 */
	void enterEbnf(@NotNull ANTLRv4Parser.EbnfContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ebnf}.
	 * @param ctx the parse tree
	 */
	void exitEbnf(@NotNull ANTLRv4Parser.EbnfContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#exceptionHandler}.
	 * @param ctx the parse tree
	 */
	void enterExceptionHandler(@NotNull ANTLRv4Parser.ExceptionHandlerContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#exceptionHandler}.
	 * @param ctx the parse tree
	 */
	void exitExceptionHandler(@NotNull ANTLRv4Parser.ExceptionHandlerContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#actionBlock}.
	 * @param ctx the parse tree
	 */
	void enterActionBlock(@NotNull ANTLRv4Parser.ActionBlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#actionBlock}.
	 * @param ctx the parse tree
	 */
	void exitActionBlock(@NotNull ANTLRv4Parser.ActionBlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#option}.
	 * @param ctx the parse tree
	 */
	void enterOption(@NotNull ANTLRv4Parser.OptionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#option}.
	 * @param ctx the parse tree
	 */
	void exitOption(@NotNull ANTLRv4Parser.OptionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleModifiers}.
	 * @param ctx the parse tree
	 */
	void enterRuleModifiers(@NotNull ANTLRv4Parser.RuleModifiersContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleModifiers}.
	 * @param ctx the parse tree
	 */
	void exitRuleModifiers(@NotNull ANTLRv4Parser.RuleModifiersContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerCommands}.
	 * @param ctx the parse tree
	 */
	void enterLexerCommands(@NotNull ANTLRv4Parser.LexerCommandsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerCommands}.
	 * @param ctx the parse tree
	 */
	void exitLexerCommands(@NotNull ANTLRv4Parser.LexerCommandsContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#parserRuleSpec}.
	 * @param ctx the parse tree
	 */
	void enterParserRuleSpec(@NotNull ANTLRv4Parser.ParserRuleSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#parserRuleSpec}.
	 * @param ctx the parse tree
	 */
	void exitParserRuleSpec(@NotNull ANTLRv4Parser.ParserRuleSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#labeledLexerElement}.
	 * @param ctx the parse tree
	 */
	void enterLabeledLexerElement(@NotNull ANTLRv4Parser.LabeledLexerElementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#labeledLexerElement}.
	 * @param ctx the parse tree
	 */
	void exitLabeledLexerElement(@NotNull ANTLRv4Parser.LabeledLexerElementContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleBlock}.
	 * @param ctx the parse tree
	 */
	void enterRuleBlock(@NotNull ANTLRv4Parser.RuleBlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleBlock}.
	 * @param ctx the parse tree
	 */
	void exitRuleBlock(@NotNull ANTLRv4Parser.RuleBlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#range}.
	 * @param ctx the parse tree
	 */
	void enterRange(@NotNull ANTLRv4Parser.RangeContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#range}.
	 * @param ctx the parse tree
	 */
	void exitRange(@NotNull ANTLRv4Parser.RangeContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#argActionBlock}.
	 * @param ctx the parse tree
	 */
	void enterArgActionBlock(@NotNull ANTLRv4Parser.ArgActionBlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#argActionBlock}.
	 * @param ctx the parse tree
	 */
	void exitArgActionBlock(@NotNull ANTLRv4Parser.ArgActionBlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#delegateGrammar}.
	 * @param ctx the parse tree
	 */
	void enterDelegateGrammar(@NotNull ANTLRv4Parser.DelegateGrammarContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#delegateGrammar}.
	 * @param ctx the parse tree
	 */
	void exitDelegateGrammar(@NotNull ANTLRv4Parser.DelegateGrammarContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#optionsSpec}.
	 * @param ctx the parse tree
	 */
	void enterOptionsSpec(@NotNull ANTLRv4Parser.OptionsSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#optionsSpec}.
	 * @param ctx the parse tree
	 */
	void exitOptionsSpec(@NotNull ANTLRv4Parser.OptionsSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleSpec}.
	 * @param ctx the parse tree
	 */
	void enterRuleSpec(@NotNull ANTLRv4Parser.RuleSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleSpec}.
	 * @param ctx the parse tree
	 */
	void exitRuleSpec(@NotNull ANTLRv4Parser.RuleSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleref}.
	 * @param ctx the parse tree
	 */
	void enterRuleref(@NotNull ANTLRv4Parser.RulerefContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleref}.
	 * @param ctx the parse tree
	 */
	void exitRuleref(@NotNull ANTLRv4Parser.RulerefContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerBlock}.
	 * @param ctx the parse tree
	 */
	void enterLexerBlock(@NotNull ANTLRv4Parser.LexerBlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerBlock}.
	 * @param ctx the parse tree
	 */
	void exitLexerBlock(@NotNull ANTLRv4Parser.LexerBlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerRuleSpec}.
	 * @param ctx the parse tree
	 */
	void enterLexerRuleSpec(@NotNull ANTLRv4Parser.LexerRuleSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerRuleSpec}.
	 * @param ctx the parse tree
	 */
	void exitLexerRuleSpec(@NotNull ANTLRv4Parser.LexerRuleSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerRuleBlock}.
	 * @param ctx the parse tree
	 */
	void enterLexerRuleBlock(@NotNull ANTLRv4Parser.LexerRuleBlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerRuleBlock}.
	 * @param ctx the parse tree
	 */
	void exitLexerRuleBlock(@NotNull ANTLRv4Parser.LexerRuleBlockContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#tokensSpec}.
	 * @param ctx the parse tree
	 */
	void enterTokensSpec(@NotNull ANTLRv4Parser.TokensSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#tokensSpec}.
	 * @param ctx the parse tree
	 */
	void exitTokensSpec(@NotNull ANTLRv4Parser.TokensSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#elementOptions}.
	 * @param ctx the parse tree
	 */
	void enterElementOptions(@NotNull ANTLRv4Parser.ElementOptionsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#elementOptions}.
	 * @param ctx the parse tree
	 */
	void exitElementOptions(@NotNull ANTLRv4Parser.ElementOptionsContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerCommandName}.
	 * @param ctx the parse tree
	 */
	void enterLexerCommandName(@NotNull ANTLRv4Parser.LexerCommandNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerCommandName}.
	 * @param ctx the parse tree
	 */
	void exitLexerCommandName(@NotNull ANTLRv4Parser.LexerCommandNameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#optionValue}.
	 * @param ctx the parse tree
	 */
	void enterOptionValue(@NotNull ANTLRv4Parser.OptionValueContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#optionValue}.
	 * @param ctx the parse tree
	 */
	void exitOptionValue(@NotNull ANTLRv4Parser.OptionValueContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#ruleReturns}.
	 * @param ctx the parse tree
	 */
	void enterRuleReturns(@NotNull ANTLRv4Parser.RuleReturnsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#ruleReturns}.
	 * @param ctx the parse tree
	 */
	void exitRuleReturns(@NotNull ANTLRv4Parser.RuleReturnsContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#alternative}.
	 * @param ctx the parse tree
	 */
	void enterAlternative(@NotNull ANTLRv4Parser.AlternativeContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#alternative}.
	 * @param ctx the parse tree
	 */
	void exitAlternative(@NotNull ANTLRv4Parser.AlternativeContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#altList}.
	 * @param ctx the parse tree
	 */
	void enterAltList(@NotNull ANTLRv4Parser.AltListContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#altList}.
	 * @param ctx the parse tree
	 */
	void exitAltList(@NotNull ANTLRv4Parser.AltListContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#terminal}.
	 * @param ctx the parse tree
	 */
	void enterTerminal(@NotNull ANTLRv4Parser.TerminalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#terminal}.
	 * @param ctx the parse tree
	 */
	void exitTerminal(@NotNull ANTLRv4Parser.TerminalContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#labeledAlt}.
	 * @param ctx the parse tree
	 */
	void enterLabeledAlt(@NotNull ANTLRv4Parser.LabeledAltContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#labeledAlt}.
	 * @param ctx the parse tree
	 */
	void exitLabeledAlt(@NotNull ANTLRv4Parser.LabeledAltContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#modeSpec}.
	 * @param ctx the parse tree
	 */
	void enterModeSpec(@NotNull ANTLRv4Parser.ModeSpecContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#modeSpec}.
	 * @param ctx the parse tree
	 */
	void exitModeSpec(@NotNull ANTLRv4Parser.ModeSpecContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#blockSet}.
	 * @param ctx the parse tree
	 */
	void enterBlockSet(@NotNull ANTLRv4Parser.BlockSetContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#blockSet}.
	 * @param ctx the parse tree
	 */
	void exitBlockSet(@NotNull ANTLRv4Parser.BlockSetContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#elementOption}.
	 * @param ctx the parse tree
	 */
	void enterElementOption(@NotNull ANTLRv4Parser.ElementOptionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#elementOption}.
	 * @param ctx the parse tree
	 */
	void exitElementOption(@NotNull ANTLRv4Parser.ElementOptionContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#blockSuffix}.
	 * @param ctx the parse tree
	 */
	void enterBlockSuffix(@NotNull ANTLRv4Parser.BlockSuffixContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#blockSuffix}.
	 * @param ctx the parse tree
	 */
	void exitBlockSuffix(@NotNull ANTLRv4Parser.BlockSuffixContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#atom}.
	 * @param ctx the parse tree
	 */
	void enterAtom(@NotNull ANTLRv4Parser.AtomContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#atom}.
	 * @param ctx the parse tree
	 */
	void exitAtom(@NotNull ANTLRv4Parser.AtomContext ctx);

	/**
	 * Enter a parse tree produced by {@link ANTLRv4Parser#lexerElements}.
	 * @param ctx the parse tree
	 */
	void enterLexerElements(@NotNull ANTLRv4Parser.LexerElementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ANTLRv4Parser#lexerElements}.
	 * @param ctx the parse tree
	 */
	void exitLexerElements(@NotNull ANTLRv4Parser.LexerElementsContext ctx);
}