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


    }
}