using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyIniFile
{
    class IniFileSectionData
    {
        public string Name { get; private set; }

        readonly Dictionary<string, string> _pairs = new Dictionary<string, string>();

        public string this[string keyName] { get { return _pairs[keyName]; } }

       
        public IniFileSectionData(string name)
        {
            this.Name = name;            
        }

        public void Add(string key, string value)
        {
            _pairs.Add(key, value);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _pairs.TryGetValue(key, out value);
        }

        public void ToContent(IniFileContent content)
        {
            foreach (var kv in _pairs)
            {
                content.AddKeyValue(kv.Key, kv.Value);
            }
        }
    }

    class IniFileData : IEnumerable<IniFileSectionData>
    {
        class ContentLinesHandler : IIniFileContentLineHandler
        {
            readonly IniFileData _owner;

            IniFileSectionData _currSection = null;

            private ContentLinesHandler(IniFileData owner)
            {
                _owner = owner;
            }

            #region IIniFileContentLineHandler implementation

            void IIniFileContentLineHandler.HandleNoDataLine(IniFileNoMeaningContentLine emptyLine)
            {
                // do nothing
            }

            void IIniFileContentLineHandler.HandleSectionDeclaration(IniFileSectionDeclarationLine sectionLine)
            {
                var sectionName = sectionLine.SectionName;

                if (!_owner._sections.TryGetValue(sectionName, out _currSection))
                {
                    _currSection = new IniFileSectionData(sectionName);
                    _owner._sections.Add(sectionName, _currSection);
                }
            }

            void IIniFileContentLineHandler.HandleKeyValue(IniFileKeyValueLine kvLine)
            {
                _currSection.Add(kvLine.Key, kvLine.Value);
            }

            #endregion

            public static void CollectLinesInfo(IniFileContent rawContent, IniFileData dataCollection)
            {
                var handler = new ContentLinesHandler(dataCollection);

                foreach (var item in rawContent)
                    item.HandleWith(handler);
            }
        }

        readonly Dictionary<string, IniFileSectionData> _sections = new Dictionary<string, IniFileSectionData>();

        public IniFileSectionData this[string sectionName] { get { return _sections[sectionName]; } }

        public IniFileData()
        {
        }

        public void Add(IniFileSectionData section)
        {
            _sections.Add(section.Name, section);
        }

        public void ToContent(IniFileContent content)
        {
            foreach (var section in _sections.Values)
            {
                content.AddSection(section.Name);
                section.ToContent(content);
            }
        }

        public void CollectFrom(IniFileContent content)
        {
            ContentLinesHandler.CollectLinesInfo(content, this);
        }

        #region IEnumerable<IniFileSectionData> implementation

        public IEnumerator<IniFileSectionData> GetEnumerator()
        {
            return _sections.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
