using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExercíciosDeFixação2
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source = OSA0625215W10 - 1; Initial Catalog = ExercicioFixação; Integrated Security = True");
        SqlCommand comando = new SqlCommand();
        SqlDataReader dr;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comando.Connection = conn;
            CarregarLista();
        }
        private void CarregarLista()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            conn.Open();
            comando.CommandText = "select * from Exercicio2";
            dr = comando.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listBox1.Items.Add(dr[1].ToString());
                    listBox2.Items.Add(dr[2].ToString());
                    listBox3.Items.Add(dr[3].ToString());
                    listBox4.Items.Add(dr[4].ToString());
                }
            }
            conn.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l.SelectedIndex != -1)
            {
                listBox1.SelectedIndex = l.SelectedIndex;
                listBox2.SelectedIndex = l.SelectedIndex;
                listBox3.SelectedIndex = l.SelectedIndex;
                listBox4.SelectedIndex = l.SelectedIndex;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l.SelectedIndex != -1)
            {
                listBox1.SelectedIndex = l.SelectedIndex;
                listBox2.SelectedIndex = l.SelectedIndex;
                listBox3.SelectedIndex = l.SelectedIndex;
                listBox4.SelectedIndex = l.SelectedIndex;
            }
        }

        private void BtnInserir_Click(object sender, EventArgs e)
        {
            if (BtnInserir.Text == "Inserir Dados")
            {
                limparControles();
                if (novoArquivo.ShowDialog() != DialogResult.OK)
                    return;

                if (!novoArquivo.FileName.Contains(".csv"))
                    return;
                string caminho = novoArquivo.FileName;
                string textoLido;

                try
                {
                    textoLido = File.ReadAllText(caminho);
                }
                catch (Exception Exc)
                {
                    tbPrincipal.SelectedIndex = 1;
                    lblErro.Text = Exc.Message;
                    return;
                }

                int i = 0;
                lboPessoas.Items.Clear();
                foreach (var linha in textoLido.Split('\n'))
                {
                    if (linha == "" || linha == "\r") break;
                    if (i != 0)
                    {
                        string[] tratamento = linha.Split(';');
                        Pessoa ps = new Pessoa(tratamento[0], tratamento[1], DateTime.Parse(tratamento[2]), tratamento[3], tratamento[4]);
                        lboPessoas.Items.Add(ps);
                    }
                    i++;
                }
                BtnInserir.Text = "Salvar Arquivo";
            }

            else
            {
                string txt = "Nome,Sobrenome,Data de Nascimento;Telefone;Email\n";
                foreach (Pessoa pessoa in lboPessoas.Items)
                {
                    txt += pessoa.Nome + ";";
                    txt += pessoa.Sobrenome + ";";
                    txt += pessoa.DataNascimento + ";";
                    txt += pessoa.Telefone + ";";
                    txt += pessoa.Email + "\n";
                }
                txt = txt.Substring(0, txt.Length - 5);
                salvarNovoArquivo.Filter = "Arquivo CSV|*.csv";
                salvarNovoArquivo.Title = "Salvar Arquivo";
                if (salvarNovoArquivo.ShowDialog() != DialogResult.OK && salvarNovoArquivo.FileName == null) return;
                if (salvarNovoArquivo.ShowDialog() != DialogResult.Cancel) return;

                try
                {
                    FileStream abrirArquivo = (FileStream)salvarNovoArquivo.OpenFile();
                    StreamWriter salvandoArquivo = new StreamWriter(abrirArquivo);

                    salvandoArquivo.WriteLine(txt);
                    salvandoArquivo.Close();
                    abrirArquivo.Close();
                    BtnInserir.Text = "Inserir Dados";
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
    }
}
