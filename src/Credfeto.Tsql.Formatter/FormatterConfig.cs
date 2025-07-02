using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EditorConfig.Core;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Credfeto.Tsql.Formatter;

internal static class FormatterConfig
{
    public static ValueTask<SqlScriptGeneratorOptions> GetOptionsFromEditorConfigAsync(string fullPath, in CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EditorConfigParser parser = new();
        FileConfiguration rules = parser.Parse(fullPath);

        SqlScriptGeneratorOptions options = TSqlOptions.DefaultOptions;

        cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.FromResult(new SqlScriptGeneratorOptions
                                    {
                                        AlignClauseBodies = GetValue(rule: rules, property: "align_clause_bodies", defaultValue: options.AlignClauseBodies),
                                        AlignColumnDefinitionFields = GetValue(rule: rules, property: "align_column_definition_fields", defaultValue: options.AlignColumnDefinitionFields),
                                        AllowExternalLanguagePaths = GetValue(rule: rules, property: "allow_external_language_paths", defaultValue: options.AllowExternalLanguagePaths),
                                        AlignSetClauseItem = GetValue(rule: rules, property: "align_set_clause_item", defaultValue: options.AlignSetClauseItem),
                                        AllowExternalLibraryPaths = GetValue(rule: rules, property: "allow_external_library_paths", defaultValue: options.AllowExternalLibraryPaths),
                                        AsKeywordOnOwnLine = GetValue(rule: rules, property: "as_keyword_on_own_line", defaultValue: options.AsKeywordOnOwnLine),
                                        IncludeSemicolons = GetValue(rule: rules, property: "include_semicolons", defaultValue: options.IncludeSemicolons),
                                        IndentSetClause = GetValue(rule: rules, property: "indent_set_clause", defaultValue: options.IndentSetClause),
                                        KeywordCasing = GetValue(rule: rules, property: "keyword_casing", defaultValue: options.KeywordCasing),
                                        IndentationSize = GetValue(rule: rules, property: "indentation_size", defaultValue: options.IndentationSize),
                                        IndentViewBody = GetValue(rule: rules, property: "indent_view_body", defaultValue: options.IndentViewBody),
                                        MultilineInsertSourcesList = GetValue(rule: rules, property: "multiline_insert_sources_list", defaultValue: options.MultilineInsertSourcesList),
                                        MultilineInsertTargetsList = GetValue(rule: rules, property: "multiline_insert_targets_list", defaultValue: options.MultilineInsertTargetsList),
                                        MultilineSelectElementsList = GetValue(rule: rules, property: "multiline_select_elements_list", defaultValue: options.MultilineSelectElementsList),
                                        MultilineSetClauseItems = GetValue(rule: rules, property: "multiline_set_clause_items", defaultValue: options.MultilineSetClauseItems),
                                        MultilineViewColumnsList = GetValue(rule: rules, property: "multiline_view_columns_list", defaultValue: options.MultilineViewColumnsList),
                                        MultilineWherePredicatesList = GetValue(rule: rules, property: "multiline_where_predicates_list", defaultValue: options.MultilineWherePredicatesList),
                                        NewLineBeforeCloseParenthesisInMultilineList =
                                            GetValue(rule: rules, property: "new_line_before_close_parenthesis_in_multiline_list", defaultValue: options.NewLineBeforeCloseParenthesisInMultilineList),
                                        NewLineBeforeFromClause = GetValue(rule: rules, property: "new_line_before_from_clause", defaultValue: options.NewLineBeforeFromClause),
                                        NewLineBeforeGroupByClause = GetValue(rule: rules, property: "new_line_before_group_by_clause", defaultValue: options.NewLineBeforeGroupByClause),
                                        NewLineBeforeHavingClause = GetValue(rule: rules, property: "new_line_before_having_clause", defaultValue: options.NewLineBeforeGroupByClause),
                                        NewLineBeforeJoinClause = GetValue(rule: rules, property: "new_line_before_join_clause", defaultValue: options.NewLineBeforeJoinClause),
                                        NewLineBeforeOffsetClause = GetValue(rule: rules, property: "new_line_before_offset_clause", defaultValue: options.NewLineBeforeOffsetClause),
                                        NewLineBeforeOpenParenthesisInMultilineList =
                                            GetValue(rule: rules, property: "new_line_before_open_parenthesis_in_multiline_list", defaultValue: options.NewLineBeforeOpenParenthesisInMultilineList),
                                        NewLineBeforeOrderByClause = GetValue(rule: rules, property: "new_line_before_order_by_clause", defaultValue: options.NewLineBeforeOrderByClause),
                                        NewLineBeforeOutputClause = GetValue(rule: rules, property: "new_line_before_output_clause", defaultValue: options.NewLineBeforeOutputClause),
                                        NewLineBeforeWhereClause = GetValue(rule: rules, property: "new_line_before_where_clause", defaultValue: options.NewLineBeforeWhereClause),
                                        NewLineBeforeWindowClause = GetValue(rule: rules, property: "new_line_before_window_clause", defaultValue: options.NewLineBeforeWindowClause),
                                        NewlineFormattedCheckConstraint = GetValue(rule: rules, property: "newline_formatted_check_constraint", defaultValue: options.NewlineFormattedCheckConstraint),
                                        NewLineFormattedIndexDefinition = GetValue(rule: rules, property: "newline_formatted_index_definition", defaultValue: options.NewLineFormattedIndexDefinition),
                                        NumNewlinesAfterStatement = GetValue(rule: rules, property: "num_newlines_after_statement", defaultValue: options.NumNewlinesAfterStatement),
                                        SpaceBetweenDataTypeAndParameters =
                                            GetValue(rule: rules, property: "space_between_data_type_and_parameters", defaultValue: options.SpaceBetweenDataTypeAndParameters),
                                        SpaceBetweenParametersInDataType =
                                            GetValue(rule: rules, property: "space_between_parameters_in_data_type", defaultValue: options.SpaceBetweenParametersInDataType),
                                        SqlEngineType = GetValue(rule: rules, property: "sql_engine_type", defaultValue: options.SqlEngineType),
                                        SqlVersion = GetValue(rule: rules, property: "sql_version", defaultValue: options.SqlVersion)
                                    });
    }

    private static T GetValue<T>(FileConfiguration rule, string property, T defaultValue)
        where T : struct, Enum
    {
        if (!rule.Properties.TryGetValue(key: property, out string? value))
        {
            return defaultValue;
        }

        return Enum.TryParse(value: value, ignoreCase: true, out T casing)
            ? casing
            : defaultValue;
    }

    private static bool GetValue(FileConfiguration rule, string property, bool defaultValue)
    {
        if (!rule.Properties.TryGetValue(key: property, out string? value))
        {
            return defaultValue;
        }

        if (StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "true") || StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "yes") || StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "1"))
        {
            return true;
        }

        if (StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "false") || StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "no") || StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "0"))
        {
            return false;
        }

        return defaultValue;
    }

    private static int GetValue(FileConfiguration rule, string property, int defaultValue)
    {
        if (!rule.Properties.TryGetValue(key: property, out string? value))
        {
            return defaultValue;
        }

        return int.TryParse(s: value, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, out int result)
            ? result
            : defaultValue;
    }
}