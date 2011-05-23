using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Markup;
using System.ComponentModel;
namespace CodeBoxControl.Decorations
{

  public static class DecorationSchemes
  {


      #region C#
      public static DecorationScheme CSharp3
      {
          get { DecorationScheme ds = new DecorationScheme();

          ds.Name = "C#";
          MultiRegexWordDecoration BlueWords = new MultiRegexWordDecoration();
          BlueWords.Brush = new SolidColorBrush(Colors.Blue);
          BlueWords.Words = CSharpReservedWords();
          ds.BaseDecorations.Add(BlueWords);

          MultiRegexWordDecoration BlueClasses = new MultiRegexWordDecoration();
          BlueClasses.Brush = new SolidColorBrush(Colors.Blue);
          BlueClasses.Words = CSharpVariableReservations();
          ds.BaseDecorations.Add(BlueClasses);

          MultiStringDecoration regions = new MultiStringDecoration();
          regions.Brush = new SolidColorBrush(Colors.Blue);
          regions.Strings.AddRange(CSharpRegions());
          ds.BaseDecorations.Add(regions);

          RegexDecoration quotedText = new RegexDecoration();
          quotedText.Brush = new SolidColorBrush(Colors.Brown );
          quotedText.RegexString = "(?s:\".*?\")";
          ds.BaseDecorations.Add(quotedText);


          //Color single line comments green
          RegexDecoration singleLineComment = new RegexDecoration();
          singleLineComment.DecorationType = EDecorationType.TextColor;
          singleLineComment.Brush = new SolidColorBrush(Colors.Green);
          singleLineComment.RegexString = "//.*";
          ds.BaseDecorations.Add(singleLineComment);

          //Color multiline comments green
          RegexDecoration multiLineComment = new RegexDecoration();
          multiLineComment.DecorationType = EDecorationType.TextColor;
          multiLineComment.Brush = new SolidColorBrush(Colors.Green);
          multiLineComment.RegexString = @"(?s:/\*.*?\*/)";
          ds.BaseDecorations.Add(multiLineComment);
          
          return ds;
          
          }
      }

      private static List<string> CSharpReservedWords()
      {
          return new List<string>() { "using" ,"namespace" , "static", "class" ,"public" ,"get" , "private" , "return" ,"partial" , "new"
          ,"set" , "value"  };
      }

      private static List<string> CSharpVariableReservations()
      {
          return new List<string>() { "string" , "int" , "double", "long"};
      }

      private static List<string> CSharpRegions()
      {
          return new List<string>() { "#region", "#endregion" };
      }

      #endregion


      #region SQL Server

      public static DecorationScheme SQLServer2008
      {
          get
          {
              DecorationScheme ds = new DecorationScheme();
              ds.Name = "SQL Server";

              // Color Built in functions Magenta
              MultiRegexWordDecoration builtInFunctions = new MultiRegexWordDecoration();
              builtInFunctions.Brush = new SolidColorBrush(Colors.Magenta);
              builtInFunctions.Words.AddRange(GetBuiltInFunctions());
              ds.BaseDecorations.Add(builtInFunctions);

              //Color global variables Magenta
              MultiStringDecoration globals = new MultiStringDecoration();
              globals.Brush = new SolidColorBrush(Colors.Magenta);
              globals.Strings.AddRange(GetGlobalVariables());
              ds.BaseDecorations.Add(globals);

              //Color most reserved words blue
              MultiRegexWordDecoration bluekeyWords = new MultiRegexWordDecoration();
              bluekeyWords.Brush = new SolidColorBrush(Colors.Blue);
              bluekeyWords.Words.AddRange(GetBlueKeyWords());
              ds.BaseDecorations.Add(bluekeyWords);

              MultiRegexWordDecoration grayKeyWords = new MultiRegexWordDecoration();
              grayKeyWords.Brush = new SolidColorBrush(Colors.Gray);
              grayKeyWords.Words.AddRange(GetGrayKeyWords());
              ds.BaseDecorations.Add(grayKeyWords);

              MultiRegexWordDecoration dataTypes = new MultiRegexWordDecoration();
              dataTypes.Brush = new SolidColorBrush(Colors.Blue);
              dataTypes.Words.AddRange(GetDataTypes());
              ds.BaseDecorations.Add(dataTypes);


              MultiRegexWordDecoration systemViews = new MultiRegexWordDecoration();
              systemViews.Brush = new SolidColorBrush(Colors.Green);
              systemViews.Words.AddRange(GetSystemViews());
              ds.BaseDecorations.Add(systemViews);

              MultiStringDecoration operators = new MultiStringDecoration();
              operators.Brush = new SolidColorBrush(Colors.Gray);
              operators.Strings.AddRange(GetOperators());
              ds.BaseDecorations.Add(operators);


              RegexDecoration quotedText = new RegexDecoration();
              quotedText.Brush = new SolidColorBrush(Colors.Red);
              quotedText.RegexString = "'.*?'";
              ds.BaseDecorations.Add(quotedText);

              RegexDecoration nQuote = new RegexDecoration();
              //nQuote.DecorationType = EDecorationType.TextColor;
              nQuote.Brush = new SolidColorBrush(Colors.Red);
              nQuote.RegexString = "N''";
              ds.BaseDecorations.Add(nQuote);


              //Color single line comments green
              RegexDecoration singleLineComment = new RegexDecoration();
              singleLineComment.DecorationType = EDecorationType.TextColor;
              singleLineComment.Brush = new SolidColorBrush(Colors.Green);
              singleLineComment.RegexString = "--.*";
              ds.BaseDecorations.Add(singleLineComment);

              //Color multiline comments green
              RegexDecoration multiLineComment = new RegexDecoration();
              multiLineComment.DecorationType = EDecorationType.Strikethrough;
              multiLineComment.Brush = new SolidColorBrush(Colors.Green);
              multiLineComment.RegexString = @"(?s:/\*.*?\*/)";
              ds.BaseDecorations.Add(multiLineComment);
              return  ds;
          }
      }



     static  string[] GetBuiltInFunctions()
      {
          string[] funct = { "parsename", "db_name", "object_id", "count", "ColumnProperty", "LEN",
                             "CHARINDEX" ,"isnull" , "SUBSTRING" };
          return funct;

      }

     static string[] GetGlobalVariables()
      {

          string[] globals = { "@@fetch_status" };
          return globals;

      }

     static string[] GetDataTypes()
      {
          string[] dt = { "int", "sysname", "nvarchar", "char" };
          return dt;

      }


     static string[] GetBlueKeyWords() // List from 
      {
          string[] res = {"ADD","EXISTS","PRECISION","ALL","EXIT","PRIMARY","ALTER","EXTERNAL",
                            "PRINT","FETCH","PROC","ANY","FILE","PROCEDURE","AS","FILLFACTOR",
                            "PUBLIC","ASC","FOR","RAISERROR","AUTHORIZATION","FOREIGN","READ","BACKUP",
                            "FREETEXT","READTEXT","BEGIN","FREETEXTTABLE","RECONFIGURE","BETWEEN","FROM",
                            "REFERENCES","BREAK","FULL","REPLICATION","BROWSE","FUNCTION","RESTORE",
                            "BULK","GOTO","RESTRICT","BY","GRANT","RETURN","CASCADE","GROUP","REVERT",
                            "CASE","HAVING","REVOKE","CHECK","HOLDLOCK","RIGHT","CHECKPOINT","IDENTITY",
                            "ROLLBACK","CLOSE","IDENTITY_INSERT","ROWCOUNT","CLUSTERED","IDENTITYCOL",
                            "ROWGUIDCOL","COALESCE","IF","RULE","COLLATE","IN","SAVE","COLUMN","INDEX",
                            "SCHEMA","COMMIT","INNER","SECURITYAUDIT","COMPUTE","INSERT","SELECT",
                            "CONSTRAINT","INTERSECT","SESSION_USER","CONTAINS","INTO","SET","CONTAINSTABLE",
                            "SETUSER","CONTINUE","JOIN","SHUTDOWN","CONVERT","KEY","SOME","CREATE",
                            "KILL","STATISTICS","CROSS","LEFT","SYSTEM_USER","CURRENT","LIKE","TABLE",
                            "CURRENT_DATE","LINENO","TABLESAMPLE","CURRENT_TIME","LOAD","TEXTSIZE",
                            "CURRENT_TIMESTAMP","MERGE","THEN","CURRENT_USER","NATIONAL","TO","CURSOR",
                            "NOCHECK","TOP","DATABASE","NONCLUSTERED","TRAN","DBCC","NOT","TRANSACTION",
                            "DEALLOCATE","NULL","TRIGGER","DECLARE","NULLIF","TRUNCATE","DEFAULT","OF",
                            "TSEQUAL","DELETE","OFF","UNION","DENY","OFFSETS","UNIQUE","DESC", "ON", 
                            "UNPIVOT","DISK","OPEN","UPDATE","DISTINCT","OPENDATASOURCE","UPDATETEXT",
                            "DISTRIBUTED","OPENQUERY","USE","DOUBLE","OPENROWSET","USER","DROP","OPENXML",
                            "VALUES","DUMP","OPTION","VARYING","ELSE","OR","VIEW","END","ORDER","WAITFOR",
                            "ERRLVL","OUTER","WHEN","ESCAPE","OVER","WHERE","EXCEPT","PERCENT","WHILE",
                            "EXEC","PIVOT","WITH","EXECUTE","PLAN","WRITETEXT", "GO", "ANSI_NULLS",
                            "NOCOUNT", "QUOTED_IDENTIFIER", "master"};

          return res;
      }


     static string[] GetGrayKeyWords()
      {
          string[] res = { "AND", "Null", "IS" };

          return res;

      }

     static string[] GetOperators()
      {
          string[] ops = { "=", "+", ".", ",", "-", "(", ")", "*", "<", ">" };

          return ops;

      }

     static string[] GetSystemViews()
      {
          string[] views = { "syscomments", "sysobjects", "sys.syscomments" };
          return views;
      }

      #endregion


      #region DBML

     public static DecorationScheme Dbml
     {
         get
         {
             DecorationScheme ds = new DecorationScheme();

             ds.Name = "Dbml#";

              MultiRegexWordDecoration BrownWords = new MultiRegexWordDecoration();
              BrownWords.Brush = new SolidColorBrush(Colors.Brown);
              BrownWords.Words = new List<string>() { "xml", "Database", "Table", "Type", "Column", "Association" };
              ds.BaseDecorations.Add(BrownWords);


              MultiRegexWordDecoration RedWords = new MultiRegexWordDecoration();
              RedWords.Brush = new SolidColorBrush(Colors.Red);
              RedWords.Words = new List<string>() { "version", "encoding", "Name", "Class", "xmlns", "Member", "Type", "DbType", "CanBeNull" , "DeleteRule", "IsPrimaryKey"
              ,"IsForeignKey", "ThisKey", "OtherKey", "IsDbGenerated" ,"UpdateCheck" };
              ds.BaseDecorations.Add(RedWords);

              DoubleQuotedDecoration quoted = new DoubleQuotedDecoration();
              quoted.Brush = new SolidColorBrush(Colors.Blue);
              ds.BaseDecorations.Add(quoted);


              MultiStringDecoration blueStrings = new MultiStringDecoration();
              blueStrings.Brush = new SolidColorBrush(Colors.Blue);
              blueStrings.Strings = new List<string>() { "<", "?", "=", "/", ">" };
              ds.BaseDecorations.Add(blueStrings);


              StringDecoration quotationMarks = new StringDecoration();
              quotationMarks.String = "\"";
              quotationMarks.Brush = new SolidColorBrush(Colors.Black);
              ds.BaseDecorations.Add(quotationMarks);



             //RegexDecoration quotedText = new RegexDecoration();
             //quotedText.Brush = new SolidColorBrush(Colors.Brown);
             //quotedText.RegexString = "(?s:\".*?\")";
             //ds.BaseDecorations.Add(quotedText);


             ////Color single line comments green
             //RegexDecoration singleLineComment = new RegexDecoration();
             //singleLineComment.DecorationType = EDecorationType.TextColor;
             //singleLineComment.Brush = new SolidColorBrush(Colors.Green);
             //singleLineComment.RegexString = "//.*";
             //ds.BaseDecorations.Add(singleLineComment);

             ////Color multiline comments green
             //RegexDecoration multiLineComment = new RegexDecoration();
             //multiLineComment.DecorationType = EDecorationType.TextColor;
             //multiLineComment.Brush = new SolidColorBrush(Colors.Green);
             //multiLineComment.RegexString = @"(?s:/\*.*?\*/)";
             //ds.BaseDecorations.Add(multiLineComment);

             return ds;

         }
     }


      #endregion

      #region XML
     public static DecorationScheme Xml
     {
         get
         {
             DecorationScheme ds = new DecorationScheme();
             ds.Name = "XML";

             MultiStringDecoration specialCharacters = new MultiStringDecoration();
             specialCharacters.Brush = new SolidColorBrush(Colors.Blue);
             specialCharacters.Strings = new List<string>() { "<", "/", ">", "<?", "?>", "=" };
             ds.BaseDecorations.Add(specialCharacters);

             RegexMatchDecoration xmlTagName = new RegexMatchDecoration();
             xmlTagName.Brush = new SolidColorBrush(Colors.Brown);
             xmlTagName.RegexString = @"</?(?<selected>\w.*?)(\s|>|/)";
             ds.BaseDecorations.Add(xmlTagName);

             RegexMatchDecoration xmlAttributeName = new RegexMatchDecoration();
             xmlAttributeName.Brush = new SolidColorBrush(Colors.Red);
             xmlAttributeName.RegexString = @"\s(?<selected>\w+|\w+:\w+|(\w|\.)+)="".*?""";
             ds.BaseDecorations.Add(xmlAttributeName);

             RegexMatchDecoration xmlAttributeValue = new RegexMatchDecoration();
             xmlAttributeValue.Brush = new SolidColorBrush(Colors.Blue);
             xmlAttributeValue.RegexString = @"\s(\w+|\w+:\w+|(\w|\.)+)\s*=\s*""(?<selected>.*?)""";
             ds.BaseDecorations.Add(xmlAttributeValue);

             RegexMatchDecoration xml = new RegexMatchDecoration();
             xml.Brush = new SolidColorBrush(Colors.Brown);
             xml.RegexString = @"<\?(?<selected>xml)";
             ds.BaseDecorations.Add(xml);


             return ds;
         }

     }

      #endregion

      #region XAML

     public static DecorationScheme Xaml
     {
         get
         {
             DecorationScheme ds = new DecorationScheme();
             ds.Name = "XAML";
             MultiStringDecoration specialCharacters = new MultiStringDecoration();
             specialCharacters.Brush = new SolidColorBrush(Colors.Blue);
             specialCharacters.Strings = new List<string>() { "<", "/", ">", "<?", "?>" };
             ds.BaseDecorations.Add(specialCharacters);

             RegexMatchDecoration xmlTagName = new RegexMatchDecoration();
             xmlTagName.Brush = new SolidColorBrush(Colors.Brown);
             xmlTagName.RegexString = @"</?(?<selected>\w.*?)(\s|>|/)";
             ds.BaseDecorations.Add(xmlTagName);



             RegexMatchDecoration xmlAttributeName = new RegexMatchDecoration();
             xmlAttributeName.Brush = new SolidColorBrush(Colors.Red);
             xmlAttributeName.RegexString = @"\s(?<selected>\w+|\w+:\w+|(\w|\.)+)="".*?""";
             ds.BaseDecorations.Add(xmlAttributeName);

             RegexMatchDecoration xmlAttributeValue = new RegexMatchDecoration();
             xmlAttributeValue.Brush = new SolidColorBrush(Colors.Blue);
             xmlAttributeValue.RegexString = @"\s(\w+|\w+:\w+|(\w|\.)+)\s*(?<selected>=\s*"".*?"")";
             ds.BaseDecorations.Add(xmlAttributeValue);

             RegexMatchDecoration xamlMarkupExtension = new RegexMatchDecoration();
             xamlMarkupExtension.RegexString = @"\s(\w+|\w+:\w+|(\w|\.)+)\s*=\s*""{(?<selected>.*?)\s+.*?}""";
             xamlMarkupExtension.Brush = new SolidColorBrush(Colors.Brown);
             ds.BaseDecorations.Add(xamlMarkupExtension);

             RegexMatchDecoration xamlMarkupExtensionValue = new RegexMatchDecoration();
             xamlMarkupExtensionValue.RegexString = @"\s(\w+|\w+:\w+|(\w|\.)+)\s*=\s*""{.*?\s+(?<selected>.*?)}""";
             xamlMarkupExtensionValue.Brush = new SolidColorBrush(Colors.Red);
             ds.BaseDecorations.Add(xamlMarkupExtensionValue);

             DoubleRegexDecoration MarkupPeriods = new DoubleRegexDecoration();
             MarkupPeriods.Brush = new SolidColorBrush(Colors.Blue);
             MarkupPeriods.OuterRegexString = @"\s(\w+|\w+:\w+|(\w|\.)+)\s*=\s*""{.*?\s+.*?}""";
             MarkupPeriods.InnerRegexString = @"\.";
             ds.BaseDecorations.Add(MarkupPeriods);

             RegexMatchDecoration xml = new RegexMatchDecoration();
             xml.Brush = new SolidColorBrush(Colors.Brown);
             xml.RegexString = @"<\?(?<selected>xml)";
             ds.BaseDecorations.Add(xml);

             RegexMatchDecoration elementValues = new RegexMatchDecoration();
             elementValues.Brush = new SolidColorBrush(Colors.Brown);
             elementValues.RegexString = @">(?<selected>.*?)</";
             ds.BaseDecorations.Add(elementValues);

             RegexDecoration comment = new RegexDecoration();
             comment.Brush = new SolidColorBrush(Colors.Green);
             comment.RegexString = @"<!--.*?-->";
             ds.BaseDecorations.Add(comment);

             return ds;
         }
     }

      #endregion 


  }
}
