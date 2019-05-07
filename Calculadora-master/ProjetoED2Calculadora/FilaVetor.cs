using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoED2Calculadora
{
    class FilaVetor<dado>
    {
        private int tamanho;
        private dado[] vetor;

        public FilaVetor(int t)
        {
            vetor = new dado[t];
            tamanho = 0;
        }

        public dado primeiro()
        {
            if (!estaVazia())
                return vetor[0];
            else
                return default(dado);
        }

        public dado retiraPrimeiro()
        {
            if (!estaVazia())
            {
                dado valorARetornar = vetor[0];
                retira();
                return valorARetornar;
            }
            else
                return default(dado);
        }

        public void adiciona(dado d)
        {
            vetor[tamanho] = d;
            tamanho++;
        }

        public int Tamanho()
        {
            return tamanho;
        }        

        private void retira()
        {
            vetor[0] = default(dado);
            for (int i = 0; i < Tamanho() - 1; i++)
                vetor[i] = vetor[i + 1];
            vetor[vetor.Length - 1] = default(dado);
            tamanho--;
        }

        public bool estaVazia()
        {
            if (vetor[0].Equals(default(dado)))
                return true;
            return false;
        }
    }
}
