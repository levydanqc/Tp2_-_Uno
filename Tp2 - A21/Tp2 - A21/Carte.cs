using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tp2___A21
{
    public class Carte
    {
        public enum Sorte
        {
            Coeur,
            Carreau,
            Trèfle,
            Pique
        }

        private int _valeur;
        private Sorte _sorte;
        private bool _jouerAnytime;

        public bool JouerAnytime
        {
            get { return _jouerAnytime; }
            set { _jouerAnytime = value; }
        }

        public int Valeur
        {
            get { return _valeur; }
            set { _valeur = value; }
        }

        public Sorte SorteCarte
        {
            get { return _sorte; }
            set { _sorte = value; }
        }

        public Carte(int pValeur, Sorte pSorte)
        {
            Valeur = pValeur;
            SorteCarte = pSorte;
            _jouerAnytime = pValeur == 8 ? true : false;
        }

        #region NomFichierPourAffichage

        /// <summary>
        /// Cette méthode permet d'obtenir le nom du fichier .png
        /// afin d'afficher la carte.
        /// </summary>
        /// <returns>Le nom du fichier .png</returns>
        public string ObtenirNomFichier()
        {
            return ObtenirChaineValeur() + ObtenirChaineSorte() + ".png";
        }

        /// <summary>
        /// Cette méthode permet d'obtenir la chaine contenant la
        /// valeur de la carte.
        /// </summary>
        /// <returns>Une string contenant la valeur</returns>
        private string ObtenirChaineValeur()
        {
            switch (Valeur)
            {
                case < 11:
                    return Valeur.ToString();
                case 11:
                    return "V";
                case 12:
                    return "D";
                case 13:
                    return "R";
                case 14:
                    return "A";
                default:
                    return "";
            }
        }


        /// <summary>
        /// Cette méthode permet d'obtenir la chaine contenant la
        /// sorte de la carte.
        /// </summary>
        /// <returns>Une string contenant la sorte</returns>
        private string ObtenirChaineSorte()
        {
            switch (SorteCarte)
            {
                case Sorte.Carreau:
                    return "CA";
                case Sorte.Coeur:
                    return "CO";
                case Sorte.Pique:
                    return "PI";
                case Sorte.Trèfle:
                    return "TR";
                default:
                    return "";
            }
        }

        #endregion

        public void ObtenirPouvoir( ref Queue<Joueur> pLesJoueurs, Stack<Carte> pCartes)
        {
            switch (Valeur)
            {
                case 2:
                    Trace.WriteLine("Pouvoir 2");
                    for (int i = 0; i < (SorteCarte == Sorte.Pique ? 3 : 1); i++)
                    {
                        pLesJoueurs.Peek().Main.Add(pCartes.Pop());
                    }
                    
                    break;
                case 10:
                    Trace.WriteLine("Pouvoir 10");
                    Stack<Joueur> stackTempo = new Stack<Joueur>();
                    while (pLesJoueurs.Count > 0)
                    {
                        stackTempo.Push(pLesJoueurs.Dequeue());
                    }

                    while (stackTempo.Count > 0)
                    {
                        pLesJoueurs.Enqueue(stackTempo.Pop());
                    }
                    break;
                case 11:
                    Trace.WriteLine("Pouvoir 11");
                    pLesJoueurs.Enqueue(pLesJoueurs.Dequeue());
                    break;
            }
        }
    }
}