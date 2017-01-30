// Generated from Quorum.g4 by ANTLR 4.6
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link QuorumParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface QuorumVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link QuorumParser#start}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitStart(QuorumParser.StartContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#package_rule}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPackage_rule(QuorumParser.Package_ruleContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#reference}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitReference(QuorumParser.ReferenceContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FullClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFullClassDeclaration(QuorumParser.FullClassDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NoClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNoClassDeclaration(QuorumParser.NoClassDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NoActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNoActionsNoClass(QuorumParser.NoActionsNoClassContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitActionsNoClass(QuorumParser.ActionsNoClassContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#inherit_stmnts}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInherit_stmnts(QuorumParser.Inherit_stmntsContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#inherit_stmt}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInherit_stmt(QuorumParser.Inherit_stmtContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#access_modifier}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAccess_modifier(QuorumParser.Access_modifierContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#class_stmnts}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitClass_stmnts(QuorumParser.Class_stmntsContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Action}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAction(QuorumParser.ActionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BlueprintAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlueprintAction(QuorumParser.BlueprintActionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NativeAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNativeAction(QuorumParser.NativeActionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Constructor}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitConstructor(QuorumParser.ConstructorContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#method_shared}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMethod_shared(QuorumParser.Method_sharedContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#formal_parameter}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFormal_parameter(QuorumParser.Formal_parameterContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#qualified_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQualified_name(QuorumParser.Qualified_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#block}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBlock(QuorumParser.BlockContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitStatement(QuorumParser.StatementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVariableSoloFunctionCall(QuorumParser.VariableSoloFunctionCallContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ParentVariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParentVariableSoloFunctionCall(QuorumParser.ParentVariableSoloFunctionCallContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#solo_method_required_method_part}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSolo_method_required_method_part(QuorumParser.Solo_method_required_method_partContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#alert_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlert_statement(QuorumParser.Alert_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#check_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCheck_statement(QuorumParser.Check_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#detect_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDetect_statement(QuorumParser.Detect_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#always_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlways_statement(QuorumParser.Always_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#print_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrint_statement(QuorumParser.Print_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#speak_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSpeak_statement(QuorumParser.Speak_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#return_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitReturn_statement(QuorumParser.Return_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#generic_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGeneric_declaration(QuorumParser.Generic_declarationContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#generic_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGeneric_statement(QuorumParser.Generic_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#class_type}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitClass_type(QuorumParser.Class_typeContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GenericAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGenericAssignmentDeclaration(QuorumParser.GenericAssignmentDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code IntegerAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIntegerAssignmentDeclaration(QuorumParser.IntegerAssignmentDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NumberAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNumberAssignmentDeclaration(QuorumParser.NumberAssignmentDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TextAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTextAssignmentDeclaration(QuorumParser.TextAssignmentDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BooleanAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBooleanAssignmentDeclaration(QuorumParser.BooleanAssignmentDeclarationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NoTypeAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNoTypeAssignment(QuorumParser.NoTypeAssignmentContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ParentAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParentAssignment(QuorumParser.ParentAssignmentContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ObjectAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitObjectAssignment(QuorumParser.ObjectAssignmentContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NormalAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNormalAssignment(QuorumParser.NormalAssignmentContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#if_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIf_statement(QuorumParser.If_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#elseif_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElseif_statement(QuorumParser.Elseif_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#else_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitElse_statement(QuorumParser.Else_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#loop_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLoop_statement(QuorumParser.Loop_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#initial_parent_action_call}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInitial_parent_action_call(QuorumParser.Initial_parent_action_callContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#action_call}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAction_call(QuorumParser.Action_callContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Cast}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCast(QuorumParser.CastContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Null}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNull(QuorumParser.NullContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Multiplication}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMultiplication(QuorumParser.MultiplicationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Addition}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAddition(QuorumParser.AdditionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Or}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOr(QuorumParser.OrContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ParenthesisExpression}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParenthesisExpression(QuorumParser.ParenthesisExpressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Inherits}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInherits(QuorumParser.InheritsContext ctx);
	/**
	 * Visit a parse tree produced by the {@code String}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitString(QuorumParser.StringContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Integer}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInteger(QuorumParser.IntegerContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVariableFunctionCall(QuorumParser.VariableFunctionCallContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Input}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInput(QuorumParser.InputContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Not}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNot(QuorumParser.NotContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Equals}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEquals(QuorumParser.EqualsContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Decimal}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDecimal(QuorumParser.DecimalContext ctx);
	/**
	 * Visit a parse tree produced by the {@code And}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAnd(QuorumParser.AndContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Me}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMe(QuorumParser.MeContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Greater}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGreater(QuorumParser.GreaterContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Boolean}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBoolean(QuorumParser.BooleanContext ctx);
	/**
	 * Visit a parse tree produced by the {@code InputNoParameters}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInputNoParameters(QuorumParser.InputNoParametersContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ParentVariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParentVariableFunctionCall(QuorumParser.ParentVariableFunctionCallContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Minus}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMinus(QuorumParser.MinusContext ctx);
	/**
	 * Visit a parse tree produced by {@link QuorumParser#function_expression_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFunction_expression_list(QuorumParser.Function_expression_listContext ctx);
}