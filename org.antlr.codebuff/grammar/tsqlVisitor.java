// Generated from tsql.g4 by ANTLR 4.6
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link tsqlParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface tsqlVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link tsqlParser#tsql_file}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTsql_file(tsqlParser.Tsql_fileContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#sql_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSql_clause(tsqlParser.Sql_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#dml_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDml_clause(tsqlParser.Dml_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#ddl_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDdl_clause(tsqlParser.Ddl_clauseContext ctx);
	/**
	 * Visit a parse tree produced by the {@code begin_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBegin_statement(tsqlParser.Begin_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code break_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBreak_statement(tsqlParser.Break_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code continue_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitContinue_statement(tsqlParser.Continue_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code goto_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGoto_statement(tsqlParser.Goto_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code if_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIf_statement(tsqlParser.If_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code return_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitReturn_statement(tsqlParser.Return_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code throw_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitThrow_statement(tsqlParser.Throw_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code try_catch_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTry_catch_statement(tsqlParser.Try_catch_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code waitfor_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWaitfor_statement(tsqlParser.Waitfor_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code while_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWhile_statement(tsqlParser.While_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code print_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrint_statement(tsqlParser.Print_statementContext ctx);
	/**
	 * Visit a parse tree produced by the {@code raiseerror_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRaiseerror_statement(tsqlParser.Raiseerror_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#another_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAnother_statement(tsqlParser.Another_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#delete_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDelete_statement(tsqlParser.Delete_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#insert_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInsert_statement(tsqlParser.Insert_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#select_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSelect_statement(tsqlParser.Select_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#update_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUpdate_statement(tsqlParser.Update_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#output_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOutput_clause(tsqlParser.Output_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#output_dml_list_elem}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOutput_dml_list_elem(tsqlParser.Output_dml_list_elemContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#output_column_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOutput_column_name(tsqlParser.Output_column_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#create_index}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCreate_index(tsqlParser.Create_indexContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#create_procedure}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCreate_procedure(tsqlParser.Create_procedureContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#procedure_param}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitProcedure_param(tsqlParser.Procedure_paramContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#procedure_option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitProcedure_option(tsqlParser.Procedure_optionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#create_statistics}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCreate_statistics(tsqlParser.Create_statisticsContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#create_table}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCreate_table(tsqlParser.Create_tableContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#create_view}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCreate_view(tsqlParser.Create_viewContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#view_attribute}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitView_attribute(tsqlParser.View_attributeContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#alter_table}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlter_table(tsqlParser.Alter_tableContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#alter_database}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAlter_database(tsqlParser.Alter_databaseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#database_option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDatabase_option(tsqlParser.Database_optionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#drop_index}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDrop_index(tsqlParser.Drop_indexContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#drop_procedure}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDrop_procedure(tsqlParser.Drop_procedureContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#drop_statistics}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDrop_statistics(tsqlParser.Drop_statisticsContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#drop_table}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDrop_table(tsqlParser.Drop_tableContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#drop_view}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDrop_view(tsqlParser.Drop_viewContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#rowset_function_limited}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRowset_function_limited(tsqlParser.Rowset_function_limitedContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#openquery}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOpenquery(tsqlParser.OpenqueryContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#opendatasource}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOpendatasource(tsqlParser.OpendatasourceContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#declare_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDeclare_statement(tsqlParser.Declare_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#cursor_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCursor_statement(tsqlParser.Cursor_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#execute_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExecute_statement(tsqlParser.Execute_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#execute_statement_arg}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExecute_statement_arg(tsqlParser.Execute_statement_argContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#execute_var_string}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExecute_var_string(tsqlParser.Execute_var_stringContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#security_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSecurity_statement(tsqlParser.Security_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#set_statment}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSet_statment(tsqlParser.Set_statmentContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#transaction_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTransaction_statement(tsqlParser.Transaction_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#go_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGo_statement(tsqlParser.Go_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#use_statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUse_statement(tsqlParser.Use_statementContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#execute_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExecute_clause(tsqlParser.Execute_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#declare_local}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDeclare_local(tsqlParser.Declare_localContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_type_definition}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_type_definition(tsqlParser.Table_type_definitionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_def_table_constraint}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_def_table_constraint(tsqlParser.Column_def_table_constraintContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_definition}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_definition(tsqlParser.Column_definitionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_constraint}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_constraint(tsqlParser.Column_constraintContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_constraint}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_constraint(tsqlParser.Table_constraintContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#index_options}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIndex_options(tsqlParser.Index_optionsContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#index_option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIndex_option(tsqlParser.Index_optionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#declare_cursor}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDeclare_cursor(tsqlParser.Declare_cursorContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#declare_set_cursor_common}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDeclare_set_cursor_common(tsqlParser.Declare_set_cursor_commonContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#fetch_cursor}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFetch_cursor(tsqlParser.Fetch_cursorContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#set_special}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSet_special(tsqlParser.Set_specialContext ctx);
	/**
	 * Visit a parse tree produced by the {@code binary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBinary_operator_expression(tsqlParser.Binary_operator_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code primitive_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPrimitive_expression(tsqlParser.Primitive_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code bracket_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBracket_expression(tsqlParser.Bracket_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code unary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUnary_operator_expression(tsqlParser.Unary_operator_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code function_call_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFunction_call_expression(tsqlParser.Function_call_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code case_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCase_expression(tsqlParser.Case_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code column_ref_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_ref_expression(tsqlParser.Column_ref_expressionContext ctx);
	/**
	 * Visit a parse tree produced by the {@code subquery_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSubquery_expression(tsqlParser.Subquery_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#constant_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitConstant_expression(tsqlParser.Constant_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#subquery}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSubquery(tsqlParser.SubqueryContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#with_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWith_expression(tsqlParser.With_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#common_table_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCommon_table_expression(tsqlParser.Common_table_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#update_elem}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUpdate_elem(tsqlParser.Update_elemContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#search_condition_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSearch_condition_list(tsqlParser.Search_condition_listContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#search_condition}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSearch_condition(tsqlParser.Search_conditionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#search_condition_or}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSearch_condition_or(tsqlParser.Search_condition_orContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#search_condition_not}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSearch_condition_not(tsqlParser.Search_condition_notContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#predicate}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPredicate(tsqlParser.PredicateContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#query_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQuery_expression(tsqlParser.Query_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#union}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUnion(tsqlParser.UnionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#query_specification}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQuery_specification(tsqlParser.Query_specificationContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#order_by_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOrder_by_clause(tsqlParser.Order_by_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#for_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFor_clause(tsqlParser.For_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#xml_common_directives}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitXml_common_directives(tsqlParser.Xml_common_directivesContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#order_by_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOrder_by_expression(tsqlParser.Order_by_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#group_by_item}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGroup_by_item(tsqlParser.Group_by_itemContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#option_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOption_clause(tsqlParser.Option_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOption(tsqlParser.OptionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#optimize_for_arg}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOptimize_for_arg(tsqlParser.Optimize_for_argContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#select_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSelect_list(tsqlParser.Select_listContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#select_list_elem}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSelect_list_elem(tsqlParser.Select_list_elemContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#partition_by_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPartition_by_clause(tsqlParser.Partition_by_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_source}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_source(tsqlParser.Table_sourceContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_source_item_joined}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_source_item_joined(tsqlParser.Table_source_item_joinedContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_source_item}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_source_item(tsqlParser.Table_source_itemContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#change_table}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitChange_table(tsqlParser.Change_tableContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#join_part}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJoin_part(tsqlParser.Join_partContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_name_with_hint}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_name_with_hint(tsqlParser.Table_name_with_hintContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#rowset_function}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRowset_function(tsqlParser.Rowset_functionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#bulk_option}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBulk_option(tsqlParser.Bulk_optionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#derived_table}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDerived_table(tsqlParser.Derived_tableContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#function_call}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFunction_call(tsqlParser.Function_callContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#datepart}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDatepart(tsqlParser.DatepartContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#as_table_alias}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAs_table_alias(tsqlParser.As_table_aliasContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_alias}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_alias(tsqlParser.Table_aliasContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#with_table_hints}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWith_table_hints(tsqlParser.With_table_hintsContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#insert_with_table_hints}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitInsert_with_table_hints(tsqlParser.Insert_with_table_hintsContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_hint}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_hint(tsqlParser.Table_hintContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#index_column_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIndex_column_name(tsqlParser.Index_column_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#index_value}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIndex_value(tsqlParser.Index_valueContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_alias_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_alias_list(tsqlParser.Column_alias_listContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_alias}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_alias(tsqlParser.Column_aliasContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#expression_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExpression_list(tsqlParser.Expression_listContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#case_expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCase_expr(tsqlParser.Case_exprContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#ranking_windowed_function}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRanking_windowed_function(tsqlParser.Ranking_windowed_functionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#aggregate_windowed_function}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAggregate_windowed_function(tsqlParser.Aggregate_windowed_functionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#all_distinct_expression}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAll_distinct_expression(tsqlParser.All_distinct_expressionContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#over_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOver_clause(tsqlParser.Over_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#row_or_range_clause}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRow_or_range_clause(tsqlParser.Row_or_range_clauseContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#window_frame_extent}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWindow_frame_extent(tsqlParser.Window_frame_extentContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#window_frame_bound}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWindow_frame_bound(tsqlParser.Window_frame_boundContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#window_frame_preceding}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWindow_frame_preceding(tsqlParser.Window_frame_precedingContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#window_frame_following}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWindow_frame_following(tsqlParser.Window_frame_followingContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#full_table_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFull_table_name(tsqlParser.Full_table_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#table_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTable_name(tsqlParser.Table_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#view_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitView_name(tsqlParser.View_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#func_proc_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFunc_proc_name(tsqlParser.Func_proc_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#ddl_object}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDdl_object(tsqlParser.Ddl_objectContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#full_column_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFull_column_name(tsqlParser.Full_column_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_name_list}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_name_list(tsqlParser.Column_name_listContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#column_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitColumn_name(tsqlParser.Column_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#cursor_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCursor_name(tsqlParser.Cursor_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#on_off}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOn_off(tsqlParser.On_offContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#clustered}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitClustered(tsqlParser.ClusteredContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#null_notnull}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNull_notnull(tsqlParser.Null_notnullContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#scalar_function_name}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitScalar_function_name(tsqlParser.Scalar_function_nameContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#data_type}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitData_type(tsqlParser.Data_typeContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#default_value}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDefault_value(tsqlParser.Default_valueContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#constant}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitConstant(tsqlParser.ConstantContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#number}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNumber(tsqlParser.NumberContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#sign}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSign(tsqlParser.SignContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#id}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitId(tsqlParser.IdContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#simple_id}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSimple_id(tsqlParser.Simple_idContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#comparison_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitComparison_operator(tsqlParser.Comparison_operatorContext ctx);
	/**
	 * Visit a parse tree produced by {@link tsqlParser#assignment_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAssignment_operator(tsqlParser.Assignment_operatorContext ctx);
}