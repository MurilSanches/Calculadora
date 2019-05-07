using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoED2Calculadora
{
    public partial class Form1 : Form
    {
        private FilaVetor<char> seqInfixa;
        private FilaVetor<char> seqPosfixa;
        private FilaVetor<double> numeros;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLimpa_Click(object sender, EventArgs e)
        {
            txtVisor.Clear();
            txtResultado.Clear();
            lbSequencias.Text = "";
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button botao = (Button)sender;

            if (txtVisor.Text != "")
            {
                if (!Eoperador(txtVisor.Text[txtVisor.Text.Length - 1] + ""))
                {
                    if (txtVisor.Text[txtVisor.Text.Length - 1] == '.')
                    {
                        if (!Eoperador(botao.Text + "") && botao.Text != ")" && botao.Text != "(")
                            txtVisor.Text += botao.Text;
                    }
                    else
                        txtVisor.Text += botao.Text;
                }                        
                else
                {
                    char[] numeros = new char[1000];
                    for (int i = 0; i < txtVisor.Text.Length; i++)
                        numeros[i] = txtVisor.Text[i];

                    if (!Eoperador(botao.Text + ""))
                        numeros[txtVisor.Text.Length] = Convert.ToChar(botao.Text);
                    else
                        numeros[txtVisor.Text.Length - 1] = Convert.ToChar(botao.Text);

                    txtVisor.Clear();

                    for (int i = 0; i < numeros.Length; i++)
                        txtVisor.Text += numeros[i];
                }                
            }
            else
                if (!Eoperador(botao.Text + ""))
                    txtVisor.Text += botao.Text;
        }

        private bool Eoperador(string coisa)
        {
            if (coisa.Equals("/") || coisa.Equals("+") || coisa.Equals("-") ||
                coisa.Equals("^") || coisa.Equals("*"))
                return true;
            else
                return false;
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            resultado();          
        }

        private void resultado()
        {
            lbSequencias.Text = "";
            seqInfixa = new FilaVetor<char>(1000);
            char letra = 'A';
            numeros = new FilaVetor<double>(100);
            string numero = "";
            int num;

            for (int i = 0; i < txtVisor.Text.Length; i++)
            {
                char caracterAtual = (txtVisor.Text)[i];

                if (int.TryParse(caracterAtual.ToString(), out num) || caracterAtual.Equals('.'))
                {
                    if (caracterAtual.Equals('.'))
                        numero += ',';
                    else
                    numero += caracterAtual;
                }
                else if (numero != "")
                {
                    numeros.adiciona(Convert.ToDouble(numero));
                    seqInfixa.adiciona(letra );
                    seqInfixa.adiciona(caracterAtual);
                    numero = "";
                    letra++;
                }
                else
                {
                    seqInfixa.adiciona(caracterAtual);
                }

                if (i + 1 == txtVisor.Text.Length && (numero != ""))
                {
                    numeros.adiciona(Convert.ToDouble(numero));
                    seqInfixa.adiciona(letra);
                    numero = "";
                    letra++;
                }
            }

            Converter();
            lbSequencias.Text += "  "; 
            while (!seqInfixa.estaVazia())
                lbSequencias.Text += seqInfixa.retiraPrimeiro() + "";

        }

        private void Converter()
        {
            seqPosfixa = new FilaVetor<char>(1000);
            char operadorComMaiorPrecedencia;
            PilhaVetor<char> pilha = new PilhaVetor<char>(1000);

            do
            {
                char simboloLido = seqInfixa.retiraPrimeiro();
                lbSequencias.Text += simboloLido + "";

                if (!(simboloLido <= 'Z' && simboloLido >= 'A'))
                {
                    
                        bool parar = false;
                        while (!parar && !pilha.estaVazia() && temPrecedencia(pilha.oTopo(), simboloLido))
                        {
                            operadorComMaiorPrecedencia = pilha.oTopo();
                            if (operadorComMaiorPrecedencia == '(')
                                parar = true;
                            else

                            {
                                seqPosfixa.adiciona(operadorComMaiorPrecedencia);
                                pilha.desempilhar();
                            }

                        }
                        if (simboloLido != ')')
                            pilha.empilhar(simboloLido);
                        else
                            operadorComMaiorPrecedencia = pilha.desempilhar();
                    
                }
                else
                {
                    seqPosfixa.adiciona(simboloLido);
                }
            }
            while (!seqInfixa.estaVazia());

            while (!pilha.estaVazia())
            {
                operadorComMaiorPrecedencia = pilha.desempilhar();
                if (operadorComMaiorPrecedencia != '(')
                    seqPosfixa.adiciona(operadorComMaiorPrecedencia);
            }

            lbSequencias.Text += "  ";
            txtResultado.Text = calcularPosfixa() + "";
        }

        private double calcularPosfixa()
        {
            double op1 = 0, op2 = 0, valor = 0;

            PilhaVetor<String> pilha = new PilhaVetor<String>(1000);

            while(!seqPosfixa.estaVazia())
            {
                char simboloAtual = seqPosfixa.retiraPrimeiro();
                lbSequencias.Text += simboloAtual + "";

                if (!Eoperador(simboloAtual + ""))
                    pilha.empilhar(numeros.retiraPrimeiro() + "");
                else
                {
                    op1 = Convert.ToDouble(pilha.desempilhar());
                    op2 = Convert.ToDouble(pilha.desempilhar());
                    valor = calcula(op1, op2, simboloAtual);
                    pilha.empilhar(valor + "");
                }
            }

            return Convert.ToDouble(pilha.desempilhar());
        }

        private double calcula(double op1, double op2, char simbolo)
        {
            switch (simbolo)
            {
                case '/':
                    if (op2 != 0)
                        return op1 / op2;
                    else
                        return 0;
                case '+':
                    return op2 + op1;
                case '-':
                    return op2 - op1;
                case '*':
                    return op2 * op1;
                case '^':
                    return Math.Pow(op2, op1);
                default:
                    return 0;
            }
            
            
        }

        private bool temPrecedencia(char topo, char simboloLido)
        {
            switch (topo)
            {
                case '(':
                    return true;

                case '^':
                    return true;

                case '*':
                    if (simboloLido == '+' || simboloLido == '-' || simboloLido == ')' || simboloLido == '(')
                        return true;
                    return false;

                case '/':
                    if (simboloLido == '+' || simboloLido == '-' || simboloLido == ')' || simboloLido == '(')
                        return true;
                    return false;

                case '+':
                    if (simboloLido == '^' || simboloLido == '*' || simboloLido == '/')
                        return true;
                    return false;

                case '-':
                    if (simboloLido == '^' || simboloLido == '*' || simboloLido == '/')
                        return true;
                    return false;

                case ')':
                    if (simboloLido == '(')
                        return true;
                    return false;
            }
            return false;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
