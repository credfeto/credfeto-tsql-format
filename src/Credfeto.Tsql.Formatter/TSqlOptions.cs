using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Credfeto.Tsql.Formatter;

public static class TSqlOptions
{
    public static SqlScriptGeneratorOptions DefaultOptions =>
        new()
        {
            AlignClauseBodies = true,
            AlignColumnDefinitionFields = true,
            AllowExternalLanguagePaths = true,
            AlignSetClauseItem = true,
            AllowExternalLibraryPaths = true,
            AsKeywordOnOwnLine = true,
            IncludeSemicolons = true,
            IndentSetClause = false,
            KeywordCasing = KeywordCasing.Uppercase,
            IndentationSize = 4,
            IndentViewBody = false,
            MultilineInsertSourcesList = true,
            MultilineInsertTargetsList = true,
            MultilineSelectElementsList = true,
            MultilineSetClauseItems = true,
            MultilineViewColumnsList = true,
            MultilineWherePredicatesList = true,
            NewLineBeforeCloseParenthesisInMultilineList = true,
            NewLineBeforeFromClause = true,
            NewLineBeforeGroupByClause = true,
            NewLineBeforeHavingClause = true,
            NewLineBeforeJoinClause = true,
            NewLineBeforeOffsetClause = true,
            NewLineBeforeOpenParenthesisInMultilineList = true,
            NewLineBeforeOrderByClause = true,
            NewLineBeforeOutputClause = true,
            NewLineBeforeWhereClause = true,
            NewLineBeforeWindowClause = true,
            NewlineFormattedCheckConstraint = true,
            NewLineFormattedIndexDefinition = true,
            NumNewlinesAfterStatement = 1,
            SpaceBetweenDataTypeAndParameters = true,
            SpaceBetweenParametersInDataType = true,
            SqlEngineType = SqlEngineType.All,
            SqlVersion = SqlVersion.Sql170
        };
}