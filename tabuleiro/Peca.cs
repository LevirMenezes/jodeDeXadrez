using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cores;
using Posicoes;
using Tabuleiros;
namespace Pecas
{
    class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int qteMoivimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab) 
        {
            Posicao = null;
            Cor = cor;
            qteMoivimentos = 0;
            Tab = tab;
        }

        public void IncrementarQteMovimentos()
        {
            qteMoivimentos++;
        }
       
    
    }
}
