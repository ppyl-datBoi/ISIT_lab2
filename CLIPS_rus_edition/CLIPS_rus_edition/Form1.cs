using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace CLIPS_rus_edition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox2.Text = "Добро пожаловать в диагностирующую систему";
        }

        

        public void parse_factss()
        {
            const string docPath = @"d:\test.doc";
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Open(docPath);

            //Get all words
            string allWords = doc.Content.Text;
            doc.Close();
            app.Quit();

            label1.Text = allWords;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parser p = new parser();
            p.parse_facts(label1, richTextBox1);
            WorkingMemory wm = new WorkingMemory();


            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            WorkingMemory wm = new WorkingMemory();
            foreach (KeyValuePair<String,String> s in wm.facts)
            {
                label2.Text = label2.Text + s.Key + s.Value;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            WorkingMemory wm = new WorkingMemory();
            wm.facts["интернет работает"] = "да";
         
            foreach (KeyValuePair<String, String> s in wm.facts)
            {
                label2.Text = label2.Text + s.Key + s.Value;
                wm.facts["интернет работает"] = "да";
            }
        }
    }
}
