using Cores;
using Posicoes;
using System;
using TabuleiroExceptions;
using tabuleiro;
using Telas;
using xadrez;
namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tab);
                    Console.WriteLine();
                    Console.Write("Turno: " + partida.turno);
                    Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();

                    bool[,] logico = partida.tab.peca(origem).MovimentosPossiveis();

                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tab, logico);
                    
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    partida.ExecutaMovimento(origem, destino);
                }
            

            }
            catch (TabuleiroException e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
