﻿using System;
using Tabuleiros;
using Pecas;
using Cores;

namespace Telas

{
    class Tela
    {
        Tabuleiro tab = new Tabuleiro(8,8);
        public static void ImprimirTabuleiro(Tabuleiro tab)
        { 
            
            for (int l = 0; l < tab.Linhas; l++) 
            {
                Console.Write(8 - l + " ");
                for (int c=0; c<tab.Colunas;c++)
                {
                    if (tab.Peca(l, c) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        Tela.ImprimirPeca(tab.Peca(l, c));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");

        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca.Cor == Cor.Branca)
            {
                Console.Write(peca);
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}
