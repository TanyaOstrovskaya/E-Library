using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace MyIniFile
{
    public interface IIniFileContentLineHandler
    {
        void HandleNoDataLine(IniFileNoMeaningContentLine iniFileNoMeaningContentLine);

        void HandleSectionDeclaration(IniFileSectionDeclarationLine iniFileSectionDeclarationLine);

        void HandleKeyValue(IniFileKeyValueLine iniFileKeyValueLine);
    }

    public abstract class IniFileLine   //описание строк файла (комментарий/секция/k-v/пустая)
    {
        public string CommentText { get; private set; }

        protected IniFileLine(string commentText)
        {
            this.CommentText = commentText;
        }

        public void HandleWith(IIniFileContentLineHandler handler)
        {
            this.HandleWithImpl(handler);
        }

        protected abstract void HandleWithImpl(IIniFileContentLineHandler handler);

        protected string FormatComment(bool addLeadingIndent)
        {
            return this.CommentText == null ? string.Empty : string.Format("{0};{1}", addLeadingIndent ? "\t\t" : string.Empty, this.CommentText);
        }

        public abstract override string ToString();

        public static bool TryParse(string line, out IniFileLine record)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                record = IniFileNoMeaningContentLine.EmptyLine;
            }
            else
            {
                var match = Regex.Match(line, @"^((\[(?<sectionName>[^\]]*)\])|((?<key>[^=]*)=(?<value>[^\;]*)))(\;(?<commentText>[^\n]*))?$");
                if (match.Success)
                {
                    var sectionNameGroup = match.Groups["sectionName"];
                    var keyGroup = match.Groups["key"];
                    var valueGroup = match.Groups["value"];
                    var commentTextGroup = match.Groups["commentText"];

                    var commentText = commentTextGroup.Success ? commentTextGroup.Value : null;
                    if (sectionNameGroup.Success)
                    {
                        record = new IniFileSectionDeclarationLine(sectionNameGroup.Value.Trim(), commentText);
                    }
                    else if (keyGroup.Success && valueGroup.Success)
                    {
                        record = new IniFileKeyValueLine(keyGroup.Value.Trim(), valueGroup.Value.Trim(), commentText);
                    }
                    else
                    {
                        record = null;
                    }
                }
                else
                {
                    record = null;
                }
            }

            return record != null;
        }
    }

    public class IniFileNoMeaningContentLine : IniFileLine
    {
        public static readonly IniFileNoMeaningContentLine EmptyLine = new IniFileNoMeaningContentLine(null);

        public IniFileNoMeaningContentLine(string comment)
            : base(comment)
        { }

        protected override void HandleWithImpl(IIniFileContentLineHandler handler)
        {
            handler.HandleNoDataLine(this);
        }

        public override string ToString()
        {
            return base.FormatComment(false);
        }
    }

    public class IniFileSectionDeclarationLine : IniFileLine
    {
        public string SectionName { get; private set; }

        public IniFileSectionDeclarationLine(string sectionName, string comment)
            : base(comment)
        {
            this.SectionName = sectionName;
        }

        protected override void HandleWithImpl(IIniFileContentLineHandler handler)
        {
            handler.HandleSectionDeclaration(this);
        }

        public override string ToString()
        {
            return string.Format("[{0}]{1}", this.SectionName, this.FormatComment(true));
        }
    }

    public class IniFileKeyValueLine : IniFileLine
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public IniFileKeyValueLine(string key, string value, string comment)
            : base(comment)
        {
            this.Key = key;
            this.Value = value;
        }

        protected override void HandleWithImpl(IIniFileContentLineHandler handler)
        {
            handler.HandleKeyValue(this);
        }

        public override string ToString()
        {
            return string.Format("{0}={1}{2}", this.Key, this.Value, this.FormatComment(true));
        }

        public string ToStringWithoutComment()
        {
            return string.Format("{0}={1}", this.Key, this.Value);
        }
    }

    public class IniFileContent : IEnumerable<IniFileLine>
    {
        public string SourceName { get; private set; }

        readonly List<IniFileLine> _lines;

        public IniFileContent(string sourceName = null)
        {
            this.SourceName = sourceName;
            _lines = new List<IniFileLine>();
        }

        private IniFileContent(IEnumerable<IniFileLine> lines, string sourceName)
        {
            this.SourceName = sourceName;
            _lines = lines.ToList();
        }

        public void AddSection(string sectionName, string comment = null)
        {
            _lines.Add(new IniFileSectionDeclarationLine(sectionName, comment));
        }

        public void AddKeyValue(string key, string value, string comment = null)
        {
            _lines.Add(new IniFileKeyValueLine(key, value, comment));
        }

        public void SaveToFile(string filePath)
        {
            File.WriteAllLines(filePath, _lines.Select(l => l.ToString()).ToArray());
        }

        public void SaveToSourceFile()
        {
            if (this.SourceName == null)
                throw new ApplicationException("Source name is empty!");

            SaveToFile(this.SourceName);
        }

        public IniFileContent ReadFromFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return new IniFileContent(ReadContentRecords(reader), fileName);
            }
        }

        private static IEnumerable<IniFileLine> ReadContentRecords(TextReader reader)
        {
            var line = reader.ReadLine();
            var lineNum = 1;

            while (line != null)
            {
                IniFileLine record;
                if (!IniFileLine.TryParse(line, out record))
                    throw new ApplicationException("Invalid line #" + lineNum);

                yield return record;

                line = reader.ReadLine();
                lineNum++;
            }
        }

        public IEnumerator<IniFileLine> GetEnumerator()
        {
            return ((IEnumerable<IniFileLine>)_lines).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IniFileLine>)_lines).GetEnumerator();
        }
    }
}
