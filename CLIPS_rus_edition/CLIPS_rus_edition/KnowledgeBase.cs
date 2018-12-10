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
        /// Список: факт = значение
        /// </summary>
        Dictionary<string, string> Preconditions { get; set; }
        /// <summary>
        /// Содержит статус использования правила, true если использовано, иначе false
        /// </summary>
        bool is_used { get; set; }
        /// <summary>
        /// Содержит вопрос для определения факта, если вопроса нет содержит значение null 
        /// </summary>
        string question { get; set; }
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

        private IWorkingMemory wm = new WorkingMemory();

        /// <summary>
        /// загрузка файла и заполнение БЗ
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            parse_facts();
        }

        public void parse_facts() //загрузка файла docx
        {
            Word.Application app = new Word.Application();
            string path = AppDomain.CurrentDomain.BaseDirectory + "example.docx";
            Object fileName = path;
            // Object fileName = dialog.FileName;
            app.Documents.Open(ref fileName);
            Word.Document doc = app.ActiveDocument;
            // Нумерация параграфов начинается с одного
            string parText = " ";
            for (int i = 1; i < doc.Paragraphs.Count; i++)
            {
                if (doc.Paragraphs[i].Range.Text.Contains("IF"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));

                }

                if (doc.Paragraphs[i].Range.Text.Contains("AND"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("OR"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("THEN"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("ASK"))//сделать чтобы переменная обрезалась до вопроса
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));                    
                }

                parText = parText + doc.Paragraphs[i].Range.Text;
            }
            app.Quit();



        }

        /*
        public void parse_rules() //импорт правил из документа
        {
            string nameR = "None";
            string nameQ = "None";
            Dictionary<string, string> preconditions = new Dictionary<string, string>();
            Dictionary<string, string> insert = new Dictionary<string, string>();
            string fact_key = "";
            string insert_key = "";
            Dictionary<string, string> question = new Dictionary<string, string>();
            //string question = quest.Values;
            bool flag_then = false;
            bool flag_ask = false;

            Word.Application app = new Word.Application();
            string path = AppDomain.CurrentDomain.BaseDirectory + "example.docx";
            Object fileName = path;
            // Object fileName = dialog.FileName;
            app.Documents.Open(ref fileName);
            Word.Document doc = app.ActiveDocument;
            // Нумерация параграфов начинается с одного
            string parText = " ";
            for (int i = 1; i < doc.Paragraphs.Count; i++)
            {
                if (doc.Paragraphs[i].Range.Text.Contains("Правило"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;
                    nameR = (doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("Вопрос"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;
                    nameQ = (doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("IF"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("AND"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("OR"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length) + "1", doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("THEN"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    insert.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_rule(nameR, preconditions, insert);
                        nameR = "";
                        nameQ = "";
                        preconditions.Clear();
                        insert.Clear();
                    }
                }

                if (doc.Paragraphs[i].Range.Text.Contains("ASK"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;
                    question.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), "none");

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_question(nameQ, preconditions, question);
                        nameQ = "";
                        nameR = "";
                        preconditions.Clear();
                        insert.Clear();
                        question.Clear();
                    }
                }


            }
            app.Quit();
        }
        */

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
        public Dictionary<string, string> Preconditions { get; set; }
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
}
