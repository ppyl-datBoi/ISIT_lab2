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
     

        private void button1_Click(object sender, EventArgs e)
        {
            KnowledgeBase kb = new KnowledgeBase();         
            kb.parse_facts(label1, richTextBox1);
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
