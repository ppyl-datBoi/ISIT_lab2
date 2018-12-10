using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace CLIPS_rus_edition
{
    /// <summary>
    /// Интерфейс базы знаний
    /// </summary>
    public interface IKnowledgeBase
    {
        /// <summary>
        /// список правил
        /// </summary>
        List<IRule> RulesList { get; set; }
        /// <summary>
        /// загрузка файла и заполнение БЗ
        /// </summary>
        /// <param name="path"></param>
        void Load(string path);
        /// <summary>
        /// очистка БЗ
        /// </summary>
        void Clear();

    }

    /// <summary>
    /// интерфейс правила
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Название правила
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Предпосылки правила
        /// </summary>
        IPreconditions Preconditions { get; set; }
        /// <summary>
        /// Содержит статус использования правила, true если использовано, иначе false
        /// </summary>
        bool is_used { get; set; }
        /// <summary>
        /// Содержит вопрос для определения факта, если вопроса нет содержит значение null 
        /// </summary>
        string question { get; set; }
        /// <summary>
        /// Допустимые значения фактов при применении правила
        /// </summary>
        Dictionary<string, string> Facts { get; set; }
    }

    /// <summary>
    /// Интерфейс предпосылок правила
    /// </summary>
    public interface IPreconditions
    {
        /// <summary>
        /// Список левых переменных для выражения PrecLeft && PrectRight
        /// </summary>
        List<string> PrecLeft { get; set; }

        /// <summary>
        /// Список правых переменных для выражения PrecLeft && PrectRight
        /// </summary>
        List<string> PrecRight { get; set; }

        /// <summary>
        /// Список: факт = значение
        /// </summary>
        Dictionary<string, string> preconditions { get; set; }

        /// <summary>
        /// Добавление предпосылки
        /// </summary>
        /// <param name="name">название параметра</param>
        /// <param name="value">значение параметра</param>
        void Add(string name, string value);
        /// <summary>
        /// добавление условия предпосылки, в виде предпосылка1 AND предпосылка2
        /// </summary>
        /// <param name="namel">предпосылка1</param>
        /// <param name="namer">предпосылка2</param>
        void AddCondition(string namel, string namer);

    }


    /// <summary>
    /// Представляет базу знаний ИС
    /// </summary>
    public class KnowledgeBase : IKnowledgeBase
    {
        /// <summary>
        /// список правил
        /// </summary>
        public List<IRule> RulesList { get; set; }
        /// <summary>
        /// загрузка файла и заполнение БЗ
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            #region Создание объектов для начала работы с БЗ
            RulesList = new List<IRule>();//создаем новый список правил
            #endregion
            #region запуск Word и открытие файла
            Word.Application app = new Word.Application();//запускаем Ворд
            Object fileName = path;//задаем путь к файлу
            app.Documents.Open(ref fileName);//открываем файл
            Word.Document doc = app.ActiveDocument;//запоминаем окрытый документ
            #endregion

            #region Заполнение базы знаний из файла .doc
            for (int i = 1; i <doc.Paragraphs.Count; i++)
            {
                IRule rule = new Rule();//создаем первое правило
                rule.Preconditions = new Preconditions();//Обект предпосылок
                rule.Preconditions.preconditions = new Dictionary<string, string>();//предпосылки для правила

                string constrains = doc.Paragraphs[i].Range.Text;

                switch(constrains)
                {
                    case ("Правило"):
                        {
                            int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                            int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                            int length = finishIndrx - startIndex;
                            rule.Name = (doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                            break;
                        }


                    case ("IF"):
                        {
                            int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                            int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                            int length = finishIndrx - startIndex;
                            int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                            int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                            int lengthV = finishIndrxV - startIndexV - 3;
                            string name = doc.Paragraphs[i].Range.Text.Substring(startIndex, length);
                            string value = doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV)
                            rule.Preconditions.preconditions.Add(name, value);
                            break;
                        }

                    case ("AND"):
                        {
                            while (doc.Paragraphs[i].Range.Text.Contains("AND"))
                            {
                                int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                                int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                                int length = finishIndrx - startIndex;
                                int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                                int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                                int lengthV = finishIndrxV - startIndexV - 3;
                                rule.Preconditions.preconditions.AddCondition(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                            }
                            break;
                        }

                        //я
                        //остановился
                        //здесь
                }
                

                if (rule.Name != null)//если у правила есть имя, записываем его
                    RulesList.Add(rule);
            }
            #endregion
        }

        private void DDD()
        {

        }

        /// <summary>
        /// очистка БЗ
        /// </summary>
        public void Clear()
        {

        }

    }

    /// <summary>
    /// Представляет продукционное правило состоящее из: название, предпосылки, вопросы, значения
    /// </summary>
    public class Rule : IRule
    {
        /// <summary>
        /// Название правила
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Предпосылки правила
        /// </summary>
        public IPreconditions Preconditions { get; set; }
        /// <summary>
        /// Содержит статус использования правила, true если использовано, иначе false
        /// </summary>
        public bool is_used { get; set; }
        /// <summary>
        /// Содержит вопрос для определения факта, если вопроса нет содержит значение null 
        /// </summary>
        public string question { get; set; }
        /// <summary>
        /// Допустимые значения фактов при применении правила
        /// </summary>
        public Dictionary<string, string> Facts { get; set; }
    }

    public class Preconditions : IPreconditions
    {

        /// <summary>
        /// Список левых переменных для выражения PrecLeft && PrectRight
        /// </summary>
        public List<string> PrecLeft { get; set; }

        /// <summary>
        /// Список правых переменных для выражения PrecLeft && PrectRight
        /// </summary>
        public List<string> PrecRight { get; set; }

        /// <summary>
        /// Список: факт = значение
        /// </summary>
        public Dictionary<string, string> preconditions { get; set; }

        /// <summary>
        /// Добавление предпосылки
        /// </summary>
        /// <param name="name">название параметра</param>
        /// <param name="value">значение параметра</param>
        public void Add(string name, string value)
        {

        }

        /// <summary>
        /// добавление условия предпосылки, в виде предпосылка1 AND предпосылка2
        /// </summary>
        /// <param name="namel">предпосылка1</param>
        /// <param name="namer">предпосылка2</param>
        public void AddCondition(string namel, string namer)
        {

        }

    }
}
