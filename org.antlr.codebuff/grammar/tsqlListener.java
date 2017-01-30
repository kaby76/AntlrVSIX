// Generated from tsql.g4 by ANTLR 4.6
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link tsqlParser}.
 */
public interface tsqlListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link tsqlParser#tsql_file}.
	 * @param ctx the parse tree
	 */
	void enterTsql_file(tsqlParser.Tsql_fileContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#tsql_file}.
	 * @param ctx the parse tree
	 */
	void exitTsql_file(tsqlParser.Tsql_fileContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#sql_clause}.
	 * @param ctx the parse tree
	 */
	void enterSql_clause(tsqlParser.Sql_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#sql_clause}.
	 * @param ctx the parse tree
	 */
	void exitSql_clause(tsqlParser.Sql_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#dml_clause}.
	 * @param ctx the parse tree
	 */
	void enterDml_clause(tsqlParser.Dml_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#dml_clause}.
	 * @param ctx the parse tree
	 */
	void exitDml_clause(tsqlParser.Dml_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#ddl_clause}.
	 * @param ctx the parse tree
	 */
	void enterDdl_clause(tsqlParser.Ddl_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#ddl_clause}.
	 * @param ctx the parse tree
	 */
	void exitDdl_clause(tsqlParser.Ddl_clauseContext ctx);
	/**
	 * Enter a parse tree produced by the {@code begin_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterBegin_statement(tsqlParser.Begin_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code begin_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitBegin_statement(tsqlParser.Begin_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code break_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterBreak_statement(tsqlParser.Break_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code break_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitBreak_statement(tsqlParser.Break_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code continue_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterContinue_statement(tsqlParser.Continue_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code continue_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitContinue_statement(tsqlParser.Continue_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code goto_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterGoto_statement(tsqlParser.Goto_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code goto_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitGoto_statement(tsqlParser.Goto_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code if_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterIf_statement(tsqlParser.If_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code if_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitIf_statement(tsqlParser.If_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code return_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterReturn_statement(tsqlParser.Return_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code return_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitReturn_statement(tsqlParser.Return_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code throw_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterThrow_statement(tsqlParser.Throw_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code throw_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitThrow_statement(tsqlParser.Throw_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code try_catch_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterTry_catch_statement(tsqlParser.Try_catch_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code try_catch_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitTry_catch_statement(tsqlParser.Try_catch_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code waitfor_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterWaitfor_statement(tsqlParser.Waitfor_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code waitfor_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitWaitfor_statement(tsqlParser.Waitfor_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code while_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterWhile_statement(tsqlParser.While_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code while_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitWhile_statement(tsqlParser.While_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code print_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterPrint_statement(tsqlParser.Print_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code print_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitPrint_statement(tsqlParser.Print_statementContext ctx);
	/**
	 * Enter a parse tree produced by the {@code raiseerror_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void enterRaiseerror_statement(tsqlParser.Raiseerror_statementContext ctx);
	/**
	 * Exit a parse tree produced by the {@code raiseerror_statement}
	 * labeled alternative in {@link tsqlParser#cfl_statement}.
	 * @param ctx the parse tree
	 */
	void exitRaiseerror_statement(tsqlParser.Raiseerror_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#another_statement}.
	 * @param ctx the parse tree
	 */
	void enterAnother_statement(tsqlParser.Another_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#another_statement}.
	 * @param ctx the parse tree
	 */
	void exitAnother_statement(tsqlParser.Another_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#delete_statement}.
	 * @param ctx the parse tree
	 */
	void enterDelete_statement(tsqlParser.Delete_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#delete_statement}.
	 * @param ctx the parse tree
	 */
	void exitDelete_statement(tsqlParser.Delete_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#insert_statement}.
	 * @param ctx the parse tree
	 */
	void enterInsert_statement(tsqlParser.Insert_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#insert_statement}.
	 * @param ctx the parse tree
	 */
	void exitInsert_statement(tsqlParser.Insert_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#select_statement}.
	 * @param ctx the parse tree
	 */
	void enterSelect_statement(tsqlParser.Select_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#select_statement}.
	 * @param ctx the parse tree
	 */
	void exitSelect_statement(tsqlParser.Select_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#update_statement}.
	 * @param ctx the parse tree
	 */
	void enterUpdate_statement(tsqlParser.Update_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#update_statement}.
	 * @param ctx the parse tree
	 */
	void exitUpdate_statement(tsqlParser.Update_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#output_clause}.
	 * @param ctx the parse tree
	 */
	void enterOutput_clause(tsqlParser.Output_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#output_clause}.
	 * @param ctx the parse tree
	 */
	void exitOutput_clause(tsqlParser.Output_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#output_dml_list_elem}.
	 * @param ctx the parse tree
	 */
	void enterOutput_dml_list_elem(tsqlParser.Output_dml_list_elemContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#output_dml_list_elem}.
	 * @param ctx the parse tree
	 */
	void exitOutput_dml_list_elem(tsqlParser.Output_dml_list_elemContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#output_column_name}.
	 * @param ctx the parse tree
	 */
	void enterOutput_column_name(tsqlParser.Output_column_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#output_column_name}.
	 * @param ctx the parse tree
	 */
	void exitOutput_column_name(tsqlParser.Output_column_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#create_index}.
	 * @param ctx the parse tree
	 */
	void enterCreate_index(tsqlParser.Create_indexContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#create_index}.
	 * @param ctx the parse tree
	 */
	void exitCreate_index(tsqlParser.Create_indexContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#create_procedure}.
	 * @param ctx the parse tree
	 */
	void enterCreate_procedure(tsqlParser.Create_procedureContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#create_procedure}.
	 * @param ctx the parse tree
	 */
	void exitCreate_procedure(tsqlParser.Create_procedureContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#procedure_param}.
	 * @param ctx the parse tree
	 */
	void enterProcedure_param(tsqlParser.Procedure_paramContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#procedure_param}.
	 * @param ctx the parse tree
	 */
	void exitProcedure_param(tsqlParser.Procedure_paramContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#procedure_option}.
	 * @param ctx the parse tree
	 */
	void enterProcedure_option(tsqlParser.Procedure_optionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#procedure_option}.
	 * @param ctx the parse tree
	 */
	void exitProcedure_option(tsqlParser.Procedure_optionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#create_statistics}.
	 * @param ctx the parse tree
	 */
	void enterCreate_statistics(tsqlParser.Create_statisticsContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#create_statistics}.
	 * @param ctx the parse tree
	 */
	void exitCreate_statistics(tsqlParser.Create_statisticsContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#create_table}.
	 * @param ctx the parse tree
	 */
	void enterCreate_table(tsqlParser.Create_tableContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#create_table}.
	 * @param ctx the parse tree
	 */
	void exitCreate_table(tsqlParser.Create_tableContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#create_view}.
	 * @param ctx the parse tree
	 */
	void enterCreate_view(tsqlParser.Create_viewContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#create_view}.
	 * @param ctx the parse tree
	 */
	void exitCreate_view(tsqlParser.Create_viewContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#view_attribute}.
	 * @param ctx the parse tree
	 */
	void enterView_attribute(tsqlParser.View_attributeContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#view_attribute}.
	 * @param ctx the parse tree
	 */
	void exitView_attribute(tsqlParser.View_attributeContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#alter_table}.
	 * @param ctx the parse tree
	 */
	void enterAlter_table(tsqlParser.Alter_tableContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#alter_table}.
	 * @param ctx the parse tree
	 */
	void exitAlter_table(tsqlParser.Alter_tableContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#alter_database}.
	 * @param ctx the parse tree
	 */
	void enterAlter_database(tsqlParser.Alter_databaseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#alter_database}.
	 * @param ctx the parse tree
	 */
	void exitAlter_database(tsqlParser.Alter_databaseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#database_option}.
	 * @param ctx the parse tree
	 */
	void enterDatabase_option(tsqlParser.Database_optionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#database_option}.
	 * @param ctx the parse tree
	 */
	void exitDatabase_option(tsqlParser.Database_optionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#drop_index}.
	 * @param ctx the parse tree
	 */
	void enterDrop_index(tsqlParser.Drop_indexContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#drop_index}.
	 * @param ctx the parse tree
	 */
	void exitDrop_index(tsqlParser.Drop_indexContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#drop_procedure}.
	 * @param ctx the parse tree
	 */
	void enterDrop_procedure(tsqlParser.Drop_procedureContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#drop_procedure}.
	 * @param ctx the parse tree
	 */
	void exitDrop_procedure(tsqlParser.Drop_procedureContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#drop_statistics}.
	 * @param ctx the parse tree
	 */
	void enterDrop_statistics(tsqlParser.Drop_statisticsContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#drop_statistics}.
	 * @param ctx the parse tree
	 */
	void exitDrop_statistics(tsqlParser.Drop_statisticsContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#drop_table}.
	 * @param ctx the parse tree
	 */
	void enterDrop_table(tsqlParser.Drop_tableContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#drop_table}.
	 * @param ctx the parse tree
	 */
	void exitDrop_table(tsqlParser.Drop_tableContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#drop_view}.
	 * @param ctx the parse tree
	 */
	void enterDrop_view(tsqlParser.Drop_viewContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#drop_view}.
	 * @param ctx the parse tree
	 */
	void exitDrop_view(tsqlParser.Drop_viewContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#rowset_function_limited}.
	 * @param ctx the parse tree
	 */
	void enterRowset_function_limited(tsqlParser.Rowset_function_limitedContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#rowset_function_limited}.
	 * @param ctx the parse tree
	 */
	void exitRowset_function_limited(tsqlParser.Rowset_function_limitedContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#openquery}.
	 * @param ctx the parse tree
	 */
	void enterOpenquery(tsqlParser.OpenqueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#openquery}.
	 * @param ctx the parse tree
	 */
	void exitOpenquery(tsqlParser.OpenqueryContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#opendatasource}.
	 * @param ctx the parse tree
	 */
	void enterOpendatasource(tsqlParser.OpendatasourceContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#opendatasource}.
	 * @param ctx the parse tree
	 */
	void exitOpendatasource(tsqlParser.OpendatasourceContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#declare_statement}.
	 * @param ctx the parse tree
	 */
	void enterDeclare_statement(tsqlParser.Declare_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#declare_statement}.
	 * @param ctx the parse tree
	 */
	void exitDeclare_statement(tsqlParser.Declare_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#cursor_statement}.
	 * @param ctx the parse tree
	 */
	void enterCursor_statement(tsqlParser.Cursor_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#cursor_statement}.
	 * @param ctx the parse tree
	 */
	void exitCursor_statement(tsqlParser.Cursor_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#execute_statement}.
	 * @param ctx the parse tree
	 */
	void enterExecute_statement(tsqlParser.Execute_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#execute_statement}.
	 * @param ctx the parse tree
	 */
	void exitExecute_statement(tsqlParser.Execute_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#execute_statement_arg}.
	 * @param ctx the parse tree
	 */
	void enterExecute_statement_arg(tsqlParser.Execute_statement_argContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#execute_statement_arg}.
	 * @param ctx the parse tree
	 */
	void exitExecute_statement_arg(tsqlParser.Execute_statement_argContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#execute_var_string}.
	 * @param ctx the parse tree
	 */
	void enterExecute_var_string(tsqlParser.Execute_var_stringContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#execute_var_string}.
	 * @param ctx the parse tree
	 */
	void exitExecute_var_string(tsqlParser.Execute_var_stringContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#security_statement}.
	 * @param ctx the parse tree
	 */
	void enterSecurity_statement(tsqlParser.Security_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#security_statement}.
	 * @param ctx the parse tree
	 */
	void exitSecurity_statement(tsqlParser.Security_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#set_statment}.
	 * @param ctx the parse tree
	 */
	void enterSet_statment(tsqlParser.Set_statmentContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#set_statment}.
	 * @param ctx the parse tree
	 */
	void exitSet_statment(tsqlParser.Set_statmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#transaction_statement}.
	 * @param ctx the parse tree
	 */
	void enterTransaction_statement(tsqlParser.Transaction_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#transaction_statement}.
	 * @param ctx the parse tree
	 */
	void exitTransaction_statement(tsqlParser.Transaction_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#go_statement}.
	 * @param ctx the parse tree
	 */
	void enterGo_statement(tsqlParser.Go_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#go_statement}.
	 * @param ctx the parse tree
	 */
	void exitGo_statement(tsqlParser.Go_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#use_statement}.
	 * @param ctx the parse tree
	 */
	void enterUse_statement(tsqlParser.Use_statementContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#use_statement}.
	 * @param ctx the parse tree
	 */
	void exitUse_statement(tsqlParser.Use_statementContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#execute_clause}.
	 * @param ctx the parse tree
	 */
	void enterExecute_clause(tsqlParser.Execute_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#execute_clause}.
	 * @param ctx the parse tree
	 */
	void exitExecute_clause(tsqlParser.Execute_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#declare_local}.
	 * @param ctx the parse tree
	 */
	void enterDeclare_local(tsqlParser.Declare_localContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#declare_local}.
	 * @param ctx the parse tree
	 */
	void exitDeclare_local(tsqlParser.Declare_localContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_type_definition}.
	 * @param ctx the parse tree
	 */
	void enterTable_type_definition(tsqlParser.Table_type_definitionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_type_definition}.
	 * @param ctx the parse tree
	 */
	void exitTable_type_definition(tsqlParser.Table_type_definitionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_def_table_constraint}.
	 * @param ctx the parse tree
	 */
	void enterColumn_def_table_constraint(tsqlParser.Column_def_table_constraintContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_def_table_constraint}.
	 * @param ctx the parse tree
	 */
	void exitColumn_def_table_constraint(tsqlParser.Column_def_table_constraintContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_definition}.
	 * @param ctx the parse tree
	 */
	void enterColumn_definition(tsqlParser.Column_definitionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_definition}.
	 * @param ctx the parse tree
	 */
	void exitColumn_definition(tsqlParser.Column_definitionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_constraint}.
	 * @param ctx the parse tree
	 */
	void enterColumn_constraint(tsqlParser.Column_constraintContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_constraint}.
	 * @param ctx the parse tree
	 */
	void exitColumn_constraint(tsqlParser.Column_constraintContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_constraint}.
	 * @param ctx the parse tree
	 */
	void enterTable_constraint(tsqlParser.Table_constraintContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_constraint}.
	 * @param ctx the parse tree
	 */
	void exitTable_constraint(tsqlParser.Table_constraintContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#index_options}.
	 * @param ctx the parse tree
	 */
	void enterIndex_options(tsqlParser.Index_optionsContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#index_options}.
	 * @param ctx the parse tree
	 */
	void exitIndex_options(tsqlParser.Index_optionsContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#index_option}.
	 * @param ctx the parse tree
	 */
	void enterIndex_option(tsqlParser.Index_optionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#index_option}.
	 * @param ctx the parse tree
	 */
	void exitIndex_option(tsqlParser.Index_optionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#declare_cursor}.
	 * @param ctx the parse tree
	 */
	void enterDeclare_cursor(tsqlParser.Declare_cursorContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#declare_cursor}.
	 * @param ctx the parse tree
	 */
	void exitDeclare_cursor(tsqlParser.Declare_cursorContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#declare_set_cursor_common}.
	 * @param ctx the parse tree
	 */
	void enterDeclare_set_cursor_common(tsqlParser.Declare_set_cursor_commonContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#declare_set_cursor_common}.
	 * @param ctx the parse tree
	 */
	void exitDeclare_set_cursor_common(tsqlParser.Declare_set_cursor_commonContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#fetch_cursor}.
	 * @param ctx the parse tree
	 */
	void enterFetch_cursor(tsqlParser.Fetch_cursorContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#fetch_cursor}.
	 * @param ctx the parse tree
	 */
	void exitFetch_cursor(tsqlParser.Fetch_cursorContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#set_special}.
	 * @param ctx the parse tree
	 */
	void enterSet_special(tsqlParser.Set_specialContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#set_special}.
	 * @param ctx the parse tree
	 */
	void exitSet_special(tsqlParser.Set_specialContext ctx);
	/**
	 * Enter a parse tree produced by the {@code binary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterBinary_operator_expression(tsqlParser.Binary_operator_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code binary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitBinary_operator_expression(tsqlParser.Binary_operator_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code primitive_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterPrimitive_expression(tsqlParser.Primitive_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code primitive_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitPrimitive_expression(tsqlParser.Primitive_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code bracket_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterBracket_expression(tsqlParser.Bracket_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code bracket_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitBracket_expression(tsqlParser.Bracket_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code unary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterUnary_operator_expression(tsqlParser.Unary_operator_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code unary_operator_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitUnary_operator_expression(tsqlParser.Unary_operator_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code function_call_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterFunction_call_expression(tsqlParser.Function_call_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code function_call_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitFunction_call_expression(tsqlParser.Function_call_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code case_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterCase_expression(tsqlParser.Case_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code case_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitCase_expression(tsqlParser.Case_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code column_ref_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterColumn_ref_expression(tsqlParser.Column_ref_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code column_ref_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitColumn_ref_expression(tsqlParser.Column_ref_expressionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code subquery_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void enterSubquery_expression(tsqlParser.Subquery_expressionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code subquery_expression}
	 * labeled alternative in {@link tsqlParser#expression}.
	 * @param ctx the parse tree
	 */
	void exitSubquery_expression(tsqlParser.Subquery_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#constant_expression}.
	 * @param ctx the parse tree
	 */
	void enterConstant_expression(tsqlParser.Constant_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#constant_expression}.
	 * @param ctx the parse tree
	 */
	void exitConstant_expression(tsqlParser.Constant_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#subquery}.
	 * @param ctx the parse tree
	 */
	void enterSubquery(tsqlParser.SubqueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#subquery}.
	 * @param ctx the parse tree
	 */
	void exitSubquery(tsqlParser.SubqueryContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#with_expression}.
	 * @param ctx the parse tree
	 */
	void enterWith_expression(tsqlParser.With_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#with_expression}.
	 * @param ctx the parse tree
	 */
	void exitWith_expression(tsqlParser.With_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#common_table_expression}.
	 * @param ctx the parse tree
	 */
	void enterCommon_table_expression(tsqlParser.Common_table_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#common_table_expression}.
	 * @param ctx the parse tree
	 */
	void exitCommon_table_expression(tsqlParser.Common_table_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#update_elem}.
	 * @param ctx the parse tree
	 */
	void enterUpdate_elem(tsqlParser.Update_elemContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#update_elem}.
	 * @param ctx the parse tree
	 */
	void exitUpdate_elem(tsqlParser.Update_elemContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#search_condition_list}.
	 * @param ctx the parse tree
	 */
	void enterSearch_condition_list(tsqlParser.Search_condition_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#search_condition_list}.
	 * @param ctx the parse tree
	 */
	void exitSearch_condition_list(tsqlParser.Search_condition_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#search_condition}.
	 * @param ctx the parse tree
	 */
	void enterSearch_condition(tsqlParser.Search_conditionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#search_condition}.
	 * @param ctx the parse tree
	 */
	void exitSearch_condition(tsqlParser.Search_conditionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#search_condition_or}.
	 * @param ctx the parse tree
	 */
	void enterSearch_condition_or(tsqlParser.Search_condition_orContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#search_condition_or}.
	 * @param ctx the parse tree
	 */
	void exitSearch_condition_or(tsqlParser.Search_condition_orContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#search_condition_not}.
	 * @param ctx the parse tree
	 */
	void enterSearch_condition_not(tsqlParser.Search_condition_notContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#search_condition_not}.
	 * @param ctx the parse tree
	 */
	void exitSearch_condition_not(tsqlParser.Search_condition_notContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#predicate}.
	 * @param ctx the parse tree
	 */
	void enterPredicate(tsqlParser.PredicateContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#predicate}.
	 * @param ctx the parse tree
	 */
	void exitPredicate(tsqlParser.PredicateContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#query_expression}.
	 * @param ctx the parse tree
	 */
	void enterQuery_expression(tsqlParser.Query_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#query_expression}.
	 * @param ctx the parse tree
	 */
	void exitQuery_expression(tsqlParser.Query_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#union}.
	 * @param ctx the parse tree
	 */
	void enterUnion(tsqlParser.UnionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#union}.
	 * @param ctx the parse tree
	 */
	void exitUnion(tsqlParser.UnionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#query_specification}.
	 * @param ctx the parse tree
	 */
	void enterQuery_specification(tsqlParser.Query_specificationContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#query_specification}.
	 * @param ctx the parse tree
	 */
	void exitQuery_specification(tsqlParser.Query_specificationContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#order_by_clause}.
	 * @param ctx the parse tree
	 */
	void enterOrder_by_clause(tsqlParser.Order_by_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#order_by_clause}.
	 * @param ctx the parse tree
	 */
	void exitOrder_by_clause(tsqlParser.Order_by_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#for_clause}.
	 * @param ctx the parse tree
	 */
	void enterFor_clause(tsqlParser.For_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#for_clause}.
	 * @param ctx the parse tree
	 */
	void exitFor_clause(tsqlParser.For_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#xml_common_directives}.
	 * @param ctx the parse tree
	 */
	void enterXml_common_directives(tsqlParser.Xml_common_directivesContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#xml_common_directives}.
	 * @param ctx the parse tree
	 */
	void exitXml_common_directives(tsqlParser.Xml_common_directivesContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#order_by_expression}.
	 * @param ctx the parse tree
	 */
	void enterOrder_by_expression(tsqlParser.Order_by_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#order_by_expression}.
	 * @param ctx the parse tree
	 */
	void exitOrder_by_expression(tsqlParser.Order_by_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#group_by_item}.
	 * @param ctx the parse tree
	 */
	void enterGroup_by_item(tsqlParser.Group_by_itemContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#group_by_item}.
	 * @param ctx the parse tree
	 */
	void exitGroup_by_item(tsqlParser.Group_by_itemContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#option_clause}.
	 * @param ctx the parse tree
	 */
	void enterOption_clause(tsqlParser.Option_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#option_clause}.
	 * @param ctx the parse tree
	 */
	void exitOption_clause(tsqlParser.Option_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#option}.
	 * @param ctx the parse tree
	 */
	void enterOption(tsqlParser.OptionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#option}.
	 * @param ctx the parse tree
	 */
	void exitOption(tsqlParser.OptionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#optimize_for_arg}.
	 * @param ctx the parse tree
	 */
	void enterOptimize_for_arg(tsqlParser.Optimize_for_argContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#optimize_for_arg}.
	 * @param ctx the parse tree
	 */
	void exitOptimize_for_arg(tsqlParser.Optimize_for_argContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#select_list}.
	 * @param ctx the parse tree
	 */
	void enterSelect_list(tsqlParser.Select_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#select_list}.
	 * @param ctx the parse tree
	 */
	void exitSelect_list(tsqlParser.Select_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#select_list_elem}.
	 * @param ctx the parse tree
	 */
	void enterSelect_list_elem(tsqlParser.Select_list_elemContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#select_list_elem}.
	 * @param ctx the parse tree
	 */
	void exitSelect_list_elem(tsqlParser.Select_list_elemContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#partition_by_clause}.
	 * @param ctx the parse tree
	 */
	void enterPartition_by_clause(tsqlParser.Partition_by_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#partition_by_clause}.
	 * @param ctx the parse tree
	 */
	void exitPartition_by_clause(tsqlParser.Partition_by_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_source}.
	 * @param ctx the parse tree
	 */
	void enterTable_source(tsqlParser.Table_sourceContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_source}.
	 * @param ctx the parse tree
	 */
	void exitTable_source(tsqlParser.Table_sourceContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_source_item_joined}.
	 * @param ctx the parse tree
	 */
	void enterTable_source_item_joined(tsqlParser.Table_source_item_joinedContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_source_item_joined}.
	 * @param ctx the parse tree
	 */
	void exitTable_source_item_joined(tsqlParser.Table_source_item_joinedContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_source_item}.
	 * @param ctx the parse tree
	 */
	void enterTable_source_item(tsqlParser.Table_source_itemContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_source_item}.
	 * @param ctx the parse tree
	 */
	void exitTable_source_item(tsqlParser.Table_source_itemContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#change_table}.
	 * @param ctx the parse tree
	 */
	void enterChange_table(tsqlParser.Change_tableContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#change_table}.
	 * @param ctx the parse tree
	 */
	void exitChange_table(tsqlParser.Change_tableContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#join_part}.
	 * @param ctx the parse tree
	 */
	void enterJoin_part(tsqlParser.Join_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#join_part}.
	 * @param ctx the parse tree
	 */
	void exitJoin_part(tsqlParser.Join_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_name_with_hint}.
	 * @param ctx the parse tree
	 */
	void enterTable_name_with_hint(tsqlParser.Table_name_with_hintContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_name_with_hint}.
	 * @param ctx the parse tree
	 */
	void exitTable_name_with_hint(tsqlParser.Table_name_with_hintContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#rowset_function}.
	 * @param ctx the parse tree
	 */
	void enterRowset_function(tsqlParser.Rowset_functionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#rowset_function}.
	 * @param ctx the parse tree
	 */
	void exitRowset_function(tsqlParser.Rowset_functionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#bulk_option}.
	 * @param ctx the parse tree
	 */
	void enterBulk_option(tsqlParser.Bulk_optionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#bulk_option}.
	 * @param ctx the parse tree
	 */
	void exitBulk_option(tsqlParser.Bulk_optionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#derived_table}.
	 * @param ctx the parse tree
	 */
	void enterDerived_table(tsqlParser.Derived_tableContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#derived_table}.
	 * @param ctx the parse tree
	 */
	void exitDerived_table(tsqlParser.Derived_tableContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#function_call}.
	 * @param ctx the parse tree
	 */
	void enterFunction_call(tsqlParser.Function_callContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#function_call}.
	 * @param ctx the parse tree
	 */
	void exitFunction_call(tsqlParser.Function_callContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#datepart}.
	 * @param ctx the parse tree
	 */
	void enterDatepart(tsqlParser.DatepartContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#datepart}.
	 * @param ctx the parse tree
	 */
	void exitDatepart(tsqlParser.DatepartContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#as_table_alias}.
	 * @param ctx the parse tree
	 */
	void enterAs_table_alias(tsqlParser.As_table_aliasContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#as_table_alias}.
	 * @param ctx the parse tree
	 */
	void exitAs_table_alias(tsqlParser.As_table_aliasContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_alias}.
	 * @param ctx the parse tree
	 */
	void enterTable_alias(tsqlParser.Table_aliasContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_alias}.
	 * @param ctx the parse tree
	 */
	void exitTable_alias(tsqlParser.Table_aliasContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#with_table_hints}.
	 * @param ctx the parse tree
	 */
	void enterWith_table_hints(tsqlParser.With_table_hintsContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#with_table_hints}.
	 * @param ctx the parse tree
	 */
	void exitWith_table_hints(tsqlParser.With_table_hintsContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#insert_with_table_hints}.
	 * @param ctx the parse tree
	 */
	void enterInsert_with_table_hints(tsqlParser.Insert_with_table_hintsContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#insert_with_table_hints}.
	 * @param ctx the parse tree
	 */
	void exitInsert_with_table_hints(tsqlParser.Insert_with_table_hintsContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_hint}.
	 * @param ctx the parse tree
	 */
	void enterTable_hint(tsqlParser.Table_hintContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_hint}.
	 * @param ctx the parse tree
	 */
	void exitTable_hint(tsqlParser.Table_hintContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#index_column_name}.
	 * @param ctx the parse tree
	 */
	void enterIndex_column_name(tsqlParser.Index_column_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#index_column_name}.
	 * @param ctx the parse tree
	 */
	void exitIndex_column_name(tsqlParser.Index_column_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#index_value}.
	 * @param ctx the parse tree
	 */
	void enterIndex_value(tsqlParser.Index_valueContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#index_value}.
	 * @param ctx the parse tree
	 */
	void exitIndex_value(tsqlParser.Index_valueContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_alias_list}.
	 * @param ctx the parse tree
	 */
	void enterColumn_alias_list(tsqlParser.Column_alias_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_alias_list}.
	 * @param ctx the parse tree
	 */
	void exitColumn_alias_list(tsqlParser.Column_alias_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_alias}.
	 * @param ctx the parse tree
	 */
	void enterColumn_alias(tsqlParser.Column_aliasContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_alias}.
	 * @param ctx the parse tree
	 */
	void exitColumn_alias(tsqlParser.Column_aliasContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#expression_list}.
	 * @param ctx the parse tree
	 */
	void enterExpression_list(tsqlParser.Expression_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#expression_list}.
	 * @param ctx the parse tree
	 */
	void exitExpression_list(tsqlParser.Expression_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#case_expr}.
	 * @param ctx the parse tree
	 */
	void enterCase_expr(tsqlParser.Case_exprContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#case_expr}.
	 * @param ctx the parse tree
	 */
	void exitCase_expr(tsqlParser.Case_exprContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#ranking_windowed_function}.
	 * @param ctx the parse tree
	 */
	void enterRanking_windowed_function(tsqlParser.Ranking_windowed_functionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#ranking_windowed_function}.
	 * @param ctx the parse tree
	 */
	void exitRanking_windowed_function(tsqlParser.Ranking_windowed_functionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#aggregate_windowed_function}.
	 * @param ctx the parse tree
	 */
	void enterAggregate_windowed_function(tsqlParser.Aggregate_windowed_functionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#aggregate_windowed_function}.
	 * @param ctx the parse tree
	 */
	void exitAggregate_windowed_function(tsqlParser.Aggregate_windowed_functionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#all_distinct_expression}.
	 * @param ctx the parse tree
	 */
	void enterAll_distinct_expression(tsqlParser.All_distinct_expressionContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#all_distinct_expression}.
	 * @param ctx the parse tree
	 */
	void exitAll_distinct_expression(tsqlParser.All_distinct_expressionContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#over_clause}.
	 * @param ctx the parse tree
	 */
	void enterOver_clause(tsqlParser.Over_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#over_clause}.
	 * @param ctx the parse tree
	 */
	void exitOver_clause(tsqlParser.Over_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#row_or_range_clause}.
	 * @param ctx the parse tree
	 */
	void enterRow_or_range_clause(tsqlParser.Row_or_range_clauseContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#row_or_range_clause}.
	 * @param ctx the parse tree
	 */
	void exitRow_or_range_clause(tsqlParser.Row_or_range_clauseContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#window_frame_extent}.
	 * @param ctx the parse tree
	 */
	void enterWindow_frame_extent(tsqlParser.Window_frame_extentContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#window_frame_extent}.
	 * @param ctx the parse tree
	 */
	void exitWindow_frame_extent(tsqlParser.Window_frame_extentContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#window_frame_bound}.
	 * @param ctx the parse tree
	 */
	void enterWindow_frame_bound(tsqlParser.Window_frame_boundContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#window_frame_bound}.
	 * @param ctx the parse tree
	 */
	void exitWindow_frame_bound(tsqlParser.Window_frame_boundContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#window_frame_preceding}.
	 * @param ctx the parse tree
	 */
	void enterWindow_frame_preceding(tsqlParser.Window_frame_precedingContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#window_frame_preceding}.
	 * @param ctx the parse tree
	 */
	void exitWindow_frame_preceding(tsqlParser.Window_frame_precedingContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#window_frame_following}.
	 * @param ctx the parse tree
	 */
	void enterWindow_frame_following(tsqlParser.Window_frame_followingContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#window_frame_following}.
	 * @param ctx the parse tree
	 */
	void exitWindow_frame_following(tsqlParser.Window_frame_followingContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#full_table_name}.
	 * @param ctx the parse tree
	 */
	void enterFull_table_name(tsqlParser.Full_table_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#full_table_name}.
	 * @param ctx the parse tree
	 */
	void exitFull_table_name(tsqlParser.Full_table_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#table_name}.
	 * @param ctx the parse tree
	 */
	void enterTable_name(tsqlParser.Table_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#table_name}.
	 * @param ctx the parse tree
	 */
	void exitTable_name(tsqlParser.Table_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#view_name}.
	 * @param ctx the parse tree
	 */
	void enterView_name(tsqlParser.View_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#view_name}.
	 * @param ctx the parse tree
	 */
	void exitView_name(tsqlParser.View_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#func_proc_name}.
	 * @param ctx the parse tree
	 */
	void enterFunc_proc_name(tsqlParser.Func_proc_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#func_proc_name}.
	 * @param ctx the parse tree
	 */
	void exitFunc_proc_name(tsqlParser.Func_proc_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#ddl_object}.
	 * @param ctx the parse tree
	 */
	void enterDdl_object(tsqlParser.Ddl_objectContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#ddl_object}.
	 * @param ctx the parse tree
	 */
	void exitDdl_object(tsqlParser.Ddl_objectContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#full_column_name}.
	 * @param ctx the parse tree
	 */
	void enterFull_column_name(tsqlParser.Full_column_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#full_column_name}.
	 * @param ctx the parse tree
	 */
	void exitFull_column_name(tsqlParser.Full_column_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_name_list}.
	 * @param ctx the parse tree
	 */
	void enterColumn_name_list(tsqlParser.Column_name_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_name_list}.
	 * @param ctx the parse tree
	 */
	void exitColumn_name_list(tsqlParser.Column_name_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#column_name}.
	 * @param ctx the parse tree
	 */
	void enterColumn_name(tsqlParser.Column_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#column_name}.
	 * @param ctx the parse tree
	 */
	void exitColumn_name(tsqlParser.Column_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#cursor_name}.
	 * @param ctx the parse tree
	 */
	void enterCursor_name(tsqlParser.Cursor_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#cursor_name}.
	 * @param ctx the parse tree
	 */
	void exitCursor_name(tsqlParser.Cursor_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#on_off}.
	 * @param ctx the parse tree
	 */
	void enterOn_off(tsqlParser.On_offContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#on_off}.
	 * @param ctx the parse tree
	 */
	void exitOn_off(tsqlParser.On_offContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#clustered}.
	 * @param ctx the parse tree
	 */
	void enterClustered(tsqlParser.ClusteredContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#clustered}.
	 * @param ctx the parse tree
	 */
	void exitClustered(tsqlParser.ClusteredContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#null_notnull}.
	 * @param ctx the parse tree
	 */
	void enterNull_notnull(tsqlParser.Null_notnullContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#null_notnull}.
	 * @param ctx the parse tree
	 */
	void exitNull_notnull(tsqlParser.Null_notnullContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#scalar_function_name}.
	 * @param ctx the parse tree
	 */
	void enterScalar_function_name(tsqlParser.Scalar_function_nameContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#scalar_function_name}.
	 * @param ctx the parse tree
	 */
	void exitScalar_function_name(tsqlParser.Scalar_function_nameContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#data_type}.
	 * @param ctx the parse tree
	 */
	void enterData_type(tsqlParser.Data_typeContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#data_type}.
	 * @param ctx the parse tree
	 */
	void exitData_type(tsqlParser.Data_typeContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#default_value}.
	 * @param ctx the parse tree
	 */
	void enterDefault_value(tsqlParser.Default_valueContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#default_value}.
	 * @param ctx the parse tree
	 */
	void exitDefault_value(tsqlParser.Default_valueContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#constant}.
	 * @param ctx the parse tree
	 */
	void enterConstant(tsqlParser.ConstantContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#constant}.
	 * @param ctx the parse tree
	 */
	void exitConstant(tsqlParser.ConstantContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#number}.
	 * @param ctx the parse tree
	 */
	void enterNumber(tsqlParser.NumberContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#number}.
	 * @param ctx the parse tree
	 */
	void exitNumber(tsqlParser.NumberContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#sign}.
	 * @param ctx the parse tree
	 */
	void enterSign(tsqlParser.SignContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#sign}.
	 * @param ctx the parse tree
	 */
	void exitSign(tsqlParser.SignContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#id}.
	 * @param ctx the parse tree
	 */
	void enterId(tsqlParser.IdContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#id}.
	 * @param ctx the parse tree
	 */
	void exitId(tsqlParser.IdContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#simple_id}.
	 * @param ctx the parse tree
	 */
	void enterSimple_id(tsqlParser.Simple_idContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#simple_id}.
	 * @param ctx the parse tree
	 */
	void exitSimple_id(tsqlParser.Simple_idContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#comparison_operator}.
	 * @param ctx the parse tree
	 */
	void enterComparison_operator(tsqlParser.Comparison_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#comparison_operator}.
	 * @param ctx the parse tree
	 */
	void exitComparison_operator(tsqlParser.Comparison_operatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link tsqlParser#assignment_operator}.
	 * @param ctx the parse tree
	 */
	void enterAssignment_operator(tsqlParser.Assignment_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link tsqlParser#assignment_operator}.
	 * @param ctx the parse tree
	 */
	void exitAssignment_operator(tsqlParser.Assignment_operatorContext ctx);
}