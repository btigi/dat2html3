using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;

namespace dat2html3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Input file is expected in this format (though with arbitary linebreaks);
            //0,1,"Stat: AC vs. Damage Type Modifier [0]","AC Modifier","Type","Applies the modifier value specified by the 'AC Modifier' field to the category specified by the 'Type' field.    Known values for 'Type' are:  0   All  1   Crushing  2   Missile  4   Piercing  8   Slashing  16  Base AC setting (sets the targets AC to the value specified by the 'AC Modifier' field. If the targets AC is already 'AC Modifier' or below, this effect will do nothing).    AC is capped to the range [-20, 20]"
            //1,2,"Stat: Attacks Per Round Modifier [1]","Key Modifier","Type","Alters a characters Attacks per Round, by changing the Key by the modifier value specified by the 'Key Value' field, in the style specified by 'Type' field.    Key     Attacks Per Round  0        0  1        1  2        2  3        3  4        4  5        5  6        0.5  7        1.5  8        2.5  9        3.5  10      4.5    Known values for 'Type' are:  0   Cumulative Modifier -> Key = Key + 'Key Modifier' value  1   Flat Value Modifier -> Key = 'Key Modifier' value  2   Percentage Modifier -> Key = (Key * 'Key Modifier' value) / 100  3   Cumulative Modifier -> Same as 0    Note: When this opcode is stacked, the values of the Key Modifier are stacked, not the number of attacks."
            //2,2,"Cure: Sleep [2]","Irrelevant","Irrelevant","Removes the state_sleeping flag from the targeted creature(s).  This effect ignores durations attributed to it.    N.B. Does not remove the state_helpless flag."

            var inputFile = args[0];
            var outputFile = args[1];

            var header = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
<head>
  <title>Opcodes</title>
  <link rel=""stylesheet"" type=""text/css"" href=""../css/general.css"" />
  <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
</head>

<body>
<div id=""nonFooter"">
  <div id=""content"">
    <hr />
    <div id=""buttonboxtop"">
      <div id=""buttonstriptop"">
        <a href=""../index.htm"">Index</a> &nbsp;&bull;&nbsp;
        <a href=""../files/2da/index.htm"">2DA</a> &nbsp;&bull;&nbsp;
        <a href=""index.htm"">Effects</a> &nbsp;&bull;&nbsp;
        <a href=""../file_formats/index.htm"">File Formats</a> &nbsp;&bull;&nbsp;
        <a href=""../scripting/actions/index.htm"">Actions</a> &nbsp;&bull;&nbsp;
        <a href=""../files/ids/index.htm"">Identifiers</a> &nbsp;&bull;&nbsp;
        <a href=""../scripting/triggers/index.htm"">Triggers</a> &nbsp;&bull;&nbsp;
        <a href=""../appendices/index.htm"">Appendices</a> &nbsp;&bull;&nbsp;
        <a href=""../site_info/index.htm"">Site Info</a> &nbsp;&bull;&nbsp;
        <a href=""../site_info/legal.htm"">Legal</a>
      </div>
    </div>
    <hr />
    <div id=""logo"">
      <img src=""../images/iesdp_logo.gif"" alt=""IESDP Logo"" width=""120"" height=""120"" />
    </div>

    <div class=""title_main"">Opcodes</div><br />
    <br />";

            var opcodeText = @"
<div id=""op{0}"" class=""opcode"">
  <span class=""opnumberdata"">#{1} (0x{2:X3})</span>
  <span class=""opnamedata"">{3}</span><br />
  <span class=""parameterheading"">Parameter #1:</span><span class=""parameterdata""> {4}</span><br />
  <span class=""parameterheading"">Parameter #2:</span><span class=""parameterdata""> {5}</span><br />
  <span class=""descriptionheading"">Description:</span><br />

  <div class=""descriptiondata"">{6}</div><br />
  <hr />
  <br />
</div>";

            var footerText = @"
  </div>
</div>
<div id=""footer"">
<hr />
<form method=""get"" action=""http://www.google.com/search"">
  <div style=""margin: 0px; padding: 0px; text-align: center"">
    <input type=""hidden"" name=""ie"" value=""UTF-8"" />
    <input type=""hidden"" name=""oe"" value=""UTF-8"" />
    <input class=""form_text"" type=""text"" name=""q"" size=""30"" maxlength=""255"" value="""" />
    <input class=""form_button"" type=""submit"" name=""btnG"" value=""Search"" />
    <input type=""hidden"" name=""domains"" value=""http://iesdp.gibberlings3.net/"" />
    <input type=""hidden"" name=""sitesearch"" value=""http://iesdp.gibberlings3.net/"" />
  </div>
</form>
<hr />
</div>
</body>
</html>";


            StringBuilder sb = new StringBuilder(header);
            using (var parser = new TextFieldParser(inputFile) { TextFieldType = FieldType.Delimited, Delimiters = new[] { "," } })
            {
                while (!parser.EndOfData)
                {
                    string[] fields;
                    fields = parser.ReadFields();
                    sb.AppendFormat(opcodeText, fields[0], fields[0], fields[0], fields[2], fields[3], fields[4], fields[5].Replace("\n", "<br/>"));
                }
            }

            sb.Append(footerText);

            File.WriteAllText(outputFile, sb.ToString());
        }
    }
}