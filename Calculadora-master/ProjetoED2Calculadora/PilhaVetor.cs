using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoED2Calculadora
{
    class PilhaVetor<dado>
    {
        private int tamanho;
        private dado[] vetor;
                           
        private int topo = -1;

        public PilhaVetor(int posic)
        {
            tamanho = posic; 
            vetor = new dado[tamanho];   
        } 

        public int Tamanho()
        { 
            return (topo + 1);
        }
        public bool estaVazia()
        { 
            return (topo < 0);
        }
        public void empilhar(dado obj)
        {
            if (Tamanho() == tamanho)
                throw new Exception();
            vetor[++topo] = obj; 
        }
        public dado oTopo()
        {
            dado obj;
            if (estaVazia())
                throw new Exception();
            obj = vetor[topo];
                return obj;
        }
        public dado desempilhar()
        {
            dado obj;
            if (estaVazia())
                throw new Exception();
            obj = vetor[topo]; 
            vetor[topo] = default(dado);
            topo--;
            return obj; 
        }
         
    }
}
