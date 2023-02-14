using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cores;
using Posicoes;
using tabuleiro;
namespace Pecas
{
    abstract class Peca
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

        public void decrementarQteMovimento()
        {
            qteMoivimentos--;
        }
        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for (int i=0; i<Tab.linhas; i++)
            {
                for (int j=0; j < Tab.colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool movimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }
        public abstract bool[,] MovimentosPossiveis();
       
    
    }
}
