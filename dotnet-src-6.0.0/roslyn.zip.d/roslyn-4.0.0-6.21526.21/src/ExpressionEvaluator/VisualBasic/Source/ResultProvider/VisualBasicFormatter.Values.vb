' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Collections.ObjectModel
Imports System.Text
Imports Microsoft.CodeAnalysis.ExpressionEvaluator
Imports Microsoft.CodeAnalysis.PooledObjects
Imports Microsoft.VisualStudio.Debugger.Clr
Imports Roslyn.Utilities
Imports Type = Microsoft.VisualStudio.Debugger.Metadata.Type

Namespace Microsoft.CodeAnalysis.VisualBasic.ExpressionEvaluator

    ' Implementation for the "displaying type names as strings" aspect of the Visual Basic Formatter component.
    Partial Friend NotInheritable Class VisualBasicFormatter

        Private Shared Sub AppendEnumValue(builder As StringBuilder, value As Object, options As ObjectDisplayOptions)
            builder.Append(" {")
            builder.Append(ObjectDisplay.FormatPrimitive(value, options))
            builder.Append("}"c)
        End Sub

        Private Sub AppendEnumTypeAndName(builder As StringBuilder, typeToDisplayOpt As Type, name As String)
            If typeToDisplayOpt IsNot Nothing Then
                Dim index1 As Integer = 0
                Dim index2 As Integer = 0
                AppendQualifiedTypeName(
                    builder,
                    typeToDisplayOpt,
                    Nothing,
                    index1,
                    Nothing,
                    index2,
                    escapeKeywordIdentifiers:=True,
                    sawInvalidIdentifier:=Nothing)
                builder.Append("."c)
            End If

            builder.Append(name)
        End Sub

        Friend Overrides Function GetArrayDisplayString(appDomain As DkmClrAppDomain, lmrType As Type, sizes As ReadOnlyCollection(Of Integer), lowerBounds As ReadOnlyCollection(Of Integer), options As ObjectDisplayOptions) As String
            Debug.Assert(lmrType.IsArray)

            ' Strip off additional array types (jagged arrays).  We'll only show the size of the "first" array.
            While (lmrType.IsArray)
                lmrType = lmrType.GetElementType()
            End While

            Dim pooled = PooledStringBuilder.GetInstance()
            Dim builder As StringBuilder = pooled.Builder

            builder.Append("{Length=")

            Debug.Assert(sizes.Count > 0)

            Dim length As Integer = sizes(0)
            For i = 1 To sizes.Count - 1
                length *= sizes(i)
            Next

            builder.Append(ObjectDisplay.FormatLiteral(length, ObjectDisplayOptions.None))

            builder.Append("}"c)

            Return pooled.ToStringAndFree()
        End Function

        Friend Overrides Function GetArrayIndexExpression(indices() As String) As String
            Return indices.ToCommaSeparatedString("("c, ")"c)
        End Function

        Friend Overrides Function GetCastExpression(argument As String, type As String, options As DkmClrCastExpressionOptions) As String
            Debug.Assert(Not String.IsNullOrEmpty(argument))
            Debug.Assert(Not String.IsNullOrEmpty(type))

            Dim pooled = PooledStringBuilder.GetInstance()
            Dim builder = pooled.Builder

            If (options And DkmClrCastExpressionOptions.ParenthesizeArgument) <> 0 Then
                argument = $"({argument})"
            End If

            If (options And DkmClrCastExpressionOptions.ConditionalCast) <> 0 Then
                builder.Append("TryCast(")
            Else
                builder.Append("DirectCast(")
            End If

            builder.Append(argument)
            builder.Append(", ")
            builder.Append(type)
            builder.Append(")"c)

            Return pooled.ToStringAndFree()
        End Function

        Friend Overrides Function GetTupleExpression(values() As String) As String
            Return values.ToCommaSeparatedString("("c, ")"c)
        End Function

        Friend Overrides Function GetNamesForFlagsEnumValue(fields As ArrayBuilder(Of EnumField), value As Object, underlyingValue As ULong, options As ObjectDisplayOptions, typeToDisplayOpt As Type) As String
            Dim usedFields = ArrayBuilder(Of EnumField).GetInstance()
            FillUsedEnumFields(usedFields, fields, underlyingValue)

            If usedFields.Count = 0 Then
                Return Nothing
            End If

            Dim pooled = PooledStringBuilder.GetInstance()
            Dim builder As StringBuilder = pooled.Builder

            For i = usedFields.Count - 1 To 0 Step -1 ' Backwards to list smallest first.
                AppendEnumTypeAndName(builder, typeToDisplayOpt, usedFields(i).Name)

                If i > 0 Then
                    builder.Append(" Or ")
                End If
            Next

            If typeToDisplayOpt Is Nothing Then
                AppendEnumValue(builder, value, options)
            End If

            usedFields.Free()

            Return pooled.ToStringAndFree()
        End Function

        Friend Overrides Function GetNameForEnumValue(fields As ArrayBuilder(Of EnumField), value As Object, underlyingValue As ULong, options As ObjectDisplayOptions, typeToDisplayOpt As Type) As String
            For Each field In fields
                If underlyingValue = field.Value Then ' First match wins (deterministic since sorted).
                    Dim pooled = PooledStringBuilder.GetInstance()
                    Dim builder As StringBuilder = pooled.Builder

                    AppendEnumTypeAndName(builder, typeToDisplayOpt, field.Name)

                    If typeToDisplayOpt Is Nothing Then
                        AppendEnumValue(builder, value, options)
                    End If

                    Return pooled.ToStringAndFree()
                End If
            Next

            Return Nothing
        End Function

        Friend Overrides Function GetObjectCreationExpression(type As String, arguments As String()) As String
            Debug.Assert(Not String.IsNullOrEmpty(type))

            Dim pooled = PooledStringBuilder.GetInstance()
            Dim builder = pooled.Builder

            builder.Append("New ")
            builder.Append(type)
            builder.Append("("c)
            builder.AppendCommaSeparatedList(arguments)
            builder.Append(")"c)

            Return pooled.ToStringAndFree()
        End Function

        Friend Overrides Function FormatLiteral(c As Char, options As ObjectDisplayOptions) As String
            ' Clear the 'IncludeCodePoints' bit, because ObjectDisplay has an Assert to make sure that this flag is not passed on other code paths.
            Return ObjectDisplay.FormatLiteral(c, options And Not ObjectDisplayOptions.IncludeCodePoints)
        End Function

        Friend Overrides Function FormatLiteral(value As Integer, options As ObjectDisplayOptions) As String
            Return ObjectDisplay.FormatLiteral(value, options)
        End Function

        Friend Overrides Function FormatPrimitiveObject(value As Object, options As ObjectDisplayOptions) As String
            Return ObjectDisplay.FormatPrimitive(value, options)
        End Function

        Friend Overloads Overrides Function FormatString(str As String, options As ObjectDisplayOptions) As String
            Return ObjectDisplay.FormatLiteral(str, options)
        End Function

    End Class

End Namespace
