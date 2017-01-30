// Generated from Quorum.g4 by ANTLR 4.6
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link QuorumParser}.
 */
public interface QuorumListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link QuorumParser#start}.
	 * @param ctx the parse tree
	 */
	void enterStart(QuorumParser.StartContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#start}.
	 * @param ctx the parse tree
	 */
	void exitStart(QuorumParser.StartContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#package_rule}.
	 * @param ctx the parse tree
	 */
	void enterPackage_rule(QuorumParser.Package_ruleContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#package_rule}.
	 * @param ctx the parse tree
	 */
	void exitPackage_rule(QuorumParser.Package_ruleContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#reference}.
	 * @param ctx the parse tree
	 */
	void enterReference(QuorumParser.ReferenceContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#reference}.
	 * @param ctx the parse tree
	 */
	void exitReference(QuorumParser.ReferenceContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FullClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 */
	void enterFullClassDeclaration(QuorumParser.FullClassDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FullClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 */
	void exitFullClassDeclaration(QuorumParser.FullClassDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NoClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 */
	void enterNoClassDeclaration(QuorumParser.NoClassDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NoClassDeclaration}
	 * labeled alternative in {@link QuorumParser#class_declaration}.
	 * @param ctx the parse tree
	 */
	void exitNoClassDeclaration(QuorumParser.NoClassDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NoActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 */
	void enterNoActionsNoClass(QuorumParser.NoActionsNoClassContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NoActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 */
	void exitNoActionsNoClass(QuorumParser.NoActionsNoClassContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 */
	void enterActionsNoClass(QuorumParser.ActionsNoClassContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ActionsNoClass}
	 * labeled alternative in {@link QuorumParser#no_class_stmnts}.
	 * @param ctx the parse tree
	 */
	void exitActionsNoClass(QuorumParser.ActionsNoClassContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#inherit_stmnts}.
	 * @param ctx the parse tree
	 */
	void enterInherit_stmnts(QuorumParser.Inherit_stmntsContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#inherit_stmnts}.
	 * @param ctx the parse tree
	 */
	void exitInherit_stmnts(QuorumParser.Inherit_stmntsContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#inherit_stmt}.
	 * @param ctx the parse tree
	 */
	void enterInherit_stmt(QuorumParser.Inherit_stmtContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#inherit_stmt}.
	 * @param ctx the parse tree
	 */
	void exitInherit_stmt(QuorumParser.Inherit_stmtContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#access_modifier}.
	 * @param ctx the parse tree
	 */
	void enterAccess_modifier(QuorumParser.Access_modifierContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#access_modifier}.
	 * @param ctx the parse tree
	 */
	void exitAccess_modifier(QuorumParser.Access_modifierContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#class_stmnts}.
	 * @param ctx the parse tree
	 */
	void enterClass_stmnts(QuorumParser.Class_stmntsContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#class_stmnts}.
	 * @param ctx the parse tree
	 */
	void exitClass_stmnts(QuorumParser.Class_stmntsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Action}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void enterAction(QuorumParser.ActionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Action}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void exitAction(QuorumParser.ActionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BlueprintAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void enterBlueprintAction(QuorumParser.BlueprintActionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BlueprintAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void exitBlueprintAction(QuorumParser.BlueprintActionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NativeAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void enterNativeAction(QuorumParser.NativeActionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NativeAction}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void exitNativeAction(QuorumParser.NativeActionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Constructor}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void enterConstructor(QuorumParser.ConstructorContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Constructor}
	 * labeled alternative in {@link QuorumParser#method_declaration}.
	 * @param ctx the parse tree
	 */
	void exitConstructor(QuorumParser.ConstructorContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#method_shared}.
	 * @param ctx the parse tree
	 */
	void enterMethod_shared(QuorumParser.Method_sharedContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#method_shared}.
	 * @param ctx the parse tree
	 */
	void exitMethod_shared(QuorumParser.Method_sharedContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#formal_parameter}.
	 * @param ctx the parse tree
	 */
	void enterFormal_parameter(QuorumParser.Formal_parameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#formal_parameter}.
	 * @param ctx the parse tree
	 */
	void exitFormal_parameter(QuorumParser.Formal_parameterContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#qualified_name}.
	 * @param ctx the parse tree
	 */
	void enterQualified_name(QuorumParser.Qualified_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#qualified_name}.
	 * @param ctx the parse tree
	 */
	void exitQualified_name(QuorumParser.Qualified_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#block}.
	 * @param ctx the parse tree
	 */
	void enterBlock(QuorumParser.BlockContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#block}.
	 * @param ctx the parse tree
	 */
	void exitBlock(QuorumParser.BlockContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(QuorumParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(QuorumParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code VariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 */
	void enterVariableSoloFunctionCall(QuorumParser.VariableSoloFunctionCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code VariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 */
	void exitVariableSoloFunctionCall(QuorumParser.VariableSoloFunctionCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ParentVariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 */
	void enterParentVariableSoloFunctionCall(QuorumParser.ParentVariableSoloFunctionCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ParentVariableSoloFunctionCall}
	 * labeled alternative in {@link QuorumParser#solo_method_call}.
	 * @param ctx the parse tree
	 */
	void exitParentVariableSoloFunctionCall(QuorumParser.ParentVariableSoloFunctionCallContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#solo_method_required_method_part}.
	 * @param ctx the parse tree
	 */
	void enterSolo_method_required_method_part(QuorumParser.Solo_method_required_method_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#solo_method_required_method_part}.
	 * @param ctx the parse tree
	 */
	void exitSolo_method_required_method_part(QuorumParser.Solo_method_required_method_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#alert_statement}.
	 * @param ctx the parse tree
	 */
	void enterAlert_statement(QuorumParser.Alert_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#alert_statement}.
	 * @param ctx the parse tree
	 */
	void exitAlert_statement(QuorumParser.Alert_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#check_statement}.
	 * @param ctx the parse tree
	 */
	void enterCheck_statement(QuorumParser.Check_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#check_statement}.
	 * @param ctx the parse tree
	 */
	void exitCheck_statement(QuorumParser.Check_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#detect_statement}.
	 * @param ctx the parse tree
	 */
	void enterDetect_statement(QuorumParser.Detect_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#detect_statement}.
	 * @param ctx the parse tree
	 */
	void exitDetect_statement(QuorumParser.Detect_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#always_statement}.
	 * @param ctx the parse tree
	 */
	void enterAlways_statement(QuorumParser.Always_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#always_statement}.
	 * @param ctx the parse tree
	 */
	void exitAlways_statement(QuorumParser.Always_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#print_statement}.
	 * @param ctx the parse tree
	 */
	void enterPrint_statement(QuorumParser.Print_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#print_statement}.
	 * @param ctx the parse tree
	 */
	void exitPrint_statement(QuorumParser.Print_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#speak_statement}.
	 * @param ctx the parse tree
	 */
	void enterSpeak_statement(QuorumParser.Speak_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#speak_statement}.
	 * @param ctx the parse tree
	 */
	void exitSpeak_statement(QuorumParser.Speak_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#return_statement}.
	 * @param ctx the parse tree
	 */
	void enterReturn_statement(QuorumParser.Return_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#return_statement}.
	 * @param ctx the parse tree
	 */
	void exitReturn_statement(QuorumParser.Return_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#generic_declaration}.
	 * @param ctx the parse tree
	 */
	void enterGeneric_declaration(QuorumParser.Generic_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#generic_declaration}.
	 * @param ctx the parse tree
	 */
	void exitGeneric_declaration(QuorumParser.Generic_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#generic_statement}.
	 * @param ctx the parse tree
	 */
	void enterGeneric_statement(QuorumParser.Generic_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#generic_statement}.
	 * @param ctx the parse tree
	 */
	void exitGeneric_statement(QuorumParser.Generic_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#class_type}.
	 * @param ctx the parse tree
	 */
	void enterClass_type(QuorumParser.Class_typeContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#class_type}.
	 * @param ctx the parse tree
	 */
	void exitClass_type(QuorumParser.Class_typeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code GenericAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void enterGenericAssignmentDeclaration(QuorumParser.GenericAssignmentDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code GenericAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void exitGenericAssignmentDeclaration(QuorumParser.GenericAssignmentDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code IntegerAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void enterIntegerAssignmentDeclaration(QuorumParser.IntegerAssignmentDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code IntegerAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void exitIntegerAssignmentDeclaration(QuorumParser.IntegerAssignmentDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NumberAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void enterNumberAssignmentDeclaration(QuorumParser.NumberAssignmentDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NumberAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void exitNumberAssignmentDeclaration(QuorumParser.NumberAssignmentDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code TextAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void enterTextAssignmentDeclaration(QuorumParser.TextAssignmentDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code TextAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void exitTextAssignmentDeclaration(QuorumParser.TextAssignmentDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BooleanAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void enterBooleanAssignmentDeclaration(QuorumParser.BooleanAssignmentDeclarationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BooleanAssignmentDeclaration}
	 * labeled alternative in {@link QuorumParser#assignment_declaration}.
	 * @param ctx the parse tree
	 */
	void exitBooleanAssignmentDeclaration(QuorumParser.BooleanAssignmentDeclarationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NoTypeAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void enterNoTypeAssignment(QuorumParser.NoTypeAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NoTypeAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void exitNoTypeAssignment(QuorumParser.NoTypeAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ParentAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void enterParentAssignment(QuorumParser.ParentAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ParentAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void exitParentAssignment(QuorumParser.ParentAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ObjectAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void enterObjectAssignment(QuorumParser.ObjectAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ObjectAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void exitObjectAssignment(QuorumParser.ObjectAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by the {@code NormalAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void enterNormalAssignment(QuorumParser.NormalAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by the {@code NormalAssignment}
	 * labeled alternative in {@link QuorumParser#assignment_statement}.
	 * @param ctx the parse tree
	 */
	void exitNormalAssignment(QuorumParser.NormalAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#if_statement}.
	 * @param ctx the parse tree
	 */
	void enterIf_statement(QuorumParser.If_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#if_statement}.
	 * @param ctx the parse tree
	 */
	void exitIf_statement(QuorumParser.If_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#elseif_statement}.
	 * @param ctx the parse tree
	 */
	void enterElseif_statement(QuorumParser.Elseif_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#elseif_statement}.
	 * @param ctx the parse tree
	 */
	void exitElseif_statement(QuorumParser.Elseif_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#else_statement}.
	 * @param ctx the parse tree
	 */
	void enterElse_statement(QuorumParser.Else_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#else_statement}.
	 * @param ctx the parse tree
	 */
	void exitElse_statement(QuorumParser.Else_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#loop_statement}.
	 * @param ctx the parse tree
	 */
	void enterLoop_statement(QuorumParser.Loop_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#loop_statement}.
	 * @param ctx the parse tree
	 */
	void exitLoop_statement(QuorumParser.Loop_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#initial_parent_action_call}.
	 * @param ctx the parse tree
	 */
	void enterInitial_parent_action_call(QuorumParser.Initial_parent_action_callContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#initial_parent_action_call}.
	 * @param ctx the parse tree
	 */
	void exitInitial_parent_action_call(QuorumParser.Initial_parent_action_callContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#action_call}.
	 * @param ctx the parse tree
	 */
	void enterAction_call(QuorumParser.Action_callContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#action_call}.
	 * @param ctx the parse tree
	 */
	void exitAction_call(QuorumParser.Action_callContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Cast}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterCast(QuorumParser.CastContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Cast}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitCast(QuorumParser.CastContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Null}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterNull(QuorumParser.NullContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Null}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitNull(QuorumParser.NullContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Multiplication}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterMultiplication(QuorumParser.MultiplicationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Multiplication}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitMultiplication(QuorumParser.MultiplicationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Addition}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterAddition(QuorumParser.AdditionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Addition}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitAddition(QuorumParser.AdditionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Or}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterOr(QuorumParser.OrContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Or}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitOr(QuorumParser.OrContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ParenthesisExpression}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterParenthesisExpression(QuorumParser.ParenthesisExpressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ParenthesisExpression}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitParenthesisExpression(QuorumParser.ParenthesisExpressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Inherits}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterInherits(QuorumParser.InheritsContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Inherits}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitInherits(QuorumParser.InheritsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code String}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterString(QuorumParser.StringContext ctx);
	/**
	 * Exit a parse tree produced by the {@code String}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitString(QuorumParser.StringContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Integer}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterInteger(QuorumParser.IntegerContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Integer}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitInteger(QuorumParser.IntegerContext ctx);
	/**
	 * Enter a parse tree produced by the {@code VariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterVariableFunctionCall(QuorumParser.VariableFunctionCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code VariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitVariableFunctionCall(QuorumParser.VariableFunctionCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Input}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterInput(QuorumParser.InputContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Input}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitInput(QuorumParser.InputContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Not}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterNot(QuorumParser.NotContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Not}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitNot(QuorumParser.NotContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Equals}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterEquals(QuorumParser.EqualsContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Equals}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitEquals(QuorumParser.EqualsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Decimal}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterDecimal(QuorumParser.DecimalContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Decimal}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitDecimal(QuorumParser.DecimalContext ctx);
	/**
	 * Enter a parse tree produced by the {@code And}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterAnd(QuorumParser.AndContext ctx);
	/**
	 * Exit a parse tree produced by the {@code And}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitAnd(QuorumParser.AndContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Me}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterMe(QuorumParser.MeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Me}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitMe(QuorumParser.MeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Greater}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterGreater(QuorumParser.GreaterContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Greater}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitGreater(QuorumParser.GreaterContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Boolean}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterBoolean(QuorumParser.BooleanContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Boolean}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitBoolean(QuorumParser.BooleanContext ctx);
	/**
	 * Enter a parse tree produced by the {@code InputNoParameters}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterInputNoParameters(QuorumParser.InputNoParametersContext ctx);
	/**
	 * Exit a parse tree produced by the {@code InputNoParameters}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitInputNoParameters(QuorumParser.InputNoParametersContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ParentVariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterParentVariableFunctionCall(QuorumParser.ParentVariableFunctionCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ParentVariableFunctionCall}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitParentVariableFunctionCall(QuorumParser.ParentVariableFunctionCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Minus}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterMinus(QuorumParser.MinusContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Minus}
	 * labeled alternative in {@link QuorumParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitMinus(QuorumParser.MinusContext ctx);
	/**
	 * Enter a parse tree produced by {@link QuorumParser#function_expression_list}.
	 * @param ctx the parse tree
	 */
	void enterFunction_expression_list(QuorumParser.Function_expression_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link QuorumParser#function_expression_list}.
	 * @param ctx the parse tree
	 */
	void exitFunction_expression_list(QuorumParser.Function_expression_listContext ctx);
}