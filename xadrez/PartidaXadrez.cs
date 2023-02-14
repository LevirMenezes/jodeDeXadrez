using System;
using tabuleiro;
using Cores;
using System.Collections.Generic;
using Posicoes;
using Pecas;
using TabuleiroExceptions;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }


        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            JogadorAtual = Cor.Branca;
            vulneravelEnPassant = null;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha+1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimento();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimento();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimento();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }

        }


        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in capturadas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in pecas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
                aux.ExceptWith(pecasCapturadas(cor));

            }
            return aux;
        }


        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach(Peca p in pecasEmJogo(cor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }
            return null;
        }


        public bool estaEmXeque(Cor cor)
            
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor "+ cor + " no tabuleiro!");
            }
            foreach (Peca p in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca p in pecasEmJogo(cor))
            {
                bool[,] mat = p.MovimentosPossiveis();
                for (int l=0; l<tab.linhas; l++)
                {
                    for (int c=0; c<tab.colunas; c++)
                    {
                        if (mat[l, c])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(l, c);
                            Peca pecaCapturada = ExecutaMovimento(origem, new Posicao(l, c));
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }
        private void ColocarPecas()
        {

            colocarNovaPeca('a', 1, new Torre(Cor.Branca,tab));
            colocarNovaPeca('b', 1, new Cavalo(Cor.Branca,tab));
            colocarNovaPeca('c', 1, new Bispo(Cor.Branca,tab));
            colocarNovaPeca('d', 1, new Dama(Cor.Branca,tab)); 
            colocarNovaPeca('e', 1, new Rei(Cor.Branca,tab, this));
            colocarNovaPeca('f', 1, new Bispo(Cor.Branca,tab));
            colocarNovaPeca('g', 1, new Cavalo(Cor.Branca,tab));
            colocarNovaPeca('h', 1, new Torre(Cor.Branca,tab));
            colocarNovaPeca('a', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('b', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('c', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('d', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('e', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('f', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('g', 2, new Peao(Cor.Branca,tab, this));
            colocarNovaPeca('h', 2, new Peao(Cor.Branca,tab, this));

            colocarNovaPeca('a', 8,new Torre(Cor.Preta, tab));
            colocarNovaPeca('b',8,new Cavalo(Cor.Preta, tab));
            colocarNovaPeca('c', 8,new Bispo(Cor.Preta, tab));
            colocarNovaPeca('d', 8, new Dama(Cor.Preta, tab));
            colocarNovaPeca('e', 8,  new Rei(Cor.Preta, tab, this));
            colocarNovaPeca('f', 8,new Bispo(Cor.Preta, tab));
            colocarNovaPeca('g',8,new Cavalo(Cor.Preta, tab));
            colocarNovaPeca('h', 8,new Torre(Cor.Preta, tab));
            colocarNovaPeca('a', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('b', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('c', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('d', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('e', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('f', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('g', 7, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('h', 7, new Peao(Cor.Preta, tab, this));
        }
        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            } 


            
            
            if (JogadorAtual != tab.peca(pos).Cor)
            {
                throw new TabuleiroException("Apeça de origem escolhida não é sua!");
            } 
            
            
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            } 
        }


        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            Peca p = tab.peca(destino);
            // #jogadaespecial promocao
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(p.Cor, tab);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }



            if (estaEmXeque(adversaria(JogadorAtual)))
            {
                xeque = true;

            }
            else
            {
                xeque = false;
            }

            if (testeXequemate(adversaria(JogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();

            }

            // #jogadaespecial em passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }

        private void mudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }

        }
    }
}
