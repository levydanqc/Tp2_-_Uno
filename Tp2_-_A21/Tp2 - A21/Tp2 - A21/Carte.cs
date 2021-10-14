using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    break;
                case 11:
                    return "V";
                    break;
                case 12:
                    return "D";
                    break;
                case 13:
                    return "R";
                    break;
                case 14:
                    return "A";
                    break;
                default:
                    return "";
                    break;
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
                    break;
                case Sorte.Coeur:
                    return "CO";
                    break;
                case Sorte.Pique:
                    return "PI";
                    break;
                case Sorte.Trèfle:
                    return "TR";
                    break;
                default:
                    return "";
                    break;
            }
        }

        #endregion
    }
}